namespace TestApp.Models;

public class Result
{
    public bool IsSuccess => Problem is null;

    public bool IsFailure => !IsSuccess;

    public Problem? Problem { get; }

    public Result()
    {
    }

    public Result(Problem? problem = null)
    {
        Problem = problem;
    }
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(Problem problem)
        : base(problem)
    {
    }

    public Result(TValue? value)
    {
        _value = value;
    }

    public TValue Value => _value ?? default!;
}

