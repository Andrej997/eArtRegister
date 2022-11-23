using AutoMapper;
using eArtRegister.API.Application.Bundles.Commands.CreateBundle;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using IPFS.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NethereumAccess.Interfaces;
using RestSharp;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.SetOnSale
{
    public class SetOnSaleCommand : IRequest
    {
        public Guid NFTId { get; set; }
        public bool EntireAmount { get; set; }
        public bool RepaymentInInstallments { get; set; }
        public bool Auction { get; set; }
        public double AmountInETH { get; set; }
        public int DaysOnSale { get; set; }
        public double MinParticipation { get; set; }
        public string Wallet { get; set; }
    }
    public class SetOnSaleCommandHandler : IRequestHandler<SetOnSaleCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public SetOnSaleCommandHandler(
                IApplicationDbContext context,
                IDateTime dateTime,
                ICurrentUserService currentUserService,
                IIPFSFile ipfs,
                INethereumBC nethereum,
                IMapper mapper)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _mapper = mapper;
            _nethereum = nethereum;
        }

        public async Task<Unit> Handle(SetOnSaleCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();

            var nft = _context.NFTs
                .Include(nft => nft.Bundle)
                .Where(nft => nft.Id == request.NFTId)
                .FirstOrDefault();

            var client = new RestClient($"http://localhost:3000/purchase");
            client.Timeout = -1;
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(new PurchaseBody(nft.Bundle.Address, nft.TokenId, request.EntireAmount, request.RepaymentInInstallments, request.Auction));
            IRestResponse restResponse = client.Execute(restRequest);
            var response = JsonSerializer.Deserialize<CreateContractResponse>(restResponse.Content);

            _context.NFTPurchases.Add(new NFTPurchase
            {
                NFTId = request.NFTId,
                DaysOnSale = request.DaysOnSale,
                EntireAmount = request.EntireAmount,
                Auction = request.Auction,
                AmountInETH = request.AmountInETH,
                MinParticipation = request.MinParticipation,
                RepaymentInInstallments = request.RepaymentInInstallments,
                Abi = response.abi,
                Bytecode = response.bytecode,
                Address = response.address,
                Contract = response.contract
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class PurchaseBody
    {
        public PurchaseBody(string erc721Address, long tokenId, bool entireAmount, bool repaymentInInstallments, bool auction)
        {
            this.erc721Address = erc721Address;
            this.tokenId = tokenId;
            this.entireAmount = entireAmount;
            this.repaymentInInstallments = repaymentInInstallments;
            this.auction = auction;
        }

        public string erc721Address { get; set; }
        public long tokenId { get; set; }
        public bool entireAmount { get; set; }
        public bool repaymentInInstallments { get; set; }
        public bool auction { get; set; }
    }
}
