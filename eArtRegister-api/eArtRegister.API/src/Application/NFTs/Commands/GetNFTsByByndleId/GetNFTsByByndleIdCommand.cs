using AutoMapper;
using AutoMapper.QueryableExtensions;
using eArtRegister.API.Application.Common.Interfaces;
using IPFS.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public GetNFTsByByndleIdCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, IIPFSFile ipfs, IMapper mapper)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _mapper = mapper;
        }

        public async Task<List<NFTDto>> Handle(GetNFTsByByndleIdCommand request, CancellationToken cancellationToken)
        {
            var ret = await _context.NFTs
                    .AsNoTracking()
                    .Where(t => t.BundleId == request.BundleId)
                    .OrderBy(t => t.Order)
                    .ProjectTo<NFTDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

            foreach (var item in ret)
            {
                item.Bytes = await _ipfs.DownloadAsync(item.IPFSId, cancellationToken);
            }

            return ret;
        }
    }
}
