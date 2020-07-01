using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Schema;
using TiendaVerduras.ValidacionLogin;
using Xceed.Wpf.Toolkit;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para ShopTienda.xaml
    /// </summary>
    public partial class ShopTienda : Page
    {
        ServiceClient s = new ServiceClient();

        public int test { get; set; }
        public static List<ProductoData> NuevoProducto { get; set; } = new List<ProductoData>();
        public static List<CarritoData> NuevoCarrito { get; set; } = new List<CarritoData>();
        readonly Utilidades u = new Utilidades();
        public bool EsAdmin { get; set; }
        public bool SePuedeAgregar { get; set; }
        

        public ShopTienda()
        {
            InitializeComponent();
            CargarUsuario();


        }


        public List<ProductoData> ListaProducto()
        {
            return NuevoProducto;
        }
        public List<CarritoData> ListaCarrito()
        {
            return NuevoCarrito;
        }



        private void lbBienUser_Loaded(object sender, RoutedEventArgs e)
        {
            
            lbBienUser.Content = "Bienvenido " + CargarUsuario();
        }

        private string CargarUsuario()
        {
            string correoline;
            string stlines = File.ReadAllLines("userdata/user.dat").First();
            correoline = u.DecodearString(stlines.Split(',')[0]);

            if (u.DecodearString(stlines.Split(',')[3]) == "admin")
            {
                EsAdmin = true;
            }
            else
            {
                EsAdmin = false;
            }

            if (!EsAdmin)
            {
                spShop.Children.Remove(btnAddProducto);
                spShop.Children.Remove(btnEditProducto);
                spShop.Children.Remove(btnBorrarProducto);

            }

            if (EsAdmin)
            {
                spShop.Children.Remove(btnCarrito);
            }

            return correoline;
            
        }

        SqlConnection sqlcon = new SqlConnection(@"Data Source=(localdb)\tiendaduoc;Initial Catalog=Tienda;Integrated Security=true;");

        public List<ProductoData> MostrarProductos()
        {

            sqlcon.Open();

            SqlCommand esecuelecom = new SqlCommand("SELECT * FROM dbo.Productos", sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(esecuelecom);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            List<ProductoData> Fixeo = new List<ProductoData>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductoData dataproductos = new ProductoData()
                {

                    Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                    NombreProducto = dt.Rows[i]["nom_prod"].ToString(),
                    PrecioProducto = Convert.ToInt32(dt.Rows[i]["Precio"]),
                    StockProducto = Convert.ToInt32(dt.Rows[i]["stock"]),
                    FuenteImagen = Directory.GetCurrentDirectory() + "/resources/" + dt.Rows[i]["Id"] + ".jpg",
                    CantidadProducto = Convert.ToInt32(new IntegerUpDown().Value),
                    UnidadProducto = dt.Rows[i]["unidad"].ToString()


                };
                Fixeo.Add(dataproductos);

            }
            sqlcon.Close();
            return Fixeo;

        }


        private void SelecCantidad_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var actboton = (sender as IntegerUpDown);
            
            int indicecantidad = ListaProductoR.Items.IndexOf(actboton);
            
        }

        private void btnCarritoAdd_Click(object sender, RoutedEventArgs e)
        {
            var ubiboton = (sender as Button).DataContext;

            int indiceboton = ListaProductoR.Items.IndexOf(ubiboton);


            var valuex = ListaProductoR.Items[indiceboton] as ProductoData;

            var cantidadvalida = valuex.CantidadProducto;
            //System.Windows.MessageBox.Show(valuex.CantidadProducto.ToString());

            int stocko = valuex.StockProducto;




            if (cantidadvalida == 0)
            {
                MessageBox.Show("Seleccione mínimo 1 o más unidades", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {

                if (cantidadvalida < stocko)
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
                    cd.StockProducto = valuex.StockProducto;
                    cd.SubtotalProducto = valuex.CantidadProducto * valuex.PrecioProducto;
                    NuevoCarrito = RevisarLista(cd, NuevoCarrito);
                    if (SePuedeAgregar)
                    {
                        MessageBox.Show("Se ha agregado un producto");

                    }
                    else
                    {
                        MessageBox.Show("La cantidad introducida supera el stock disponible");

                    }
                }
                else
                {
                    MessageBox.Show("La cantidad introducida supera el stock disponible");

                }


            }

        }


        public List<CarritoData> RevisarLista(CarritoData cd, List<CarritoData> Listas)

        {
            bool EsEncontrado = false;
            bool SeSobrepasa = false;

            List<CarritoData> NuevaLista = new List<CarritoData>();

            if (Listas.Count == 0)
            {
                SePuedeAgregar = true;
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
                        item.SubtotalProducto += cd.SubtotalProducto;

                        if (item.CantidadProducto >= cd.StockProducto)
                        {
                            item.CantidadProducto -= cd.CantidadProducto;
                            item.SubtotalProducto -= cd.SubtotalProducto;

                            SeSobrepasa = true;
                            

                        }
                        else
                        {
                            EsEncontrado = true;
                            if (EsEncontrado)
                            {
                                break;
                            }
                        }

                    }

                }

                if (SeSobrepasa)
                {
                    SePuedeAgregar = false;
                    return Listas;
                }

                if (!EsEncontrado)
                {
                    NuevaLista.Add(cd);
                    Listas = Listas.Concat(NuevaLista).ToList();
                    SePuedeAgregar = true;
                    return Listas;

                }
            }

            SePuedeAgregar = true;
            return Listas;

        }

        public void btnCarrito_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new CarritoScreen());
            //ArchivoCarrito(NuevoCarrito);

        }


        private void ArchivoEditarProducto(int id, string nomprod, int precio, int stock, string uniprod)
        {

            StringBuilder sb = new StringBuilder();
                sb.AppendLine(
                    
                    u.EncodearStrings(id.ToString()) + "," +
                    u.EncodearStrings(nomprod) + "," +
                    u.EncodearStrings(precio.ToString()) + "," + 
                    u.EncodearStrings(stock.ToString()) + "," +
                    u.EncodearStrings(uniprod)
                    
                    );


            File.WriteAllText("userdata/producto.dat", sb.ToString());
        }

        private void btnAddProducto_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductoScreen());
        }

        private void btnEditProducto_Click(object sender, RoutedEventArgs e)
        {
            var cosa = (ListaProductoR.SelectedItem as ProductoData);

            if (ListaProductoR.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("No se ha seleccionado ningún producto", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ArchivoEditarProducto(cosa.Id, cosa.NombreProducto, cosa.PrecioProducto, cosa.StockProducto, cosa.UnidadProducto);
                this.NavigationService.Navigate(new EditProductoScreen());


            }


        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            NuevoCarrito.Clear();

            this.NavigationService.Navigate(new LoginScreen());
            File.Delete("userdata/user.dat");

            if (File.Exists("userdata/producto.dat"))
            {
                File.Delete("userdata/producto.dat");
            }

            if (File.Exists("userdata/carrito.dat"))
            {
                File.Delete("userdata/carrito.dat");
            }

        }

        private void btnAddProducto_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnCarritoAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (EsAdmin)
            {
                var loadd = (sender as Button).Visibility = Visibility.Hidden;

            }
        }

        private void SelecCantidad_Loaded(object sender, RoutedEventArgs e)
        {
            if (EsAdmin)
            {
                var loadd = (sender as IntegerUpDown).Visibility = Visibility.Hidden;
                gvcCantidad.Header = "";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListaProductoR.SelectedItem = null;
            ListaProductoR.ItemsSource = null;
            ListaProductoR.ItemsSource = MostrarProductos(); 


        }

        private void btnBorrarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (ListaProductoR.SelectedItem == null)
            {
                MessageBox.Show("No se ha seleccionado ningún producto", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                var itemseleccionado = (ListaProductoR.SelectedItem as ProductoData);
                s.TraerDato("id", "nom_prod", itemseleccionado.NombreProducto, "dbo.Productos");

                string nombrep = itemseleccionado.NombreProducto;

                if (MessageBox.Show("¿Desea eliminar el producto " + nombrep + "?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    s.BorrarPorId(Convert.ToInt32(s.TraerDato("id", "nom_prod", nombrep, "dbo.Productos")), "dbo.Productos");
                    MessageBox.Show("El producto " + nombrep + " se ha eliminado exitosamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.Navigate(new ShopTienda());

                }


            }
        }
    }



}
