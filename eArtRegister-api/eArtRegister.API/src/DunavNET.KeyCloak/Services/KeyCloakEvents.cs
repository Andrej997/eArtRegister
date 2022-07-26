using KeyCloak.Common;
using KeyCloak.Interfaces;
using KeyCloak.Models;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyCloak.Services
{
    public class KeyCloakEvents : IKeyCloakEvents
    {
        private readonly KeyCloakConfig config;

        public KeyCloakEvents(IOptions<KeyCloakConfig> settings)
        {
            config = settings.Value;
        }

        public IEnumerable<Event> GetEvents(EventFilter filter)
        {
            var token = CommonService.GetToken(config);
            var client = new RestClient($"{config.Url}/auth/admin/realms/{config.Realm}/events");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            if (filter.DateFrom.Year != 1)
                request.AddParameter("dateFrom", $"{filter.DateFrom.Year}-{filter.DateFrom.Month}-{filter.DateFrom.Day}");

            if (filter.DateTo.Year != 1)
                request.AddParameter("dateTo", $"{filter.DateTo.Year}-{filter.DateTo.Month}-{filter.DateTo.Day}");

            if (filter.UserId != null && filter.UserId != "")
                request.AddParameter("user", filter.UserId); 

            if (filter.EventTypes != null && filter.EventTypes.Any())
            {
                foreach (var eventType in filter.EventTypes)
                {
                    request.AddParameter("type", eventType);
                }
            }
           
            IRestResponse response = client.Execute(request);
            if (!CommonService.IsError(response.Content))
                return CommonTemplateService<Event>.ConvertToEntities(response.Content);
            else
                throw new Exception();
        }
    }
}
