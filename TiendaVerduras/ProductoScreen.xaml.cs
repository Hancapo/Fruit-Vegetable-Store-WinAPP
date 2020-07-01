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
using System.Windows.Forms;
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
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Archivos JPEG (*.jpg, *.jpeg) | *.jpg; *.jpeg;";
            ofd.ShowDialog();
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


        }

        private void AgregarP()
        {

            try
            {
                if (s.AgregarProducto(tbNombre.Text, tbUnidad.Text, Convert.ToInt32(tbStock.Text), Convert.ToInt32(tbPrecio.Text)))
                {
                    System.Windows.MessageBox.Show("Producto agregado exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    Directory.CreateDirectory("resources");

                    if (String.IsNullOrEmpty(filenamu))
                    {
                        filenamu = "userdata/dummy.jpg";
                        File.Copy(filenamu, "resources/" + s.TraerDato("id", "nom_prod", tbNombre.Text, "dbo.Productos") + System.IO.Path.GetExtension(filenamu));

                    }
                    else
                    {
                        File.Copy(filenamu, "resources/" + s.TraerDato("id", "nom_prod", tbNombre.Text, "dbo.Productos") + System.IO.Path.GetExtension(filenamu));

                    }


                    this.NavigationService.Navigate(new ShopTienda());


                }
                else
                {
                    System.Windows.MessageBox.Show("No se ha podido agregar el producto, el producto ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No se ha podido agregar el producto, verifique los datos e inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
