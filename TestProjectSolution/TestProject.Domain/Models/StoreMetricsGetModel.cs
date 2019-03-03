using TestProject.Domain.Models.Metrics;

namespace TestProject.Domain.Models
{
    public class StoreMetricsGetModel
    {
        public StoreMetricsGetModel()
        {
            MaxTotalStock = new StoreMetric();
            MaxStockAccuracy = new StoreMetric();
            MaxOnFloorAvailability = new StoreMetric();
            MaxStockMeanAge = new StoreMetric();
            MinTotalStock = new StoreMetric();
            MinStockAccuracy = new StoreMetric();
            MinOnFloorAvailability = new StoreMetric();
            MinStockMeanAge = new StoreMetric();
            AvgTotalStock = new StoreMetric();
            AvgStockAccuracy = new StoreMetric();
            AvgOnFloorAvailability = new StoreMetric();
            AvgStockMeanAge = new StoreMetric();
        }

        public StoreMetric MaxTotalStock { get; set; }
        public StoreMetric MaxStockAccuracy { get; set; }
        public StoreMetric MaxOnFloorAvailability { get; set; }
        public StoreMetric MaxStockMeanAge { get; set; }
        public StoreMetric MinTotalStock { get; set; }
        public StoreMetric MinStockAccuracy { get; set; }
        public StoreMetric MinOnFloorAvailability { get; set; }
        public StoreMetric MinStockMeanAge { get; set; }
        public StoreMetric AvgTotalStock { get; set; }
        public StoreMetric AvgStockAccuracy { get; set; }
        public StoreMetric AvgOnFloorAvailability { get; set; }
        public StoreMetric AvgStockMeanAge { get; set; }
    }
}
