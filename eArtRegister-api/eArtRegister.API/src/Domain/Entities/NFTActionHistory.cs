using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTActionHistory
    {
        public Guid Id { get; set; }
        public string Wallet { get; set; }
        public Guid? NFTId { get; set; }
        public string EventAction { get; set; }
        public long EventTimestamp { get; set; }
        public string TransactionHash { get; set; }
        public bool IsCompleted { get; set; }

        public virtual NFT NFT { get; set; }
    }
}
