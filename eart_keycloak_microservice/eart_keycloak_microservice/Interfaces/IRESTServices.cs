using RestSharp;

namespace eart_keycloak_microservice.Interfaces
{
    public interface IRESTServices
    {
        Task<IRestResponse> GetDBIds(string bearer, CancellationToken cancellationToken);

        Task<IRestResponse> GetKeyCloakIds(string bearer, CancellationToken cancellationToken);
    }
}
