using Mediator;

namespace TestApp.Naming;

public sealed record SetSurname(string Surname) : IRequest<string>;
