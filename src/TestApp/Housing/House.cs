using TestApp.Models;

namespace TestApp.Housing;

public record House : IAmEntity
{
    public string Name { get; set; } = string.Empty;
}

