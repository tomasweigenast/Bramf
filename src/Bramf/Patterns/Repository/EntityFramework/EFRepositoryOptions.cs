using Bramf.Patterns.Repository.Base;

namespace Bramf.Patterns.Repository.EntityFramework
{
    /// <summary>
    /// Contains properties to configure a <see cref="EFRepository{TEntity, TContext}"/>
    /// </summary>
    public class EFRepositoryOptions : IRepositoryOptions
    {
        /// <summary>
        /// The name of the repository.
        /// If null, the entity type name will be used
        /// </summary>
        public string Name { get; set; }
    }
}