namespace Todos.Models.Domain;

public class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public bool Completed { get; set; }

    public DateTime? DueDate { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }
    
    public int PriorityId { get; set; }

    public TodoPriority? Priority { get; set; }
}