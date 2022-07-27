using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.Common.Models;
using eArtRegister.API.Domain.Entities;
using IPFS.Interfaces;
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
        public Guid BundleId { get; set; }
        public double Price { get; set; }
        public double Royality { get; set; }
        public List<string> Categories { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<AddNFTCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;

        public AddNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, IIPFSFile ipfs)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
        }

        public async Task<Guid> Handle(AddNFTCommand request, CancellationToken cancellationToken)
        {
            var retVal = await _ipfs.UploadAsync(request.Name, request.File.Content, cancellationToken);

            var entry = new NFT
            {
                IPFSId = retVal.Hash,
                Name = request.Name,
                Description = request.Description,
                OwnerId = _currentUserService.UserId,
                Order = _context.Bundles.Where(t => t.OwnerId == _currentUserService.UserId).Count() + 1,
                BundleId = request.BundleId,
                StatusId = Domain.Enums.NFTStatus.Minted,
                CreatorId = _currentUserService.UserId,
                MintedAt = _dateTime.UtcNow,
                CurrentPrice = request.Price,
                CurrentPriceDate = _dateTime.UtcNow,
                CreatorRoyalty = request.Royality,
            };

            _context.NFTs.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }
    }
}
