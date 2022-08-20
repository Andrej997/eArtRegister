using Microsoft.Extensions.Options;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NethereumAccess.Common;
using NethereumAccess.Definitions;
using NethereumAccess.Interfaces;
using NethereumAccess.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using static NethereumAccess.Definitions.DepositDefinition;

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
                var erc721Deployment = new EArtRegisterDeployment()
                {
                    Gas = config.Gass,
                    Name = name
                };

                return await ERC721Service.DeployContractAndWaitForReceiptAsync(web3, erc721Deployment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<string>> IPFSIds(string contractAddress)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var erc721Service = new ERC721Service(web3, contractAddress);

                var total = await erc721Service.TotalSupplyQueryAsync();

                var ipfsIds = new List<string>();
                for (int i = 0; i < total; i++)
                {
                    ipfsIds.Add(await erc721Service.TokenURIQueryAsync(i));
                }

                return ipfsIds;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionReceipt> TransferNFT(string contractAddress, string from, string to, long tokenId)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var erc721Service = new ERC721Service(web3, contractAddress);
                return await erc721Service.TransferFromRequestAndWaitForReceiptAsync(from, to, tokenId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionReceipt> CreatePurchaseContract(string contractAddress, long tokenId)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var traderDeployment = new TraderDeployment()
                {
                    Gas = config.Gass,
                    ContractAddress = contractAddress,
                    TokenId = tokenId,
                    Server = config.ServerWallet
                };

                return await TraderService.DeployContractAndWaitForReceiptAsync(web3, traderDeployment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BigInteger> BalanceOfTrader(string traderContractAddress, string myWallet)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var balances = new BalanceFunction();
                balances.ReturnValue1 = myWallet;

                var traderService = new TraderService(web3, traderContractAddress);
                return await traderService.BalancesRequestAndWaitForReceiptAsync(balances);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionReceipt> CreateDepositContract(string ownerWallet)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var depositDeployment = new DepositDeployment()
                {
                    Gas = config.Gass,
                    Owner = ownerWallet
                };

                return await DepositService.DeployContractAndWaitForReceiptAsync(web3, depositDeployment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BigInteger> GetDepositBalance(string depositContractAddress, string myWallet)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var balances = new DepositDefinition.BalancesFunction();
                balances.ReturnValue1 = myWallet;

                var depositService = new DepositService(web3, depositContractAddress);
                return await depositService.BalanceQueryAsync(balances);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionReceipt> WithdrawDepositContract(string depositContractAddress)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var withdraw = new DepositDefinition.WithdrawFunction();
                withdraw.Server = config.ServerWallet;
                withdraw.Amount = 1000000000000000;

                var depositService = new DepositService(web3, depositContractAddress);
                return await depositService.WithdrawRequestAndWaitForReceiptAsync(withdraw);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BigInteger> TotalSupply(string contractAddress)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var erc721Service = new ERC721Service(web3, contractAddress);
                return await erc721Service.TotalSupplyQueryAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> OwnerOf(string contractAddress, long tokenId)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var ownerOf = new OwnerOfFunction();
                ownerOf.TokenId = tokenId;
                var erc721Service = new ERC721Service(web3, contractAddress);
                return await erc721Service.OwnerOfQueryAsync(ownerOf);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> TokenUri(string contractAddress, long tokenId)
        {
            var account = new Account(config.PrivateKey, config.ChainId);
            var web3 = new Web3(account, config.Url);
            web3.Eth.TransactionManager.UseLegacyAsDefault = true;

            try
            {
                var tokenUri = new TokenURIFunction();
                tokenUri.TokenId = tokenId;
                var erc721Service = new ERC721Service(web3, contractAddress);
                return await erc721Service.TokenURIQueryAsync(tokenUri);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
