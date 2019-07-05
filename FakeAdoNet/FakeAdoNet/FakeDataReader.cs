using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeDataReader : DbDataReader
    {
        public List<List<object>> Rezults { get; set; }

        private int currentRow = 0;

        public override object this[int ordinal] => Rezults[currentRow][ordinal];

        public override object this[string name] => Rezults[currentRow][Rezults[0].FindIndex(x=> x.ToString().ToLower() == name.ToLower())];

        public override int Depth => currentRow - 1;

        public override int FieldCount => Rezults[0].Count;

        public override bool HasRows => Rezults.Count > 1;

        public override bool IsClosed => throw new NotImplementedException();

        public override int RecordsAffected => throw new NotImplementedException();

        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            return Rezults[currentRow][ordinal].GetType().Name;
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            return Rezults.GetEnumerator();
        }

        public override Type GetFieldType(int ordinal)
        {
            return Rezults[currentRow][ordinal].GetType();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal)
        {
            return Rezults[0][ordinal].ToString();
        }

        public override int GetOrdinal(string name)
        {
            return Rezults[0].FindIndex(x => x.ToString().ToLower() == name.ToLower());
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            return Rezults[currentRow][ordinal] == null;
        }

        public override bool NextResult()
        {
            if (currentRow + 1 >= Rezults.Count)
                return false;

            return true;
        }

        public override bool Read()
        {
            if (currentRow + 1 >= Rezults.Count)
                return false;

            currentRow++;
            return true;
        }
    }
}
