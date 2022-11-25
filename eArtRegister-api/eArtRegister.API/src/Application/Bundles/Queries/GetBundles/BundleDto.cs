using AutoMapper;
using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;

namespace eArtRegister.API.Application.Bundles.Queries.GetBundles
{
    public class BundleDto : IMapFrom<Bundle>
    {
        public BundleDto() { }

        public string CustomRoot { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Abi { get; set; }
        public string Address { get; set; }
        public string Contract { get; set; }
        public string OwnerWallet { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Bundle, BundleDto>()
                .ForMember(x => x.OwnerWallet, y => y.MapFrom(z => z.Owner.Wallet));
        }
    }
}
