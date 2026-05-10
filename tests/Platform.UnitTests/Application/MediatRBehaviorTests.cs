using MediatR;
using Microsoft.Extensions.Logging;
using Platform.Application.Common.Behaviors;

namespace Platform.UnitTests.Application;

public sealed class MediatRBehaviorTests
{
    [Fact]
    public async Task RequestLoggingBehaviorShouldCallNextHandler()
    {
        var logger = new TestLogger<RequestLoggingBehavior<TestRequest, string>>();
        var behavior = new RequestLoggingBehavior<TestRequest, string>(logger);

        var response = await behavior.Handle(
            new TestRequest(),
            _ => Task.FromResult("handled"),
            CancellationToken.None);

        Assert.Equal("handled", response);
        Assert.Equal(2, logger.Logs.Count(log => log.Level == LogLevel.Information));
    }

    [Fact]
    public async Task UnhandledExceptionBehaviorShouldLogAndRethrow()
    {
        var logger = new TestLogger<UnhandledExceptionBehavior<TestRequest, string>>();
        var behavior = new UnhandledExceptionBehavior<TestRequest, string>(logger);
        var exception = new InvalidOperationException("boom");

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            behavior.Handle(
                new TestRequest(),
                _ => Task.FromException<string>(exception),
                CancellationToken.None));

        Assert.Same(exception, thrown);
        Assert.Contains(logger.Logs, log => log.Level == LogLevel.Error);
    }

    private sealed record TestRequest : IRequest<string>;

    private sealed class TestLogger<T> : ILogger<T>
    {
        public List<LogEntry> Logs { get; } = [];

        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Logs.Add(new LogEntry(logLevel, exception));
        }
    }

    private sealed record LogEntry(LogLevel Level, Exception? Exception);

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
