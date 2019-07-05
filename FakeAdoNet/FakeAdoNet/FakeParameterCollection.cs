using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace AdoHelper.FakeAdoNet
{
    public class FakeParameterCollection : DbParameterCollection
    {
        private List<FakeParameter> _parameters = new List<FakeParameter>();

        public override int Count => _parameters.Count;

        public override object SyncRoot => throw new NotImplementedException();

        public override int Add(object value)
        {
            if (value is FakeParameter param)
            {
                _parameters.Add(param);
                return _parameters.Count - 1;
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }

        public override void AddRange(Array values)
        {
            foreach (var value in values)
            {
                if (value is FakeParameter param)
                {
                    _parameters.Add(param);
                }
                else
                    throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
            }
        }

        public override void Clear()
        {
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            if (value is FakeParameter param)
            {
                return _parameters.Contains(param);
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }

        public override bool Contains(string value)
        {
            return _parameters.Any(u => u.ParameterName.ToLower() == value.Trim().ToLower());
        }

        public override void CopyTo(Array array, int index)
        {
            if(array is FakeParameter[] paramArray)
                _parameters.CopyTo(paramArray, index);
            else
                throw new ArgumentException("PCol002: Incorrect param array type. Should be FakeParameter[]");
        }

        public override IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            if (value is FakeParameter param)
            {
                return _parameters.IndexOf(param);
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }

        public override int IndexOf(string parameterName)
        {
            var item = _parameters.FirstOrDefault(u => u.ParameterName.ToLower() == parameterName.Trim().ToLower());
            if(item == null) return -1;
            return _parameters.IndexOf(item);
        }

        public override void Insert(int index, object value)
        {
            if (value is FakeParameter param)
            {
                _parameters.Insert(index, param);
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }

        public override void Remove(object value)
        {
            if (value is FakeParameter param)
            {
                _parameters.Remove(param);
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }

        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            var item = _parameters.FirstOrDefault(u => u.ParameterName.ToLower() == parameterName.Trim().ToLower());
            if (item == null) throw new ArgumentException("PCol003: Parameter with such name is not exist");
            _parameters.Remove(item);
        }

        protected override DbParameter GetParameter(int index)
        {
            return _parameters[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            var item = _parameters.FirstOrDefault(u => u.ParameterName.ToLower() == parameterName.Trim().ToLower());
            if (item == null) throw new ArgumentException("PCol003: Parameter with such name is not exist");
            return item;
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            if (value is FakeParameter param)
            {
                _parameters[index] = param;
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var item = _parameters.FirstOrDefault(u => u.ParameterName.ToLower() == parameterName.Trim().ToLower());
            if (item == null) throw new ArgumentException("PCol003: Parameter with such name is not exist");
            if (value is FakeParameter param)
            {
                item = param;
            }
            else
                throw new ArgumentException("PCol001: Incorrect param type. Should be FakeParameter");
        }
    }
}
