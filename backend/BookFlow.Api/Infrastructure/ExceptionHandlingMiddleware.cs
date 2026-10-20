using System.Text.Json;
using BookFlow.Domain.Entities;

namespace BookFlow.Api.Infrastructure;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request failed: {Method} {Path}", context.Request.Method, context.Request.Path);
            if (context.Response.HasStarted) throw;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                BusinessRuleException or ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            var message = context.Response.StatusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected server error occurred." : ex.Message;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
        }
    }
}
