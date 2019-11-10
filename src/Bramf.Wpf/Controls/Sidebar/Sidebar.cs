using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bramf.Wpf.Controls
{
    /// <summary>
    /// Represents a sidebar menu
    /// </summary>
    public class Sidebar : ContentControl
    {
        #region Properties

        /// <summary>
        /// A list of all sidebar items of the control
        /// </summary>
        public List<BaseSidebarItem> Items 
        {
            get => (List<BaseSidebarItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        /// <summary>
        /// The optional header of the sidebar
        /// </summary>
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// The selected item
        /// </summary>
        public BaseSidebarItem SelectedItem
        {
            get => (BaseSidebarItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// The index of the selected item
        /// </summary>
        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        /// <summary>
        /// The font size size of the label of every item
        /// </summary>
        public double ItemFontSize
        {
            get => (double)GetValue(ItemFontSizeProperty);
            set => SetValue(ItemFontSizeProperty, value);
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// The font size of the label of every item
        /// </summary>
        public static readonly DependencyProperty ItemFontSizeProperty = DependencyProperty.Register(
            nameof(ItemFontSize),
            typeof(double),
            typeof(Sidebar),
            new FrameworkPropertyMetadata((double)16));

        /// <summary>
        /// The index of the selected item
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            nameof(SelectedIndex),
            typeof(int),
            typeof(Sidebar),
            new FrameworkPropertyMetadata(0, SelectedIndexChangedCallback));

        /// <summary>
        /// The selected item
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(BaseSidebarItem),
            typeof(Sidebar),
            new FrameworkPropertyMetadata(null, SelectedItemChangedCallback));

        /// <summary>
        /// The optional header of the sidebar
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(Sidebar),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// A list of all sidebar items of the control
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(List<BaseSidebarItem>),
            typeof(Sidebar),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region Constructors

        /// <summary>
        /// Override default style
        /// </summary>
        static Sidebar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Sidebar), new FrameworkPropertyMetadata(typeof(Sidebar)));
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Initializes the control
        /// </summary>
        public override void BeginInit()
        {
            Items = new List<BaseSidebarItem>();
            base.BeginInit(); 
        }

        #endregion

        #region Private Callbacks

        private static void SelectedItemChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            => (obj as Sidebar).SelectedItemChangedHandler(args.NewValue as BaseSidebarItem);

        private void SelectedItemChangedHandler(BaseSidebarItem item)
        {
            if (!Items.Contains(item))
                return;

            SelectedIndex = Items.IndexOf(item);
        }

        private static void SelectedIndexChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
            => (obj as Sidebar).SelectedIndexChangedCallback((int)args.NewValue);

        private void SelectedIndexChangedCallback(int index)
        {
            if (Items.ElementAt(index) == null)
                throw new ArgumentOutOfRangeException(nameof(index));

            SelectedItem = Items.ElementAt(index);
        }

        #endregion
    }
}