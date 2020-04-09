using System;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// Factory used to create and retrieve <see cref="IRepository{TEntity}"/> instances
    /// </summary>
    public interface IRepositoryFactory : IDisposable
    {
        /// <summary>
        /// Returns a repository by its name
        /// </summary>
        /// <param name="name">The name of the repository.</param>
        IRepository<TEntity> Load<TEntity>(string name) where TEntity : IEntity;

        /// <summary>
        /// Returns a repository by its entity type
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        IRepository<TEntity> Load<TEntity>() where TEntity : IEntity;
    }
}