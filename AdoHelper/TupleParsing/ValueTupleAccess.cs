using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper.TupleParsing
{
    /// <summary>
    /// Value tuple access provider
    /// </summary>
    public class ValueTupleAccess : TupleAccessBase
    {
        /// <summary>
        /// Count of elements in tuple including all inner tuples (TRest)
        /// </summary>
        public override int Count => ItemCount(_tuple);

        /// <summary>
        /// Creates value tuple access provider
        /// </summary>
        /// <param name="tuple">Value tuple object</param>
        public ValueTupleAccess(object tuple)
        {
            if (!IsValueTuple(tuple.GetType()))
                throw new ArgumentException("Argument is not a System.ValueTuple object");
            _tuple = tuple;
        }

        /// <summary>
        /// Is value tuple type match
        /// </summary>
        public override bool IsTypeMatch => IsValueTuple(_tuple.GetType());

        /// <summary>
        /// Provides access to tuple elemnts including all inner tuples (TRest)
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Value tuple element</returns>
        public object this[int index]
        {
            get => Get(_tuple, index);
            set => Set(_tuple, index, value);
        }

        /// <summary>
        /// Count of elements in value tuple including all inner tuples (TRest)
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <returns></returns>
        public static int ItemCount(object valueTuple)
            => ItemCount(valueTuple.GetType());

        /// <summary>
        /// Count of elements in value tuple including all inner tuples (TRest) 
        /// </summary>
        /// <param name="valueTupleType">Type of value tuple</param>
        /// <returns>Count of elements in value tuple</returns>
        public static int ItemCount(Type valueTupleType)
        {
            Type inner = valueTupleType;
            int i;
            for (i = 0; inner.GetFields().Length == 8; i++)
            {
                FieldInfo field = inner.GetFields().FirstOrDefault(u => u.Name == "Rest");
                inner = field.FieldType;
            }
            return i * 7 + inner.GetFields().Length;
        }

        /// <summary>
        /// Gets the type of value tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple type</param>
        /// <param name="index">Index of element</param>
        /// <returns>Type of value tuple element</returns>
        public static Type GetItemType(Type valueTuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            Type inner = valueTuple;
            for (int i = 0; i < restCount; i++)
            {
                FieldInfo field = inner.GetFields().FirstOrDefault(u => u.Name == "Rest");
                if (field != null)
                    inner = field.FieldType;
                else
                    throw new IndexOutOfRangeException("Specified index is out of value tuple range");
            }

            FieldInfo[] fields = inner.GetFields();
            if (fields.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            return inner.GetField($"Item{innerIndex + 1}").FieldType;
        }

        /// <summary>
        /// Gets the value tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <param name="index">Index of element</param>
        /// <returns>Value tuple element</returns>
        public static object Get(object valueTuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            object inner = valueTuple;
            for (int i = 0; i < restCount; i++)
            {
                FieldInfo field = inner.GetType().GetFields().FirstOrDefault(u => u.Name == "Rest");
                if (field != null)
                    inner = field.GetValue(inner);
                else
                    throw new IndexOutOfRangeException("Specified index is out of value tuple range");
            }

            FieldInfo[] fields = inner.GetType().GetFields();
            if (fields.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            object result = inner.GetType().GetField($"Item{innerIndex + 1}").GetValue(inner);
            return result;
        }

        /// <summary>
        /// Sets the value tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <param name="index">Index of element</param>
        /// <param name="value">New element value</param>
        public static void Set(object valueTuple, int index, object value)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            object inner = valueTuple;
            for (int i = 0; i < restCount; i++)
            {
                FieldInfo field = inner.GetType().GetFields().FirstOrDefault(u => u.Name == "Rest");
                if (field != null)
                    inner = field.GetValue(inner);
                else
                    throw new IndexOutOfRangeException("Specified index is out of value tuple range");
            }

            FieldInfo[] fields = inner.GetType().GetFields();
            if (fields.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            inner.GetType().GetField($"Item{innerIndex + 1}").SetValue(inner, value);
        }

        /// <summary>
        /// Is object type a value tuple
        /// </summary>
        /// <param name="_mtype">Object type</param>
        /// <returns>Is object type a value tuple</returns>
        public static bool IsValueTuple(Type _mtype)
                => _mtype.IsGenericType && (
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,,>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,,,>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,,,,>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,,,,,>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,,,,,,>) ||
                _mtype.GetGenericTypeDefinition() == typeof(System.ValueTuple<,,,,,,,>)
                );
    }
}
