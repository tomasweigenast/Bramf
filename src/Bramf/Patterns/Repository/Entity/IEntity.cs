using MongoDB.Bson;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// Represents an entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The primary key of the entity
        /// </summary>
        public object Id { get; set; }
    }

    /// <summary>
    /// Represents an entity with a primary key
    /// </summary>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// The primary key
        /// </summary>
        new TKey Id { get; set; }
    }

    /// <summary>
    /// Represents a base entity which uses a string as primary key
    /// </summary>
    public abstract class BaseEntity : IEntity<string>
    {
        /// <summary>
        /// The primary key
        /// </summary>
        public string Id { get; set; }

        object IEntity.Id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}