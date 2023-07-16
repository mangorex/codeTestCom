using codeTestCom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.Net;

namespace codeTestCom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "RentalDB";
        private string containerId = "Items";

        private readonly IConfiguration _configuration;
        // The Azure Cosmos DB endpoint for running this sample.
        private readonly string _endpointUri;
        // The primary key for the Azure Cosmos account.
        private readonly string _primaryKey;

        public RentalController(IConfiguration configuration)
        {
            _configuration = configuration;
            _endpointUri = _configuration["appSettings:EndpointUri"];
            _primaryKey = _configuration["appSettings:PrimaryKey"];
        }

        [HttpPost("CalculatePrice")]
        public ActionResult<Price> CalculatePrice(Rental rental)
        {
            return rental.CalculatePrice();
        }

        [HttpPost("CalculatePriceAndSurcharges")]
        public ActionResult<Price> CalculatePriceAndSurcharges(Rental rental)
        {
            return rental.CalculatePriceAndSurcharges(rental.NumOfDaysUsed);
        }

        [HttpPost("RentCar")]
        public async Task<ActionResult<Rental>> RentCar(Rental rental)
        {
            // Create a new instance of the Cosmos Client
            using (this.cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "CodeTestComPopulate" }))
            {
                Car car = await GetCarAsync(rental.CarId);
                if (!car.IsRented)
                {
                    rental = await UpdateCarRented(car, rental, true);
                }
                else
                {
                    return Conflict(Utils.ERROR_CAR_RENTED);
                }
                return rental;
            }
        }

        private async Task<Car> GetCarAsync(string carId)
        {
            Database database = (Database)this.cosmosClient.GetDatabase(databaseId);
            this.container = database.GetContainer(containerId);
            var sqlQueryText = "SELECT * FROM c WHERE c.id = '" + carId + "'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Car> queryResultSetIterator = this.container.GetItemQueryIterator<Car>(queryDefinition);

            Car car = new Car();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Car> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Car item in currentResultSet)
                {
                    car = item;
                }
            }

            return car;
        }

        private async Task <Rental>UpdateCarRented(Car car, Rental rental, bool rented)
        {
            ItemResponse<Car> bmwfieldCarResponse = await this.container.ReadItemAsync<Car>(car.Id, new PartitionKey(car.PartitionKey));

            var itemBody = bmwfieldCarResponse.Resource;

            // update rented status from false to true
            itemBody.IsRented = rented;

            // replace the item with the updated content
            bmwfieldCarResponse = await this.container.ReplaceItemAsync<Car>(itemBody, itemBody.Id, new PartitionKey(itemBody.PartitionKey));
            Console.WriteLine("Updated Car [{0},{1}].\n \tBody is now: {2}\n", itemBody.Name, itemBody.Id, bmwfieldCarResponse.Resource);

            if (itemBody.IsRented)
            {
                Rental rentalDB = new Rental(car, rental.NumOfContractedDays);
                rentalDB.CalculatePrice();
                await PopulateItem(rentalDB, rentalDB.Id, rentalDB.PartitionKey);
                return rentalDB;
            }

            return null;
        }

        // <PopulateItem>
        /// <summary>
        /// Add one item to the container
        /// </summary>
        private async Task PopulateItem<T>(T item, string id, string partitionKey)
        {
            try
            {
                // Read the item to see if it exists.
                ItemResponse<T> itemResponse = await this.container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                Console.WriteLine("Item in database with id: {0} already exists\n", id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container.
                ItemResponse<T> itemResponse = await this.container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", id, itemResponse.RequestCharge);
            }
        }
        // </PopulateItem>

    }
}