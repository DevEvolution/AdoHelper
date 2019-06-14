using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace AdoHelper
{
    public static class AdoHelperExtensions
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

        //public static T Execute<T>(this QueryInfo<T> queryInfo)
        //{
        //    T model = Activator.CreateInstance<T>();
        //    Type modelType = typeof(T);

        //    using (IDataReader reader = queryInfo.Command.ExecuteReader())
        //    {
        //        if (reader.Read())
        //        {
        //            foreach (ModelStructure structure in queryInfo.ModelStructureTable)
        //            {
        //                if (structure.isNullable)
        //                {
        //                    object value = (reader[structure.fieldName].Equals(System.DBNull.Value)) ?
        //                        null : reader[structure.fieldName];
        //                    PropertyInfo property = modelType.GetProperty(structure.propertyName);
        //                    Type propertyType = property.PropertyType;
        //                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //                    {
        //                        var targetType = Nullable.GetUnderlyingType(propertyType);
        //                        value = Convert.ChangeType(value, targetType);
        //                    }
        //                    property.SetValue(model, value, null);
        //                }
        //                else
        //                {
        //                    if (!(reader[structure.fieldName].Equals(System.DBNull.Value)))
        //                    {
        //                        object value = reader[structure.fieldName];
        //                        PropertyInfo property = modelType.GetProperty(structure.propertyName);
        //                        property.SetValue(model, value, null);
        //                    }
        //                    else
        //                    {
        //                        throw new ArgumentNullException("Reader trying to pass DbNull value to non-nullable model");
        //                    }
        //                }
        //            }
        //        }
        //        else
        //            return default(T);
        //    }
        //    return model;
        //}

        public static T ExecuteScalar<T>(this QueryInfo<T> queryInfo)
        {
            T value;
            try
            {
                value = (T)Convert.ChangeType(queryInfo.Command.ExecuteScalar(), typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
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

            using (IDataReader reader = queryInfo.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    T model = Activator.CreateInstance<T>();

                    // For struct support
                    object boxedModel = model;

                    foreach (FieldMapInfo structure in queryInfo.ModelStructureTable)
                    {
                        if (structure.isNullable)
                        {
                            try
                            {
                                object value = (reader[structure.dbFieldName].Equals(System.DBNull.Value)) ?
                                    null : reader[structure.dbFieldName];

                                PropertyInfo property = modelType.GetProperty(structure.mapFieldName);

                                Convert.ChangeType(value, structure.innerType);

                                Type propertyType = property.PropertyType;
                                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    var targetType = Nullable.GetUnderlyingType(propertyType);
                                    value = Convert.ChangeType(value, targetType);
                                }
                                property.SetValue(boxedModel, value, null);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        else
                        {
                            if (!(reader[structure.dbFieldName].Equals(System.DBNull.Value)))
                            {
                                try
                                {
                                    object value = reader[structure.dbFieldName];
                                    PropertyInfo property = modelType.GetProperty(structure.mapFieldName);
                                    property.SetValue(boxedModel, value, null);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            else
                            {
                                throw new ArgumentNullException("Reader trying to pass DbNull value to non-nullable model");
                            }
                        }
                    }
                    model = (T)boxedModel;

                    enumerable.Add(model);
                }
            }
            return enumerable;
        }
    }
}
