using eArtRegister.API.Application.Common.Interfaces;
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

            nft.StatusId = Domain.Enums.NFTStatus.WaitingForApproval;

            var bundle = _context.Bundles.Find(nft.BundleId);

            var purchaseContractTransaction = await _nethereum.CreatePurchaseContract();
            var transaction = await _etherscan.GetTransactionStatus(purchaseContractTransaction.TransactionHash, cancellationToken);
            
            nft.PurchaseContract = purchaseContractTransaction.ContractAddress;

            var user = _context.Users.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();
            var withdraw = await _nethereum.WithdrawDepositContract(user.DepositContract);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
