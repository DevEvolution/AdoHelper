using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.Mapping
{
    public class CombinedImplicitTestEntity
    {
        public short Id { get; set; }

        public string textField;

        public float floatField;

        public decimal NumericField { get; set; }

        public short IntegerField { get; set; }
    }
}
