using AutoMapper;
using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public double AmountInETH { get; set; }
        public int DaysOnSale { get; set; }
        public double MinParticipation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime SaleEnds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NFTPurchase, NFTPurchaseDto>()
                .ForMember(x => x.SaleEnds, y => y.MapFrom(z => z.CreatedOn.Value.AddDays(z.DaysOnSale)));
        }
    }
}
