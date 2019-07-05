using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeException : Exception
    {
        public FakeException()
        {

        }

        public FakeException(string message) : base(message)
        {
            
        }
    }
}
