using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
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
            var user = _context.Users.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            if (user == null)
            {
                _context.Users.Add(new User
                {
                    Wallet = request.Wallet.ToLower(),
                    DateWalletAdded = _dateTime.UtcNow,
                    Roles = new List<UserRole>
                    {
                        new UserRole
                        {
                            RoleId = (long)Domain.Enums.Role.Observer
                        }
                    }
                });

                _context.NFTActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    Wallet = request.Wallet,
                    IsCompleted = true,
                    EventAction = Domain.Enums.EventAction.USER_CREATED
                });

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
