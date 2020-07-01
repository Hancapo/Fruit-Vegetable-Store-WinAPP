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

        public int validacion { get; set; } = 0;

        public RegisterScreen()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {


            ValidacionLogin.ServiceClient servicioregister = new ValidacionLogin.ServiceClient();


            if (String.IsNullOrEmpty(tbCorreo.Text))
            {
                MessageBox.Show("Introduzca un correo electrónico", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                validacion++;
            }

            if (String.IsNullOrEmpty(tbPassword.Password))
            {
                MessageBox.Show("Introduzca una contraseña", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                validacion++;
            }

            if (String.IsNullOrEmpty(tbRUN.Text))
            {
                MessageBox.Show("Introduzca un RUN", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                validacion++;
            }

            if (String.IsNullOrEmpty(tbTelefono.Text))
            {
                MessageBox.Show("Introduzca un teléfono", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                validacion++;
            }

            if (String.IsNullOrEmpty(tbUsuario.Text))
            {
                MessageBox.Show("Introduzca un nombre de usuario", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                validacion++;
            }

            if (validacion == 5)
            {
                if (servicioregister.CrearUsuario(tbCorreo.Text, tbPassword.Password, tbUsuario.Text, "user", tbRUN.Text, tbTelefono.Text))
                {
                    MessageBox.Show("Cuenta creada exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.NavigationService.GoBack();

                }
                else
                {
                    MessageBox.Show("El correo que ha ingresado ya tiene un usuario asignado.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }




            
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void tbRUN_TextChanged(object sender, TextChangedEventArgs e)
        {
            string run_ = tbRUN.Text;
            if (run_.Length == 9)
            {
                run_ = run_.Insert(8,"-");
            }

            tbRUN.Text = run_;
        }


    }
}
