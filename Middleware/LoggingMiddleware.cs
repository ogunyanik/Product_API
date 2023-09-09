namespace Product_API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log request details
        _logger.LogInformation($"Received {context.Request.Method} request for {context.Request.Path}");

        // Capture the response body
        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            // Log response details
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = new StreamReader(responseBody).ReadToEnd();
            _logger.LogInformation($"Response: {responseText}");

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}