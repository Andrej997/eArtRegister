using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eArtRegister.API.Domain.Common;

namespace eArtRegister.API.Domain.Entities
{
    public class JobSpecification : AuditableEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

    }
}
