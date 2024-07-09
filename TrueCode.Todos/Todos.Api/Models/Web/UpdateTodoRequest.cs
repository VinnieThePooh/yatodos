using Todos.Models.Domain;

namespace TrueCode.Todos.Models;

public class UpdateTodoRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public PriorityLevel Priority { get; set; }
}