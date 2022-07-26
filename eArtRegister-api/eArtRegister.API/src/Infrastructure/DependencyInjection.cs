using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Infrastructure.Files;
using eArtRegister.API.Infrastructure.Persistence;
using eArtRegister.API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Laraue.EfCoreTriggers.PostgreSql.Extensions;

namespace eArtRegister.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("eArtRegister.APIDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(
                            configuration.GetConnectionString("DefaultConnection"),
                            b =>
                            {
                                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                                b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            }).UseSnakeCaseNamingConvention();
                    options.UsePostgreSqlTriggers();
                });
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            return services;
        }
    }
}