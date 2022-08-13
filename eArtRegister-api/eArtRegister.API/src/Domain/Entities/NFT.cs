using eArtRegister.API.Domain.Common;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class NFT : AuditableEntity
    {
        public Guid Id { get; set; }
        public string IPFSId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long TokenId { get; set; }
        public string StatusId { get; set; }
        public Guid BundleId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime MintedAt { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime CurrentPriceDate { get; set; }
        public double CreatorRoyalty { get; set; }
        public string CurrentWallet { get; set; }
        public bool FixedPrice { get; set; }
        public double MinBidPrice { get; set; }
        public string TransactionHash { get; set; }
        public long TransactionIndex { get; set; }
        public string BlockHash { get; set; }
        public long BlockNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public long CumulativeGasUsed { get; set; }
        public long GasUsed { get; set; }
        public long EffectiveGasPrice { get; set; }
        public long MintStatus { get; set; }

        public virtual NFTStatus Status { get; set; }
        public virtual Bundle Bundle { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<NFTCategory> Categories { get; set; }
        public virtual ICollection<FollowNFT> Followers { get; set; }
        public virtual ICollection<NFTLike> Likes { get; set; }
        public virtual ICollection<NFTRate> Rates { get; set; }
        public virtual ICollection<PriceOffer> PriceOffers { get; set; }
        public virtual ICollection<BidPrice> Bids { get; set; }
        public virtual ICollection<NFTTransaction> Transactions { get; set; }
        //public virtual ICollection<NFTPriceHistory> PriceHistory { get; set; }
    }
}
