using KeyCloak.Models;
using System.Collections.Generic;

namespace KeyCloak.Interfaces
{
    public interface IKeyCloakUsers
    {
        string UpdatePassword();

        void VerifyEmail(string userId);

        string CheckUserSessions(string userId);

        void RemoveAllUserSessions(string userId);

        IEnumerable<User> GetUsers(bool? enabled = null, int? first = 0, int? max = 100);

        IEnumerable<User> GetUsers();

        User GetUserByUsername(string username);

        User GetUserByEmail(string email);

        int GetUsersCount();

        User GetUser(string userId);

        IEnumerable<User> GetUserWithClientRole(string clientId, string roleName);

        string CreateUser(User user);

        string UpdateUser(string userId, User user);

        string UpdatePassword(string userId, Credentials credentials);

        IEnumerable<Role> GetAvaibleRolesForUser(string userId);

        string EnableDisableUser(string userId, bool enabled);

        bool CheckUsername(string username);

        bool CheckEmail(string email);
        IEnumerable<GroupRepresentation> GetUserGroups(string userId);
    }
}
