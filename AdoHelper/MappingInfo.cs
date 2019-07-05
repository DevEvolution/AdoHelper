using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper
{
    /// <summary>
    /// Field mapping information
    /// </summary>
    public class MappingInfo
    {
        /// <summary>
        /// Name of field in database
        /// </summary>
        public string DbFieldName { get; set; }

        /// <summary>
        /// Name of field in mapping object
        /// </summary>
        public string MapFieldName { get; set; }

        /// <summary>
        /// <para>Type of mapping field</para>
        /// <para>Influences on how field should be mapped</para>
        /// </summary>
        public FieldType MapFieldType { get; set; }

        /// <summary>
        /// Full type of mapping field
        /// </summary>
        public Type FullType { get; set; }

        /// <summary>
        /// <para>Inner type of mapping field</para>
        /// <para>Used for mapping</para>
        /// </summary>
        public Type InnerType { get; set; }

        /// <summary>
        /// Is full type nullable
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Shows type of field in object
        /// </summary>
        public enum FieldType
        {
            Field,
            Property,
            CollectionItem
        }
    }
}
