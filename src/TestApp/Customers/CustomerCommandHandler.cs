using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Customers;

public sealed partial class CreateCustomerHandler(ILogger<CreateCustomer> logger) : IRequestHandler<CreateCustomer, Result<Customer>>
{
    private readonly ILogger<CreateCustomer> _logger = logger;

    public ValueTask<Result<Customer>> Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.CustomerName);
        LogCustomerCreated(_logger, customer.CustomerName, customer.Created);
        return ValueTask.FromResult(new Result<Customer>(customer));
    }

    [LoggerMessage(1, LogLevel.Information, "Customer {CustomerName} created on {Created:dd.MMMM.yyyy HH:mm:ss}")]
    private static partial void LogCustomerCreated(ILogger logger, string customerName, DateTime created);

}
