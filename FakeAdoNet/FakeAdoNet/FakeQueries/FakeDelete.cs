using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeQueries
{
    public class FakeDelete : IFakeOperation
    {
        public string From { get; set; }

        public string Where { get; set; }
    }
}
