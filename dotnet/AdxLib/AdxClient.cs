using System;
using System.Linq;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;

namespace AdxLib
{
    public class AdxClient : IDisposable
    {
        private bool _isDisposed = false;
        private readonly ICslAdminProvider _connection;

        public AdxClient(string kustoClusterName, string database, string aadTenantId, string appId, string appSecret)
        {
            var kustoConnectionStringBuilder = new KustoConnectionStringBuilder(
                $"https://{kustoClusterName}.kusto.windows.net", 
                database)
            {
                FederatedSecurity = true,
                Authority = aadTenantId,
                ApplicationClientId = appId,
                ApplicationKey = appSecret
            };

           _connection = KustoClientFactory.CreateCslAdminProvider(kustoConnectionStringBuilder);
        }

        public void ExecuteCommand(string command)
        {
            _connection.ExecuteControlCommand(command);
        }

        public void Dispose()
        {
           Dispose(true);
           GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                _connection?.Dispose();
            }

            _isDisposed = true;
        }
    }

    public class AdxCreateTableCommand
    {
        private readonly AdxClient _adxClient;
        private readonly AdxTableSchema _schema;
        
        public AdxCreateTableCommand(AdxClient adxClient, AdxTableSchema schema)
        {
            _adxClient = adxClient;
            _schema = schema;
        }

        public void Execute()
        {
            var rowFields = _schema.ColumnSchemas.Select(s => new Tuple<string, string>(s.Name, s.Type.ToString()));

            using (_adxClient)
            {
                var createTableCommand = CslCommandGenerator.GenerateTableCreateCommand(
                    _schema.Name,
                    rowFields);

                _adxClient.ExecuteCommand(createTableCommand);

                var columnMappings = _schema.ColumnSchemas.Select(s => new JsonColumnMapping {ColumnName = s.Name, JsonPath = s.Mapping});

                var createMappingCommand = CslCommandGenerator.GenerateTableJsonMappingCreateCommand(
                    _schema.Name,
                    $"{_schema.Name}_mapping",
                    columnMappings);

                _adxClient.ExecuteCommand(createMappingCommand);
            }
        }
    }
}
