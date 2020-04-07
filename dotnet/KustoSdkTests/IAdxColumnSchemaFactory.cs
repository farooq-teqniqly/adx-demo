using System.Collections.Generic;

namespace KustoSdkTests
{
    public interface IAdxColumnSchemaFactory
    {
        IEnumerable<AdxColumnSchema> GenerateColumnSchemas(IEnumerable<JsonTreeNode> nodes);
    }
}