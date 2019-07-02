using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.TupleParsing
{
    public abstract class TupleAccessBase
    {
        protected object _tuple;

        public virtual object InnerTuple => _tuple;

        public abstract int Count { get; }

        public abstract bool IsTypeMatch { get; }
    }
}
