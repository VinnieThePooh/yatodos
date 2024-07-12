using Todos.Models.Domain;

namespace TrueCode.Todos.Models;

public class CreateTodoRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? IsCompleted { get; set; }

    public PriorityLevel Priority { get; set; }
}