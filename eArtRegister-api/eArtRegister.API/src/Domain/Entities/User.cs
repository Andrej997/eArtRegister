using eArtRegister.API.Domain.Common;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MetamaskWallet { get; set; }
        public DateTime? DateAddedWallet { get; set; }
        public bool EmailNotification { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
        public virtual ICollection<NFT> OwnedNFTs { get; set; }
        public virtual ICollection<NFT> MintedNFTs { get; set; }
    }
}
