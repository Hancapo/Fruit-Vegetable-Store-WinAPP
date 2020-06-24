using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaVerduras
{
    class Utilidades
    {
        public string EncodearStrings(string uno)
        {

            var StringEncoded = Encoding.UTF8.GetBytes(uno);

            var STREnc64 = Convert.ToBase64String(StringEncoded);

            return STREnc64;
        }

        public string DecodearString(string uno)
        {
            var base64EncodedBytes = Convert.FromBase64String(uno);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
