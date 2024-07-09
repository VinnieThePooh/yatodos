namespace Todos.Models.Domain;

public class TodoPriority
{
    public int Id { get; set; }
    
    public PriorityLevel Value { get; set; }

    public string Name { get; set; } = null!;

    public List<TodoItem> TodoItems { get; set; } = [];
}