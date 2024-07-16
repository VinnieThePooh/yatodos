using System.Collections.ObjectModel;
using Todos.DataAccess;
using Todos.Models.Domain;

namespace TrueCode.Todos.Services;

public class CacheService
{
    /// <summary>
    /// Maps priority value (PriorityLevel enum) to TodoPriority.Id PK
    /// </summary>
    private readonly Dictionary<int, int> _priorityCache = new();

    public ReadOnlyDictionary<int, int> PriorityCache => _priorityCache.AsReadOnly();

    public async Task InitCache(TodosContext context)
    {
        foreach (var priority in context.Set<TodoPriority>())
        {
            _priorityCache.Add((int)priority.Value, priority.Id);
        }
    }
}