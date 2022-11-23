using eArtRegister.API.Domain.Common;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class NFT : AuditableEntity
    {
        public Guid Id { get; set; }
        public string IPFSImageHash { get; set; }
        public string IPFSImageSize { get; set; }
        public string IPFSNFTHash { get; set; }
        public string IPFSNFTSize { get; set; }
        public long TokenId { get; set; }
        public string StatusId { get; set; }
        public Guid BundleId { get; set; }
        public string TransactionHash { get; set; }
        public string TokenStandard { get; set; }

        public virtual NFTStatus Status { get; set; }
        public virtual Bundle Bundle { get; set; }
        public virtual ICollection<NFTCategory> Categories { get; set; }
        public virtual ICollection<FollowNFT> Followers { get; set; }
        public virtual ICollection<NFTLike> Likes { get; set; }
        public virtual ICollection<NFTRate> Rates { get; set; }
        public virtual ICollection<NFTPurchase> PurchaseContracts { get; set; }
        public virtual ICollection<PriceOffer> PriceOffers { get; set; }
        //public virtual ICollection<NFTPriceHistory> PriceHistory { get; set; }
    }
}
