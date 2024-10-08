﻿// See https://aka.ms/new-console-template for more information
using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TestApp.Models;
using TestApp.Services;

namespace TestApp;

internal static class Program
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
            .ConfigureServices((_, services) =>
            {
                services.AddMediator(cfg =>
                {
                    cfg.ServiceLifetime = ServiceLifetime.Transient;
                    cfg.Namespace = "TestApp";
                });
                services.AddLamar();
                services.AddHttpClient();
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
            .UseSerilog((_, loggerConfiguration) =>
            {
                loggerConfiguration
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Override("AspNetCore", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Override("System.Net.Http.HttpClient", Serilog.Events.LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Async(cfg => cfg.Console(theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true));
            });
}
