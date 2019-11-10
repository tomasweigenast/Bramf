using Bramf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Bramf.AspNet
{
    /// <summary>
    /// Extensions for <see cref="IWebHostBuilder"/>
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Adds the Bramf construct to the ASP.NET Core application
        /// </summary>
        /// <param name="builder">The web host builder</param>
        /// <param name="configure">Custom action to configure the Dna Framework</param>
        /// <returns></returns>
        public static IWebHostBuilder UseBramf(this IWebHostBuilder builder, Action<FrameworkConstruction> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                // Construct a hosted Dna Framework
                Framework.Construct<HostedFrameworkConstruction>();

                // Setup this service collection to
                // be used by Bramf 
                services.AddFramework()
                        // Add configuration
                        .AddConfiguration(context.Configuration)
                        // Add default services
                        .AddDefaultServices();

                // Fire off construction configuration
                configure?.Invoke(Framework.Construction);

                // NOTE: Framework will do .Build() from the Startup.cs Configure call
                //       app.UseBramf()
            });

            // Return builder for chaining
            return builder;
        }

        /// <summary>
        /// Adds the Bramf construct to the ASP.NET Core application
        /// </summary>
        /// <param name="builder">The host builder</param>
        /// <param name="configure">Custom action to configure the Dna Framework</param>
        public static IHostBuilder UseBramf(this IHostBuilder builder, Action<FrameworkConstruction> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                // Constrct a hosted Framework
                Framework.Construct<HostedFrameworkConstruction>();

                // Setup this service collection to be used by Bramf
                services
                    .AddFramework()
                    .AddConfiguration(context.Configuration)
                    .AddDefaultServices();

                // Fire off construction configuration
                configure?.Invoke(Framework.Construction);
            });

            // Return the builder
            return builder;
        }
    }
}
