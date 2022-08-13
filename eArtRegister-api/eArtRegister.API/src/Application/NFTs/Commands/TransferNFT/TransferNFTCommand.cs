using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.TransferNFT
{
    public class TransferNFTCommand : IRequest
    {
        public Guid NFTId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransactionHash { get; set; }
    }
    public class TransferNFTCommandHandler : IRequestHandler<TransferNFTCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly INethereumBC _nethereum;

        public TransferNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, INethereumBC nethereum)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
        }

        public async Task<Unit> Handle(TransferNFTCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Find(request.From);
            var nft = _context.NFTs.Find(request.NFTId);

            _context.NFTTransactions.Add(new NFTTransaction
            {
                TransactionHash = request.TransactionHash.ToLower(),
                FromWallet = nft.CurrentWallet.ToLower(),
                ToWallet = request.To.ToLower(),
                DateOfTransaction = _dateTime.UtcNow,
                NFTId = request.NFTId,
            });

            nft.OwnerId = user.Id;
            nft.CurrentWallet = request.To;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
