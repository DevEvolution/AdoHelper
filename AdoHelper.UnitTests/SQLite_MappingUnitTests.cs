using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace AdoHelper.UnitTests
{
    [TestClass]
    public class SQLite_MappingUnitTests
    {
        private const string CONNECTION_STRING = "Data Source=testdb.db; Version=3;";

        private IDbConnection _connection = new SQLiteConnection(CONNECTION_STRING);

        [TestMethod]
        public void SimplePropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void ImplicitPropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<ImplicitTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual(entity.TextField, "Hello");
            Assert.AreEqual(entity.FloatField, 123.123f);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
        }

        [TestMethod]
        public void CombinedImplicitPropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<CombinedImplicitTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual(entity.textField, "Hello");
            Assert.AreEqual(entity.floatField, 123.123f);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
        }

        [TestMethod]
        public void SimplePropertyMapping_ValueTupleParams()
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

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void SimplePropertyMapping_TupleParams()
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

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }


        [TestMethod]
        public void NamedPropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<NamedTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.Test_TextField == "Hello");
            Assert.IsTrue(entity.Test_FloatField == 123.123);
            Assert.IsTrue(entity.Test_NumericField == 123);
            Assert.IsTrue(entity.Test_IntegerField == 123);
        }

        [TestMethod]
        public void ShortenedPropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<ShortenedTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
        }

        [TestMethod]
        public void SimpleFieldMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<FieldTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void StructPropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<StructTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.TextField == "Hello");
            Assert.IsTrue(entity.FloatField == 123.123);
            Assert.IsTrue(entity.NumericField == 123);
            Assert.IsTrue(entity.IntegerField == 123);
        }

        [TestMethod]
        public void ValueTupleMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<(long id, string textField, double floatField, decimal numericField, long integerField)>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.textField == "Hello");
            Assert.IsTrue(entity.floatField == 123.123);
            Assert.IsTrue(entity.numericField == 123);
            Assert.IsTrue(entity.integerField == 123);
        }

        [TestMethod]
        public void TupleMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<Tuple<long, string, double, decimal, long>>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsTrue(entity.Item2 == "Hello");
            Assert.IsTrue(entity.Item3 == 123.123);
            Assert.IsTrue(entity.Item4 == 123);
            Assert.IsTrue(entity.Item5 == 123);
        }

        [TestMethod]
        public void IntScalarQuery()
        {
            _connection.Open();
            var scalar = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            _connection.Close();

            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public void NullableIntScalarQuery()
        {
            _connection.Open();
            var scalar = new AdoHelper<int?>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            _connection.Close();

            Assert.IsNotNull(scalar);
            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public void FloatScalarQuery()
        {
            _connection.Open();
            var scalar = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            _connection.Close();

            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public void ComplexTransaction()
        {
            _connection.Open();

            int defaultCount = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();

            var transaction = _connection.BeginTransaction();
            new AdoHelper<int>(_connection)
                .Query("INSERT INTO TestTable (TextField, FloatField, NumericField, IntegerField) VALUES (@text, @float, @decimal, @int)")
                .Parameters(
                ("@text","Test hello"), 
                ("@float", 9.09), 
                ("@decimal", 193.123), 
                ("@int",85))
                .Transaction(transaction)
                .ExecuteNonQuery();

            int addCount = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .Transaction(transaction)
                .ExecuteScalar();
            transaction.Commit();

            new AdoHelper<int>(_connection)
                .Query("DELETE FROM TestTable WHERE TextField = @text")
                .Parameters(("@text", "Test hello"))
                .ExecuteNonQuery();

            int count = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();

            _connection.Close();

            Assert.IsTrue(addCount > defaultCount);
            Assert.IsTrue(defaultCount == count);
        }
    }
}
