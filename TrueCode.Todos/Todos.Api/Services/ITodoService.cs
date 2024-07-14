using Todos.Models.Domain;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public interface ITodoService
{
    Task<PaginationModel<TodoListItem>> GetTodos(int? pageNumber, int? pageSize, int userId);

    Task<CreateTodoResponse> CreateTodo(CreateTodoRequest request, int userId, DateTime? timeTrace = default);

    Task UpdateTodo(UpdateTodoRequest request);

    Task UpdatePriority(UpdatePriorityRequest request);

    Task DeleteTodo(int todoId, int userId);
}