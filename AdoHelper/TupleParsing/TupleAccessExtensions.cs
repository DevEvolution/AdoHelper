using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper.TupleParsing
{
    public static class TupleAccessExtensions
    {
        public static int Count<T>(this Tuple<T> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2>(this Tuple<T1, T2> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2, T3>(this Tuple<T1, T2, T3> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple) => TupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple) => TupleItemCount(tuple);

        public static object Get<T>(this Tuple<T> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2>(this Tuple<T1, T2> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index) => GetTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index) => GetTupleItem(tuple, index);

        public static Tuple<T> Set<T>(this Tuple<T> tuple, int index, object value) => (Tuple<T>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2> Set<T1, T2>(this Tuple<T1, T2> tuple, int index, object value) => (Tuple<T1, T2>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2, T3> Set<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, int index, object value) => (Tuple<T1, T2, T3>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2, T3, T4> Set<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple, int index, object value) => (Tuple<T1, T2, T3, T4>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2, T3, T4, T5> Set<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2, T3, T4, T5, T6> Set<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5, T6>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Set<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5, T6, T7>)SetTupleItem(tuple, index, value);
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Set<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>)SetTupleItem(tuple, index, value);

        public static int TupleItemCount(object tuple)
            => tuple.GetType().TupleItemCount();

        public static int TupleItemCount(this Type tupleType)
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

        public static int Count<T>(this ValueTuple<T> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2>(this ValueTuple<T1, T2> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5>(this ValueTuple<T1, T2, T3, T4, T5> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5, T6>(this ValueTuple<T1, T2, T3, T4, T5, T6> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5, T6, T7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple) => ValueTupleItemCount(tuple);
        public static int Count<T1, T2, T3, T4, T5, T6, T7, TRest>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple) where TRest : struct => ValueTupleItemCount(tuple);

        public static object Get<T>(this ValueTuple<T> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2>(this ValueTuple<T1, T2> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5>(this ValueTuple<T1, T2, T3, T4, T5> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5, T6>(this ValueTuple<T1, T2, T3, T4, T5, T6> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5, T6, T7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index) => GetValueTupleItem(tuple, index);
        public static object Get<T1, T2, T3, T4, T5, T6, T7, TRest>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index) where TRest : struct => GetValueTupleItem(tuple, index);

        public static ValueTuple<T> Set<T>(this ValueTuple<T> tuple, int index, object value) => (ValueTuple<T>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2> Set<T1, T2>(this ValueTuple<T1, T2> tuple, int index, object value) => (ValueTuple<T1, T2>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2, T3> Set<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple, int index, object value) => (ValueTuple<T1, T2, T3>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2, T3, T4> Set<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2, T3, T4, T5> Set<T1, T2, T3, T4, T5>(this ValueTuple<T1, T2, T3, T4, T5> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4, T5>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2, T3, T4, T5, T6> Set<T1, T2, T3, T4, T5, T6>(this ValueTuple<T1, T2, T3, T4, T5, T6> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4, T5, T6>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Set<T1, T2, T3, T4, T5, T6, T7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4, T5, T6, T7>)SetValueTupleItem(tuple, index, value);
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> Set<T1, T2, T3, T4, T5, T6, T7, TRest>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index, object value) where TRest : struct => (ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Count of elements in value tuple including all inner tuples (TRest)
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <returns></returns>
        public static int ValueTupleItemCount(object valueTuple)
            => valueTuple.GetType().ValueTupleItemCount();

        /// <summary>
        /// Count of elements in value tuple including all inner tuples (TRest) 
        /// </summary>
        /// <param name="valueTupleType">Type of value tuple</param>
        /// <returns>Count of elements in value tuple</returns>
        public static int ValueTupleItemCount(this Type valueTupleType)
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
        /// Gets the tuple element at specified index
        /// </summary>
        /// <param name="tuple">Tuple object</param>
        /// <param name="index">Index of element</param>
        /// <returns>Tuple element</returns>
        private static object GetTupleItem(object tuple, int index)
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
        /// Gets the value tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <param name="index">Index of element</param>
        /// <returns>Value tuple element</returns>
        private static object GetValueTupleItem(object valueTuple, int index)
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
        /// Sets the tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <param name="index">Index of element</param>
        /// <param name="value">New element value</param>
        private static object SetTupleItem(object tuple, int index, object value)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            Stack<object> innerTupleStack = new Stack<object>();
            object rezult = null;
            object inner = tuple;
            for (int i = 0; i < restCount; i++)
            {
                PropertyInfo property = inner.GetType().GetProperties().FirstOrDefault(u => u.Name == "Rest");
                if (property != null)
                {
                    innerTupleStack.Push(inner);
                    inner = property.GetValue(inner);
                }
                else
                    throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            }

            PropertyInfo[] properties = inner.GetType().GetProperties();
            if (properties.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            {
                List<object> parameters = new List<object>();
                for (int i = 0; i < 7; i++)
                {
                    PropertyInfo itemInfo = inner.GetType().GetProperty($"Item{i + 1}");
                    if (itemInfo == null) break;
                    if (i == innerIndex)
                    {
                        if (itemInfo.PropertyType != value.GetType())
                            throw new ArgumentException($"Incorrect value type: {value.GetType()}, type {itemInfo.PropertyType} needed");

                        parameters.Add(value);
                    }
                    else
                    {
                        parameters.Add(itemInfo.GetValue(inner));
                    }
                }
                PropertyInfo rest = inner.GetType().GetProperties().FirstOrDefault(u => u.Name == "Rest");
                if (rest != null)
                    parameters.Add(rest.GetValue(inner));

                rezult = Activator.CreateInstance(inner.GetType(), parameters.ToArray());
            }

            while (innerTupleStack.Count > 0)
            {
                object innerTuple = innerTupleStack.Pop();

                List<object> parameters = new List<object>();
                for (int i = 0; i < 7; i++)
                {
                    PropertyInfo itemInfo = innerTuple.GetType().GetProperty($"Item{i + 1}");
                    parameters.Add(itemInfo.GetValue(innerTuple));
                }
                PropertyInfo rest = innerTuple.GetType().GetProperties().FirstOrDefault(u => u.Name == "Rest");
                if (rest != null)
                    parameters.Add(rezult);

                rezult = Activator.CreateInstance(innerTuple.GetType(), parameters.ToArray());
            }

            return rezult;
        }

        /// <summary>
        /// Sets the value tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple object</param>
        /// <param name="index">Index of element</param>
        /// <param name="value">New element value</param>
        private static object SetValueTupleItem(object valueTuple, int index, object value)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            Stack<object> innerTupleStack = new Stack<object>();
            object rezult = null;
            object inner = valueTuple;
            for (int i = 0; i < restCount; i++)
            {
                FieldInfo field = inner.GetType().GetFields().FirstOrDefault(u => u.Name == "Rest");
                if (field != null)
                {
                    innerTupleStack.Push(inner);
                    inner = field.GetValue(inner);
                }
                else
                    throw new IndexOutOfRangeException("Specified index is out of value tuple range");
            }

            FieldInfo[] fields = inner.GetType().GetFields();
            if (fields.Length < innerIndex + 1)
                throw new IndexOutOfRangeException("Specified index is out of value tuple range");

            {
                List<object> parameters = new List<object>();
                for (int i = 0; i < 7; i++)
                {
                    FieldInfo itemInfo = inner.GetType().GetField($"Item{i + 1}");
                    if (itemInfo == null) break;
                    if (i == innerIndex)
                    {
                        if (itemInfo.FieldType != value.GetType())
                            throw new ArgumentException($"Incorrect value type: {value.GetType()}, type {itemInfo.FieldType} needed");

                        parameters.Add(value);
                    }
                    else
                    {
                        parameters.Add(itemInfo.GetValue(inner));
                    }
                }
                FieldInfo rest = inner.GetType().GetFields().FirstOrDefault(u => u.Name == "Rest");
                if (rest != null)
                    parameters.Add(rest.GetValue(inner));

                rezult = Activator.CreateInstance(inner.GetType(), parameters.ToArray());
            }

            while (innerTupleStack.Count > 0)
            {
                object innerTuple = innerTupleStack.Pop();

                List<object> parameters = new List<object>();
                for (int i = 0; i < 7; i++)
                {
                    FieldInfo itemInfo = innerTuple.GetType().GetField($"Item{i + 1}");
                    parameters.Add(itemInfo.GetValue(innerTuple));
                }
                FieldInfo rest = innerTuple.GetType().GetFields().FirstOrDefault(u => u.Name == "Rest");
                if (rest != null)
                    parameters.Add(rezult);

                rezult = Activator.CreateInstance(innerTuple.GetType(), parameters.ToArray());
            }

            return rezult;
        }


        /// <summary>
        /// Is object type a tuple
        /// </summary>
        /// <param name="_mtype">Type</param>
        /// <returns>Is type a tuple</returns>
        public static bool IsTuple(this Type _mtype)
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

        /// <summary>
        /// Is object type a value tuple
        /// </summary>
        /// <param name="_mtype">Object type</param>
        /// <returns>Is object type a value tuple</returns>
        public static bool IsValueTuple(this Type _mtype)
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
