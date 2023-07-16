using codeTestCom.Models;
using codeTestCom.Repository;
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

        private readonly ICarRepository _carRepository;
        private readonly IRentalRepository _rentalRepository;

        public RentalController(IConfiguration configuration, ICarRepository carRepository, IRentalRepository rentalRepository)
        {
            _configuration = configuration;
            _carRepository = carRepository;
            _rentalRepository = rentalRepository;
            _endpointUri = _configuration["appSettings:EndpointUri"];
            _primaryKey = _configuration["appSettings:PrimaryKey"];
            _rentalRepository = rentalRepository;
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
            Car car = await _carRepository.GetCarAsync(rental.CarId);
            
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            if (car.IsRented)
            {
                return Conflict("The car has already been rented.");
            }

            car.IsRented = true;

            car = await _carRepository.UpdateCarAsync(car, true);

            if(car.IsRented)
            {
                Rental rentalDB = new Rental(car, 10);

                rental = await _rentalRepository.CreateRentalAsync(rentalDB);
            }
            return rental;
        }


        // <PopulateItem>
        /// <summary>
        /// Add one item to the container
        /// </summary>
        private async Task CreateRentalAsync(Rental rental)
        {
            try
            {
                // Read the item to see if it exists.
                ItemResponse<Rental> itemResponse = await this.container.ReadItemAsync<Rental>(rental.Id, new PartitionKey(rental.PartitionKey));
                Console.WriteLine("Item in database with id: {0} already exists\n", rental.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container.
                ItemResponse<Rental> itemResponse = await this.container.CreateItemAsync<Rental>(rental, new PartitionKey(rental.PartitionKey));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", rental.Id, itemResponse.RequestCharge);
            }
        }
        // </PopulateItem>

    }
}