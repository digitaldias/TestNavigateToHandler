using Mediator;
using TestApp.Repositories;

namespace TestApp.Naming;

public class SetNameHandler : IRequestHandler<SetName, string>
{
    public async ValueTask<string> Handle(SetName request, CancellationToken cancellationToken)
    {
        var SomeOddity = new
        {
            request.Name,
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow
        };
        var writer = new DiskWriter();
        await DiskWriter.Write(SomeOddity, cancellationToken);
        return request.Name;
    }
}
