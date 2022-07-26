using System;

namespace eArtRegister.API.Domain.Entities
{
    public class FollowNFT
    {
        public Guid UserId { get; set; }
        public Guid NFTId { get; set; }
        public DateTime FollowDate { get; set; }

        public virtual User User { get; set; }
        public virtual NFT NFT { get; set; }
    }
}
