using eArtRegister.API.Application.Common.Interfaces;
using Etherscan.Interfaces;
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
        private readonly IEtherscan _etherscan;

        public GetRolesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime, INethereumBC nethereum, IEtherscan etherscan)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<UserDto> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            if (user == null)
            {
                return new UserDto
                {
                    RoleIds = new List<long>()
                };
            }
            var roles = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(u => u.RoleId).ToList();
            long balanace = 0;
            if (!string.IsNullOrEmpty(user.DepositContract))
            {
                var balanaceString = (await _nethereum.GetDepositBalance(user.DepositContract, user.Wallet)).ToString();
                balanace = long.Parse(balanaceString);
            }

            if (!string.IsNullOrEmpty(user.DepositContract) && balanace > 0)
            {
                if (!_context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == (long)Domain.Enums.Role.Seller))
                {
                    _context.UserRoles.Add(new Domain.Entities.UserRole
                    {
                        UserId = user.Id,
                        RoleId = (long)Domain.Enums.Role.Seller
                    });
                    await _context.SaveChangesAsync(cancellationToken);
                    roles.Add((long)Domain.Enums.Role.Seller);
                }
            }

            return new UserDto
            {
                Wallet = user.Wallet,
                RoleIds = roles,
                DepositContract = user.DepositContract,
                DepositBalance = balanace,
                ServerBalance = user.ServerBalance,
                WalletBalance = await _etherscan.GetBalance(user.Wallet, cancellationToken)
            };
        }
    }
}
