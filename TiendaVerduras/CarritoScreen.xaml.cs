using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para CarritoScreen.xaml
    /// </summary>
    public partial class CarritoScreen : Page
    {
        Utilidades u = new Utilidades();
        public string Indeterminado { get; set; }
        ShopTienda tienda = new ShopTienda();

        public static int valortotal { get; set; }
 
        public static List<CarritoData> CarritoFinal { get; set; } = new List<CarritoData>();

        public CarritoScreen()
        {
            InitializeComponent();
            lblTotal.TextAlignment = TextAlignment.Left;
            CarritoFinal = tienda.ListaCarrito();
            valortotal = CalcularTotal(CarritoFinal);
            CargarCarrito();

        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ShopTienda());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public List<CarritoData> ListaCarritoFinal()
        {
            return CarritoFinal;
        }

        public void CargarCarrito()
        {

            if (CarritoFinal != null)
            {
                ListCarritoR.ItemsSource = CarritoFinal;
                lblTotal.Text = CalcularTotal(CarritoFinal).ToString("C", CultureInfo.GetCultureInfo("es-CL"));
            }


        }

        //private List<CarritoData> GenerarCarrito()
        //{
        //    List<CarritoData> ListaCarrito = new List<CarritoData>();
            
        //    //string[] CarritoLineas = File.ReadAllLines("userdata/carrito.dat");

        //    //foreach (var linea in CarritoLineas)
        //    //{
        //    //    string[] LineasDivid = linea.Split(',');

        //    //        CarritoData cd = new CarritoData();
        //    //        cd.IdProducto = Convert.ToInt32(u.DecodearString(LineasDivid[0]));
        //    //        cd.NombreProducto = u.DecodearString(LineasDivid[1]);
        //    //        cd.IdUsuario = Convert.ToInt32(u.DecodearString(LineasDivid[2]));
        //    //        int.TryParse(u.DecodearString(LineasDivid[3]), out int PrecioProd);
        //    //        cd.PrecioProducto = PrecioProd;
        //    //        int.TryParse(u.DecodearString(LineasDivid[4]), out int CantidadProd);
        //    //        cd.CantidadProducto = CantidadProd;
        //    //        int CalculoA = PrecioProd * CantidadProd;
        //    //        cd.SubtotalProducto = CalculoA;

        //    //        int STProductoIVa = Convert.ToInt32(CalculoA + CalculoA * 0.19);

        //    //        cd.SubtotalProductoIVA = CalculoA;
        //    //        cd.UnidadProducto = u.DecodearString(LineasDivid[5]);

        //    //        ListaCarrito.Add(cd);

        //    //}

             
             

        //    return sp.NuevoCarrito;
        //}

        //public List<CarritoData> GenerarCarrito()
        //{

        //}

        private int CalcularTotal(List<CarritoData> ListaActual)
        {
            int Preciototal = 0;
            for (int i = 0; i < ListaActual.Count; i++)
            {
                var PrecioSub = ListaActual[i] as CarritoData;
                var Sumade = PrecioSub.SubtotalProducto;
                Preciototal += Sumade;
            }

            return Preciototal;
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (ListCarritoR.Items.Count == 0)
            {
                MessageBox.Show("Por favor, agrega como mínimo un producto para continuar con la compra", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.NavigationService.Navigate(new DireccionScreen());

            }
        }

        private void btnVaciar_Click(object sender, RoutedEventArgs e)
        {
            tienda.ListaCarrito().Clear();
            ListCarritoR.ItemsSource = null;
            CargarCarrito();

        }

        private void IudCantidadCarrito_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {


            
        }


    }
}
