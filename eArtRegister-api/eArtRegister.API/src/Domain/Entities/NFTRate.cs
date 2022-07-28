using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTRate
    {
        public Guid NFTId { get; set; }
        public double Rate { get; set; }
        public DateTime DateOfRate { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }

        public virtual NFT NFT { get; set; }
        public virtual User User { get; set; }
    }
}
