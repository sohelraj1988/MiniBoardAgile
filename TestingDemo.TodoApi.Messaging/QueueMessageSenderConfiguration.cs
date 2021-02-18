namespace TestingDemo.TodoApi.Messaging
{
    public class QueueMessageSenderConfiguration
    {
        public const string SectionName = "ServiceBus";

        public string NamespaceConnectionString { get; set; }
        public string CompletedTodosQueueName { get; set; }
    }
}