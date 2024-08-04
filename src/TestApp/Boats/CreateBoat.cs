using TestApp.Models;

namespace TestApp.Boats;

public sealed record CreateBoat(string BoatName) : Command<Result<Boat>>;