using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class NFT
    {
        public Guid Id { get; set; }
        public string IPFSId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Order { get; set; }
        public string StatusId { get; set; }
        public Guid BundleId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime MintedAt { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime CurrentPriceDate { get; set; }
        public double CreatorRoyalty { get; set; }
        public Guid OwnerId { get; set; }
        public bool FixedPrice { get; set; }
        public double MinBidPrice { get; set; }

        public virtual NFTStatus Status { get; set; }
        public virtual Bundle Bundle { get; set; }
        public virtual User Creator { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<NFTCategory> Categories { get; set; }
        public virtual ICollection<FollowNFT> Followers { get; set; }
        public virtual ICollection<NFTLike> Likes { get; set; }
        //public virtual ICollection<NFTPriceHistory> PriceHistory { get; set; }
    }
}
