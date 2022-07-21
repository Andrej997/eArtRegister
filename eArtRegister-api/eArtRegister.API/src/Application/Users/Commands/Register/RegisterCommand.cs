using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.Users.Commands.Login;
using eArtRegister.API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.Register
{
    public class RegisterCommand : IRequest<UserDto>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly IApplicationDbContext _context;

        public RegisterCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                if (request.Username == null || request.Username == "") throw new Exception("Missing username");
                if (_context.Users.Any(user => user.Username == request.Username)) throw new Exception("Username taken");
                if (request.Password == null || request.Password == "") throw new Exception("Missing password");
                if (request.Name == null || request.Name == "") throw new Exception("Missing name");
                if (request.Surname == null || request.Surname == "") throw new Exception("Missing surname");

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var userDb = _context.Users
                    .Add(new User
                    {
                        Username = request.Username,
                        Name = request.Name,
                        Surname = request.Surname,
                        Password = passwordHash
                    });

                await _context.SaveChangesAsync(cancellationToken);

                _context.UserRoles
                    .Add(new UserRole
                    {
                        UserId = userDb.Entity.Id,
                        RoleId = _context.Roles.Where(role => role.Name == "user").Select(role => role.Id).FirstOrDefault()
                    });

                await _context.SaveChangesAsync(cancellationToken);

                return _context.Users
                    .Where(user => user.Id == userDb.Entity.Id)
                    .Select(user => new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Name = user.Name,
                        Surname = user.Surname,
                        Roles = _context.Roles.Where(role => _context.UserRoles.Any(ur => ur.UserId == user.Id && role.Id == ur.RoleId)).Select(role => role.Name).ToList()
                    })
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
