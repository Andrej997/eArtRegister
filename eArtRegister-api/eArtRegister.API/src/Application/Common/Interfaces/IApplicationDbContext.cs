using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<Role> Roles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
