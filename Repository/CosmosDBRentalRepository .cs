using codeTestCom.Models;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace codeTestCom.Repository
{
    public class CosmosDBRentalRepository : IRentalRepository
    {
        // The Cosmos client instance
        private CosmosClient _cosmosClient;

        private Database _database;
        // The container we will create.
        private Container _container;

        // The Azure Cosmos DB endpoint for running this sample.
        private readonly string _endpointUri;
        // The primary key for the Azure Cosmos account.
        private readonly string _primaryKey;

        public CosmosDBRentalRepository(IConfiguration configuration)
        {
            _endpointUri = configuration["appSettings:EndpointUri"];
            _primaryKey = configuration["appSettings:PrimaryKey"];
            _cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "CodeTestCom" });
            _database = (Database)_cosmosClient.GetDatabase(Utils.DATABASE_ID);
            _container = _database.GetContainer(Utils.CONTAINER_RENTAL_ID);
        }
        public async Task<Rental> CreateRentalAsync(Rental rental)
        {
            try
            {
                // Read the item to see if it exists.
                ItemResponse<Rental> itemResponse = await _container.ReadItemAsync<Rental>(rental.Id, new PartitionKey(rental.PartitionKey));
                Console.WriteLine("Item in database with id: {0} already exists\n", rental.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container.
                ItemResponse<Rental> itemResponse = await _container.CreateItemAsync<Rental>(rental, new PartitionKey(rental.PartitionKey));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", rental.Id, itemResponse.RequestCharge);
            }

            rental.CalculatePrice();
            return rental;
            
        }
    }
}
