using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para Chamada.xaml
    /// </summary>
    public partial class Chamada : Window
    {

        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioPc_Painel _AppServicoPc_Painel = new RepositorioPc_Painel();
        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();
        RepositorioUsuario _AppServicoUsuario = new RepositorioUsuario();
        RepositorioControle_Uso _AppServicoControle_Uso = new RepositorioControle_Uso();
        ClassAtendimento classAtendimento = new ClassAtendimento();
        List<Senha> senhas = new List<Senha>();
        List<Senha> senhasConsulta = new List<Senha>();
        List<Senha> senhasConsultaRelatorio = new List<Senha>();
        List<Cadastro_Painel> paineis = new List<Cadastro_Painel>();
        List<Cadastro_Pc> estacoes = new List<Cadastro_Pc>();
        List<Pc_Painel> relacoesPcPainal = new List<Pc_Painel>();
        List<Usuario> Usuarios = new List<Usuario>();
        bool piscarAvisoLicenca = false;

        int qtdDias = 0;

        Usuario _usuario;

        List<Controle_Uso> controles = new List<Controle_Uso>();

        Cadastro_Pc meuPc = new Cadastro_Pc();

        string _idMaquina;



        private delegate void AtualizaStatusCallback(string strMensagem);

        List<Cadastro_Pc> maquinasEstacao = new List<Cadastro_Pc>();

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private IPAddress enderecoIPCamadaSenha;
        private StreamWriter stwEnviadorCamadaSenha;
        private TcpClient tcpServidorCamadaSenha;

        private IPAddress enderecoIPAtualizarGridsEstacoes;
        private StreamWriter stwEnviadorAtualizarGridsEstacoes;
        private TcpClient tcpServidorAtualizarGridsEstacoes;

        WinPrincipal _principal;

        public Senha chamarSenha;
        Senha ultimaSenha;

        WinLogin _login;

        public string acao = string.Empty;

        public Chamada(Usuario usuario, string idMaquina, WinPrincipal principal, WinLogin login)
        {
            _idMaquina = idMaquina;
            _usuario = usuario;
            _principal = principal;
            _login = login;
            InitializeComponent();
        }

        public void mainServidor_StatusChanged(object sender, ClassStatusChangedEventArgs e)
        {

            if (e.EventMessage == "Atualizar DataGrids")
                _login.atualizar = true;

            if (e.EventMessage == "Parametros")
                _login.atualizarParametros = true;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                CarregarTudo();

                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();

            }
            catch (Exception)
            {

            }
        }

        public void InicializaConexaoAtualizarGridsEstacoes(Cadastro_Pc maquinaEstacao)
        {
            try
            {

                if (maquinaEstacao.Tipo_Atendimento == "GUICHÊ")
                {
                    enderecoIPAtualizarGridsEstacoes = IPAddress.Parse(maquinaEstacao.Ip_Pc);


                    tcpServidorAtualizarGridsEstacoes = new TcpClient();
                    tcpServidorAtualizarGridsEstacoes.NoDelay = true;

                    tcpServidorAtualizarGridsEstacoes.Connect(enderecoIPAtualizarGridsEstacoes, maquinaEstacao.Porta_Pc);

                    if (tcpServidorAtualizarGridsEstacoes.Connected == true)
                    {
                        stwEnviadorAtualizarGridsEstacoes = new StreamWriter(tcpServidorAtualizarGridsEstacoes.GetStream());
                        stwEnviadorAtualizarGridsEstacoes.WriteLine("Atualizar DataGrids");
                        stwEnviadorAtualizarGridsEstacoes.Flush();
                    }

                    tcpServidorAtualizarGridsEstacoes.Close();
                }
            }

            catch (Exception) { }


        }

        public string InicializaConexaoChamarSenha(Senha senha, Pc_Painel relacoesPcPainal)
        {
            Cadastro_Painel painel = paineis.Where(p => p.Identificador_Pc == relacoesPcPainal.Cadastro_Painel_Id).FirstOrDefault();
            try
            {

                Senha senhaVerificar = _AppServicoSenha.ObterPorId(senha.Senha_Id);

                enderecoIPCamadaSenha = IPAddress.Parse(painel.Ip_Pc);

                tcpServidorCamadaSenha = new TcpClient();
                tcpServidorCamadaSenha.NoDelay = true;
                tcpServidorCamadaSenha.Connect(enderecoIPCamadaSenha, painel.Porta_Pc);

                if (tcpServidorCamadaSenha.Connected == true)
                {
                    stwEnviadorCamadaSenha = new StreamWriter(tcpServidorCamadaSenha.GetStream());
                    stwEnviadorCamadaSenha.WriteLine(senha.Senha_Id.ToString());
                    stwEnviadorCamadaSenha.Flush();
                    tcpServidorCamadaSenha.Close();
                    return "ok";
                }
                else
                    return painel.Nome_Pc;



            }
            catch (Exception) { return painel.Nome_Pc; }


        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_login.atualizar == true)
            {
                CarregarGrids();
                _login.atualizar = false;
            }

            if (_login.atualizarParametros == true)
            {
                CarregarTudo();
                _login.atualizarParametros = false;
            }

            if (piscarAvisoLicenca == true)
            {
                if (lblAvisoLicenca.Visibility == Visibility.Visible)
                    lblAvisoLicenca.Visibility = Visibility.Hidden;
                else
                    lblAvisoLicenca.Visibility = Visibility.Visible;
            }

            if (_login.parametros.DesligarEstacao == true)
            {
                if (_login.parametros.HoraDesligarEstacao == DateTime.Now.ToLongTimeString())
                {
                    Process.Start("Shutdown", "-s -f -t 00");
                }
            }
        }

        private void VerificaLicenca()
        {
            Controle_Uso controle = controles.Where(p => p.AtivacaoUso == ClassCriptografia.Encrypt("V")).FirstOrDefault();

            int licencaAdicional = controles.Where(p => p.ControleId > controle.ControleId).Count();

            if (licencaAdicional == 0)
            {
                DateTime dataFim = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataValidadeFim));

                if (DateTime.Now.Date.AddDays(5) >= dataFim)
                {
                    //int dia = dataFim.Day - DateTime.Now.Day;
                    int dia = (int)dataFim.Subtract(DateTime.Now).TotalDays;

                    if (dia > 1)
                        lblAvisoLicenca.Content = string.Format("Licença expira em {0} dias", dia);
                    else
                        if (dia == 1)
                            lblAvisoLicenca.Content = string.Format("Licença expira em {0} dia", dia);
                        else
                            lblAvisoLicenca.Content = string.Format("Licença expira hoje", dia);

                    piscarAvisoLicenca = true;
                }
                else
                {
                    lblAvisoLicenca.Content = "";
                    piscarAvisoLicenca = false;
                }
            }
        }

        public void CarregarTudo()
        {
            _usuario = _AppServicoUsuario.ObterPorId(_usuario.Id_Usuario);
            lblUsuario.Content = string.Format("Usuário: {0} - {1}", _usuario.NomeUsu, _usuario.Qualificacao);
            _login.parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();
            maquinasEstacao = _AppServicoCadastro_Pc.ObterTodos().Where(p => p.Tipo_Entrada == 1).ToList();
            paineis = _AppServicoCadastro_Painel.ObterTodos().ToList();
            estacoes = _AppServicoCadastro_Pc.ObterTodos().ToList();
            controles = _AppServicoControle_Uso.ObterTodos().ToList();
            meuPc = estacoes.Where(p => p.Identificador_Pc == _idMaquina).FirstOrDefault();
            relacoesPcPainal = _AppServicoPc_Painel.ObterTodos().Where(p => p.Cadastro_Pc_Id == meuPc.Identificador_Pc).ToList();
            Usuarios = _AppServicoUsuario.ObterTodos().ToList();
            switch (meuPc.TipoChamadaSenha)
            {
                case 0:
                    lblTipoSenha.Content = "Máquina configurada para chamar apenas senhas normais.";
                    break;
                case 1:
                    lblTipoSenha.Content = "Máquina configurada para chamar apenas senhas preferenciais.";
                    break;
                case 2:
                    lblTipoSenha.Content = "Máquina configurada para chamar senhas alternadas.";
                    break;
                default:
                    break;
            }

            string setor = "";

            switch (meuPc.SetorId)
            {
                case 0:
                    setor = string.Format("{0}", _login.parametros.Nome_Setor_1);
                    break;
                case 1:
                    setor = string.Format("{0}", _login.parametros.Nome_Setor_2);
                    break;
                case 2:
                    setor = string.Format("{0}", _login.parametros.Nome_Setor_3);
                    break;
                case 3:
                    setor = string.Format("{0}", _login.parametros.Nome_Setor_4);
                    break;
                default:
                    break;
            }


            if (setor != "")
                lblTitulo.Content = string.Format("{0} {1} - {2}", meuPc.Tipo_Atendimento, meuPc.Caracter, setor);
            else
                lblTitulo.Content = string.Format("{0} {1}", meuPc.Tipo_Atendimento, meuPc.Caracter);




            if (_usuario.Chamar_Senha_Fora_Sequencia == false)
            {
                menuPrioridadeEmEspera.Visibility = Visibility.Hidden;
                menuNormalEspera.Visibility = Visibility.Hidden;
            }

            if (_login.atualizarParametros == false)
            {

                switch (_login.parametros.Qtd_Caracteres_Senha)
                {
                    case 0:

                        colSenhaEmEsperaPrioridade.Binding.StringFormat = "000";
                        colSenhaEmEsperaNormal.Binding.StringFormat = "000";
                        colSenhaFinalizadaPrioridade.Binding.StringFormat = "000";
                        colSenhaFinalizadaNormal.Binding.StringFormat = "000";
                        colSenhaChamadasPrioridade.Binding.StringFormat = "000";
                        colSenhaChamadasNormal.Binding.StringFormat = "000";
                        colSenhaCanceladasPrioridade.Binding.StringFormat = "000";
                        colSenhaCanceladasNormal.Binding.StringFormat = "000";
                        colSenhaConsultaPrioridade.Binding.StringFormat = "000";
                        colSenhaConsultaNormal.Binding.StringFormat = "000";
                        colSenhaConsulta.Binding.StringFormat = "000";
                        break;
                    case 1:

                        colSenhaEmEsperaPrioridade.Binding.StringFormat = "0000";
                        colSenhaEmEsperaNormal.Binding.StringFormat = "0000";
                        colSenhaFinalizadaPrioridade.Binding.StringFormat = "0000";
                        colSenhaFinalizadaNormal.Binding.StringFormat = "0000";
                        colSenhaChamadasPrioridade.Binding.StringFormat = "0000";
                        colSenhaChamadasNormal.Binding.StringFormat = "0000";
                        colSenhaCanceladasPrioridade.Binding.StringFormat = "0000";
                        colSenhaCanceladasNormal.Binding.StringFormat = "0000";
                        colSenhaConsultaPrioridade.Binding.StringFormat = "0000";
                        colSenhaConsultaNormal.Binding.StringFormat = "0000";
                        colSenhaConsulta.Binding.StringFormat = "0000";
                        break;
                    case 2:

                        colSenhaEmEsperaPrioridade.Binding.StringFormat = "00000";
                        colSenhaEmEsperaNormal.Binding.StringFormat = "00000";
                        colSenhaFinalizadaPrioridade.Binding.StringFormat = "00000";
                        colSenhaFinalizadaNormal.Binding.StringFormat = "00000";
                        colSenhaChamadasPrioridade.Binding.StringFormat = "00000";
                        colSenhaChamadasNormal.Binding.StringFormat = "00000";
                        colSenhaCanceladasPrioridade.Binding.StringFormat = "00000";
                        colSenhaCanceladasNormal.Binding.StringFormat = "00000";
                        colSenhaConsultaPrioridade.Binding.StringFormat = "00000";
                        colSenhaConsultaNormal.Binding.StringFormat = "00000";
                        colSenhaConsulta.Binding.StringFormat = "00000";
                        break;
                    default:

                        break;
                }

            }

            lblGeralAtendimento.Content = _login.parametros.Nome_Botao_1;
            lblGeralCancelada.Content = _login.parametros.Nome_Botao_1;
            lblGeralChamada.Content = _login.parametros.Nome_Botao_1;
            lblGeralFila.Content = _login.parametros.Nome_Botao_1;
            lblGeralFinalizada.Content = _login.parametros.Nome_Botao_1;

            lblPreferincialAtendimento.Content = _login.parametros.Nome_Botao_2;
            lblPreferincialCancelada.Content = _login.parametros.Nome_Botao_2;
            lblPreferencialChamada.Content = _login.parametros.Nome_Botao_2;
            lblPreferincialFila.Content = _login.parametros.Nome_Botao_2;
            lblPreferincialFinalizada.Content = _login.parametros.Nome_Botao_2;


            CarregarGrids();

            VerificaLicenca();

        }


        private void CarregarGrids()
        {

            dtData.SelectedDate = DateTime.Now.Date;

            if (_login.parametros.ModoRetiradaSenhaManual == false)
                senhas = _AppServicoSenha.ObterTodosPorSetorData(meuPc.SetorId, DateTime.Now.Date);
            else
            {
                int ultimaSequencia = _AppServicoSenha.ObterUltimaSequenciaManual();
                senhas = _AppServicoSenha.ObterTodosPorSetorSequencia(5, ultimaSequencia);
            }

            lblPessoasFila.Content = string.Format("Qtd. Em Espera: {0}", senhas.Where(p => p.Status == "EM ESPERA").Count());

            ultimaSenha = senhas.Where(p => p.Status != "EM ESPERA" && p.Usuario_Id == _usuario.Id_Usuario).OrderBy(p => p.Sequencia_Chamada).LastOrDefault();

            dataGridPrioridade.ItemsSource = senhas.Where(p => (p.SenhaTipo == 2 || p.SenhaTipo == 3) && (p.Status == "EM ESPERA"));
            dataGridNormal.ItemsSource = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "EM ESPERA");

            dataGridPrioridadeFinalizada.ItemsSource = senhas.Where(p => (p.SenhaTipo == 2 || p.SenhaTipo == 3) && (p.Status == "FINALIZADA"));
            dataGridNormalFinalizada.ItemsSource = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "FINALIZADA");

            dataGridPrioridadeChamada.ItemsSource = senhas.Where(p => (p.SenhaTipo == 2 || p.SenhaTipo == 3) && (p.Status == "CHAMADA"));
            dataGridNormalChamada.ItemsSource = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "CHAMADA");

            dataGridPrioridadeCanceladas.ItemsSource = senhas.Where(p => (p.SenhaTipo == 2 || p.SenhaTipo == 3) && (p.Status == "CANCELADA"));
            dataGridNormalCanceladas.ItemsSource = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "CANCELADA");

            dataGridPrioridadeRestrito.ItemsSource = senhas.Where(p => p.SenhaTipo == 2 || p.SenhaTipo == 3);
            dataGridNormalRestrito.ItemsSource = senhas.Where(p => p.SenhaTipo == 1);

            switch (_login.parametros.Qtd_Caracteres_Senha)
            {
                case 0:

                    if (ultimaSenha != null)
                        lblUltSenha.Content = string.Format("{0}{1}{2:000}", ultimaSenha.Tipo, ultimaSenha.LetraSetor, ultimaSenha.Numero_Senha);
                    else
                        lblUltSenha.Content = "";

                    break;
                case 1:
                    if (ultimaSenha != null)
                        lblUltSenha.Content = string.Format("{0}{1}{2:0000}", ultimaSenha.Tipo, ultimaSenha.LetraSetor, ultimaSenha.Numero_Senha);
                    else
                        lblUltSenha.Content = "";


                    break;
                case 2:
                    if (ultimaSenha != null)
                        lblUltSenha.Content = string.Format("{0}{1}{2:00000}", ultimaSenha.Tipo, ultimaSenha.LetraSetor, ultimaSenha.Numero_Senha);
                    else
                        lblUltSenha.Content = "";

                    break;
                default:

                    break;
            }


        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (MessageBox.Show("Deseja realmente sair do sistema?", "Sair", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    Application.Current.Shutdown();
            }

            if (e.Key == Key.F5)
                CarregarGrids();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }


        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente sair?", "Sair", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Close();
        }

        public Senha AtualizarStatusSenha(Senha senha, string status)
        {
            senha.Status = status;
            senha.Caracter_Atendimento = meuPc.Caracter;
            senha.Identificador_Pc = meuPc.Identificador_Pc;
            senha.Usuario_Id = _usuario.Id_Usuario;
            senha.Nome_Usuario = _usuario.NomeUsu;
            senha.FalaOutros = meuPc.FalaOutros;


            if (_login.parametros.ModoRetiradaSenhaManual == false)
                switch (senha.Status)
                {
                    case "CHAMADA":
                        senha.Hora_Chamada = DateTime.Now.ToLongTimeString();
                        senha.Sequencia_Chamada = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date).Max(p => p.Sequencia_Chamada) + 1;
                        senha.DescricaoLocalAtendimento = meuPc.Tipo_Atendimento;
                        break;
                    case "CANCELADA":
                        senha.Hora_Cancelado = DateTime.Now.ToLongTimeString();
                        break;
                    case "FINALIZADA":
                        senha.Hora_Finalizado = DateTime.Now.ToLongTimeString();
                        break;
                    default:
                        senha.Hora_Retirada = DateTime.Now.ToLongTimeString();
                        break;
                }
            else
                switch (senha.Status)
                {
                    case "CHAMADA":
                        senha.Hora_Chamada = DateTime.Now.ToLongTimeString();
                        senha.Data = DateTime.Now.Date;
                        senha.Sequencia_Chamada = _AppServicoSenha.ObterTodosPorSetorSequencia(5, senha.NumeroSequencia).Max(p => p.Sequencia_Chamada) + 1;
                        senha.DescricaoLocalAtendimento = meuPc.Tipo_Atendimento;
                        break;
                    case "CANCELADA":
                        senha.Hora_Cancelado = DateTime.Now.ToLongTimeString();
                        break;
                    case "FINALIZADA":
                        senha.Hora_Finalizado = DateTime.Now.ToLongTimeString();
                        break;
                    default:
                        senha.Hora_Retirada = DateTime.Now.ToLongTimeString();
                        break;
                }

            _AppServicoSenha.Update(senha);
            return senha;
        }


        private void btnChamarSenha_Click(object sender, RoutedEventArgs e)
        {
            tabItemFila.IsSelected = true;
            int tempo1 = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));
            acao = "";
            for (int i = 0; i < tempo1; i++)
            {

            }
            ChamarSenha();
        }

        public void ChamarSenha()
        {

            CarregarGrids();

            if (relacoesPcPainal.Count == 0)
            {
                MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            if (meuPc.TipoChamadaSenha == 0)
            {


                if (senhas.Where(p => p.Status == "EM ESPERA").Count() > 0)
                {

                    if (senhas.Where(p => p.SenhaTipo == 1 && p.Status == "EM ESPERA").Count() > 0)
                    {
                        chamarSenha = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "EM ESPERA").OrderByDescending(p => p.Sequencia_Chamada).FirstOrDefault();
                        chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");


                        if (chamarSenha != null && chamarSenha.Senha_Id > 0)
                        {

                            int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(5));

                            for (int i = 0; i < tempo; i++)
                            {

                            }

                            Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                            if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                            {
                                ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                                chamadaSenha.Owner = this;
                                chamadaSenha.ShowDialog();
                                CarregarGrids();
                            }
                            else
                            {
                                MessageBox.Show("Tentativa de chamada da mesma senha por usuários diferentes. Favor chame outra senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Não existem senhas normais a serem chamadas no momento. Favor aguarde a retirada de senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                }
                else
                {
                    MessageBox.Show("Não existem senhas a serem chamadas no momento. Favor aguarde a retirada de senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }


            if (meuPc.TipoChamadaSenha == 1)
            {
                if (senhas.Where(p => p.Status == "EM ESPERA").Count() > 0)
                {

                    if (senhas.Where(p => p.SenhaTipo == 3 && p.Status == "EM ESPERA").Count() > 0)
                    {
                        chamarSenha = senhas.Where(p => p.SenhaTipo == 3 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                        chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                        int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(5));

                        for (int i = 0; i < tempo; i++)
                        {

                        }

                        Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                        if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                        {
                            ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                            chamadaSenha.Owner = this;
                            chamadaSenha.ShowDialog();
                            CarregarGrids();
                        }
                        else
                        {
                            MessageBox.Show("Tentativa de chamada da mesma senha por usuários diferentes. Favor chame outra senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        chamarSenha = new Senha();

                        if (senhas.Where(p => p.SenhaTipo == 2 && p.Status == "EM ESPERA").Count() > 0)
                        {
                            chamarSenha = senhas.Where(p => p.SenhaTipo == 2 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                            chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");


                            int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(5));

                            for (int i = 0; i < tempo; i++)
                            {

                            }

                            Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                            if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                            {
                                ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                                chamadaSenha.Owner = this;
                                chamadaSenha.ShowDialog();
                                CarregarGrids();
                            }
                            else
                            {
                                MessageBox.Show("Tentativa de chamada da mesma senha por usuários diferentes. Favor chame outra senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Não existem senhas de prioridade a serem chamadas no momento. Favor aguarde a retirada de senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return;
                        }
                    }


                }
                else
                {
                    MessageBox.Show("Não existem senhas a serem chamadas no momento. Favor aguarde a retirada de senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }


            if (meuPc.TipoChamadaSenha == 2)
            {
                if (senhas.Where(p => p.Status == "EM ESPERA").Count() > 0)
                {

                    if (senhas.Where(p => p.SenhaTipo == 3 && p.Status == "EM ESPERA").Count() > 0)
                    {
                        chamarSenha = senhas.Where(p => p.SenhaTipo == 3 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                        chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                        int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(5));

                        for (int i = 0; i < tempo; i++)
                        {

                        }

                        Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                        if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                        {
                            ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                            chamadaSenha.Owner = this;
                            chamadaSenha.ShowDialog();
                            CarregarGrids();
                        }
                        else
                        {
                            MessageBox.Show("Tentativa de chamada da mesma senha por usuários diferentes. Favor chame outra senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        chamarSenha = new Senha();

                        Senha ultimaSenha = new Senha();

                        ultimaSenha = senhas.Where(p => p.Status != "EM ESPERA" && p.SenhaTipo != 3 && p.SenhaTipo != 4).OrderBy(p => p.Sequencia_Chamada).LastOrDefault();

                        if (ultimaSenha == null)
                        {
                            chamarSenha = senhas.Where(p => p.Status == "EM ESPERA").OrderByDescending(p => p.Sequencia_Chamada).FirstOrDefault();
                            chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");
                        }
                        else
                        {
                            if (ultimaSenha.SenhaTipo == 1)
                            {
                                if (senhas.Where(p => p.SenhaTipo == 2 && p.Status == "EM ESPERA").Count() > 0)
                                {
                                    chamarSenha = senhas.Where(p => p.SenhaTipo == 2 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                                    chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");
                                }
                                else
                                {
                                    chamarSenha = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                                    chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");
                                }
                            }
                            else
                            {
                                if (senhas.Where(p => p.SenhaTipo == 1 && p.Status == "EM ESPERA").Count() > 0)
                                {
                                    chamarSenha = senhas.Where(p => p.SenhaTipo == 1 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                                    chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");
                                }
                                else
                                {
                                    chamarSenha = senhas.Where(p => p.SenhaTipo == 2 && p.Status == "EM ESPERA").OrderBy(p => p.Sequencia_Chamada).FirstOrDefault();
                                    chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");
                                }
                            }

                        }
                        if (chamarSenha != null && chamarSenha.Senha_Id > 0)
                        {

                            int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(5));

                            for (int i = 0; i < tempo; i++)
                            {

                            }

                            Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                            if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                            {
                                ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                                chamadaSenha.Owner = this;
                                chamadaSenha.ShowDialog();
                                CarregarGrids();
                            }
                            else
                            {
                                MessageBox.Show("Tentativa de chamada da mesma senha por usuários diferentes. Favor chame outra senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Não existem senhas a serem chamadas no momento. Favor aguarde a retirada de senha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }





        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            tabItemFila.IsSelected = true;
            CarregarGrids();

        }

        private void lblTitulo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MenuItemChamarSenhaPrioridadeEmEspera_Click(object sender, RoutedEventArgs e)
        {



            Senha SenhaSelecinada = (Senha)dataGridPrioridade.SelectedItem;

            if (SenhaSelecinada != null && SenhaSelecinada.Senha_Id > 0)
            {
                chamarSenha = SenhaSelecinada;
                chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));

                if (relacoesPcPainal.Count == 0)
                {
                    MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                for (int i = 0; i < tempo; i++)
                {

                }

                Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                {
                    ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                    chamadaSenha.Owner = this;
                    chamadaSenha.ShowDialog();

                }

            }
            CarregarGrids();
        }

        private void MenuItemChamarSenhaNormalEmEspera_Click(object sender, RoutedEventArgs e)
        {


            Senha SenhaSelecinada = (Senha)dataGridNormal.SelectedItem;

            if (SenhaSelecinada != null && SenhaSelecinada.Senha_Id > 0)
            {
                chamarSenha = SenhaSelecinada;
                chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));

                if (relacoesPcPainal.Count == 0)
                {
                    MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }


                for (int i = 0; i < tempo; i++)
                {

                }

                Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                {
                    ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                    chamadaSenha.Owner = this;
                    chamadaSenha.ShowDialog();

                }

            }
            CarregarGrids();
        }

        private void MenuItemChamarSenhaNormalCancelada_Click(object sender, RoutedEventArgs e)
        {

            Senha SenhaSelecinada = (Senha)dataGridNormalCanceladas.SelectedItem;

            if (SenhaSelecinada != null && SenhaSelecinada.Senha_Id > 0)
            {
                chamarSenha = SenhaSelecinada;
                chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));

                if (relacoesPcPainal.Count == 0)
                {
                    MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                for (int i = 0; i < tempo; i++)
                {

                }

                Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                {
                    ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                    chamadaSenha.Owner = this;
                    chamadaSenha.ShowDialog();

                }

            }
            CarregarGrids();
        }

        private void MenuItemChamarSenhaPrioridadeCancelada_Click(object sender, RoutedEventArgs e)
        {

            Senha SenhaSelecinada = (Senha)dataGridPrioridadeCanceladas.SelectedItem;

            if (SenhaSelecinada != null && SenhaSelecinada.Senha_Id > 0)
            {
                chamarSenha = SenhaSelecinada;
                chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));

                if (relacoesPcPainal.Count == 0)
                {
                    MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                for (int i = 0; i < tempo; i++)
                {

                }

                Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                {
                    ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                    chamadaSenha.Owner = this;
                    chamadaSenha.ShowDialog();

                }

            }
            CarregarGrids();
        }



        private void MenuItemChamarSenhaNormalChamada_Click(object sender, RoutedEventArgs e)
        {

            Senha SenhaSelecinada = (Senha)dataGridNormalChamada.SelectedItem;

            if (SenhaSelecinada != null && SenhaSelecinada.Senha_Id > 0)
            {
                chamarSenha = SenhaSelecinada;
                chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));

                if (relacoesPcPainal.Count == 0)
                {
                    MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }


                for (int i = 0; i < tempo; i++)
                {

                }

                Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                {
                    ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                    chamadaSenha.Owner = this;
                    chamadaSenha.ShowDialog();

                }

            }
            CarregarGrids();
        }

        private void MenuItemChamarSenhaPrioridadeChamada_Click(object sender, RoutedEventArgs e)
        {

            Senha SenhaSelecinada = (Senha)dataGridPrioridadeChamada.SelectedItem;

            if (SenhaSelecinada != null && SenhaSelecinada.Senha_Id > 0)
            {
                chamarSenha = SenhaSelecinada;
                chamarSenha = AtualizarStatusSenha(chamarSenha, "CHAMADA");

                int tempo = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(6));

                if (relacoesPcPainal.Count == 0)
                {
                    MessageBox.Show("Não existe painel configurado para essa máquina. Favor, relacione essa máquina à um painel.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                for (int i = 0; i < tempo; i++)
                {

                }

                Senha senha = _AppServicoSenha.ObterPorId(chamarSenha.Senha_Id);

                if (senha.Identificador_Pc == chamarSenha.Identificador_Pc)
                {
                    ConfirmarChamadaSenha chamadaSenha = new ConfirmarChamadaSenha(chamarSenha, estacoes, paineis, relacoesPcPainal, meuPc, _usuario, this, _principal, _login);
                    chamadaSenha.Owner = this;
                    chamadaSenha.ShowDialog();

                }

            }
            CarregarGrids();
        }

        private void cmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipo.Focus())
            {
                if (dtData.SelectedDate != null)
                {
                    ConsultaDetalhada(dtData.SelectedDate.Value);

                    if (cmbTipo.SelectedIndex > -1)
                    {
                        txtLetraSenha.IsEnabled = true;
                        txtLetraSenha.Text = "";
                        txtNumeroSenha.IsEnabled = true;
                        txtNumeroSenha.Text = "";
                        txtLetraSenha.Focus();

                        switch (cmbTipo.SelectedIndex)
                        {

                            case 0:
                                dataGridConsulta.ItemsSource = senhasConsulta.Where(p => p.Status == "FINALIZADA").ToList();
                                break;
                            case 1:
                                dataGridConsulta.ItemsSource = senhasConsulta.Where(p => p.Status == "CANCELADA").ToList();
                                break;
                            case 2:
                                dataGridConsulta.ItemsSource = senhasConsulta.Where(p => p.Status == "CHAMADA").ToList();
                                break;
                            default:
                                break;
                        }


                        dataGridConsulta.Items.Refresh();
                        if (dataGridConsulta.Items.Count > 0)
                            dataGridConsulta.SelectedIndex = 0;
                    }
                    else
                    {
                        txtLetraSenha.IsEnabled = false;
                        txtLetraSenha.Text = "";
                        txtNumeroSenha.IsEnabled = false;
                        txtNumeroSenha.Text = "";

                        dataGridConsulta.ItemsSource = null;
                        dataGridConsulta.Items.Refresh();
                    }


                }
            }
        }

        private void dtData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTipo.SelectedIndex = -1;
            txtLetraSenha.Text = "";
            txtNumeroSenha.Text = "";


            if (dtData.SelectedDate == null)
                dtData.SelectedDate = DateTime.Now.Date;


            ConsultaDetalhada(dtData.SelectedDate.Value);
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConsultaDetalhada(DateTime data)
        {
            senhasConsulta = _AppServicoSenha.ObterTodosPorSetorData(meuPc.SetorId, data);

            dataGridConsulta.ItemsSource = senhasConsulta;

            

        }

        private void btnAtendimentoRapido_Click(object sender, RoutedEventArgs e)
        {
            chamarSenha = new Senha();

            acao = "";

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


                    try
                    {

                        int senha = 0;
                        string tipoSenha = "";
                        int senhatipo = 5;
                        string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                        string status = "ATENDENDO";

                        int numSequecia = 0;

                        if (_login.parametros.Tipo_Senha == 0)
                            senha = _AppServicoSenha.OberProximaSenha(_login.parametros.ZerarSenhaDiaSeguinte, meuPc.SetorId, senhatipo, out numSequecia, _login.parametros.Qtd_Caracteres_Senha);
                        else
                        {
                            do
                            {
                                senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(_login.parametros.Qtd_Caracteres_Senha + 3));
                            } while (senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);

                        }


                        chamarSenha.Data = DateTime.Now.Date;


                        if (_login.parametros.Utilizar_Aleatorio == true)
                        {
                            do
                            {
                                chamarSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(_login.parametros.Qtd_Caracteres_Senha + 3);
                            }
                            while (chamarSenha.Aleatorio_Confirmacao == senha.ToString() || chamarSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                        }

                        chamarSenha.Hora_Retirada = HoraRetiradaSenha;
                        chamarSenha.Numero_Senha = senha;
                        chamarSenha.Tipo = tipoSenha;
                        chamarSenha.SenhaTipo = senhatipo;
                        chamarSenha.Status = status;
                        chamarSenha.SetorId = meuPc.SetorId;



                        if (_login.parametros.ZerarSenhaDiaSeguinte == true)
                            chamarSenha.ModoSequencial = false;
                        else
                            chamarSenha.ModoSequencial = true;

                        chamarSenha.QtdCaracteres = _login.parametros.Qtd_Caracteres_Senha;

                        chamarSenha.NumeroSequencia = numSequecia;



                        switch (meuPc.SetorId)
                        {
                            case -1:

                                switch (chamarSenha.SenhaTipo)
                                {
                                    case 1:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = _login.parametros.Voz_Botao_1;
                                        break;

                                    case 2:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = _login.parametros.Voz_Botao_2;
                                        break;

                                    case 3:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = _login.parametros.Voz_Botao_3;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 0:
                                chamarSenha.LetraSetor = _login.parametros.Letra_Setor_1;
                                chamarSenha.NomeSetor = _login.parametros.Nome_Setor_1;
                                chamarSenha.Voz = _login.parametros.Voz_Setor_1;
                                break;

                            case 1:
                                chamarSenha.LetraSetor = _login.parametros.Letra_Setor_2;
                                chamarSenha.NomeSetor = _login.parametros.Nome_Setor_2;
                                chamarSenha.Voz = _login.parametros.Voz_Setor_2;
                                break;

                            case 2:
                                chamarSenha.LetraSetor = _login.parametros.Letra_Setor_3;
                                chamarSenha.NomeSetor = _login.parametros.Nome_Setor_3;
                                chamarSenha.Voz = _login.parametros.Voz_Setor_3;
                                break;

                            case 3:
                                chamarSenha.LetraSetor = _login.parametros.Letra_Setor_4;
                                chamarSenha.NomeSetor = _login.parametros.Nome_Setor_4;
                                chamarSenha.Voz = _login.parametros.Voz_Setor_4;
                                break;

                            default:
                                break;
                        }

                        _AppServicoSenha.Adicionar(chamarSenha);

                        CarregarGrids();

                        WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                        balcao.Owner = this;
                        balcao.ShowDialog();

                        if (chamarSenha.SenhaTipo == 5)
                        {
                            if (acao == "finalizar")
                                AtualizarStatusSenha(chamarSenha, "FINALIZADA");
                            else
                                AtualizarStatusSenha(chamarSenha, "CANCELADA");

                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possível obter a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
            }
            else
            {
                try
                {

                    int senha = 0;
                    string tipoSenha = "";
                    int senhatipo = 5;
                    string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                    string status = "ATENDENDO";

                    int numSequecia = 0;

                    if (_login.parametros.Tipo_Senha == 0)
                        senha = _AppServicoSenha.OberProximaSenha(_login.parametros.ZerarSenhaDiaSeguinte, meuPc.SetorId, senhatipo, out numSequecia, _login.parametros.Qtd_Caracteres_Senha);
                    else
                    {
                        do
                        {
                            senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(_login.parametros.Qtd_Caracteres_Senha + 3));
                        } while (senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);

                    }



                    chamarSenha.Data = DateTime.Now.Date;


                    if (_login.parametros.Utilizar_Aleatorio == true)
                    {
                        do
                        {
                            chamarSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(_login.parametros.Qtd_Caracteres_Senha + 3);
                        }
                        while (chamarSenha.Aleatorio_Confirmacao == senha.ToString() || chamarSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                    }

                    chamarSenha.Hora_Retirada = HoraRetiradaSenha;
                    chamarSenha.Numero_Senha = senha;
                    chamarSenha.Tipo = tipoSenha;
                    chamarSenha.SenhaTipo = senhatipo;
                    chamarSenha.Status = status;
                    chamarSenha.SetorId = meuPc.SetorId;



                    if (_login.parametros.ZerarSenhaDiaSeguinte == true)
                        chamarSenha.ModoSequencial = false;
                    else
                        chamarSenha.ModoSequencial = true;

                    chamarSenha.QtdCaracteres = _login.parametros.Qtd_Caracteres_Senha;

                    chamarSenha.NumeroSequencia = numSequecia;



                    switch (meuPc.SetorId)
                    {
                        case -1:

                            switch (chamarSenha.SenhaTipo)
                            {
                                case 1:
                                    chamarSenha.LetraSetor = "";
                                    chamarSenha.Voz = _login.parametros.Voz_Botao_1;
                                    break;

                                case 2:
                                    chamarSenha.LetraSetor = "";
                                    chamarSenha.Voz = _login.parametros.Voz_Botao_2;
                                    break;

                                case 3:
                                    chamarSenha.LetraSetor = "";
                                    chamarSenha.Voz = _login.parametros.Voz_Botao_3;
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case 0:
                            chamarSenha.LetraSetor = _login.parametros.Letra_Setor_1;
                            chamarSenha.NomeSetor = _login.parametros.Nome_Setor_1;
                            chamarSenha.Voz = _login.parametros.Voz_Setor_1;
                            break;

                        case 1:
                            chamarSenha.LetraSetor = _login.parametros.Letra_Setor_2;
                            chamarSenha.NomeSetor = _login.parametros.Nome_Setor_2;
                            chamarSenha.Voz = _login.parametros.Voz_Setor_2;
                            break;

                        case 2:
                            chamarSenha.LetraSetor = _login.parametros.Letra_Setor_3;
                            chamarSenha.NomeSetor = _login.parametros.Nome_Setor_3;
                            chamarSenha.Voz = _login.parametros.Voz_Setor_3;
                            break;

                        case 3:
                            chamarSenha.LetraSetor = _login.parametros.Letra_Setor_4;
                            chamarSenha.NomeSetor = _login.parametros.Nome_Setor_4;
                            chamarSenha.Voz = _login.parametros.Voz_Setor_4;
                            break;

                        default:
                            break;
                    }

                    _AppServicoSenha.Adicionar(chamarSenha);

                    CarregarGrids();

                    WinBalcaoNovo balcao = new WinBalcaoNovo(this, _principal);
                    balcao.Owner = this;
                    balcao.ShowDialog();

                    if (chamarSenha.SenhaTipo == 5)
                    {
                        if (acao == "finalizar")
                            AtualizarStatusSenha(chamarSenha, "FINALIZADA");
                        else
                            AtualizarStatusSenha(chamarSenha, "CANCELADA");

                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível obter a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }



        private void MenuItemMudarStatusPrioridade_Click(object sender, RoutedEventArgs e)
        {

            if (_usuario.Alterar_Status_Senha == true)
                if (dataGridPrioridadeRestrito.SelectedItem != null)
                {
                    Senha senhaAtualizar = _AppServicoSenha.ObterPorId(((Senha)dataGridPrioridadeRestrito.SelectedItem).Senha_Id);
                    senhaAtualizar.Hora_Atendimento = "";
                    senhaAtualizar.Hora_Cancelado = "";
                    senhaAtualizar.Hora_Chamada = "";
                    senhaAtualizar.Hora_Finalizado = "";
                    senhaAtualizar.Identificador_Pc = "";
                    senhaAtualizar.DescricaoLocalAtendimento = "";
                    senhaAtualizar.Caracter_Atendimento = "";
                    senhaAtualizar.Nome_Usuario = "";
                    senhaAtualizar.Usuario_Id = 0;
                    AtualizarStatusSenha(senhaAtualizar, "EM ESPERA");

                    _AppServicoSenha.Update(senhaAtualizar);


                    foreach (var item in estacoes)
                    {
                        if (item.SetorId == meuPc.SetorId)
                            InicializaConexaoAtualizarGridsEstacoes(item);
                    }
                }
        }

        private void MenuItemMudarStatusNormal_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Alterar_Status_Senha == true)
                if (dataGridNormalRestrito.SelectedItem != null)
                {
                    Senha senhaAtualizar = _AppServicoSenha.ObterPorId(((Senha)dataGridNormalRestrito.SelectedItem).Senha_Id);
                    senhaAtualizar.Hora_Atendimento = "";
                    senhaAtualizar.Hora_Cancelado = "";
                    senhaAtualizar.Hora_Chamada = "";
                    senhaAtualizar.Hora_Finalizado = "";
                    senhaAtualizar.Identificador_Pc = "";
                    senhaAtualizar.DescricaoLocalAtendimento = "";
                    senhaAtualizar.Caracter_Atendimento = "";
                    senhaAtualizar.Nome_Usuario = "";
                    senhaAtualizar.Usuario_Id = 0;
                    AtualizarStatusSenha(senhaAtualizar, "EM ESPERA");

                    _AppServicoSenha.Update(senhaAtualizar);


                    foreach (var item in estacoes)
                    {
                        if (item.SetorId == meuPc.SetorId)
                            InicializaConexaoAtualizarGridsEstacoes(item);
                    }
                }
        }

        private void txtLetraSenha_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLetraSenha.Focus())
            {
                dataGridConsulta.ItemsSource = senhasConsulta.Where(p => p.Tipo == txtLetraSenha.Text).ToList();
                dataGridConsulta.Items.Refresh();
                if (dataGridConsulta.Items.Count > 0)
                    dataGridConsulta.SelectedIndex = 0;
            }
        }

        private void txtNumeroSenha_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dtData.SelectedDate != null && cmbTipo.SelectedIndex > -1)
            {
                ConsultaDetalhada(dtData.SelectedDate.Value);

                if (txtNumeroSenha.Focus())
                {
                    if (dtData.SelectedDate != null && cmbTipo.SelectedIndex > -1)
                    {
                        ConsultaDetalhada(dtData.SelectedDate.Value);

                        if (txtNumeroSenha.Text.Trim() != "")
                        {
                            dataGridConsulta.ItemsSource = senhasConsulta.Where(p => p.Numero_Senha == Convert.ToInt32(txtNumeroSenha.Text)).ToList();
                            dataGridConsulta.Items.Refresh();
                            if (dataGridConsulta.Items.Count > 0)
                                dataGridConsulta.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void btnConsultaRelatorio_Click(object sender, RoutedEventArgs e)
        {

            if (datePickerInicioConsulta.SelectedDate == null)
                return;


            if (datePickerInicioConsulta.SelectedDate != null && datePickerFimConsulta.SelectedDate != null)
                ConsultarRelatorios();
            ObterResultados();

            var funcionario = (Usuario)cmbLogin.SelectedItem;
            dataGridConsultaRelatorios.ItemsSource = senhasConsultaRelatorio.Where(p => p.Usuario_Id == funcionario.Id_Usuario);
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

        private void ConsultarRelatorios()
        {
            senhasConsultaRelatorio = classAtendimento.ObterAtendimentosFinalizadosPorPeriodo(datePickerInicioConsulta.SelectedDate.Value, datePickerFimConsulta.SelectedDate.Value);
            qtdDias = CountDiasUteis(datePickerInicioConsulta.SelectedDate.Value, datePickerFimConsulta.SelectedDate.Value);

            var listaFuncionarios = senhasConsultaRelatorio.Select(p => p.Usuario_Id).Distinct().ToList();

            List<Usuario> usu = new List<Usuario>();


            int m = 0;
            string nomeMelhor = "";

            for (int i = 0; i < listaFuncionarios.Count; i++)
            {
                var nome = Usuarios.Where(p => p.Id_Usuario == listaFuncionarios[i]).FirstOrDefault();

                usu.Add(nome);
                if (senhasConsultaRelatorio.Where(p => p.Usuario_Id == nome.Id_Usuario).Count() > m)
                {
                    m = senhasConsultaRelatorio.Where(p => p.Usuario_Id == nome.Id_Usuario).Count();
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

                var quantAtendimento = senhasConsultaRelatorio.Where(p => p.Usuario_Id == nome.Id_Usuario).Count();

                lblTotalDiasUteis.Content = string.Format("Total de dias úteis no Período = {0}", qtdDias);
                lblTotalAtendimento.Content = string.Format("Total de atendimentos no Período = {0}", quantAtendimento);
                lblMediaAtendimento.Content = string.Format("Média de atendimentos por dia = {0}", quantAtendimento / qtdDias);


                dataGridConsultaRelatorios.ItemsSource = senhasConsultaRelatorio.Where(p => p.Usuario_Id == nome.Id_Usuario);
            }
            catch (Exception)
            {
                //MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
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

       

        private void dataGridConsultaRelatorios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridConsultaRelatorios.SelectedItem != null)
            {
                dataGridReciboBalcao_Copy.ItemsSource = classAtendimento.ObterReciboPorIdAtendimento(((Senha)dataGridConsultaRelatorios.SelectedItem).Senha_Id);
                dataGridReciboBalcao_Copy.SelectedIndex = 0;
            }
            else
                dataGridReciboBalcao_Copy.ItemsSource = null;
        }

        private void cmbLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbLogin.Focus())
            ObterResultados();
        }

    }
}