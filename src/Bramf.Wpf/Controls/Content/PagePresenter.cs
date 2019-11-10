using Bramf.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bramf.Wpf.Controls
{
    /// <summary>
    /// Stores pages and adds the ability to change them across the application
    /// </summary>
    public class PagePresenter : ItemsControl, INotifyPropertyChanged
    {
        #region Members

        private List<AppPage> mPages; // Contains a list of all pages for this presenter
        private static readonly List<PagePresenter> mPagePresenters = new List<PagePresenter>();

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(
            "Identifier",
            typeof(string),
            typeof(PagePresenter),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            "CurrentPage",
            typeof(AppPage),
            typeof(PagePresenter),
            new UIPropertyMetadata(null));

        #endregion

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        private void OnPropertyChanged(string name) => PropertyChanged(this, new PropertyChangedEventArgs(name));

        #endregion

        #region Properties

        /// <summary>
        /// The identifier name of this page presenter
        /// </summary>
        public string Identifier
        {
            get => (string)GetValue(IdentifierProperty);
            set => SetValue(IdentifierProperty, value);
        }

        /// <summary>
        /// The current showing page
        /// </summary>
        public AppPage CurrentPage
        {
            get => (AppPage)GetValue(CurrentPageProperty);
            set => SetValue(IdentifierProperty, value);
        }

        #endregion

        #region Constructors

        static PagePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PagePresenter), new FrameworkPropertyMetadata(typeof(PagePresenter)));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PagePresenter()
        {
            mPages = new List<AppPage>();
            mPagePresenters.Add(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the current page
        /// </summary>
        /// <param name="page">The new page to set</param>
        /// <param name="viewModel">An optional view model object</param>
        public void ChangePage(AppPage page, object viewModel = null)
        {
            // Avoid bugs
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (!mPages.Contains(page)) throw new InvalidOperationException($"The page '{page.PageName}' is not in the actual page presenter.");

            // Set view model if not null
            if (viewModel != null)
                page.DataContext = viewModel;

            // Update current page
            CurrentPage = page;
            OnPropertyChanged(nameof(CurrentPage));
        }

        /// <summary>
        /// Changes the current page
        /// </summary>
        /// <param name="index">The index of the new page to set</param>
        /// <param name="viewModel">An optional view model object</param>
        public void ChangePage(int index, object viewModel = null)
        {
            // Avoid bugs
            if (index < 0 || index > mPages.Count - 1) throw new ArgumentOutOfRangeException(nameof(index));

            // Try to get page
            var page = mPages.ElementAt(index);

            // Page not found
            if (page == null) throw new InvalidOperationException("Page not found.");

            // Set view model if not null
            if (viewModel != null)
                page.DataContext = viewModel;

            // Update current page
            CurrentPage = page;
            OnPropertyChanged(nameof(CurrentPage));
        }

        /// <summary>
        /// Changes the current page
        /// </summary>
        /// <param name="pageName">The name of the new page to set</param>
        /// <param name="viewModel">An optional view model object</param>
        public void ChangePage(string pageName, object viewModel = null)
        {
            // Avoid bugs
            if (pageName.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(pageName));

            // Try to get page
            var page = mPages.FirstOrDefault(x => x.PageName == pageName);

            // Page not found
            if (page == null) throw new InvalidOperationException("Page not found.");

            // Set view model if not null
            if (viewModel != null)
                page.DataContext = viewModel;

            // Update current page
            CurrentPage = page;
            OnPropertyChanged(nameof(CurrentPage));
        }

        #region Static

        /// <summary>
        /// Returns a <see cref="PagePresenter"/> searching by its identifier name
        /// </summary>
        /// <param name="identifier">The identifier name of the page presenter</param>
        public static PagePresenter GetPagePresenter(string identifier)
        {
            // Avoid bugs
            if (identifier.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(identifier));

            // Try to get presenter
            PagePresenter presenter = default;
            Application.Current.Dispatcher.Invoke(() =>
            {
                presenter = mPagePresenters.FirstOrDefault(x => x.Identifier == identifier);
            });

            // If null, throw exception
            if (presenter == null) throw new InvalidOperationException($"Cannot find a page presenter with identifier '{identifier}'.");

            return presenter;
        }

        #endregion

        #endregion
    }
}
