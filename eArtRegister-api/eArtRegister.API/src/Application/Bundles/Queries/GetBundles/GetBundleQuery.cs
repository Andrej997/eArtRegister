using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
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
        public Guid Id { get; set; }

        public GetBundleQuery(Guid id)
        {
            Id = id;
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
                .AsNoTracking()
                    .Where(t => t.Id == request.Id)
                    .ProjectTo<BundleDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefault();

            var user = _context.Users.Where(u => u.Id == bundle.OwnerId).FirstOrDefault();

            bundle.OwnerWallet = user.Wallet.ToLower();

            return bundle;
        }
    }
}
