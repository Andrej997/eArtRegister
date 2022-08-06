using Microsoft.Extensions.Options;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NethereumAccess.Common;
using NethereumAccess.Definitions;
using NethereumAccess.Interfaces;
using NethereumAccess.Util;
using System;
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

        public async Task<TransactionReceipt> SafeMint(string contractAddress, string to, string uri)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var erc721Service = new ERC721Service(web3, contractAddress);
                return await erc721Service.SafeMintRequestAndWaitForReceiptAsync(to, uri);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionReceipt> CreateContact(string name)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var erc721Deployment = new ERC721Deployment()
                {
                    Gas = config.Gass,
                    Name = name,
                    Symbol = config.Symbol
                };

                return await ERC721Service.DeployContractAndWaitForReceiptAsync(web3, erc721Deployment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
