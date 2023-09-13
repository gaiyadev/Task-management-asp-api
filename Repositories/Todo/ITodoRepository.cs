﻿using Pagination.EntityFrameworkCore.Extensions;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Pagination;

namespace TaskManagementAPI.Repositories.Todo;

public interface ITodoRepository
{
    Task<PagedResult<Models.Todo>> GetTodos(int page, int itemsPerPage);
    
    Task<Models.Todo> GetTodo(int id);

    Task<Models.Todo> AddTodo(TodoDto todoDto, int userId);

    Task<Models.Todo?> UpdateTodo(Models.Todo todo, int id);

    Task<Models.Todo?> DeleteTodo(int id);
}