using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Orders;

public sealed partial class CreateOrderHandler(ILogger<CreateOrderHandler> logger) : IRequestHandler<CreateOrder, Result<Order>>
{
    private readonly ILogger<CreateOrderHandler> _logger = logger;

    public ValueTask<Result<Order>> Handle(CreateOrder request, CancellationToken cancellationToken)
    {
        var order = new Order(request.Id, request.CustomerId);

        LogOrderCreated(_logger, order.OrderId, order.Created);
        return ValueTask.FromResult(new Result<Order>(order));
    }

    [LoggerMessage(1, LogLevel.Information, "Order {OrderId} created on {Created:dd.MMMM.yyyy HH:mm:ss}")]
    private static partial void LogOrderCreated(ILogger logger, Guid orderId, DateTime created);
}
