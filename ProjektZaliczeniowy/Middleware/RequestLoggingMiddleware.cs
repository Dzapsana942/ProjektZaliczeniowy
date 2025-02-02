using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ProjektZaliczeniowy.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"Middleware działa! Żądanie: {context.Request.Method} {context.Request.Path}");
            _logger.LogInformation($"Żądanie: {context.Request.Method} {context.Request.Path}");
            await _next(context);
        }

    }
}
