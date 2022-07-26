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
    }

    public class CreateBundleCommandHandler : IRequestHandler<CreateBundleCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateBundleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateBundleCommand request, CancellationToken cancellationToken)
        {
            if (_context.Bundles.Any(t => t.Name == request.Name && t.OwnerId == _currentUserService.UserId))
                throw new Exception("Name is already taken");

            var entry = new Bundle
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = _currentUserService.UserId,
                Order = _context.Bundles.Where(t => t.OwnerId == _currentUserService.UserId).Count() + 1,
                IsObservable = false,
                IsDeleted = false
            };

            _context.Bundles.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }
    }
}
