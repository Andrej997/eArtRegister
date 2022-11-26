using AutoMapper;
using eArtRegister.API.Application.Bundles.Commands.CreateBundle;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.NFTs.Commands.AddNFT;
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

namespace eArtRegister.API.Application.NFTs.Commands.CreatePurchase
{
    public class CreatePurchaseCommand : IRequest
    {
        public string CustomRouth { get; set; }
        public long TokenId { get; set; }
        public bool EntireAmount { get; set; }
        public bool RepaymentInInstallments { get; set; }
        public bool Auction { get; set; }
        public string Wallet { get; set; }
    }
    public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;

        public CreatePurchaseCommandHandler(
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

        public async Task<Unit> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            if (!request.EntireAmount && !request.RepaymentInInstallments && !request.Auction) throw new Exception("You need to select at least one action");

            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();

            var bundle = _context.Bundles
                .Where(b => b.CustomRoot == request.CustomRouth)
                .FirstOrDefault();

            var nft = _context.NFTs
                .Include(nft => nft.Bundle)
                .Where(nft => nft.TokenId == request.TokenId && nft.Bundle.CustomRoot == request.CustomRouth)
                .FirstOrDefault();

            var client = new RestClient($"http://localhost:3000/purchase");
            client.Timeout = -1;
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(new PurchaseBody(nft.Bundle.Address, nft.TokenId, request.EntireAmount, request.RepaymentInInstallments, request.Auction, request.Wallet));
            IRestResponse restResponse = client.Execute(restRequest);
            var response = JsonSerializer.Deserialize<CreateContractResponse>(restResponse.Content);

            _context.NFTPurchases.Add(new NFTPurchase
            {
                NFTId = nft.Id,
                EntireAmount = request.EntireAmount,
                Auction = request.Auction,
                RepaymentInInstallments = request.RepaymentInInstallments,
                Abi = response.abi,
                Bytecode = response.bytecode,
                Address = response.address.ToLower(),
                Contract = response.contract,
                CreatedOn = _dateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class PurchaseBody
    {
        public PurchaseBody(string erc721Address, long tokenId, bool entireAmount, bool repaymentInInstallments, bool auction, string contractOwner)
        {
            this.erc721Address = erc721Address;
            this.tokenId = tokenId;
            this.entireAmount = entireAmount;
            this.repaymentInInstallments = repaymentInInstallments;
            this.auction = auction;
            this.contractOwner = contractOwner;
        }

        public string erc721Address { get; set; }
        public long tokenId { get; set; }
        public bool entireAmount { get; set; }
        public bool repaymentInInstallments { get; set; }
        public bool auction { get; set; }
        public string contractOwner { get; set; }
    }
    public class SetApprovalForAllBody
    {
        public SetApprovalForAllBody(string abi, string address, string operatorAddress)
        {
            this.abi = abi;
            this.address = address;
            this.operatorAddress = operatorAddress;
        }

        public string abi { get; set; }
        public string address { get; set; }
        public string operatorAddress { get; set; }
    }
    public class SetPriceBody
    {
        public SetPriceBody(string abi, string address, double amount, long daysOnSale, double participation)
        {
            this.abi = abi;
            this.address = address;
            this.amount = amount;
            this.daysOnSale = daysOnSale;
            this.participation = participation;
        }

        public string abi { get; set; }
        public string address { get; set; }
        public double amount { get; set; }
        public long daysOnSale { get; set; }
        public double participation { get; set; }
    }
}
