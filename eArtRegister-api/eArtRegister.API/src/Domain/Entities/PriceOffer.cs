using System;

namespace eArtRegister.API.Domain.Entities
{
    public class PriceOffer
    {
        public Guid NFTId { get; set; }
        public double Offer { get; set; }
        public DateTime DateOfOffer { get; set; }
        public Guid UserId { get; set; }

        public virtual NFT NFT { get; set; }
        public virtual User User { get; set; }
    }
}
