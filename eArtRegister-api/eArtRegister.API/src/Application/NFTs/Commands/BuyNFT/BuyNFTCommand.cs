using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.BuyNFT
{
    public class BuyNFTCommand : IRequest
    {
        public Guid NFTId { get; set; }
    }
    public class GetBundlesQueryHandler : IRequestHandler<BuyNFTCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public GetBundlesQueryHandler(IApplicationDbContext context,
                                               IDateTime dateTime,
                                               ICurrentUserService currentUserService,
                                               IIPFSFile ipfs,
                                               INethereumBC nethereum,
                                               IMapper mapper)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _mapper = mapper;
            _nethereum = nethereum;
        }

        public async Task<Unit> Handle(BuyNFTCommand request, CancellationToken cancellationToken)
        {
            var nft = _context.NFTs.Find(request.NFTId);
            var bundle = _context.Bundles.Find(nft.BundleId);
            //var purchaseContactTransaction = await _nethereum.CreatePurchaseContract();
            //var approveTransaction = await _nethereum.ApprovePurchaseContract(bundle.ContractAddress, purchaseContactTransaction.ContractAddress);
            //var onSaleTransaction = await _nethereum.SetNFTonSale(purchaseContactTransaction.ContractAddress, (long)nft.CurrentPrice, bundle.ContractAddress, nft.TokenId);
            //var buyTransaction = await _nethereum.BuyNFT("", bundle.ContractAddress, nft.TokenId);

            return Unit.Value;
        }
    }
}
