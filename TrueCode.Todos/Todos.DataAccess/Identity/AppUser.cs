using Microsoft.AspNetCore.Identity;
using Todos.Models.Domain;

namespace Todos.DataAccess.Identity;

public class AppUser : IdentityUser<int>
{
    public List<TodoItem> Todos { get; set; } = [];
}