using Ipfs.Http;
using IPFS.Common;
using IPFS.Interfaces;
using IPFS.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IPFS.Services
{
    public class IPFSFile : IIPFSFile
    {
        private readonly IPFSConfig _config;
        private readonly IpfsClient _ipfsRead;
        private readonly IpfsClient _ipfsStore;

        public IPFSFile(IOptions<IPFSConfig> settings)
        {
            _config = settings.Value;
            _ipfsRead = new IpfsClient(_config.UrlRead);
            _ipfsStore = new IpfsClient(_config.UrlStore);
        }

        public async Task<byte[]> DownloadAsync(string hash, CancellationToken cancellationToken)
        {
            using (var stream = await _ipfsRead.FileSystem.ReadFileAsync(hash, cancellationToken))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public async Task<RetVal> UploadAsync(string name, Stream data, CancellationToken cancellationToken)
        {
            var retVal = await _ipfsStore.UploadAsync("add", cancellationToken, data, name);

            var result = JsonConvert.DeserializeObject<RetVal>(retVal);

            return result;
        }
    }
}
