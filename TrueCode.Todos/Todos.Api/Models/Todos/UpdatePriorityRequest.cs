
using Todos.Models.Domain;
namespace TrueCode.Todos.Models;
public record struct UpdatePriorityRequest(int UserId, int TodoId, int Priority);