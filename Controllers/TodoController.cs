using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.CustomException.Exceptions;
using TaskManagementAPI.CustomException.Helper;
using TaskManagementAPI.CustomException.Responses;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories.Todo;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository  _todoRepository;
    private readonly IValidator<TodoDto> _validator;
    private readonly AuthUserIdExtractor _authUserIdExtractor;

    public TodoController(ITodoRepository todoRepository, IValidator<TodoDto> validator, AuthUserIdExtractor authUserIdExtractor)
    {
        _todoRepository = todoRepository;
        _validator = validator;
        _authUserIdExtractor = authUserIdExtractor;

    }
    
    [HttpGet]
    public async Task<IActionResult> GetTodos([FromQuery] int page = 1, [FromQuery] int itemsPerPage = 10)
    {
        try
        {
            var todos = await _todoRepository.GetTodos(page, itemsPerPage);
            return SuccessResponse.HandleOk("Successfully Fetch", todos, null);
        }
        catch (InternalServerException ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTodo(int id)
    {
        try
        {
            var todo = await _todoRepository.GetTodo(id);
            return SuccessResponse.HandleOk("Successfully Fetched", todo, null);
        }
        catch (NotFoundException ex)
        {
            return ApplicationExceptionResponseHelper.HandleNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddTodo(TodoDto todoDto)
    {
        // Validate the DTO using Fluent Validation
        var validationResult = await _validator.ValidateAsync(todoDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(error => error.ErrorMessage)
                .ToList();
            return BadRequest(errors);
        }
        // Get the user from HttpContext or your authentication mechanism
        var user = HttpContext.User;
        // Pass the user to the repository to get the user's ID
        var userId = _authUserIdExtractor.GetUserId(user);
        try
        {
            var  todo = await _todoRepository.AddTodo(todoDto, userId);
            var apiResponse = new List<object>
            {
                new { id = todo.Id, title = todo.Title }
            };
            return SuccessResponse.HandleCreated("Successfully Added", null, apiResponse);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
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
        try
        {
            var todo = await _todoRepository.DeleteTodo(id);
            return Ok(todo);
        }
        catch (NotFoundException ex)
        {
            return ApplicationExceptionResponseHelper.HandleNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }
}