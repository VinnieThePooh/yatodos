using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;
using TrueCode.Todos.Constants;
using TrueCode.Todos.Extensions;
using TrueCode.Todos.Models;
using TrueCode.Todos.Services;

namespace TrueCode.Todos.Controllers;

// [Authorize("User")]
[ApiController]
[Route("[controller]")]
public class TodosController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITodoService _todoService;
    private int CurrentUserId => User.GetUserId<int>();
    
    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }
    
    [HttpGet("getuser")]
    [Authorize(Roles = RoleNames.USER)]
    public async Task<ActionResult<User>> GetUser()
    {
        // Retrieve userId from the claims
        var userIdClaim = _userManager.GetUserId(User);
        
        Console.WriteLine("Claims received:");
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }
        
        if(userIdClaim is null)
            return Unauthorized("No user ID claim present in token.");
        
        try
        {
            var user = await _userManager.FindByIdAsync(userIdClaim); 
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // GET
    public async Task<PaginationModel<TodoListItem>> Index(int? pageSize, int? pageNumber)
    {
        var model = await _todoService.GetTodos(pageSize, pageNumber, CurrentUserId);
        return model;
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Index(CreateTodoRequest request)
    {
        var result = await _todoService.CreateTodo(request, CurrentUserId);
        return Ok(result);
    }
    
    
    [HttpPut]
    public async Task<IActionResult> Index(UpdateTodoRequest request)
    {
        await _todoService.UpdateTodo(request);
        return NoContent();
    }

    [HttpDelete("{todoId:int}")]
    public async Task<IActionResult> Index(int todoId)
    {
        await _todoService.DeleteTodo(todoId);
        return NoContent();
    }
}
