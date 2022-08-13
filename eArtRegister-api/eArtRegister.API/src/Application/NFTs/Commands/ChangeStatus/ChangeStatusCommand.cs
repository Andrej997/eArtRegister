using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.ChangeStatus
{
    public class ChangeStatusCommand : IRequest
    {
        public Guid Id { get; set; }
        public string StatusId { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<ChangeStatusCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;

        public AddNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var nft = _context.NFTs.Find(request.Id);
            if (nft == null)
                throw new Exception("Unknown NFT");

            nft.StatusId = request.StatusId;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
