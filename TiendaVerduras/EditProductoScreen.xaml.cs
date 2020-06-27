using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Lógica de interacción para EditProductoScreen.xaml
    /// </summary>
    public partial class EditProductoScreen : Page
    {
        Utilidades u = new Utilidades();

        ValidacionLogin.ServiceClient serviciox = new ValidacionLogin.ServiceClient();

        int idd { get; set; }
        

        public EditProductoScreen()
        {
            InitializeComponent();
            SetData();
            
        }



        private void SetData()
        {

            string readfile = File.ReadAllText("userdata/producto.dat");

            tbNombre.Text = u.DecodearString(readfile.Split(',')[1]);
            tbPrecio.Text = u.DecodearString(readfile.Split(',')[2]);
            tbStock.Text = u.DecodearString(readfile.Split(',')[3]);
            tbUnidad.Text = u.DecodearString(readfile.Split(',')[4]);
            idd = Convert.ToInt32(u.DecodearString(readfile.Split(',')[0]));

            
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (serviciox.ActualizarDatoProductos(tbNombre.Text, Convert.ToInt32(tbPrecio.Text), Convert.ToInt32(tbStock.Text), tbUnidad.Text, idd))
            {
                MessageBox.Show("Datos del producto actualizados con éxito.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.Navigate(new ShopTienda());
            }
        }
    }
}
