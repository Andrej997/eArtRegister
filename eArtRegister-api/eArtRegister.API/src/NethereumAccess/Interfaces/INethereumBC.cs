﻿using Nethereum.RPC.Eth.DTOs;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace NethereumAccess.Interfaces
{
    public interface INethereumBC
    {
        Task<TransactionReceipt> SafeMint(string contractAddress, string to, string uri);
        Task<BigInteger> TotalSupply(string contractAddress);
        Task<string> OwnerOf(string contractAddress, long tokenId);
        Task<string> TokenUri(string contractAddress, long tokenId);
        Task<TransactionReceipt> CreateContact(string name);
        Task<List<string>> IPFSIds(string contractAddress);
        Task<TransactionReceipt> TransferNFT(string contractAddress, string from, string to, long tokenId);
        Task<TransactionReceipt> CreatePurchaseContract(string contractAddress, long tokenId);
        Task<BigInteger> BalanceOfTrader(string traderContractAddress, string myWallet);
        Task<TransactionReceipt> CreateDepositContract(string ownerWallet);
        Task<BigInteger> GetDepositBalance(string depositContractAddress, string myWallet);
        Task<TransactionReceipt> WithdrawDepositContract(string depositContractAddress);
    }
}
