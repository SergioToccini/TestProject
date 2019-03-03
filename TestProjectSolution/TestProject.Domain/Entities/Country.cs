using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Domain.Entities.Base;

namespace TestProject.Entities
{
    public class Country : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
