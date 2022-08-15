using Etherscan.Models;

namespace Etherscan.Interfaces
{
    public interface IEtherscan
    {
        Task<long> GetBalance(string wallet, CancellationToken cancellationToken);
        Task<RetVal> GetTransactionStatus(string transactionHex, CancellationToken cancellationToken);
    }
}
