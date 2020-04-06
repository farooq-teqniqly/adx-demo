using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace AdxApp
{
    public class TelemetryEventProcessor : IEventProcessor
    {
        private Stopwatch _checkpointStopwatch;

        public event EventHandler<EventProcessorEventArgs> EventProcessorOpened; 
        public event EventHandler<EventProcessorEventArgs> EventProcessorClosed; 
        public event EventHandler<TelemetryEventProcessorEventArgs> EventProcessorMessageReceived; 

        public Task OpenAsync(PartitionContext context)
        {
            FireEvent(new EventProcessorEventArgs(context.Lease.PartitionId, context.Lease.Offset), EventProcessorOpened);
            
            _checkpointStopwatch = new Stopwatch();
            _checkpointStopwatch.Start();
            return Task.FromResult<object>(null);
        }

        private void FireEvent<TEventArgs>(TEventArgs e, EventHandler<TEventArgs> handler) where TEventArgs : EventProcessorEventArgs
        {
            handler?.Invoke(this, e);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var message in messages)
            {
                var body = Encoding.UTF8.GetString(message.GetBytes());
                var props = message.Properties;

                FireEvent(new TelemetryEventProcessorEventArgs(body, props, context.Lease.PartitionId, context.Lease.Offset), EventProcessorMessageReceived);
            }

            if (_checkpointStopwatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                _checkpointStopwatch.Restart();
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            FireEvent(new EventProcessorEventArgs(context.Lease.PartitionId, context.Lease.Offset), EventProcessorClosed);

            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }
    }
}
