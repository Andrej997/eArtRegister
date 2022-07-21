using System;

namespace eArtRegister.API.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public virtual bool IsDeleted { get; set; }
    }
}
