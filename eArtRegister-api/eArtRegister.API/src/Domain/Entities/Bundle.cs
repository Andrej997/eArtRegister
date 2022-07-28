using eArtRegister.API.Domain.Common;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class Bundle : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Order { get; set; }
        public bool IsObservable { get; set; }
        public Guid OwnerId { get; set; }

        public virtual User Owner { get; set; }
        public virtual ICollection<NFT> NFTs { get; set; }
        public virtual ICollection<FollowBundle> Followers { get; set; }
        public virtual ICollection<BundleRate> Rates { get; set; }
    }
}
