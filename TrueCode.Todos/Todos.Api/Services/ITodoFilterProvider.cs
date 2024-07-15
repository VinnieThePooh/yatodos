using System.Linq.Expressions;
using Todos.Models.Domain;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public interface ITodoFilterProvider
{
    Expression<Func<TodoItem, bool>> CreateFilterExpression(TodoFilter filter);
}