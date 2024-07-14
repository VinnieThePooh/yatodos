using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Todos.Models.Domain;

namespace TrueCode.Todos.Models;

public class CreateTodoRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? IsCompleted { get; set; }
    
    public int Priority { get; set; }
    
    //todo: how to input and output both enums treated as numbers?
    // public PriorityLevel Priority { get; set; }
}