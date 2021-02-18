using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestingDemo.TodoApi.Business.Repositories;
using TestingDemo.TodoApi.Business.Services.Dtos;

namespace TestingDemo.TodoApi.Business.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ILogger<ToDoService> _logger;
        private readonly IToDoRepository _toDoRepository;

        public ToDoService(ILogger<ToDoService> logger, IToDoRepository toDoRepository)
        {
            _logger = logger;
            _toDoRepository = toDoRepository;
        }

        public ToDo[] GetAll()
        {
            var todos = _toDoRepository.GetAllAsync();
            return todos;
        }

        public async Task<ToDo> CreateAsync(CreateToDoDto createToDoDto)
        {
            var todo = ToDo.Create(
                createToDoDto.Name,
                createToDoDto.Description,
                createToDoDto.Price,
                createToDoDto.DueAt,
                createToDoDto.BillToEmail);

            _logger.LogTrace("Created new todo with id {id}", todo.id);

            await _toDoRepository.AddAsync(todo);

            _logger.LogTrace("Stored new todo with id {id}", todo.id);

            return todo;
        }

        public async Task<ToDo> UpdateTodo(Guid id, UpdateToDoDto updateToDoDto)
        {
            var todo = _toDoRepository.GetById(id);

            _logger.LogTrace("Updating todo with id {id}, setting completed at {completedAt}", todo.id, updateToDoDto.CompletedAt);

            todo.SetCompletedAt(updateToDoDto.CompletedAt);

            await _toDoRepository.UpdateAsync(todo);

            return todo;
        }

        public async Task DeleteAsync(Guid id)
        {
            var todo = _toDoRepository.GetById(id);

            _logger.LogTrace("Deleting todo with id {id}", todo.id);

            await _toDoRepository.DeleteAsync(todo);
        }
    }
}
