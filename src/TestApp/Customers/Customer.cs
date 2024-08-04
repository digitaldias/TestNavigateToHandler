using TestApp.Models;

namespace TestApp.Customers;

public sealed record Customer(string CustomerName) : IAmEntity
{
    public DateTime Created { get; init; } = DateTime.UtcNow;
}
