using Bramf.Extensions;
using System;

namespace Bramf.Patterns
{
    /// <summary>
    /// Specifies the name of the table or collection that will store
    /// the specific type of entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        /// The name of the collection
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Creates a new <see cref="CollectionNameAttribute"/>
        /// </summary>
        /// <param name="name">The name of the collection</param>
        public CollectionNameAttribute(string name)
        {
            if (name.IsNullOrWhitespace())
                throw new ArgumentNullException(nameof(name));

            CollectionName = name;
        }
    }
}