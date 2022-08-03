using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.RegisterNewUsers
{
    public class RegisterNewUsersCommand : IRequest
    {
        public List<Guid> Ids { get; set; }
    }
    public class RequestRolePermissionCommandHandler : IRequestHandler<RegisterNewUsersCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RequestRolePermissionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(RegisterNewUsersCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                _context.Users.Add(new Domain.Entities.User
                {
                    Id = id,
                    IsDeleted = false
                });

                _context.UserRoles.Add(new Domain.Entities.UserRole
                {
                    UserId = id,
                    RoleId = (long)Domain.Enums.Role.Observer
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
