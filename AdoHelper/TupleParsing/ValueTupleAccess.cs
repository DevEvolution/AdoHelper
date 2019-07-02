using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper.TupleParsing
{
    public class ValueTupleAccess : TupleAccessBase
    {
        public override int Count => ItemCount(_tuple);

        public ValueTupleAccess(object tuple)
        {
            if (!IsValueTuple(tuple.GetType()))
                throw new ArgumentException("Argument is not a System.ValueTuple object");
            _tuple = tuple;
        }

        public override bool IsTypeMatch => IsValueTuple(_tuple.GetType());

        public object this[int index]
        {
            get => Get(_tuple, index);
            set => Set(_tuple, index, value);
        }

        public static int ItemCount(object valueTuple)
            => ItemCount(valueTuple.GetType());

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
