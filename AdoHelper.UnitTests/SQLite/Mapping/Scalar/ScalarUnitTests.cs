using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdoHelper.UnitTests.SQLite.Mapping.Scalar
{
    [TestClass]
    public class ScalarUnitTests : SQLiteDbConfig
    {
        [TestMethod]
        public void ScalarQuery_Int()
        {
            _connection.Open();
            var scalar = new AdoHelper<int>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            _connection.Close();

            Assert.IsTrue(scalar > 0);
        }

        [TestMethod]
        public void ScalarQuery_NullableInt()
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
        public void ScalarQuery_Float()
        {
            _connection.Open();
            var scalar = new AdoHelper<float>(_connection)
                .Query("SELECT COUNT(*) FROM TestTable")
                .ExecuteScalar();
            _connection.Close();

            Assert.IsTrue(scalar > 0);
        }
    }
}
