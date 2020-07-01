using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using MessageBox = System.Windows.MessageBox;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para DireccionScreen.xaml
    /// </summary>
    public partial class DireccionScreen : Page
    {
        Utilidades u = new Utilidades();
        SqlConnection ss = new SqlConnection(@"Data Source=(Localdb)\tiendaduoc;Initial Catalog=Tienda;Integrated Security=true;");
        ValidacionLogin.ServiceClient vsc = new ValidacionLogin.ServiceClient();

        static public DomicilioData dd { get; set; } = new DomicilioData();

        public DireccionScreen()
        {
            InitializeComponent();
            MostrarDomicilios();
        }

        public DomicilioData Domicilios()
        {
            return dd;
        }


        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        public void MostrarDomicilios()
        {

            ss.Open();

            SqlCommand esecuelecom = new SqlCommand("SELECT * FROM dbo.direccion WHERE idUsuario = " +  IDUs(), ss);
            SqlDataAdapter sda = new SqlDataAdapter(esecuelecom);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            ObservableCollection<DomicilioData> listdomicilios = new ObservableCollection<DomicilioData>();
            //List<ProductoData> listproducto = new List<ProductoData>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DomicilioData dd = new DomicilioData()
                {

                    IdUsuario = Convert.ToInt32(dt.Rows[i]["idUsuario"]),
                    Calle = dt.Rows[i]["Calle"].ToString(),
                    NumeroDomicilio = Convert.ToInt32(dt.Rows[i]["N_domic"]),
                    CodigoPostal = Convert.ToInt32(dt.Rows[i]["cp"]),
                    Ciudad = dt.Rows[i]["Ciudad"].ToString(),
                    Comuna = dt.Rows[i]["Comuna"].ToString(),
                    Pais = dt.Rows[i]["Pais"].ToString()



                };

                listdomicilios.Add(dd);

            }
            ss.Close();
            ListaDomicilios.ItemsSource = listdomicilios;
        }

        public int IDUs()
        {
            string ids = File.ReadAllLines("userdata/user.dat").First();
            string parts = u.DecodearString(ids.Split(',')[2]);
            return Convert.ToInt32(parts);
        }

        private void btnAgregarDomicilio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vsc.AgregarDomicilio(tbCalle.Text, cbCiudad.Text, cbComuna.Text, Convert.ToInt32(tbCP.Text), IDUs(), Convert.ToInt32(tbNDomic.Text), cbPais.Text))
                {
                    MessageBox.Show("Se ha un domicilio exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    MostrarDomicilios();

                }
                else
                {

                }
            }
            catch (Exception)
            {

                MessageBox.Show("No se pudo agregar el domicilio, verifica los datos e inténtalo nuevamente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListaDomicilios.SelectedItem == null)
            {
                MessageBox.Show("No se ha seleccionado ningún domicilio", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var datos = ListaDomicilios.SelectedItem as DomicilioData;

                dd.Calle = datos.Calle;
                dd.Ciudad = datos.Ciudad;
                dd.CodigoPostal = datos.CodigoPostal;
                dd.Comuna = datos.Comuna;
                dd.IdUsuario = datos.IdUsuario;
                dd.NumeroDomicilio = datos.NumeroDomicilio;
                dd.Pais = datos.Pais;
                dd.IdDom = vsc.TraerIdDomicilio(datos.Calle, datos.NumeroDomicilio, datos.CodigoPostal);
                

                this.NavigationService.Navigate(new MetodoPagoScreen());
            }
        }
    }
}
