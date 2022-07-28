using eArtRegister.API.Domain.Common;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        public string MetamaskWallet { get; set; }
        public DateTime? DateAddedWallet { get; set; }
        public bool EmailNotification { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<NFT> OwnedNFTs { get; set; }
        public virtual ICollection<NFT> MintedNFTs { get; set; }
        public virtual ICollection<FollowBundle> FollowingBundles { get; set; }
        public virtual ICollection<FollowNFT> FollowingNFTs { get; set; }
        public virtual ICollection<FollowUser> FollowingUsers { get; set; }
        public virtual ICollection<FollowUser> Followers { get; set; }
        public virtual ICollection<NFTLike> Liked { get; set; }
        public virtual ICollection<UserPortalNotification> Notifications { get; set; }
        public virtual ICollection<BundleRate> RatedBundles { get; set; }
        public virtual ICollection<NFTRate> RatedNFTs { get; set; }
        public virtual ICollection<BidPrice> Bids { get; set; }
        public virtual ICollection<PriceOffer> Offers { get; set; }
    }
}
