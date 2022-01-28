using eArtRegister.API.Application.Common.Interfaces;
using System;

namespace eArtRegister.API.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
