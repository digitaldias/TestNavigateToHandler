using FizzWare.NBuilder;
using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Boats;
using TestApp.Customers;
using TestApp.Models;
using TestApp.Orders;

namespace TestApp.Services;

public sealed class TestAppRunner(IMediator mediator, ILogger<TestAppRunner> logger)
{
    private static readonly Random _random = new((int)DateTime.Now.Ticks);

    private readonly IMediator _mediator = mediator;
    private readonly ILogger<TestAppRunner> _logger = logger;

    public async Task Run()
    {
        var commands = new List<Models.ICommand>();

        commands.AddRange(Builder<CreateBoat>.CreateListOfSize(50).All().WithFactory(() => new CreateBoat(Faker.Address.City())).Build());
        commands.AddRange(Builder<CreateCustomer>.CreateListOfSize(85).All().WithFactory(() => new CreateCustomer(Faker.Name.Last())).Build());
        commands.AddRange(Builder<InventBoat>.CreateListOfSize(100).All().WithFactory(() => new InventBoat(Faker.Name.First())).Build());
        commands.AddRange(Builder<CreateOrder>.CreateListOfSize(50).All().WithFactory(() => new CreateOrder(Guid.NewGuid(), Guid.NewGuid())).Build());

        Shuffle(commands);
        var tasks = commands.ConvertAll(SendMessageAsync);

        try
        {
            _logger.LogInformation("Awaiting execution of {CommandCount} tasks...", tasks.Count);
            var results = await Task.WhenAll(tasks);

            var averageExecutionTimes = results
                .GroupBy(result => result.Key)
                .Select(group => new
                {
                    CommandType = group.Key,
                    InvocationCount = group.Count(),
                    AverageExecutionTime = group.Average(result => result.Value.time),
                    PercentageFailed = ((double)group.Count(result => !result.Value.succeeded) / group.Count()) * 100
                });

            _logger.LogInformation("{Divider}", new string('-', 80));

            foreach (var avg in averageExecutionTimes)
            {
                _logger.LogInformation("{CommandType} executed {InvocationCount} times with avg: {AverageExecutionTime:0.00}ms. Percentage failed: {PercentageFailed:0.00}%",
                    avg.CommandType,
                    avg.InvocationCount,
                    avg.AverageExecutionTime,
                    avg.PercentageFailed);
            }

            _logger.LogInformation("{Divider}", new string('-', 80));

            _logger.LogInformation("Average execution time overall: {AverageExecutionTime:0.00}ms", MessagePipelineBehaviorBase.AverageExecutionTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while awaiting tasks.");
        }
    }

    private async Task<KeyValuePair<string, (double time, bool succeeded)>> SendMessageAsync(Models.ICommand command)
    {
        try
        {
            var sendResult = await _mediator.Send(command);
            if (sendResult is Result result)
            {
                return new KeyValuePair<string, (double, bool)>(command.MessageType, (command.CompletionTime, result.IsSuccess));
            }
        }
        catch (Exception)
        {
            _logger.LogError("An error occurred while sending the command.");
        }
        return new KeyValuePair<string, (double, bool)>(command.MessageType, (command.CompletionTime, false));
    }

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }
}
