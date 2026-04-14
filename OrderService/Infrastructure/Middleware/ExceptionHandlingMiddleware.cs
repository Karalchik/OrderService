using System.Net;
using System.Text.Json;
using OrderService.Domain.Exceptions;

namespace OrderService.Infrastructure.Middleware
{
    /// <summary>
    /// Global middleware that catches unhandled exceptions and returns
    /// a structured JSON error response with an appropriate HTTP status code.
    /// </summary>
    /// <remarks>
    /// Status code mapping:
    /// <list type="bullet">
    ///   <item><see cref="InvalidOperationException"/> → 400 Bad Request</item>
    ///   <item><see cref="KeyNotFoundException"/> → 404 Not Found</item>
    ///   <item><see cref="ConcurrencyException"/> → 409 Conflict</item>
    ///   <item>All other exceptions → 500 Internal Server Error</item>
    /// </list>
    /// </remarks>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="ExceptionHandlingMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        /// <param name="logger">Logger for recording exception details.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware. Delegates to the next middleware and catches any unhandled exceptions.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maps the exception type to an HTTP status code and writes a JSON error body.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch
            {
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ConcurrencyException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                error = exception.Message,
                detail = "Please check the request parameters or contact support."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}