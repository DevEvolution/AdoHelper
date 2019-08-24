using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace AdoHelper.UnitTests.SQLite
{
    public class SQLiteDbConfig
    {
        protected const string CONNECTION_STRING = "Data Source=testdb.db; Version=3;";

        protected IDbConnection _connection = new SQLiteConnection(CONNECTION_STRING);
    }
}
