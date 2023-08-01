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
    /// Lógica interna para WinConfirmaSenhaMaster.xaml
    /// </summary>
    public partial class WinConfirmaSenhaMaster : Window
    {


        Usuario usuarioLogado = new Usuario();

        List<Usuario> Usuarios = new List<Usuario>();

        ClassUsuario classUsuario = new ClassUsuario();

        WinDigitarAtoRgi _winDigitarAtoRgi;

        WinDigitarAtoProtesto _winDigitarAtoProtesto;

        WinDigitarAtoNotas _winDigitarAtoNotas;

        WinBalcaoNovo _winBalcaoNovo;

        string entrada = string.Empty;

        public WinConfirmaSenhaMaster(WinDigitarAtoRgi winDigitarAtoRgi)
        {
            _winDigitarAtoRgi = winDigitarAtoRgi;
            entrada = "RGI";
            InitializeComponent();
        }

        public WinConfirmaSenhaMaster(WinDigitarAtoProtesto winDigitarAtoProtesto)
        {
            _winDigitarAtoProtesto = winDigitarAtoProtesto;
            entrada = "PROTESTO";
            InitializeComponent();
        }

        public WinConfirmaSenhaMaster(WinDigitarAtoNotas winDigitarAtoNotas)
        {
            _winDigitarAtoNotas = winDigitarAtoNotas;
            entrada = "NOTAS";
            InitializeComponent();
        }

        public WinConfirmaSenhaMaster(WinBalcaoNovo winBalcaoNovo)
        {
            _winBalcaoNovo = winBalcaoNovo;
            entrada = "BALCAO";
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Usuarios = classUsuario.ListaUsuariosMaster();

            cmbLogin.ItemsSource = Usuarios.Select(p => p.NomeUsu);

            cmbLogin.Focus();
        }
        private void cmbLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuarioLogado = (Usuario)Usuarios[cmbLogin.SelectedIndex];
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {

            
            string senha = ClassCriptografia.Encrypt(txtSenha.Password);


            bool autorizado = classUsuario.VerificaAutenticacao(usuarioLogado.Id_Usuario, senha);

            if (autorizado)
            {
                if(entrada == "RGI")
                _winDigitarAtoRgi.senhaConfirmada = true;

                if (entrada == "PROTESTO")
                    _winDigitarAtoProtesto.senhaConfirmada = true;

                if (entrada == "NOTAS")
                    _winDigitarAtoNotas.senhaConfirmada = true;

                if (entrada == "BALCAO")
                    _winBalcaoNovo.senhaConfirmada = true;
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
