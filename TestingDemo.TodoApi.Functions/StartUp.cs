using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestingDemo.TodoApi.Functions;
using TestingDemo.TodoApi.Functions.Messaging;
using TestingDemo.TodoApi.Messaging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace TestingDemo.TodoApi.Functions
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddScoped<IQueueMessageSender, QueueMessageSender>();
            services.Configure<QueueMessageSenderConfiguration>(
                configuration.GetSection(QueueMessageSenderConfiguration.SectionName));
        }
    }
}