using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper.TupleParsing
{
    /// <summary>
    /// Tuple access provider
    /// </summary>
    public class TupleAccess : TupleAccessBase
    {
        /// <summary>
        /// Count of elements in tuple including all inner tuples (TRest)
        /// </summary>
        public override int Count => ItemCount(_tuple);

        /// <summary>
        /// Creates tuple access provider
        /// </summary>
        /// <param name="tuple">Tuple to access</param>
        public TupleAccess(object tuple)
        {
            if (!IsTuple(tuple.GetType()))
                throw new ArgumentException("Argument is not a System.Tuple object");
            _tuple = tuple;
        }

        /// <summary>
        /// Gets the element at specified index not including TRest field
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element</returns>
        public object this[int index]
        {
            get => Get(_tuple, index);
        }

        /// <summary>
        /// Is tuple type match
        /// </summary>
        public override bool IsTypeMatch => IsTuple(_tuple.GetType());

        /// <summary>
        /// Count of elements in tuple including all inner tuples (TRest)
        /// </summary>
        /// <param name="tuple">Tuple object</param>
        /// <returns>Count of elements in tuple</returns>
        public static int ItemCount(object tuple)
            => ItemCount(tuple.GetType());

        /// <summary>
        /// Count of elements in tuple including all inner tuples (TRest)
        /// </summary>
        /// <param name="tupleType">Tuple type</param>
        /// <returns>Count of elements in tuple</returns>
        public static int ItemCount(Type tupleType)
        {
            Type inner = tupleType;
            int i;
            for (i = 0; inner.GetProperties().Length == 8; i++)
            {
                PropertyInfo property = inner.GetProperties().FirstOrDefault(u => u.Name == "Rest");
                inner = property.PropertyType;
            }
            return i * 7 + inner.GetProperties().Length;
        }

        /// <summary>
        /// Gets the type of tuple element at specified index
        /// </summary>
        /// <param name="tuple">Tuple type</param>
        /// <param name="index">Index of element</param>
        /// <returns>Type of tuple element</returns>
        public static Type GetItemType(Type tuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            Type inner = tuple;
            for (int i = 0; i < restCount; i++)
            {
                PropertyInfo property = inner.GetProperties().FirstOrDefault(u => u.Name == "Rest");
                if (property != null)
                    inner = property.PropertyType;
                else
                    throw new IndexOutOfRangeException("Specified index is out of tuple range");
            }

            PropertyInfo[] properties = inner.GetProperties();
            if (properties.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of tuple range");

            return inner.GetProperty($"Item{innerIndex + 1}").PropertyType;
        }

        /// <summary>
        /// Gets the tuple element at specified index
        /// </summary>
        /// <param name="tuple">Tuple object</param>
        /// <param name="index">Index of element</param>
        /// <returns>Tuple element</returns>
        public static object Get(object tuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            object inner = tuple;
            for (int i = 0; i < restCount; i++)
            {
                PropertyInfo field = inner.GetType().GetProperties().FirstOrDefault(u => u.Name == "Rest");
                if (field != null)
                    inner = field.GetValue(inner);
                else
                    throw new IndexOutOfRangeException("Specified index is out of value tuple range");
            }

            PropertyInfo[] fields = inner.GetType().GetProperties();
            if (fields.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            object result = inner.GetType().GetProperty($"Item{innerIndex + 1}").GetValue(inner);
            return result;
        }

        /// <summary>
        /// Is type a tuple
        /// </summary>
        /// <param name="_mtype">Type</param>
        /// <returns>Is type a tuple</returns>
        public static bool IsTuple(Type _mtype)
            => _mtype.IsGenericType && (
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,,>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,,,>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,,,,>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,,,,,>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,,,,,,>) ||
            _mtype.GetGenericTypeDefinition() == typeof(System.Tuple<,,,,,,,>)
            );
    }
}
