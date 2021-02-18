using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using TestingDemo.TodoApi.Persistence.Configuration;

namespace TestingDemo.TodoApi.Persistence.IntegrationTest.TestHelpers
{
    internal class CosmosTestContext
    {
        private readonly Database _database;
        private readonly string _containerName;

        public CosmosTestContext()
        {
            var comsosClient = new CosmosClient(
                TestContext.Parameters["CosmosDbEndpoint"],
                TestContext.Parameters["CosmosDbKey"]);

            _database = comsosClient.GetDatabase(TestContext.Parameters["CosmosDbDatabaseName"]);
            _containerName = Guid.NewGuid().ToString();

        }

        public IOptions<CosmosDbConfiguration> GetConfiguration()
        {
            return Options.Create(new CosmosDbConfiguration
            {
                DbEndpoint = TestContext.Parameters["CosmosDbEndpoint"],
                DbKey = TestContext.Parameters["CosmosDbKey"],
                DatabaseName = TestContext.Parameters["CosmosDbDatabaseName"],
                CollectionName = _containerName
            });
        }

        public async Task SetUpAsync()
        {
            await _database.CreateContainerAsync(_containerName, "/id");
        }

        public async Task TearDownAsync()
        {
            await _database.GetContainer(_containerName).DeleteContainerAsync();
        }
    }
}