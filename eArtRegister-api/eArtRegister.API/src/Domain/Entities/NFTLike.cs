using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTLike
    {
        public Guid UserId { get; set; }
        public string NFTId { get; set; }
        public DateTime LikedDate { get; set; }

        public virtual User User { get; set; }
        public virtual NFT NFT { get; set; }
    }
}
