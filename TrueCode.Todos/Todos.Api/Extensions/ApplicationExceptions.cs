using System.Globalization;
using System.Security.Claims;

namespace TrueCode.Todos.Extensions;

public static class ApplicationExceptions
{
    public static T GetUserId<T>(this ClaimsPrincipal principal) where T : IParsable<T>
    {
        var value = T.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier), CultureInfo.InvariantCulture);
        return value;
    }
}