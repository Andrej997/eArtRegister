using Duende.IdentityServer.EntityFramework.Options;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Domain.Common;
using eArtRegister.API.Domain.Entities;
using eArtRegister.API.Infrastructure.Identity;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDomainEventService domainEventService,
            IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FollowBundle> FollowBundles { get; set; }
        public DbSet<FollowNFT> FollowNFTs { get; set; }
        public DbSet<FollowUser> FollowUsers { get; set; }
        public DbSet<NFTCategory> NFTCategories { get; set; }
        public DbSet<NFT> NFTs { get; set; }
        public DbSet<NFTLike> NFTLikes { get; set; }
        public DbSet<NFTPriceHistory> NFTPriceHistory { get; set; }
        public DbSet<NFTStatus> NFTStatuses { get; set; }
        public DbSet<NFTTransaction> NFTTransactions { get; set; }
        public DbSet<NFTSale> NFTSales { get; set; }
        public DbSet<NFTActionHistory> NFTActionHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDeposit> UserDeposits { get; set; }
        public DbSet<UserPortalNotification> UserPortalNotifications { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.ModifiedBy = _currentUserService.UserId.ToString();
                        entry.Entity.ModifiedOn = _dateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _currentUserService.UserId.ToString();
                        entry.Entity.ModifiedOn = _dateTime.UtcNow;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("uuid-ossp");

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .FirstOrDefault();
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}
