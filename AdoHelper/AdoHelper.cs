using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using AdoHelper.TupleParsing;

namespace AdoHelper
{
    public class AdoHelper<T>
    {
        // Context
        protected QueryInfo<T> _queryInfo;

        /// <summary>
        /// Starts ADO.NET query
        /// </summary>
        /// <param name="connection">ADO.NET Connection</param>
        public AdoHelper(IDbConnection connection)
        {
            _queryInfo = new QueryInfo<T>() { Connection = connection };

            Type _modelType = typeof(T);

            if (_modelType.IsGenericType && (_modelType.GetInterface("ICollection") != null || _modelType.GetInterface("IEnumerable") != null))
            {
                // Collection
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.Collection;
                ParseCollectionStructure(_modelType);
            }
            else if (_modelType.IsTuple())
            {
                // Tuple
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.Tuple;
                ParseTupleStructure(_modelType, false);

            }
            else if (_modelType.IsValueTuple())
            {
                // Value tuple
                _queryInfo.ModelType = QueryInfo<T>.ModelEntityType.Tuple;
                ParseTupleStructure(_modelType, true);
            }
            else
            {
                // Class or structure
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

        /// <summary>
        /// Parsing the inner structure of tuple
        /// </summary>
        /// <param name="modelType">Tuple type</param>
        /// <param name="isValueTuple">Is value tuple type</param>
        private void ParseTupleStructure(Type modelType, bool isValueTuple)
        {
            _queryInfo.ModelStructureTable = new List<MappingInfo>();

            int itemCount = modelType.TupleItemCount();
            for (int i = 0; i < itemCount; i++)
            {
                MappingInfo structure = new MappingInfo();
                if (isValueTuple)
                {
                    structure.MapFieldType = MappingInfo.FieldType.Field;
                    structure.FullType = modelType.GetTupleItemType(i);
                }
                else
                {
                    structure.MapFieldType = MappingInfo.FieldType.Property;
                    structure.FullType = modelType.GetTupleItemType(i);
                }
                  
                structure.MapFieldName = String.Empty;

                // Parse type
                ParseInnerType(structure);

                // Add to structure
                _queryInfo.ModelStructureTable.Add(structure);
            }
        }

        /// <summary>
        /// Parsing the inner structure of object
        /// </summary>
        /// <param name="modelType">Object type</param>
        private void ParseModelStructure(Type modelType)
        {
            _queryInfo.ModelStructureTable = new List<MappingInfo>();

            // Parse properties
            foreach (MemberInfo memberInfo in modelType.GetMembers())
            {
                MappingInfo structure = new MappingInfo();

                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    structure.MapFieldType = MappingInfo.FieldType.Field;
                    structure.FullType = (memberInfo as FieldInfo).FieldType;
                }
                else if (memberInfo.MemberType == MemberTypes.Property && ((memberInfo as PropertyInfo).CanWrite || _queryInfo.ModelType == QueryInfo<T>.ModelEntityType.Tuple))
                {
                    structure.MapFieldType = MappingInfo.FieldType.Property;
                    structure.FullType = (memberInfo as PropertyInfo).PropertyType;
                }
                else
                {
                    continue;
                }

                structure.MapFieldName = memberInfo.Name;

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

        /// <summary>
        /// Parsing the collection structure
        /// </summary>
        /// <param name="modelType">Collection type</param>
        private void ParseCollectionStructure(Type modelType)
        {
            _queryInfo.ModelStructureTable = new List<MappingInfo>();

            MappingInfo structure = new MappingInfo();
            structure.MapFieldType = MappingInfo.FieldType.CollectionItem;
            structure.FullType = modelType.GetGenericArguments().First();
            structure.MapFieldName = String.Empty;

            ParseInnerType(structure);

            _queryInfo.ModelStructureTable.Add(structure);
        }

        /// <summary>
        /// Check NonMapped attibute
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <returns>Is member can be mapped</returns>
        private bool CheckMappingRights(MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<NonMappedAttribute>() == null;    
        
        /// <summary>
        /// If Field attribute set, maps to its name, else to own name of member
        /// </summary>
        /// <param name="memberInfo">Member info</param>
        /// <param name="structure">Mapping info</param>
        private void SetPropertyMapName(MemberInfo memberInfo, MappingInfo structure)
        {
            FieldAttribute attribute = memberInfo.GetCustomAttribute<FieldAttribute>();
            if (attribute != null && attribute.Name != null)
            {
                structure.DbFieldName = attribute.Name.ToLower();
            }
            else
            {
                structure.DbFieldName = structure.MapFieldName.ToLower();
            }
        }

        /// <summary>
        /// Inner structure definition
        /// </summary>
        /// <param name="structure">Mapping info</param>
        private void ParseInnerType(MappingInfo structure)
        {
            if (structure.FullType.IsGenericType && structure.FullType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Nullable type
                structure.InnerType = structure.FullType.GetGenericArguments()[0];
                structure.IsNullable = true;
            }
            else if (structure.FullType.IsClass)
            {
                // Class
                structure.InnerType = structure.FullType;
                structure.IsNullable = true;
            }
            else if (structure.FullType.IsValueType)
            {
                // Value type (structure)
                structure.InnerType = structure.FullType;
                structure.IsNullable = false;
            }
            else
            {
                structure.InnerType = structure.FullType;
                structure.IsNullable = false;
            }
        }
    }
}
