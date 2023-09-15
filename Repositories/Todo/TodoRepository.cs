using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.CustomException.Exceptions;
using TaskManagementAPI.Database;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Pagination;

namespace TaskManagementAPI.Repositories.Todo;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TodoRepository> _logger;
    
    public TodoRepository(ApplicationDbContext context, ILogger<TodoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<Models.Todo>> GetTodos(int page, int itemsPerPage)
    {
        try
        {
            // Calculate total items and total pages
            var totalItems = await _context.Todos.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            // Apply pagination
            var todos = await _context.Todos
                .Include(todo => todo.User) 
                .ThenInclude(user => user.Profile) 
                .OrderByDescending(todo => todo.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
            
            // Create the pagination metadata
            var meta = new PaginationMetaData
            {
                TotalItems = totalItems,
                ItemCount = todos.Count,
                ItemsPerPage = itemsPerPage,
                TotalPages = totalPages,
                CurrentPage = page
            };

            var paginationLinks = new PaginationLinks("http://localhost:5178/api/", page, totalPages, itemsPerPage);

            // Create the paged result
            return new PagedResult<Models.Todo> 
            {
                Data = todos,
                Meta = meta,
                Links = paginationLinks 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new InternalServerException(ex.Message);
        }
    }

    public async Task<Models.Todo> GetTodo(int id)
    {
        var todo = await _context.Todos.Include(todo => todo.User) 
            .ThenInclude(user => user.Profile)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (todo == null)
        {
            throw new NotFoundException("Todo not found"); 
        }
        return todo;
    }


    public async Task<Models.Todo> AddTodo(TodoDto todoDto, int userId)
    {
        try
        {
            var todo = new Models.Todo()
            {
                Duration = todoDto.Duration,
                Title = todoDto.Title,
                IsCompleted = false,
                UserId = userId
            };
            await _context.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new InternalServerException(ex.Message);
        }
    }

    public async Task<Models.Todo?> UpdateTodo(Models.Todo todo, int id)
    {
        var findTodo = await _context.Todos.FindAsync(id);
        if (findTodo == null)
        {
            return null;
        }

        findTodo.Title = todo.Title;
        findTodo.Duration = todo.Duration;
        findTodo.IsCompleted = todo.IsCompleted;
        _context.Todos.Update(findTodo);
        await _context.SaveChangesAsync();
        return todo;

    }

    public async Task<Models.Todo?> DeleteTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            throw new NotFoundException("Not found");
        }
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
}