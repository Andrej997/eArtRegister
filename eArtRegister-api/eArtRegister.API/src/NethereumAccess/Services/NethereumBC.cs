using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using NethereumAccess.Common;
using NethereumAccess.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NethereumAccess.Services
{
    public class NethereumBC : INethereumBC
    {
        private readonly NethereumConfig config;

        public NethereumBC(IOptions<NethereumConfig> settings)
        {
            config = settings.Value;
        }

        public async Task TestContract()
        {
            var abi = @"[{""inputs"":[],""name"":""get"",""outputs"":[{""internalType"":""uint256"",""name"":"""",""type"":""uint256""}],""stateMutability"":""view"",""type"":""function""}]";
            var byteCode = "0x6080604052348015600f57600080fd5b50607780601d6000396000f3fe6080604052348015600f57600080fd5b506004361060285760003560e01c80636d4ce63c14602d575b600080fd5b600860405190815260200160405180910390f3fea2646970667358221220b82df5e4c342f8f5343cf0594fde4d13b0f0bfdc39969c4cdf65e402b21ff24164736f6c634300080b0033";
            var web3 = new Web3();
            try
            {
                var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(config.SenderAddress, config.SenderPassword, 120);
                if (!unlockAccountResult) throw new Exception();
                var trasactionHash = await web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, config.SenderAddress, gas: new HexBigInteger(100000));
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trasactionHash);
                while (receipt == null)
                {
                    Thread.Sleep(5000);
                    receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(trasactionHash);
                }
                var contractAddress = receipt.ContractAddress;

                var contract = web3.Eth.GetContract(abi, contractAddress);

                var callFunction = contract.GetFunction("get");

                var result = await callFunction.CallAsync<int>();

                if (result != 8) throw new Exception();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
