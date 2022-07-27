using Ipfs.Http;
using IPFS.Common;
using IPFS.Interfaces;
using IPFS.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace IPFS.Services
{
    public class IPFSFile : IIPFSFile
    {
        private readonly IPFSConfig config;

        public IPFSFile(IOptions<IPFSConfig> settings)
        {
            config = settings.Value;
        }

        public async Task<RetVal> UploadAsync(string name, Stream data, CancellationToken cancellationToken)
        {
            var ipfs = new IpfsClient(config.Url);
            var retVal = await ipfs.UploadAsync("add", cancellationToken, data, name);

            var json = JObject.Parse(retVal);

            return null;
        }
    }
}
