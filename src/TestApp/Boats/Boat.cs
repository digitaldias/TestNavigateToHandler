using TestApp.Models;

namespace TestApp.Boats;

public sealed class Boat(string boatName, Guid? boatId = null) : IAmEntity
{
    public Guid Id { get; init; } = boatId ?? Guid.NewGuid();

    public string Name { get; init; } = boatName;

    public DateTime Created { get; init; } = DateTime.UtcNow;
}
