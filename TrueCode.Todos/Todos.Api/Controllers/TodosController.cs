using Microsoft.AspNetCore.Authentication.BearerToken;
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
[Route("api/[controller]")]
public class TodosController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITodoService _todoService;
    private readonly ILogger<TodosController> _logger;
    private int CurrentUserId => User.GetUserId<int>();
    
    public TodosController(ITodoService todoService, UserManager<AppUser> userManager, ILogger<TodosController> logger)
    {
        _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet("getuser")]
    [Authorize(Roles = RoleNames.USER)]
    public async Task<ActionResult<AppUser>> GetUser()
    {
        var id = User.GetUserId<int>();
        
        Console.WriteLine("Claims received:");
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }
        
        if(id == default)
            return Unauthorized("No user ID claim present in token.");
        
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString()); 
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet]
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
