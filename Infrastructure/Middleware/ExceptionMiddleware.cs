using System.Net;
using SubscriptionApi.Common.Exceptions;

namespace SubscriptionApi.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, message) = exception switch
        {
            ServiceNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            DuplicateSubscriptionException => (StatusCodes.Status409Conflict, exception.Message),
            SubscriptionNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            InvalidSubscriptionDurationException => (StatusCodes.Status400BadRequest, exception.Message),
            InvalidPhoneNumberException => (StatusCodes.Status400BadRequest, exception.Message),
            SubscriptionException => (StatusCodes.Status400BadRequest, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var response = new
        {
            statusCode,
            message,
            error = exception.GetType().Name
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}