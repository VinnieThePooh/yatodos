using Todos.DataAccess;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public class TodoService : ITodoService
{
    private readonly TodosContext _context;

    public TodoService(TodosContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<PaginationModel<TodoListItem>> GetTodos(int? pageSize, int? pageNumber)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateTodo(CreateTodoRequest request)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTodo(UpdateTodoRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTodo(int todoId)
    {
        throw new NotImplementedException();
    }
}