using System;
using Microsoft.ServiceBus.Messaging;

namespace AdxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHubConnectionString = "Endpoint=sb://fm-d-usw2-adx-demo-eh-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yNdNMdUyjAMb2t8eQ8P2uh19EtBpej9RGXmgI7kU3mc=";
            var eventHubName = "fm-d-usw2-adx-demo-eh";
            var storageAccountName = "fmdusw2adxdemosto";
            var storageAccountKey = "IWH5uErqU31wQlLs7w68avQXzCITBOxhPPjGzG62CPtZaQMotAadY7u74g5JyiAprFNWrXzGcCKuUyVe+HiIAg==";
            var storageConnectionString = $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";

            var eventProcessorHostName = Guid.NewGuid().ToString();
            var eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            
            Console.WriteLine("Registering EventProcessor...");
            
            var options = new EventProcessorOptions();

            options.ExceptionReceived += (sender, e) => WriteException(e.Exception);
            
            var eventProcessorFactory = new TelemetryEventProcessorFactory(
                (sender, e) =>
                    WriteInformation(
                        $"EventProcessor initialized.  Partition: '{e.PartitionId}', Offset: '{e.Offset}'"),
                (sender, e) =>
                    WriteInformation(
                        $"EventProcessor shutting down.  Partition: '{e.PartitionId}', Offset: '{e.Offset}'"),
                (sender, e) =>
                    WriteInformation(
                        $"Message received. Partition: '{e.PartitionId}', Offset: '{e.Offset}', Body: {e.Body}"));

            eventProcessorHost.RegisterEventProcessorFactoryAsync(eventProcessorFactory, options).Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }

        private static void WriteInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void WriteException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        }
    }
}
