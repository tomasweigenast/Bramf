using Bramf.Controllers;
using Bramf.Controllers.Base;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bramf.Wpf.Controllers
{
    /// <summary>
    /// Helpers
    /// </summary>
    public static class FrameworkConstructionHelpers
    {
        /// <summary>
        /// Injects a controller to the di
        /// </summary>
        /// <typeparam name="TControllerImplementation">The type of controller to inject</typeparam>
        public static FrameworkConstruction AddController<TControllerImplementation>(this FrameworkConstruction construction)
            where TControllerImplementation : BaseController
        {
            construction.Services.AddSingleton<BaseController, TControllerImplementation>();

            return construction;
        }

        /// <summary>
        /// Adds the snackbar controller to the services
        /// </summary>
        public static FrameworkConstruction AddSnackbar(this FrameworkConstruction construction)
        {
            // Create a new snackbar
            BaseSnackbarController snackbar = new SnackbarController();
            snackbar.Load(); // Load it

            // Add to services
            construction.Services.AddSingleton(typeof(BaseSnackbarController), snackbar);

            return construction;
        }

        /// <summary>
        /// Adds the snackbar controller to the services
        /// </summary>
        /// <typeparam name="TSnackbarImplementation">The snackbar custom implementation</typeparam>
        public static FrameworkConstruction AddSnackbar<TSnackbarImplementation>(this FrameworkConstruction construction)
            where TSnackbarImplementation : BaseSnackbarController, new()
        {
            // Create a new snackbar
            BaseSnackbarController snackbar = new TSnackbarImplementation();
            snackbar.Load(); // Load it

            // Add to services
            construction.Services.AddSingleton(typeof(BaseSnackbarController), snackbar);

            return construction;
        }

        /// <summary>
        /// Adds the snackbar controller to the services
        /// </summary>
        /// <typeparam name="TSnackbarImplementation">The snackbar custom implementation</typeparam>
        public static FrameworkConstruction AddSnackbar<TSnackbarImplementation>(this FrameworkConstruction construction, TimeSpan duration)
            where TSnackbarImplementation : BaseSnackbarController, new()
        {
            // Create a new snackbar
            BaseSnackbarController snackbar = new TSnackbarImplementation();
            snackbar.Load(duration); // Load it

            // Add to services
            construction.Services.AddSingleton(typeof(BaseSnackbarController), snackbar);

            return construction;
        }

        /// <summary>
        /// Adds the snackbar controller to the services
        /// </summary>
        public static FrameworkConstruction AddSnackbar(this FrameworkConstruction construction, TimeSpan duration)
        {
            // Create a new snackbar
            BaseSnackbarController snackbar = new SnackbarController();
            snackbar.Load(duration); // Load it

            // Add to services
            construction.Services.AddSingleton(typeof(BaseSnackbarController), snackbar);

            return construction;
        }
    }
}
