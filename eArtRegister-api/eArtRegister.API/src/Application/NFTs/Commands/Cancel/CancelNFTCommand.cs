﻿using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.Cancel
{
    public class CancelNFTCommand : IRequest
    {
        public Guid NFTId { get; set; }
        public string Wallet { get; set; }
        public string TransactionHash { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class CancelNFTCommandHandler : IRequestHandler<CancelNFTCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public CancelNFTCommandHandler(IApplicationDbContext context,
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

        public async Task<Unit> Handle(CancelNFTCommand request, CancellationToken cancellationToken)
        {
            var nft = _context.NFTs.Where(nft => nft.Id == request.NFTId).FirstOrDefault();

            _context.ServerActionHistories.Add(new NFTActionHistory
            {
                EventTimestamp = _dateTime.UtcNow.Ticks,
                TransactionHash = request.TransactionHash,
                Wallet = request.Wallet,
                IsCompleted = request.IsCompleted,
                EventAction = request.IsCompleted ? Domain.Enums.EventAction.NFT_SALE_CANCELED : Domain.Enums.EventAction.NFT_SALE_CANCELED_FAIL,
                NFTId = request.NFTId
            });
            if (request.IsCompleted)
                nft.StatusId = Domain.Enums.NFTStatus.Canceled;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
