using System;
using System.Collections.Generic;
using System.Linq;
using CS_Caixa.Controls;
using CS_Caixa.Models;
using System.IO;
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
    /// Lógica interna para WinChamadaSenhaNotas.xaml
    /// </summary>
    public partial class WinChamadaSenhaNotas : Window
    {
        ClassAtendimento classAtendimento = new ClassAtendimento();

        List<Atendimento> ListaFilaNotas = new List<Atendimento>();

        List<Atendimento> ListaFilaNotasVinculados = new List<Atendimento>();

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




        public WinChamadaSenhaNotas(WinPrincipal principal)
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
                    txtIp.Text = conexao.IpServidorAtendimentoNotas;
                    txtPorta.Text = conexao.PortaConexaoNotas.ToString();
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
                enderecoIP = IPAddress.Parse(conexao.IpServidorAtendimentoNotas);
                // Inicia uma nova conexão TCP com o servidor chat
                tcpServidor = new TcpClient();
                tcpServidor.Connect(enderecoIP, Convert.ToInt32(conexao.PortaConexaoNotas));


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

            cmbLogin.ItemsSource = Usuarios.Select(p => p.NomeUsu);
          
        }

        private void CarregarGrids()
        {
            try
            {                
                ListaFilaNotas = classAtendimento.ListaEmEsperaNotas(DateTime.Now.Date);
                dataGridNormal.ItemsSource = ListaFilaNotas;
                dataGridNormal.SelectedIndex = 0;

                ListaFilaNotasVinculados = classAtendimento.ListaVinculadosNotasEmEspera(DateTime.Now.Date);
                dataGridVinculadas.ItemsSource = ListaFilaNotasVinculados;

                ListaTodosAtendimentos = classAtendimento.ListaTodosNotas(DateTime.Now.Date);
                dataGridNormalRestrito.ItemsSource = ListaTodosAtendimentos;

                lblUltSenha.Content = classAtendimento.UltimaSenhaAtendidaUsuario(_usuario.Id_Usuario, DateTime.Now.Date);

                lblPessoasFila.Content = string.Format("Quantidade de pessoas na fila : {0}", ListaFilaNotasVinculados.Count);
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

        public void AtualizaStatusDesvincular(Atendimento atendimento, string status)
        {
            var atendimentoAlterar = atendimento;

            atendimentoAlterar.HoraAtendimento = atendimento.HoraAtendimento;
            atendimentoAlterar.Status = status;
            atendimentoAlterar.HoraFinalizado = atendimento.HoraFinalizado;
            atendimentoAlterar.HoraRetirada = atendimento.HoraRetirada;
            atendimentoAlterar.IdUsuario = null;
            atendimentoAlterar.NomeAtendente = null;
            atendimentoAlterar.OrdemChamada = atendimento.OrdemChamada;

            classAtendimento.AtualizaAtendimento(atendimentoAlterar);

        }


        private void btnChamarSenha_Click(object sender, RoutedEventArgs e)
        {
            
            if (Environment.MachineName.Length > 4)
            {

                string maquina = Environment.MachineName.Substring(0, 4);


                if (maquina != "SALA" && maquina != "ERIC")
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

            if (ListaFilaNotasVinculados.Count > 0)
            {
                var notas = ListaFilaNotasVinculados.Where(p => p.IdUsuario == _usuario.Id_Usuario).FirstOrDefault();
                
                if (notas != null)
                {
                    notas.OrdemChamada = DateTime.Now.ToLongTimeString();
                    result = InicializaConexao(notas.Senha);
                    if (result == false)
                        return;
                    AtualizaStatus(notas, "SENHA CHAMADA");
                    atendimento = notas;
                    senhaAtual = notas.Senha;
                }
                else
                {
                    MessageBox.Show("Não foi encontrado cliente aguardando atendimento para " + _usuario.NomeUsu + ". Favor aguardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarGrids();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Não foi encontrado cliente aguardando atendimento para "+ _usuario.NomeUsu + ". Favor aguardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
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

            AtualizaStatusDesvincular(alterar, "EM ESPERA");

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


                conexao = classAtendimento.SalvarConexaoServidorAtendimentoNotas(ip, porta);

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

        private void btnVincular_Click(object sender, RoutedEventArgs e)
        {

            if (dataGridNormal.Items.Count < 1)
                return;

            if (dataGridNormal.SelectedItem != null)
            {
                var selecionado = (Atendimento)dataGridNormal.SelectedItem;
                var usuario = Usuarios[cmbLogin.SelectedIndex];


                selecionado.IdUsuario = usuario.Id_Usuario;
                selecionado.NomeAtendente = usuario.NomeUsu;
                atendimento = classAtendimento.AtualizaAtendimento(selecionado);
                CarregarGrids();

                var senha = ListaFilaNotasVinculados.Where(p => p.AtendimentoId == atendimento.AtendimentoId).FirstOrDefault();
                dataGridVinculadas.SelectedItem = senha;
                dataGridVinculadas.ScrollIntoView(senha);
                
            }
            else
            {
                MessageBox.Show("Selecione uma senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
