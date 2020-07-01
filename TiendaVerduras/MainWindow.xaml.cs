using System;
using System.IO;
using System.Windows;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Principal.NavigationService.Navigate(new LoginScreen());
            if (File.Exists("userdata/user.dat"))
            {
                File.Delete("userdata/user.dat");
            }
            if (File.Exists("userdata/producto.dat"))
            {
                File.Delete("userdata/producto.dat");

            }
            if (File.Exists("userdata/carrito.dat"))
            {
                File.Delete("userdata/carrito.dat");

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
