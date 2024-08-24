using System.Text.Json;

namespace TestApp.Repositories;

public class DiskWriter
{
    public static async Task<bool> Write<T>(T data, CancellationToken cancellationToken)
    {
        // Not really writing data to disk
        var dataString = JsonSerializer.Serialize(data);
        await Task.Delay(1, cancellationToken);
        return true;
    }
}
