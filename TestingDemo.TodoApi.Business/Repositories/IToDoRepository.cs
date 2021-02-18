using System;
using System.Threading.Tasks;
using TestingDemo.TodoApi.Business.Services;

namespace TestingDemo.TodoApi.Business.Repositories
{
    public interface IToDoRepository
    {
        ToDo GetById(Guid id);
        ToDo[] GetAllAsync();
        Task AddAsync(ToDo todo);
        Task UpdateAsync(ToDo todo);
        Task DeleteAsync(ToDo todo);
    }
}
