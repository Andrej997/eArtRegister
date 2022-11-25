using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;

namespace eArtRegister.API.Application.NFTs.Queries.GetNFTsByByndleId
{
    public class NFTPurchaseDto : IMapFrom<NFTPurchase>
    {
        public string Address { get; set; }
        public string Contract { get; set; }
        public string Abi { get; set; }
        public string Bytecode { get; set; }
        public Guid NFTId { get; set; }
        public bool EntireAmount { get; set; }
        public bool RepaymentInInstallments { get; set; }
        public bool Auction { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
