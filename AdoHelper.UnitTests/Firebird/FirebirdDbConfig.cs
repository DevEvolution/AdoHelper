using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdoHelper.UnitTests.Firebird
{
    public class FirebirdDbConfig
    {
        protected const string CONNECTION_STRING = "user id=SYSDBA;password=masterkey;initial catalog=testdb.fdb;server type=Embedded";

        protected IDbConnection _connection = new FbConnection(CONNECTION_STRING);
    }
}
