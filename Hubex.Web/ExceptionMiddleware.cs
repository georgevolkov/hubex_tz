using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Hubex.Web;

public class ExceptionMiddleware
{
    private readonly RequestDelegate              _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    private static readonly JsonSerializerOptions Options =
        new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            await HandleExceptionAsync (httpContext, HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode httpStatusCode, string message)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)httpStatusCode;

        var response = new
        {
            title  = message,
            status = context.Response.StatusCode
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, Options));
    }
}