using KeyCloak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyCloak.Interfaces
{
    public interface IKeyCloakGroups
    {
        void CreateGroup(string groupName);
        IEnumerable<GroupRepresentation> GetGroups(string searchQuery = null);
        GroupRepresentation GetGroup(string groupId);
        void UpdateGroup(string groupId, GroupRepresentation group);
        void DeleteGroup(string groupId);
        IEnumerable<GroupMember> GetGroupMembers(string groupId);
        void AddUserToGroup(string userId, string groupId);
        void RemoveUserFromGroup(string userId, string groupId);
    }
}
