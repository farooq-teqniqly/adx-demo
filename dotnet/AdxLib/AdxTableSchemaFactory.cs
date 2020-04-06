namespace AdxLib
{
    public class AdxTableSchemaFactory : IAdxTableSchemaFactory
    {
        private readonly IAdxColumnSchemaFactory _columnSchemaFactory;
        
        public AdxTableSchemaFactory(IAdxColumnSchemaFactory columnSchemaFactory)
        {
            _columnSchemaFactory = columnSchemaFactory;
        }

        public AdxTableSchema GenerateTableSchema(string name, string json)
        {
            // Get leaf nodes from the JSON. The leaf nodes contain the 
            // necessary information to generate the ADX table schema.

            var jsonTree = new JsonTree(json);
            var leafNodes = jsonTree.GetLeafNodes();

            // generate the ADX column definitions from the leaf nodes.
            var adxColumnSchemas = _columnSchemaFactory.GenerateColumnSchemas(leafNodes);

            return new AdxTableSchema(name, adxColumnSchemas);

        }
    }
}