using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTRate
    {
        private double rate;

        public Guid NFTId { get; set; }
        public double Rate
        {
            get
            {
                return rate;
            }
            set
            {
                CheckRate(value);
                rate = value;
            }
        }
        public DateTime DateOfRate { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }

        public virtual NFT NFT { get; set; }
        public virtual User User { get; set; }

        private void CheckRate(double rate)
        {
            if (rate <= 0)
                throw new Exception("Rate can't be lower or equal to 0");
            if (rate > 5)
                throw new Exception("Rate can't be bigger than five");
        }
    }
}
