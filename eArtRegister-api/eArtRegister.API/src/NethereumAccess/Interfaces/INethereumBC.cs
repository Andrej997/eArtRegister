using Nethereum.RPC.Eth.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NethereumAccess.Interfaces
{
    public interface INethereumBC
    {
        Task<TransactionReceipt> SafeMint(string contractAddress, string to, string uri);
        Task<TransactionReceipt> CreateContact(string name);
        Task<List<string>> IPFSIds(string contractAddress);
    }
}
