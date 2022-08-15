using Etherscan.Interfaces;
using Etherscan.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Etherscan
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEtherscan(this IServiceCollection services)
        {
            services.AddTransient<IEtherscan, EtherscanService>();

            return services;
        }
    }
}
