using Mediator;

namespace TestApp.Naming;

public sealed record SetName(string Name) : IRequest<string>;
