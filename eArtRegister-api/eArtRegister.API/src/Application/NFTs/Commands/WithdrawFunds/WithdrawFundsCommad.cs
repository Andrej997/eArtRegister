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

namespace eArtRegister.API.Application.NFTs.Commands.WithdrawFunds
{
    public class WithdrawFundsCommad : IRequest
    {
        public Guid NFTId { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class WithdrawFundsCommadHandler : IRequestHandler<WithdrawFundsCommad, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public WithdrawFundsCommadHandler(IApplicationDbContext context,
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

        public async Task<Unit> Handle(WithdrawFundsCommad request, CancellationToken cancellationToken)
        {
            _context.NFTActionHistories.Add(new NFTActionHistory
            {
                EventTimestamp = _dateTime.UtcNow.Ticks,
                TransactionHash = request.TransactionHash,
                Wallet = request.Wallet,
                IsCompleted = request.IsCompleted,
                EventAction = request.IsCompleted ? Domain.Enums.EventAction.WITHDRAW_FROM_SOLD_NFT : Domain.Enums.EventAction.WITHDRAW_FROM_SOLD_NFT_FAIL,
                NFTId = request.NFTId
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
