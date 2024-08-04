using TestApp.Models;

namespace TestApp.Housing;

public sealed record House : IAmEntity
{
    public string Name { get; set; } = string.Empty;
}

