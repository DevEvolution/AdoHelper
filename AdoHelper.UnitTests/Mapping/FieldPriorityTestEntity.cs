using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.Mapping
{
    public class FieldPriorityTestEntity : PriorityTestEntityBase
    {
        public int Id { get; set; }

        public string TextField { get; set; }

        [Field(Name = "TextField")]
        public string textField;
    }
}
