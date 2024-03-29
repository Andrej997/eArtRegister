﻿using AutoMapper;
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
        public string Search { get; set; }
        public bool Mine { get; set; }
        public string Wallet { get; set; }

        public GetBundlesQuery(bool mine = false, string wallet = null)
        {
            Mine = mine;
            Wallet = wallet;
        }

        public GetBundlesQuery(string search, bool mine = false, string wallet = null)
        {
            Search = search;
            Mine = mine;
            Wallet = wallet;
        }
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
            var user = _context.SystemUsers.Where(x => x.Wallet.ToLower() == request.Wallet).FirstOrDefault();
            var bundlesQuery = _context.Bundles
                    .Include(x => x.Owner)
                    .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
                bundlesQuery = bundlesQuery.Where(t => 
                    t.Name.ToLower().Contains(request.Search.ToLower()) || 
                    t.Symbol.ToLower().Contains(request.Search.ToLower()) ||
                    t.Description.ToLower().Contains(request.Search.ToLower()) ||
                    t.CustomRoot.ToLower().Contains(request.Search.ToLower()) ||
                    t.Address.ToLower().Contains(request.Search.ToLower()) ||
                    t.Owner.Wallet.ToLower().Contains(request.Search.ToLower()));

            if (request.Mine)
                bundlesQuery = bundlesQuery.Where(t => t.OwnerId == user.Id);

            var bundles = await bundlesQuery
                    .ProjectTo<BundleDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            return bundles;
        }
    }
}
