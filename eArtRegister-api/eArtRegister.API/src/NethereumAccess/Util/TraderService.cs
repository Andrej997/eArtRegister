using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using NethereumAccess.Definitions;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace NethereumAccess.Util
{
    public class TraderService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TraderDeployment tradorDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TraderDeployment>().SendRequestAndWaitForReceiptAsync(tradorDeployment, cancellationTokenSource);
        }
    }
}
