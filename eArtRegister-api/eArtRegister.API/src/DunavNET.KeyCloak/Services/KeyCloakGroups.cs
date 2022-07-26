using KeyCloak.Common;
using KeyCloak.Interfaces;
using KeyCloak.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyCloak.Services
{
    public class KeyCloakGroups : IKeyCloakGroups
    {
        private readonly KeyCloakConfig config;

        public KeyCloakGroups(IOptions<KeyCloakConfig> settings)
        {
            config = settings.Value;
        }

        public void CreateGroup(string groupName)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/groups");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var stringJSON = String.Format("{{\"name\": \"{0}\"}}", groupName);
            request.AddParameter("application/json", stringJSON, ParameterType.RequestBody);

            client.Execute(request);
        }

        public IEnumerable<GroupRepresentation> GetGroups(string searchQuery = null)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/groups" + (String.IsNullOrWhiteSpace(searchQuery) ? "" : ("?search=" + searchQuery)));
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<GroupRepresentation>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public GroupRepresentation GetGroup(string groupId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/groups/{groupId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<GroupRepresentation>.ConvertToEntity(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public void UpdateGroup(string groupId, GroupRepresentation group)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/groups/{groupId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var stringJSON = String.Format("{{\"name\": \"{0}\"}}", group.Name);
            request.AddParameter("application/json", stringJSON, ParameterType.RequestBody);

            client.Execute(request);
        }

        public void DeleteGroup(string groupId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/groups/{groupId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            client.Execute(request);
        }

        public IEnumerable<GroupMember> GetGroupMembers(string groupId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/groups/{groupId}/members");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<GroupMember>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public void AddUserToGroup(string userId, string groupId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/groups/{groupId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            client.Execute(request);
        }

        public void RemoveUserFromGroup(string userId, string groupId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/groups/{groupId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            client.Execute(request);
        }
    }
}
