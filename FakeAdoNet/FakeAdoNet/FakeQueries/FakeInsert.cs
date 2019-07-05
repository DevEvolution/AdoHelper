using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeQueries
{
    public class FakeInsert : IFakeOperation
    {
        public string Into { get; set; }

        public List<string> Headers { get; set; } = new List<string>();
        public List<object> Values { get; set; } = new List<object>();
    }
}
