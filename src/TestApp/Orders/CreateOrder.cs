using TestApp.Models;

namespace TestApp.Orders;

public sealed record CreateOrder(Guid OrderId, Guid CustomerId) : Command<Result<Order>>;
