using KeyCloak.Models;
using RestSharp;
using System.Collections.Generic;

namespace KeyCloak.Interfaces
{
    public interface IKeyCloakRoles
    {
        IEnumerable<Role> GetServices();

        IEnumerable<Role> GetClientRoles(string clientId);

        IEnumerable<Role> GetUserClientRoles(string userId, string clientId);

        Role GetClientRoleByNema(string clientRoleName, string clientId);

        Role GetServiceById(string serviceId);

        Role GetServiceByNema(string serviceName);

        IEnumerable<Role> GetUserServices(string userId);

        string PostUserServices(string userId, IEnumerable<Role> roles, Method method);

        string UpdateService(string serviceName, string jsonService);

        IEnumerable<User> GetUsersWithServiceByNema(string serviceName);

        string UpdateUserClientRoles(string userId, string clientId, List<Role> roles, Method method);


    }
}
