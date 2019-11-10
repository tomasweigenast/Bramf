using System;
using System.Collections.Generic;

namespace Bramf.App
{
    /// <summary>
    ///  Manages creation, show, close methods
    /// </summary>
    /// <typeparam name="TWindow">The type of windows that this manager handles</typeparam>
    public class WindowManager<TWindow> where TWindow : class
    {
        #region Private Members

        private IDictionary<string, TWindow> mActiveWindows;
        private int mMaxOpenedWindows;
        private Action mOpenWindowMethod;
        private Action mCloseWindowMethod;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the amount of opened windows
        /// </summary>
        public int OpenedWindows => mActiveWindows.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowManager(int maxOpenedWindows = 10)
        {
            mMaxOpenedWindows = maxOpenedWindows;
            mActiveWindows = new Dictionary<string, TWindow>(maxOpenedWindows); // Max opened windows
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a new window
        /// </summary>
        /// <param name="window">The window to open</param>
        /// <param name="identifier">The identifier name of the window. If null, the window type name will be used.</param>
        public void AddWindow(TWindow window, string identifier = null)
        {
            // Prevent window to accept null
            if (window == null) throw new ArgumentNullException(nameof(window));

            // If identifier is not provided
            if (!string.IsNullOrWhiteSpace(identifier))
                identifier = window.GetType().Name; // Get name from the window

            // If there are already a window with that identifier
            if (mActiveWindows.ContainsKey(identifier)) throw new InvalidOperationException($"There is already an opened window with '{identifier}' as identifier.");

            // If the maximum opened windows was reached
            if (mActiveWindows.Count >= mMaxOpenedWindows) throw new InvalidOperationException($"The maximum opened windows has been reached. Close one and try again.");

            // Add the window
            mActiveWindows.Add(identifier, window);
        }

        /// <summary>
        /// Closes a window
        /// </summary>
        /// <param name="identifier">The identifier name of the window to close.</param>
        public TWindow RemoveWindow(string identifier)
        {
            // Avoid null
            if (string.IsNullOrWhiteSpace(identifier)) throw new ArgumentNullException(nameof(identifier));

            // If there is no any window 
            if (!mActiveWindows.ContainsKey(identifier)) throw new InvalidOperationException($"There is not any window with the identifier '{identifier}'");

            TWindow win = mActiveWindows[identifier];

            // Remove the window
            mActiveWindows.Remove(identifier);

            return win;
        }

        #endregion
    }
}
