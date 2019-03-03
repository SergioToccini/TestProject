using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Publisher.Base;
using Publisher.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Publisher
{
    public class NotificationPublisher : INotificationPublisher
    {
        public const string BROKER_NAME = "DetegoExchange";

        private IPersistentConnection _persistentConnection;

        private readonly int _retryCount;

        public NotificationPublisher(IPersistentConnection persistentConnection, int retryCount = 5)
        {
            _persistentConnection = persistentConnection;
            _retryCount = retryCount;
        }

        public void Publish(PublisherEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType()
                    .Name;

                var message = JsonConvert.SerializeObject(@event, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(exchange: BROKER_NAME,
                                     routingKey: eventName,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
                });
            }
        }
    }
}
