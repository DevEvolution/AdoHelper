using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeQueries
{
    public class FakeUpdate : IFakeOperation
    {
        public string Table { get; set; }

        public List<FakeUpdateSet> To { get; set; } = new List<FakeUpdateSet>();

        public string Where { get; set; }
    }
}
