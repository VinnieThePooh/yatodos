using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Todos.DataAccess;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;

namespace TrueCode.Todos;

public static class SampleData
{
    public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        string[] rolesNames = ["Administrator", "User"];
        List<AppRole> roles = new();
        var context = serviceProvider.GetRequiredService<TodosContext>();
        var roleStore = new RoleStore<AppRole, TodosContext, int>(context);

        foreach (string name in rolesNames)
        {
            if (!context.Roles.Any(r => r.Name == name))
            {
                var role = new AppRole() { Name = name, NormalizedName = name.ToLower() };
                roles.Add(role);
                await roleStore.CreateAsync(role);
            }
        }

        var user = new AppUser
        {
            Email = "valeraadmin@ryansoftware.com",
            NormalizedEmail = "valeraadmin@ryansoftware.com",
            UserName = "ValeraAdmin",
            NormalizedUserName = "valeraadmin",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };

        var userStore = new UserStore<AppUser, AppRole, TodosContext, int>(context);
        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<AppUser>();
            var hashed = password.HashPassword(user, "secret");
            user.PasswordHash = hashed;
            //our domain part
            user.User = new User() { UserName = user.UserName };
            _ = await userStore.CreateAsync(user);
        }
        await AssignRoles(userStore, user, roles);
    }

    private static async Task AssignRoles(UserStore<AppUser, AppRole, TodosContext, int> userStore, AppUser user, IEnumerable<AppRole> roles)
    {
        foreach (var role in roles)
            await userStore.AddToRoleAsync(user, role.NormalizedName);
    }
}