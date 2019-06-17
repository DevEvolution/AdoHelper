using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace AdoHelper
{
    public static partial class AdoHelperExtensions
    {
        public static QueryInfo<T> Parameters<T>(this QueryInfo<T> queryInfo, params AdoParameter[] parameters)
        {
            queryInfo.QueryInfoParameters = new List<IDbDataParameter>();
            for (int i = 0; i < parameters.Length; i++)
            {
                IDbDataParameter param = queryInfo.Command.CreateParameter();
                param.ParameterName = parameters[i].Name;
                param.Value = parameters[i].Value;
                queryInfo.Command.Parameters.Add(param);
                queryInfo.QueryInfoParameters.Add(param);
            }

            return queryInfo;
        }

        public static QueryInfo<T> Transaction<T>(this QueryInfo<T> queryInfo, IDbTransaction transaction)
        {
            queryInfo.Transaction = transaction;
            queryInfo.Command.Transaction = transaction;

            return queryInfo;
        }

        public static T ExecuteScalar<T>(this QueryInfo<T> queryInfo)
        {
            T model;
            object value = queryInfo.Command.ExecuteScalar();
            Type modelType = typeof(T);

            // TODO: What if DbNull ?

            try
            {
                if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var targetType = Nullable.GetUnderlyingType(modelType);
                    value = Convert.ChangeType(value, targetType);
                }
                else
                {
                    value = Convert.ChangeType(value, modelType);
                }
                model = (T)value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public static void ExecuteNonQuery<T>(this QueryInfo<T> queryInfo)
        {
            try
            {
                queryInfo.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<T> ExecuteReader<T>(this QueryInfo<T> queryInfo)
        {
            Type modelType = typeof(T);
            List<T> enumerable = new List<T>();

            switch (queryInfo.ModelType)
            {
                case QueryInfo<T>.ModelEntityType.Object:
                    enumerable = ExecuteObjectReader(queryInfo, modelType);
                    break;
                case QueryInfo<T>.ModelEntityType.ValueTuple:
                    enumerable = ExecuteValueTupleReader(queryInfo, modelType);
                    break;
                case QueryInfo<T>.ModelEntityType.Tuple:
                    enumerable = ExecuteTupleReader(queryInfo, modelType);
                    break;
                case QueryInfo<T>.ModelEntityType.GenericObject:
                    break;
            }

            return enumerable;
        }
    }
}
