using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public async Task AsyncReaderMapping()
        {
            _connection.Open();
            var entity = (await new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReaderAsync())
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public async Task AsyncReaderMapping_Cancel()
        {
            _connection.Open();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await new AdoHelper<SimpleTestEntity>(_connection)
               .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
               .Parameters(new AdoParameter("@intValue", 123))
               .ExecuteReaderAsync(token));
            _connection.Close();
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
            Assert.AreEqual(123.123f, entity.FloatField);
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
            Assert.AreEqual(123.123f, entity.floatField);
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
            Assert.AreEqual(123.123, entity.FloatField);
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
            Assert.AreEqual(123.123, entity.FloatField);
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
                new Tuple<string, object>("@floatParam", 123.123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField);
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
            Assert.AreEqual(123.123, entity.Test_FloatField);
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
            Assert.AreEqual(123.123, entity.FloatField);
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
            Assert.AreEqual(123.123, entity.FloatField);
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
            Assert.AreEqual(123.123, entity.floatField);
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
            Assert.AreEqual(123.123, entity.Item3);
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
        public async Task AsyncScalarQuery()
        {
            _connection.Open();
            var scalar = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync();
            _connection.Close();

            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public async Task AsyncScalarQuery_Cancel()
        {
            _connection.Open();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            source.Cancel();
            Task<int> task = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync(token);

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => { await task; });
            _connection.Close();
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
        public async Task AsyncComplexTransaction()
        {
            _connection.Open();

            int defaultCount = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync();

            var transaction = _connection.BeginTransaction();
            await new AdoHelper<int>(_connection)
                .Query("INSERT INTO TestTable (TextField, FloatField, NumericField, IntegerField) VALUES (@text, @float, @decimal, @int)")
                .Parameters(
                ("@text", "Test hello"),
                ("@float", 9.09),
                ("@decimal", 193.123),
                ("@int", 85))
                .Transaction(transaction)
                .ExecuteNonQueryAsync();

            int addCount = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .Transaction(transaction)
                .ExecuteScalarAsync();
            transaction.Commit();

            await new AdoHelper<int>(_connection)
                .Query("DELETE FROM TestTable WHERE TextField = @text")
                .Parameters(("@text", "Test hello"))
                .ExecuteNonQueryAsync();

            int count = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync();

            _connection.Close();

            Assert.IsTrue(addCount > defaultCount);
            Assert.IsTrue(defaultCount == count);
        }

        public async Task AsyncComplexTransaction_Cancel()
        {
            _connection.Open();

            int defaultCount = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            source.Cancel();
            var transaction = _connection.BeginTransaction();
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await new AdoHelper<int>(_connection)
                .Query("INSERT INTO TestTable (TextField, FloatField, NumericField, IntegerField) VALUES (@text, @float, @decimal, @int)")
                .Parameters(
                ("@text", "Test hello"),
                ("@float", 9.09),
                ("@decimal", 193.123),
                ("@int", 85))
                .Transaction(transaction)
                .ExecuteNonQueryAsync(token));

            transaction.Rollback();

            int addCount = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .Transaction(transaction)
                .ExecuteScalarAsync();

            _connection.Close();

            Assert.IsTrue(addCount == defaultCount);
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
            var entity = new AdoHelper<(int id, double double_value, string date_value, string time_value, string text_fixed, string text_varying, short small_value, byte[] array, int int_value, float float_value)>(_connection)
                .Query("SELECT * FROM LongTestTable WHERE id = @id")
                .Parameters(("@id", 3))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsNotNull(entity);
            Assert.AreEqual(3, entity.id);
            Assert.AreEqual(999.001001, entity.double_value);
            Assert.AreEqual(new DateTime(2007, 07, 07), DateTime.Parse(entity.date_value));
            Assert.AreEqual(new TimeSpan(08, 0, 0), TimeSpan.Parse(entity.time_value));
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
            Assert.AreEqual(0.451, entity.float_value, 10e-5);
        }

        [TestMethod]
        public void LongTupleMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<Tuple<int, double, string, string, string, string, short, Tuple<byte[], int, float>>>(_connection)
                .Query("SELECT * FROM LongTestTable WHERE id = @id")
                .Parameters(("@id", 3))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.IsNotNull(entity);
            Assert.AreEqual(3, entity.Item1);
            Assert.AreEqual(999.001001, entity.Item2);
            Assert.AreEqual(new DateTime(2007, 07, 07), DateTime.Parse(entity.Item3));
            Assert.AreEqual(new TimeSpan(08, 0, 0), TimeSpan.Parse(entity.Item4));
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
            Assert.AreEqual(0.451, entity.Rest.Item3, 10e-5);
        }

        [TestMethod]
        public void CollectionMapping_IEnumerable()
        {
            _connection.Open();
            var entity = new AdoHelper<IEnumerable<string>>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReader();
            _connection.Close();

            Assert.AreNotEqual(0, entity.Count());

            var model = entity.First().ToList();
            Assert.AreEqual(5, model.Count);
            Assert.AreEqual("Hello", model[1]);
            Assert.AreEqual("123.123", model[2].Replace(',', '.'));
            Assert.AreEqual("123", model[3]);
            Assert.AreEqual("123", model[4]);
        }

        [TestMethod]
        public void CollectionMapping_List()
        {
            _connection.Open();
            var entity = new AdoHelper<List<string>>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReader();
            _connection.Close();

            Assert.AreNotEqual(0, entity.Count());

            var model = entity.First();
            Assert.AreEqual(5, model.Count);
            Assert.AreEqual("Hello", model[1]);
            Assert.AreEqual("123.123", model[2].Replace(',', '.'));
            Assert.AreEqual("123", model[3]);
            Assert.AreEqual("123", model[4]);
        }

        [TestMethod]
        public void CollectionMapping_Collection()
        {
            _connection.Open();
            var entity = new AdoHelper<ICollection<string>>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReader();
            _connection.Close();

            Assert.AreNotEqual(0, entity.Count());

            var model = entity.First().ToList();

            Assert.AreEqual(5, model.Count);

            Assert.AreEqual("Hello", model[1]);
            Assert.AreEqual("123.123", model[2].Replace(',', '.'));
            Assert.AreEqual("123", model[3]);
            Assert.AreEqual("123", model[4]);
        }

        [TestMethod]
        public void DynamicMapping()
        {
            _connection.Open();
            var entity = new AdoHelper<dynamic>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReader();
            _connection.Close();

            var model = entity.First();

            Assert.AreEqual(5, ((IDictionary<String, Object>)model).Count);

            Assert.AreEqual("Hello", model.TextField);
            Assert.AreEqual(123.123, model.FloatField);
            Assert.AreEqual(123, model.NumericField);
            Assert.AreEqual(123, model.IntegerField);
        }

        [TestMethod]
        public async Task AsyncDynamicMapping()
        {
            _connection.Open();
            var entity = await new AdoHelper<dynamic>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReaderAsync();
            _connection.Close();

            var model = entity.First();

            Assert.AreEqual(5, ((IDictionary<String, Object>)model).Count);

            Assert.AreEqual("Hello", model.TextField);
            Assert.AreEqual(123.123, model.FloatField);
            Assert.AreEqual(123, model.NumericField);
            Assert.AreEqual(123, model.IntegerField);
        }

        [TestMethod]
        public async Task AsyncDynamicMapping_Cancel()
        {
            _connection.Open();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await new AdoHelper<dynamic>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReaderAsync(token)
            );
            _connection.Close();
        }
    }
}
