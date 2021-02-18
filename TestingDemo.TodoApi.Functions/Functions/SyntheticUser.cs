using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Business.Services.Dtos;

namespace TestingDemo.TodoApi.Functions.Functions
{
    public static class SyntheticUser
    {
        private static HttpClient _httpClient = new HttpClient();

        [FunctionName("SyntheticUser")]
        public static async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(new CreateToDoDto
            {
                Name = $"syntheticuser-{Guid.NewGuid()}",
                Description = "syntheticuser",
                DueAt = DateTime.UtcNow.AddDays(1),
                Price = 500,
                BillToEmail = "system@test.com"
            }), Encoding.UTF8, "application/json");

            var postResponse = await _httpClient.PostAsync("https://testingdemo-api-test.azurewebsites.net/todos", postContent);
            postResponse.EnsureSuccessStatusCode();
            var todo = JsonConvert.DeserializeObject<ToDo>(await postResponse.Content.ReadAsStringAsync());

            var patchContent = new StringContent(JsonConvert.SerializeObject(new UpdateToDoDto
            {
                CompletedAt = DateTime.UtcNow,
            }), Encoding.UTF8, "application/json");
            var patchResponse = await _httpClient.PatchAsync($"https://testingdemo-api-test.azurewebsites.net/todos/{todo.id}", patchContent);
            patchResponse.EnsureSuccessStatusCode();

            var found = false;
            do
            {
                await Task.Delay(10000);

                var getResponse = await _httpClient.GetAsync("https://testextdemo.azurewebsites.net/api/GetBills");
                getResponse.EnsureSuccessStatusCode();
                var bills = JsonConvert.DeserializeObject<Bill[]>(await getResponse.Content.ReadAsStringAsync());

                found = bills.Any(x => x.TodoId == todo.id);
            } while (! found);
        }

        private class Bill
        {
            public int Id { get; set; }
            public Guid TodoId { get; set; }
            public string Email { get; set; }
            public decimal Price { get; set; }
        }
    }

}
