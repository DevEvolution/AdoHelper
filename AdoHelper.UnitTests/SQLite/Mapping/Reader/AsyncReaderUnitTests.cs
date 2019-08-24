using AdoHelper.UnitTests.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoHelper.UnitTests.SQLite.Mapping.Reader
{
    [TestClass]
    public class AsyncReaderUnitTests : SQLiteDbConfig
    {
        [TestMethod]
        public async Task Simple()
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
        public async Task Simple_Cancel()
        {
            _connection.Open();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await new AdoHelper<SimpleTestEntity>(_connection)
               .Query("SELECT * FROM TestTable WHERE IntegerField = @intValue")
               .Parameters(new AdoParameter("@intValue", 123))
               .ExecuteReaderAsync(token));
            source.Dispose();
            _connection.Close();
        }

        [TestMethod]
        public async Task Dynamic()
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
        public async Task Dynamic_Cancel()
        {
            _connection.Open();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(async () => await new AdoHelper<dynamic>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReaderAsync(token)
            );
            source.Dispose();
            _connection.Close();
        }
    }
}
