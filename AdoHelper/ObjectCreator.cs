﻿using AdoHelper.TupleParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdoHelper
{
    /// <summary>
    /// Reflection based object creator
    /// </summary>
    public static class ObjectCreator
    {
        /// <summary>
        /// Creates type instance with any available constructor with default params
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <returns>Created object</returns>
        public static T Create<T>()
        {
            Type modelType = typeof(T);

            return (T)CreateModelObject(modelType);
        }

        /// <summary>
        /// Creates type instance with constructor that take passed params
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Create<T>(List<object> parameters)
        {
            Type modelType = typeof(T);

            object[] objParam = parameters.ToArray();
            return (T)Activator.CreateInstance(modelType, objParam);
        }

        /// <summary>
        /// Creates collection instance of specified type
        /// <para>Collection must implement IEnumerable or ICollection type</para>
        /// </summary>
        /// <typeparam name="T">Type of enumerable</typeparam>
        /// <returns>Instance of collection</returns>
        public static T CreateEnumerable<T>()
        {
            return (T)CreateEnumerable(typeof(T));
        }

        /// <summary>
        /// Creates collection instance of specified type
        /// <para>Collection must implement IEnumerable or ICollection type</para>
        /// </summary>
        /// <param name="type">Type of enumerable</param>
        /// <returns>Instance of collection</returns>
        public static object CreateEnumerable(Type type)
        {
            if (type.GetInterface("ICollection") != null)
            {
                return CreateModelObject(type);
            }
            else if (type.GetInterface("IEnumerable") != null)
            {
                var listType = typeof(List<>);
                var genericArgs = type.GetGenericArguments();
                if (genericArgs.Length == 0)
                    throw new NotSupportedException("Only generic collections are now supported");
                var concreteType = listType.MakeGenericType(genericArgs);
                return CreateModelObject(concreteType);
            }

            throw new NotSupportedException("Collection type must implement an IEnumerable interface");
        }

        /// <summary>
        /// Creates tuple instance of specified type
        /// <para>Supports long (items > 7) tuples</para>
        /// </summary>
        /// <typeparam name="T">Type of tuple</typeparam>
        /// <param name="parameters">List of initial values of tuple</param>
        /// <returns>Tuple instance</returns>
        public static T CreateTuple<T>(List<object> parameters)
        {
            return (T)CreateTuple(typeof(T), parameters);
        }

        /// <summary>
        /// Creates tuple instance of specified type
        /// <para>Supports long (items > 7) tuples</para>
        /// </summary>
        /// <param name="tupleType">Type of tuple</param>
        /// <param name="parameters">List of initial values of tuple</param>
        /// <returns>Tuple instance</returns>
        public static object CreateTuple(Type tupleType, List<object> parameters)
        {
            bool isValueTuple;

            if (tupleType.IsValueTuple()) isValueTuple = true;
            else if (tupleType.IsTuple()) isValueTuple = false;
            else
                throw new ArgumentException("This method supports only System.Tuple or System.ValueTuple types");

            Stack<(Type type, int paramBase, int paramCount)> tuplesStack = new Stack<(Type, int, int)>();
            int length = isValueTuple ?
                tupleType.GetFields().Length :
                tupleType.GetProperties().Length;
            length = length == 8 ? 7 : length;
            int totalParamCount = length;
            tuplesStack.Push((tupleType, 0, length));

            MemberInfo rest = null;
            do
            {
                rest = isValueTuple ?
                    (MemberInfo)tuplesStack.Peek().type.GetField("Rest") :
                    (MemberInfo)tuplesStack.Peek().type.GetProperty("Rest");
                if (rest != null)
                {
                    int paramCount = isValueTuple ?
                        (rest as FieldInfo).FieldType.GetFields().Length :
                        (rest as PropertyInfo).PropertyType.GetProperties().Length;
                    paramCount = paramCount == 8 ? 7 : paramCount;
                    tuplesStack.Push((isValueTuple ? (rest as FieldInfo).FieldType : (rest as PropertyInfo).PropertyType, totalParamCount, paramCount));
                    totalParamCount += paramCount;
                }
            }
            while (rest != null);

            if (parameters.Count != totalParamCount)
            {
                throw new ArgumentException("Count of parameters is not equals to count of arguments declared in tuple");
            }

            object tuple = null;
            bool first = true;
            while (tuplesStack.Count > 0)
            {
                (Type type, int paramBase, int paramCount) tupleParamRange = tuplesStack.Pop();

                List<object> extendedTupleParams = parameters.GetRange(tupleParamRange.paramBase, tupleParamRange.paramCount);
                ParameterInfo[] parameterInfos = tupleParamRange.type.GetConstructors().First().GetParameters();
                for (int i = 0; i < tupleParamRange.paramCount; i++)
                {
                    extendedTupleParams[i] = Convert.ChangeType(extendedTupleParams[i], parameterInfos[i].ParameterType);
                }

                if (first)
                {
                    tuple = Activator.CreateInstance(tupleParamRange.type, extendedTupleParams.ToArray());
                    first = false;
                }
                else
                {
                    extendedTupleParams.Add(tuple);
                    tuple = Activator.CreateInstance(tupleParamRange.type, extendedTupleParams.ToArray());
                }
            }

            return tuple;
        }

        /// <summary>
        /// Creates object instance of specified type
        /// </summary>
        /// <param name="modelType">Object type</param>
        /// <returns>Instance of object</returns>
        private static object CreateModelObject(Type modelType)
        {
            object model;
            if (modelType == typeof(string))
            {
                return String.Empty;
            }

            List<Type> constructorParams = GetIdealConstructor(modelType);

            if (constructorParams.Count > 0)
            {
                object[] parameters = new object[constructorParams.Count];
                for (int i = 0; i < constructorParams.Count; i++)
                {
                    parameters[i] = CreateModelObject(constructorParams[i]);
                }
                model = Activator.CreateInstance(modelType, parameters);
            }
            else
            {
                model = Activator.CreateInstance(modelType);
            }
            return model;
        }

        /// <summary>
        /// Gets constructor with the least param count
        /// </summary>
        /// <param name="modelType">Type of object</param>
        /// <returns>Constructor</returns>
        private static List<Type> GetIdealConstructor(Type modelType)
        {
            List<Type> bestConstructor = new List<Type>();
            int bestParamCount = int.MaxValue;

            foreach (var ctor in modelType.GetConstructors())
            {
                int paramCount = 0;
                List<Type> paramTypes = new List<Type>();

                foreach (var par in ctor.GetParameters())
                {
                    paramTypes.Add(par.ParameterType);
                    paramCount++;
                }

                if (paramCount == 0) return paramTypes;

                if (paramCount < bestParamCount)
                {
                    bestConstructor = paramTypes;
                    bestParamCount = paramCount;
                }
            }

            return bestConstructor;
        }
    }
}
