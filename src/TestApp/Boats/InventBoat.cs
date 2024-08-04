using Mediator;
using TestApp.Models;

namespace TestApp.Boats;

public sealed record InventBoat(string BoatName) : IRequest<Result<Boat>>;

