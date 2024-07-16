using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TrueCode.Todos.Exceptions;

public class ExceptionsHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionsHandler> _logger;

    public ExceptionsHandler(ILogger<ExceptionsHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception");
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = "Internal server error",
            Detail = "Something went wrong",
            Status = httpContext.Response.StatusCode
        }, cancellationToken);
        await httpContext.Response.StartAsync(CancellationToken.None);
        return true;
    }
}