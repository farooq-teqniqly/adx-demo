namespace KustoSdkTests
{
    public interface IAdxTableSchemaFactory
    {
        AdxTableSchema GenerateTableSchema(string name, string json);
    }
}