using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.ChangeStatus
{
    public class ChangeStatusCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Wallet { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<ChangeStatusCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly INethereumBC _nethereum;

        public AddNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, INethereumBC nethereum)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _nethereum = nethereum;
        }

        public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var nft = _context.NFTs.Find(request.Id);
            if (nft == null)
                throw new Exception("Unknown NFT");

            nft.StatusId = Domain.Enums.NFTStatus.Pending;

            await _context.SaveChangesAsync(cancellationToken);

            var user = _context.Users.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();
            user.ServerBalance = user.ServerBalance - 1000000000000000;

            var bundle = _context.Bundles.Find(nft.BundleId);

            var purchaseContractTransaction = await _nethereum.CreatePurchaseContract();
            Thread.Sleep(10);
            var approvedTransaction = await _nethereum.ApprovePurchaseContract(bundle.ContractAddress, purchaseContractTransaction.ContractAddress);
            Thread.Sleep(10);

            nft.PurchaseContract = purchaseContractTransaction.ContractAddress;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
