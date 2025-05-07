using Fiap.CloudGames.Fase1.Infrastructure.Logging;
using Microsoft.Extensions.Primitives;

namespace Fiap.CloudGames.Fase1.API.Middleware.Logging
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _correlationIdHeader = "x-correlation-id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, ICorrelationIdGenerator correlationIdGenerator)
        {
            var correlationId = GetCorrelationId(httpContext, correlationIdGenerator);
            AddCorrelationIdHeaderToResponse(httpContext, correlationId);

            return _next(httpContext);
        }

        private static StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            {
                correlationIdGenerator.Set(correlationId);
                return correlationId;
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                correlationIdGenerator.Set(correlationId);
                return correlationId;
            }
        }

        private static void AddCorrelationIdHeaderToResponse(HttpContext context, StringValues correlationId)
       => context.Response.OnStarting(() =>
       {
           context.Response.Headers[_correlationIdHeader] = new[] { correlationId.ToString() };
           return Task.CompletedTask;
       });
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CorrelationIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
