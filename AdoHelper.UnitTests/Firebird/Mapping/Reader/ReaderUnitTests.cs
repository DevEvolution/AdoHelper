using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdoHelper.UnitTests.Firebird.Mapping.Reader
{
    [TestClass]
    public class ReaderUnitTests : FirebirdDbConfig
    {
        [TestMethod]
        public void Simple()
        {
            _connection.Open();
            var entity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void ImplicitProperty()
        {
            _connection.Open();
            var entity = new AdoHelper<ImplicitTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123f, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void ImplicitProperty_Combined()
        {
            _connection.Open();
            var entity = new AdoHelper<CombinedImplicitTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.textField);
            Assert.AreEqual(123.123f, entity.floatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void NamedProperty()
        {
            _connection.Open();
            var entity = new AdoHelper<NamedTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.Test_TextField);
            Assert.AreEqual(123.123, entity.Test_FloatField, 10e-5);
            Assert.AreEqual(123, entity.Test_NumericField);
            Assert.AreEqual(123, entity.Test_IntegerField);
        }

        [TestMethod]
        public void ComplexAttributeProperty()
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
        public void ShortenedProperty()
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
        public void SimpleField()
        {
            _connection.Open();
            var entity = new AdoHelper<FieldTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void Struct()
        {
            _connection.Open();
            var entity = new AdoHelper<StructTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual(123.123, entity.FloatField, 10e-5);
            Assert.AreEqual(123, entity.NumericField);
            Assert.AreEqual(123, entity.IntegerField);
        }

        [TestMethod]
        public void ValueTuple()
        {
            _connection.Open();
            var entity = new AdoHelper<(long id, string textField, double floatField, decimal numericField, long integerField)>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.textField);
            Assert.AreEqual(123.123, entity.floatField, 10e-5);
            Assert.AreEqual(123, entity.numericField);
            Assert.AreEqual(123, entity.integerField);
        }

        [TestMethod]
        public void Tuple()
        {
            _connection.Open();
            var entity = new AdoHelper<Tuple<long, string, double, decimal, long>>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
                .Parameters(new AdoParameter("@intValue", 123))
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual("Hello", entity.Item2);
            Assert.AreEqual(123.123, entity.Item3, 10e-5);
            Assert.AreEqual(123, entity.Item4);
            Assert.AreEqual(123, entity.Item5);
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
        public void LongValueTuple()
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
            Assert.AreEqual(new DateTime(2007, 07, 07), entity.date_value);
            Assert.AreEqual(new TimeSpan(08, 0, 0), entity.time_value);
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
        public void LongTuple()
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
            Assert.AreEqual(0.451, entity.Rest.Item3, 10e-5);
        }

        [TestMethod]
        public void IEnumerable()
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
            Assert.AreEqual(123.123, double.Parse(model[2]), 10e-5);
            Assert.AreEqual(123M, decimal.Parse(model[3]));
            Assert.AreEqual(123, int.Parse(model[4]));
        }

        [TestMethod]
        public void List()
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
            Assert.AreEqual(123.123, double.Parse(model[2]), 10e-5);
            Assert.AreEqual(123M, decimal.Parse(model[3]));
            Assert.AreEqual(123, int.Parse(model[4]));
        }

        [TestMethod]
        public void Collection()
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
            Assert.AreEqual(123.123, double.Parse(model[2]), 10e-5);
            Assert.AreEqual(123M, decimal.Parse(model[3]));
            Assert.AreEqual(123, int.Parse(model[4]));
        }

        [TestMethod]
        public void Dynamic()
        {
            _connection.Open();
            var entity = new AdoHelper<dynamic>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReader();
            _connection.Close();

            var model = entity.First();

            Assert.AreEqual(5, ((IDictionary<String, Object>)model).Count);

            Assert.AreEqual("Hello", model.TEXTFIELD);
            Assert.AreEqual(123.123, model.FLOATFIELD, 10e-5);
            Assert.AreEqual(123, model.NUMERICFIELD);
            Assert.AreEqual(123, model.INTEGERFIELD);
        }

        [TestMethod]
        public void EqualNamesDifferentCase()
        {
            _connection.Open();
            var entity = new AdoHelper<OverloadedTestEntity>(_connection)
                .Query("SELECT * FROM TestTable ORDER BY id")
                .ExecuteReader()
                .FirstOrDefault();
            _connection.Close();

            Assert.AreEqual(1, entity.Id);
            Assert.AreEqual(1f, entity.id);
            Assert.AreEqual("Hello", entity.TextField);
            Assert.AreEqual("Hello", entity.textField);
        }
    }
}
