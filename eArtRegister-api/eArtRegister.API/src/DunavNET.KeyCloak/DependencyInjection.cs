using KeyCloak.Interfaces;
using KeyCloak.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KeyCloak
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKeyCloak(this IServiceCollection services)
        {
            services.AddTransient<IKeyCloakUsers, KeyCloakUsers>();
            services.AddTransient<IKeyCloakRoles, KeyCloakRoles>();
            services.AddTransient<IKeyCloakClients, KeyCloakClients>();
            services.AddTransient<IKeyCloakEvents, KeyCloakEvents>();
            services.AddTransient<IKeyCloakGroups, KeyCloakGroups>();

            return services;
        }
    }
}
