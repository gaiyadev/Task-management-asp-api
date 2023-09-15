using TaskManagementAPI.DTOs;
using TaskManagementAPI.Pagination;
using TaskManagementAPI.Repositories.Todo;

namespace TaskManagementAPI.Services.Todo;

public class TodoService: ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    
    public async Task<PagedResult<Models.Todo>> GetTodos(int page, int itemsPerPage)
    {
        return await _todoRepository.GetTodos(page, itemsPerPage);
    }

    public async Task<Models.Todo> GetTodo(int id)
    {
        return await _todoRepository.GetTodo(id);
    }

    public async Task<Models.Todo> AddTodo(TodoDto todoDto, int userId)
    {
        return await _todoRepository.AddTodo(todoDto, userId);
    }

    public async Task<Models.Todo?> UpdateTodo(Models.Todo todo, int id)
    {
        return await _todoRepository.UpdateTodo(todo, id);
    }

    public async Task<Models.Todo?> DeleteTodo(int id)
    {
        return await _todoRepository.DeleteTodo(id);
    }
}