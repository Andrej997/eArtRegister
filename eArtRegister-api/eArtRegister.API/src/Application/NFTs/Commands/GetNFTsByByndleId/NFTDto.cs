﻿using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace eArtRegister.API.Application.NFTs.Commands.GetNFTsByByndleId
{
    public class NFTDto : IMapFrom<NFT>
    {
        public NFTDto()
        {
            //Categories = new List<NFTCategory>();
            //Followers = new List<FollowNFT>();
            //Likes = new List<NFTLike>();
        }

        public Guid Id { get; set; }
        public string IPFSId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Order { get; set; }
        public string StatusId { get; set; }
        public Guid BundleId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime MintedAt { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime CurrentPriceDate { get; set; }
        public double CreatorRoyalty { get; set; }
        public Guid OwnerId { get; set; }
        public byte[] Bytes { get; set; }

        //public virtual NFTStatus Status { get; set; }
        //public virtual Bundle Bundle { get; set; }
        //public virtual User Creator { get; set; }
        //public virtual User Owner { get; set; }
        //public virtual ICollection<NFTCategory> Categories { get; set; }
        //public virtual ICollection<FollowNFT> Followers { get; set; }
        //public virtual ICollection<NFTLike> Likes { get; set; }
    }
}