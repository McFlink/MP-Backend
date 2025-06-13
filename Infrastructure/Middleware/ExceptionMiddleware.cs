using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security;
using System.Text.Json;

namespace MP_Backend.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // kör nästa middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");

                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    ArgumentNullException => (int)HttpStatusCode.BadRequest,          // T.ex. null i viktiga parametrar
                    ArgumentException => (int)HttpStatusCode.BadRequest,              // Felaktiga värden
                    InvalidOperationException => (int)HttpStatusCode.Conflict,        // Fel i operationens kontext (t.ex. state)
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,             // Något saknas i databas/dictionary
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,  // Behörighetsproblem (ej inloggad)
                    SecurityException => (int)HttpStatusCode.Forbidden,               // Behörighet finns men otillräcklig roll
                    NotImplementedException => (int)HttpStatusCode.NotImplemented,    // Metod ännu inte byggd
                    NotSupportedException => (int)HttpStatusCode.MethodNotAllowed,    // Ex. operation stöds inte
                    TimeoutException => (int)HttpStatusCode.RequestTimeout,           // T.ex. långsam extern tjänst
                    DbUpdateException => (int)HttpStatusCode.InternalServerError,     // Fel vid databasuppdatering
                    _ => (int)HttpStatusCode.InternalServerError                      // Övrigt
                };

                context.Response.StatusCode = statusCode;

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = statusCode switch
                    {
                        400 => "Bad request – the request was invalid",
                        401 => "Unauthorized – please log in",
                        403 => "Forbidden – access denied",
                        404 => "Not found – resource missing",
                        405 => "Method not allowed",
                        408 => "Request timeout",
                        409 => "Conflict – operation could not be completed",
                        500 => "Unexpected error occurred",
                        501 => "Not implemented – feature missing",
                        _ => "Error"
                    },
                    Detail = _env.IsDevelopment() ? ex.Message : null
                };

                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
