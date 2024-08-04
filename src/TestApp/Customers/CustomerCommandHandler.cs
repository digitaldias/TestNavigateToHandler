using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Customers;

public sealed class CreateCustomerHandler(ILogger<CreateCustomer> logger) : IRequestHandler<CreateCustomer, Result<Customer>>
{
    private readonly ILogger<CreateCustomer> _logger = logger;

    public ValueTask<Result<Customer>> Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.CustomerName);
        _logger.LogInformation("Customer {CustomerName} created on {Created}", customer.CustomerName, customer.Created.ToString("dd.MMMM.yyyy HH:mm:ss"));
        return ValueTask.FromResult(new Result<Customer>(customer));
    }
}
