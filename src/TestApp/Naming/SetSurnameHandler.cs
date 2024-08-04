using Mediator;
using TestApp.Repositories;

namespace TestApp.Naming;

public class SetSurnameHandler : IRequestHandler<SetSurname, string>
{
    public async ValueTask<string> Handle(SetSurname request, CancellationToken cancellationToken)
    {
        var SomeOddity = new
        {
            Id = Guid.NewGuid(),
            Surname = request.Surname,
            Created = DateTime.UtcNow
        };
        var writer = new DiskWriter();
        await writer.Write(SomeOddity, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), cancellationToken);
        return request.Surname;
    }
}