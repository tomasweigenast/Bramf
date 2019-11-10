using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bramf.Extensions
{
    /// <summary>
    /// Provides methods to help with collections and arrays
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Takes an <see cref="List{T}"/> and converts it to a <see cref="ObservableCollection{T}"/>
        /// </summary>
        /// <typeparam name="T">The type to convert the collection to</typeparam>
        /// <param name="list">The list to be converted</param>
        /// <returns>The list converted to a observable collection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            return new ObservableCollection<T>(list);
        }

        /// <summary>
        /// Takes an <see cref="IEnumerable{T}"/> and converts it to a <see cref="ObservableCollection{T}"/>
        /// </summary>
        /// <typeparam name="T">The type to convert the collection to</typeparam>
        /// <param name="enumerable">The enumerable list to be converted</param>
        /// <returns>The enumerable converted to a observable collection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
        
        /// <summary>
        /// Removes items from an <see cref="ObservableCollection{T}"/> depending on a condition
        /// </summary>
        /// <typeparam name="T">The type of collection</typeparam>
        /// <param name="collection">The collection to remove items from</param>
        /// <param name="condition">The condition</param>
        public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            // Get items based on the condition
            var itemsToRemove = collection.Where(condition).ToList();

            // Foreach item
            foreach (var itemToRemove in itemsToRemove)
            {
                // Remove it
                collection.Remove(itemToRemove);
            }

            // Return items in the collection
            return itemsToRemove.Count;
        }

        /// <summary>
        /// Paginates a <see cref="IList{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of object that collection has</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> to paginate</param>
        /// <param name="page">The page index to get</param>
        /// <param name="pageSize">The amount of items to show per page</param>
        public static IList<T> Paginate<T>(this IList<T> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Takes an <see cref="IEnumerable{T}"/> and sorts it depending on a <see cref="IComparer{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of object to sort</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> containing the items</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/></param>
        public static List<T> ToSortedList<T>(this IEnumerable<T> enumerable, IComparer<T> comparer)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            // Enumerate to a list
            var list = enumerable.ToList();

            // Sort it
            list.Sort(comparer);

            // Return it
            return list;
        }

        /// <summary>
        /// Enumerates the given enumerable to a sorted list.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparer">The comparer used to sort the elements.</param>
        /// <returns>The sorted list.</returns>
        public static List<T> ToSortedList<T>(this IEnumerable<T> enumerable, Func<T, T, int> comparer) => 
            enumerable.ToSortedList(Comparer<T>.Create((t1, t2) => comparer(t1, t2)));

        /// <summary>
        /// Executes an action for each element in an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of object of the items</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> containing the items</param>
        /// <param name="action">The action to execute</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // For each element...
            foreach (var element in enumerable)
                // Invoke the action for the element.
                action(element);
        }

        /// <summary>
        /// Performs an action for every value in an array
        /// </summary>
        /// <param name="array">The array</param>
        /// <param name="action">The action to perform</param>
        public static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0) return;
            ArrayTranverse walker = new ArrayTranverse(array);
            do action(array, walker.Position);
            while (walker.Step());
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// Inclusive for start index, exclusive for end index.
        /// </summary>
        /// <typeparam name="T">The type of collection</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="start">The start index</param>
        /// <param name="end">The end index</param>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        #region Private Methods Helpers

        internal class ArrayTranverse
        {
            public int[] Position;
            private int[] mMaxLengths;

            public ArrayTranverse(Array array)
            {
                mMaxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; i++)
                    mMaxLengths[i] = array.GetLength(i) - 1;

                Position = new int[array.Rank];
            }

            public bool Step()
            {
                for(int i = 0; i < Position.Length; i++)
                    if(Position[i] < mMaxLengths[i])
                    {
                        Position[i]++;
                        for (int j = 0; j < i; j++)
                            Position[j] = 0;

                        return true;
                    }

                return false;
            }
        }

        #endregion
    }
}
