using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todos.DataAccess.Configurations;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;

namespace Todos.DataAccess;

public class TodosContext : IdentityDbContext<AppUser, AppRole, int>
{
    public TodosContext(DbContextOptions<TodosContext> options):base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ApplyConfigurations(builder);
    }

    private void ApplyConfigurations(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TodoItemConfiguration());
        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new AppRoleConfiguration());
        builder.ApplyConfiguration(new TodoPriorityConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
    }
}

