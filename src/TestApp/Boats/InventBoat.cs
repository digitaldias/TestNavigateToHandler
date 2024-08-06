using TestApp.Models;

namespace TestApp.Boats;

public sealed record InventBoat(string BoatName) : Command<Result<Boat>>;

