using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Platform.Application.Common.Behaviors;

public sealed class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Handling application request {RequestName}.", requestName);

        var response = await next();

        stopwatch.Stop();
        _logger.LogInformation(
            "Handled application request {RequestName} in {ElapsedMilliseconds} ms.",
            requestName,
            stopwatch.ElapsedMilliseconds);

        return response;
    }
}
