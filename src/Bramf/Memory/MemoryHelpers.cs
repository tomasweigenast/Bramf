using System;
using System.Diagnostics;

namespace Bramf.Memory
{
    /// <summary>
    /// A class that contains static methods to help doing something in the memory (RAM)
    /// </summary>
    public static class MemoryHelpers
    {
        /// <summary>
        /// Gets the size in RAM memory of an object
        /// </summary>
        /// <param name="obj">The object to get its size in memory</param>
        unsafe public static int SizeOf(this object obj)
        {
            // Return 0 if the object is null
            if (obj == null) return 0;

            // Get the type handle of the object
            RuntimeTypeHandle rth = obj.GetType().TypeHandle;

            unsafe
            {
                // Get the size
                int size = *(*(int**)&rth + 1);

                return size;
            }
        }

        /// <summary>
        /// Gets the time (in milliseconds) that the system took to execute an action
        /// </summary>
        /// <param name="action">The action to execute</param>
        public static string GetExecutionTime(this Action action)
        {
            // Start a new stopwatch
            var watcher = Stopwatch.StartNew();

            // Execute the action
            action.Invoke();

            // Stop the counter
            watcher.Stop();

            // Return the elapsed milliseconds
            return $"{watcher.ElapsedMilliseconds}ms";
        }
    }
}
