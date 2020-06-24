using System;
using System.Collections.Generic;
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
using System.IO;
using System.Security.Permissions;

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Page
    {
        string userdatafile = "userdata/user.dat";
        Utilidades u = new Utilidades();

        public LoginScreen()
        {
            InitializeComponent();
        }
        

        private void btnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            UsuarioData ud = new UsuarioData();
            ud.contrasenaUsuario = tbContrasena.Password;
            ud.nombreDeUsuario = tbUsuario.Text;

            ValidacionLogin.ServiceClient serviciologin = new ValidacionLogin.ServiceClient();
            if (serviciologin.VerificarAcceso(ud.nombreDeUsuario, ud.contrasenaUsuario) == true)
            {
                CreateLocalData(tbUsuario.Text, tbContrasena.Password, serviciologin.TraerDato("id","nom_usuario", tbUsuario.Text, "dbo.Usuario"));

                MessageBox.Show("Inicio de sesión exitoso");
                this.NavigationService.Navigate(new ShopTienda());

            }
            else
            {
                MessageBox.Show("No se pudo iniciar sesión");
            }
        }

        private void btnAgregarDatos_Click(object sender, RoutedEventArgs e)
        {
            tbContrasena.Password = "MMC7BPWmtVQD";
            tbUsuario.Text = "cgilby4";
        }

        private void CreateLocalData(string usr, string passw, string iduser)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/userdata");

                File.Delete(userdatafile);
                StringBuilder sb = new StringBuilder();

                sb.Append(u.EncodearStrings(usr) + ",");
                sb.Append(u.EncodearStrings(passw) + ",");
                sb.Append(u.EncodearStrings(iduser));
                File.WriteAllText(userdatafile, sb.ToString());


        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterScreen());

        }
    }
}
