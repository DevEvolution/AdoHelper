using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdoHelper
{
    public class QueryInfo<T>
    {
        public IDbConnection Connection { get; set; }
        public T Model { get; set; }
        public IDbCommand Command { get; set; }
        public IDbTransaction Transaction { get; set; }
        public List<IDbDataParameter> QueryInfoParameters { get; set; }
        public ModelEntityType ModelType { get; set; }
        public List<FieldMapInfo> ModelStructureTable { get; set; }

        public enum ModelEntityType
        {
            Object,
            Tuple,
            ValueTuple,
            GenericObject
        }
    }
}
