using System;

namespace eArtRegister.API.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string MetamaskWallet { get; set; }
        public DateTime? DateAddedWallet { get; set; }


        public virtual User User { get; set; }
    }
}
