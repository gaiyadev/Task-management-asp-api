using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Database;

namespace TaskManagementAPI.Repositories.Todo;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;

    public TodoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Models.Todo>> GetTodos()
    {
        return await _context.Todos.OrderByDescending(todo => todo.Id)
            .ToListAsync();
    }

    public async Task<Models.Todo?> GetTodo(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task<Models.Todo> AddTodo(Models.Todo todo)
    {
         await  _context.AddAsync(todo);
         await _context.SaveChangesAsync();
         return todo;
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

    public async   Task<Models.Todo?> DeleteTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return null;
        }
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
}