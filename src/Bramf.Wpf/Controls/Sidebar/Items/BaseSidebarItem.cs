using System.Windows;

namespace Bramf.Wpf.Controls
{
    /// <summary>
    /// Represents a base item that can be in a <see cref="Sidebar"/>
    /// </summary>
    public abstract class BaseSidebarItem : DependencyObject
    {
        #region Members

        /// <summary>
        /// Indicates if the item is selected
        /// </summary>
        protected bool mIsSelected;

        /// <summary>
        /// Indicates if the item can be selected
        /// </summary>
        protected bool mIsSelectable;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="BaseSidebarItem"/>
        /// </summary>
        public BaseSidebarItem()
        {
            mIsSelectable = true;
        }

        #endregion
    }
}