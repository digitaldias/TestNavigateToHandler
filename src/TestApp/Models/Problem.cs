namespace TestApp.Models;

public sealed class Problem(string message)
{
    public string Message { get; } = message;
}

