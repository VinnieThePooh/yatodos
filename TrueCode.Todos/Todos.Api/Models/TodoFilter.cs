namespace TrueCode.Todos.Models;

public struct TodoFilter
{
    public bool? IsCompleted { get; set; }
    public int? Priority { get; set; }
}