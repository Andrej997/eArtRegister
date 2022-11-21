using System;

namespace eArtRegister.API.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
