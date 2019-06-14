using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper
{
    public class FieldMapInfo
    {
        public string dbFieldName;
        public string mapFieldName;
        public FieldType mapFieldType;

        public Type fullType;
        public Type innerType;
        public bool isNullable;

        public enum FieldType
        {
            Field,
            Property,
            TupleItem
        }
    }
}
