using IPFS.Models;

namespace IPFS.Interfaces
{
    public interface IIPFSFile
    {
        Task<RetVal> UploadAsync(string name, Stream data, CancellationToken cancellationToken);

        Task<byte[]> DownloadAsync(string hash, CancellationToken cancellationToken);
    }
}
