using AutoMapper;
using eArtRegister.API.Application.Common.Interfaces;
using eArtRegister.API.Application.Users.Commands.Login;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eArtRegister.API.Application.Users.Queries.Search
{
    public class SearchQuerry : IRequest<List<UserDto>>
    {
        public string InputSearch { get; set; }
    }

    public class SearchQuerryHandler : IRequestHandler<SearchQuerry, List<UserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchQuerryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<UserDto>> Handle(SearchQuerry request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var user = _context.Users
                    .Where(user => user.Username.Contains(request.InputSearch) 
                        && _context.Roles.Any(role => _context.UserRoles.Any(ur => ur.UserId == user.Id && role.Name != "user" && role.Id == ur.RoleId)))
                    .Select(user => new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Name = user.Name,
                        Surname = user.Surname,
                        Roles = _context.Roles.Where(role => _context.UserRoles.Any(ur => ur.UserId == user.Id && role.Id == ur.RoleId)).Select(role => role.Name).ToList()
                    })
                    .ToList();

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
