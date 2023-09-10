namespace TaskManagementAPI.Repositories.Todo;

public interface ITodoRepository
{
    Task<List<Models.Todo>> GetTodos();
    
    Task<Models.Todo?> GetTodo(int id);

    Task<Models.Todo> AddTodo(Models.Todo todo);

    Task<Models.Todo?> UpdateTodo(Models.Todo todo, int id);

    Task<Models.Todo?> DeleteTodo(int id);
}