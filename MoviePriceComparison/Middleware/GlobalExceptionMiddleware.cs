using System.Net;
using System.Text.Json;

namespace MoviePriceComparison.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Proceed to the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception occurred while processing request to {httpContext.Request.Path}");

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    Message = "An unexpected error occurred.",
                    Error = ex.Message, // Optional: hide in production
                    Path = httpContext.Request.Path
                };

                var json = JsonSerializer.Serialize(response);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }

}
