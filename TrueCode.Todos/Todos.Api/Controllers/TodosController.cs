using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todos.DataAccess.Identity;
using TrueCode.Todos.Constants;
using TrueCode.Todos.Extensions;
using TrueCode.Todos.Models;
using TrueCode.Todos.Services;
using TrueCode.Todos.Validation;

namespace TrueCode.Todos.Controllers;

[Authorize(Roles = RoleNames.USER)]
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
    
    [HttpGet]
    public async Task<PaginationModel<TodoListItem>> Index(int? pageSize, int? pageNumber)
    {
        var model = await _todoService.GetTodos(pageSize, pageNumber, CurrentUserId);
        return model;
    }
    
    [HttpPost("filter")]
    public async Task<PaginationModel<TodoListItem>> Index(int? pageSize, int? pageNumber, [FromBody]TodoFilter filter)
    {
        var model = await _todoService.GetTodos(pageNumber, pageSize, CurrentUserId, filter);
        return model;
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Index([FromServices]IValidator<CreateTodoRequest> validator, CreateTodoRequest request)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());
        
        var result = await _todoService.CreateTodo(request, CurrentUserId);
        return Ok(result);
    }
    
    
    [HttpPut]
    public async Task<IActionResult> Index([FromServices]IValidator<UpdateTodoRequest> validator, UpdateTodoRequest request)
    {
        //todo: how to pass HttpContext data into validator?
        if (CurrentUserId != request.UserId)
            return Unauthorized();

        var result = validator.Validate(request);
        if (!result.IsValid)
            return BadRequest(result.ToDictionary());
        
        await _todoService.UpdateTodo(request);
        return NoContent();
    }
    
    [HttpPut("priority")]
    public async Task<IActionResult> UpdatePriority([FromServices]IValidator<UpdatePriorityRequest> validator,  UpdatePriorityRequest request)
    {
        //todo: how to pass HttpContext data into validator?
        if (CurrentUserId != request.UserId)
            return Unauthorized();
        
        var result = validator.Validate(request);
        if (!result.IsValid)
            return BadRequest(result.ToDictionary());
        
        await _todoService.UpdatePriority(request);
        return NoContent();
    }

    [HttpDelete("{todoId:int}")]
    public async Task<IActionResult> Index(int todoId)
    {
        await _todoService.DeleteTodo(todoId, CurrentUserId);
        return NoContent();
    }
    
    [Authorize(nameof(RoleNames.ADMIN))]
    [HttpPost("assign")]
    public async Task<ActionResult<int>> Assign(int userId, int todoId)
    {
        var result = await _todoService.AssignTodoToUser(userId, todoId);
        if (!result)
            return BadRequest("Incorrect parameters");
        return NoContent();
    }
}
