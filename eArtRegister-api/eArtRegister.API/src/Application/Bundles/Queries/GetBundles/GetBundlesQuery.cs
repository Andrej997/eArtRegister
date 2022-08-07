using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using IPFS.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NethereumAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Bundles.Queries.GetBundles
{
    public class GetBundlesQuery : IRequest<List<BundleDto>>
    {
    }
    public class GetBundlesQueryHandler : IRequestHandler<GetBundlesQuery, List<BundleDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public GetBundlesQueryHandler(IApplicationDbContext context,
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

        public async Task<List<BundleDto>> Handle(GetBundlesQuery request, CancellationToken cancellationToken)
        {
            var ret = await _context.Bundles
                    .AsNoTracking()
                    .OrderBy(t => t.Order)
                    .ProjectTo<BundleDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            return ret;
        }
    }
}
