using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Boats;

public partial class CreateBoatHandler(ILogger<CreateBoatHandler> logger, IMediator mediator) : IRequestHandler<CreateBoat, Result<Boat>>
{
    private readonly IMediator _mediator = mediator;

    async ValueTask<Result<Boat>> IRequestHandler<CreateBoat, Result<Boat>>.Handle(CreateBoat request, CancellationToken cancellationToken)
    {
        var boat = new Boat(request.BoatName);
        LogBoatCreated(logger, request.MessageType, boat.Name, boat.Created);

        var processResult = await _mediator.Send(new ProcessBoat(boat), cancellationToken);
        if (processResult.IsFailure)
        {
            LogBoatNotProcessed(logger, request.MessageType, boat.Name);
            return new Result<Boat>(processResult.Problem!);
        }

        return new Result<Boat>(boat);
    }

    [LoggerMessage(1, LogLevel.Information, "{Command}: Boat {BoatName} created on {Created}", SkipEnabledCheck = true)]
    private static partial void LogBoatCreated(ILogger logger, string command, string boatName, DateTime created);

    [LoggerMessage(2, LogLevel.Error, "{Command}: Boat {BoatName} could not be processed", SkipEnabledCheck = true)]
    private static partial void LogBoatNotProcessed(ILogger logger, string command, string boatName);
}
