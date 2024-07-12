using System.Globalization;
using System.Security.Claims;

namespace TrueCode.Todos.Extensions;

public static class ApplicationExtensions
{
    public static T GetUserId<T>(this ClaimsPrincipal principal) where T : IParsable<T>
    {
        return (T.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), CultureInfo.InvariantCulture,
            out var result)
            ? result
            : default)!;
    }
}