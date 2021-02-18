using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestingDemo.TodoApi.Business.Repositories;
using TestingDemo.TodoApi.Business.Services;
using TestingDemo.TodoApi.Business.Services.Dtos;

namespace TestingDemo.TodoApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDosController : ControllerBase
    {
        private readonly ILogger<ToDosController> _logger;
        private readonly IToDoService _toDoService;

        public ToDosController(ILogger<ToDosController> logger, IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateToDoDto createToDoDto)
        {
            _logger.LogTrace("Adding todo");

            var todo = await _toDoService.CreateAsync(createToDoDto);

            return Ok(todo);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogTrace("Deleting todo");

            await _toDoService.DeleteAsync(id);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogTrace("Retrieving all todos");

            var todos = _toDoService.GetAll();
            return Ok(todos);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] UpdateToDoDto updateToDoDto)
        {
            _logger.LogTrace("Updating todo with id '{id}'", id);

            try
            {
                var todo = await _toDoService.UpdateTodo(id, updateToDoDto);
                return Ok(todo);

            }
            catch (ModelNotFoundException)
            {
                _logger.LogTrace("Aborting update of todo with id '{id}' as it could not be found", id);
                return NotFound();
            }
        }
    }
}
