using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bramf.Extensions
{
    /// <summary>
    /// Extension methods to work with <see cref="object"/>s
    /// </summary>
    public static class ObjectExtensions
    {
        #region Private Members

        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance); // Gets clone method from object clas/

        #endregion

        #region Public Methods

        /// <summary>
        /// Indicates if an object type is a primitive type
        /// </summary>
        public static bool IsPrimitive(this Type type)
        {
            // If the type is string, return true
            if (type == typeof(string))
                return true;

            // Decide if the type is a primitive type
            return (type.IsValueType & type.IsPrimitive);
        }

        /// <summary>
        /// Creates a deep copy of an object
        /// </summary>
        /// <typeparam name="T">The type of object to copy</typeparam>
        /// <param name="original">The object to copy</param>
        public static T Copy<T>(this T original) => (T)Copy((object)original);

        /// <summary>
        /// Creates a deep copy of an object
        /// </summary>
        /// <param name="originalObject">The object to copy</param>
        public static object Copy(this object originalObject)
            => InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));

        #endregion

        #region Types

        internal class ReferenceEqualityComparer : EqualityComparer<object>
        {
            public override bool Equals(object x, object y)
                => ReferenceEquals(x, y);

            public override int GetHashCode(object obj)
            {
                if (obj == null) return 0;
                return obj.GetHashCode();
            }
        }

        #endregion

        #region Private Helpers Methods

        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            // If the object to copy is null, return null
            if (originalObject == null)
                return null;

            // Get the type of the object to copy
            var typeToReflect = originalObject.GetType();

            // If the object is a primitive type, return the original object
            if (IsPrimitive(typeToReflect))
                return originalObject;

            if (visited.ContainsKey(originalObject))
                return visited[originalObject];

            // If the object can be an instance of the object, return null
            if (typeof(Delegate).IsAssignableFrom(typeToReflect))
                return null;

            // Clone the object
            var cloneObject = CloneMethod.Invoke(originalObject, null);

            // If the object is an array
            if(typeToReflect.IsArray)
            {
                // Clone every object in the array
                var arrayType = typeToReflect.GetElementType();
                if(IsPrimitive(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }

            // Copy final values
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);

            // Return copied object
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(
            object originalObject,
            IDictionary<object, object> visited,
            object cloneObject,
            Type typeToReflect,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy,
            Func<FieldInfo, bool> filter = null)
        {
            foreach(FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false)
                    continue;

                if (IsPrimitive(fieldInfo.FieldType))
                    continue;

                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        #endregion
    }
}
