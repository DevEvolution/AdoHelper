using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeParameter : DbParameter
    {
        public override DbType DbType { get; set; } = DbType.String;
        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get; set; }
        public override string ParameterName { get; set; }
        public override int Size { get; set; }
        public override string SourceColumn { get; set; }
        public override bool SourceColumnNullMapping { get; set; }
        public override object Value { get; set; }

        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }

        public FakeParameter()
        { }

        public FakeParameter(string name)
        {
            ParameterName = name;
        }

        public FakeParameter(string name, object value)
        {
            ParameterName = name;
            Value = value;
        }
    }
}
