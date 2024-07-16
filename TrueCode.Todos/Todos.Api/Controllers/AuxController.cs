using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrueCode.Todos.Auth;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api")]
public class AuxController : Controller
{
    [HttpGet("healthcheck")]
    public IActionResult Index()
    {
        return Ok("It works!");
    }
    
    [HttpGet("configuration")]
    public ConfigSnapshot Index(
        [FromServices] IConfiguration configuration, 
        [FromServices] JwtSettings jwt,
        [FromServices] CorsSettings cors)
    {
        return new ConfigSnapshot
        {
            Jwt = jwt,
            Cors = cors,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!,
            DefaultConnectionString = configuration.GetConnectionString("DefaultConnection")!
        };
    }
}