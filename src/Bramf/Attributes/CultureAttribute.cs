using System;

namespace Bramf.Attributes
{
    /// <summary>
    /// Specifies the information of a culture
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CultureAttribute : Attribute
    {
        /// <summary>
        /// The name of the culture
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The short name of the culture
        /// </summary>
        public string ShortName { get; set; }
    }
}
