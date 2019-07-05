using AdoHelper.FakeAdoNet.FakeContainer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeConnection : DbConnection
    {
        private string _connectionString = null;
        private string _database = null;
        private ushort _serverVersion = 1;
        private ConnectionState _connectionState = ConnectionState.Closed;

        private FakeDatabase _db;

        public override string ConnectionString { get => _connectionString; set { ParseConnectionString(value); _connectionString = value; } }

        public override string Database => _database;

        public override string DataSource => _database;

        public override string ServerVersion => _serverVersion.ToString();

        public override ConnectionState State => _connectionState;

        public FakeConnection() : base()
        { }

        public FakeConnection(string connectionString) : base()
        {
            ParseConnectionString(connectionString);
            ConnectionString = connectionString;
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            _connectionState = ConnectionState.Closed;
        }

        public override void Open()
        {
            if (!File.Exists(_database))
            {
                File.Create(_database);
            }
            _connectionState = ConnectionState.Open;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new FakeTransaction();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new FakeCommand(this);
        }

        protected void ParseConnectionString(string connectionString)
        {
            string[] phrases = connectionString.ToLower().Trim().Split(';');
            if (phrases.Length < 1) throw new ArgumentException("Con001: Not enough phrases in connection string");

            foreach (string phrase in phrases)
            {
                string[] words = phrase.Split('=');
                if (words.Length != 2) throw new ArgumentException("Con002: Not enough words in phrases in connection string");

                switch (words[0].Trim())
                {
                    case "data source":
                        {
                            _database = words[1].Trim();
                            break;
                        }
                    case "database":
                        {
                            _database = words[1].Trim();
                            break;
                        }
                    case "version":
                        {
                            if (!ushort.TryParse(words[1].Trim(), out _serverVersion))
                            {
                                throw new ArgumentException("Con003: Version must be a unsigned number");
                            }
                            
                            break;
                        }

                }
            }
        }
    }
}
