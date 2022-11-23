using AutoMapper;
using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Application.NFTs.Queries.GetNFTsByByndleId
{
    public class NFTDto : IMapFrom<NFT>
    {
        public NFTDto()
        {
            PurchaseContracts = new List<NFTPurchaseDto>();
        }

        public Guid Id { get; set; }
        public string IPFSImageHash { get; set; }
        public string IPFSImageSize { get; set; }
        public string IPFSNFTHash { get; set; }
        public string IPFSNFTSize { get; set; }
        public long TokenId { get; set; }
        public string StatusId { get; set; }
        public Guid BundleId { get; set; }
        public string TransactionHash { get; set; }
        public string NFTData { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatorWallet { get; set; }
        public string TokenStandard { get; set; }
        public List<NFTPurchaseDto> PurchaseContracts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NFT, NFTDto>()
                .ForMember(x => x.CreatorWallet, y => y.MapFrom(z => z.Bundle.Owner.Wallet));
        }
    }
}
