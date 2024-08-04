using System.Diagnostics;

namespace TestApp.Models;

public abstract record class Command<TResult> : ICommand, ICommand<TResult>
{
    public Guid Id => Guid.NewGuid();

    public long Started { get; set; } = Stopwatch.GetTimestamp();

    public double CompletionTime { get; set; } = 0;

    public string MessageType
    {
        get
        {
            var thisType = GetType();
            var baseType = thisType.BaseType;

            // Check if the base type is a generic type and its definition is Command<>
            if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Command<>))
            {
                return thisType.Name.TrimEnd('1', '`'); // To remove any generic suffix
            }

            return typeof(TResult).Name;
        }
    }
}