using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;

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
            int acceder = 0;

            if (String.IsNullOrEmpty(tbUsuario.Text))
            {
                MessageBox.Show("Introduzca un usuario.", "Advertencia");
            }
            else
            {
                acceder ++;
            }

            if (String.IsNullOrEmpty(tbContrasena.Password))
            {
                MessageBox.Show("Introduzca la contraseña.", "Advertencia");


            }
            else
            {
                acceder++;

            }

            if (acceder == 2)
            {
                ValidacionLogin.ServiceClient serviciologin = new ValidacionLogin.ServiceClient();

                UsuarioData ud = new UsuarioData();
                ud.nombreDeUsuario = tbUsuario.Text;
                ud.contrasenaUsuario = tbContrasena.Password;

                if (serviciologin.VerificarAcceso(ud.nombreDeUsuario, ud.contrasenaUsuario) == false)
                {
                    MessageBox.Show("No se ha podido iniciar sesión, verifique los datos e inténtelo nuevamente.");
                }
                else
                {
                    CreateLocalData(ud.nombreDeUsuario, ud.contrasenaUsuario, serviciologin.TraerDato("id", "nom_user", ud.nombreDeUsuario, "dbo.Usuario"), serviciologin.TraerDato("TipoUsuario", "nom_user", ud.nombreDeUsuario, "dbo.Usuario"));
                    this.NavigationService.Navigate(new ShopTienda());
                }


            }


        }


        private void CreateLocalData(string usr, string passw, string iduser, string tipouser)
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/userdata");

                File.Delete(userdatafile);
                StringBuilder sb = new StringBuilder();

                sb.Append(u.EncodearStrings(usr) + ",");
                sb.Append(u.EncodearStrings(passw) + ",");
                sb.Append(u.EncodearStrings(iduser) + ","); 
                sb.Append(u.EncodearStrings(tipouser));
                File.WriteAllText(userdatafile, sb.ToString());


        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterScreen());

        }


        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            tbContrasena.Password = "soyadmin1";
            tbUsuario.Text = "vicho0202";
        }

        private void btnUsuario_Click(object sender, RoutedEventArgs e)
        {
            tbContrasena.Password = "IrbmmUy3oalb";
            tbUsuario.Text = "bmundell0";
        }

    }
}
