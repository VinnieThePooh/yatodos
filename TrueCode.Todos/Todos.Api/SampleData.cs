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
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        
        string[] rolesNames = ["Administrator", "User"];
        List<AppRole> roles = new();
        var context = serviceProvider.GetRequiredService<TodosContext>();
        var roleStore = new RoleStore<AppRole, TodosContext, int>(context);

        //init roles
        foreach (string roleName in rolesNames)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                var role = new AppRole { Name = roleName, NormalizedName = userManager.KeyNormalizer.NormalizeName(roleName) };
                roles.Add(role);
                await roleStore.CreateAsync(role);
            }
        }
        
        //init users
        var name = "ValeraAdmin";
        var email = "valeraadmin@ryansoftware.com";
        var password = "secret";
        
        var user = new AppUser
        {
            Email = email,
            NormalizedEmail =  userManager.KeyNormalizer.NormalizeEmail(email),
            UserName = name,
            NormalizedUserName = userManager.KeyNormalizer.NormalizeName(name),
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };
        
        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
            //our domain part
            user.User = new User { UserName = user.UserName };
            _ = await userManager.CreateAsync(user);
        }
        await AssignRoles(userManager, user, roles);
    }

    private static async Task AssignRoles(UserManager<AppUser> userManager, AppUser user, IEnumerable<AppRole> roles)
    {
        foreach (var role in roles)
            await userManager.AddToRoleAsync(user, role.NormalizedName);
    }
}