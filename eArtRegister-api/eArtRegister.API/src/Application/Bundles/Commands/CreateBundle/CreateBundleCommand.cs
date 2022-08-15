using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
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
        private readonly IEtherscan _etherscan;

        public CreateBundleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, INethereumBC nethereum, IEtherscan etherscan)
        {
            _context = context;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<Guid> Handle(CreateBundleCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();
            if (_context.Bundles.Any(t => t.Name == request.Name && t.OwnerId == _currentUserService.UserId))
                throw new Exception("Name is already taken");

            var withdraw = await _nethereum.WithdrawDepositContract(user.DepositContract);

            var transactionReceipt = await _nethereum.CreateContact(request.Name);

            var transaction = await _etherscan.GetTransactionStatus(transactionReceipt.TransactionHash, cancellationToken);

            if (transaction.IsError == true)
                throw new Exception("Bundle not created!");

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
