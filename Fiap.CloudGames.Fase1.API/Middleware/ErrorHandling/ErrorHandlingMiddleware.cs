using System.Net;
using Fiap.CloudGames.Fase1.Domain.Exceptions;

namespace Fiap.CloudGames.Fase1.API.Middleware.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "A domain error occurred.");
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var response = new
                {
                    error = "Invalid operation",
                    details = ex.Message
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                // The technical details are logged, but not presented to the user.
                _logger.LogError(ex, "An unhandled exception occurred.");

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    error = "An unexpected error occurred.",
                    details = "If you need help, please contact the technical support."
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
