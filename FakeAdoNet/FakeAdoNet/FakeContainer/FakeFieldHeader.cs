using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeContainer
{
    public class FakeFieldHeader
    {
        public string Name { get; set; }
        public DbType DbType { get; set; }

        public List<FakeDbAttribute> Attributes { get; set; } = new List<FakeDbAttribute>();
    }
}
