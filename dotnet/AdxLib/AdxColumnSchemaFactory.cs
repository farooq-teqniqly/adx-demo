using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AdxLib
{
    public class AdxColumnSchemaFactory : IAdxColumnSchemaFactory
    {
        public IEnumerable<AdxColumnSchema> GenerateColumnSchemas(IEnumerable<JsonTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                var name = GenerateAdxColumnName(node);
                var mapping = $"$.{node.Path}";
                var type = GetDotNetTypeForNode(node);

                yield return new AdxColumnSchema(name, type, mapping);
            }
        }

        private static Type GetDotNetTypeForNode(JsonTreeNode node)
        {
            switch (node.Type)
            {
                case JTokenType.Boolean:
                    return typeof(bool);

                case JTokenType.Date:
                    return typeof(DateTime);

                case JTokenType.Float:
                    return typeof(double);

                case JTokenType.Integer:
                    return typeof(int);

                default:
                    return typeof(string);
            }
        }

        private static string GenerateAdxColumnName(JsonTreeNode node)
        {
            // replace all periods with underscores
            var columnName = node.Path.Replace(".", "_");

            // replace array indexes with underscores
            var match = Regex.Match(columnName, @"\[[\d]+\]");

            if (match.Success)
            {
                columnName = columnName.Replace("[", "_").Replace("]", string.Empty);
            }

            // replace beginning "@" with underscore

            if (columnName[0] == '@')
            {
                columnName = $"_{columnName.Substring(1)}";
            }

            return columnName;
        }
    }
}
