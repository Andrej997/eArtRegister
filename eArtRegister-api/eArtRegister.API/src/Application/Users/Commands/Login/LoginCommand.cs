using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<UserDto>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var passwordHash = _context.Users
                    .Where(user => user.Username == request.Username)
                    .Select(user => user.Password)
                    .FirstOrDefault();

                if (passwordHash == null || passwordHash == "") throw new Exception("Unknown username or password");

                bool verified = BCrypt.Net.BCrypt.Verify(request.Password, passwordHash);
                if (verified == false) throw new Exception("Unknown username or password");

                var user = _context.Users
                    .Where(user => user.Username == request.Username)
                    .Select(user => new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Name = user.Name,
                        Surname = user.Surname,
                        Roles = _context.Roles.Where(role => _context.UserRoles.Any(ur => ur.UserId == user.Id && role.Id == ur.RoleId)).Select(role => role.Name).ToList()
                    })
                    .FirstOrDefault();

                if (user == null) throw new Exception("Unknown user");

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
