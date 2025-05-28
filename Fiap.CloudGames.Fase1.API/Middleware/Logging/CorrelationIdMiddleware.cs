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

        public async Task Invoke(HttpContext httpContext, ICorrelationIdGenerator correlationIdGenerator)
        {
            var correlationId = GetCorrelationId(httpContext, correlationIdGenerator);
            var logContext = Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId);

            httpContext.Response.Headers[_correlationIdHeader] = new[] { correlationId.ToString() };
            try
            {
                await _next(httpContext);
            }
            finally
            {
                logContext.Dispose();
            }
        }

        private static StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            {
                correlationIdGenerator.Set(correlationId);
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                correlationIdGenerator.Set(correlationId);
            }
            context.Items["CorrelationId"] = correlationId;

            return correlationId;
        }
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
