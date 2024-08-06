using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Boats;

public sealed partial class InventBoatHandler(ILogger<InventBoatHandler> logger) : IRequestHandler<InventBoat, Result<Boat>>
{
    ValueTask<Result<Boat>> IRequestHandler<InventBoat, Result<Boat>>.Handle(InventBoat request, CancellationToken cancellationToken)
    {
        var boat = new Boat(request.BoatName);
        LogBoatInvented(logger, boat.Name, boat.Created);

        return ValueTask.FromResult(new Result<Boat>(boat));
    }

    [LoggerMessage(1, LogLevel.Information, "Boat {BoatName} was reimagined on {Created}", SkipEnabledCheck = true)]
    private static partial void LogBoatInvented(ILogger logger, string boatName, DateTime created);
}
