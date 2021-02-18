using System.Threading.Tasks;
using TestingDemo.TodoApi.Business.Services;

namespace TestingDemo.TodoApi.Functions.Messaging
{
    public interface IQueueMessageSender
    {
        Task SendCompletedTodoAsync(ToDo todo);
    }
}
