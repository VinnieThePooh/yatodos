using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todos.Models.Domain;

namespace Todos.DataAccess.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("Todos");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Priority)
            .WithMany(x => x.TodoItems)
            .HasForeignKey(x => x.PriorityId)
            .IsRequired(false);
    }
}