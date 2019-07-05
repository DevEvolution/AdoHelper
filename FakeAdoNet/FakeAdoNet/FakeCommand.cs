using AdoHelper.FakeAdoNet.FakeContainer;
using AdoHelper.FakeAdoNet.FakeQueries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeCommand : DbCommand
    {
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }

        private FakeParameterCollection _parameterCollection = new FakeParameterCollection();
        protected override DbParameterCollection DbParameterCollection => _parameterCollection;

        protected override DbTransaction DbTransaction { get; set; }

        public FakeCommand()
        {

        }

        public FakeCommand(FakeConnection connection)
        {
            DbConnection = connection;
        }

        public FakeCommand(FakeConnection connection, string commandText)
        {
            DbConnection = connection;
            CommandText = commandText;
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            object rezult = ParseCommand();
            return int.Parse(rezult.ToString());
        }

        public override object ExecuteScalar()
        {
            object rezult = ParseCommand();
            return rezult;
        }

        public new FakeDataReader ExecuteReader()
        {
            object rezult = ParseCommand();
            return new FakeDataReader() { Rezults = rezult as List<List<object>> };
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            return new FakeParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            object rezult = ParseCommand();
            return new FakeDataReader() { Rezults = rezult as List<List<object>> };
        }

        private object ParseCommand()
        {
            if (Connection.State != ConnectionState.Open)
                throw new Exception("Cmd001: Connection must be open before calling a command");

            foreach (FakeParameter param in _parameterCollection)
            {
                CommandText = CommandText.Replace(param.ParameterName, param.Value.ToString());
            }

            FakeQuery operations = JsonConvert.DeserializeObject<FakeQuery>(CommandText);

            object result = null;
            {
                {
                    if (operations.Create != null)
                    {
                        if (operations.Create.Table == null)
                            throw new FakeException("Create:Table is not specified");

                        {
                            FakeDatabase db = JsonConvert.DeserializeObject<FakeDatabase>(File.ReadAllText(Connection.Database));
                            if (db == null)
                                db = new FakeDatabase() { Name = Connection.Database };

                            if (db.Tables.Find(x => x.Name == operations.Create.Table.Name) != null)
                                throw new FakeException("Create:Table is already exists");

                            db.Tables.Add(operations.Create.Table);
                            File.WriteAllText(Connection.Database, JsonConvert.SerializeObject(db));
                        }
                        result = 1;
                    }
                    else if (operations.Insert != null)
                    {
                        int rowsAffected = 0;
                        {
                            FakeDatabase db = JsonConvert.DeserializeObject<FakeDatabase>(File.ReadAllText(Connection.Database));
                            var table = db.Tables.Find(x => x.Name == operations.Insert.Into);
                            if (table == null)
                                throw new FakeException("Insert:Table is not exists");

                            List<object> rows = new List<object>();
                            for (int i = 0; i < table.Headers.Count; i++)
                            {
                                rows.Add(new object());
                            }

                            for (int i = 0; i < operations.Insert.Values.Count; i++)
                            {
                                int index = i % operations.Insert.Headers.Count;

                                var header = table.Headers.Find(x => x.Name == operations.Insert.Headers[index]);
                                if (header.Attributes.Contains(FakeDbAttribute.NotNull) && operations.Insert.Values[i] == null)
                                    throw new FakeException($"Insert: Field {operations.Insert.Headers[index]} has NotNull attribute, but passed value is null");

                                rows[i] = operations.Insert.Values[i];

                                if (i % operations.Insert.Headers.Count == operations.Insert.Headers.Count - 1)
                                {
                                    table.Rows.Add(rows);
                                    rows = new List<object>(table.Headers.Count);
                                    rowsAffected++;
                                }
                            }

                            File.WriteAllText(Connection.Database, JsonConvert.SerializeObject(db));
                        }

                        result = rowsAffected;
                    }
                    else if (operations.Update != null)
                    {
                        int rowsAffected = 0;
                        {
                            FakeDatabase db = JsonConvert.DeserializeObject<FakeDatabase>(File.ReadAllText(Connection.Database));
                            var table = db.Tables.Find(x => x.Name == operations.Update.Table);

                            // Parse where
                            string[] words = operations.Update.Where.Split('=');
                            int index = table.Headers.FindIndex(u => u.Name == words[0]);
                            foreach (var row in table.Rows.Where(u => u[index].ToString() == words[1].Trim()))
                            {
                                for (int i = 0; i < operations.Update.To.Count; i++)
                                {
                                    int idx = table.Headers.FindIndex(x => x.Name.ToLower() == operations.Update.To[i].Field.Trim().ToLower());
                                    row[idx] = operations.Update.To[i].Value;
                                }
                                rowsAffected++;
                            }

                            File.WriteAllText(Connection.Database, JsonConvert.SerializeObject(db));
                        }

                        result = rowsAffected;
                    }
                    else if (operations.Delete != null)
                    {
                        int rowsAffected = 0;
                        {
                            FakeDatabase db = JsonConvert.DeserializeObject<FakeDatabase>(File.ReadAllText(Connection.Database));
                            var table = db.Tables.Find(x => x.Name == operations.Delete.From);

                            // Parse where
                            string[] words = operations.Delete.Where.Split('=');
                            int index = table.Headers.FindIndex(u => u.Name == words[0]);

                            var rowsToDelete = table.Rows.Where(u => u[index].ToString() == words[1].Trim()).ToList();

                            for (int i = 0; i < rowsToDelete.Count; i++)
                            {
                                table.Rows.Remove(rowsToDelete[i]);
                                rowsAffected++;
                            }

                            File.WriteAllText(Connection.Database, JsonConvert.SerializeObject(db));
                        }

                        result = rowsAffected;
                    }
                    else if (operations.Select != null)
                    {
                        {
                            FakeDatabase db = JsonConvert.DeserializeObject<FakeDatabase>(File.ReadAllText(Connection.Database));
                            var table = db.Tables.Find(x => x.Name == operations.Select.From);

                            List<List<object>> rowsToSelect;

                            if (operations.Select.Where != null)
                            {
                                // Where filter
                                string[] words = operations.Select.Where.Split('=');
                                int index = table.Headers.FindIndex(u => u.Name == words[0]);
                                rowsToSelect = table.Rows.Where(u => u[index].ToString() == words[1].Trim()).ToList();
                            }
                            else
                            {
                                // Without where
                                rowsToSelect = table.Rows;
                            }
                            List<List<object>> tableResults = new List<List<object>>();
                            List<object> headers = new List<object>();
                            List<int> indices = new List<int>();
                            if (operations.Select.Fields != null && operations.Select.Fields.Count > 0)
                            {
                                // Fields filter
                                for (int i = 0; i < operations.Select.Fields.Count; i++)
                                {
                                    int indexOfHeader = table.Headers.FindIndex(u => u.Name == operations.Select.Fields[i]);
                                    if (indexOfHeader == -1) throw new FakeException($"Select: Field {operations.Select.Fields[i]} is not exists");
                                    headers.Add(operations.Select.Fields[i]);
                                    indices.Add(indexOfHeader);
                                }
                            }
                            else
                            {
                                // All fields
                                for (int i = 0; i < table.Headers.Count; i++)
                                {
                                    headers.Add(table.Headers[i].Name);
                                    indices.Add(i);
                                }
                            }
                            tableResults.Add(headers);

                            foreach (List<object> row in rowsToSelect)
                            {
                                List<object> fieldRow = new List<object>();
                                for (int i = 0; i < indices.Count; i++)
                                {
                                    fieldRow.Add(row[indices[i]]);
                                }
                                tableResults.Add(fieldRow);
                            }

                            result = tableResults;
                        }
                    }
                }
            }

            return result;
        }

    }
}
