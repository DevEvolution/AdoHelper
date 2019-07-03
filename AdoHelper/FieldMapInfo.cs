using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper
{
    /// <summary>
    /// Field mapping information
    /// </summary>
    public class FieldMapInfo
    {
        /// <summary>
        /// Name of field in database
        /// </summary>
        public string dbFieldName;

        /// <summary>
        /// Name of field in mapping object
        /// </summary>
        public string mapFieldName;

        /// <summary>
        /// <para>Type of mapping field</para>
        /// <para>Influences on how field should be mapped</para>
        /// </summary>
        public FieldType mapFieldType;

        /// <summary>
        /// Full type of mapping field
        /// </summary>
        public Type fullType;

        /// <summary>
        /// <para>Inner type of mapping field</para>
        /// <para>Used for mapping</para>
        /// </summary>
        public Type innerType;

        /// <summary>
        /// Is full type nullable
        /// </summary>
        public bool isNullable;


        public enum FieldType
        {
            Field,
            Property,
            CollectionItem
        }
    }
}
