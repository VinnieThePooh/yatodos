using Microsoft.EntityFrameworkCore;
using Todos.DataAccess;
using Todos.Models.Domain;
using TrueCode.Todos.Extensions;
using TrueCode.Todos.Models;

namespace TrueCode.Todos.Services;

public class TodoService : ITodoService
{
    private const int DEFAULT_PAGE_SIZE = 10;
    
    private readonly TodosContext _context;

    public TodoService(TodosContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PaginationModel<TodoListItem>> GetTodos(int? pageNumber, int? pageSize, int userId)
    {
        var ps = pageSize ?? DEFAULT_PAGE_SIZE;
        var pn = pageNumber ?? 1;

        var data = await _context.Set<TodoItem>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreateDate).Skip((pn - 1) * ps).Take(ps)
            .Select(x => x.ToListItem()).ToArrayAsync();

        var totalCount = await _context.Set<TodoItem>().CountAsync();
        return new PaginationModel<TodoListItem>
        {
            PageCount =  GetPageCount(totalCount, ps),
            PageData = data,
            PageNumber = pn,
            PageSize = ps,
            TotalCount = totalCount
        };
    }

    public async Task<int> CreateTodo(CreateTodoRequest request, int userId, DateTime? timeTrace)
    {
        var item = new TodoItem
        {
            CreateDate = timeTrace ?? DateTime.Now,
            UserId = userId,
            DueDate = request.DueDate,
            Description = request.Description,
            Title = request.Title,
            PriorityId =  await FindPriorityId(request.Priority),
            Completed = request.IsCompleted ?? false
        };
        _context.Set<TodoItem>().Add(item);
        await _context.SaveChangesAsync();
        return item.Id;
    }

    public async Task UpdateTodo(UpdateTodoRequest request)
    {
        //todo: get from memory
        var priorityId = await FindPriorityId(request.Priority);
        await _context.Set<TodoItem>().Where(x => x.Id == request.TodoId)
            .ExecuteUpdateAsync((setters) => setters
                .SetProperty(x => x.Title, request.Title)
                .SetProperty(x => x.Completed, request.IsCompleted)
                .SetProperty(x => x.Description, request.Description)
                .SetProperty(x => x.DueDate, request.DueDate)
                .SetProperty(x => x.PriorityId, priorityId)
            );
    }

    public PriorityLevel Priority { get; set; }

    public async Task DeleteTodo(int todoId)
    {
        await _context.Set<TodoItem>().Where(x => x.Id == todoId).ExecuteDeleteAsync();
    }

    private static int GetPageCount(int totalCount, int pageSize)
    {
        if (totalCount == 0)
            return 0;
        
        var quotient = Math.DivRem(totalCount, pageSize, out var remainder);
        return remainder == 0 ? quotient : quotient + 1;
    }
    
    //todo: should be cached
    async Task<int> FindPriorityId(PriorityLevel priorityValue)
    {
        var priority = await _context.Set<TodoPriority>().FirstAsync(x => x.Value == priorityValue);
        return priority.Id;
    }
}