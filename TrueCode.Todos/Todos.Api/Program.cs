using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Todos.DataAccess;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;
using TrueCode.Todos;
using TrueCode.Todos.Auth;

var builder = WebApplication.CreateBuilder(args);
var conString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings.SettingsKey)));

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SettingsKey).Get<JwtSettings>();

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = BearerTokenDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = BearerTokenDefaults.AuthenticationScheme;
    })
    .AddBearerToken(x =>
    {
        x.BearerTokenExpiration = jwtSettings.AccessTokenExpiration;
        x.RefreshTokenExpiration = jwtSettings.RefreshTokenExpiration;
        // x.BearerTokenProtector = 
        // x.RefreshTokenProtector
        // x.TokenValidationParameters = new TokenValidationParameters
        // {
        //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSettings.PrivateKey)),
        //     ValidateIssuer = false,
        //     ValidateAudience = false
        // };
    });
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Todos API", Version = "v1" });
    // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
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