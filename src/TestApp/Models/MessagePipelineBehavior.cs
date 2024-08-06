using System.Diagnostics;
using Mediator;
using Microsoft.Extensions.Logging;

namespace TestApp.Models;

public partial class MessagePipelineBehavior<TRequest, TResult>(ILogger<MessagePipelineBehavior<TRequest, TResult>> logger)
    : MessagePipelineBehavior, IPipelineBehavior<TRequest, TResult>
    where TRequest : Command<TResult>
{
    private static long _totalExecutions = 0;
    private readonly ILogger<MessagePipelineBehavior<TRequest, TResult>> _logger = logger;

    public ValueTask<TResult> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResult> next)
    {
        message.Started = Stopwatch.GetTimestamp();
        Begin(message);

        var result = next(message, cancellationToken);

        Complete(message);

        return result;
    }

    protected void Begin(ICommand message)
        => LogMessageInvocation(_logger, message.MessageType, message.Id);

    protected void Complete(ICommand message)
    {
        var elapsedTicks = Stopwatch.GetTimestamp() - message.Started;
        var elapsedMs = (elapsedTicks * 1000.0) / Stopwatch.Frequency;

        message.CompletionTime = elapsedMs;
        UpdateAverageExecutionTime(elapsedMs);

        LogMessageCompleted(_logger, message.MessageType, elapsedMs, message.Id);
    }

    private static void UpdateAverageExecutionTime(double newExecutionTime)
    {
        // Increment the total number of executions
        Interlocked.Increment(ref _totalExecutions);

        // Use a thread-safe way to update the average execution time
        double currentAverage, newAverage;
        do
        {
            currentAverage = AverageExecutionTime;
            newAverage = ((currentAverage * (_totalExecutions - 1)) + newExecutionTime) / _totalExecutions;
        } while (Interlocked.CompareExchange(ref AverageExecutionTime, newAverage, currentAverage) != currentAverage);
    }

    [LoggerMessage(1, LogLevel.Information, "Received {MessageType}:{MessageId}", SkipEnabledCheck = false)]
    private static partial void LogMessageInvocation(ILogger logger, string messageType, Guid messageId);

    [LoggerMessage(2, LogLevel.Information, "{MessageType}:{MessageId} complete in {ExecutionTime}ms", SkipEnabledCheck = false)]
    private static partial void LogMessageCompleted(ILogger logger, string messageType, double executionTime, Guid messageId);
}

public class MessagePipelineBehavior
{
    public static double AverageExecutionTime = 0;
}
