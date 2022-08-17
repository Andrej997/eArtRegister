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

namespace eArtRegister.API.Application.NFTs.Commands.GetNFTsByByndleId
{
    public class GetNFTsByByndleIdCommand : IRequest<List<NFTDto>>
    {
        public Guid BundleId { get; set; }
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
            var bundle = _context.Bundles.Find(request.BundleId);

            var user = _context.Users.Where(u => u.Id == bundle.OwnerId).FirstOrDefault();
            var ipfsIds = await _nethereum.IPFSIds(_context.Bundles.Where(bundle => bundle.Id == request.BundleId).Select(bundle => bundle.ContractAddress).First());

            var ret = await _context.NFTs
                    .AsNoTracking()
                    .Where(t => t.BundleId == request.BundleId && ipfsIds.Contains("ipfs://" + t.IPFSId))
                    .OrderBy(t => t.TokenId)
                    .ProjectTo<NFTDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            var totalBigHex = await _nethereum.TotalSupply(bundle.ContractAddress);
            long totalNFTs = Convert.ToInt64(totalBigHex.ToString(), 16);

            for (long i = 0; i < totalNFTs; i++)
            {
                var wallet = await _nethereum.OwnerOf(bundle.ContractAddress, i);
                ret.ForEach(r => {
                    if (r.TokenId == i)
                        r.CurrentWallet = wallet.ToLower();

                });
                //var uri = await _nethereum.TokenUri(bundle.ContractAddress, i);
            }

            foreach (var item in ret)
            {
                item.Bytes = await _ipfs.DownloadAsync(item.IPFSId, cancellationToken);
                item.BundleWalletOwner = user.Wallet;
                item.ERC721ContractAddress = bundle.ContractAddress;
            }

            return ret;
        }
    }
}
