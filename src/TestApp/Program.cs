// See https://aka.ms/new-console-template for more information
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TestApp.Models;
using TestApp.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
            .Build()
            .Services.GetRequiredService<TestAppRunner>()
            .Run(args);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseLamar()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediator(conf =>
                {
                    conf.ServiceLifetime = ServiceLifetime.Transient;
                    conf.Namespace = "TestApp";
                });
                services.AddLamar([]);
                services.AddSingleton<TestAppRunner>();
            })
            .ConfigureContainer<ServiceRegistry>(services =>
            {
                services.Scan(scan =>
                {
                    scan.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf(typeof(Command<>));
                    scan.AddAllTypesOf(typeof(IRequestHandler<,>));
                    services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessagePipelineBehavior<,>));
                });
            })
            .UseSerilog((hostContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .Filter.ByExcluding(logEvent =>
                        logEvent.Properties.TryGetValue("SourceContext", out var source) &&
                        source.ToString().StartsWith("\"Microsoft\"") &&
                        logEvent.Level < Serilog.Events.LogEventLevel.Warning);
            });
}
