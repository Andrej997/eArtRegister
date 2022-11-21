using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using Etherscan.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Bundles.Queries.GetBundles
{
    public class GetBundleQuery : IRequest<BundleDto>
    {
        public string CustomRoot { get; set; }

        public GetBundleQuery(string customRoot)
        {
            CustomRoot = customRoot;
        }
    }
    public class GetBundleQueryHandler : IRequestHandler<GetBundleQuery, BundleDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        

        public GetBundleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BundleDto> Handle(GetBundleQuery request, CancellationToken cancellationToken)
        {
            var bundle = _context.Bundles
                .Include(x => x.Owner)
                .AsNoTracking()
                .Where(t => t.CustomRoot == request.CustomRoot)
                .ProjectTo<BundleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            return bundle;
        }
    }
}
