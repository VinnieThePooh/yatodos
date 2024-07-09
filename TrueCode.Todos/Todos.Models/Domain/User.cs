namespace Todos.Models.Domain;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public List<TodoItem> TodoItems { get; set; } = [];
}