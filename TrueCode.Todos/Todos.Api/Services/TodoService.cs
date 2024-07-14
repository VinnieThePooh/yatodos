using Microsoft.EntityFrameworkCore;
using Todos.DataAccess;
using Todos.Models.Domain;
using TrueCode.Todos.Controllers;
using TrueCode.Todos.Extensions;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public class TodoService : ITodoService
{
    private readonly IDbContextFactory<TodosContext> _contextFactory;
    private const int DEFAULT_PAGE_SIZE = 10;

    public TodoService(IDbContextFactory<TodosContext> contextFactory)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    public async Task<PaginationModel<TodoListItem>> GetTodos(int? pageNumber, int? pageSize, int userId)
    {
        var ps = pageSize ?? DEFAULT_PAGE_SIZE;
        var pn = pageNumber ?? 1;

        await using var context = await _contextFactory.CreateDbContextAsync();

        var data = await context.Set<TodoItem>().Include(x => x.Priority)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreateDate).Skip((pn - 1) * ps).Take(ps)
            .Select(x => x.ToListItem()).ToArrayAsync();

        var totalCount = await context.Set<TodoItem>().CountAsync();
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
            PriorityId =  await FindPriorityId((PriorityLevel)request.Priority, context),
            Completed = request.IsCompleted ?? false
        };
        
        context.Set<TodoItem>().Add(item);
        await context.SaveChangesAsync();
        return new CreateTodoResponse(item.Id, item.CreateDate);
    }

    public async Task UpdateTodo(UpdateTodoRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        //todo: get from memory
        var priorityId = await FindPriorityId((PriorityLevel)request.Priority, context);
        await context.Set<TodoItem>().Where(x => x.Id == request.Id)
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
        
        //todo: get from memory
        var priorityId = await FindPriorityId(request.Priority, context);
        await context.Set<TodoItem>().Where(x => x.Id == request.TodoId && x.UserId == request.UserId)
            .ExecuteUpdateAsync((setters) => setters.SetProperty(x => x.PriorityId, priorityId));
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
    
    //todo: should be cached
    async Task<int> FindPriorityId(PriorityLevel priorityValue, TodosContext context)
    {
        var priority = await context.Set<TodoPriority>().FirstAsync(x => x.Value == priorityValue);
        return priority.Id;
    }
}