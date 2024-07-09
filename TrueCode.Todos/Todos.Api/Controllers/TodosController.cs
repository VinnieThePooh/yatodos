using Microsoft.AspNetCore.Mvc;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Controllers;

// [Authorize("User")]
[ApiController]
[Route("[controller]")]
public class TodosController : Controller
{
    // GET
    public IActionResult Index(int pageSize, int pageNumber)
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Index(CreateTodoRequest request)
    {
        return NoContent();
    }
    
    
    [HttpPut]
    public IActionResult Index(UpdateTodoRequest request)
    {
        return NoContent();
    }

    [HttpDelete("{todoId:int}")]
    public IActionResult Index(int todoId)
    {
        return NoContent();
    }
}
