using System;

namespace eArtRegister.API.Domain.Entities
{
    public class BundleRate
    {
        private double rate;

        public Guid BundleId { get; set; }
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

        public virtual Bundle Bundle { get; set; }
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
