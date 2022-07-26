using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Bundles.Commands.AddNFT
{
    public class AddNFTCommand : IRequest
    {
        public string NFTId { get; set; }
        public Guid BundleId { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<AddNFTCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddNFTCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddNFTCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_context.Bundles.Any(t => t.Id == request.BundleId))
                    throw new Exception("Unknown bundle");

                if (!_context.NFTs.Any(t => t.Id == request.NFTId))
                    throw new Exception("Unknown NFT");

                var entity = await _context.NFTs.FindAsync(request.NFTId);

                entity.BundleId = request.BundleId;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
