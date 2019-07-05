using AdoHelper.FakeAdoNet.FakeContainer;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeQueries
{
    public class FakeCreate : IFakeOperation
    {
        public FakeTable Table { get; set; }
    }
}
