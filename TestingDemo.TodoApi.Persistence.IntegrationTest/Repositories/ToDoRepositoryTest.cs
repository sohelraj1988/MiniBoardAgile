using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Persistence.IntegrationTest.TestHelpers;
using TestingDemo.TodoApi.Persistence.Repositories;

namespace TestingDemo.TodoApi.Persistence.IntegrationTest.Repositories
{
    [TestFixture]
    public class ToDoRepositoryTest
    {
        private CosmosTestContext _cosmosTestContext;
        private ToDoRepository _subject;

        [SetUp]
        public async Task SetUp()
        {
            _cosmosTestContext = new CosmosTestContext();

            await _cosmosTestContext.SetUpAsync();
            _subject = new ToDoRepository(_cosmosTestContext.GetConfiguration());
        }

        [TearDown]
        public async Task TearDown()
        {
            await _cosmosTestContext.TearDownAsync();
        }

        [Test]
        public async Task WhenToDoIsAdded_ItCanBeRetrievedAgainById()
        {
            var todo = ToDo.Create("myTodo", "blalbla", 123, DateTime.UtcNow.AddDays(1), "demo@demo.com");

            await _subject.AddAsync(todo);

            var actual = _subject.GetById(todo.id);

            Assert.AreEqual(todo.Name, actual.Name);
        }
    }
}
