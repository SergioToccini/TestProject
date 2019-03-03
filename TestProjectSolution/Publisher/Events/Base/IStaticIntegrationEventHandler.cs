using Publisher.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Events.Base
{
    public interface IStaticIntegrationEventHandler<in TIntegrationEvent> : IStaticIntegrationEventHandler where TIntegrationEvent : PublisherEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IStaticIntegrationEventHandler
    {
    }
}
