using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Events
{
    public class PublisherEvent
    {
        public Guid EventId { get; } = new Guid();
        public DateTime EventDateTime { get; } = DateTime.Now;
        public object Dto { get; set; }
    }
}
