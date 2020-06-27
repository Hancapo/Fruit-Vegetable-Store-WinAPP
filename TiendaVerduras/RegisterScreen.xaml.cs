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

namespace TiendaVerduras
{
    /// <summary>
    /// Lógica de interacción para RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Page
    {
        
        public RegisterScreen()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            ValidacionLogin.ServiceClient servicioregister = new ValidacionLogin.ServiceClient();


            if (servicioregister.CrearUsuario(tbCorreo.Text, tbPassword.Password, tbUsuario.Text))
            {
                MessageBox.Show("Cuenta creada exitosamente");
                this.NavigationService.GoBack();

            }
            else
            {
                MessageBox.Show("El correo que ha ingresado ya tiene un usuario asignado.");

            }
            
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

    }
}
