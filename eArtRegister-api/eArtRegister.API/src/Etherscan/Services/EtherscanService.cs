using Etherscan.Common;
using Etherscan.Interfaces;
using Etherscan.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Etherscan.Services
{
    public class EtherscanService : IEtherscan
    {
        private readonly EtherscanConfig _config;

        public EtherscanService(IOptions<EtherscanConfig> settings)
        {
            _config = settings.Value;
        }

        public async Task<long> GetBalance(string wallet, CancellationToken cancellationToken)
        {
            var client2 = new RestClient($"{_config.NetworkUrl}?module=account&action=balance&address={wallet}&tag=latest&apikey={_config.ApiKey}");
            client2.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client2.Execute(request);
            var jObject = JObject.Parse(response.Content);
            return long.Parse(jObject["result"]?.Value<string>());

        }

        public async Task<RetVal> GetTransactionStatus(string transactionHex, CancellationToken cancellationToken)
        {
            var client2 = new RestClient($"{_config.NetworkUrl}?module=transaction&action=getstatus&txhash={transactionHex}&apikey={_config.ApiKey}");
            client2.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client2.Execute(request);
            var jObject = JObject.Parse(response.Content);
            var isError = false;
            if (jObject["result"]?["isError"]?.Value<string>() == "1")
                isError = true;

            return new RetVal
            {
                IsError = isError ? isError : false,
                ErrDescription = jObject["result"]?["errDescription"]?.Value<string>()
            };
        }
    }
}
