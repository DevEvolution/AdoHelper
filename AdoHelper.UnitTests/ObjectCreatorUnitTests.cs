using AdoHelper.TupleParsing;
using AdoHelper.UnitTests.ObjectsToCreate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdoHelper.UnitTests
{
    [TestClass]
    public class ObjectCreatorUnitTests
    {

        [TestMethod]
        public void Create_String()
        {
            string value = ObjectCreator.Create<string>();

            Assert.AreEqual(String.Empty, value);
        }

        [TestMethod]
        public void Create_UInt16()
        {
            ushort value = ObjectCreator.Create<ushort>();

            Assert.AreEqual(0, value);
        }

        [TestMethod]
        public void Create_DateTime()
        {
            DateTime value = ObjectCreator.Create<DateTime>(new List<object>() { 2004, 12, 04 });

            Assert.AreEqual(new DateTime(2004, 12, 04), value);
        }

        [TestMethod]
        public void Create_ComplexObject()
        {
            ComplexClass complex = ObjectCreator.Create<ComplexClass>();

            Assert.AreEqual(String.Empty, complex.Name);
            Assert.AreEqual((ConsoleKey)0, complex.Key);
            Assert.AreEqual(new StringBuilder().ToString(), complex.builder.ToString());
        }

        [TestMethod]
        public void CreateTuple_SimpleTuple()
        {
            var tuple = ObjectCreator.CreateTuple<Tuple<int, string>>(new List<object>() { 10, "Hello" });

            Assert.AreEqual(10, tuple.Item1);
            Assert.AreEqual("Hello", tuple.Item2);
        }

        [TestMethod]
        public void CreateTuple_LongTuple()
        {
            List<object> parameters = new List<object>();
            for (int i = 0; i < 28; i++)
            {
                parameters.Add(i * 100);
            }
            var tuple = ObjectCreator.CreateTuple<Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>>(parameters);
            TupleAccess access = new TupleAccess(tuple);

            Assert.AreEqual(28, access.Count);
            for (int i = 0; i < 28; i++)
            {
                Assert.AreEqual(i * 100, access[i]);
            }
        }

        [TestMethod]
        public void CreateTuple_SimpleValueTuple()
        {
            var tuple = ObjectCreator.CreateTuple<(int intValue, string stringValue)>(new List<object>() { 10, "Hello" });

            Assert.AreEqual(10, tuple.intValue);
            Assert.AreEqual("Hello", tuple.stringValue);
        }

        [TestMethod]
        public void CreateTuple_LongValueTuple()
        {
            List<object> parameters = new List<object>();
            for (int i = 0; i < 100; i++)
            {
                parameters.Add(i * 100);
            }
            var tuple = ObjectCreator.CreateTuple<(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int)>(parameters);
            ValueTupleAccess access = new ValueTupleAccess(tuple);

            Assert.AreEqual(100, access.Count);
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(i * 100, access[i]);
            }
        }

        [TestMethod]
        public void CreateTuple_NotTuple_Int32_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => ObjectCreator.CreateTuple<int>(new List<object> { 10 }));
        }

        [TestMethod]
        public void CreateTuple_NotTuple_String_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => ObjectCreator.CreateTuple<string>(new List<object> { 10 }));
        }

        [TestMethod]
        public void CreateTuple_ParameterCountMismatch_Less_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => ObjectCreator.CreateTuple<(string, string)>(new List<object> { "Hello" }));
        }

        [TestMethod]
        public void CreateTuple_ParameterCountMismatch_More_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => ObjectCreator.CreateTuple<(string, string)>(new List<object> { "Hello", "world", "third" }));
        }

        [TestMethod]
        public void CreateEnumerable_IEnumerable()
        {
            var enumerable = ObjectCreator.CreateEnumerable<IEnumerable<int>>();

            Assert.IsInstanceOfType(enumerable, typeof(IEnumerable<int>));
            Assert.IsInstanceOfType(enumerable, typeof(ICollection<int>));
            Assert.IsInstanceOfType(enumerable, typeof(List<int>));
        }

        [TestMethod]
        public void CreateEnumerable_ICollection()
        {
            var enumerable = ObjectCreator.CreateEnumerable<ICollection<int>>();

            Assert.IsInstanceOfType(enumerable, typeof(ICollection<int>));
            Assert.IsInstanceOfType(enumerable, typeof(List<int>));
        }

        [TestMethod]
        public void CreateEnumerable_List()
        {
            var enumerable = ObjectCreator.CreateEnumerable<List<int>>();

            Assert.IsInstanceOfType(enumerable, typeof(List<int>));
        }

        [TestMethod]
        public void CreateEnumerable_Array()
        {
            var enumerable = ObjectCreator.CreateEnumerable<int[]>();

            Assert.IsInstanceOfType(enumerable, typeof(int[]));
        }

        [TestMethod]
        public void CreateEnumerable_NotEnumerable_Int32_Exception()
        {
            Assert.ThrowsException<NotSupportedException>(() => ObjectCreator.CreateEnumerable<int>());
        }

        [TestMethod]
        public void CreateEnumerable_NotEnumerable_String_Exception()
        {
            Assert.ThrowsException<NotSupportedException>(() => ObjectCreator.CreateEnumerable<string>());
        }
    }
}
