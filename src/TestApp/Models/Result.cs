namespace TestApp.Models;

public class Result
{
    public bool IsSuccess => Problem is null;

    public bool IsFailure => !IsSuccess;

    public Problem? Problem { get; internal set; }

    public Result()
    {
    }

    public Result(Problem problem)
    {
        Problem = problem ?? throw new ArgumentNullException(nameof(problem));
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(Problem problem)
        : base(problem)
    {
    }

    public Result(TValue? value, string? errorMessage = null)
    {
        _value = value;
        if (value is null)
        {
            base.Problem = new(errorMessage ?? "Value cannot be null");
        }
    }

    public TValue Value => _value ?? default!;
}

