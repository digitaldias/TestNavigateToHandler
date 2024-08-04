using TestApp.Models;

namespace TestApp.Customers;

public sealed record CreateCustomer(string customerName) : Command<Result<Customer>>
{
    public string CustomerName { get; } = customerName;
}
