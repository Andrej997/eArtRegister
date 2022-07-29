using eart_keycloak_microservice.Interfaces;
using eart_keycloak_microservice.Models;
using Microsoft.Extensions.Options;
using RestSharp;

namespace eart_keycloak_microservice.Services
{
    public class RESTServices : IRESTServices
    {
        private readonly IRestClient _eArtHTTPClient;
        private readonly IRestClient _keyCloakHTTPClient;
        private string eArtAPI { get; set; }
        private string keyCloakAPI { get; set; }
        public RESTServices(Config config)
        {
            eArtAPI = config.EArtUrl;
            _eArtHTTPClient = new RestClient(eArtAPI);
            _eArtHTTPClient.Timeout = -1;

            keyCloakAPI = config.KeyCloakUrl;
            _keyCloakHTTPClient = new RestClient(keyCloakAPI);
            _keyCloakHTTPClient.Timeout = -1;
        }

        public async Task<IRestResponse> GetDBIds(string bearer, CancellationToken cancellationToken)
        {
            return new RestResponse();
            //var request = new RestRequest("/api/Users/ids", Method.GET);
            //request.AddHeader("Authorization", $"Bearer {bearer}");
            //request.AddHeader("Content-Type", "application/json");
            //return await _eArtHTTPClient.ExecuteAsync(request, cancellationToken);
        }

        public async Task<IRestResponse> GetKeyCloakIds(string bearer, CancellationToken cancellationToken)
        {
            return new RestResponse();
            //var request = new RestRequest("/", Method.GET);
            //request.AddHeader("Authorization", $"Bearer {bearer}");
            //request.AddHeader("Content-Type", "application/json");
            //return await _keyCloakHTTPClient.ExecuteAsync(request, cancellationToken);
        }
    }
}
