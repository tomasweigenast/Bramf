using System;

namespace Bramf.Controllers.Base
{
    /// <summary>
    /// A base implementation to create a snackbar controller
    /// </summary>
    public abstract class BaseSnackbarController : BaseController
    {
        /// <summary>
        /// Loads the snackbar controller with specific open duration
        /// </summary>
        /// <param name="duration">The duration time of the snackbar when its opened</param>
        public abstract void Load(TimeSpan duration);

        /// <summary>
        /// Enqueues a message to the snackbar
        /// </summary>
        /// <param name="message">The message to enqueue</param>
        public abstract void Send(string message);

        /// <summary>
        /// Enqueues a message to the snackbar
        /// </summary>
        /// <param name="message">The message to enqueue</param>
        /// <param name="actionText">The text the action button will have</param>
        /// <param name="action">The action the button will execute</param>
        public abstract void Send(string message, string actionText, Action action);

        /// <summary>
        /// Enqueues a message to the snackbar
        /// </summary>
        /// <param name="message">The message to enqueue</param>
        /// <param name="actionText">The text the action button will have</param>
        /// <param name="action">The action the button will execute</param>
        /// <param name="parameter">The parameter of the action</param>
        public abstract void Send(string message, string actionText, Action<object> action, object parameter);
    }
}
