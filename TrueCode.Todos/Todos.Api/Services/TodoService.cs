using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Todos.DataAccess;
using Todos.DataAccess.Identity;
using Todos.Models.Domain;
using TrueCode.Todos.Extensions;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public class TodoService : ITodoService, ITodoFilterProvider
{
    private readonly IDbContextFactory<TodosContext> _contextFactory;
    private const int DEFAULT_PAGE_SIZE = 10;
    private readonly ITodoFilterProvider _filterProvider;
    private readonly CacheService _cacheService;

    public TodoService(IDbContextFactory<TodosContext> contextFactory, CacheService cacheService)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _filterProvider = this;
    }

    public async Task<PaginationModel<TodoListItem>> GetTodos(int? pageNumber, int? pageSize, int userId, TodoFilter? filter = null)
    {
        var ps = pageSize ?? DEFAULT_PAGE_SIZE;
        var pn = pageNumber ?? 1;
        
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        IQueryable<TodoItem> sourceData = context.Set<TodoItem>().Where(x => x.UserId == userId);
        if (filter.HasValue)
        {
            var filterExpression =  _filterProvider.CreateFilterExpression(filter.Value);
            sourceData = sourceData.Where(filterExpression);
        }

        var data = await sourceData.Include(x => x.Priority)
            .OrderByDescending(x => x.CreateDate).Skip((pn - 1) * ps).Take(ps)
            .Select(x => x.ToListItem()).ToArrayAsync();

        var totalCount = await sourceData.CountAsync();
        return new PaginationModel<TodoListItem>
        {
            PageCount =  GetPageCount(totalCount, ps),
            PageData = data,
            PageNumber = pn,
            PageSize = ps,
            TotalCount = totalCount
        };
    }

    public async Task<CreateTodoResponse> CreateTodo(CreateTodoRequest request, int userId, DateTime? timeTrace)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var item = new TodoItem
        {
            CreateDate = timeTrace ?? DateTime.UtcNow,
            UserId = userId,
            DueDate = request.DueDate,
            Description = request.Description,
            Title = request.Title,
            PriorityId =  _cacheService.PriorityCache[request.Priority],
            Completed = request.IsCompleted ?? false
        };
        
        context.Set<TodoItem>().Add(item);
        await context.SaveChangesAsync();
        return new CreateTodoResponse(item.Id, item.CreateDate);
    }

    public async Task UpdateTodo(UpdateTodoRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var priorityId = _cacheService.PriorityCache[request.Priority];
        
        await context.Set<TodoItem>().Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .ExecuteUpdateAsync((setters) => setters
                .SetProperty(x => x.Title, request.Title)
                .SetProperty(x => x.Completed, request.IsCompleted)
                .SetProperty(x => x.Description, request.Description)
                .SetProperty(x => x.DueDate, request.DueDate)
                .SetProperty(x => x.PriorityId, priorityId)
            );
    }

    public async Task UpdatePriority(UpdatePriorityRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var priorityId = _cacheService.PriorityCache[request.Priority];
        await context.Set<TodoItem>().Where(x => x.Id == request.TodoId && x.UserId == request.UserId)
            .ExecuteUpdateAsync((setters) => setters.SetProperty(x => x.PriorityId, priorityId));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="todoId"></param>
    /// <param name="userId"></param>
    /// <returns>Operation result - succeeded or not</returns>
    public async Task<bool> AssignTodoToUser(int userId, int todoId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var user = await context.Set<AppUser>().FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return false;

        var affected = await context.Set<TodoItem>().Where(x => x.Id == todoId)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.UserId, userId));
        return affected > 0;
    }

    public async Task DeleteTodo(int todoId, int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Set<TodoItem>().Where(x => x.Id == todoId && x.UserId == userId).ExecuteDeleteAsync();
    }

    private static int GetPageCount(int totalCount, int pageSize)
    {
        if (totalCount == 0)
            return 0;
        
        var quotient = Math.DivRem(totalCount, pageSize, out var remainder);
        return remainder == 0 ? quotient : quotient + 1;
    }

    Expression<Func<TodoItem, bool>> ITodoFilterProvider.CreateFilterExpression(TodoFilter filter)
    {
        using var context = _contextFactory.CreateDbContext();
        int? priorityId = filter.Priority.HasValue ? _cacheService.PriorityCache[filter.Priority.Value] : null;

        return todo =>
            (filter.IsCompleted != null && todo.Completed == filter.IsCompleted || filter.IsCompleted == null) &&
            (priorityId != null && todo.PriorityId == priorityId || priorityId == null);
    }
}