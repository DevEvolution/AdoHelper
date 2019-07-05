using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.TupleParsing
{
    /// <summary>
    /// Base class of tuple access provider
    /// </summary>
    public abstract class TupleAccessBase
    {
        /// <summary>
        /// Tuple
        /// </summary>
        protected object _tuple;

        /// <summary>
        /// Tuple accessor
        /// </summary>
        public virtual object InnerTuple => _tuple;

        /// <summary>
        /// Count of elements in tuple including all inner tuples (TRest)
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Is tuple type match
        /// </summary>
        public abstract bool IsTypeMatch { get; }
    }
}
