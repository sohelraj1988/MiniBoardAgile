using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using NUnit.Framework;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Business.Services.Dtos;

namespace TestingDemo.TodoApi.SystemTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task WhenATodoIsSubmitted_ThenItCanBeFoundInDatabase()
        {
            var client = new HttpClient();
            var createTodo = new CreateToDoDto
            {
                Name = $"systemtest-{Guid.NewGuid()}",
                Description = "systemtest",
                DueAt = DateTime.UtcNow.AddDays(1),
                Price = 500,
                BillToEmail = "system@test.com"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createTodo), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://testingdemo-api-test.azurewebsites.net/todos", content);

            response.EnsureSuccessStatusCode();

            var todo = JsonConvert.DeserializeObject<ToDo>(await response.Content.ReadAsStringAsync());

            var comsosClient = new CosmosClient(
                TestContext.Parameters["CosmosDbEndpoint"],
                TestContext.Parameters["CosmosDbKey"]);

            var database = comsosClient.GetDatabase(TestContext.Parameters["CosmosDbDatabaseName"]);
            var container = database.GetContainer(TestContext.Parameters["CosmosDbCollectionName"]);

            var actual = container
                .GetItemLinqQueryable<ToDo>(true)
                .Where(x => x.id == todo.id)
                .ToArray()
                .SingleOrDefault();
            
            Assert.IsNotNull(actual);
        }
    }
}