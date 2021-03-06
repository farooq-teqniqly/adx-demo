﻿using System.Collections.Generic;

namespace AdxLib
{
    public class AdxTableSchema
    {
        public string Name { get; }
        public IEnumerable<AdxColumnSchema> ColumnSchemas { get; }

        public AdxTableSchema(string name, IEnumerable<AdxColumnSchema> columnSchemas)
        {
            Name = name;
            ColumnSchemas = columnSchemas;
        }
    }
}