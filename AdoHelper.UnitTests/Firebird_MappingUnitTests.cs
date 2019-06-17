using AdoHelper.UnitTests.Mapping;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Linq;

namespace AdoHelper.UnitTests
{
    [TestClass]
    public class Firebird_MappingUnitTests
    {
        private const string CONNECTION_STRING = "user id=SYSDBA;password=masterkey;initial catalog=testdb.fdb;server type=Embedded";

        private IDbConnection _connection = new FbConnection(CONNECTION_STRING);

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

            Assert.AreEqual(entity.TextField, "Hello");
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
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
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123f) < 10e-5);
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
            Assert.IsTrue(Math.Abs(entity.floatField - 123.123f) < 10e-5);
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

            Assert.AreEqual(entity.TextField, "Hello");
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
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

            Assert.AreEqual(entity.TextField, "Hello");
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
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

            Assert.AreEqual(entity.Test_TextField, "Hello");
            Assert.IsTrue(Math.Abs(entity.Test_FloatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.Test_NumericField, 123);
            Assert.AreEqual(entity.Test_IntegerField, 123);
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

            Assert.AreEqual(entity.TextField, "Hello");
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
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

            Assert.AreEqual(entity.TextField, "Hello");
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.NumericField, 123);
            Assert.AreEqual(entity.IntegerField, 123);
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

            Assert.AreEqual(entity.textField, "Hello");
            Assert.IsTrue(Math.Abs(entity.floatField - 123.123) < 10e-5);
            Assert.AreEqual(entity.numericField, 123);
            Assert.AreEqual(entity.integerField, 123);
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

            Assert.AreEqual(entity.Item2, "Hello");
            Assert.IsTrue(Math.Abs(entity.Item3 - 123.123) < 10e-5);
            Assert.AreEqual(entity.Item4, 123);
            Assert.AreEqual(entity.Item5, 123);
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
                .Query("INSERT INTO TestTable (id, TextField, FloatField, NumericField, IntegerField) VALUES (@id, @text, @float, @decimal, @int)")
                .Parameters(
                ("@id", 5),
                ("@text", "Test hello"),
                ("@float", 9.09),
                ("@decimal", 193.123),
                ("@int", 85))
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
