using System.Windows;
using System.Windows.Controls;

namespace Bramf.Wpf.Controls
{
    /// <summary>
    /// Represents a text box that can be editable
    /// </summary>
    public class EditableTextBox : TextBox
    {
        #region Properties

        /// <summary>
        /// Indicates if the control is being edited
        /// </summary>
        public bool IsEditing
        {
            get => (bool)GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }

        /// <summary>
        /// Indicates if the text box can be edited
        /// </summary>
        public bool CanEdit
        {
            get => (bool)GetValue(CanEditProperty);
            set => SetValue(CanEditProperty, value);
        }

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Indicates if the text box can be edited
        /// </summary>
        public static readonly DependencyProperty CanEditProperty = DependencyProperty.Register(
            nameof(CanEdit),
            typeof(bool),
            typeof(EditableTextBox),
            new FrameworkPropertyMetadata(true, CanEditChangeHandler));

        /// <summary>
        /// Indicates if the control is being edited
        /// </summary>
        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register(
            nameof(IsEditing),
            typeof(bool),
            typeof(EditableTextBox),
            new FrameworkPropertyMetadata(false, IsEditingChangeHandler));

        #endregion

        #region Methods

        private static void IsEditingChangeHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

        }

        private static void CanEditChangeHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

        }

        #endregion

        #region Constructor

        static EditableTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBox), new FrameworkPropertyMetadata(typeof(EditableTextBox)));
        }

        #endregion
    }
}