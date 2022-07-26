using KeyCloak.Common;
using KeyCloak.Exceptions;
using KeyCloak.Interfaces;
using KeyCloak.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KeyCloak.Services
{
    public class KeyCloakUsers : IKeyCloakUsers
    {
        private readonly KeyCloakConfig config;
        
        public KeyCloakUsers(IOptions<KeyCloakConfig> settings)
        {
            config = settings.Value;
        }

        public string UpdatePassword()
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/roles");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public void VerifyEmail(string userId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/send-verify-email");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
        }

        public string CheckUserSessions(string userId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/sessions");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public void RemoveAllUserSessions(string userId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/logout");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");
            client.Execute(request);
        }

        public IEnumerable<User> GetUsers(bool? enabled = null, int? first = 0, int? max = 100)
        {
            var token = CommonService.GetToken(config);
            RestClient client = new RestClient();
            bool enabledParam = enabled != null ? enabled.Value : true;
            if (enabled != null)
                client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users?briefRepresentation=false&enabled={enabledParam}&first={first}&max={max}");
            else
                client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users?briefRepresentation=false&first={first}&max={max}");

            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<User>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public IEnumerable<User> GetUsers()
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<User>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public bool CheckUsername(string username)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users?exact=true&username={username}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.Content == "[]")
                return false;
            else
                return true;
        }

        public bool CheckEmail(string email)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users?exact=true&email={email}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.Content == "[]")
                return false;
            else
                return true;
        }

        public User GetUserByUsername(string username)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users?exact=true&username={username}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.Content == "[]")
                return null;
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<User>.ConvertToEntity(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public User GetUserByEmail(string email)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users?exact=true&email={email}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.Content == "[]")
                return null;
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<User>.ConvertToEntity(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public int GetUsersCount()
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/count");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            return int.Parse(response.Content);
        }

        public User GetUser(string userId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<User>.ConvertToEntity(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception( "Keycloak: " + (jObject["error"]).ToString());
            }

        }

        public IEnumerable<User> GetUserWithClientRole(string clientId, string roleName)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/clients/{clientId}/roles/{roleName}/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<User>.ConvertToEntities(response.Content);
            else
                throw new Exception();
        }

        public string CreateUser(User user)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var stringJSON = String.Format("{{\"enabled\": {0},\"email\": \"{1}\",\"emailVerified\": {2},\"firstName\": \"{3}\",\"lastName\": \"{4}\",\"username\": \"{5}\"}}",
                user.Enabled.Value.ToString().ToLower(),
                user.Email,
                user.EmailVerified.Value.ToString().ToLower(),
                user.FirstName,
                user.LastName,
                user.UserName);
            request.AddParameter("application/json", stringJSON, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
            {
                try
                {
                    throw new KeyCloakUserException("Keycloak: " + JObject.Parse(response.Content)["error"].ToString(), response.StatusCode);
                }
                catch 
                {
                    throw new KeyCloakUserException("Keycloak: " + JObject.Parse(response.Content)["errorMessage"].ToString(), response.StatusCode);
                }
            }
                

            var header = response.Headers
                .Where(x => x.Name == "Location")
                .Select(x => x.Value)
                .FirstOrDefault()
                .ToString()
                .Split('/');

            return header[header.Length - 1];
        }

        public string UpdateUser(string userId, User user)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var stringJSON = String.Format("{{\"enabled\": {0},\"email\": \"{1}\",\"emailVerified\": {2},\"firstName\": \"{3}\",\"lastName\": \"{4}\",\"username\": \"{5}\", \"id\": \"{6}\"}}",
                user.Enabled.Value.ToString().ToLower(),
                user.Email,
                user.EmailVerified.Value.ToString().ToLower(),
                user.FirstName,
                user.LastName,
                user.UserName,
                userId);
            request.AddParameter("application/json", stringJSON, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return response.Content;

            if (response.StatusCode != HttpStatusCode.OK)
                throw new KeyCloakUserException("Keycloak: " + JObject.Parse(response.Content)["errorMessage"].ToString());

            return response.Content;
        }

        public string UpdatePassword(string userId, Credentials credentials)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/reset-password");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var stringJSON = String.Format("{{\"temporary\": {0},\"value\": \"{1}\"}}",
                credentials.Temporary.Value.ToString().ToLower(),
                credentials.Value);
            request.AddParameter("application/json", stringJSON, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return response.Content;

            if ( response.StatusCode != HttpStatusCode.OK)
                throw new KeyCloakUserException(response.Content);

            return response.Content;
        }

        public IEnumerable<Role> GetAvaibleRolesForUser(string userId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/role-mappings/realm/available");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Role>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public string EnableDisableUser(string userId, bool enabled)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var stringJSON = String.Format("{{\"enabled\": {0},\"id\": \"{1}\"}}",
                enabled.ToString().ToLower(),
                userId);
            request.AddParameter("application/json", stringJSON, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return response.Content;

            if (response.StatusCode != HttpStatusCode.OK)
                throw new KeyCloakUserException(response.Content);


            return response.Content;
        }

        public IEnumerable<GroupRepresentation> GetUserGroups(string userId)
        {
            var token = CommonService.GetToken(config);
            RestClient client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/groups");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<GroupRepresentation>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }
    }
}
