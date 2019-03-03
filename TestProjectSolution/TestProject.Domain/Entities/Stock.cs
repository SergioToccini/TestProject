using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Domain.Entities.Base;

namespace TestProject.Entities
{
    public class Stock : BaseEntity<Guid>
    {
        public int BackstoreAmount { get; set; }
        public int FrontstoreAmount { get; set; }
        public int ShoppingWindowAmount { get; set; }
        public double StockAccuracy { get; set; }
        public double OnFloorAvailability { get; set; }
        public int StockMeanAgeInDays { get; set; }

    }
}
