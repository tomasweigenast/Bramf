using Bramf.Patterns.Repository.Base;

namespace Bramf.Patterns.Repository.Mongo
{
    /// <summary>
    /// Contains properties to configure a <see cref="MongoRepository{TEntity}"/>
    /// </summary>
    public class MongoRepositoryOptions : IRepositoryOptions
    {
        /// <summary>
        /// The connection string used to connect to a Mongo instance
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the repository.
        /// If null, the entity type name will be used
        /// </summary>
        public string RepositoryName { get; set; }

        /// <summary>
        /// The name of the database to use
        /// </summary>
        public string DatabaseName { get; set; }
    }
}