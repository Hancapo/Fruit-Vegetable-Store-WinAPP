using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para CarritoScreen.xaml
    /// </summary>
    public partial class CarritoScreen : Page
    {
        List<CarritoData> NuevoCarrito = new List<CarritoData>();
        Utilidades u = new Utilidades();

        public CarritoScreen()
        {
            InitializeComponent();
            lblTotal.TextAlignment = TextAlignment.Left;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ShopTienda());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NuevoCarrito = GenerarCarrito();
            ListCarritoR.ItemsSource = NuevoCarrito;
            lblTotal.Text = CalcularTotal(NuevoCarrito).ToString("C", CultureInfo.GetCultureInfo("es-CL"));
        }

        private List<CarritoData> GenerarCarrito()
        {
            List<CarritoData> ListaCarrito = new List<CarritoData>();

            string[] CarritoLineas = File.ReadAllLines("userdata/carrito.dat");

            foreach (var linea in CarritoLineas)
            {
                string[] LineasDivid = linea.Split(',');

                    CarritoData cd = new CarritoData();
                    cd.IdProducto = Convert.ToInt32(u.DecodearString(LineasDivid[0]));
                    cd.NombreProducto = u.DecodearString(LineasDivid[1]);
                    cd.IdUsuario = Convert.ToInt32(u.DecodearString(LineasDivid[2]));
                    int.TryParse(u.DecodearString(LineasDivid[3]), out int PrecioProd);
                    cd.PrecioProducto = PrecioProd;
                    int.TryParse(u.DecodearString(LineasDivid[4]), out int CantidadProd);
                    cd.CantidadProducto = CantidadProd;
                    int CalculoA = PrecioProd * CantidadProd;
                    cd.SubtotalProducto = CalculoA;
                    
                    int STProductoIVa = Convert.ToInt32(CalculoA + CalculoA * 0.19);

                    cd.SubtotalProductoIVA = CalculoA;
                    cd.UnidadProducto = u.DecodearString(LineasDivid[5]);

                    ListaCarrito.Add(cd);

            }
             

            return ListaCarrito;
        }

        private int CalcularTotal(List<CarritoData> ListaActual)
        {
            int Preciototal = 0;
            for (int i = 0; i < ListaActual.Count; i++)
            {
                var PrecioSub = ListaActual[i] as CarritoData;
                var Sumade = PrecioSub.SubtotalProductoIVA;
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
            ListCarritoR.ItemsSource = null;
            File.Delete("userdata/carrito.dat");
            int cero = 0;
            lblTotal.Text = cero.ToString("C", CultureInfo.GetCultureInfo("es-CL"));
        }
    }
}
