using Microsoft.Extensions.DependencyInjection;
using NethereumAccess.Interfaces;
using NethereumAccess.Services;

namespace NethereumAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNethereumBlockchain(this IServiceCollection services)
        {
            services.AddTransient<INethereumBC, NethereumBC>();

            return services;
        }
    }
}
