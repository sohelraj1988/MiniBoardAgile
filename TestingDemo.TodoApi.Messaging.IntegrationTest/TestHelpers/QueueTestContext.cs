using Microsoft.Azure.ServiceBus.Management;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestingDemo.TodoApi.Messaging.IntegrationTest.TestHelpers
{
    internal class QueueTestContext
    {
        private ManagementClient _managmentclient;
        private string _completedTodosQueueName;

        internal async Task SetUpAsync()
        {
            _managmentclient = new ManagementClient(TestContext.Parameters["ServiceBusConnectionString"]);
            _completedTodosQueueName = Guid.NewGuid().ToString();

            await _managmentclient.CreateQueueAsync(new QueueDescription(_completedTodosQueueName));
        }

        internal async Task TearDownAsync()
        {
            await _managmentclient.DeleteQueueAsync(_completedTodosQueueName);
        }

        internal QueueMessageSenderConfiguration GetConfiguration()
        {
            return new QueueMessageSenderConfiguration
            {
                NamespaceConnectionString = TestContext.Parameters["ServiceBusConnectionString"],
                CompletedTodosQueueName = _completedTodosQueueName
            };
        }
    }
}
