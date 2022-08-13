using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using MediatR;
using NethereumAccess.Interfaces;
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
        public string Wallet { get; set; }
    }

    public class CreateBundleCommandHandler : IRequestHandler<CreateBundleCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INethereumBC _nethereum;

        public CreateBundleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, INethereumBC nethereum)
        {
            _context = context;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
        }

        public async Task<Guid> Handle(CreateBundleCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();
            if (_context.Bundles.Any(t => t.Name == request.Name && t.OwnerId == _currentUserService.UserId))
                throw new Exception("Name is already taken");

            var transactionReceipt = await _nethereum.CreateContact(request.Name);

            var entry = new Bundle
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = user.Id,
                Order = _context.Bundles.Where(t => t.OwnerId == user.Id).Count() + 1,
                IsObservable = false,
                ContractAddress = transactionReceipt.ContractAddress,
                From = transactionReceipt.From,
                TransactionHash = transactionReceipt.TransactionHash,
                BlockHash = transactionReceipt.BlockHash,
                IsDeleted = false
            };

            _context.Bundles.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }
    }
}
