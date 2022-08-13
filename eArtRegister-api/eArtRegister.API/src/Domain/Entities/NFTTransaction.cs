using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTTransaction
    {
        public Guid Id { get; set; }
        public Guid NFTId { get; set; }
        public string FromWallet { get; set; }
        public string ToWallet { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string TransactionHash { get; set; }

        public NFT NFT { get; set; }
    }
}
