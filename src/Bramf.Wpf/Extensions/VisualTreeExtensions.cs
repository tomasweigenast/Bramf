using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Bramf.Wpf.Extensions
{
    /// <summary>
    /// Provides methods to work with visual tree
    /// </summary>
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// Gets the child of an dependency object
        /// </summary>
        /// <typeparam name="T">The type of the child to get</typeparam>
        /// <param name="depObj">The dependency object from where the child must be returned</param>
        public static T GetChildOfType<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        /// <summary>
        /// Finds a visual children
        /// </summary>
        /// <typeparam name="T">The type of element to find</typeparam>
        /// <param name="parent">The parent object to search from</param>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var queue = new Queue<DependencyObject>(new[] { parent });

            while (queue.Any())
            {
                var reference = queue.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(reference);

                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    if (child is T children)
                        yield return children;

                    queue.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// Finds a logical children
        /// </summary>
        /// <typeparam name="T">The type of element to find</typeparam>
        /// <param name="parent">The parent object to search from</param>
        public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var queue = new Queue<DependencyObject>(new[] { parent });

            while (queue.Any())
            {
                var reference = queue.Dequeue();
                var children = LogicalTreeHelper.GetChildren(reference);
                var objects = children.OfType<DependencyObject>();

                foreach (var o in objects)
                {
                    if (o is T child)
                        yield return child;

                    queue.Enqueue(o);
                }
            }
        }
    }
}
