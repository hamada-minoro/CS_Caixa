using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
    /// Lógica interna para WinChamarSenhas.xaml
    /// </summary>
    public partial class WinChamarSenhas : Window
    {

        ClassAtendimento classAtendimento = new ClassAtendimento();

        List<Atendimento> ListaFilaPrioridadeEspecial = new List<Atendimento>();

        List<Atendimento> ListaFilaPrioridade = new List<Atendimento>();

        List<Atendimento> ListaFilaNormal = new List<Atendimento>();

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


        int qtdDias = 0;

        public WinChamarSenhas(WinPrincipal principal)
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

                //var ping = new Ping();
                //var reply = ping.Send("192.168.254.1");
                //if (reply.Status == IPStatus.Success)
                //{
                //    Console.WriteLine("Terminal Online");
                //}
                //else
                //{
                //    Console.WriteLine("Terminal Offline");
                //}

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
                txtIp.IsReadOnly = false;
                txtPorta.IsReadOnly = false;
                btnAlterarIpPorta.IsEnabled = true;
            }
            else
            {
                txtIp.IsReadOnly = true;
                txtPorta.IsReadOnly = true;
                btnAlterarIpPorta.IsEnabled = false;
            }

            ObterConexao();
            CarregarGrids();

            Usuarios = classUsuario.ListaUsuarios();



            datePickerInicioConsulta.SelectedDate = DateTime.Now.Date;

            datePickerFimConsulta.SelectedDate = DateTime.Now.Date;

            cmbLogin.SelectedIndex = -1;

            txtInform.Visibility = Visibility.Hidden;
            datePickerConsulta.Visibility = Visibility.Visible;
            lblInform.Content = "Data:";
            datePickerConsulta.SelectedDate = DateTime.Now.Date;
        }

        private void CarregarGrids()
        {
            try
            {

                ListaFilaPrioridade = classAtendimento.ListaEmEsperaPrioridade(DateTime.Now.Date);
                dataGridPrioridade.ItemsSource = ListaFilaPrioridade;

                ListaFilaPrioridadeEspecial = ListaFilaPrioridade.Where(p => p.TipoAtendimento == "E").ToList();

                ListaFilaNormal = classAtendimento.ListaEmEsperaNormal(DateTime.Now.Date);
                dataGridNormal.ItemsSource = ListaFilaNormal;

                var atendimento = classAtendimento.ObterAtendimentosFinalizadosData(DateTime.Now.Date);

                lblResultados.Content = string.Format("G = {0}  -  P = {1}  -  E = {2}  -  R = {3}", atendimento.Where(p => p.TipoAtendimento == "G").Count(), atendimento.Where(p => p.TipoAtendimento == "P").Count(), atendimento.Where(p => p.TipoAtendimento == "E").Count(), atendimento.Where(p => p.TipoAtendimento == "R").Count());


                dataGridMeusAtendimentos.ItemsSource = atendimento;

                lblUltSenha.Content = classAtendimento.UltimaSenhaAtendidaUsuario(_usuario.Id_Usuario, DateTime.Now.Date);

                var atendendo = classAtendimento.ObterAtendimentosEmAtendimentoData(DateTime.Now.Date);

                dataGridPrioridadeEmAtendimento.ItemsSource = atendendo.Where(p => p.TipoAtendimento == "P" || p.TipoAtendimento == "E");

                dataGridNormalEmAtendimento.ItemsSource = atendendo.Where(p => p.TipoAtendimento == "G");

                var chamada = classAtendimento.ObterAtendimentosChamadaData(DateTime.Now.Date);

                dataGridPrioridadeChamada.ItemsSource = chamada.Where(p => p.TipoAtendimento == "P" || p.TipoAtendimento == "E");

                dataGridNormalChamada.ItemsSource = chamada.Where(p => p.TipoAtendimento == "G");

                var canceladas = classAtendimento.ObterAtendimentosCanceladasData(DateTime.Now.Date);

                dataGridPrioridadeCanceladas.ItemsSource = canceladas.Where(p => p.TipoAtendimento == "P" || p.TipoAtendimento == "E");

                dataGridNormalCanceladas.ItemsSource = canceladas.Where(p => p.TipoAtendimento == "G");

                var emEspera = classAtendimento.ObterAtendimentosData(DateTime.Now.Date);

                dataGridPrioridadeRestrito.ItemsSource = emEspera.Where(p => p.TipoAtendimento == "P" || p.TipoAtendimento == "E");

                dataGridNormalRestrito.ItemsSource = emEspera.Where(p => p.TipoAtendimento == "G");

                lblPessoasFila.Content = string.Format("Quantidade de pessoas na fila : {0}", ListaFilaPrioridade.Count + ListaFilaNormal.Count);
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

            if (atendimentoAlterar == null)
                return;

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


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            acaoConfirmaSenha = "";
            CarregarGrids();
            bool result;

            if (ListaFilaPrioridadeEspecial.Count > 0)
            {
                var prioridadeEspecial = ListaFilaPrioridadeEspecial.FirstOrDefault();
                prioridadeEspecial.IdUsuario = _usuario.Id_Usuario;
                prioridadeEspecial.NomeAtendente = _usuario.NomeUsu;
                prioridadeEspecial.OrdemChamada = DateTime.Now.ToLongTimeString();
                result = InicializaConexao(prioridadeEspecial.Senha);
                if (result == false)
                    return;
                AtualizaStatus(prioridadeEspecial, "SENHA CHAMADA");
                atendimento = prioridadeEspecial;
                senhaAtual = prioridadeEspecial.Senha;

            }
            else
            {
                if (ListaFilaPrioridade.Count > 0)
                {

                    Atendimento ultChamada = classAtendimento.ObterUltimaChamada(DateTime.Now.Date);

                    if (ultChamada != null)
                    {

                        if (ultChamada.TipoAtendimento == "P")
                        {
                            if (ListaFilaNormal.Count > 0)
                            {

                                var normal = ListaFilaNormal.FirstOrDefault();
                                normal.IdUsuario = _usuario.Id_Usuario;
                                normal.NomeAtendente = _usuario.NomeUsu;
                                normal.OrdemChamada = DateTime.Now.ToLongTimeString();
                                result = InicializaConexao(normal.Senha);
                                if (result == false)
                                    return;
                                AtualizaStatus(normal, "SENHA CHAMADA");
                                atendimento = normal;
                                senhaAtual = normal.Senha;
                            }
                            else
                            {
                                if (ListaFilaPrioridade.Count > 0)
                                {
                                    var prioridade = ListaFilaPrioridade.FirstOrDefault();
                                    prioridade.IdUsuario = _usuario.Id_Usuario;
                                    prioridade.NomeAtendente = _usuario.NomeUsu;
                                    prioridade.OrdemChamada = DateTime.Now.ToLongTimeString();
                                    result = InicializaConexao(prioridade.Senha);
                                    if (result == false)
                                        return;
                                    AtualizaStatus(prioridade, "SENHA CHAMADA");
                                    atendimento = prioridade;
                                    senhaAtual = prioridade.Senha;

                                }
                                else
                                {
                                    MessageBox.Show("Não foi encontrado cliente aguardando atendimento. Favor agurardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                                    CarregarGrids();
                                    return;
                                }
                            }

                        }
                        else
                        {
                            if (ListaFilaPrioridade.Count > 0)
                            {
                                var prioridade = ListaFilaPrioridade.FirstOrDefault();
                                prioridade.IdUsuario = _usuario.Id_Usuario;
                                prioridade.NomeAtendente = _usuario.NomeUsu;
                                prioridade.OrdemChamada = DateTime.Now.ToLongTimeString();
                                result = InicializaConexao(prioridade.Senha);
                                if (result == false)
                                    return;
                                AtualizaStatus(prioridade, "SENHA CHAMADA");
                                atendimento = prioridade;
                                senhaAtual = prioridade.Senha;

                            }
                            else
                            {
                                if (ListaFilaNormal.Count > 0)
                                {

                                    var normal = ListaFilaNormal.FirstOrDefault();
                                    normal.IdUsuario = _usuario.Id_Usuario;
                                    normal.NomeAtendente = _usuario.NomeUsu;
                                    normal.OrdemChamada = DateTime.Now.ToLongTimeString();
                                    result = InicializaConexao(normal.Senha);
                                    if (result == false)
                                        return;
                                    AtualizaStatus(normal, "SENHA CHAMADA");
                                    atendimento = normal;
                                    senhaAtual = normal.Senha;
                                }
                                else
                                {
                                    MessageBox.Show("Não foi encontrado cliente aguardando atendimento. Favor agurardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                                    CarregarGrids();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {

                        if (ListaFilaPrioridade.Count > 0)
                        {
                            var prioridade = ListaFilaPrioridade.FirstOrDefault();
                            prioridade.IdUsuario = _usuario.Id_Usuario;
                            prioridade.NomeAtendente = _usuario.NomeUsu;
                            prioridade.OrdemChamada = DateTime.Now.ToLongTimeString();
                            result = InicializaConexao(prioridade.Senha);
                            if (result == false)
                                return;
                            AtualizaStatus(prioridade, "SENHA CHAMADA");
                            atendimento = prioridade;
                            senhaAtual = prioridade.Senha;

                        }
                        else
                        {
                            if (ListaFilaNormal.Count > 0)
                            {

                                var normal = ListaFilaNormal.FirstOrDefault();
                                normal.IdUsuario = _usuario.Id_Usuario;
                                normal.NomeAtendente = _usuario.NomeUsu;
                                normal.OrdemChamada = DateTime.Now.ToLongTimeString();
                                result = InicializaConexao(normal.Senha);
                                if (result == false)
                                    return;
                                AtualizaStatus(normal, "SENHA CHAMADA");
                                atendimento = normal;
                                senhaAtual = normal.Senha;
                            }
                            else
                            {
                                MessageBox.Show("Não foi encontrado cliente aguardando atendimento. Favor agurardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                                CarregarGrids();
                                return;
                            }
                        }


                    }

                }
                else
                {
                    if (ListaFilaNormal.Count > 0)
                    {

                        var normal = ListaFilaNormal.FirstOrDefault();
                        normal.IdUsuario = _usuario.Id_Usuario;
                        normal.NomeAtendente = _usuario.NomeUsu;
                        normal.OrdemChamada = DateTime.Now.ToLongTimeString();
                        result = InicializaConexao(normal.Senha);
                        if (result == false)
                            return;
                        AtualizaStatus(normal, "SENHA CHAMADA");
                        atendimento = normal;
                        senhaAtual = normal.Senha;
                    }
                    else
                    {
                        MessageBox.Show("Não foi encontrado cliente aguardando atendimento. Favor agurardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                        CarregarGrids();
                        return;
                    }
                }

            }
            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();

            }


            CarregarGrids();
        }

        private void ChamarPrioridade()
        {
            bool result;

            var prioridade = ListaFilaPrioridade.FirstOrDefault();
            prioridade.IdUsuario = _usuario.Id_Usuario;
            prioridade.NomeAtendente = _usuario.NomeUsu;
            result = InicializaConexao(prioridade.Senha);
            if (result == false)
                return;
            AtualizaStatus(prioridade, "SENHA CHAMADA");
            atendimento = prioridade;
            senhaAtual = prioridade.Senha;
        }

        private void ChamarNormal()
        {
            if (ListaFilaNormal.Count > 0)
            {
                bool result;

                var normal = ListaFilaNormal.FirstOrDefault();
                normal.IdUsuario = _usuario.Id_Usuario;
                normal.NomeAtendente = _usuario.NomeUsu;
                result = InicializaConexao(normal.Senha);
                if (result == false)
                    return;
                AtualizaStatus(normal, "SENHA CHAMADA");
                atendimento = normal;
                senhaAtual = normal.Senha;
            }
            else
            {
                MessageBox.Show("Não foi encontrado cliente aguardando atendimento. Favor agurardar a retirada da senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarGrids();
                return;
            }
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            CarregarGrids();



        }

        private void dataGridReciboBalcao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridReciboBalcao.SelectedItem != null)
            {
                dataGridSelos.ItemsSource = classAtendimento.ObterSelosRecibo(((ReciboBalcao)dataGridReciboBalcao.SelectedItem).IdReciboBalcao);
                dataGridSelos.SelectedIndex = 0;
            }
            else
                dataGridSelos.ItemsSource = null;
        }

        private void dataGridMeusAtendimentos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridMeusAtendimentos.SelectedItem != null)
            {
                dataGridReciboBalcao.ItemsSource = classAtendimento.ObterReciboPorIdAtendimento(((Atendimento)dataGridMeusAtendimentos.SelectedItem).AtendimentoId);
                dataGridReciboBalcao.SelectedIndex = 0;
            }
            else
                dataGridReciboBalcao.ItemsSource = null;
        }

        private void MenuItemChamarSenha_Click(object sender, RoutedEventArgs e)
        {

            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var atend = (Atendimento)dataGridPrioridadeEmAtendimento.SelectedItem;



            if (atend == null)
                return;

            bool result;

            var verificaAtend = classAtendimento.ObterAtendimentosPorId(atend.AtendimentoId);

            if (verificaAtend.Status != "ATENDENDO")
            {
                MessageBox.Show("Esta senha já foi chamada por outro atendente", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                CarregarGrids();
                return;
            }

            result = InicializaConexao(atend.Senha);
            if (result == false)
                return;

            AtualizaStatus(atend, "SENHA CHAMADA");
            atendimento = atend;
            senhaAtual = atend.Senha;

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();

            }

            CarregarGrids();
        }

        private void MenuItemChamarSenhaNormal_Click(object sender, RoutedEventArgs e)
        {

            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var atend = (Atendimento)dataGridNormalEmAtendimento.SelectedItem;

            if (atend == null)
                return;

            bool result;

            var verificaAtend = classAtendimento.ObterAtendimentosPorId(atend.AtendimentoId);

            if (verificaAtend.Status != "ATENDENDO")
            {
                MessageBox.Show("Esta senha já foi chamada por outro atendente", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                CarregarGrids();
                return;
            }


            result = InicializaConexao(atend.Senha);
            if (result == false)
                return;

            AtualizaStatus(atend, "SENHA CHAMADA");
            atendimento = atend;
            senhaAtual = atend.Senha;

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();


            }

            CarregarGrids();
        }

        private void MenuItemChamarSenhaPrioridadeChamada_Click(object sender, RoutedEventArgs e)
        {

            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var atend = (Atendimento)dataGridPrioridadeChamada.SelectedItem;

            if (atend == null)
                return;

            bool result;

            var verificaAtend = classAtendimento.ObterAtendimentosPorId(atend.AtendimentoId);

            if (verificaAtend.Status != "SENHA CHAMADA")
            {
                MessageBox.Show("Esta senha já foi chamada por outro atendente", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                CarregarGrids();
                return;
            }

            result = InicializaConexao(atend.Senha);
            if (result == false)
                return;

            AtualizaStatus(atend, "SENHA CHAMADA");
            atendimento = atend;
            senhaAtual = atend.Senha;

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();


            }
            CarregarGrids();
        }

        private void MenuItemChamarSenhaNormalChamada_Click(object sender, RoutedEventArgs e)
        {
            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var atend = (Atendimento)dataGridNormalChamada.SelectedItem;

            if (atend == null)
                return;

            bool result;

            var verificaAtend = classAtendimento.ObterAtendimentosPorId(atend.AtendimentoId);

            if (verificaAtend.Status != "SENHA CHAMADA")
            {
                MessageBox.Show("Esta senha já foi chamada por outro atendente", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                CarregarGrids();
                return;
            }

            result = InicializaConexao(atend.Senha);
            if (result == false)
                return;

            AtualizaStatus(atend, "SENHA CHAMADA");
            atendimento = atend;
            senhaAtual = atend.Senha;

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();


            }

            CarregarGrids();
        }

        private void MenuItemChamarSenhaNormalRestrito_Click(object sender, RoutedEventArgs e)
        {
            var alterar = (Atendimento)dataGridNormalRestrito.SelectedItem;

            AtualizaStatus(alterar, "EM ESPERA");

            CarregarGrids();
        }

        private void MenuItemChamarSenhaPrioridadeRestrito_Click(object sender, RoutedEventArgs e)
        {
            var alterar = (Atendimento)dataGridPrioridadeRestrito.SelectedItem;

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

        private void btnConsultaRelatorio_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerInicioConsulta.SelectedDate != null && datePickerFimConsulta.SelectedDate != null)
                ConsultarRelatorios();
            ObterResultados();

            var funcionario = (Usuario)cmbLogin.SelectedItem;
            dataGridConsultaRelatorios.ItemsSource = ListaTodosAtendimentos.Where(p => p.IdUsuario == funcionario.Id_Usuario);
        }


        private void ConsultarRelatorios()
        {
            //ListaTodosAtendimentos = classAtendimento.ObterAtendimentosFinalizadosPorPeriodo(datePickerInicioConsulta.SelectedDate.Value, datePickerFimConsulta.SelectedDate.Value);
            qtdDias = CountDiasUteis(datePickerInicioConsulta.SelectedDate.Value, datePickerFimConsulta.SelectedDate.Value);

            var listaFuncionarios = ListaTodosAtendimentos.Select(p => p.IdUsuario).Distinct().ToList();

            List<Usuario> usu = new List<Usuario>();


            int m = 0;
            string nomeMelhor = "";

            for (int i = 0; i < listaFuncionarios.Count; i++)
            {
                var nome = Usuarios.Where(p => p.Id_Usuario == listaFuncionarios[i]).FirstOrDefault();

                usu.Add(nome);
                if (ListaTodosAtendimentos.Where(p => p.IdUsuario == nome.Id_Usuario).Count() > m)
                {
                    m = ListaTodosAtendimentos.Where(p => p.IdUsuario == nome.Id_Usuario).Count();
                    nomeMelhor = nome.NomeUsu;
                }
            }

            lblMelhorDesempenho.Content = nomeMelhor;



            cmbLogin.ItemsSource = usu;
            cmbLogin.DisplayMemberPath = "NomeUsu";

            cmbLogin.SelectedItem = usu.Where(p => p.NomeUsu == nomeMelhor).FirstOrDefault();
        }

        private void ObterResultados()
        {
            try
            {
                var nome = (Usuario)cmbLogin.SelectedItem;

                var quantAtendimento = ListaTodosAtendimentos.Where(p => p.IdUsuario == nome.Id_Usuario).Count();

                lblTotalDiasUteis.Content = string.Format("Total de dias úteis no Período = {0}", qtdDias);
                lblTotalAtendimento.Content = string.Format("Total de atendimentos no Período = {0}", quantAtendimento);
                lblMediaAtendimento.Content = string.Format("Média de atendimentos por dia = {0}", quantAtendimento / qtdDias);


                dataGridConsultaRelatorios.ItemsSource = ListaTodosAtendimentos.Where(p => p.IdUsuario == nome.Id_Usuario);
            }
            catch (Exception)
            {
                //MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        public static int CountDiasUteis(DateTime d1, DateTime d2)
        {

            int fator = (d1 > d2 ? -1 : 1);

            List<DateTime> datas = new List<DateTime>();

            for (DateTime i = d1.Date; (fator == -1 ? i >= d2.Date : i <= d2.Date); i = i.AddDays(fator))
            {
                datas.Add(i);
            }

            return datas.Count(d => d.DayOfWeek != DayOfWeek.Saturday &&
                                    d.DayOfWeek != DayOfWeek.Sunday) * fator;

        }

        private void cmbLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObterResultados();
        }

        private void datePickerInicioConsulta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioConsulta.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioConsulta.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimConsulta.SelectedDate = datePickerInicioConsulta.SelectedDate;

            if (datePickerInicioConsulta.SelectedDate > datePickerFimConsulta.SelectedDate)
            {
                datePickerFimConsulta.SelectedDate = datePickerInicioConsulta.SelectedDate;
            }
        }

        private void datePickerFimConsulta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioConsulta.SelectedDate != null)
            {
                if (datePickerInicioConsulta.SelectedDate > datePickerFimConsulta.SelectedDate)
                {
                    datePickerFimConsulta.SelectedDate = datePickerInicioConsulta.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void cmbTipoConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoConsulta.Focus())
            {
                if (cmbTipoConsulta.SelectedIndex == 0)
                {

                    txtInform.Visibility = Visibility.Hidden;
                    datePickerConsulta.Visibility = Visibility.Visible;
                    lblInform.Content = "Data:";
                    datePickerConsulta.SelectedDate = DateTime.Now.Date;
                }

                if (cmbTipoConsulta.SelectedIndex == 1)
                {
                    txtInform.Visibility = Visibility.Visible;
                    datePickerConsulta.Visibility = Visibility.Hidden;
                    lblInform.Content = "Recibo:";

                    txtInform.Text = "";
                }

                if (cmbTipoConsulta.SelectedIndex == 2)
                {
                    txtInform.Visibility = Visibility.Visible;
                    datePickerConsulta.Visibility = Visibility.Hidden;
                    lblInform.Content = "Selo:";

                    txtInform.Text = "";
                }

                if (cmbTipoConsulta.SelectedIndex == 3)
                {
                    txtInform.Visibility = Visibility.Visible;
                    datePickerConsulta.Visibility = Visibility.Hidden;
                    lblInform.Content = "Senha:";

                    txtInform.Text = "";
                }
            }
        }

        private void txtInform_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (cmbTipoConsulta.SelectedIndex == 1)
            {
                txtInform.MaxLength = 8;

                int key = (int)e.Key;

                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            }

            if (cmbTipoConsulta.SelectedIndex == 2)
            {
                txtInform.MaxLength = 9;
                if (txtInform.Text.Length <= 3)
                {
                    int key = (int)e.Key;

                    e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
                }

                if (txtInform.Text.Length >= 4 && txtInform.Text.Length <= 8)
                {
                    int key = (int)e.Key;

                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                }
            }

            if (cmbTipoConsulta.SelectedIndex == 3)
            {
                txtInform.MaxLength = 4;

                if (txtInform.Text.Length <= 0)
                {
                    int key = (int)e.Key;

                    e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
                }

                if (txtInform.Text.Length >= 1 && txtInform.Text.Length <= 3)
                {
                    int key = (int)e.Key;

                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                }
            }


        }



        private void btnConsultaFinalizados_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTipoConsulta.SelectedIndex == 0)
            {
                if (datePickerConsulta.SelectedDate != null)
                {
                    var atendimento = classAtendimento.ObterAtendimentosFinalizadosData(datePickerConsulta.SelectedDate.Value);

                    lblResultados.Content = string.Format("G = {0}  -  P = {1}  -  E = {2}  -  R = {3}", atendimento.Where(p => p.TipoAtendimento == "G").Count(), atendimento.Where(p => p.TipoAtendimento == "P").Count(), atendimento.Where(p => p.TipoAtendimento == "E").Count(), atendimento.Where(p => p.TipoAtendimento == "R").Count());


                    dataGridMeusAtendimentos.ItemsSource = atendimento;
                    dataGridMeusAtendimentos.SelectedIndex = 0;
                }
            }

            if (cmbTipoConsulta.SelectedIndex == 1)
            {
                if (txtInform.Text != "")
                {
                    var recibo = classBalcao.ListaRecibosBalcaoRecibo(Convert.ToInt32(txtInform.Text));
                    int idAtend = 0;

                    if (recibo.FirstOrDefault().IdAtendimento != null)
                        idAtend = Convert.ToInt32(recibo.FirstOrDefault().IdAtendimento);
                    else
                    {
                        MessageBox.Show("Este Recibo não foi chamado por senha de atendimento. Favor informar um Recibo chamado por senha de atendimento.", "Recibo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var atendimento = classAtendimento.ObterAtendimentosPorId(idAtend);

                    var atendiment = new List<Atendimento>();

                    atendiment.Add(atendimento);

                    dataGridMeusAtendimentos.ItemsSource = atendiment;

                    dataGridMeusAtendimentos.SelectedIndex = 0;
                }
            }

            if (cmbTipoConsulta.SelectedIndex == 2)
            {
                if (txtInform.Text.Length == 9)
                {
                    var letraSelo = txtInform.Text.Substring(0, 4);

                    var numeroSelo = Convert.ToInt32(txtInform.Text.Substring(4, 5));

                    var selo = classAto.ListarAtoSeloBalcao(letraSelo, numeroSelo);

                    List<ReciboBalcao> recibo = new List<ReciboBalcao>();

                    if (selo.Count < 1)
                    {
                        MessageBox.Show("Selo não encontrado.", "Selo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }


                    if (selo.FirstOrDefault().ReciboBalcao != null)
                        recibo = classBalcao.ListaRecibosBalcaoRecibo(Convert.ToInt32(selo.FirstOrDefault().ReciboBalcao));

                    int idAtend = 0;

                    if (recibo.FirstOrDefault().IdAtendimento != null)
                        idAtend = Convert.ToInt32(recibo.FirstOrDefault().IdAtendimento);
                    else
                    {
                        MessageBox.Show("Este Selo não foi chamado por senha de atendimento. Favor informar um Selo chamado por senha de atendimento.", "Selo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var atendimento = classAtendimento.ObterAtendimentosPorId(idAtend);

                    var atendiment = new List<Atendimento>();

                    atendiment.Add(atendimento);

                    dataGridMeusAtendimentos.ItemsSource = atendiment;

                    dataGridMeusAtendimentos.SelectedIndex = 0;

                }
                else
                {
                    MessageBox.Show("Digite um Selo Válido.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (cmbTipoConsulta.SelectedIndex == 3)
            {
                if (txtInform.Text.Length == 4)
                {
                    var senha = classAtendimento.ObterAtendimentosSenha(txtInform.Text);

                    dataGridMeusAtendimentos.ItemsSource = senha;

                    dataGridMeusAtendimentos.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Digite uma Senha Válida.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

            }

        }

        private void dataGridConsultaRelatorios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridConsultaRelatorios.SelectedItem != null)
            {
                dataGridReciboBalcao_Copy.ItemsSource = classAtendimento.ObterReciboPorIdAtendimento(((Atendimento)dataGridConsultaRelatorios.SelectedItem).AtendimentoId);
                dataGridReciboBalcao_Copy.SelectedIndex = 0;
            }
            else
                dataGridReciboBalcao_Copy.ItemsSource = null;
        }

        private void btnAtendimentoRapido_Click(object sender, RoutedEventArgs e)
        {


            if (_principal.usuarioLogado.Master == true || _principal.usuarioLogado.Caixa == true)
            {
                if (MessageBox.Show("Deseja lançar novo recibo? Se deseja apenas alterar um recibo existente clique 'Não'", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    WinBalcaoNovo balcao = new WinBalcaoNovo(_principal, false);
                    balcao.Owner = this;
                    balcao.ShowDialog();
                }
                else
                {

                    int fila = 0;
                    string senha = "";
                    string tipoSenha = "R";
                    string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                    string status = "ATENDENDO";
                    string data = _principal._dataSistema.ToShortDateString();

                    try
                    {

                        string queryFila = "select MAX(Fila) from Atendimento where Data = '" + data + "' and TipoAtendimento = 'R'";

                        using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.CS_CAIXA_DBConnectionString))
                        {
                            SqlCommand command = new SqlCommand(queryFila, connection);
                            command.Connection.Open();
                            string result = command.ExecuteScalar().ToString();

                            if (result != "")
                                fila = Convert.ToInt32(result);

                            if (fila > -1)
                                fila = fila + 1;


                        }



                        senha = string.Format("{0}{1:000}", tipoSenha, fila);

                        senhaAtual = senha;

                        atendimento = new Atendimento();

                        atendimento.Data = Convert.ToDateTime(data).Date;
                        atendimento.Fila = fila;
                        atendimento.Status = status;
                        atendimento.HoraAtendimento = HoraRetiradaSenha;
                        atendimento.HoraRetirada = HoraRetiradaSenha;
                        atendimento.IdUsuario = _usuario.Id_Usuario;
                        atendimento.NomeAtendente = _usuario.NomeUsu;
                        atendimento.OrdemChamada = HoraRetiradaSenha;
                        atendimento.Senha = senha;
                        atendimento.TipoAtendimento = "R";



                        atendimento = classAtendimento.RetirarSenhaRapida(atendimento);


                        CarregarGrids();

                        WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                        balcao.Owner = this;
                        balcao.ShowDialog();

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível obter a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
            }
            else
            {

                int fila = 0;
                string senha = "";
                string tipoSenha = "R";
                string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                string status = "ATENDENDO";
                string data = _principal._dataSistema.ToShortDateString();

                try
                {

                    string queryFila = "select MAX(Fila) from Atendimento where Data = '" + data + "' and TipoAtendimento = 'R'";

                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.CS_CAIXA_DBConnectionString))
                    {
                        SqlCommand command = new SqlCommand(queryFila, connection);
                        command.Connection.Open();
                        string result = command.ExecuteScalar().ToString();

                        if (result != "")
                            fila = Convert.ToInt32(result);

                        if (fila > -1)
                            fila = fila + 1;


                    }



                    senha = string.Format("{0}{1:000}", tipoSenha, fila);

                    senhaAtual = senha;

                    atendimento = new Atendimento();

                    atendimento.Data = Convert.ToDateTime(data).Date;
                    atendimento.Fila = fila;
                    atendimento.Status = status;
                    atendimento.HoraAtendimento = HoraRetiradaSenha;
                    atendimento.HoraRetirada = HoraRetiradaSenha;
                    atendimento.IdUsuario = _usuario.Id_Usuario;
                    atendimento.NomeAtendente = _usuario.NomeUsu;
                    atendimento.OrdemChamada = HoraRetiradaSenha;
                    atendimento.Senha = senha;
                    atendimento.TipoAtendimento = "R";



                    atendimento = classAtendimento.RetirarSenhaRapida(atendimento);


                    CarregarGrids();

                    WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                    balcao.Owner = this;
                    balcao.ShowDialog();

                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível obter a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
        }

        private void MenuItemChamarSenhaPrioridadeCancelada_Click(object sender, RoutedEventArgs e)
        {
            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var atend = (Atendimento)dataGridPrioridadeCanceladas.SelectedItem;

            if (atend == null)
                return;

            bool result;

            var verificaAtend = classAtendimento.ObterAtendimentosPorId(atend.AtendimentoId);

            if (verificaAtend.Status != "CANCELADO")
            {
                MessageBox.Show("Esta senha já foi chamada por outro atendente", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                CarregarGrids();
                return;
            }

            result = InicializaConexao(atend.Senha);
            if (result == false)
                return;

            AtualizaStatus(atend, "SENHA CHAMADA");
            atendimento = atend;
            senhaAtual = atend.Senha;

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();


            }

            CarregarGrids();
        }

        private void MenuItemChamarSenhaNormalCancelada_Click(object sender, RoutedEventArgs e)
        {
            if (Environment.MachineName.Length > 5)
            {

                string maquina = Environment.MachineName.Substring(0, 5);


                if (maquina != "BALCA" && maquina != "ERICK")
                {
                    MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Somente as máquinas do Balcão podem chamar senhas.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var atend = (Atendimento)dataGridNormalCanceladas.SelectedItem;

            if (atend == null)
                return;

            bool result;

            var verificaAtend = classAtendimento.ObterAtendimentosPorId(atend.AtendimentoId);

            if (verificaAtend.Status != "CANCELADO")
            {
                MessageBox.Show("Esta senha já foi chamada por outro atendente", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                CarregarGrids();
                return;
            }

            result = InicializaConexao(atend.Senha);
            if (result == false)
                return;

            AtualizaStatus(atend, "SENHA CHAMADA");
            atendimento = atend;
            senhaAtual = atend.Senha;

            var confirmaChamada = new WinConfirmaChamadaSenha(this);
            confirmaChamada.Owner = this;
            confirmaChamada.ShowDialog();

            if (acaoConfirmaSenha == "iniciar")
            {
                atendimento.HoraAtendimento = DateTime.Now.ToLongTimeString();
                AtualizaStatus(atendimento, "ATENDENDO");

                CarregarGrids();

                WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                balcao.Owner = this;
                balcao.ShowDialog();


            }

            CarregarGrids();

        }
    }
}
