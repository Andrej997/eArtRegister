using IPFS.Interfaces;
using IPFS.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IPFS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIPFS(this IServiceCollection services)
        {
            services.AddTransient<IIPFSFile, IPFSFile>();

            return services;
        }
    }
}
