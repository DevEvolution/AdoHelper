using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.Mapping
{
    public class PropertyPriorityTestEntity : PriorityTestEntityBase
    {
        public int Id { get; set; }

        public string TextField { get; set; }

        public string textField;
    }
}
