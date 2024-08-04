using Mediator;

namespace TestApp.Models;

public interface ICommand
{
    Guid Id { get; }

    string MessageType { get; }

    long Started { get; }

    public double CompletionTime { get; set; }
}

public interface ICommand<TResult> : ICommand, IRequest<TResult>;
