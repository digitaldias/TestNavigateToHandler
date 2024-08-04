using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Boats;

public sealed partial class ProcessBoatHandler(ILogger<ProcessBoatHandler> logger) : IRequestHandler<ProcessBoat, Result<bool>>
{
    public ValueTask<Result<bool>> Handle(ProcessBoat request, CancellationToken cancellationToken)
    {
        if (request.TheBoat.Name.StartsWith('W'))
        {
            LogBoatOnDryLand(logger, request.MessageType, request.TheBoat.Name);
            var result = new Result<bool>(new Problem("This boat is on dry land waiting to be scrapped"));
            return ValueTask.FromResult(result);
        }

        return ValueTask.FromResult(new Result<bool>(true));
    }

    [LoggerMessage(1, LogLevel.Warning, "{Command}: Boat {BoatName} is on dry land waiting to be scrapped", SkipEnabledCheck = true)]
    private static partial void LogBoatOnDryLand(ILogger logger, string command, string boatName);
}
