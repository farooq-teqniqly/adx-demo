using System;
using System.Linq;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace KustoSdkTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            
            var modelFactory = new ModelFactory();
            var telemetry = modelFactory.CreateTelemetryWithRandomValues();

            var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };
            var json = JsonConvert.SerializeObject(telemetry, jsonSerializerSettings);

            var columnSchemaFactory = new AdxColumnSchemaFactory();
            var tableSchemaFactory = new AdxTableSchemaFactory(columnSchemaFactory);

            var tableSchema = tableSchemaFactory.GenerateTableSchema("telemetry_demo", json);

            var kustoClusterName = "ingest-fmdusw2adxdemo.westus2";
            var database = "demo";
            var aadTenantId = "e5f498be-7303-45fd-a594-9e3fd22a77d7";
            var appId = "33e9f686-413d-45b9-96e1-97088514e5ea";
            var appSecret = "aed96418-0610-4394-8365-daeacd48ec3f";

            var kustoConnectionStringBuilder = new KustoConnectionStringBuilder(
                $"https://{kustoClusterName}.kusto.windows.net",
                database)
            {
                FederatedSecurity = true,
                Authority = aadTenantId,
                ApplicationClientId = appId,
                ApplicationKey = appSecret
            };

            var kustoClient = KustoClientFactory.CreateCslAdminProvider(kustoConnectionStringBuilder);

            var table = "StormEvents";
            using (kustoClient)
            {
                //var createTableCommand =
                //    CslCommandGenerator.GenerateTableCreateCommand(
                //        table,
                //        new[]
                //        {
                //            Tuple.Create("StartTime", "System.DateTime"),
                //            Tuple.Create("EndTime", "System.DateTime")
                //        });

                //kustoClient.ExecuteControlCommand(createTableCommand);

                var createMappingCommand = CslCommandGenerator.GenerateTableJsonMappingCreateCommand(
                    table,
                    $"{table}_mapping",
                    new[]
                    {
                        new JsonColumnMapping {ColumnName = "StartTime", JsonPath = "$.StartTime"},
                        new JsonColumnMapping {ColumnName = "EndTime", JsonPath = "$.EndTime"},
                    });

                kustoClient.ExecuteControlCommand(createMappingCommand);

            }


            //var rowFields = tableSchema.ColumnSchemas.Select(s => new Tuple<string, string>(s.Name, s.Type.ToString()));
            //var mappings = tableSchema.ColumnSchemas.Select(s => new JsonColumnMapping { ColumnName = s.Name, JsonPath = s.Mapping });

            //using (kustoClient)
            //{
            //    var createTableCommand = CslCommandGenerator.GenerateTableCreateCommand(
            //        tableSchema.Name,
            //        rowFields);

            //    kustoClient.ExecuteControlCommand(createTableCommand);

            //    var createMappingCommand = CslCommandGenerator.GenerateTableJsonMappingCreateCommand(
            //        tableSchema.Name,
            //        $"{tableSchema.Name}_mapping",
            //        mappings);

            //    kustoClient.ExecuteControlCommand(createMappingCommand);
            //}
        }
    }
}
