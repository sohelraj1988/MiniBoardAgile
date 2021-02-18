using System;

namespace TestingDemo.TodoApi.Business.Services.Dtos
{
    public class CreateToDoDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DueAt { get; set; }
        public string BillToEmail { get; set; }
    }
}