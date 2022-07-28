using System;

namespace eArtRegister.API.Domain.Entities
{
    public class BidPrice
    {
        public Guid NFTId { get; set; }
        public double Bid { get; set; }
        public DateTime BidDate { get; set; }
        public Guid UserId { get; set; }

        public virtual NFT NFT { get; set; }
        public virtual User User { get; set; }
    }
}
