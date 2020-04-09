using Bramf.Patterns.Repository.Base;
using Bramf.Patterns.Repository.EntityFramework;
using Bramf.Patterns.Repository.Mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bramf.Patterns.Repository
{
    /// <summary>
    /// Extension methods for <see cref="IRepository"/>
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Injects the <see cref="IRepositoryFactory"/> as a scoped service
        /// </summary>
        /// <param name="services">The current services.</param>
        /// <param name="setupAction">The builder action.</param>
        public static IServiceCollection AddRepository(this IServiceCollection services, Action<RepositoryBuilder> setupAction)
        {
            RepositoryBuilder builder = new RepositoryBuilder(services);
            setupAction.Invoke(builder);

            services.AddSingleton<IRepositoryFactory>((factory) => new RepositoryFactory(services.BuildServiceProvider(), builder.GetRepositories()));

            return services;
        }
    }

    /// <summary>
    /// Contains methods to build repositories
    /// </summary>
    public class RepositoryBuilder
    {
        private IList<IRepository> mRepositories;

        internal IServiceCollection Services { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RepositoryBuilder(IServiceCollection services)
        {
            mRepositories = new List<IRepository>();
            Services = services;
        }

        /*public RepositoryBuilder Implement<TRepository>(IRepositoryOptions options) 
            where TRepository : IRepository
        {
            TRepository repositoryInstance;
            Type repositoryType = typeof(TRepository);

            // Get repository constructors
            var constructors = repositoryType.GetConstructors();

            // If there are no constructors, call the parameterless one
            if (constructors.Length <= 0)
                repositoryInstance = Activator.CreateInstance<TRepository>();

            // If there are more than 1 constructor..
            else if (constructors.Length > 1)
                if (constructors.Any(x => x.GetParameters().Length == 0)) // Check for parameterless one
                    repositoryInstance = Activator.CreateInstance<TRepository>(); // If exists, call it

                // Otherwise throw an exception
                else
                    throw new ArgumentException($"Cannot instantiate repository of type '{repositoryType}' because more than 1 parameter was found and no one is parameterless.");
            
            // There are just 1 constructor
            else
            {
                // Get the constructor
                ConstructorInfo constructor = constructors.Single();

                // Try to call the constructor with the given parameters
                try
                {
                    repositoryInstance = (TRepository)constructor.Invoke(new[] { options });
                }
                catch
                {
                    throw new ArgumentException($"Cannot instantiate repository of type '{repositoryType}' because the given parameters does not match with {repositoryType} constructor parameters.");
                }
            }

            // Get the generic argument of TRepository
            Type entityType = repositoryType.GetGenericArguments()?.FirstOrDefault();

            // There is no generic arguments
            if (entityType == null)
                throw new ArgumentException($"Repository of type '{repositoryType}' cannot be added because it does not contains generic arguments.");

            // Make IRepository type with the passed generic argument
            Type baseType = typeof(IRepository).MakeGenericType(entityType);

            // Inject service
            Services.AddScoped(baseType, typeof(TRepository));

            // Add repository to list
            mRepositories.Add(repositoryInstance);
                
            return this;
        }*/

        /// <summary>
        /// Adds a new repository.
        /// If the repository type passed contains at least two constructors, the parameterless one always
        /// will be called first. So, if you want to pass 
        /// </summary>
        /// <typeparam name="TRepository">The repository type to create.</typeparam>
        /// <typeparam name="TRepositoryOptions">The options to configure the repository.</typeparam>
        /// <param name="options">The repository options to configure it.</param>
        public RepositoryBuilder Implement<TRepository, TRepositoryOptions>(TRepositoryOptions options)
            where TRepositoryOptions : class, IRepositoryOptions
        {
            Type repositoryType = typeof(TRepository);

            // Inject repository options
            Services.AddSingleton(options);

            // Get the generic argument of TRepository
            Type entityType = repositoryType.GetGenericArguments()?.FirstOrDefault();

            // There is no generic arguments
            if (entityType == null)
                throw new ArgumentException($"Repository of type '{repositoryType}' cannot be added because it does not contains generic arguments.");

            // Make IRepository type with the passed generic argument
            Type baseType = typeof(IRepository).MakeGenericType(entityType);

            // Add the repository as scoped instance
            Services.AddScoped(baseType, (factory) =>
            {
                // Get repository options
                TRepositoryOptions repositoryOptions = factory.GetRequiredService<TRepositoryOptions>();

                // Get repository constructors
                var constructors = repositoryType.GetConstructors();

                // If there are no constructors, call the parameterless one
                if (constructors.Length <= 0)
                    return Activator.CreateInstance<TRepository>();

                // If there are more than 1 constructor..
                else if (constructors.Length > 1)
                    if (constructors.Any(x => x.GetParameters().Length == 0)) // Check for parameterless one
                        return Activator.CreateInstance<TRepository>(); // If exists, call it

                    // Otherwise throw an exception
                    else
                        throw new ArgumentException($"Cannot instantiate repository of type '{repositoryType}' because more than 1 parameter was found and no one is parameterless.");

                // There are just 1 constructor
                else
                {
                    // Get the constructor
                    ConstructorInfo constructor = constructors.Single();

                    // Try to call the constructor with the given parameters
                    try
                    {
                        // Invoke the constructor
                        return (TRepository)constructor.Invoke(new object[] { repositoryOptions });
                    }
                    catch
                    {
                        throw new ArgumentException($"Cannot instantiate repository of type '{repositoryType}' because the given parameters does not match with {repositoryType} constructor parameters.");
                    }
                }
            });

            return this;
        }

        internal IEnumerable<IRepository> GetRepositories()
            => mRepositories;
    }

    /// <summary>
    /// Extension methods to easily add built-in repositories
    /// </summary>
    public static class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Adds a new <see cref="MongoRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity">The entity type that will be stored.</typeparam>
        /// <param name="builder">The current <see cref="RepositoryBuilder"/></param>
        /// <param name="options">The options for the repository.</param>
        public static RepositoryBuilder ImplementMongo<TEntity>(this RepositoryBuilder builder, MongoRepositoryOptions options)
            where TEntity : IEntity
        {
            builder.Implement<MongoRepository<TEntity>, MongoRepositoryOptions>(options);

            return builder;
        }

        /// <summary>
        /// Adds a new <see cref="EFRepository{TEntity, TContext}"/>
        /// </summary>
        /// <typeparam name="TEntity">The entity type that will be stored.</typeparam>
        /// <typeparam name="TContext">The context type to be used.</typeparam>
        /// <param name="builder">The current <see cref="RepositoryBuilder"/></param>
        /// <param name="options">The repository options.</param>
        /// <param name="dbContextOptionsAction">The DbContext options builder.</param>
        public static RepositoryBuilder ImplementEntityFramework<TEntity, TContext>(this RepositoryBuilder builder, EFRepositoryOptions options, Action<DbContextOptionsBuilder> dbContextOptionsAction)
            where TEntity : class, IEntity
            where TContext : DbContext
        {
            // Inject the DbContext
            builder.Services.AddDbContextPool<TContext>(dbContextOptionsAction);

            // Implement the repository
            builder.Implement<EFRepository<TEntity, TContext>, EFRepositoryOptions>(options);

            return builder;
        }
    }
}