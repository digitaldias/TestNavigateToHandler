using TestApp.Models;

namespace TestApp.Boats;

public sealed record ProcessBoat(Boat TheBoat) : Command<Result<bool>>
{
    public Boat Input { get; } = TheBoat;
}
