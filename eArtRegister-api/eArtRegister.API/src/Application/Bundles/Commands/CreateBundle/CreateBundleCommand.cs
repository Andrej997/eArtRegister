using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Bundles.Commands.CreateBundle
{
    public class CreateBundleCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
    }

    public class CreateBundleCommandHandler : IRequestHandler<CreateBundleCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateBundleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateBundleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.Bundles.Any(t => t.Name == request.Name && t.OwnerId == request.OwnerId))
                    throw new Exception("Name is already taken");

                var entry = new Bundle
                {
                    Name = request.Name,
                    Description = request.Description,
                    OwnerId = request.OwnerId,
                    Order = _context.Bundles.Where(t => t.OwnerId == request.OwnerId).Count() + 1,
                    IsObservable = false,
                    IsDeleted = false
                };

                _context.Bundles.Add(entry);

                await _context.SaveChangesAsync(cancellationToken);

                return entry.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
