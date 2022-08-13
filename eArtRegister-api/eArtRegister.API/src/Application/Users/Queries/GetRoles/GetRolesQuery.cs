using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<UserDto>
    {
        public string Wallet { get; set; }

        public GetRolesQuery(string wallet)
        {
            Wallet = wallet;
        }
    }
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly INethereumBC _nethereum;

        public GetRolesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime, INethereumBC nethereum)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _nethereum = nethereum;
        }

        public async Task<UserDto> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            var roles = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(u => u.RoleId).ToList();
            long balanace = 0;
            if (!string.IsNullOrEmpty(user.DepositContract))
            {
                var balanaceString = (await _nethereum.GetDepositBalance(user.DepositContract, user.Wallet)).ToString();
                balanace = long.Parse(balanaceString);
            }

            return new UserDto
            {
                Wallet = user.Wallet,
                RoleIds = roles,
                DepositContract = user.DepositContract,
                DepositBalance = balanace,
                ServerBalance = user.ServerBalance
            };
        }
    }
}
