using System.Windows.Controls;

namespace Bramf.Wpf.Controls
{
    /// <summary>
    /// Represents a page with content
    /// </summary>
    public class AppPage : UserControl
    {
        #region Members

        private object mViewModel;

        #endregion

        /// <summary>
        /// The name of the page
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// The view model for the page
        /// </summary>
        public object ViewModel
        {
            get => mViewModel;
            set
            {
                // Ignore if the same
                if (mViewModel == value) return;

                // Set value
                mViewModel = value;
                DataContext = mViewModel;
            }
        }
    }

    /// <summary>
    /// Represents a page with content and a data context
    /// </summary>
    /// <typeparam name="TViewModel">The view model data context</typeparam>
    public class AppPage<TViewModel> : AppPage
    {
        /// <summary>
        /// The view model for the page
        /// </summary>
        public new TViewModel ViewModel
        {
            get => (TViewModel)base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}
