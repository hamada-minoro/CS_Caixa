using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinConfirmarData.xaml
    /// </summary>
    public partial class WinConfirmarData : Window
    {
        WinLogin _login;

         Usuario usuarioLogado = new Usuario();

         List<Usuario> Usuarios = new List<Usuario>();

         ClassUsuario classUsuario = new ClassUsuario();

        public WinConfirmarData(WinLogin login)
        {
            _login = login;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDataSistema.SelectedDate = DateTime.Now.Date;

            Usuarios = classUsuario.ListaUsuariosMaster();

            cmbLogin.ItemsSource = Usuarios.Select(p => p.NomeUsu);
            
            cmbLogin.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void cmbLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuarioLogado = (Usuario)Usuarios[cmbLogin.SelectedIndex];
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {

            if (dpDataSistema.SelectedDate == null)
            {
                MessageBox.Show("Informe a data do sistema.", "Data", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }


            string senha = ClassCriptografia.Encrypt(txtSenha.Password);
                       

            bool autorizado = classUsuario.VerificaAutenticacao(usuarioLogado.Id_Usuario, senha);

            if (autorizado)
            {
                _login.dataSistema = dpDataSistema.SelectedDate.Value;
                _login.lblData.Content = string.Format("Data e Hora: {0}", _login.dataSistema);
                this.Close();
            }
            else
            {
                MessageBox.Show("Senha Inválida.", "Senha", MessageBoxButton.OK, MessageBoxImage.Stop);
                txtSenha.Focus();
                txtSenha.SelectAll();
            }
           
        }

        private void groupBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest
                    (FocusNavigationDirection.Next));

            }
        }
    }
}
