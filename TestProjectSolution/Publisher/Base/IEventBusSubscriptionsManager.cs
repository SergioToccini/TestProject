using Publisher.Events;
using Publisher.Events.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Base
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;
        void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

        void AddStaticSubscription<T, TH>()
           where T : PublisherEvent
           where TH : IStaticIntegrationEventHandler<T>;

        void RemoveStaticSubscription<T, TH>()
             where TH : IStaticIntegrationEventHandler<T>
             where T : PublisherEvent;

        bool HasSubscriptionsForEvent<T>() where T : PublisherEvent;
        bool HasSubscriptionsForEvent(string eventName);

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : PublisherEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        Type GetEventTypeByName(string eventName);
        string GetEventKey<T>();

        void Clear();
    }
}
