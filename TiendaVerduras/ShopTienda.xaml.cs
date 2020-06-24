using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para ShopTienda.xaml
    /// </summary>
    public partial class ShopTienda : Page
    {
        public static int test;
        List<CarritoData> NuevoCarrito = new List<CarritoData>();
        Utilidades u = new Utilidades();

        public ShopTienda()
        {
            InitializeComponent();
            CargarUsuario();
            MostrarProductos();
            MetodoPagoData mpd = new MetodoPagoData();
            mpd.

        }




        private void lbBienUser_Loaded(object sender, RoutedEventArgs e)
        {
            
            lbBienUser.Content = "Hola " + CargarUsuario();
        }

        private string CargarUsuario()
        {
            string correoline;
            string stlines = File.ReadAllLines("userdata/user.dat").First();
            correoline = u.DecodearString(stlines.Split(',')[0]);
            return correoline;
            
        }

        SqlConnection sqljaja = new SqlConnection(@"Data Source=(localdb)\servertest;Initial Catalog=Fruteria;Integrated Security=true;");

        public void MostrarProductos()
        {


            sqljaja.Open();

            SqlCommand esecuelecom = new SqlCommand("SELECT * FROM dbo.Producto", sqljaja);
            SqlDataAdapter sda = new SqlDataAdapter(esecuelecom);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            ObservableCollection<ProductoData> listproducto = new ObservableCollection<ProductoData>();
            //List<ProductoData> listproducto = new List<ProductoData>();



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductoData dataproductos = new ProductoData()
                {

                    Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                    NombreProducto = dt.Rows[i]["nom_prod"].ToString(),
                    PrecioProducto = Convert.ToInt32(dt.Rows[i]["Precio"]),
                    StockProducto = Convert.ToInt32(dt.Rows[i]["Stock"]),
                    FuenteImagen = Directory.GetCurrentDirectory() + "/resources/" + dt.Rows[i]["Id"] + ".jpg",
                    CantidadProducto = Convert.ToInt32(new IntegerUpDown().Value),
                    UnidadProducto = dt.Rows[i]["Unidad"].ToString()
                    
                    
                };

                listproducto.Add(dataproductos);

            }
            sqljaja.Close();
            ListaProductoR.ItemsSource = listproducto;

             



        }

        public void VerificarCantidadLogica()
        {
            
        }

        private void SelecCantidad_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var actboton = (sender as IntegerUpDown).Value;
            
            int indicecantidad = ListaProductoR.Items.IndexOf(actboton);
            
        }

        private void btnCarritoAdd_Click(object sender, RoutedEventArgs e)
        {
            var ubiboton = (sender as Button).DataContext;

            int indiceboton = ListaProductoR.Items.IndexOf(ubiboton);


            var valuex = ListaProductoR.Items[indiceboton] as ProductoData;

            var cantidadvalida = valuex.CantidadProducto;
            //System.Windows.MessageBox.Show(valuex.CantidadProducto.ToString());

            if (cantidadvalida == 0)
            {
                System.Windows.MessageBox.Show("Seleccione mínimo 1 o más unidades");

            }
            else
            {
                string stlines = File.ReadAllLines("userdata/user.dat").First();

                var StringIDUsuario = u.DecodearString(stlines.Split(',')[2]);

                CarritoData cd = new CarritoData();
                cd.IdProducto = valuex.Id;
                cd.NombreProducto = valuex.NombreProducto;
                cd.CantidadProducto = valuex.CantidadProducto;
                cd.PrecioProducto = valuex.PrecioProducto;
                cd.IdUsuario = Convert.ToInt32(StringIDUsuario);
                cd.UnidadProducto = valuex.UnidadProducto;
                NuevoCarrito = RevisarLista(cd, NuevoCarrito);
                System.Windows.MessageBox.Show("Añadido al carrito");

             }

        }


        private void ListaProductoR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            

        }



        //public void busca()
        //{
        //    foreach (sh)
        //}

        private List<CarritoData> RevisarLista(CarritoData cd, List<CarritoData> Listas)

        {
            bool EsEncontrado = false;

            List<CarritoData> NuevaLista = new List<CarritoData>();
            if (Listas.Count == 0)
            {
                Listas.Add(cd);
                return Listas;

            }
            else
            {
                foreach (var item in Listas)
                {
                    if (cd.IdProducto == item.IdProducto)
                    {
                        item.CantidadProducto += cd.CantidadProducto;
                        EsEncontrado = true;
                        if (EsEncontrado)
                        {
                            break;
                        }
                    }

                }

                if (!EsEncontrado)
                {
                    NuevaLista.Add(cd);
                    Listas = Listas.Concat(NuevaLista).ToList();
                    return Listas;

                }
            }

            return Listas;

        }

        private void btnCarrito_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new CarritoScreen());
            ArchivoCarrito(NuevoCarrito);

        }


        private void ArchivoCarrito(List<CarritoData> ItemsCarrito)
        {
            StringBuilder sb = new StringBuilder();

            

            foreach (var item in ItemsCarrito)
            {
                sb.AppendLine(
                    
                    u.EncodearStrings(item.IdProducto.ToString()) + "," +
                    u.EncodearStrings(item.NombreProducto) + "," + u.EncodearStrings(item.IdUsuario.ToString()) + "," + u.EncodearStrings(item.PrecioProducto.ToString()) + "," + u.EncodearStrings(item.CantidadProducto.ToString()) + ","
                    + u.EncodearStrings(item.UnidadProducto)
                    

                    
                    );
            }

            File.WriteAllText("userdata/carrito.dat", sb.ToString());

        }

        private void btnAddProducto_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductoScreen());
        }

        private void btnEditProducto_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditProductoScreen());
        }
    }



}
