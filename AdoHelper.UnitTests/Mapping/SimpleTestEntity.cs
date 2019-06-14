using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.Mapping
{
    public class SimpleTestEntity
    {
        public long Id { get; set; }

        public string TextField { get; set; }

        public double FloatField { get; set; }

        public decimal NumericField { get; set; }

        public long IntegerField { get; set; }
    }
}
