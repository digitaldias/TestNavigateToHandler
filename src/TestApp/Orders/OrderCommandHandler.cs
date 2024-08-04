using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Orders;

public sealed class CreateCommandHandler(ILogger<CreateCommandHandler> logger) : IRequestHandler<CreateOrder, Result<Order>>
{
    private readonly ILogger<CreateCommandHandler> _logger = logger;

    public ValueTask<Result<Order>> Handle(CreateOrder request, CancellationToken cancellationToken)
    {
        var order = new Order(request.Id, request.CustomerId);

        _logger.LogDebug("Order {OrderId} created on {Created}", order.OrderId, order.Created.ToString("dd.MMMM.yyyy HH:mm:ss"));
        return ValueTask.FromResult(new Result<Order>(order));
    }
}
