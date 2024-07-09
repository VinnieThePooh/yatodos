using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todos.Models.Domain;

namespace Todos.DataAccess.Configurations;

public class TodoPriorityConfiguration : IEntityTypeConfiguration<TodoPriority>
{
    public void Configure(EntityTypeBuilder<TodoPriority> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("TodoPriorities");
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasData(new[] {
            new TodoPriority { Id = 1, Name = "Несрочный", Value = PriorityLevel.Relaxed },
            new TodoPriority { Id = 2, Name = "Умеренный", Value = PriorityLevel.Average },
            new TodoPriority { Id = 3, Name = "Срочный", Value = PriorityLevel.Urgent },
            new TodoPriority { Id = 4, Name = "Чрезвычайно срочный", Value = PriorityLevel.SuperUrgent },
            }
        );
    }
}