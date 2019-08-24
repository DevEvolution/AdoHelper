using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoHelper.UnitTests.Firebird.Mapping.Scalar
{
    [TestClass]
    public class AsyncScalarUnitTests : FirebirdDbConfig
    {
        [TestMethod]
        public async Task ScalarQuery_Int()
        {
            _connection.Open();
            var scalar = await new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync();
            _connection.Close();

            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public async Task ScalarQuery_Cancel()
        {
            _connection.Open();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            source.Cancel();
            Task<int> task = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalarAsync(token);

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => { await task; });
            source.Dispose();
            _connection.Close();
        }
    }
}
