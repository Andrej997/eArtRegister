using RestSharp;

namespace eart_keycloak_microservice.Interfaces
{
    public interface IRESTServices
    {
        Task<IRestResponse> GetDBIds(CancellationToken cancellationToken);

        Task<IRestResponse> GetKeyCloakIds(CancellationToken cancellationToken);

        Task<IRestResponse> RegisterUser(object body, CancellationToken cancellationToken);
    }
}
