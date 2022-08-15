using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.Bought
{
    public class BoughtCommand : IRequest
    {
        public Guid NFTId { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
    }
    public class BoughtCommandHandler : IRequestHandler<BoughtCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public BoughtCommandHandler(IApplicationDbContext context,
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

        public async Task<Unit> Handle(BoughtCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();

            var nft = _context.NFTs.Where(nft => nft.Id == request.NFTId).FirstOrDefault();

            //var nftSale = _context.NFTSales.Where(s => s.NFTId == request.NFTId && s.SaleContractAddress == nft.PurchaseContract).FirstOrDefault();

            //nftSale.WalletBought = request.Wallet;
            //nftSale.DateOfPurchase = _dateTime.UtcNow;
            //nftSale.TransactionHashPurchase = request.TransactionHash;

            nft.CurrentWallet = request.Wallet;
            nft.StatusId = Domain.Enums.NFTStatus.Sold;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
