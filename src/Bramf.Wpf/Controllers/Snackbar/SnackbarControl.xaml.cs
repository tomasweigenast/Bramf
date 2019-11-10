using System.Windows;
using System.Windows.Controls;

namespace Bramf.Wpf.Controllers.Snackbar
{
    /// <summary>
    /// Lógica de interacción para Snackbar.xaml
    /// </summary>
    public partial class SnackbarControl : UserControl
    {
        /// <summary>
        /// Default constructor for the snackbar
        /// </summary>
        public SnackbarControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SnackbarController.AddSnackbar(this);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            SnackbarController.RemoveSnackbar(this);
        }
    }
}
