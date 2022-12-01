using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.DeleteDeposit
{
    public class DeleteDepositCommand : IRequest
    {
        public string Wallet { get; set; }

        public DeleteDepositCommand(string wallet)
        {
            Wallet = wallet;
        }
    }
    public class DeleteDepositCommandHandler : IRequestHandler<DeleteDepositCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public DeleteDepositCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(DeleteDepositCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            if (user != null)
            {
                user.DepositAbi = null;
                user.DepositAddress = null;
                user.DepositBytecode = null;
                user.DepositCreated = null;
                user.DepositContract = null;

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
