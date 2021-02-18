using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Functions.Messaging;

namespace TestingDemo.TodoApi.Functions.Functions
{
    public class TodoChangeTrigger
    {
        private readonly ILogger<TodoChangeTrigger> _logger;
        private readonly IQueueMessageSender _queueMessageSender;

        public TodoChangeTrigger(ILogger<TodoChangeTrigger> logger, IQueueMessageSender queueMessageSender)
        {
            _logger = logger;
            _queueMessageSender = queueMessageSender;
        }

        [FunctionName("TodoChangeTrigger")]
        public async Task Run(
            [CosmosDBTrigger("database", "todos",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true,
            ConnectionStringSetting = "CosmosDb:ConnectionString")] IReadOnlyList<Document> input)
        {
            if (input == null || input.Count <= 0)
            {
                return;
            }

            foreach (var document in input)
            {
                var todo = Newtonsoft.Json.JsonConvert.DeserializeObject<ToDo>(document.ToString());

                if (todo.CompletedAt.HasValue)
                {
                    await _queueMessageSender.SendCompletedTodoAsync(todo);
                }
            }
        }
    }
}
