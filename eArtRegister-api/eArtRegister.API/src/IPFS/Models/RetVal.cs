using Newtonsoft.Json;

namespace IPFS.Models
{
    public class RetVal
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Hash")]
        public string Hash { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }
    }
}
