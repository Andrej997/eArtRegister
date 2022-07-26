using System;

namespace eArtRegister.API.Domain.Entities
{
    public class FollowUser
    {
        public Guid UserId { get; set; }
        public Guid FollowedUserId { get; set; }
        public DateTime FollowDate { get; set; }

        public virtual User User { get; set; }
        public virtual User FollowedUser { get; set; }
    }
}
