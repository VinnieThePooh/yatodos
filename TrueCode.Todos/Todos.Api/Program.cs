using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todos.DataAccess;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;
using TrueCode.Todos;

var builder = WebApplication.CreateBuilder(args);
var conString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodosContext>(options => options.UseNpgsql(conString));
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddUserStore<UserStore<AppUser, AppRole, TodosContext, int>>()
    .AddRoleStore<RoleStore<AppRole, TodosContext, int>>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
    await SampleData.SeedUsersAndRoles(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();