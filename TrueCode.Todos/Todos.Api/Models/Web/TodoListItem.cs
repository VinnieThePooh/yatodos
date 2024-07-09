using Todos.Models.Domain;

namespace TrueCode.Todos.Models;

public class TodoListItem
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public PriorityLevel Priority { get; set; }

    public bool Completed { get; set; }
    
    public DateTime? DueDate { get; set; }
}