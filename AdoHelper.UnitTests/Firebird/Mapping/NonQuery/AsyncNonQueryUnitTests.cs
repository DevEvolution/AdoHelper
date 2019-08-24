using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoHelper.UnitTests.Firebird.Mapping.NonQuery
{
    [TestClass]
    public class AsyncNonQueryUnitTests : FirebirdDbConfig
    {
        [TestMethod]
        public async Task ComplexTransaction()
        {
            _connection.Open();

            int defaultCount = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync();

            var transaction = _connection.BeginTransaction();
            await new AdoHelper<int>(_connection)
                .Query("INSERT INTO TestTable (id, TextField, FloatField, NumericField, IntegerField) VALUES (@id, @text, @float, @decimal, @int)")
                .Parameters(
                ("@id", 5),
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

        [TestMethod]
        public async Task ComplexTransaction_Cancel()
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
                .Query("INSERT INTO TestTable (id, TextField, FloatField, NumericField, IntegerField) VALUES (@id, @text, @float, @decimal, @int)")
                .Parameters(
                ("@id", 5),
                ("@text", "Test hello"),
                ("@float", 9.09),
                ("@decimal", 193.123),
                ("@int", 85))
                .Transaction(transaction)
                .ExecuteNonQueryAsync(token));

            transaction.Rollback();

            int addCount = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync();

            source.Dispose();
            _connection.Close();

            Assert.IsTrue(addCount == defaultCount);
        }
    }
}
