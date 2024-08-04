// See https://aka.ms/new-console-template for more information
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TestApp.Models;
using TestApp.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var appRunner = host.Services.GetRequiredService<TestAppRunner>();
        await appRunner.Run();

        Log.CloseAndFlush();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseLamar()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMediator(cfg =>
                {
                    cfg.ServiceLifetime = ServiceLifetime.Transient;
                    cfg.Namespace = "TestApp";
                });
                services.AddLamar();
            })
            .ConfigureContainer<ServiceRegistry>(services =>
            {
                services.Scan(scan =>
                {
                    scan.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    scan.WithDefaultConventions();
                    services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(MessagePipelineBehavior<,>));
                });
                services.Use<TestAppRunner>().Transient();
            })
            .UseSerilog((hostContext, loggerConfiguration) =>
            {
                // Switch to Debug() to see more information

                loggerConfiguration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Async(cfg => cfg.Console(theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true))
                    .Enrich.FromLogContext()
                    .Filter.ByExcluding(logEvent => logEvent.Properties.TryGetValue("SourceContext", out var source) && source.ToString().StartsWith("\"Microsoft\"") && logEvent.Level < Serilog.Events.LogEventLevel.Warning);
            });
}
