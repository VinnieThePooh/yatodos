using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;

namespace Todos.DataAccess.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.User).WithOne().HasForeignKey<User>(x => x.Id).IsRequired();
    }
}