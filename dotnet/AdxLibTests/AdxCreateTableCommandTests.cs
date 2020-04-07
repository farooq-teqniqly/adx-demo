using System;
using AdxApp.Models;
using AdxLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdxLibTests
{
    [TestClass]
    public class AdxCreateTableCommandTests
    {
        [TestMethod]
        public void Can_Create_Adx_Table_And_Mapping()
        {
            // Arrange
            
            var modelFactory = new ModelFactory();
            var telemetry = modelFactory.CreateTelemetryWithRandomValues();

            var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };
            var json = JsonConvert.SerializeObject(telemetry, jsonSerializerSettings);

            var columnSchemaFactory = new AdxColumnSchemaFactory();
            var tableSchemaFactory = new AdxTableSchemaFactory(columnSchemaFactory);

            var tableSchema = tableSchemaFactory.GenerateTableSchema("telemetry_demo", json);

            var adxClient = new AdxClient(
                "ingest-fmdusw2adxdemo.westus2",
                "demo",
                "e5f498be-7303-45fd-a594-9e3fd22a77d7",
                "33e9f686-413d-45b9-96e1-97088514e5ea",
                "aed96418-0610-4394-8365-daeacd48ec3f");

            var adxCreateTableCommand = new AdxCreateTableCommand(adxClient, tableSchema);

            // Act
            adxCreateTableCommand.Execute();

            // Assert
        }
    }
}
