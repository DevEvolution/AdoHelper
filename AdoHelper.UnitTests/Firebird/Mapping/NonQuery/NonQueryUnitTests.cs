using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.Firebird.Mapping.NonQuery
{
    [TestClass]
    public class NonQueryUnitTests : FirebirdDbConfig
    {
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
