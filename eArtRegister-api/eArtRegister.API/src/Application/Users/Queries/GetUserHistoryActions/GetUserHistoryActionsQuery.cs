using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using Etherscan.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NethereumAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Queries.GetUserHistoryActions
{
    public class GetUserHistoryActionsQuery : IRequest<List<ActionHistoryDto>>
    {
        public string Wallet { get; set; }

        public GetUserHistoryActionsQuery(string wallet)
        {
            Wallet = wallet;
        }
    }
    public class GetUserHistoryActionsQueryHandler : IRequestHandler<GetUserHistoryActionsQuery, List<ActionHistoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;
        private readonly IMapper _mapper;

        public GetUserHistoryActionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IDateTime dateTime, INethereumBC nethereum, IEtherscan etherscan, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _nethereum = nethereum;
            _etherscan = etherscan;
            _mapper = mapper;
        }

        public async Task<List<ActionHistoryDto>> Handle(GetUserHistoryActionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.NFTActionHistories
                    .AsNoTracking()
                    .Where(t => t.Wallet.ToLower() == request.Wallet.ToLower())
                    .OrderByDescending(t => t.EventTimestamp)
                    .ProjectTo<ActionHistoryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
        }
    }
}
