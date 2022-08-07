using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.CheckWallet
{
    public class CheckWalletCommand : IRequest
    {
        public string Wallet { get; set; }
    }
    public class CheckWalletCommandHandler : IRequestHandler<CheckWalletCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public CheckWalletCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CheckWalletCommand request, CancellationToken cancellationToken)
        {
            if (_context.Wallets.Any(t => t.UserId == Guid.Parse("290839d8-0572-45c4-9622-2840d6d613c5") && t.MetamaskWallet == request.Wallet))
            {
                _context.Wallets.Add(new Domain.Entities.Wallet
                {
                    MetamaskWallet = request.Wallet,
                    UserId = Guid.Parse("290839d8-0572-45c4-9622-2840d6d613c5"),
                    DateAddedWallet = _dateTime.UtcNow
                });

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
