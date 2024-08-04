using TestApp.Models;

namespace TestApp.Boats;

public record CreateBoat(string BoatName) : Command<Result<Boat>>;