using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.TransferNFT
{
    public class TransferNFTCommand : IRequest
    {
        public Guid NFTId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
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
            var nft = _context.NFTs.Find(request.NFTId);
            var contractAddress = _context.Bundles.Where(t => t.Id == nft.BundleId).Select(t => t.ContractAddress).FirstOrDefault();
            var transactionHash = await _nethereum.TransferNFT(contractAddress, request.From, request.To, nft.TokenId);

            _context.NFTTransactions.Add(new Domain.Entities.NFTTransaction
            {
                TransactionHash = transactionHash,
                ContactAddress = contractAddress,
                FromWallet = request.From,
                ToWallet = request.To,
                BundleId = nft.BundleId,
                DateOfTransaction = _dateTime.UtcNow,
                NFTId = request.NFTId,
            });

            nft.OwnerId = _currentUserService.UserId;
            nft.CurrentWallet = request.To;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
