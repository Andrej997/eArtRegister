using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using Etherscan.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NethereumAccess.Interfaces;
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
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime, INethereumBC nethereum, IEtherscan etherscan, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _nethereum = nethereum;
            _etherscan = etherscan;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.SystemUsers
                .Where(u => u.Wallet.ToLower() == request.Wallet.ToLower())
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }
    }
}
