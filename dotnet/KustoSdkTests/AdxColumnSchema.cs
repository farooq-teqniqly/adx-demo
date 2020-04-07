using System;

namespace KustoSdkTests
{
    public class AdxColumnSchema
    {
        public string Name { get; }
        public Type Type { get; }
        public string Mapping { get; }

        public AdxColumnSchema(string name, Type type, string mapping)
        {
            Name = name;
            Type = type;
            Mapping = mapping;
        }

    }
}