using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper
{
    public struct AdoParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public static implicit operator AdoParameter((string name, object value) paramToCast)
        {
            return new AdoParameter(paramToCast.name, paramToCast.value);
        }

        public static implicit operator AdoParameter(Tuple<string, object> paramToCast)
        {
            return new AdoParameter(paramToCast.Item1, paramToCast.Item2);
        }

        public AdoParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
