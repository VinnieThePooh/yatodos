using Todos.Models.Domain;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Extensions;

public static class DomainExtensions
{
    public static TodoListItem ToListItem(this TodoItem item)
    {
        return new TodoListItem
        {
            Id = item.Id,
            Priority = item.Priority.Value,
            Completed = item.Completed,
            PriorityName = item.Priority.Name,
            Title = item.Title,
            DueDate = item.DueDate,
            CreateDate = item.CreateDate,
            Description = item.Description
        };
    }
}