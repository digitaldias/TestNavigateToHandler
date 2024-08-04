using TestApp.Models;

namespace TestApp.Boats;

public sealed class Boat(string boatName) : IAmEntity
{
    public string Name { get; init; } = boatName;
    public DateTime Created { get; init; } = DateTime.UtcNow;
}
