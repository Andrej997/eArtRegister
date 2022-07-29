using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Queries.GetUserIds
{
    public class GetUserIdsQuery : IRequest<List<Guid>>
    {
    }
    public class GetUserIdsQueryHandler : IRequestHandler<GetUserIdsQuery, List<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserIdsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<Guid>> Handle(GetUserIdsQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return _context.Users.Select(t => t.Id).ToList();
        }
    }
}
