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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123f) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.textField);
            Assert.IsTrue(Math.Abs(entity.floatField - 123.123f) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void SimplePropertyMapping_CombinedParams()
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
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.Test_TextField);
            Assert.IsTrue(Math.Abs(entity.Test_FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.Test_NumericField);
            Assert.AreEqual(123, entity.Test_IntegerField);
        }

        [TestMethod]
        public void ComplexAttributePropertyMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<ExcludedFieldTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual(null, entity.TextField);
            Assert.AreEqual(default(float), entity.FloatField);
            Assert.AreEqual(123, entity.Numeric);
            Assert.AreEqual(123, entity.Integer);
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

            Assert.AreEqual("Hello", entity.TextField);
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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.IsTrue(Math.Abs(entity.FloatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
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

            Assert.AreEqual("Hello", entity.textField);
            Assert.IsTrue(Math.Abs(entity.floatField - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.numericField);
            Assert.AreEqual(123, entity.integerField);
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

            Assert.AreEqual("Hello", entity.Item2);
            Assert.IsTrue(Math.Abs(entity.Item3 - 123.123) < 10e-5);
            Assert.AreEqual(123, entity.Item4);
            Assert.AreEqual(123, entity.Item5);
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

        [TestMethod]
        public void AreDifferentEntityTypesEqual()
        {
            _connection.Open();
            var classEntity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE id=@id")
                .Parameters(("@id", 1))
                .ExecuteReader().First();

            var structEntity = new AdoHelper<StructTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE id=@id")
                .Parameters(("@id", 1))
                .ExecuteReader().First();

            var valueTupleEntity = new AdoHelper<(int id, string text)>(_connection)
                .Query("SELECT id, TextField FROM TestTable WHERE id=@id")
                .Parameters(("@id", 1))
                .ExecuteReader().First();

            var tupleEntity = new AdoHelper<Tuple<int, string>>(_connection)
                .Query("SELECT id, TextField FROM TestTable WHERE id=@id")
                .Parameters(("@id", 1))
                .ExecuteReader().First();

            _connection.Close();

            Assert.AreEqual(classEntity.TextField, structEntity.TextField);
            Assert.AreEqual(structEntity.TextField, valueTupleEntity.text);
            Assert.AreEqual(valueTupleEntity.text, tupleEntity.Item2);
        } 

        [TestMethod]
        public void LongValueTupleMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<(int id, double double_value, DateTime date_value, TimeSpan time_value, string text_fixed, string text_varying, short small_value, byte[] array, int int_value, float float_value)>(_connection)
                .Query("SELECT * FROM LongTestTable WHERE id = @id")
                .Parameters(("@id", 2))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsNotNull(entity);
            Assert.AreEqual(2, entity.id);
            Assert.AreEqual(999.001001, entity.double_value);
            Assert.AreEqual(new DateTime(2007,07,07), entity.date_value);
            Assert.AreEqual(new TimeSpan(08,0,0), entity.time_value);
            Assert.AreEqual("fixed123", entity.text_fixed);
            Assert.AreEqual("varyin", entity.text_varying);
            Assert.AreEqual(1000, entity.small_value);
            var array = new byte[] { 48, 49, 49, 48 };
            Assert.AreEqual(entity.array.Length, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(array[0], entity.array[0]);
            }
            Assert.AreEqual(950, entity.int_value);
            Assert.IsTrue(Math.Abs(0.451 - entity.float_value) < 10e-5);
        }

        [TestMethod]
        public void LongTupleMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<Tuple<int, double, DateTime, TimeSpan, string, string, short, Tuple<byte[], int, float>>>(_connection)
                .Query("SELECT * FROM LongTestTable WHERE id = @id")
                .Parameters(("@id", 2))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsNotNull(entity);
            Assert.AreEqual(2, entity.Item1);
            Assert.AreEqual(999.001001, entity.Item2);
            Assert.AreEqual(new DateTime(2007, 07, 07), entity.Item3);
            Assert.AreEqual(new TimeSpan(08, 0, 0), entity.Item4);
            Assert.AreEqual("fixed123", entity.Item5);
            Assert.AreEqual("varyin", entity.Item6);
            Assert.AreEqual(1000, entity.Item7);
            var array = new byte[] { 48, 49, 49, 48 };
            Assert.AreEqual(entity.Rest.Item1.Length, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(array[0], entity.Rest.Item1[0]);
            }
            Assert.AreEqual(950, entity.Rest.Item2);
            Assert.IsTrue(Math.Abs(0.451 - entity.Rest.Item3) < 10e-5);
        }
    }
}
