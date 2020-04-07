using System;
using System.Configuration;
using System.Text;
using System.Threading;
using AdxApp.Models;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TelemetryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;

            var eventHubConnectionString = appSettings["eventHubConnectionString"];
            var eventHubName = appSettings["eventHubName"];
            var messageIntervalSeconds = int.Parse(appSettings["messageIntervalSeconds"]);

            var modelFactory = new ModelFactory();


            var contractResolver = new DefaultContractResolver {NamingStrategy = new CamelCaseNamingStrategy()};
            var jsonSerializerSettings = new JsonSerializerSettings {ContractResolver = contractResolver};
            var messagesSent = 0;

            var eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);

            while (true)
            {
                var telemetry = modelFactory.CreateTelemetryWithRandomValues();
                var json = JsonConvert.SerializeObject(telemetry, jsonSerializerSettings);

                var message = new EventData(Encoding.UTF8.GetBytes(json))
                {
                    Properties = { {"adxTableName", "adx_demo_telemetry"}}
                };
                

                eventHubClient.Send(message);

                messagesSent++;

                if (messagesSent > 0 && messagesSent % 5 == 0)
                {
                    Console.Write($"\r{messagesSent} messages sent.");
                }

                Thread.Sleep(TimeSpan.FromSeconds(messageIntervalSeconds));
            }

        }
    }
}
