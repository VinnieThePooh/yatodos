using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrueCode.Todos.Controllers;

[AllowAnonymous]
public class Healthcheck : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("It works!");
    }
}