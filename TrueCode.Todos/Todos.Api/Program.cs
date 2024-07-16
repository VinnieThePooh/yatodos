using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Todos.DataAccess;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;
using TrueCode.Todos;
using TrueCode.Todos.Auth;
using TrueCode.Todos.Controllers;
using TrueCode.Todos.Exceptions;
using TrueCode.Todos.Models;
using TrueCode.Todos.Services;
using TrueCode.Todos.Validation;

var builder = WebApplication.CreateBuilder(args);
var conString = builder.Configuration.GetConnectionString("DefaultConnection");

ConfigurationExt.ConfigureSingletonSettings<JwtSettings>(builder);
ConfigurationExt.ConfigureSingletonSettings<CorsSettings>(builder);

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionKey).Get<JwtSettings>();
var corsSettings = builder.Configuration.GetSection(CorsSettings.SectionKey).Get<CorsSettings>();

builder.Services.AddSingleton<IValidator<CreateTodoRequest>, CreateRequestValidator>();
builder.Services.AddSingleton<IValidator<UpdateTodoRequest>, UpdateRequestValidator>();
builder.Services.AddSingleton<IValidator<UpdatePriorityRequest>, UpdatePriorityValidator>();
builder.Services.AddSingleton<ITodoService, TodoService>();
builder.Services.AddDbContextFactory<TodosContext>(options => options.UseNpgsql(conString));
builder.Services.AddExceptionHandler<ExceptionsHandler>();

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddUserStore<UserStore<AppUser, AppRole, TodosContext, int>>()
    .AddRoleStore<RoleStore<AppRole, TodosContext, int>>();

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.PrivateKey))
        };
    });

var  localAngularApp = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: localAngularApp,
        policy  =>
        {
            policy.WithOrigins(corsSettings.AllowedOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder();
builder.Services
    .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonNumberEnumConverter<PriorityLevel>());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Todos API", Version = "v1" });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

var app = builder.Build();

app.MapGet("", context =>
{
    context.Response.Redirect("swagger/index.html");
    return Task.CompletedTask;
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        ExceptionHandler = _ => Task.CompletedTask
    });
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodosContext>();
    await context.Database.MigrateAsync();
    await SampleData.SeedUsersAndRoles(scope.ServiceProvider);
}

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors(localAngularApp);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();