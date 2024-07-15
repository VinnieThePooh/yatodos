using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrueCode.Todos.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class Healthcheck : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("It works!");
    }
}