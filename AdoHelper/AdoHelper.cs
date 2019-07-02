using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using AdoHelper.TupleParsing;

namespace AdoHelper
{
    public class AdoHelper<T>
    {
        QueryInfo<T> _queryInfo;
        Type _modelType;

        /// <summary>
        /// Starts ADO.NET query
        /// </summary>
        /// <param name="connection">ADO.NET Connection</param>
        public AdoHelper(IDbConnection connection)
        {
            _queryInfo = new QueryInfo<T>() { Connection = connection };

            _modelType = typeof(T);


            if (_modelType.GetInterface("IEnumerable") != null)
            {
                // Enumerable<Model>
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.GenericObject;
                ParseModelStructure(_modelType);
            }
            else if (TupleAccess.IsTuple(_modelType))
            {
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.Tuple;
                ParseTupleStructure(_modelType);

            }
            else if (ValueTupleAccess.IsValueTuple(_modelType))
            {
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.ValueTuple;
                ParseValueTupleStructure(_modelType);
            }
            else
            {
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.Object;
                ParseModelStructure(_modelType);
            }
        }

        /// <summary>
        /// Sets query text
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns>Query info</returns>
        public QueryInfo<T> Query(string query)
        {
            _queryInfo.Command = _queryInfo.Connection.CreateCommand();
            _queryInfo.Command.CommandText = query;

            return _queryInfo;
        }


        bool IsSimpleType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimpleType(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        void ParseValueTupleStructure(Type modelType)
        {
            _queryInfo.ModelStructureTable = new List<FieldMapInfo>();

            for (int i = 0; i < ValueTupleAccess.ItemCount(modelType); i++)
            {
                FieldMapInfo structure = new FieldMapInfo();
                structure.mapFieldType = FieldMapInfo.FieldType.Field;
                structure.fullType = ValueTupleAccess.GetItemType(modelType, i);
                structure.mapFieldName = String.Empty;

                // Parse type
                ParseInnerType(structure);

                // Add to structure
                _queryInfo.ModelStructureTable.Add(structure);
            }
        }

        void ParseTupleStructure(Type modelType)
        {
            _queryInfo.ModelStructureTable = new List<FieldMapInfo>();

            for (int i = 0; i < TupleAccess.ItemCount(modelType); i++)
            {
                FieldMapInfo structure = new FieldMapInfo();
                structure.mapFieldType = FieldMapInfo.FieldType.Property;
                structure.fullType = TupleAccess.GetItemType(modelType, i);
                structure.mapFieldName = String.Empty;

                // Parse type
                ParseInnerType(structure);

                // Add to structure
                _queryInfo.ModelStructureTable.Add(structure);
            }
        }

        void ParseModelStructure(Type modelType)
        {
            _queryInfo.ModelStructureTable = new List<FieldMapInfo>();

            // Parse properties
            foreach (MemberInfo memberInfo in modelType.GetMembers())
            {
                FieldMapInfo structure = new FieldMapInfo();

                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    structure.mapFieldType = FieldMapInfo.FieldType.Field;
                    structure.fullType = (memberInfo as FieldInfo).FieldType;
                }
                else if (memberInfo.MemberType == MemberTypes.Property && ((memberInfo as PropertyInfo).CanWrite || _queryInfo.ModelType == QueryInfo<T>.ModelEntityType.Tuple))
                {
                    structure.mapFieldType = FieldMapInfo.FieldType.Property;
                    structure.fullType = (memberInfo as PropertyInfo).PropertyType;
                }
                else
                {
                    continue;
                }

                structure.mapFieldName = memberInfo.Name;

                if (!CheckMappingRights(memberInfo))
                    continue;

                // Set field attribute
                SetPropertyMapName(memberInfo, structure);

                // Parse type
                ParseInnerType(structure);

                // Add to structure
                _queryInfo.ModelStructureTable.Add(structure);
            }
        }

        private bool CheckMappingRights(MemberInfo propertyInfo)
            => propertyInfo.GetCustomAttribute<NonMappedAttribute>() == null;    
        

        private void SetPropertyMapName(MemberInfo propertyInfo, FieldMapInfo structure)
        {
            FieldAttribute attribute = propertyInfo.GetCustomAttribute<FieldAttribute>();
            if (attribute != null && attribute.Name != null)
            {
                structure.dbFieldName = attribute.Name;
            }
            else
            {
                structure.dbFieldName = structure.mapFieldName.ToLower();
            }
        }


        private void ParseInnerType(FieldMapInfo structure)
        {
            if (structure.fullType.IsGenericType && structure.fullType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                structure.innerType = structure.fullType.GetGenericArguments()[0];
                structure.isNullable = true;
            }
            else if (structure.fullType.IsClass)
            {
                structure.innerType = structure.fullType;
                structure.isNullable = true;
            }
            else if (structure.fullType.IsValueType)
            {
                structure.innerType = structure.fullType;
                structure.isNullable = false;
            }
            else
            {
                structure.innerType = structure.fullType;
                structure.isNullable = false;
            }
        }
    }
}
