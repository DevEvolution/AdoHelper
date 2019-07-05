using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeQueries
{
    public class FakeSelect : IFakeOperation
    {
        public List<string> Fields { get; set; } = new List<string>();

        public string From { get; set; }

        public string Where { get; set; }
    }
}
