using TestApp.Models;

namespace TestApp.Customers;

public record CreateCustomer(string customerName) : Command<Result<Customer>>
{
    public string CustomerName { get; } = customerName;
}
