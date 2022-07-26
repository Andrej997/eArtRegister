using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTPriceHistory
    {
        public Guid ParentId { get; set; }
        public double Price { get; set; }
        public DateTime DateOfPrice { get; set; }

        public virtual NFT NFT { get; set; }
    }
}
