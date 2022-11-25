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
        public string CustomRouth { get; set; }
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
        public string CustomRouth { get; set; }
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
            await ValidateRequest(request);
            var user = _context.SystemUsers.Where(x => x.Wallet.ToLower() == request.Wallet.ToLower()).FirstOrDefault();

            var bundle = _context.Bundles.Where(x => x.CustomRoot == request.CustomRouth).FirstOrDefault();

            var retVal = await _ipfs.UploadAsync(request.File.Title + request.File.Extension, request.File.Content, cancellationToken);

            var attributes = new List<NFTAttribute>();
            if (request.AttributeKeys != null && request.AttributeKeys.Any())
            {
                attributes = new List<NFTAttribute>(request.AttributeKeys.Count);
                for (int i = 0; i < request.AttributeKeys.Count; i++)
                {
                    attributes.Add(new NFTAttribute
                    {
                        TraitType = request.AttributeKeys[i],
                        Value = request.AttributeValues[i]
                    });
                }
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
            var response = JsonSerializer.Deserialize<ActionResponse>(restResponse.Content);

            var transaction = await _etherscan.GetTransactionStatus(response.transactionHash, cancellationToken);
            if (transaction.IsError == true)
            {
                // TODO: delete from IPFS, both files
                throw new Exception("NFT not minted!");
            }

            long tokenId = _context.NFTs.Where(t => t.BundleId == bundle.Id).Count();

            var entry = new NFT
            {
                IPFSImageHash = retVal.Hash,
                TokenId = tokenId,
                BundleId = bundle.Id,
                StatusId = Domain.Enums.NFTStatus.Minted,
                TransactionHash = response.transactionHash,
                IPFSNFTHash = retValData.Hash,
                IPFSImageSize = retVal.Size,
                IPFSNFTSize = retValData.Size,
                TokenStandard = "ERC721"
            };

            _context.NFTs.Add(entry);

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Id;
        }

        private async Task ValidateRequest(AddNFTCommand request)
        {
            if (request == null) throw new Exception("Missing NFT data!");

            if (request.AttributeKeys?.Count != request.AttributeValues?.Count) throw new Exception("Invalid NFT attribute data!");

            if (request.AttributeKeys?.Distinct().Count() != request.AttributeKeys?.Count()) throw new Exception("Invalid NFT attribute data!");

            if (string.IsNullOrEmpty(request.Name)) throw new Exception("Missing name of NFT!");

            if (request.AttributeKeys != null && request.AttributeKeys.Any())
                foreach (var key in request.AttributeKeys)
                {
                    if (string.IsNullOrEmpty(key)) throw new Exception("Invalid NFT attribute data!");
                }

            if (request.AttributeValues != null && request.AttributeValues.Any())
                foreach (var value in request.AttributeValues)
                {
                    if (string.IsNullOrEmpty(value)) throw new Exception("Invalid NFT attribute data!");
                }
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
    public class ActionResponse
    {
        public string transactionHash { get; set; }
    }
}
