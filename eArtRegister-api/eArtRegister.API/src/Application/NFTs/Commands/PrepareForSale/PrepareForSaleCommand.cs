using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.PrepareForSale
{
    public class PrepareForSaleCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Wallet { get; set; }
    }
    public class PrepareForSaleCommandHandler : IRequestHandler<PrepareForSaleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;

        public PrepareForSaleCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, INethereumBC nethereum, IEtherscan etherscan)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<Unit> Handle(PrepareForSaleCommand request, CancellationToken cancellationToken)
        {
            var nft = _context.NFTs.Find(request.Id);
            if (nft == null)
                throw new Exception("Unknown NFT");

            

            var bundle = _context.Bundles.Find(nft.BundleId);

            var user = _context.SystemUsers.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();
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

            var purchaseContractTransaction = await _nethereum.CreatePurchaseContract(bundle.ContractAddress, nft.TokenId);
            var transaction = await _etherscan.GetTransactionStatus(purchaseContractTransaction.TransactionHash, cancellationToken);
            if (transaction.IsError == false)
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = purchaseContractTransaction.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = true,
                    EventAction = Domain.Enums.EventAction.PURCHASE_CONTRACT_CREATED,
                    NFTId = request.Id
                });
            }
            else
            {
                _context.ServerActionHistories.Add(new NFTActionHistory
                {
                    EventTimestamp = _dateTime.UtcNow.Ticks,
                    TransactionHash = purchaseContractTransaction.TransactionHash,
                    Wallet = request.Wallet,
                    IsCompleted = false,
                    EventAction = Domain.Enums.EventAction.PURCHASE_CONTRACT_CREATED_FAIL,
                    NFTId = request.Id
                });
                throw new Exception("Failed to create purchase contract!");
            }

            if (transaction.IsError == false)
            {
                nft.PurchaseContract = purchaseContractTransaction.ContractAddress;
                nft.StatusId = Domain.Enums.NFTStatus.WaitingForApproval;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
