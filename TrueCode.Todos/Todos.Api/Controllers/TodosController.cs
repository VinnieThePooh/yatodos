using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TrueCode.Todos.Extensions;
using TrueCode.Todos.Models;
using TrueCode.Todos.Services;

namespace TrueCode.Todos.Controllers;

// [Authorize("User")]
[ApiController]
[Route("[controller]")]
public class TodosController : Controller
{
    private readonly ITodoService todoService;
    private int CurrentUserId => User.GetUserId<int>();
    
    public TodosController(ITodoService todoService)
    {
        this.todoService = todoService;
    }
    
    // GET
    public async Task<PaginationModel<TodoListItem>> Index(int? pageSize, int? pageNumber)
    {
        var model = await todoService.GetTodos(pageSize, pageNumber, CurrentUserId);
        return model;
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Index(CreateTodoRequest request)
    {
        var result = await todoService.CreateTodo(request, CurrentUserId);
        return Ok(result);
    }
    
    
    [HttpPut]
    public async Task<IActionResult> Index(UpdateTodoRequest request)
    {
        await todoService.UpdateTodo(request);
        return NoContent();
    }

    [HttpDelete("{todoId:int}")]
    public async Task<IActionResult> Index(int todoId)
    {
        await todoService.DeleteTodo(todoId);
        return NoContent();
    }
}
