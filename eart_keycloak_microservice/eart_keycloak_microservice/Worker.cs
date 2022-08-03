using eart_keycloak_microservice.Interfaces;
using eart_keycloak_microservice.Models;
using Newtonsoft.Json;

namespace eart_keycloak_microservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRESTServices _rest;

        public Worker(ILogger<Worker> logger, IRESTServices rest)
        {
            _logger = logger;
            _rest = rest;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var dbIds = JsonConvert.DeserializeObject<List<Guid>>((await _rest.GetDBIds(cancellationToken)).Content);
                var keyCloakIds = JsonConvert.DeserializeObject<List<UserKeyCloak>>((await _rest.GetKeyCloakIds(cancellationToken)).Content).Select(x => x.Id).ToList();

                var newUsers = keyCloakIds.Except(dbIds).ToList();

                if (newUsers != null && newUsers.Any())
                {
                    await _rest.RegisterUser(new RegisterUser(newUsers), cancellationToken);
                    _logger.LogInformation($"New users are added as observers: {String.Join(",", newUsers)}");
                }
                

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, cancellationToken);
            }
        }
    }
}