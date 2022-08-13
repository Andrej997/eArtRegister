using System;

namespace eArtRegister.API.Domain.Entities
{
    public class UserDeposit
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long DepositValue { get; set; }
        public DateTime DepositeDate { get; set; }
        public string TransactionHash { get; set; }

        public virtual User User { get; set; }
    }
}
