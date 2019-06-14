using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper
{
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
