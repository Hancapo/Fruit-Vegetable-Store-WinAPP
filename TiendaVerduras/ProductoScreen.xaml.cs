using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para ProductoScreen.xaml
    /// </summary>
    public partial class ProductoScreen : Page
    {
        ValidacionLogin.ServiceClient s = new ValidacionLogin.ServiceClient();
        string filenamu { get; set; }

        public ProductoScreen()
        {
            InitializeComponent();
        }

        private void btnSelecImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            ofd.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            filenamu = ofd.FileName;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(filenamu, UriKind.Absolute);
            bi.EndInit();

            ImImagen.Source = bi;
        }


        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {

            AgregarP();
            File.Copy(filenamu, "resources/" + s.TraerDato("id", "nom_prod", tbNombre.Text , "dbo.Productos") + System.IO.Path.GetExtension(filenamu));

            this.NavigationService.Navigate(new ShopTienda());
        }

        private void AgregarP()
        {
            if (s.AgregarProducto(tbNombre.Text, tbUnidad.Text, Convert.ToInt32(tbStock.Text), Convert.ToInt32(tbPrecio.Text)))
            {
                MessageBox.Show("Producto agregado exitosamente");


            }
            else
            {
                MessageBox.Show("No se ha podido agregar el producto");

            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
