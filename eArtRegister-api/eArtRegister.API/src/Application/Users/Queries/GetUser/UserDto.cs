using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;
using System.Collections.Generic;

namespace eArtRegister.API.Application.Users.Queries.GetUser
{
    public class UserDto : IMapFrom<User>
    {
        public UserDto()
        {
            Roles = new List<UserRole>();
        }
        public Guid Id { get; set; }
        public string Wallet { get; set; }
        public string DepositContract { get; set; }
        public string DepositAddress { get; set; }
        public string DepositAbi { get; set; }
        public string DepositBytecode { get; set; }
        public DateTime? DepositCreated { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}
