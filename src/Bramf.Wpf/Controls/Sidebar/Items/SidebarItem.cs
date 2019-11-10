using System.Windows;
using System.Windows.Input;

namespace Bramf.Wpf.Controls
{
    /// <summary>
    /// Represents a simple sidebar item that contains a label, and icon and a command
    /// that gets executed when the item is clicked
    /// </summary>
    public class SidebarItem : BaseSidebarItem
    {
        /// <summary>
        /// The label of the item
        /// </summary>
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        /// <summary>
        /// The icon of the item
        /// </summary>
        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// A command that gets executed when the item is clicked
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// The icon of the item
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(object),
            typeof(SidebarItem),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// The label of the item
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            nameof(Label),
            typeof(string),
            typeof(SidebarItem),
            new FrameworkPropertyMetadata(""));

        /// <summary>
        /// A command that gets executed when the item is clicked
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(SidebarItem),
            new FrameworkPropertyMetadata(null));
    }
}
