using TestApp.Models;

namespace TestApp.Boats;

public record InventBoat(string BoatName) : Command<Result<Boat>>;

