using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper.TupleParsing
{
    public class TupleAccess : TupleAccessBase
    {
        public override int Count => ItemCount(_tuple);

        public TupleAccess(object tuple)
        {
            if (!IsTuple(tuple.GetType()))
                throw new ArgumentException("Argument is not a System.Tuple object");
            _tuple = tuple;
        }

        public object this[int index]
        {
            get => Get(_tuple, index);
        }

        public override bool IsTypeMatch => IsTuple(_tuple.GetType());

        public static int ItemCount(object valueTuple)
            => ItemCount(valueTuple.GetType());

        public static int ItemCount(Type valueTupleType)
        {
            Type inner = valueTupleType;
            int i;
            for (i = 0; inner.GetProperties().Length == 8; i++)
            {
                PropertyInfo property = inner.GetProperties().FirstOrDefault(u => u.Name == "Rest");
                inner = property.PropertyType;
            }
            return i * 7 + inner.GetProperties().Length;
        }

        public static Type GetItemType(Type valueTuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            Type inner = valueTuple;
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

        public static object Get(object valueTuple, int index)
        {
            int restCount = index / 7;
            int innerIndex = index % 7;

            object inner = valueTuple;
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
