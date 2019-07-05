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

            Assert.AreEqual(2, new TupleAccess(tuple1).Count);
            Assert.AreEqual(6, new TupleAccess(tuple2).Count);
        }

        [TestMethod]
        public void ValueTuple_Single_Count()
        {
            (int, string) tuple1 = (10, "Hello");
            (int, int, int, int, int, int) tuple2 = (1, 2, 3, 4, 5, 6);

            Assert.AreEqual(2, new ValueTupleAccess(tuple1).Count);
            Assert.AreEqual(6, new ValueTupleAccess(tuple2).Count);
        }

        [TestMethod]
        public void Tuple_Simple_Get()
        {
            Tuple<int, string> tuple1 = new Tuple<int, string>(10, "Hello");
            Tuple<int, int, int, int, int, int> tuple2 = new Tuple<int, int, int, int, int, int>(1, 2, 3, 4, 5, 6);

            Assert.AreEqual("Hello", new TupleAccess(tuple1)[1]);
            Assert.AreEqual("Hello", TupleAccess.Get(tuple1, 1));

            Assert.AreEqual(5, new TupleAccess(tuple2)[4]);
            Assert.AreEqual(5, TupleAccess.Get(tuple2, 4));

            Assert.AreEqual(typeof(string), TupleAccess.GetItemType(tuple1.GetType(), 1));
            Assert.AreEqual(typeof(int), TupleAccess.GetItemType(tuple2.GetType(), 4));
        }

        [TestMethod]
        public void ValueTuple_Simple_Get()
        {
            (int, string) tuple1 = (10, "Hello");
            (int, int, int, int, int, int) tuple2 = (1, 2, 3, 4, 5, 6);

            Assert.AreEqual("Hello", new ValueTupleAccess(tuple1)[1]);
            Assert.AreEqual("Hello", ValueTupleAccess.Get(tuple1, 1));

            Assert.AreEqual(5, new ValueTupleAccess(tuple2)[4]);
            Assert.AreEqual(5, ValueTupleAccess.Get(tuple2, 4));

            Assert.AreEqual(typeof(string), ValueTupleAccess.GetItemType(tuple1.GetType(), 1));
            Assert.AreEqual(typeof(int), ValueTupleAccess.GetItemType(tuple2.GetType(), 4));
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

            Assert.AreEqual(13, new TupleAccess(tuple1).Count);
            Assert.AreEqual(35, new TupleAccess(tuple2).Count);
        }

        [TestMethod]
        public void ValueTuple_Long_Count()
        {
            (int, string, int, string, int, string, int, int, string, int, string, int, string) tuple1 = (10, "Hello", 11, "World", 12, "Test", 13, 14, "Happy", 15, "Fiction", 16, "Last");
            (int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) tuple2 =
                (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35);

            Assert.AreEqual(13, new ValueTupleAccess(tuple1).Count);
            Assert.AreEqual(35, new ValueTupleAccess(tuple2).Count);
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

            Assert.AreEqual("Happy", new TupleAccess(tuple1)[8]);
            Assert.AreEqual("Last", TupleAccess.Get(tuple1, 12));

            Assert.AreEqual(25, new TupleAccess(tuple2)[24]);
            Assert.AreEqual(31, TupleAccess.Get(tuple2, 30));

            Assert.AreEqual(typeof(string), TupleAccess.GetItemType(tuple1.GetType(), 10));
            Assert.AreEqual(typeof(int), TupleAccess.GetItemType(tuple2.GetType(), 26));
        }

        [TestMethod]
        public void ValueTuple_Long_Get()
        {
            (int, string, int, string, int, string, int, int, string, int, string, int, string) tuple1 = (10, "Hello", 11, "World", 12, "Test", 13, 14, "Happy", 15, "Fiction", 16, "Last");
            (int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) tuple2 =
                (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35);

            Assert.AreEqual("Happy", new ValueTupleAccess(tuple1)[8]);
            Assert.AreEqual("Last", ValueTupleAccess.Get(tuple1, 12));

            Assert.AreEqual(25, new ValueTupleAccess(tuple2)[24]);
            Assert.AreEqual(31, ValueTupleAccess.Get(tuple2, 30));

            Assert.AreEqual(typeof(string), ValueTupleAccess.GetItemType(tuple1.GetType(), 10));
            Assert.AreEqual(typeof(int), ValueTupleAccess.GetItemType(tuple2.GetType(), 26));
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
            Assert.AreEqual(true, TupleAccess.IsTuple(typeof(Tuple<int, sbyte>)));
            Assert.AreEqual(true, TupleAccess.IsTuple(typeof(Tuple<,>)));
            Assert.AreEqual(true, TupleAccess.IsTuple(typeof(Tuple<int, string, int, string, int, string, int, Tuple<int, string, int, string, int, string>>)));
            Assert.AreEqual(true, TupleAccess.IsTuple(typeof(Tuple<int, short, UInt16, DateTime, StringBuilder>)));
            Assert.AreEqual(true, TupleAccess.IsTuple(typeof(Tuple<(int, int), (int, int)>)));

            Assert.AreEqual(false, TupleAccess.IsTuple(typeof(int)));
            Assert.AreEqual(false, TupleAccess.IsTuple(typeof(string)));
            Assert.AreEqual(false, TupleAccess.IsTuple(typeof(Dictionary<int, sbyte>)));
            Assert.AreEqual(false, TupleAccess.IsTuple(typeof((int, int))));
            Assert.AreEqual(false, TupleAccess.IsTuple(typeof((Tuple<int, int>, Tuple<int, int>))));
        }

        [TestMethod]
        public void ValueTuple_Match()
        {
            Assert.AreEqual(true, ValueTupleAccess.IsValueTuple(typeof((int, string, int, string, int, string, int, int, string, int, string, int, string))));
            Assert.AreEqual(true, ValueTupleAccess.IsValueTuple(typeof(ValueTuple<int>)));
            Assert.AreEqual(true, ValueTupleAccess.IsValueTuple(typeof((int, short, UInt16, DateTime, StringBuilder))));
            Assert.AreEqual(true, ValueTupleAccess.IsValueTuple(typeof((int, int))));
            Assert.AreEqual(true, ValueTupleAccess.IsValueTuple(typeof((Tuple<int, int>, Tuple<int, int>))));

            Assert.AreEqual(false, ValueTupleAccess.IsValueTuple(typeof(int)));
            Assert.AreEqual(false, ValueTupleAccess.IsValueTuple(typeof(string)));
            Assert.AreEqual(false, ValueTupleAccess.IsValueTuple(typeof(Dictionary<int, sbyte>)));
            Assert.AreEqual(false, ValueTupleAccess.IsValueTuple(typeof(Tuple<int, sbyte>)));
            Assert.AreEqual(false, ValueTupleAccess.IsValueTuple(typeof(Tuple<(int, int), (int, int)>)));
        }
    }
}
