using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoHelper
{
    public static partial class AdoHelperExtensions
    {
        /// <summary>
        /// <para>Sets the query parameters</para>
        /// <para>Is optional method</para>
        /// <para>Params can be Tuple&lt;string,object&gt; or ValueTuple&lt;string,object&gt;</para>
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Query info</returns>
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

        /// <summary>
        /// <para>Sets the query transaction</para>
        /// <para>Is optional method</para>
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <param name="transaction">ADO.NET transaction</param>
        /// <returns>Query info</returns>
        public static QueryInfo<T> Transaction<T>(this QueryInfo<T> queryInfo, IDbTransaction transaction)
        {
            queryInfo.Transaction = transaction;
            queryInfo.Command.Transaction = transaction;

            return queryInfo;
        }

        /// <summary>
        /// Executes query and returns a single value
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <returns>Return value</returns>
        public static T ExecuteScalar<T>(this QueryInfo<T> queryInfo)
        {
            T model;
            object value = queryInfo.Command.ExecuteScalar();
            Type modelType = typeof(T);

            try
            {
                if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var targetType = Nullable.GetUnderlyingType(modelType);
                    value = Convert.ChangeType(value, targetType);
                }
                else
                {
                    if (value == null)
                        value = default(T);
                    else
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

        /// <summary>
        /// Asynchronously executes query and returns a single value
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Return value</returns>
        public static async Task<T> ExecuteScalarAsync<T>(this QueryInfo<T> queryInfo, CancellationToken? cancellationToken = null)
        {
            T model;
            Type modelType = typeof(T);
            object value;
            try
            {
                if (queryInfo.Command is DbCommand command)
                {
                    value = await (cancellationToken == null ?
                        command.ExecuteScalarAsync() :
                        command.ExecuteScalarAsync(cancellationToken.Value));
                }
                else
                {
                    value = await (cancellationToken == null ?
                        Task.Factory.StartNew<T>(() => ExecuteScalar(queryInfo)) :
                        Task.Factory.StartNew<T>(() => ExecuteScalar(queryInfo), cancellationToken.Value));
                }

                if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var targetType = Nullable.GetUnderlyingType(modelType);
                    value = Convert.ChangeType(value, targetType);
                }
                else
                {
                    if (value == null)
                        value = default(T);
                    else
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

        /// <summary>
        /// Executes query and returns number of changed rows
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <returns>Count of changed rows</returns>
        public static int ExecuteNonQuery<T>(this QueryInfo<T> queryInfo)
        {
            int value;
            try
            {
                value = queryInfo.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
        }

        /// <summary>
        /// Asynchronously executes query and returns number of changed rows
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Count of changed rows</returns>
        public static async Task<int> ExecuteNonQueryAsync<T>(this QueryInfo<T> queryInfo, CancellationToken? cancellationToken = null)
        {
            int value;
            try
            {
                if (queryInfo.Command is DbCommand command)
                {
                    value = await (cancellationToken == null ?
                        command.ExecuteNonQueryAsync() :
                        command.ExecuteNonQueryAsync(cancellationToken.Value));
                }
                else
                {
                    value = await (cancellationToken == null ?
                        Task.Factory.StartNew<int>(() => ExecuteNonQuery(queryInfo)) :
                        Task.Factory.StartNew<int>(() => ExecuteNonQuery(queryInfo), cancellationToken.Value));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
        }

        /// <summary>
        /// Executes query and return a collection of mapped entities
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <returns>Mapped entities collection</returns>
        public static IEnumerable<T> ExecuteReader<T>(this QueryInfo<T> queryInfo)
        {
            List<T> enumerable = new List<T>();

            switch (queryInfo.ModelType)
            {
                case QueryInfo<T>.ModelEntityType.Object:
                    enumerable = ExecuteObjectReader(queryInfo);
                    break;
                case QueryInfo<T>.ModelEntityType.Tuple:
                    enumerable = ExecuteTupleReader(queryInfo);
                    break;
                case QueryInfo<T>.ModelEntityType.Collection:
                    enumerable = ExecuteCollectionReader(queryInfo);
                    break;
            }

            return enumerable;
        }

        /// <summary>
        /// Asynchronously executes query and return a collection of mapped entities
        /// </summary>
        /// <typeparam name="T">Return entity type</typeparam>
        /// <param name="queryInfo">Query info</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Mapped entities collection</returns>
        public static async Task<IEnumerable<T>> ExecuteReaderAsync<T>(this QueryInfo<T> queryInfo, CancellationToken? cancellationToken = null)
        {
            List<T> enumerable = new List<T>();

            try
            {
                switch (queryInfo.ModelType)
                {
                    case QueryInfo<T>.ModelEntityType.Object:
                        enumerable = await ExecuteObjectReaderAsync(queryInfo, cancellationToken);
                        break;
                    case QueryInfo<T>.ModelEntityType.Tuple:
                        enumerable = await ExecuteTupleReaderAsync(queryInfo, cancellationToken);
                        break;
                    case QueryInfo<T>.ModelEntityType.Collection:
                        enumerable = await ExecuteCollectionReaderAsync(queryInfo, cancellationToken);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return enumerable;
        }
    }
}
