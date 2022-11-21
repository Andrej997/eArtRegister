using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using eArtRegister.API.Application.Common.Interfaces;
using System;

namespace eArtRegister.API.WebApi.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => Guid.Parse("a0c7dcfa-cd69-4a14-bf6f-f004165f4a53");// Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
