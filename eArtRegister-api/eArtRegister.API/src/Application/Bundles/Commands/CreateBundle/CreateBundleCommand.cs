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
        private readonly IDateTime _dateTime;

        public CreateBundleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, INethereumBC nethereum, IEtherscan etherscan, IDateTime dateTime)
        {
            _context = context;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
            _etherscan = etherscan;
            _dateTime = dateTime;
        }

        public async Task<Guid> Handle(CreateBundleCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();
            if (_context.Bundles.Any(t => t.Name == request.Name && t.OwnerId == _currentUserService.UserId))
                throw new Exception("Name is already taken");

            var withdraw = await _nethereum.WithdrawDepositContract(user.DepositContract);
            var withdrawTransaction = await _etherscan.GetTransactionStatus(withdraw.TransactionHash, cancellationToken);
            if (withdrawTransaction.IsError == false)
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = withdraw.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = true,
                    EventAction = Domain.Enums.EventAction.WITHDRAW_FROM_DEPOSIT,
                });
            }
            else
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = withdraw.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = false,
                    EventAction = Domain.Enums.EventAction.WITHDRAW_FROM_DEPOSIT_FAIL,
                });
                throw new Exception("Failed to withdraw from deposit!");
            }

            var erc721ContractReceipt = await _nethereum.CreateContact(request.Name);

            var transaction = await _etherscan.GetTransactionStatus(erc721ContractReceipt.TransactionHash, cancellationToken);
            if (transaction.IsError == false)
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = erc721ContractReceipt.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = true,
                    EventAction = Domain.Enums.EventAction.BUNDLE_CREATED
                });
            }
            else
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = erc721ContractReceipt.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = false,
                    EventAction = Domain.Enums.EventAction.BUNDLE_CREATED_FAIL
                });

                throw new Exception("Bundle not created!");
            }

            var entry = new Bundle
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = user.Id,
                Order = _context.Bundles.Where(t => t.OwnerId == user.Id).Count() + 1,
                IsObservable = false,
                ContractAddress = erc721ContractReceipt.ContractAddress,
                From = erc721ContractReceipt.From,
                TransactionHash = erc721ContractReceipt.TransactionHash,
                BlockHash = erc721ContractReceipt.BlockHash,
                IsDeleted = false
            };

            _context.Bundles.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }
    }
}
