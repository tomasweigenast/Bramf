using System;
using SystemCache = System.Runtime.Caching;

namespace Bramf.Cache
{
    /// <summary>
    /// Class used to handle cache
    /// </summary>
    public static class Caching
    {
        /// <summary>
        /// A generic method for getting objects from memory cache
        /// </summary>
        /// <typeparam name="T">The type of object to be returned</typeparam>
        /// <param name="cacheItemId">The id of the cache object</param>
        /// <param name="throwExceptionNotFound">If true, the method will throw an exception if the object was not found, otherwise, it will return the default value of T</param>
        public static T GetObjectFromCache<T>(string cacheItemId, bool throwExceptionNotFound = false)
        {
            // Get default cache
            SystemCache.ObjectCache cache = SystemCache.MemoryCache.Default;

            // Try to get cache from id
            var cachedObject = (T)cache[cacheItemId];

            // If the object was not found...
            if (cachedObject == null)
                // If throw exception is enabled, throw it
                if (throwExceptionNotFound)
                    throw new Exception($"The object with id '{cacheItemId}' cannot be found.");

                // Otherwise, return null
                else
                    return default;

            // Return the object
            return cachedObject;
        }

        /// <summary>
        /// A generic method for setting objects to memory cache
        /// </summary>
        /// <typeparam name="T">The type of object to be added</typeparam>
        /// <param name="cacheId"></param>
        /// <param name="aliveTime"></param>
        /// <param name="objectToAdd">The object to be added to the cache</param>
        public static void AddObjectToCache<T>(string cacheId, TimeSpan aliveTime, T objectToAdd)
        {
            // Get default cache
            SystemCache.ObjectCache cache = SystemCache.MemoryCache.Default;

            // Try to get cache from id to figure out if there is already one in the id
            var cachedObject = (T)cache[cacheId];

            // If there is not an object in that cache id
            if (cachedObject == null)
            {
                // Create a new cache policy
                SystemCache.CacheItemPolicy cachePolicy = new SystemCache.CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(aliveTime) };

                // Add object to cache
                cache.Set(cacheId, objectToAdd, cachePolicy);
            }

            // Otherwise, if the id is duplicated, throw an exception
            else
                throw new Exception($"Cache object with id '{cacheId}' already exists.");
        }

        /// <summary>
        /// A methods that returns a boolean indicating if a cache is already in memory or not.
        /// </summary>
        /// <param name="cacheId">The id of the cache to search for</param>
        public static bool IsCacheAlive(string cacheId)
        {
            // Get default cache
            SystemCache.ObjectCache cache = SystemCache.MemoryCache.Default;

            // Return a boolean indicating the result
            return cache[cacheId] != null;
        }
    }
}
