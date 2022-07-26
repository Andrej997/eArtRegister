using System;

namespace eArtRegister.API.Domain.Entities
{
    public class UserPortalNotification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOfNotification { get; set; }
        public bool Seen { get; set; }
        public DateTime SeenDate { get; set; }

        public virtual User User { get; set; }
    }
}
