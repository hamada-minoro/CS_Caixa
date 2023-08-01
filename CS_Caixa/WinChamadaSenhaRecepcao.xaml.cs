using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    /// Lógica interna para WinChamadaSenhaRecepcao.xaml
    /// </summary>
    public partial class WinChamadaSenhaRecepcao : Window
    {


        ClassAtendimento classAtendimento = new ClassAtendimento();

        List<Atendimento> ListaFilaInformacao = new List<Atendimento>();

        List<Atendimento> ListaTodosAtendimentos = new List<Atendimento>();

        public Atendimento atendimento;

        Usuario _usuario;

        ConexaoPainelSenha conexao = new ConexaoPainelSenha();

        public string acaoConfirmaSenha;

        public string senhaAtual;

        WinPrincipal _principal;

        List<Usuario> Usuarios = new List<Usuario>();

        ClassUsuario classUsuario = new ClassUsuario();

        ClassBalcao classBalcao = new ClassBalcao();

        ClassAto classAto = new ClassAto();

        private StreamWriter stwEnviador;
        private TcpClient tcpServidor;
        // Necessário para atualizar o formulário com mensagens da outra thread
        private delegate void AtualizaLogCallBack(string strMensagem);
        // Necessário para definir o formulário para o estado "disconnected" de outra thread
        private delegate void FechaConexaoCallBack(string strMotivo);
        private IPAddress enderecoIP;
        string nomeMaquina = Environment.MachineName.Substring(Environment.MachineName.Length - 2, 2);




        public WinChamadaSenhaRecepcao(WinPrincipal principal)
        {
            InitializeComponent();
            _usuario = principal.usuarioLogado;
            _principal = principal;
        }

        private void ObterConexao()
        {
            try
            {
                conexao = classAtendimento.ObterConexaoServidorAtendimento();

                if (conexao != null)
                {
                    txtIp.Text = conexao.IpServidorAtendimento;
                    txtPorta.Text = conexao.PortaConexao.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível obter a porta de conexão. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }





        public bool InicializaConexao(string senha)
        {
            try
            {


                // Trata o endereço IP informado em um objeto IPAdress
                enderecoIP = IPAddress.Parse(conexao.IpServidorAtendimento);
                // Inicia uma nova conexão TCP com o servidor chat
                tcpServidor = new TcpClient();
                tcpServidor.Connect(enderecoIP, conexao.PortaConexao);




                // Envia o nome do usuário ao servidor
                stwEnviador = new StreamWriter(tcpServidor.GetStream());
                stwEnviador.WriteLine(senha + "_" + nomeMaquina);
                stwEnviador.Flush();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message, "Erro na conexão com servidor", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }


        }




        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_principal.usuarioLogado.Master == true)
            {
                tabItemRestrito.Visibility = Visibility.Visible;
            }
            else
            {
                tabItemRestrito.Visibility = Visibility.Hidden;
            }

            ObterConexao();
            CarregarGrids();

            Usuarios = classUsuario.ListaUsuarios();

            
          
        }

        private void CarregarGrids()
        {
            try
            {

                
                ListaFilaInformacao = classAtendimento.ListaEmEsperaInformacao(DateTime.Now.Date);
                dataGridNormal.ItemsSource = ListaFilaInformacao;

                ListaTodosAtendimentos = classAtendimento.ListaTodosInformacao(DateTime.Now.Date);
                dataGridNormalRestrito.ItemsSource = ListaTodosAtendimentos;

                lblUltSenha.Content = classAtendimento.UltimaSenhaAtendidaUsuario(_usuario.Id_Usuario, DateTime.Now.Date);

                lblPessoasFila.Content = string.Format("Quantidade de pessoas na fila : {0}", ListaFilaInformacao.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }


        public void AtualizaStatus(Atendimento atendimento, string status)
        {
            var atendimentoAlterar = atendimento;

            atendimentoAlterar.HoraAtendimento = atendimento.HoraAtendimento;
            atendimentoAlterar.Status = status;
            atendimentoAlterar.HoraFinalizado = atendimento.HoraFinalizado;
            atendimentoAlterar.HoraRetirada = atendimento.HoraRetirada;
            atendimentoAlterar.IdUsuario = _usuario.Id_Usuario;
            atendimentoAlterar.NomeAtendente = _usuario.NomeUsu;
            atendimentoAlterar.OrdemChamada = atendimento.OrdemChamada;

            classAtendimento.AtualizaAtendimento(atendimentoAlterar);

        }



        public void ChamarSenha()
        {

        }



        private void btnChamarSenha_Click(object sender, RoutedEventArgs e)
        {



            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "RECEP" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente a máquina da Recepção pode chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente a máquina da Recepção pode chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            acaoConfirmaSenha = "";
            CarregarGrids();
            bool result;

            if (ListaFilaInformacao.Count > 0)
            {
                var Informacao = ListaFilaInformacao.FirstOrDefault();
                Informacao.IdUsuario = _usuario.Id_Usuario;
                Informacao.NomeAtendente = _usuario.NomeUsu;
                Informacao.OrdemChamada = DateTime.Now.ToLongTimeString();
                result = InicializaConexao(Informacao.Senha);
                if (result == false)
                    return;
                AtualizaStatus(Informacao, "SENHA CHAMADA");
                atendimento = Informacao;
                senhaAtual = Informacao.Senha;
            }
            else
            {
                MessageBox.Show("A foi encontrado cliente aguardando atendimento. Favor aguardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarGrids();
                return;
            }

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            CarregarGrids();
        }
       

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            CarregarGrids();



        }

        

       

        private void MenuItemChamarSenhaNormalRestrito_Click(object sender, RoutedEventArgs e)
        {
            var alterar = (Atendimento)dataGridNormalRestrito.SelectedItem;

            AtualizaStatus(alterar, "EM ESPERA");

            CarregarGrids();
        }

        

        private void btnAlterarIpPorta_Click(object sender, RoutedEventArgs e)
        {
            string ip = string.Empty;
            int porta = 0;
            try
            {
                if (txtIp.Text != "")
                {
                    ip = txtIp.Text;

                }
                else
                {
                    MessageBox.Show("Informe o endereço IP do Painel Servidor.");
                    return;
                }


                if (txtPorta.Text != "")
                    porta = Convert.ToInt32(txtPorta.Text);
                else
                {
                    MessageBox.Show("Informe a Porta de Comunicação.");
                    return;
                }


                conexao = classAtendimento.SalvarConexaoServidorAtendimento(ip, porta);

                MessageBox.Show("Alteração salva com sucesso!.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPorta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtIp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 144 || key == 148);
        }

    }
}
