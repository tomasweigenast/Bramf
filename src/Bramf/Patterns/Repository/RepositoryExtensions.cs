using Bramf.Patterns.Repository.Base;
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

            services.AddScoped<IRepositoryFactory>((factory) => new RepositoryFactory(builder.GetRepositories()));

            return services;
        }
    }

    /// <summary>
    /// Contains methods to build repositories
    /// </summary>
    public class RepositoryBuilder
    {
        private IList<IRepository> mRepositories;
        private IServiceCollection mServices;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RepositoryBuilder(IServiceCollection services)
        {
            mRepositories = new List<IRepository>();
            mServices = services;
        }

        /// <summary>
        /// Adds a new repository.
        /// If the repository type passed contains at least two constructors, the parameterless one always
        /// will be called first. So, if you want to pass 
        /// </summary>
        /// <typeparam name="TRepository">The repository type to create.</typeparam>
        /// <param name="name">The name of the repository.</param>
        /// <param name="options">The options to configure the repository.</param>
        public RepositoryBuilder Implement<TRepository>(string name, IRepositoryOptions options) 
            where TRepository : IRepository
        {
            if (mRepositories.Any(x => x.Name == name))
                throw new ArgumentException($"Already exists a repository named '{name}'.");

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
                var parameters = constructor.GetParameters();

                // If there are no just 2 parameters
                if (parameters.Length != 2)
                    throw new ArgumentException($"Cannot instantiate repository of type '{repositoryType}' because its constructor does not contains exactly 2 parameters.");

                // Try to call the constructor with the given parameters
                try
                {
                    repositoryInstance = (TRepository)constructor.Invoke(new object[] { name, options });
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
            mServices.AddScoped(baseType, typeof(TRepository));

            // Add repository to list
            mRepositories.Add(repositoryInstance);
                
            return this;
        }

        internal IEnumerable<IRepository> GetRepositories()
            => mRepositories;
    }
}