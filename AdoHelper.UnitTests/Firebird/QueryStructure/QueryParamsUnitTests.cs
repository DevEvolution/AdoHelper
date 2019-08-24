using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdoHelper.UnitTests.Firebird.QueryStructure
{
    [TestClass]
    public class QueryParamsUnitTests : FirebirdDbConfig
    {
        [TestMethod]
        public void ValueTupleParams()
        {
            _connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intParam AND TextField = @textParam")
                .Parameters(
                ("@intParam", 123),
                ("@textParam", "Hello"))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void TupleParams()
        {
            _connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intParam AND TextField = @textParam")
                .Parameters(
                new Tuple<string, object>("@intParam", 123),
                new Tuple<string, object>("@textParam", "Hello"))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void CombinedParams()
        {
            _connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intParam AND TextField = @textParam AND FloatField = @floatParam")
                .Parameters(
                new AdoParameter("@intParam", 123),
                ("@textParam", "Hello"),
                new Tuple<string, object>("@floatParam", 123.123f))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }
    }
}
