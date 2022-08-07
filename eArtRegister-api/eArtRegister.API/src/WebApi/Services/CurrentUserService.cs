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

        public Guid UserId => Guid.Parse("290839d8-0572-45c4-9622-2840d6d613c5");// Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
