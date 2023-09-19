using System.Text;

namespace TaskManagementAPI.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger; 


    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger; 
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the start time of the request
        var startTime = DateTime.Now;

        // Call the next middleware in the pipeline
        await _next(context);

        // Calculate the request processing time
        var processingTime = DateTime.Now - startTime;

        // Log the request details in a single line
        LogRequest(context.Request, context.Response, processingTime);
    }

    private void LogRequest(HttpRequest request, HttpResponse response, TimeSpan processingTime)
    {
        var logMessage = new StringBuilder();
        logMessage.Append($"{request.Method} {request.Path} ");
        logMessage.Append($"Status Code: {response.StatusCode} ");
        logMessage.Append($"Processing Time: {processingTime.TotalMilliseconds} ms");

        // Log the message (you can replace this with your preferred logging mechanism)
        _logger.LogInformation(logMessage.ToString());
    }
}