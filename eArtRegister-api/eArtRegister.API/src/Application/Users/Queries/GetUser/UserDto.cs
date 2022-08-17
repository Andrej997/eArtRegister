using System.Collections.Generic;

namespace eArtRegister.API.Application.Users.Queries.GetUser
{
    public class UserDto
    {
        public List<long> RoleIds { get; set; }
        public string Wallet { get; set; }
        public long WalletBalance { get; set; }
        public string DepositContract { get; set; }
        public long DepositBalance { get; set; }
        public long ServerBalance { get; set; }
    }
}
