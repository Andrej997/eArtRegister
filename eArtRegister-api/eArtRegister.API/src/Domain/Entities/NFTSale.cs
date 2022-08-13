using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTSale
    {
        public Guid Id { get; set; }
        public Guid NFTId { get; set; }
        public string WalletSet { get; set; }
        public string WalletBought { get; set; }
        public DateTime DateOfSet { get; set; }
        public string TransactionHashSet { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string TransactionHashPurchase { get; set; }
        public string SaleContractAddress { get; set; }

        public NFT NFT { get; set; }
    }
}
