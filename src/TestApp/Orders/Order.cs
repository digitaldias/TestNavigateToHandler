using TestApp.Models;

namespace TestApp.Orders;

public sealed record Order(Guid OrderId, Guid CustomerId) : IAmEntity
{
    public DateTime Created { get; init; } = DateTime.UtcNow;
}
