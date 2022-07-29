using eart_keycloak_microservice.Interfaces;

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
                var dbIds = await _rest.GetDBIds("", cancellationToken);
                var keyCloakIds = await _rest.GetKeyCloakIds("", cancellationToken);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}