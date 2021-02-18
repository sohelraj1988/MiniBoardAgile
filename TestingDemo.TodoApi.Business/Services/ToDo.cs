using System;
using System.Text;
using Newtonsoft.Json;

namespace TestingDemo.TodoApi.Business.Services
{
    public class ToDo
    {
        public const string NameNotNullEmptyMessage = "Name cannot be null or empty";
        public const string DescriptionNotNullEmptyMessage = "Description cannot be null or empty";
        public const string PriceNotZeroOrNegativeMessage = "Price cannot be negative or zero";
        public const string DueAtNotBeforeNow = "DueAt must be set to a valid date in the future";
        public const string BillToEmailNotNullEmptyMessage = "BillToEmail cannot be null or empty";

        [JsonProperty]
        public Guid id { get; set; }
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; private set; }
        [JsonProperty]
        public decimal Price { get; private set; }
        [JsonProperty]
        public DateTime CreatedAt { get; private set; }
        [JsonProperty]
        public DateTime DueAt { get; private set; }
        [JsonProperty]
        public DateTime? CompletedAt { get; private set; }
        [JsonProperty]
        public string BillToEmail { get; private set; }

        public static ToDo Create(string name, string description, decimal price, DateTime dueAt, string billToEmail)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidTodoException(NameNotNullEmptyMessage);
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new InvalidTodoException(DescriptionNotNullEmptyMessage);
            }

            if (price <= 0)
            {
                throw new InvalidTodoException(PriceNotZeroOrNegativeMessage);
            }

            if (dueAt <= DateTime.UtcNow)
            {
                throw new InvalidTodoException(DueAtNotBeforeNow);
            }

            if (string.IsNullOrEmpty(billToEmail))
            {
                throw new InvalidTodoException(BillToEmailNotNullEmptyMessage);
            }

            return new ToDo()
            {
                id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                CreatedAt = DateTime.UtcNow,
                DueAt = dueAt,
                BillToEmail = billToEmail
            };
        }

        public void SetCompletedAt(DateTime completedAt)
        {
            if (CompletedAt.HasValue)
            {
                throw new TodoAlreadyCompletedException($"Cannot complete ToDo with id '{id}' as it has already been completed before");
            }

            CompletedAt = completedAt;
        }
    }
}
