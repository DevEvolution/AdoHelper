using System;
using System.Collections.Generic;
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
