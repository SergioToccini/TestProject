using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Events.Base
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
