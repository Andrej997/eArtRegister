using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Bundles.Commands.CreateBundle
{
    public class CreateBundleCommand : IRequest<RetBundle>
    {
        public string CustomRoute { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Wallet { get; set; }
    }

    public class CreateBundleCommandHandler : IRequestHandler<CreateBundleCommand, RetBundle>
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

        public async Task<RetBundle> Handle(CreateBundleCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();

            if (_context.Bundles.Any(t => t.CustomRoot == request.CustomRoute))
                throw new Exception("That custom root is taken");

            if (_context.Bundles.Any(t => t.Name == request.Name && t.Symbol == request.Symbol))
                throw new Exception("Combination of name and symbol is already been taken");

            var client = new RestClient($"http://localhost:3000/erc721");
            client.Timeout = -1;
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(new ERC721Body(request.Wallet, request.Name, request.Symbol));
            IRestResponse restResponse = client.Execute(restRequest);
            var response = JsonSerializer.Deserialize<ERC721Response>(restResponse.Content);

            var entry = new Bundle
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = user.Id,
                CustomRoot = request.CustomRoute,
                Symbol = request.Symbol,
                Abi = response.abi,
                Bytecode =response.bytecode,
                Address = response.address,
                Contract = response.contract
            };

            _context.Bundles.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return new RetBundle
            {
                CustomRoute = entry.CustomRoot,
            }; 
        }
    }

    public class RetBundle
    {
        public string CustomRoute { get; set; }
    }

    public class ERC721Body
    {
        public ERC721Body(string owner, string bundleName, string bundleSymbol)
        {
            this.owner = owner;
            this.bundleName = bundleName;
            this.bundleSymbol = bundleSymbol;
        }

        public string owner { get; set; }
        public string bundleName { get; set; }
        public string bundleSymbol { get; set; }
    }

    public class ERC721Response
    {
        public string abi { get; set; }
        public string bytecode { get; set; }
        public string address { get; set; }
        public string contract { get; set; }
    }
}
