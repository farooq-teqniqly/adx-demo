using System.Collections.Generic;

namespace AdxLib
{
    public interface IAdxColumnSchemaFactory
    {
        IEnumerable<AdxColumnSchema> GenerateColumnSchemas(IEnumerable<JsonTreeNode> nodes);
    }
}