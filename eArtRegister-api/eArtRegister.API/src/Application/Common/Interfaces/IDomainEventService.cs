using eArtRegister.API.Domain.Common;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
