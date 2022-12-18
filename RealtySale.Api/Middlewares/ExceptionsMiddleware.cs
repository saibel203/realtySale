using System.Net;
using RealtySale.Shared.Errors;

namespace RealtySale.Api.Middlewares;

public class ExceptionsMiddleware
{
    private readonly ILogger<ExceptionsMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private readonly RequestDelegate _next;

    public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            HttpStatusCode statusCode;
            string message;

            var exceptionType = ex.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                statusCode = HttpStatusCode.Forbidden;
                message = "You are not authorized";
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = "Some unknown error occurred";
            }
            
            ApiError response = _env.IsDevelopment() ? new((int)statusCode, ex.Message, ex.StackTrace) 
                : new((int)statusCode, message);

            _logger.LogError(ex, "Error: {Error}", ex.Message);
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response.ToString());
        }   
    }
}
