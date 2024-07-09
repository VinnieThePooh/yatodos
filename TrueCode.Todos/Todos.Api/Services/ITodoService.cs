using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public interface ITodoService
{
    Task<PaginationModel<TodoListItem>> GetTodos(int? pageSize, int? pageNumber);

    Task<int> CreateTodo(CreateTodoRequest request);

    Task UpdateTodo(UpdateTodoRequest request);

    Task DeleteTodo(int todoId);
}