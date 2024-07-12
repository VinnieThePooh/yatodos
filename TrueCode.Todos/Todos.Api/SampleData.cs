using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todos.DataAccess;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;

namespace TrueCode.Todos;

public static class SampleData
{
    public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var context = serviceProvider.GetRequiredService<TodosContext>();
        var roleStore = new RoleStore<AppRole, TodosContext, int>(context);
        
        string[] rolesNames = ["Administrator", "User"];
        
        var allRoles = await context.Roles.ToArrayAsync();
        await InitRoles(rolesNames, roleStore, userManager, allRoles);
        
        //init users
        var users = new [] {
        
            ("ValeraAdmin", "valeraadmin@ryansoftware.com", "secret"),
            ("NarutoUzumaki", "uzumaki@ryansoftware.com", "narutoSecret")
        };
        await InitUsers(users, allRoles, userManager);
    }

    private static async Task InitRoles(string[] roleNames, IRoleStore<AppRole> roleStore, UserManager<AppUser> userManager, AppRole[] allRoles)
    {
        foreach (string roleName in roleNames)
        {
            if (!allRoles.Any(r => r.Name == roleName))
            {
                var role = new AppRole { Name = roleName, NormalizedName = userManager.KeyNormalizer.NormalizeName(roleName) };
                await roleStore.CreateAsync(role, CancellationToken.None);
            }
        }
    }

    private static async Task InitUsers(
        (string name, string email, string password)[] users, 
        AppRole[] allRoles,
        UserManager<AppUser> userManager)
    {
        foreach (var (name, email, password) in users)
        {
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
            
            if (!userManager.Users.Any(u => u.UserName == user.UserName))
            {
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
                //our domain part
                user.User = new User { UserName = user.UserName };
                _ = await userManager.CreateAsync(user);
            }
            await AssignRoles(userManager, user, allRoles);
        }
    }

    private static async Task AssignRoles(UserManager<AppUser> userManager, AppUser user, IEnumerable<AppRole> roles)
    {
        foreach (var role in roles)
            await userManager.AddToRoleAsync(user, role.NormalizedName);
    }
}