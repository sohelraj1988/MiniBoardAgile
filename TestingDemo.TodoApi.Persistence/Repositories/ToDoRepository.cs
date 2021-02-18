using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestingDemo.TodoApi.Business.Repositories;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Persistence.Configuration;

namespace TestingDemo.TodoApi.Persistence.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly IOptions<CosmosDbConfiguration> _cosmosDbConfiguration;
        private readonly Lazy<CosmosClient> _lazyCosmosClient;

        public ToDoRepository(IOptions<CosmosDbConfiguration> cosmosDbConfiguration)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration;
            _lazyCosmosClient = new Lazy<CosmosClient>(() => new CosmosClient(
                cosmosDbConfiguration.Value.DbEndpoint, cosmosDbConfiguration.Value.DbKey));
        }

        public ToDo GetById(Guid id)
        {
            var collection = GetCollection();

            var resource = collection
                .GetItemLinqQueryable<ToDo>(true)
                .Where(x => x.id == id)
                .ToArray()
                .Single();

            return resource;
        }

        public ToDo[] GetAllAsync()
        {
            var collection = GetCollection();

            var resources = collection
                .GetItemLinqQueryable<ToDo>(true)
                .ToArray();

            return resources;

        }

        public async Task AddAsync(ToDo todo)
        {
            var collection = GetCollection();

            await collection.UpsertItemAsync(todo);
        }

        public async Task UpdateAsync(ToDo todo)
        {
            var collection = GetCollection();

            await collection.UpsertItemAsync(todo);
        }

        public async Task DeleteAsync(ToDo todo)
        {
            var collection = GetCollection();

            await collection.DeleteItemAsync<ToDo>(todo.id.ToString(), new PartitionKey(todo.id.ToString()));
        }

        private Container GetCollection()
        {
            var cosmosDbClient = _lazyCosmosClient.Value;
            var database = cosmosDbClient.GetDatabase(_cosmosDbConfiguration.Value.DatabaseName);

            var collection = database.GetContainer(_cosmosDbConfiguration.Value.CollectionName);
            return collection;
        }

    }
}
