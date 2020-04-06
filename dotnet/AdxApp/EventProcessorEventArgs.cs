using System.Collections.Generic;

namespace AdxApp
{
    public class EventProcessorEventArgs
    {
        public string PartitionId { get; }
        public string Offset { get; }

        public EventProcessorEventArgs(string partitionId, string offset)
        {
            PartitionId = partitionId;
            Offset = offset;
        }
    }

    public class TelemetryEventProcessorEventArgs : EventProcessorEventArgs
    {
        public string Body { get; }
        public IDictionary<string, object> Properties { get; }

        public TelemetryEventProcessorEventArgs(string body, IDictionary<string, object> properties, string partitionId, string offset): base(partitionId, offset)
        {
            Body = body;
            Properties = properties;
        }
    }
}
