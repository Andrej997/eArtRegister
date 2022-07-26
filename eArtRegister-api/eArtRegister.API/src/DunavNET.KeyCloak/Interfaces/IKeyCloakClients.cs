using KeyCloak.Models;
using RestSharp;
using System.Collections.Generic;

namespace KeyCloak.Interfaces
{
    public interface IKeyCloakClients
    {
        IEnumerable<Client> GetClients();

        IEnumerable<Role> GetClientRoles(string clientId);

        IEnumerable<Role> GetUserClientRoles(string userId, string clientId);

        string UpdateUserClientRoles(string userId, string clientId, List<Role> roles, Method method);
    }
}
