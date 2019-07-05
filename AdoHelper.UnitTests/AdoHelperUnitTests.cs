using AdoHelper.FakeAdoNet;
using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdoHelper.UnitTests
{
    [TestClass]
    public class AdoHelperUnitTests
    {
        private const string CONNECTION_STRING = "Data Source=faketest.json; Version=1";

        private IDbConnection _connection = new FakeConnection(CONNECTION_STRING);

        [TestMethod]
        public void PrimaryMapping_Int32()
        {
            var adoHelper = new AdoHelper<int>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<int>.ModelEntityType.Object, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);
        }

        [TestMethod]
        public void PrimaryMapping_String()
        {
            var adoHelper = new AdoHelper<string>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<string>.ModelEntityType.Object, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);
        }

        [TestMethod]
        public void PrimaryMapping_Class()
        {
            var adoHelper = new AdoHelper<SimpleTestEntity>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<SimpleTestEntity>.ModelEntityType.Object, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(5, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable.Find(x => x.MapFieldName == "Id");
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.Property, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(long), mappingInfo.FullType);
            Assert.AreEqual(typeof(long), mappingInfo.InnerType);
            Assert.AreEqual("id", mappingInfo.DbFieldName);
        }

        [TestMethod]
        public void PrimaryMapping_Struct()
        {
            var adoHelper = new AdoHelper<StructTestEntity>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<StructTestEntity>.ModelEntityType.Object, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(5, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable.Find(x => x.MapFieldName == "Id");
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.Property, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(long), mappingInfo.FullType);
            Assert.AreEqual(typeof(long), mappingInfo.InnerType);
            Assert.AreEqual("id", mappingInfo.DbFieldName);
        }

        [TestMethod]
        public void PrimaryMapping_NamedFields()
        {
            var adoHelper = new AdoHelper<NamedTestEntity>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<NamedTestEntity>.ModelEntityType.Object, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(5, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable.Find(x => x.MapFieldName == "Test_Id");
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.Property, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(long), mappingInfo.FullType);
            Assert.AreEqual(typeof(long), mappingInfo.InnerType);
            Assert.AreEqual("id", mappingInfo.DbFieldName);
        }

        [TestMethod]
        public void PrimaryMapping_ExcludedFields()
        {
            var adoHelper = new AdoHelper<ExcludedFieldTestEntity>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<ExcludedFieldTestEntity>.ModelEntityType.Object, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(3, queryInfo.ModelStructureTable.Count);

            Assert.IsNull(queryInfo.ModelStructureTable.Find(x => x.MapFieldName == "TextField"));
            Assert.IsNull(queryInfo.ModelStructureTable.Find(x => x.MapFieldName == "FloatField"));

            MappingInfo mappingInfo = queryInfo.ModelStructureTable.Find(x => x.MapFieldName == "Numeric");
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.Property, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(decimal), mappingInfo.FullType);
            Assert.AreEqual(typeof(decimal), mappingInfo.InnerType);
            Assert.AreEqual("numericfield", mappingInfo.DbFieldName);
        }


        [TestMethod]
        public void PrimaryMapping_Priority_PropertyFirst()
        {
            var adoHelper = new AdoHelper<PropertyPriorityTestEntity>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(3, queryInfo.ModelStructureTable.Count);
            Assert.AreEqual(2, queryInfo.ModelStructureTable.FindAll(x => x.DbFieldName == "textfield").Count);
        }

        [TestMethod]
        public void PrimaryMapping_Tuple()
        {
            var adoHelper = new AdoHelper<Tuple<int, string>>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<Tuple<int, string>>.ModelEntityType.Tuple, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(2, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable[0];
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.Property, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(int), mappingInfo.FullType);
            Assert.AreEqual(typeof(int), mappingInfo.InnerType);
        }

        [TestMethod]
        public void PrimaryMapping_ValueTuple()
        {
            var adoHelper = new AdoHelper<(int, string)>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<(int, string)>.ModelEntityType.Tuple, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(2, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable[0];
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.Field, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(int), mappingInfo.FullType);
            Assert.AreEqual(typeof(int), mappingInfo.InnerType);
        }

        [TestMethod]
        public void PrimaryMapping_Collection_IEnumerable()
        {
            var adoHelper = new AdoHelper<IEnumerable<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<IEnumerable<int>>.ModelEntityType.Collection, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(1, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable[0];
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.CollectionItem, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(int), mappingInfo.FullType);
            Assert.AreEqual(typeof(int), mappingInfo.InnerType);
        }

        [TestMethod]
        public void PrimaryMapping_Collection_ICollection()
        {
            var adoHelper = new AdoHelper<ICollection<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<ICollection<int>>.ModelEntityType.Collection, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(1, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable[0];
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.CollectionItem, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(int), mappingInfo.FullType);
            Assert.AreEqual(typeof(int), mappingInfo.InnerType);
        }

        [TestMethod]
        public void PrimaryMapping_Collection_List()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello");

            Assert.AreEqual(_connection, queryInfo.Connection);
            Assert.AreEqual(QueryInfo<List<int>>.ModelEntityType.Collection, queryInfo.ModelType);
            Assert.AreEqual("Hello", queryInfo.Command.CommandText);

            Assert.AreEqual(1, queryInfo.ModelStructureTable.Count);

            MappingInfo mappingInfo = queryInfo.ModelStructureTable[0];
            Assert.IsNotNull(mappingInfo);
            Assert.AreEqual(MappingInfo.FieldType.CollectionItem, mappingInfo.MapFieldType);
            Assert.AreEqual(false, mappingInfo.IsNullable);
            Assert.AreEqual(typeof(int), mappingInfo.FullType);
            Assert.AreEqual(typeof(int), mappingInfo.InnerType);
        }

        [TestMethod]
        public void TransactionAdd()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            _connection.Open();
            IDbTransaction transaction = _connection.BeginTransaction();
            var queryInfo = adoHelper.Query("Hello").Transaction(transaction);
            _connection.Close();

            Assert.AreEqual(transaction, queryInfo.Transaction);
        }

        [TestMethod]
        public void ParametersAdd_Constructor_Default()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello").Parameters(new AdoParameter());

            Assert.AreEqual(1, queryInfo.QueryInfoParameters.Count);
        }

        [TestMethod]
        public void ParametersAdd_Constructor_Values()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello").Parameters(new AdoParameter("@#$", 123), new AdoParameter("%$#", 3245));

            Assert.AreEqual(2, queryInfo.QueryInfoParameters.Count);
            Assert.AreEqual("%$#", queryInfo.QueryInfoParameters[1].ParameterName);
            Assert.AreEqual(123, queryInfo.QueryInfoParameters[0].Value);
        }

        [TestMethod]
        public void ParametersAdd_Tuple_Values()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello").Parameters(new Tuple<string, object>("@#$", 123), new Tuple<string, object>("%$#", 3245));

            Assert.AreEqual(2, queryInfo.QueryInfoParameters.Count);
            Assert.AreEqual("%$#", queryInfo.QueryInfoParameters[1].ParameterName);
            Assert.AreEqual(123, queryInfo.QueryInfoParameters[0].Value);
        }

        [TestMethod]
        public void ParametersAdd_ValueTuple_Values()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello").Parameters(("@#$", 123), ("%$#", 3245));

            Assert.AreEqual(2, queryInfo.QueryInfoParameters.Count);
            Assert.AreEqual("%$#", queryInfo.QueryInfoParameters[1].ParameterName);
            Assert.AreEqual(123, queryInfo.QueryInfoParameters[0].Value);
        }

        [TestMethod]
        public void ParametersAdd_DbParameter_Values()
        {
            var adoHelper = new AdoHelper<List<int>>(_connection);
            var queryInfo = adoHelper.Query("Hello").Parameters(new FakeParameter("@#$", 123), new FakeParameter("%$#", 3245));

            Assert.AreEqual(2, queryInfo.QueryInfoParameters.Count);
            Assert.AreEqual("%$#", queryInfo.QueryInfoParameters[1].ParameterName);
            Assert.AreEqual(123, queryInfo.QueryInfoParameters[0].Value);
        }
    }
}
