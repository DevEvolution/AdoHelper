using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeContainer
{
    public class FakeTable
    {
        public string Name { get; set; }

        public List<FakeFieldHeader> Headers { get; set; } = new List<FakeFieldHeader>();

        public List<List<object>> Rows { get; set; } = new List<List<object>>();
    }
}
