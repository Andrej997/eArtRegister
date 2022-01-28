using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        DbSet<eArtRegister.API.Domain.Entities.JobSpecification> JobSpecifications { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
