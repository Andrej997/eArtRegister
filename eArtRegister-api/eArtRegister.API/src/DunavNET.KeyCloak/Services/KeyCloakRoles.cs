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
using System.Text;

namespace KeyCloak.Services
{
    public class KeyCloakRoles : IKeyCloakRoles
    {
        private readonly KeyCloakConfig config;

        public KeyCloakRoles(IOptions<KeyCloakConfig> settings)
        {
            config = settings.Value;
        }

        public IEnumerable<Role> GetServices()
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/roles");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
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

        public IEnumerable<Role> GetClientRoles(string clientId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/clients/{clientId}/roles");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Role>.ConvertToEntities(response.Content);
            else
                throw new Exception();
        }

        public IEnumerable<Role> GetUserClientRoles(string userId, string clientId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/role-mappings/clients/{clientId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Role>.ConvertToEntities(response.Content);
            else
                throw new Exception();
        }

        public Role GetClientRoleByNema(string clientRoleName, string clientId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/clients/{clientId}/roles/{clientRoleName}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Role>.ConvertToEntity(response.Content);
            else
                throw new Exception();
        }

        public Role GetServiceById(string serviceId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/roles-by-id/{serviceId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Role>.ConvertToEntity(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public Role GetServiceByNema(string serviceName)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/roles/{serviceName}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Role>.ConvertToEntity(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
        }

        public IEnumerable<Role> GetUserServices(string userId)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/role-mappings/realm");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
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

        public string PostUserServices(string userId, IEnumerable<Role> roles, Method method)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/role-mappings/realm");
            client.Timeout = -1;
            var request = new RestRequest(method);
            request.AddHeader("Authorization", $"Bearer {token}");

            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < roles.Count(); i++)
            {
                var roleJson = "";
                if (i < roles.Count() - 1)
                    roleJson = String.Format("{{\"id\": \"{0}\",\"name\": \"{1}\"}},", roles.ElementAt(i).Id, roles.ElementAt(i).Name);
                else
                    roleJson = String.Format("{{\"id\": \"{0}\",\"name\": \"{1}\"}}", roles.ElementAt(i).Id, roles.ElementAt(i).Name);
                sb.Append(roleJson);
            }
            sb.Append(']');
            request.AddParameter("application/json", sb, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
                throw new KeyCloakUserException(response.Content);

            return response.Content;
        }

        public string UpdateService(string serviceName, string jsonService)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/roles/{serviceName}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", $"{jsonService}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if ( response.StatusCode != HttpStatusCode.OK)
                throw new KeyCloakUserException(response.Content);

            return response.Content;
        }

        public IEnumerable<User> GetUsersWithServiceByNema(string serviceName)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/roles/{serviceName}/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
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

        public string UpdateUserClientRoles(string userId, string clientId, List<Role> roles, Method method)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/users/{userId}/role-mappings/clients/{clientId}");
            client.Timeout = -1;
            var request = new RestRequest(method);
            request.AddHeader("Authorization", $"Bearer {token}");

            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < roles.Count(); i++)
            {
                var roleJson = "";
                if (i < roles.Count() - 1)
                    roleJson = String.Format("{{\"id\": \"{0}\",\"name\": \"{1}\"}},", roles.ElementAt(i).Id, roles.ElementAt(i).Name);
                else
                    roleJson = String.Format("{{\"id\": \"{0}\",\"name\": \"{1}\"}}", roles.ElementAt(i).Id, roles.ElementAt(i).Name);
                sb.Append(roleJson);
            }
            sb.Append(']');
            request.AddParameter("application/json", sb, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
