using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using IPFS.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NethereumAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Queries.GetNFTsByByndleId
{
    public class GetNFTsByByndleIdCommand : IRequest<List<NFTDto>>
    {
        public GetNFTsByByndleIdCommand(string customRoute)
        {
            CustomRoute = customRoute;
            TokenId = -1;
        }

        public GetNFTsByByndleIdCommand(string customRoute, long tokenId)
        {
            CustomRoute = customRoute;
            TokenId = tokenId;
        }

        public string CustomRoute { get; set; }
        public long TokenId { get; }
    }
    public class GetNFTsByByndleIdCommandHandler : IRequestHandler<GetNFTsByByndleIdCommand, List<NFTDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public GetNFTsByByndleIdCommandHandler(IApplicationDbContext context,
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

        public async Task<List<NFTDto>> Handle(GetNFTsByByndleIdCommand request, CancellationToken cancellationToken)
        {
            var bundle = _context.Bundles.FirstOrDefault(x => x.CustomRoot == request.CustomRoute);

            var user = _context.SystemUsers.Where(u => u.Id == bundle.OwnerId).FirstOrDefault();

            var query = _context.NFTs
                    .AsNoTracking()
                    .Where(t => t.BundleId == bundle.Id);

            if (request.TokenId >= 0)
                query = query
                    .Include(x => x.PurchaseContracts)
                    .Where(x => x.TokenId == request.TokenId);

            var ret = await query
                    .OrderBy(t => t.TokenId)
                    .Include(t => t.PurchaseContracts)
                    .ProjectTo<NFTDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            if (request.TokenId >= 0 && ret.Any() && ret[0].PurchaseContracts.Any())
                ret[0].PurchaseContracts = ret[0].PurchaseContracts.OrderByDescending(x => x.CreatedOn).ToList();

            return ret;
        }
    }
}
