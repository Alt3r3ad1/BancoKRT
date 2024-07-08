using BancoKRT.API.Domain.ViewModels;

namespace BancoKRT.API.Middlewares;
public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpException ex)
        {
            var traceId = Guid.NewGuid();
            _logger.LogError($"Error occure while processing the request, TraceId : ${traceId}," +
                $" Message : ${ex.Message}, StackTrace: ${ex.StackTrace}");

            context.Response.StatusCode = ex.StatusCode;

            var problemDetails = new ExceptionViewModel
            {
                message = ex.Message,
                traceId = traceId.ToString()
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}