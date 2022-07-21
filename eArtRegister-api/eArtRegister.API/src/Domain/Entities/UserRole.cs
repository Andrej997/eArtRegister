using System;

namespace eArtRegister.API.Domain.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }

        public long RoleId { get; set; }


        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
