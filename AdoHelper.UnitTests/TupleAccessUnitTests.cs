using AdoHelper.TupleParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests
{
    [TestClass]
    public class TupleAccessUnitTests
    {
        [TestMethod]
        public void Tuple_Single_Count()
        {
            Tuple<int, string> tuple1 = new Tuple<int, string>(10, "Hello");
            Tuple<int, int, int, int, int, int> tuple2 = new Tuple<int, int, int, int, int, int>(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(2, tuple1.Count());
            Assert.AreEqual(6, tuple2.Count());
        }

        [TestMethod]
        public void ValueTuple_Single_Count()
        {
            (int, string) tuple1 = (10, "Hello");
            (int, int, int, int, int, int) tuple2 = (1, 2, 3, 4, 5, 6);

            Assert.AreEqual(2, tuple1.Count());
            Assert.AreEqual(6, tuple2.Count());
        }

        [TestMethod]
        public void Tuple_Simple_Get()
        {
            Tuple<int, string> tuple1 = new Tuple<int, string>(10, "Hello");
            Tuple<int, int, int, int, int, int> tuple2 = new Tuple<int, int, int, int, int, int>(1, 2, 3, 4, 5, 6);

            Assert.AreEqual("Hello", tuple1.Get(1));
            Assert.AreEqual(5, tuple2.Get(4));
        }

        [TestMethod]
        public void ValueTuple_Simple_Get()
        {
            (int, string) tuple1 = (10, "Hello");
            (int, int, int, int, int, int) tuple2 = (1, 2, 3, 4, 5, 6);

            Assert.AreEqual("Hello", tuple1.Get(1));
            Assert.AreEqual(5, tuple2.Get(4));
        }

        [TestMethod]
        public void Tuple_Simple_Set()
        {
            Tuple<int, string> tuple1 = new Tuple<int, string>(10, "Hello");
            Tuple<int, int, int, int, int, int> tuple2 = new Tuple<int, int, int, int, int, int>(1, 2, 3, 4, 5, 6);

            tuple1 = tuple1.Set(1, "Hi");
            tuple2 = tuple2.Set(4, 100);

            Assert.AreEqual("Hi", tuple1.Get(1));
            Assert.AreEqual(100, tuple2.Get(4));
        }

        [TestMethod]
        public void ValueTuple_Simple_Set()
        {
            (int, string) tuple1 = (10, "Hello");
            (int, int, int, int, int, int) tuple2 = (1, 2, 3, 4, 5, 6);

            tuple1 = tuple1.Set(1, "Hi");
            tuple2 = tuple2.Set(4, 100);

            Assert.AreEqual("Hi", tuple1.Get(1));
            Assert.AreEqual(100, tuple2.Get(4));
        }

        [TestMethod]
        public void Tuple_Long_Count()
        {
            Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>> tuple1 = new Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>>(10, "Hello", 11, "World", 12, "Test", 13,
                new Tuple<int, string, int, string, int, string>(14, "Happy", 15, "Fiction", 16, "Last"));
            Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>> tuple2 = new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>>(1, 2, 3, 4, 5, 6, 7,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>(8, 9, 10, 11, 12, 13, 14,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>(15, 16, 17, 18, 19, 20, 21,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>(22, 23, 24, 25, 26, 27, 28,
                new Tuple<int, int, int, int, int, int, int>(29, 30, 31, 32, 33, 34, 35)))));

            Assert.AreEqual(13, tuple1.Count());
            Assert.AreEqual(35, tuple2.Count());
        }

        [TestMethod]
        public void ValueTuple_Long_Count()
        {
            (int, string, int, string, int, string, int, int, string, int, string, int, string) tuple1 = (10, "Hello", 11, "World", 12, "Test", 13, 14, "Happy", 15, "Fiction", 16, "Last");
            (int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) tuple2 =
                (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35);

            Assert.AreEqual(13, tuple1.Count());
            Assert.AreEqual(35, tuple2.Count());
        }

        [TestMethod]
        public void Tuple_Long_Get()
        {
            Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>> tuple1 = new Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>>(10, "Hello", 11, "World", 12, "Test", 13,
                new Tuple<int, string, int, string, int, string>(14, "Happy", 15, "Fiction", 16, "Last"));
            Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>> tuple2 = new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>>(1, 2, 3, 4, 5, 6, 7,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>(8, 9, 10, 11, 12, 13, 14,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>(15, 16, 17, 18, 19, 20, 21,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>(22, 23, 24, 25, 26, 27, 28,
                new Tuple<int, int, int, int, int, int, int>(29, 30, 31, 32, 33, 34, 35)))));

            Assert.AreEqual("Happy", tuple1.Get(8));
            Assert.AreEqual("Last", tuple1.Get(12));

            Assert.AreEqual(25, tuple2.Get(24));
            Assert.AreEqual(31, tuple2.Get(30));

            Assert.AreEqual(typeof(string), tuple1.Get(10).GetType());
            Assert.AreEqual(typeof(int), tuple2.Get(26).GetType());
        }

        [TestMethod]
        public void ValueTuple_Long_Get()
        {
            (int, string, int, string, int, string, int, int, string, int, string, int, string) tuple1 = (10, "Hello", 11, "World", 12, "Test", 13, 14, "Happy", 15, "Fiction", 16, "Last");
            (int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) tuple2 =
                (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35);

            Assert.AreEqual("Happy", tuple1.Get(8));
            Assert.AreEqual("Last", tuple1.Get(12));

            Assert.AreEqual(25, tuple2.Get(24));
            Assert.AreEqual(31, tuple2.Get(30));

            Assert.AreEqual(typeof(string), tuple1.Get(10).GetType());
            Assert.AreEqual(typeof(int), tuple2.Get(26).GetType());
        }

        [TestMethod]
        public void Tuple_Long_Set()
        {
            Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>> tuple1 = new Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>>(10, "Hello", 11, "World", 12, "Test", 13,
                new Tuple<int, string, int, string, int, string>(14, "Happy", 15, "Fiction", 16, "Last"));
            Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>> tuple2 = new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>>(1, 2, 3, 4, 5, 6, 7,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>>(8, 9, 10, 11, 12, 13, 14,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>>(15, 16, 17, 18, 19, 20, 21,
                new Tuple<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>>(22, 23, 24, 25, 26, 27, 28,
                new Tuple<int, int, int, int, int, int, int>(29, 30, 31, 32, 33, 34, 35)))));

            tuple1 = tuple1.Set(8, "Helo");
            tuple1 = tuple1.Set(9, 29);
            tuple2 = tuple2.Set(14, 2015);
            tuple2 = tuple2.Set(30, 3100);

            Assert.AreEqual("Helo", tuple1.Get(8));
            Assert.AreEqual(29, tuple1.Get(9));
            Assert.AreEqual(2015, tuple2.Get(14));
            Assert.AreEqual(3100, tuple2.Get(30));
        }

        [TestMethod]
        public void ValueTuple_Long_Set()
        {
            (int, string, int, string, int, string, int, int, string, int, string, int, string) tuple1 = (10, "Hello", 11, "World", 12, "Test", 13, 14, "Happy", 15, "Fiction", 16, "Last");
            (int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) tuple2 =
                (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35);

            tuple1 = tuple1.Set(8, "Helo");
            tuple1 = tuple1.Set(9, 29);
            tuple2 = tuple2.Set(14, 2015);
            tuple2 = tuple2.Set(30, 3100);

            Assert.AreEqual("Helo", tuple1.Get(8));
            Assert.AreEqual(29, tuple1.Get(9));
            Assert.AreEqual(2015, tuple2.Get(14));
            Assert.AreEqual(3100, tuple2.Get(30));
        }

        [TestMethod]
        public void Tuple_NotTuple_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => new TupleAccess("Hi"));
            Assert.ThrowsException<ArgumentException>(() => new TupleAccess(123.123));
            Assert.ThrowsException<ArgumentException>(() => new TupleAccess(new List<char>() { 'a', 'b' }));
            Assert.ThrowsException<ArgumentException>(() => new TupleAccess(new byte[] { 11, 51 }));
        }

        [TestMethod]
        public void ValueTuple_NotTuple_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => new ValueTupleAccess("Hi"));
            Assert.ThrowsException<ArgumentException>(() => new ValueTupleAccess(123.123));
            Assert.ThrowsException<ArgumentException>(() => new ValueTupleAccess(new List<char>() { 'a', 'b' }));
            Assert.ThrowsException<ArgumentException>(() => new ValueTupleAccess(new byte[] { 11, 51 }));
        }

        [TestMethod]
        public void Tuple_Match()
        {
            Assert.AreEqual(true, typeof(Tuple<int, sbyte>).IsTuple());
            Assert.AreEqual(true, typeof(Tuple<,>).IsTuple());
            Assert.AreEqual(true, typeof(Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>>).IsTuple());
            Assert.AreEqual(true, typeof(Tuple<int, short, UInt16, DateTime, StringBuilder>).IsTuple());
            Assert.AreEqual(true, typeof(Tuple<(int, int), (int, int)>).IsTuple());

            Assert.AreEqual(false, typeof(int).IsTuple());
            Assert.AreEqual(false, typeof(string).IsTuple());
            Assert.AreEqual(false, typeof(Dictionary<int, sbyte>).IsTuple());
            Assert.AreEqual(false, typeof((int, int)).IsTuple());
            Assert.AreEqual(false, typeof((Tuple<int, int>, Tuple<int, int>)).IsTuple());
        }

        [TestMethod]
        public void ValueTuple_Match()
        {
            Assert.AreEqual(true, typeof((int, string, int, string, int, string, int, int, string, int, string, int, string)).IsValueTuple());
            Assert.AreEqual(true, typeof(ValueTuple<int>).IsValueTuple());
            Assert.AreEqual(true, typeof((int, short, UInt16, DateTime, StringBuilder)).IsValueTuple());
            Assert.AreEqual(true, typeof((int, int)).IsValueTuple());
            Assert.AreEqual(true, typeof((Tuple<int, int>, Tuple<int, int>)).IsValueTuple());

            Assert.AreEqual(false, typeof(int).IsValueTuple());
            Assert.AreEqual(false, typeof(string).IsValueTuple());
            Assert.AreEqual(false, typeof(Dictionary<int, sbyte>).IsValueTuple());
            Assert.AreEqual(false, typeof(Tuple<int, sbyte>).IsValueTuple());
            Assert.AreEqual(false, typeof(Tuple<(int, int), (int, int)>).IsValueTuple());
        }
    }
}
