using TestApp.Models;

namespace TestApp.Orders;

public record CreateOrder(Guid OrderId, Guid CustomerId) : Command<Result<Order>>;
