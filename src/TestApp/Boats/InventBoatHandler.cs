using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Boats;

public sealed partial class InventBoatHandler(ILogger<InventBoatHandler> logger) : IRequestHandler<InventBoat, Result<Boat>>
{
    ValueTask<Result<Boat>> IRequestHandler<InventBoat, Result<Boat>>.Handle(InventBoat request, CancellationToken cancellationToken)
    {
        if (request.BoatName == "Silverspring")
            throw new ArgumentException("This is not a legal name for a boat", nameof(request.BoatName));

        var boat = new Boat(request.BoatName);
        LogBoatCreated(logger, boat.Name, boat.Created);

        return ValueTask.FromResult(new Result<Boat>(boat));
    }

    [LoggerMessage(1, LogLevel.Information, "Boat {BoatName} created on {Created}", SkipEnabledCheck = true)]
    private static partial void LogBoatCreated(ILogger logger, string boatName, DateTime created);

}
