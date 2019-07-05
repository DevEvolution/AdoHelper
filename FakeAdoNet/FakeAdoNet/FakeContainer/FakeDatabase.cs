using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeContainer
{
    public class FakeDatabase
    {
        public string Name { get; set; }
        public List<FakeTable> Tables { get; set; } = new List<FakeTable>();
    }
}
