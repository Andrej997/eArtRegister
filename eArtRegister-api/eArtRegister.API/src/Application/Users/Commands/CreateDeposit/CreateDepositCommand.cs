using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
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

        public CreateDepositCommandHandler(IApplicationDbContext context,
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

        public async Task<Unit> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            var depositTransaction = await _nethereum.CreateDepositContract();

            var user = _context.Users.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            user.DepositContract = depositTransaction.ContractAddress;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
