using System.Text;
using System.Text.Json;
using Mediator;
using Microsoft.Extensions.Logging;
using TestApp.Models;

namespace TestApp.Boats;

public sealed partial class ProcessBoatHandler(ILogger<ProcessBoatHandler> logger, IHttpClientFactory clientFactory) : IRequestHandler<ProcessBoat, Result<bool>>
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient();

    public async ValueTask<Result<bool>> Handle(ProcessBoat request, CancellationToken cancellationToken)
    {
        if (request.TheBoat.Name.StartsWith('F'))
        {
            LogBoatOnDryLand(logger, request.MessageType, request.TheBoat.Name);
            var result = new Result<bool>(new Problem("This boat is on dry land waiting to be scrapped"));
            return result;
        }

        var boatJson = JsonSerializer.Serialize(request.TheBoat);

        // Test it at https://testnavigatetohandler.requestcatcher.com
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://testnavigatetohandler.requestcatcher.com/test");
        httpRequest.Headers.Add("Command", request.MessageType);
        httpRequest.Headers.Add("BoatId", request.TheBoat.Id.ToString());
        httpRequest.Content = new StringContent(boatJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return new Result<bool>(true);
        }
        else
        {
            var problem = new Problem("Failed to send the boat");
            return new Result<bool>(problem);
        }
    }

    [LoggerMessage(1, LogLevel.Warning, "{Command}: Boat {BoatName} is on dry land waiting to be scrapped", SkipEnabledCheck = true)]
    private static partial void LogBoatOnDryLand(ILogger logger, string command, string boatName);
}
