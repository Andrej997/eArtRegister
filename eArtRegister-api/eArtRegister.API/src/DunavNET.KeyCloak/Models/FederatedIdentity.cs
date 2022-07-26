using Newtonsoft.Json;

namespace KeyCloak.Models
{
    public class FederatedIdentity
    {
        public FederatedIdentity()
        {

        }
        [JsonProperty("identityProvider")]
        public string IdentityProvider { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
    }
}
