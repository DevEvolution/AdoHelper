using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.FakeAdoNet.FakeQueries
{
    public class FakeQuery
    {
        public FakeCreate Create { get; set; }

        public FakeInsert Insert { get; set; }

        public FakeUpdate Update { get; set; }

        public FakeDelete Delete { get; set; }

        public FakeSelect Select { get; set; }
    }
}
