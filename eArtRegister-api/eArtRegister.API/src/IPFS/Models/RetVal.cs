using Newtonsoft.Json;

namespace IPFS.Models
{
    public class RetVal
    {
        [JsonProperty("Hash")]
        public string Hash { get; set; }
    }
}
