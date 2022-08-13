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

        protected Nethereum.Web3.Web3 Web3 { get; }

        public ContractHandler ContractHandler { get; }

        public TraderService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<TransactionReceipt> AddListingRequestAndWaitForReceiptAsync(AddListingFunction addListingFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(addListingFunction, cancellationToken);
        }

        public Task<TransactionReceipt> BalancesRequestAndWaitForReceiptAsync(BalancesFunction balancesFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(balancesFunction, cancellationToken);
        }

        public Task<TransactionReceipt> PurchaseRequestAndWaitForReceiptAsync(PurchaseFunction purchaseFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(purchaseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(WithdrawFunction withdrawFunction, CancellationTokenSource cancellationToken = null)
        {
            return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }
    }
}
