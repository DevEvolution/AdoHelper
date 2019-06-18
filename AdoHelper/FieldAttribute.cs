using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper
{
    /// <summary>
    /// Indicates that field of mapped class/struct should be prioritizely used for mapping
    /// </summary>
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// Sets the mapped database field name for class/struct field
        /// </summary>
        public string Name { get; set; }
    }
}
