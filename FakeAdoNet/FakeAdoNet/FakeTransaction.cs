using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeTransaction : DbTransaction
    {
        private FakeConnection _connection;
        private IsolationLevel _isolationLevel = IsolationLevel.Chaos;
        public override IsolationLevel IsolationLevel => _isolationLevel;

        protected override DbConnection DbConnection => _connection;

        public override void Commit()
        {
            throw new NotImplementedException();
        }

        public override void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
