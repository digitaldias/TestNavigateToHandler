using Mediator;
using TestApp.Repositories;

namespace TestApp.Naming;

public class SetNameHandler : IRequestHandler<SetName, string>
{
    public async ValueTask<string> Handle(SetName request, CancellationToken cancellationToken)
    {
        var SomeOddity = new
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Created = DateTime.UtcNow
        };
        var writer = new DiskWriter();
        await writer.Write(SomeOddity, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), cancellationToken);
        return request.Name;
    }
}
