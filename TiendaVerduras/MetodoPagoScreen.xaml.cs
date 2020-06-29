using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para MetodoPagoScreen.xaml
    /// </summary>
    public partial class MetodoPagoScreen : Page
    {
        public MetodoPagoScreen()
        {
            InitializeComponent();
        }

        private void tbNumeroTarjeta_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbNumeroTarjeta.Text.Length == 5)
            {
                tbNumeroTarjeta.Text = tbNumeroTarjeta.Text.Insert(4, "-");
                tbNumeroTarjeta.SelectionStart = tbNumeroTarjeta.Text.Length;
            }

            if (tbNumeroTarjeta.Text.Length == 5 + 4)
            {
                tbNumeroTarjeta.Text = tbNumeroTarjeta.Text.Insert(9, "-");
                tbNumeroTarjeta.SelectionStart = tbNumeroTarjeta.Text.Length;
            }

            if (tbNumeroTarjeta.Text.Length == 9 + 5)
            {
                tbNumeroTarjeta.Text = tbNumeroTarjeta.Text.Insert(9 + 5, "-");
                tbNumeroTarjeta.SelectionStart = tbNumeroTarjeta.Text.Length;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            spDatosTarjeta.Visibility = Visibility.Hidden;
            btnPagarEfectivo.Visibility = Visibility.Hidden;
        }

        private void rbTarjeta_Checked(object sender, RoutedEventArgs e)
        {
            spDatosTarjeta.Visibility = Visibility.Visible;
            btnPagarEfectivo.Visibility = Visibility.Hidden;


        }

        private void rbTarjeta_Unchecked(object sender, RoutedEventArgs e)
        {
            spDatosTarjeta.Visibility = Visibility.Hidden;
            btnPagarEfectivo.Visibility = Visibility.Visible;

        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            int contadorvalidacion = 0;


            if (tbNumeroTarjeta.Text.Length < 19)
            {
                MessageBox.Show("Introduzca un número de tarjeta válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                contadorvalidacion++;
            }

            if (tbCCV.Text.Length < 3)
            {
                MessageBox.Show("Introduzca un CCV.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                contadorvalidacion++;

            }

            if (cbAnio.SelectedIndex == -1 || cbMes.SelectedIndex == -1)
            {
                MessageBox.Show("Introduzca una fecha de vencimiento válida.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                contadorvalidacion++;

            }

            if (contadorvalidacion == 3)
            {
                MessageBox.Show("Se ha realizado un pago efectivo.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
