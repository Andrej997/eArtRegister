using eart_keycloak_microservice.Interfaces;
using eart_keycloak_microservice.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace eart_keycloak_microservice.Services
{
    public class RESTServices : IRESTServices
    {
        private readonly IRestClient _eArtHTTPClient;
        private readonly IRestClient _keyCloakHTTPClient;
        public Config _config { get; set; }
        public RESTServices(Config config)
        {
            _config = config;

            _eArtHTTPClient = new RestClient(_config.EArtUrl);
            _eArtHTTPClient.Timeout = -1;

            _keyCloakHTTPClient = new RestClient(_config.KeyCloakUrl);
            _keyCloakHTTPClient.Timeout = -1;
        }

        private string GetToken()
        {
            var request = new RestRequest($"/auth/realms/{_config.Realm}/protocol/openid-connect/token", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", _config.Client);
            request.AddParameter("client_secret", _config.ClientSecret);
            IRestResponse response = _keyCloakHTTPClient.Execute(request);

            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return JObject.Parse(response.Content)["access_token"].ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch
            {
                throw new Exception("Failed to aquire token");
            }
        }

        public async Task<IRestResponse> GetDBIds(CancellationToken cancellationToken)
        {
            var token = GetToken();
            var request = new RestRequest("/api/UserWorker/ids", Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("ApiKey", _config.ApiKey);
            return await _eArtHTTPClient.ExecuteAsync(request, cancellationToken);
        }

        public async Task<IRestResponse> GetKeyCloakIds(CancellationToken cancellationToken)
        {
            var token = GetToken();
            var request = new RestRequest($"/auth/admin/realms/{_config.Realm}/users", Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            return await _keyCloakHTTPClient.ExecuteAsync(request, cancellationToken);
        }
    }
}
