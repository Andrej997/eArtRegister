using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.Common.Models;
using eArtRegister.API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.AddNFT
{
    public class AddNFTCommand : IRequest<Guid>
    {
        public UploadedFileModel File { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
        public Guid BundleId { get; set; }
        public double CurrentPrice { get; set; }
        public double CreatorRoyality { get; set; }
        public List<string> Categories { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<AddNFTCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public AddNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Guid> Handle(AddNFTCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // store to IPFS
                var ipfsId = "";

                var categories = new List<NFTCategory>(request.Categories.Count);
                foreach (var category in request.Categories)
                {
                    categories.Add(new NFTCategory { CategoryId = category });
                }

                var entry = new NFT
                {
                    IPFSId = ipfsId,
                    Name = request.Name,
                    Description = request.Description,
                    OwnerId = request.OwnerId,
                    Order = _context.Bundles.Where(t => t.OwnerId == request.OwnerId).Count() + 1,
                    BundleId = request.BundleId,
                    StatusId = Domain.Enums.NFTStatus.Minted,
                    CreatorId = request.OwnerId,
                    MintedAt = _dateTime.UtcNow,
                    CurrentPrice = request.CurrentPrice,
                    CurrentPriceDate = _dateTime.UtcNow,
                    CreatorRoyalty = request.CreatorRoyality,
                    Categories = categories
                };

                _context.NFTs.Add(entry);

                await _context.SaveChangesAsync(cancellationToken);

                return entry.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
