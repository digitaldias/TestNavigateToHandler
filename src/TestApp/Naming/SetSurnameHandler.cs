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
        await DiskWriter.Write(SomeOddity, cancellationToken);
        return request.Surname;
    }
}