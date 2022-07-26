using Newtonsoft.Json;
using System.Collections.Generic;

namespace KeyCloak.Models
{
    public class UserConsent
    {
        public UserConsent() { }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        [JsonProperty("grantedClientScopes")]
        public IEnumerable<string> GrantedClientScopes { get; set; }
        [JsonProperty("createdDate")]
        public long? CreatedDate { get; set; }
        [JsonProperty("lastUpdatedDate")]
        public long? LastUpdatedDate { get; set; }
    }
}
