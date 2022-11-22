using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.Common.Models;
using eArtRegister.API.Domain.Entities;
using Etherscan.Interfaces;
using IPFS.Interfaces;
using MediatR;
using NethereumAccess.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.NFTs.Commands.AddNFT
{
    public class AddNFTData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BundleId { get; set; }
        public string Wallet { get; set; }
        public string ExternalUrl { get; set; }
        public List<string> AttributeKeys { get; set; }
        public List<string> AttributeValues { get; set; }
    }

    public class AddNFTCommand : IRequest<Guid>
    {
        public UploadedFileModel File { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BundleId { get; set; }
        public string ExternalUrl { get; set; }
        public string Wallet { get; set; }
        public List<string> AttributeKeys { get; set; }
        public List<string> AttributeValues { get; set; }
    }
    public class AddNFTCommandHandler : IRequestHandler<AddNFTCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIPFSFile _ipfs;
        private readonly INethereumBC _nethereum;
        private readonly IEtherscan _etherscan;

        public AddNFTCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUserService, IIPFSFile ipfs, INethereumBC nethereum, IEtherscan etherscan)
        {
            _context = context;
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            _ipfs = ipfs;
            _nethereum = nethereum;
            _etherscan = etherscan;
        }

        public async Task<Guid> Handle(AddNFTCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Where(x => x.Wallet == request.Wallet.ToLower()).FirstOrDefault();

            var bundle = _context.Bundles.Where(x => x.Id == request.BundleId).FirstOrDefault();

            var retVal = await _ipfs.UploadAsync(request.File.Title + request.File.Extension, request.File.Content, cancellationToken);

            var attributes = new List<NFTAttribute>(request.AttributeKeys.Count);
            for (int i = 0; i < request.AttributeKeys.Count; i++)
            {
                attributes.Add(new NFTAttribute
                {
                    TraitType = request.AttributeKeys[i],
                    Value = request.AttributeValues[i]
                });
            }

            var nftData = new NFTData
            {
                Description = request.Description,
                Name = request.Name,
                ExternalUrl = request.ExternalUrl,
                Image = "http://localhost:8080/ipfs/" + retVal.Hash,
                Attributes = attributes
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(nftData);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            var retValData = await _ipfs.UploadAsync(request.Name + ".json", stream, cancellationToken);

            var client = new RestClient($"http://localhost:3000/safeMint");
            client.Timeout = -1;
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddJsonBody(new SafeMintBody(bundle.Abi, bundle.Address, request.Wallet, "ipfs://" + retVal.Hash));
            IRestResponse restResponse = client.Execute(restRequest);
            var response = JsonSerializer.Deserialize<SafeMintResponse>(restResponse.Content);

            var transaction = await _etherscan.GetTransactionStatus(response.transactionHash, cancellationToken);
            if (transaction.IsError == true)
            {
                throw new Exception("NFT not minted!");
            }

            long tokenId = _context.NFTs.Where(t => t.BundleId == request.BundleId).Count();

            // TODO: save json file hash
            var entry = new NFT
            {
                IPFSId = retVal.Hash,
                Name = request.Name,
                Description = request.Description,
                TokenId = tokenId,
                BundleId = request.BundleId,
                StatusId = Domain.Enums.NFTStatus.Minted,
                CreatorId = user.Id,
                MintedAt = _dateTime.UtcNow,
                TransactionHash = response.transactionHash,
            };

            _context.NFTs.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }
    }
    public class NFTData
    {
        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty("external_url")]
        public string ExternalUrl { get; set; }
        [Newtonsoft.Json.JsonProperty("image")]
        public string Image { get; set; }
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("attributes")]
        public List<NFTAttribute> Attributes { get; set; }
    }
    public class NFTAttribute 
    {
        [Newtonsoft.Json.JsonProperty("trait_type")]
        public string TraitType { get; set; }
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }
    }

    public class SafeMintBody
    {
        public SafeMintBody(string abi, string address, string to, string uri)
        {
            this.abi = abi;
            this.address = address;
            this.to = to;
            this.uri = uri;
        }

        public string abi { get; set; }
        public string address { get; set; }
        public string to { get; set; }
        public string uri { get; set; }
    }
    public class SafeMintResponse
    {
        public string transactionHash { get; set; }
    }
}
