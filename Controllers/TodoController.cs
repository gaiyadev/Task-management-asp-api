using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories.Todo;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository  _todoRepository;

    public TodoController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var todos = await _todoRepository.GetTodos();
    
        var response = new
        {
            message = "Fetch successfully",
            statusCode = 200,
            status = "Success",
            data = todos
        };

        return Ok(response);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodo(int id)
    {
        var  todo = await _todoRepository.GetTodo(id);
        if (todo == null)
        {
            return NotFound("Todo not found");
        }
        var response = new
        {
            message = "Fetch successfully",
            statusCode = 200,
            status = "Success",
            data = todo
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddTodo(Todo data)
    {
        var  todo = await _todoRepository.AddTodo(data);
        var response = new
        {
            message = "Added successfully",
            statusCode = 201,
            status = "Success",
            data = new {id = todo.Id, title = todo.Title}
        };
        return Ok(response);
        
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTodo(Todo todo, int id)
    {
        var item = await _todoRepository.UpdateTodo(todo, id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _todoRepository.DeleteTodo(id);
        if (todo == null)
        {
            return NotFound();
        }
        return Ok(todo);
    }
}