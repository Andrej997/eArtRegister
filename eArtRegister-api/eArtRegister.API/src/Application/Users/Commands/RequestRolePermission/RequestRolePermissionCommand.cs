﻿using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.RequestRolePermission
{
    public class RequestRolePermissionCommand : IRequest
    {
        public long RoleId { get; set; }
    }
    public class RequestRolePermissionCommandHandler : IRequestHandler<RequestRolePermissionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RequestRolePermissionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(RequestRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var user = _context.SystemUsers.Find(_currentUserService.UserId);
            if (user == null)
                throw new Exception("Unknown user");

            if (request.RoleId < 1)
                throw new Exception("Unknown role");

            if (_context.SystemUserRoles.Any(t => t.UserId == _currentUserService.UserId && t.RoleId == request.RoleId))
                throw new Exception("You already have that role");

            if (request.RoleId == (long)Domain.Enums.Role.Administrator)
                throw new Exception("Only administrator can add you as administrator");
            //else if (request.RoleId == (long)Domain.Enums.Role.Buyer)
            //{
            //    if (string.IsNullOrEmpty(user.Wallet))
            //        throw new Exception("You need to register a wallet to be a buyer");
            //}
            //else if (request.RoleId == (long)Domain.Enums.Role.Seller)
            //{
            //    if (user.Wallets != null && user.Wallets.Any())
            //        throw new Exception("You need to register a wallet to be a seller");
            //}

            _context.SystemUserRoles.Add(new Domain.Entities.UserRole
            {
                UserId = _currentUserService.UserId,
                RoleId = request.RoleId
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
