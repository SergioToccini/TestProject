using System;

namespace TestProject.Domain.Models.Metrics
{
    public class StockMetric
    {
        public Guid StockId { get; set; }
        public double MetricValue { get; set; }
        public double Delta { get; set; }
    }
}
