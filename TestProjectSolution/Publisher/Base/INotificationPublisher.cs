using Publisher.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Base
{
    public interface INotificationPublisher
    {
        void Publish(PublisherEvent @event);
    }
}
