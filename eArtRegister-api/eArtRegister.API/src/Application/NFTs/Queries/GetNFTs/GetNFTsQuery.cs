using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.NFTs.Commands.GetNFTsByByndleId;
using IPFS.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NethereumAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Queries.GetNFTs
{
    public class GetNFTsQuery : IRequest<List<NFTDto>>
    {
        public string Wallet { get; set; }

        public GetNFTsQuery(string wallet)
        {
            Wallet = wallet;
        }
    }
    public class GetNFTsQueryHandler : IRequestHandler<GetNFTsQuery, List<NFTDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public GetNFTsQueryHandler(IApplicationDbContext context,
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

        public async Task<List<NFTDto>> Handle(GetNFTsQuery request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();

            var ret = await _context.NFTs
                    .AsNoTracking()
                    .Where(t => t.OwnerId == user.Id)
                    .OrderBy(t => t.TokenId)
                    .ProjectTo<NFTDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            foreach (var item in ret)
            {
                var bundle = _context.Bundles.Where(b => b.Id == item.BundleId).FirstOrDefault();
                item.Bytes = await _ipfs.DownloadAsync(item.IPFSId, cancellationToken);
                item.BundleWalletOwner = user.Wallet;
                item.ERC721ContractAddress = bundle.ContractAddress;
            }

            return ret;
        }
    }
}
