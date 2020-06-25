using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaVerduras
{
    class CarritoData
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadProducto { get; set; }
        public int PrecioProducto { get; set; }
        public int IdUsuario { get; set; }
        public int SubtotalProducto { get; set; }
        public int SubtotalProductoIVA { get; set; }
        public string UnidadProducto { get; set; }
        public int StockProducto { get; set; }
    }
}
