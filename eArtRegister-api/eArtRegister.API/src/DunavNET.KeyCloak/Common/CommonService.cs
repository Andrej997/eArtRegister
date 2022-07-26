using Newtonsoft.Json.Linq;
using RestSharp;
using System;

namespace KeyCloak.Common
{
    public class CommonService
    {
        public static string GetToken(KeyCloakConfig config)
        {
            var client = new RestClient($"{config.Url}/auth/realms/{config.Realm}/protocol/openid-connect/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", config.Client);
            request.AddParameter("client_secret", config.ClientSecret);
            //request.AddParameter("username", config.Username);
            //request.AddParameter("password", config.Password);
            IRestResponse response = client.Execute(request);

            try
            {
                return JObject.Parse(response.Content)["access_token"].ToString();
            }
            catch
            {
                throw new Exception("Failed to aquire token");
            }
            
        }

        public static bool IsError(string json)
        {
            try
            {
                JObject jObject = JObject.Parse(json);
                var error = jObject["error"];
                if (error == null)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
