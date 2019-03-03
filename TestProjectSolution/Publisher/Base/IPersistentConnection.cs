using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Base
{
    public interface IPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
