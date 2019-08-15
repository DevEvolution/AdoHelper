using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper.TupleParsing
{
    public static class TupleAccessExtensions
    {
        #region interface
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

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T>(this Tuple<T> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2>(this Tuple<T1, T2> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2, T3>(this Tuple<T1, T2, T3> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int Count<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T>(this Tuple<T> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2>(this Tuple<T1, T2> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T> Set<T>(this Tuple<T> tuple, int index, object value) => (Tuple<T>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2> Set<T1, T2>(this Tuple<T1, T2> tuple, int index, object value) => (Tuple<T1, T2>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2, T3> Set<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, int index, object value) => (Tuple<T1, T2, T3>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2, T3, T4> Set<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple, int index, object value) => (Tuple<T1, T2, T3, T4>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2, T3, T4, T5> Set<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6> Set<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5, T6>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Set<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5, T6, T7>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of tuple with updated element value</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Set<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index, object value) => (Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>)SetTupleItem(tuple, index, value);

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int TupleItemCount(object tuple)
            => tuple.GetType().TupleItemCount();

        /// <summary>
        /// Gets the number of elements contained in tuple and all rest tuples
        /// </summary>
        /// <returns>Number of elements contained in tuple and all rest tuples</returns>
        public static int TupleItemCount(this Type tupleType)
        {
            if (tupleType.IsTuple()) return _GetTupleItemCount(tupleType);
            else if (tupleType.IsValueTuple()) return _GetValueTupleItemCount(tupleType);
            else
                throw new ArgumentException("Object type is not Tuple or ValueTuple type");
        }

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T>(this ValueTuple<T> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2>(this ValueTuple<T1, T2> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2, T3, T4, T5>(this ValueTuple<T1, T2, T3, T4, T5> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2, T3, T4, T5, T6>(this ValueTuple<T1, T2, T3, T4, T5, T6> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2, T3, T4, T5, T6, T7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple) => TupleItemCount(tuple);

        /// <summary>
        /// Gets the number of elements contained in value tuple and all rest value tuples
        /// </summary>
        /// <returns>Number of elements contained in value tuple and all rest value tuples</returns>
        public static int Count<T1, T2, T3, T4, T5, T6, T7, TRest>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple) where TRest : struct => TupleItemCount(tuple);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T>(this ValueTuple<T> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2>(this ValueTuple<T1, T2> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5>(this ValueTuple<T1, T2, T3, T4, T5> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5, T6>(this ValueTuple<T1, T2, T3, T4, T5, T6> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5, T6, T7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index) => GetTupleItem(tuple, index);

        /// <summary>
        /// Gets the element value from value tuple at specified index
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Element value from value tuple at specified index</returns>
        public static object Get<T1, T2, T3, T4, T5, T6, T7, TRest>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index) where TRest : struct => GetTupleItem(tuple, index);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T> Set<T>(this ValueTuple<T> tuple, int index, object value) => (ValueTuple<T>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2> Set<T1, T2>(this ValueTuple<T1, T2> tuple, int index, object value) => (ValueTuple<T1, T2>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2, T3> Set<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple, int index, object value) => (ValueTuple<T1, T2, T3>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2, T3, T4> Set<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2, T3, T4, T5> Set<T1, T2, T3, T4, T5>(this ValueTuple<T1, T2, T3, T4, T5> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4, T5>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2, T3, T4, T5, T6> Set<T1, T2, T3, T4, T5, T6>(this ValueTuple<T1, T2, T3, T4, T5, T6> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4, T5, T6>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7> Set<T1, T2, T3, T4, T5, T6, T7>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7> tuple, int index, object value) => (ValueTuple<T1, T2, T3, T4, T5, T6, T7>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Sets the tuple element value at specified index to provided value
        /// <para>Warning! This method creates a copy of value tuple with updated value. </para>
        /// Usage: <code>tuple = tuple.Set(1, "hello");</code>
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value</param>
        /// <returns>Copy of value tuple with updated element value</returns>
        public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> Set<T1, T2, T3, T4, T5, T6, T7, TRest>(this ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple, int index, object value) where TRest : struct => (ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)SetValueTupleItem(tuple, index, value);

        /// <summary>
        /// Gets the type of tuple element at specified index
        /// </summary>
        /// <param name="tuple">Tuple type</param>
        /// <param name="index">Index of element</param>
        /// <returns>Type of tuple element</returns>
        public static Type GetTupleItemType(this Type tuple, int index)
        {
            if (tuple.IsTuple()) return _GetTupleItemType(tuple, index);
            else if (tuple.IsValueTuple()) return _GetValueTupleItemType(tuple, index);
            else
                throw new ArgumentException("Object type is not Tuple or ValueTuple type");
        }

        #endregion

        #region backend
        /// <summary>
        /// Gets the count of tuple elements
        /// </summary>
        /// <param name="tupleType">Tuple type</param>
        /// <returns>Count of tuple elements</returns>
        private static int _GetTupleItemCount(Type tupleType)
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
        /// Gets the count of value tuple elements
        /// </summary>
        /// <param name="tupleType">Tuple type</param>
        /// <returns>Count of value tuple elements</returns>
        private static int _GetValueTupleItemCount(Type tupleType)
        {
            Type inner = tupleType;
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
            if (tuple.GetType().IsTuple()) return _GetTupleItem(tuple, index);
            else if (tuple.GetType().IsValueTuple()) return _GetValueTupleItem(tuple, index);
            else
                throw new ArgumentException("Object type is not Tuple or ValueTuple type");
        }

        /// <summary>
        /// Gets the tuple element at specified index
        /// </summary>
        /// <param name="tuple">Tuple object</param>
        /// <param name="index">Index of element</param>
        /// <returns>Tuple element</returns>
        private static object _GetTupleItem(object tuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            object inner = tuple;
            for (int i = 0; i < restCount; i++)
            {
                PropertyInfo property = inner.GetType().GetProperties().FirstOrDefault(u => u.Name == "Rest");
                if (property != null)
                    inner = property.GetValue(inner);
                else
                    throw new IndexOutOfRangeException("Specified index is out of tuple range");
            }

            PropertyInfo[] properties = inner.GetType().GetProperties();
            if (properties.Length < innerIndex + 1)
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
        private static object _GetValueTupleItem(object valueTuple, int index)
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
        /// Gets the tuple element at specified index
        /// </summary>
        /// <param name="tuple">Tuple object</param>
        /// <param name="index">Index of element</param>
        /// <returns>Value tuple element</returns>
        private static Type _GetTupleItemType(Type tuple, int index)
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
        /// Gets the type of value tuple element at specified index
        /// </summary>
        /// <param name="valueTuple">Value tuple type</param>
        /// <param name="index">Index of element</param>
        /// <returns>Type of value tuple element</returns>
        private static Type _GetValueTupleItemType(Type valueTuple, int index)
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
        /// Sets the tuple element at specified index
        /// </summary>
        /// <param name="tuple">Value tuple object</param>
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
                            throw new ArrayTypeMismatchException($"Incorrect value type: {value.GetType()}, type {itemInfo.PropertyType} needed");

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
                            throw new ArrayTypeMismatchException($"Incorrect value type: {value.GetType()}, type {itemInfo.FieldType} needed");

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
        #endregion
    }
}
