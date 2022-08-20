using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public string Wallet { get; set; }

        public GetUserQuery(string wallet)
        {
            Wallet = wallet;
        }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;

        public GetUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime, INethereumBC nethereum, IEtherscan etherscan)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            if (user == null)
            {
                return new UserDto
                {
                    RoleIds = new List<long>()
                };
            }
            var roles = _context.SystemUserRoles.Where(ur => ur.UserId == user.Id).Select(u => u.RoleId).ToList();
            long balanace = 0;
            if (!string.IsNullOrEmpty(user.DepositContract))
            {
                var balanaceString = (await _nethereum.GetDepositBalance(user.DepositContract, user.Wallet)).ToString();
                balanace = long.Parse(balanaceString);
            }

            if (!string.IsNullOrEmpty(user.DepositContract) && balanace > 0)
            {
                if (!_context.SystemUserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == (long)Domain.Enums.Role.Seller))
                {
                    _context.SystemUserRoles.Add(new UserRole
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
                Wallet = user.Wallet.ToLower(),
                RoleIds = roles,
                DepositContract = user.DepositContract,
                DepositBalance = balanace,
                WalletBalance = await _etherscan.GetBalance(user.Wallet, cancellationToken)
            };
        }
    }
}
