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
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        
        string[] rolesNames = ["Administrator", "User"];
        var rolesToAdd = await InitRoles(rolesNames, roleManager);
        
        //init users
        var users = new [] {
        
            ("ValeraAdmin", "valeraadmin@ryansoftware.com", "secret"),
            ("NarutoUzumaki", "uzumaki@ryansoftware.com", "narutoSecret")
        };
        await InitUsers(users, rolesToAdd, userManager);
    }

    private static async Task<AppRole[]> InitRoles(string[] roleNames, RoleManager<AppRole> roleManager)
    {
        var keysNormalizer = roleManager.KeyNormalizer;
        foreach (string roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole { Name = roleName, NormalizedName = keysNormalizer.NormalizeName(roleName) };
                await roleManager.CreateAsync(role);
            }
        }
        
        return await roleManager.Roles.Where(x => roleNames.Contains(x.Name)).ToArrayAsync();
    }

    private static async Task InitUsers(
        (string name, string email, string password)[] users, 
        AppRole[] rolesToAdd,
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
                _ = await userManager.CreateAsync(user);
            }
            await AssignRoles(userManager, user, rolesToAdd);
        }
    }

    private static async Task AssignRoles(UserManager<AppUser> userManager, AppUser user, IEnumerable<AppRole> roles)
    {
        await userManager.AddToRolesAsync(user, roles.Select(x => x.Name));
    }
}