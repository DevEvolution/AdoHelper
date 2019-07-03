using AdoHelper.TupleParsing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace AdoHelper
{
    public static partial class AdoHelperExtensions
    {
        private static List<T> ExecuteTupleReader<T>(QueryInfo<T> queryInfo, Type modelType)
        {
            List<T> enumerable = new List<T>();
            using (IDataReader reader = queryInfo.Command.ExecuteReader())
            {
                int itemCount = queryInfo.ModelStructureTable.Count;

                while (reader.Read())
                {
                    List<object> parameters = new List<object>();

                    if (reader.FieldCount != itemCount)
                        throw new ArgumentException("Number of items in tuple should be equal to number of fields in query columns");

                    int index = 0;

                    foreach (FieldMapInfo structure in queryInfo.ModelStructureTable)
                    {
                        try
                        {
                            object value = (reader[index].Equals(System.DBNull.Value)) ?
                                    null : reader[index];

                            value = Convert.ChangeType(value, structure.innerType);
                            parameters.Add(value);
                            index++;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    T model = ObjectCreator.CreateTuple<T>(parameters);

                    enumerable.Add(model);
                }
            }
            return enumerable;
        }

        private static List<T> ExecuteObjectReader<T>(QueryInfo<T> queryInfo, Type modelType)
        {
            List<T> enumerable = new List<T>();

            using (IDataReader reader = queryInfo.Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    T model = ObjectCreator.Create<T>();

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
                                else if (structure.mapFieldType == FieldMapInfo.FieldType.Property)
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


        private static List<T> ExecuteCollectionReader<T>(QueryInfo<T> queryInfo, Type modelType)
        {
            List<T> enumerable = new List<T>();
            using (IDataReader reader = queryInfo.Command.ExecuteReader())
            {
                FieldMapInfo structure = queryInfo.ModelStructureTable[0];

                while (reader.Read())
                {
                    List<object> parameters = new List<object>();

                    object collection = ObjectCreator.CreateEnumerable(modelType, new List<object>());
                    MethodInfo addMethod = collection.GetType().GetMethod("Add");
                    if (addMethod == null)
                        throw new NotSupportedException("Collection should implement 'void Add(object value)' method");

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        object value = (reader[i].Equals(System.DBNull.Value)) ?
                                    null : reader[i];

                        value = Convert.ChangeType(value, structure.innerType);
                        addMethod.Invoke(collection, new object[] { value });
                    }

                    enumerable.Add((T)collection);
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
