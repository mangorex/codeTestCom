using codeTestCom.Models;
using Microsoft.Azure.Cosmos;
using User = codeTestCom.Models.User;

namespace codeTestCom.Repository
{
    public class CosmosDBUserRepository : IUserRepository
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

        public CosmosDBUserRepository(IConfiguration configuration)
        {
            _endpointUri = configuration["appSettings:EndpointUri"];
            _primaryKey = configuration["appSettings:PrimaryKey"];
            _cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "CodeTestCom" });
            _database = (Database)_cosmosClient.GetDatabase(Utils.DATABASE_ID);
            _container = _database.GetContainer(Utils.CONTAINER_USER_ID);
        }

        public async Task<User> GetUserAsyncByDni(string dni)
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.Dni = '" + dni + "'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<User> queryResultSetIterator = _container.GetItemQueryIterator<User>(queryDefinition);

            User user = new User();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<User> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (User item in currentResultSet)
                {
                    user = item;
                    break;
                }
            }

            return user;
        }

        public async Task<User> UpdateUserLoyaltyAsync(User user, int loyaltyPoints)
        {
            ItemResponse<User> fieldResponse = await _container.ReadItemAsync<User>(user.Id, new PartitionKey(user.PartitionKey));

            var item = fieldResponse.Resource;

            // update rented status from false to true
            item.LoyaltyPoints = loyaltyPoints;

            // replace the item with the updated content
            fieldResponse = await _container.ReplaceItemAsync<User>(item, item.Id, new PartitionKey(item.PartitionKey));
            Console.WriteLine("Updated User [{0},{1}].\n \tBody is now: {2}\n", item.Id, item.LoyaltyPoints, fieldResponse.Resource);

            return item;
        }
    }
}
