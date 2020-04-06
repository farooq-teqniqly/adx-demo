using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace AdxApp
{
    public class TelemetryEventProcessorFactory : IEventProcessorFactory
    {
        private readonly EventHandler<EventProcessorEventArgs> _eventProcessorOpened;
        private readonly EventHandler<EventProcessorEventArgs> _eventProcessorClosed;
        private readonly EventHandler<TelemetryEventProcessorEventArgs> _eventProcessorMessageReceived;

        public TelemetryEventProcessorFactory(
            EventHandler<EventProcessorEventArgs> eventProcessorOpened = null,
            EventHandler<EventProcessorEventArgs> eventProcessorClosed = null,
            EventHandler<TelemetryEventProcessorEventArgs> eventProcessorMessageReceived = null)
        {
            _eventProcessorOpened = eventProcessorOpened;
            _eventProcessorClosed = eventProcessorClosed;
            _eventProcessorMessageReceived = eventProcessorMessageReceived;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            var processor = new TelemetryEventProcessor();

            if (_eventProcessorClosed != null)
            {
                processor.EventProcessorClosed += _eventProcessorClosed;
            }

            if (_eventProcessorOpened != null)
            {
                processor.EventProcessorOpened += _eventProcessorOpened;
            }

            if (_eventProcessorMessageReceived != null)
            {
                processor.EventProcessorMessageReceived += _eventProcessorMessageReceived;
            }

            return processor;
        }
    }
}
