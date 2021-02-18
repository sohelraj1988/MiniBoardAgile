using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Messaging.IntegrationTest.TestHelpers;

namespace TestingDemo.TodoApi.Messaging.IntegrationTest
{
    public class QueueMessageSenderTest
    {
        private QueueTestContext _queueTestContext;
        private QueueMessageSender _subject;

        [SetUp]
        public async Task SetUp()
        {
            _queueTestContext = new QueueTestContext();

            await _queueTestContext.SetUpAsync();

            _subject = new QueueMessageSender(Options.Create(_queueTestContext.GetConfiguration()));
        }

        [TearDown]
        public async Task TearDown()
        {
            await _queueTestContext.TearDownAsync();
        }

        [Test]
        public async Task WhenSendingAMessage_ThenItCanBeRetrievedAgain()
        {
            var todo = ToDo.Create("le nom", "hoi", 34, DateTime.UtcNow.AddDays(1), "demo@demo.com");
            var received = false;

            var configuration = _queueTestContext.GetConfiguration();
            var receiverClient = new QueueClient(configuration.NamespaceConnectionString, configuration.CompletedTodosQueueName);
            receiverClient.RegisterMessageHandler((message, cancelationToken) => { 
                var bytes = message.Body;
                var json = Encoding.UTF8.GetString(bytes);
                var actual = JsonConvert.DeserializeObject<ToDo>(json);
                if (actual.id == todo.id)
                {
                    received = true;
                }

                return Task.CompletedTask;
            }, new MessageHandlerOptions((_) => { return Task.CompletedTask; }){ MaxConcurrentCalls = 1, AutoComplete = true });

            await _subject.SendCompletedTodoAsync(todo);

            do
            {
                await Task.Delay(1000);
            } while (!received);
        }
    }
}
