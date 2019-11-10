using Bramf.App;
using Bramf.Controllers.Base;
using Bramf.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bramf
{
    /// <summary>
    /// The core services that could be available in the Dna Framework
    /// for quick and easy access anywhere in code.
    /// </summary>
    /// <example>
    /// <code>
    ///     using static Dna.FrameworkDI
    ///     
    ///     Logger.Log(Configuration["something"]);
    /// </code>
    /// </example>
    public static class FrameworkDI
    {
        /// <summary>
        /// Gets the configuration
        /// </summary>
        //public static IConfiguration Configuration => Framework.Provider.GetService<IConfiguration>();

        /// <summary>
        /// Gets the default logger
        /// </summary>
        public static ILogger Logger => Framework.Provider.GetService<ILogger>();

        /// <summary>
        /// Gets the logger factory for creating loggers
        /// </summary>
        public static ILoggerFactory LoggerFactory => Framework.Provider.GetService<ILoggerFactory>();

        /// <summary>
        /// Gets the framework environment
        /// </summary>
        public static IFrameworkEnvironment FrameworkEnvironment => Framework.Provider.GetService<IFrameworkEnvironment>();

        /// <summary>
        /// Gets the framework exception handler
        /// </summary>
        public static IExceptionHandler ExceptionHandler => Framework.Provider.GetService<IExceptionHandler>();

        /// <summary>
        /// Gets the configuration
        /// </summary>
        public static cfgConfig.Core.IConfiguration Configuration => Framework.Provider.GetService<cfgConfig.Core.IConfiguration>();

        /// <summary>
        /// Gets the snackbar controller if it is
        /// </summary>
        public static BaseSnackbarController Snackbar => Framework.Provider.GetService<BaseSnackbarController>();

        /// <summary>
        /// Gets the application behaviour
        /// </summary>
        public static IAppBehaviour<IAppStorage> App => Framework.Provider.GetService<IAppBehaviour<IAppStorage>>();

        /// <summary>
        /// Gets the application reusable http request
        /// </summary>
        public static IReusableHttpRequest HttpRequestor => Framework.Provider.GetService<IReusableHttpRequest>();
    }
}
