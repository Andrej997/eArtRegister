using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.Common.Models;
using eArtRegister.API.Domain.Entities;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
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
        public string Wallet { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<AddNFTCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly INethereumBC _nethereum;

        public AddNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, IIPFSFile ipfs, INethereumBC nethereum)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _nethereum = nethereum;
        }

        public async Task<Guid> Handle(AddNFTCommand request, CancellationToken cancellationToken)
        {
            var retVal = await _ipfs.UploadAsync(request.Name, request.File.Content, cancellationToken);

            var minted = await _nethereum.SafeMint(
                _context.Bundles.Where(bundle => bundle.Id == request.BundleId).Select(bundle => bundle.ContractAddress).First(),
                request.Wallet,
                "ipfs://" + retVal.Hash
                );

            long tokenId = _context.NFTs.Where(t => t.BundleId == request.BundleId).Count();


            var entry = new NFT
            {
                IPFSId = retVal.Hash,
                Name = request.Name,
                Description = request.Description,
                OwnerId = _currentUserService.UserId,
                TokenId = tokenId,
                BundleId = request.BundleId,
                StatusId = Domain.Enums.NFTStatus.Minted,
                CreatorId = _currentUserService.UserId,
                MintedAt = _dateTime.UtcNow,
                CurrentPrice = request.Price,
                CurrentPriceDate = _dateTime.UtcNow,
                CreatorRoyalty = request.Royality,
                TransactionHash = minted.TransactionHash,
                To = minted.To,
                TransactionIndex = Convert.ToInt64(minted.TransactionIndex.ToString(), 16),
                From = minted.From,
                CumulativeGasUsed = Convert.ToInt64(minted.CumulativeGasUsed.ToString(), 16),
                MintStatus = Convert.ToInt64(minted.Status.ToString(), 16),
                BlockHash = minted.BlockHash,
                BlockNumber = Convert.ToInt64(minted.BlockNumber.ToString(), 16),
                GasUsed = Convert.ToInt64(minted.GasUsed.ToString(), 16),
                EffectiveGasPrice = Convert.ToInt64(minted.EffectiveGasPrice.ToString(), 16),
                ModifiedBy = _currentUserService.UserId.ToString(),
                ModifiedOn = _dateTime.UtcNow
            };

            _context.NFTs.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }
    }
}
