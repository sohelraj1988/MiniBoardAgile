namespace TestingDemo.TodoApi.Persistence.Configuration
{
    public class CosmosDbConfiguration
    {
        public const string SectionName = "CosmosDb";

        public string DbEndpoint { get; set; }
        public string DbKey { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}