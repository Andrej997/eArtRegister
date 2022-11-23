using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Bundle> Bundles { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<FollowBundle> FollowBundles { get; set; }
        DbSet<FollowNFT> FollowNFTs { get; set; }
        DbSet<FollowUser> FollowUsers { get; set; }
        DbSet<NFTCategory> NFTCategories { get; set; }
        DbSet<NFTPurchase> NFTPurchases { get; set; }
        DbSet<NFT> NFTs { get; set; }
        DbSet<NFTLike> NFTLikes { get; set; }
        DbSet<NFTPriceHistory> NFTPriceHistory { get; set; }
        DbSet<NFTStatus> NFTStatuses { get; set; }
        DbSet<NFTActionHistory> ServerActionHistories { get; set; }
        DbSet<Role> SystemRoles { get; set; }
        DbSet<User> SystemUsers { get; set; }
        DbSet<UserPortalNotification> UserPortalNotifications { get; set; }
        DbSet<UserRole> SystemUserRoles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
