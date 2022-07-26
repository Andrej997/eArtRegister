using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Bundles.Commands.AddNFTToBundle
{
    public class AddNFTToBundleCommand : IRequest
    {
        public Guid NFTId { get; set; }
        public Guid BundleId { get; set; }
    }
    public class AddNFTToBundleCommandHandler : IRequestHandler<AddNFTToBundleCommand>
    {
        private readonly IApplicationDbContext _context;

        public AddNFTToBundleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddNFTToBundleCommand request, CancellationToken cancellationToken)
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
