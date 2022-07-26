using Newtonsoft.Json;
using System.Collections.Generic;

namespace KeyCloak.Models
{
    public class RoleComposite
    {
        public RoleComposite() { }

        [JsonProperty("client")]
        public IDictionary<string, string> Client { get; set; }
        [JsonProperty("realm")]
        public IEnumerable<string> Realm { get; set; }
    }
}
