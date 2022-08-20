using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.Approved
{
    public class ApprovedCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class ApprovedCommandHandler : IRequestHandler<ApprovedCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;

        public ApprovedCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, INethereumBC nethereum, IEtherscan etherscan)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<Unit> Handle(ApprovedCommand request, CancellationToken cancellationToken)
        {
            var nft = _context.NFTs.Find(request.Id);
            if (nft == null)
                throw new Exception("Unknown NFT");

            if (request.IsCompleted)
                nft.StatusId = Domain.Enums.NFTStatus.Approved;

            _context.ServerActionHistories.Add(new NFTActionHistory
            {
                EventTimestamp = _dateTime.UtcNow.Ticks,
                TransactionHash = request.TransactionHash,
                Wallet = request.Wallet,
                IsCompleted = request.IsCompleted,
                EventAction = request.IsCompleted? Domain.Enums.EventAction.PURCHASE_CONTRACT_APPROVED : Domain.Enums.EventAction.PURCHASE_CONTRACT_APPROVED_FAIL,
                NFTId = request.Id
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
