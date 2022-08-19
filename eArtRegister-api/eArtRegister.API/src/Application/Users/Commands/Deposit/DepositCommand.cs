using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.Deposit
{
    public class DepositCommand : IRequest
    {
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public DepositCommandHandler(IApplicationDbContext context,
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

        public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();

            _context.ServerActionHistories.Add(new NFTActionHistory
            {
                EventTimestamp = _dateTime.UtcNow.Ticks,
                TransactionHash = request.TransactionHash,
                Wallet = request.Wallet,
                IsCompleted = request.IsCompleted,
                EventAction = request.IsCompleted? Domain.Enums.EventAction.USER_ADDED_DEPOSIT : Domain.Enums.EventAction.USER_ADDED_DEPOSIT_FAIL
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
