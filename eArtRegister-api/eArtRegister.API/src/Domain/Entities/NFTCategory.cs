﻿using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFTCategory
    {
        public Guid NFTId { get; set; }
        public string CategoryId { get; set; }

        public virtual NFT NFT { get; set; }
        public virtual Category Category { get; set; }
    }
}
