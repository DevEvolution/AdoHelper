﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdoHelper
{
    /// <summary>
    /// Query information
    /// </summary>
    /// <typeparam name="T">Return entity type</typeparam>
    public class QueryInfo<T>
    {
        /// <summary>
        /// ADO.NET connection
        /// </summary>
        public IDbConnection Connection { get; set; }
        
        /// <summary>
        /// ADO.NET query command
        /// </summary>
        public IDbCommand Command { get; set; }

        /// <summary>
        /// ADO.NET transaction
        /// </summary>
        public IDbTransaction Transaction { get; set; }

        /// <summary>
        /// List of ADO.NET query parameters
        /// </summary>
        public List<IDbDataParameter> QueryInfoParameters { get; set; }

        /// <summary>
        /// Type of mapped model
        /// </summary>
        public ModelEntityType ModelType { get; set; }

        /// <summary>
        /// Inner structure of model
        /// </summary>
        public List<MappingInfo> ModelStructureTable { get; set; }

        /// <summary>
        /// Shows which entity model type should use mapper 
        /// </summary>
        public enum ModelEntityType
        {
            Object,
            Tuple,
            Collection,
            Dynamic
        }
    }
}
