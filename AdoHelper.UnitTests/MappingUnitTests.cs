using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace AdoHelper.UnitTests
{
    [TestClass]
    public class MappingUnitTests
    {
        private const string CONNECTION_STRING = "Data Source=testdb.db; Version=3;";

        [TestMethod]
        public void Test_SimplePropertyMapping()
        {
            SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING);
            connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void Test_SimplePropertyMapping_ValueTupleParams()
        {
            SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING);
            connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intParam AND TextField = @textParam")
                .Parameters(
                ("@intParam", 123),
                ("@textParam", "Hello"))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void Test_SimplePropertyMapping_TupleParams()
        {
            SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING);
            connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intParam AND TextField = @textParam")
                .Parameters(
                new Tuple<string, object>("@intParam", 123),
                new Tuple<string, object>("@textParam", "Hello"))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }


        [TestMethod]
        public void Test_NamedPropertyMapping()
        {
            SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING);
            connection.Open();
            var entity = new AdoHelper<NamedTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.Test_TextField == "Hello");
            Assert.IsTrue(entity.Test_FloatField == 123.123);
            Assert.IsTrue(entity.Test_NumericField == 123);
            Assert.IsTrue(entity.Test_IntegerField == 123);
        }

        [TestMethod]
        public void Test_ShortenedPropertyMapping()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var entity = new AdoHelper<ShortenedTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
        }

        [TestMethod]
        public void Test_SimpleFieldMapping()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var entity = new AdoHelper<FieldTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void Test_StructPropertyMapping()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var entity = new AdoHelper<StructTestEntity>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void Test_ValueTupleMapping()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var entity = new AdoHelper<(long id, string textField, double floatField, decimal numericField, long integerField)>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.textField == "Hello");
            Assert.IsTrue(entity.floatField == 123.123);
            Assert.IsTrue(entity.numericField == 123);
            Assert.IsTrue(entity.integerField == 123);
        }

        [TestMethod]
        public void Test_TupleMapping()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var entity = new AdoHelper<Tuple<long, string, double, decimal, long>>(connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            connection.Close();

            Assert.IsTrue(entity.Item2 == "Hello");
            Assert.IsTrue(entity.Item3 == 123.123);
            Assert.IsTrue(entity.Item4 == 123);
            Assert.IsTrue(entity.Item5 == 123);
        }

        [TestMethod]
        public void Test_IntScalarQuery()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var scalar = new AdoHelper<int>(connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            connection.Close();

            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public void Test_NullableIntScalarQuery()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var scalar = new AdoHelper<int?>(connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            connection.Close();

            Assert.IsNotNull(scalar);
            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public void Test_FloatScalarQuery()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=testdb.db; Version=3;");
            connection.Open();
            var scalar = new AdoHelper<int>(connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            connection.Close();

            Assert.IsTrue(scalar > 0);
        }
    }
}
