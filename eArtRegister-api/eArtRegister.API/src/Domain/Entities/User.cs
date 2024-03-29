﻿using eArtRegister.API.Domain.Common;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Wallet { get; set; }
        public string DepositContract { get; set; }
        public string DepositAddress { get; set; }
        public string DepositAbi { get; set; }
        public string DepositBytecode { get; set; }
        public DateTime? DepositCreated { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<FollowBundle> FollowingBundles { get; set; }
        public virtual ICollection<FollowNFT> FollowingNFTs { get; set; }
        public virtual ICollection<FollowUser> FollowingUsers { get; set; }
        public virtual ICollection<FollowUser> Followers { get; set; }
        public virtual ICollection<NFTLike> NFTLikes { get; set; }
        public virtual ICollection<UserPortalNotification> Notifications { get; set; }
        public virtual ICollection<BundleRate> BundleRates { get; set; }
        public virtual ICollection<NFTRate> NFTRates { get; set; }
        public virtual ICollection<PriceOffer> Offers { get; set; }
    }
}
