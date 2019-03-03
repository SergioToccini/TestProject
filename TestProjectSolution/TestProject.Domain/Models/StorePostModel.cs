using System;

namespace TestProject.Domain.Models
{
    public class StorePostModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Category { get; set; }

        public Guid StoreManagerId { get; set; }
        public Guid CountryId { get; set; }
        public Guid StockId { get; set; }
    }
}
