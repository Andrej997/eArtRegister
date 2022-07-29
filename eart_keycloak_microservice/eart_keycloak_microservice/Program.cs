using eart_keycloak_microservice;
using eart_keycloak_microservice.Interfaces;
using eart_keycloak_microservice.Models;
using eart_keycloak_microservice.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        Config config = configuration.GetSection("Config").Get<Config>();
        services.AddSingleton(config);
        services.AddTransient<IRESTServices, RESTServices>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
