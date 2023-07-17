using codeTestCom.Models;
using Microsoft.Azure.Cosmos;

namespace codeTestCom.Repository
{
    public class CosmosDBCarRepository : ICarRepository
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

        public CosmosDBCarRepository(IConfiguration configuration)
        {
            _endpointUri = configuration["appSettings:EndpointUri"];
            _primaryKey = configuration["appSettings:PrimaryKey"];
            _cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "CodeTestCom" });
            _database = (Database)_cosmosClient.GetDatabase(Utils.DATABASE_ID);
            _container = _database.GetContainer(Utils.CONTAINER_CAR_ID);
        }

        public async Task<Car> GetCarAsyncById(string id)
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.id = '" + id + "'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Car> queryResultSetIterator = _container.GetItemQueryIterator<Car>(queryDefinition);

            Car car = new Car();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Car> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Car item in currentResultSet)
                {
                    car = item;
                    break;
                }
            }

            return car;
        }

        public async Task<Car> UpdateCarAsync(Car car, bool rented)
        {
            ItemResponse<Car> fieldResponse = await _container.ReadItemAsync<Car>(car.Id, new PartitionKey(car.PartitionKey));

            var item = fieldResponse.Resource;

            // update rented status from false to true
            item.IsRented = rented;

            // replace the item with the updated content
            fieldResponse = await _container.ReplaceItemAsync<Car>(item, item.Id, new PartitionKey(item.PartitionKey));
            Console.WriteLine("Updated Car [{0},{1}].\n \tBody is now: {2}\n", item.Name, item.Id, fieldResponse.Resource);

            return item;
        }
        public async Task<Car> AddCarAsync(Car car)
        {
            Car userNew = new Car(car.Id, car.Name, car.Brand, car.Type);
            userNew.PartitionKey = car.Brand.ToString();
            ItemResponse<Car> response = await _container.CreateItemAsync(userNew, new PartitionKey(userNew.PartitionKey));
            return response.Resource;
        }
    }
}
