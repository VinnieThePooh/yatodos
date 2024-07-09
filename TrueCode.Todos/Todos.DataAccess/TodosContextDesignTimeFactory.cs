using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Todos.DataAccess;

public class TodosContextDesignTimeFactory : IDesignTimeDbContextFactory<TodosContext>
{
    public TodosContext CreateDbContext(string[] args)
    {
        var conString = "User ID=ryan;Host=localhost;Port=5432;Database=Todos;Pooling=true;";
        var builder = new DbContextOptionsBuilder<TodosContext>().UseNpgsql(conString);
        return new TodosContext(builder.Options);
    }
}