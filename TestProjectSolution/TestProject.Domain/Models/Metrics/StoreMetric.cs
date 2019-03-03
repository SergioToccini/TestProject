using System;

namespace TestProject.Domain.Models.Metrics
{
    public class StoreMetric
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public double MetricValue { get; set; }
    }
}
