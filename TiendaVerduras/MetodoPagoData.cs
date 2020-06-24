using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaVerduras
{
    class MetodoPagoData
    {
        public int idPago { get; set; }


        enum TipoPago
        {
            Credito,
            Debito,
            Transferencia
        }


    }
}
