using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SubscriptionApi.Common;

namespace SubscriptionApi.Infrastructure.Middleware;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;

    public IdempotencyMiddleware(RequestDelegate next, IDistributedCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        var idempotencyAttribute = endpoint?.Metadata
            .GetMetadata<IdempotencyAttribute>();

        if (idempotencyAttribute == null)
        {
            await _next(context);
            return;
        }

        var idempotencyKey = context.Request.Headers["Idempotency-Key"].ToString();
        
        try
        {
            var key = new IdempotencyKey(idempotencyKey);
            var cachedResponse = await _cache.GetStringAsync(key.Value);

            if (cachedResponse != null)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResponse);
                return;
            }

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            responseBody.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            await _cache.SetStringAsync(
                key.Value,
                response,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = idempotencyAttribute.ExpirationTime
                });

            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var error = new { error = "Invalid Idempotency-Key header" };
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}