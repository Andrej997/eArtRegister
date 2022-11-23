using eArtRegister.API.Domain.Common;
using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTPurchase : AuditableEntity
    {
        public string Address { get; set; }
        public string Contract { get; set; }
        public string Abi { get; set; }
        public string Bytecode { get; set; }
        public Guid NFTId { get; set; }
        public bool EntireAmount { get; set; }
        public bool RepaymentInInstallments { get; set; }
        public bool Auction { get; set; }
        public double AmountInETH { get; set; }
        public int DaysOnSale { get; set; }
        public double MinParticipation { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual NFT NFT { get; set; }
    }
}
