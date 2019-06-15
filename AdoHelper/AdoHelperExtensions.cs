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

                                MemberInfo memberInfo = GetAppropriateMember(modelType.GetMember(structure.mapFieldName), structure);
                                if (memberInfo == null)
                                    continue;

                                value = Convert.ChangeType(value, structure.innerType);

                                Type memberType;
                                if (structure.mapFieldType == FieldMapInfo.FieldType.Field)
                                {
                                    memberType = (memberInfo as FieldInfo).FieldType;
                                }
                                else if (structure.mapFieldType == FieldMapInfo.FieldType.Property)
                                {
                                    memberType = (memberInfo as PropertyInfo).PropertyType;
                                }
                                else
                                    continue;

                                if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    var targetType = Nullable.GetUnderlyingType(memberType);
                                    value = Convert.ChangeType(value, targetType);
                                }

                                if (structure.mapFieldType == FieldMapInfo.FieldType.Field)
                                {
                                    (memberInfo as FieldInfo).SetValue(boxedModel, value);
                                }
                                else if(structure.mapFieldType == FieldMapInfo.FieldType.Property)
                                {
                                    (memberInfo as PropertyInfo).SetValue(boxedModel, value, null);
                                }
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

                                    MemberInfo memberInfo = GetAppropriateMember(modelType.GetMember(structure.mapFieldName), structure);
                                    if (memberInfo == null)
                                        continue;

                                    value = Convert.ChangeType(value, structure.innerType);

                                    if (structure.mapFieldType == FieldMapInfo.FieldType.Field)
                                    {
                                        (memberInfo as FieldInfo).SetValue(boxedModel, value);
                                    }
                                    else if (structure.mapFieldType == FieldMapInfo.FieldType.Property)
                                    {
                                        (memberInfo as PropertyInfo).SetValue(boxedModel, value, null);
                                    }

                                    //memberInfo.SetValue(boxedModel, value, null);
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

        private static MemberInfo GetAppropriateMember(MemberInfo[] members, FieldMapInfo structure)
        {
            MemberTypes appropriateMemberType;
            switch (structure.mapFieldType)
            {
                case FieldMapInfo.FieldType.Field:
                    appropriateMemberType = MemberTypes.Field;
                    break;
                case FieldMapInfo.FieldType.Property:
                    appropriateMemberType = MemberTypes.Property;
                    break;
                default:
                    appropriateMemberType = MemberTypes.Property;
                    break;
            }

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].MemberType == appropriateMemberType)
                    return members[i];
            }

            return null;
        }
    }
}
