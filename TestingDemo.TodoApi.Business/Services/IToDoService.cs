using System;
using System.Threading.Tasks;
using TestingDemo.TodoApi.Business.Services.Dtos;

namespace TestingDemo.TodoApi.Business.Services
{
    public interface IToDoService
    {
        ToDo[] GetAll();
        Task<ToDo> CreateAsync(CreateToDoDto createToDoDto);
        Task<ToDo> UpdateTodo(Guid id, UpdateToDoDto updateToDoDto);
        Task DeleteAsync(Guid id);
    }
}
