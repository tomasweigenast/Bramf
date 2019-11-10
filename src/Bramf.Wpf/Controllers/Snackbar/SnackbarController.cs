using Bramf.Controllers.Base;
using Bramf.Wpf.Controllers.Snackbar;
using MaterialDesignThemes.Wpf;
using System;

namespace Bramf.Wpf.Controllers
{
    /// <summary>
    /// The controller for the snackbar
    /// </summary>
    public class SnackbarController : BaseSnackbarController
    {
        #region Members

        private static SnackbarControl SnackbarControl;

        #endregion

        #region Properties

        /// <summary>
        /// The queue for the snackbar
        /// </summary>
        public SnackbarMessageQueue Queue { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Loads the snackbar controller
        /// </summary>
        public override void Load()
        {
            Queue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// Loads the snackbar controller with specific open duration
        /// </summary>
        /// <param name="duration">The duration time of the snackbar when its opened</param>
        public override void Load(TimeSpan duration)
        {
            Queue = new SnackbarMessageQueue(duration);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SnackbarController()
        {
            Load();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SnackbarController(TimeSpan duration)
        {
            Load(duration);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enqueues a message to the snackbar
        /// </summary>
        /// <param name="message"></param>
        public override void Send(string message)
        {
            SnackbarControl.Dispatcher.Invoke(() =>
            {
                if (SnackbarControl.Controller.MessageQueue == null)
                    SnackbarControl.Controller.MessageQueue = Queue;
            });

            // Enqueques the message
            Queue.Enqueue(message);
        }

        /// <summary>
        /// Enqueues a message to the snackbar
        /// </summary>
        /// <param name="message">The message to enqueue</param>
        /// <param name="actionText">The text the action button will have</param>
        /// <param name="action">The action the button will execute</param>
        public override void Send(string message, string actionText, Action action)
        {
            SnackbarControl.Dispatcher.Invoke(() =>
            {
                if (SnackbarControl.Controller.MessageQueue == null)
                    SnackbarControl.Controller.MessageQueue = Queue;
            });

            // Enqueue
            Queue.Enqueue(message, actionText, action);
        }

        /// <summary>
        /// Enqueues a message to the snackbar
        /// </summary>
        /// <param name="message">The message to enqueue</param>
        /// <param name="actionText">The text the action button will have</param>
        /// <param name="action">The action the button will execute</param>
        /// <param name="parameter">The parameter of the action</param>
        public override void Send(string message, string actionText, Action<object> action, object parameter)
        {
            SnackbarControl.Dispatcher.Invoke(() =>
            {
                if (SnackbarControl.Controller.MessageQueue == null)
                    SnackbarControl.Controller.MessageQueue = Queue;
            });

            // Enqueue
            Queue.Enqueue(message, actionText, action, parameter);
        }

        #endregion

        #region Internal Methods

        internal static void AddSnackbar(SnackbarControl control)
        {
            if (SnackbarControl != null) throw new InvalidOperationException($"You can only have one instance of the snackbar control.");

            SnackbarControl = control;
        }

        internal static void RemoveSnackbar(SnackbarControl control)
        {
            if (SnackbarControl == null) return;

            SnackbarControl = null;
        }

        #endregion
    }
}
