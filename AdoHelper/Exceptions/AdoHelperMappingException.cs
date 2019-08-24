using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.Exceptions
{
    public sealed class AdoHelperMappingException : AdoHelperException
    {
        public string ReaderFieldName { get; }
        
        public Type ReaderFieldType { get; }

        public string MappedFieldName { get; }

        public Type MappedFieldType { get; }

        public AdoHelperMappingException(string readerFieldName, Type readerFieldType, string mappedFieldName, Type mappedFieldType) :
            base($"Could not map query field {readerFieldName} of type {readerFieldType} to entity field {mappedFieldName} of type {mappedFieldType}")
        {
            ReaderFieldName = readerFieldName;
            ReaderFieldType = readerFieldType;
            MappedFieldName = mappedFieldName;
            MappedFieldType = mappedFieldType;
        }
    }
}
