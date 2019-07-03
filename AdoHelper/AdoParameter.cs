using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AdoHelper
{
    /// <summary>
    /// AdoHelper query parameter
    /// </summary>
    public struct AdoParameter
    {
        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parameter value
        /// </summary>
        public object Value { get; set; }

        public static explicit operator AdoParameter(DbParameter parameter)
        {
            return new AdoParameter(parameter.ParameterName, parameter.Value);
        }

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
