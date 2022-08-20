using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.CreateDeposit
{
    public class CreateDepositCommand : IRequest
    {
        public string Wallet { get; set; }
    }
    public class CreateDepositCommandHandler : IRequestHandler<CreateDepositCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;

        public CreateDepositCommandHandler(IApplicationDbContext context,
                                               IDateTime dateTime,
                                               ICurrentUserService currentUserService,
                                               IIPFSFile ipfs,
                                               INethereumBC nethereum,
                                               IMapper mapper,
                                               IEtherscan etherscan)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _mapper = mapper;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<Unit> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            var depositTransaction = await _nethereum.CreateDepositContract(request.Wallet);

            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            user.DepositContract = depositTransaction.ContractAddress;

            var transaction = await _etherscan.GetTransactionStatus(depositTransaction.TransactionHash, cancellationToken);
            if (transaction.IsError == false)
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = depositTransaction.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = true,
                    EventAction = Domain.Enums.EventAction.USER_DEPOSIT_CREATED
                });
            }
            else
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = depositTransaction.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = false,
                    EventAction = Domain.Enums.EventAction.USER_DEPOSIT_CREATED_FAIL
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
