using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.Exceptions
{
    public class AdoHelperException : Exception
    {
        public AdoHelperException(string message) : base(message)
        { }
    }
}
