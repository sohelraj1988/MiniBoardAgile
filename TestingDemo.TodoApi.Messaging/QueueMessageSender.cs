using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Functions.Messaging;

namespace TestingDemo.TodoApi.Messaging
{
    public class QueueMessageSender : IQueueMessageSender
    {
        private readonly QueueClient _completedTodosQueueClient;

        public QueueMessageSender(IOptions<QueueMessageSenderConfiguration> queueMessageSenderConfiguration)
        {
            _completedTodosQueueClient = new QueueClient(
                queueMessageSenderConfiguration.Value.NamespaceConnectionString,
                queueMessageSenderConfiguration.Value.CompletedTodosQueueName);
        }

        public async Task SendCompletedTodoAsync(ToDo todo)
        {
            var json = JsonConvert.SerializeObject(todo);
            var bytes = Encoding.UTF8.GetBytes(json);
            var message = new Message(bytes);
            await _completedTodosQueueClient.SendAsync(message);
        }
    }
}
