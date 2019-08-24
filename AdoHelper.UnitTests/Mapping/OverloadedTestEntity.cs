using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.Mapping
{
    public class OverloadedTestEntity
    {
        public int Id { get; set; }
        public float id { get; set; }

        public string TextField { get; set; }
        public string textField { get; set; }
    }
}
