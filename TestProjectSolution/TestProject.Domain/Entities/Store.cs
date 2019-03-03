using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Domain.Entities.Base;

namespace TestProject.Entities
{
    public class Store : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Category { get; set; }

        public StoreManager StoreManager { get; set; }
        public Guid StoreManagerId { get; set; }

        public Country Country { get; set; }
        public Guid CountryId { get; set; }

        public Stock Stock { get; set; }
        public Guid StockId { get; set; }
    }
}
