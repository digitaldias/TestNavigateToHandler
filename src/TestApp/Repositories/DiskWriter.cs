using System.Text.Json;

namespace TestApp.Repositories;

public class DiskWriter
{
    public async Task<bool> Write<T>(T data, string path, CancellationToken cancellationToken)
    {
        // Not really writing data to disk
        var dataString = JsonSerializer.Serialize(data);
        await Task.Delay(1, cancellationToken);
        return true;
    }
}


public class SomeWeirdClass
{

}
