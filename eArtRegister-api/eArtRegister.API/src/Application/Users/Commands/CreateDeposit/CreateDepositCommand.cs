using AutoMapper;
using eArtRegister.API.Application.Bundles.Commands.CreateBundle;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using RestSharp;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.CreateDeposit
{
    public class CreateDepositCommand : IRequest
    {
        public string Wallet { get; set; }
    }
    public class CreateDepositCommandHandler : IRequestHandler<CreateDepositCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly IMapper _mapper;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;

        public CreateDepositCommandHandler(IApplicationDbContext context,
                                               IDateTime dateTime,
                                               ICurrentUserService currentUserService,
                                               IIPFSFile ipfs,
                                               INethereumBC nethereum,
                                               IMapper mapper,
                                               IEtherscan etherscan)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _mapper = mapper;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<Unit> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            var client = new RestClient($"http://localhost:3000/deposit");
            client.Timeout = -1;
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(new DepositBody(request.Wallet));
            IRestResponse restResponse = client.Execute(restRequest);
            var response = JsonSerializer.Deserialize<CreateContractResponse>(restResponse.Content);

            var user = _context.SystemUsers.Where(u => u.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();
            user.DepositContract = response.contract;
            user.DepositAbi = response.abi;
            user.DepositBytecode = response.bytecode;
            user.DepositAddress = response.address;
            user.DepositCreated = _dateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class DepositBody
    {
        public string owner { get; set; }

        public DepositBody(string owner)
        {
            this.owner = owner;
        }
    }
}
