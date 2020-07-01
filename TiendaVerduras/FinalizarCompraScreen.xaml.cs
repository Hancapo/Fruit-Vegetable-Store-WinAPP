using Invoicer.Models;
using Invoicer.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para FinalizarCompraScreen.xaml
    /// </summary>
    public partial class FinalizarCompraScreen : Page
    {
        string guardar { get; set; }
        ValidacionLogin.ServiceClient vsc = new ValidacionLogin.ServiceClient();
        string preciototalarreglado { get; set; }
        DireccionScreen ds = new DireccionScreen();
        CarritoScreen cs = new CarritoScreen();
        MetodoPagoScreen mp = new MetodoPagoScreen();

        Utilidades u = new Utilidades();
        

        public FinalizarCompraScreen()
        {
            InitializeComponent();
        }

        public void testing()
        {

            string nombreu = vsc.TraerDato("nom_user", "id", ds.Domicilios().IdUsuario.ToString(), "dbo.Usuario");



            
            string[] empresa = new string[5];

            string[] persona = new string[6];

            empresa[0] = "Frudasu Limited";
            empresa[1] = "Chile";
            empresa[2] = "Santiago";
            empresa[3] = "La Florida";
            empresa[4] = "Avenida Meggaton 8845";

            persona[0] = nombreu;
            persona[1] = "Chile";
            persona[2] = "Santiago";
            persona[3] = ds.Domicilios().Comuna;
            persona[4] = ds.Domicilios().Calle + " " + ds.Domicilios().NumeroDomicilio;
            persona[5] = ds.Domicilios().CodigoPostal.ToString();

            new InvoicerApi(SizeOption.A4, OrientationOption.Portrait, "$")
                .Title("FACTURA")
                .Reference("333")
                .TextColor("#CC0000")
                .BackColor("#E0fAFF")
                .Image("resources/logo.png", 70, 30)
                .Company(Address.Make("Frudarasu", empresa))
                .Client(Address.Make("Cliente", persona))
                .Items(ListaFinalFinal(cs.ListaCarritoFinal()))
                .Totals(ListaPreciosTotales())
                .Details(DetallesFinales())
                .Footer("www.frudurasu.cl")
                .Save(guardar);

        }

        public List<ItemRow> ListaFinalFinal(List<CarritoData> ListaCarritoTraer)
        {

            List<ItemRow> irr = new List<ItemRow>();
            List<ItemRow> irrIVA = new List<ItemRow>();

            foreach (var item in ListaCarritoTraer)
            {
                ItemRow ir = new ItemRow();

                ir.Name = item.NombreProducto;
                ir.Price = item.PrecioProducto;
                ir.Amount = item.CantidadProducto;
                ir.Total = item.SubtotalProducto;
                ir.Description = item.UnidadProducto;
                ir.VAT = (decimal)19;

                vsc.ActualizarStock(item.CantidadProducto, item.NombreProducto);

                irr.Add(ir);
            }

            return irr;
        }

        public List<TotalRow> ListaPreciosTotales()
        {

            preciototalarreglado = cs.lblTotal.Text.Replace(".", "").Replace("$", "").Trim();

            decimal totalproductos = Convert.ToDecimal(preciototalarreglado);
            decimal totalproductosiva = Convert.ToDecimal(preciototalarreglado) * 0.19M;

            string nombretabla = "Subtotal";
            string nombretablaIVA = "IVA 19%";
            string nombretotal = "Total";

            TotalRow tr = new TotalRow();
            TotalRow tr2 = new TotalRow();
            TotalRow tr3 = new TotalRow();

            tr.Name = nombretabla;
            tr.Value = totalproductos - totalproductosiva;

            tr2.Name = nombretablaIVA;
            tr2.Value = totalproductosiva;

            tr3.Name = nombretotal;
            tr3.Value = totalproductos;

            List<TotalRow> ltr = new List<TotalRow>();
            ltr.Add(tr);
            ltr.Add(tr2);
            ltr.Add(tr3);


            return ltr;

        }

        public List<DetailRow> DetallesFinales()
        {
            DetailRow dr = new DetailRow();

            List<DetailRow> ldr = new List<DetailRow>();

            string titulo = "En caso de cualquier duda acerca de la factura, favor contactarse en nuestro departamento de ventas en ventas@frudurasu.cl";
            string agradecer = "Gracias por preferirnos";

            List<string> parrafo = new List<string>();
            parrafo.Add(titulo);
            parrafo.Add(agradecer);

            dr.Title = "Información de pago";
            dr.Paragraphs = parrafo;

            ldr.Add(dr);

            return ldr;

        }

        public int cantidadtotal(List<CarritoData> CarritoCantidad)
        {
            int cantidad = 0;

            foreach (var item in CarritoCantidad)
            {
                cantidad = cantidad + item.CantidadProducto;
            }

            return cantidad;
        }

        private string traeridusuario()
        {
            string iduser;
            string stlines = File.ReadAllLines("userdata/user.dat").First();
            iduser = u.DecodearString(stlines.Split(',')[2]);
            return iduser;
        }

        private void btnDetallescompra_Click(object sender, RoutedEventArgs e)
        {

            GuardarPDF();

            testing();

            vsc.AgregarDetalleCompra(
            cantidadtotal(cs.ListaCarritoFinal()),
            int.Parse(preciototalarreglado),
            vsc.TraerIdDomicilio(ds.Domicilios().Calle, ds.Domicilios().NumeroDomicilio, ds.Domicilios().CodigoPostal),
            Convert.ToInt32(traeridusuario()), mp.tipopago()
            );
            MessageBox.Show("Compra realizada y factura guardada exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.Navigate(new ShopTienda());

        }

        private void GuardarPDF()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Documentos PDF | *.pdf";
            sfd.ShowDialog();
            guardar = sfd.FileName;
        }

    }
}
