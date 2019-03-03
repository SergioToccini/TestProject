namespace TestProject.Domain.Models
{
    public class StockPostModel
    {
        public int BackstoreAmount { get; set; }
        public int FrontstoreAmount { get; set; }
        public int ShoppingWindowAmount { get; set; }
        public double StockAccuracy { get; set; }
        public double OnFloorAvailability { get; set; }
        public int StockMeanAgeInDays { get; set; }
    }
}
