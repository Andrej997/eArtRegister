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
    public class KeyCloakClients : IKeyCloakClients
    {
        private readonly KeyCloakConfig config;

        public KeyCloakClients(IOptions<KeyCloakConfig> settings)
        {
            config = settings.Value;
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

        public IEnumerable<Client> GetClients()
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/clients");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            //return response.Content;
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Client>.ConvertToEntities(response.Content);
            else
            {
                var jObject = JObject.Parse(response.Content);
                throw new Exception("Keycloak: " + (jObject["error"]).ToString());
            }
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

            if (response.StatusCode == HttpStatusCode.NoContent)
                return response.Content;

            if (response.StatusCode != HttpStatusCode.OK)
                throw new KeyCloakUserException(response.Content);

            return response.Content;
        }
    }
}
