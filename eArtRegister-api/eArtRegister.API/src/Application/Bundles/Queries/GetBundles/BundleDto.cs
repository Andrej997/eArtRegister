using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;

namespace eArtRegister.API.Application.Bundles.Queries.GetBundles
{
    public class BundleDto : IMapFrom<Bundle>
    {
        public BundleDto() { }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Order { get; set; }
        public bool IsObservable { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerWallet { get; set; }
    }
}
