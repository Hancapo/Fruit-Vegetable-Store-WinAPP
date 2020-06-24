using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace TiendaVerduras
{
    class ProductoData
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public int StockProducto { get; set; }
        public int PrecioProducto { get; set; }
        public string FuenteImagen { get; set; }
        public int CantidadProducto { get; set; }
        public string UnidadProducto { get; set; }
    }
}
