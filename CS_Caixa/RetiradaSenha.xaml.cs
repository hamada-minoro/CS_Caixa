
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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Speech.Synthesis;
using System.Net.NetworkInformation;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using CS_Caixa.Repositorios;
using CS_Caixa.Models;
using CS_Caixa.Controls;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para RetiradaSenha.xaml
    /// </summary>
    public partial class RetiradaSenha : Window
    {

        //private StreamWriter stwEnviador;
        //private TcpClient tcpServidor;
        // Necessário para atualizar o formulário com mensagens da outra thread
        private delegate void AtualizaLogCallBack(string strMensagem);
        // Necessário para definir o formulário para o estado "disconnected" de outra thread
        private delegate void FechaConexaoCallBack(string strMotivo);

        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();

        public List<VoiceInfo> listaVozes = new List<VoiceInfo>();
        public List<Senha> senhasEnviarEstacoes = new List<Senha>();
        public List<Cadastro_Pc> maquinasEstacao = new List<Cadastro_Pc>();
        public Parametro parametros = new Parametro();
        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        BackgroundWorker worker;
        public int setorSelecionado = -1;
        public List<Senha> senhas = new List<Senha>();

        ClassServidor mainServidor;
        Cadastro_Pc meuPc = new Cadastro_Pc();
        string _idMaquina;
        //bool atualizarParametros = false;
        List<Cadastro_Pc> estacoes = new List<Cadastro_Pc>();
        List<Cadastro_Painel> paineis = new List<Cadastro_Painel>();
        private IPAddress enderecoIPAtualizarGridsEstacoes;
        private StreamWriter stwEnviadorAtualizarGridsEstacoes;
        private TcpClient tcpServidorAtualizarGridsEstacoes;
        Ping pingSender;


        public List<string> envioEstacoes = new List<string>();

        public bool enviar = true;

        public RetiradaSenha(string idMaquina)
        {
            _idMaquina = idMaquina;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.None;
                CarregamentoInicial();
                ImpressaoDeTeste();
                // Analisa o endereço IP do servidor informado no textbox
                IPAddress enderecoIP = IPAddress.Parse(meuPc.Ip_Pc);

                // Cria uma nova instância do objeto ChatServidor
                mainServidor = new ClassServidor(enderecoIP, meuPc.Porta_Pc);

                // Vincula o tratamento de evento StatusChanged a mainServer_StatusChanged
                ClassServidor.StatusChanged += new StatusChangedEventHandler(mainServidor_StatusChanged);

                // Inicia o atendimento das conexões
                mainServidor.IniciaAtendimento();

                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();


                if (parametros.Voz_RetiradaSenha == true)
                    ClassFalarTexto.FalarTexto("Sistema iniciado.", listaVozes, parametros.Voz_Botao_1);


            }
            catch (Exception) { }
        }

        public void mainServidor_StatusChanged(object sender, ClassStatusChangedEventArgs e)
        {

            //if (e.EventMessage == "Parametros")
            //    atualizarParametros = true;
        }


        public void InicializaConexao()
        {

            foreach (var item in paineis)
            {
                pingSender = new Ping();

                // Create a buffer of 32 bytes of data to be transmitted.

                PingReply reply = pingSender.Send(item.Ip_Pc);

                if (reply.Status == IPStatus.Success)
                {
                    try
                    {

                        enderecoIPAtualizarGridsEstacoes = IPAddress.Parse(item.Ip_Pc);

                        tcpServidorAtualizarGridsEstacoes = new TcpClient();
                        tcpServidorAtualizarGridsEstacoes.Connect(enderecoIPAtualizarGridsEstacoes, item.Porta_Pc);

                        if (tcpServidorAtualizarGridsEstacoes.Connected == true)
                        {
                            stwEnviadorAtualizarGridsEstacoes = new StreamWriter(tcpServidorAtualizarGridsEstacoes.GetStream());
                            stwEnviadorAtualizarGridsEstacoes.WriteLine("Atualizar DataGrids");
                            stwEnviadorAtualizarGridsEstacoes.Flush();
                        }


                    }
                    catch (Exception) { }
                }
            }


            foreach (var item in estacoes)
            {
                if (item.Tipo_Atendimento == "GUICHÊ")
                {
                    pingSender = new Ping();

                    // Create a buffer of 32 bytes of data to be transmitted.

                    PingReply reply = pingSender.Send(item.Ip_Pc);

                    if (reply.Status == IPStatus.Success)
                    {
                        try
                        {
                            tcpServidorAtualizarGridsEstacoes = new TcpClient();
                            tcpServidorAtualizarGridsEstacoes.Connect(item.Ip_Pc, item.Porta_Pc);


                            if (tcpServidorAtualizarGridsEstacoes.Connected == true)
                            {

                                stwEnviadorAtualizarGridsEstacoes = new StreamWriter(tcpServidorAtualizarGridsEstacoes.GetStream());
                                stwEnviadorAtualizarGridsEstacoes.WriteLine("Atualizar DataGrids");
                                stwEnviadorAtualizarGridsEstacoes.Flush();


                                stwEnviadorAtualizarGridsEstacoes.Close();
                                tcpServidorAtualizarGridsEstacoes.Close();
                            }

                        }
                        catch (Exception) { }
                    }
                }

            }



        }

        private void ImpressaoDeTeste()
        {
            FormImprimirSenha aguarde = new FormImprimirSenha("Teste");
            aguarde.ShowDialog();
        }

        private void CarregamentoInicial()
        {
            try
            {
                parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();
                HabilitarBotoes();
                lblSaudacao.Content = parametros.Saudacao.Trim();
                lblEmpresa.Content = parametros.Nome_Empresa.Trim();
                listaVozes = ClassFalarTexto.CarregaComboVozes();
                senhas = _AppServicoSenha.ObterTodosPorData(DateTime.Now.Date);
                estacoes = _AppServicoCadastro_Pc.ObterTodos().ToList();
                paineis = _AppServicoCadastro_Painel.ObterTodos().ToList();
                meuPc = estacoes.Where(p => p.Identificador_Pc == _idMaquina).FirstOrDefault();
                lblHora.Content = "";

                

            }
            catch (Exception)
            {

            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (parametros.Mostrar_Hora == true)
                {
                    lblHora.Content = DateTime.Now.ToString();
                    CommandManager.InvalidateRequerySuggested();
                }

                if (envioEstacoes.Count > 0 && enviar == true)
                {
                    enviar = false;
                    envioEstacoes.RemoveAt(0);
                    EnviarEstacoes();

                }


                if (parametros.InicioFimExpediente == true)
                {
                    if (parametros.DesligarSenha == true)
                    {
                        if (parametros.HoraDesligarSenha == DateTime.Now.ToLongTimeString())
                        {
                            Process.Start("Shutdown", "-s -f -t 00");
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
        }


        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            InicializaConexao();
            enviar = true;

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void HabilitarBotoes()
        {
            try
            {
                if (parametros.Habilitado_Botao_1 == true)
                {
                    btnGeral.Visibility = Visibility.Visible;
                    btnGeral.Content = parametros.Nome_Botao_1;
                }
                else
                {
                    btnGeral.Visibility = Visibility.Hidden;
                }

                if (parametros.Habilitado_Botao_2 == true)
                {
                    btnPreferencial.Visibility = Visibility.Visible;
                    btnPreferencial.Content = parametros.Nome_Botao_2;
                }
                else
                {
                    btnPreferencial.Visibility = Visibility.Hidden;
                }

                if (parametros.Habilitado_Botao_3 == true)
                {
                    btnMaior80.Visibility = Visibility.Visible;
                    btnMaior80.Content = parametros.Nome_Botao_3;
                }
                else
                {
                    btnMaior80.Visibility = Visibility.Hidden;
                }

            }
            catch (Exception)
            {

            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    if (this.Visibility == Visibility.Visible)
                        if (parametros.Falar_Senha == true)
                        {
                            try
                            {
                                if (parametros.Voz_RetiradaSenha == true)
                                    ClassFalarTexto.FalarTexto("Sistema encerrado.", listaVozes, parametros.Voz_Botao_1);
                            }
                            catch (Exception) { }
                        }

                    Application.Current.Shutdown();
                    Environment.Exit(Environment.ExitCode);

                }


                if (e.Key == Key.F12)
                {
                    if (btnGeral.IsVisible == true)
                        btnGeral_Click(sender, e);
                }


                if (e.Key == Key.F9)
                {
                    if (btnPreferencial.IsVisible == true)
                        btnPreferencial_Click(sender, e);
                }

                if (e.Key == Key.F5)
                {
                    if (btnMaior80.IsVisible == true)
                        btnMaior80_Click(sender, e);

                }
            }
            catch (Exception)
            {

            }
        }

        public void EnviarEstacoes()
        {
            try
            {
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception)
            {

            }
        }

        private void btnGeral_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                setorSelecionado = -1;

                if (parametros.Habilitado_Setor_1 == false)
                {
                    FormImprimirSenha aguarde = new FormImprimirSenha("SenhaNormal", this, setorSelecionado);
                    aguarde.ShowDialog();
                }
                else
                {
                    if (parametros.Habilitado_Setor_2 == false)
                    {
                        setorSelecionado = 0;
                        FormImprimirSenha aguarde = new FormImprimirSenha("SenhaNormal", this, setorSelecionado);
                        aguarde.ShowDialog();
                    }
                    else
                    {
                        EscolherSetor setor = new EscolherSetor(this);
                        setor.Owner = this;
                        setor.ShowDialog();

                        if (setorSelecionado < 5)
                        {
                            FormImprimirSenha aguarde = new FormImprimirSenha("SenhaNormal", this, setorSelecionado);
                            aguarde.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnPreferencial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                setorSelecionado = -1;

                if (parametros.Habilitado_Setor_1 == false)
                {
                    FormImprimirSenha aguarde = new FormImprimirSenha("SenhaPrioridade", this, setorSelecionado);
                    aguarde.ShowDialog();
                }
                else
                {
                    if (parametros.Habilitado_Setor_2 == false)
                    {
                        setorSelecionado = 0;
                        FormImprimirSenha aguarde = new FormImprimirSenha("SenhaPrioridade", this, setorSelecionado);
                        aguarde.ShowDialog();
                    }
                    else
                    {
                        EscolherSetor setor = new EscolherSetor(this);
                        setor.Owner = this;
                        setor.ShowDialog();

                        if (setorSelecionado < 5)
                        {
                            FormImprimirSenha aguarde = new FormImprimirSenha("SenhaPrioridade", this, setorSelecionado);
                            aguarde.ShowDialog();
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void btnMaior80_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                setorSelecionado = -1;

                if (parametros.Habilitado_Setor_1 == false)
                {
                    FormImprimirSenha aguarde = new FormImprimirSenha("SenhaPrioridade80", this, setorSelecionado);
                    aguarde.ShowDialog();
                }
                else
                {
                    if (parametros.Habilitado_Setor_2 == false)
                    {
                        setorSelecionado = 0;
                        FormImprimirSenha aguarde = new FormImprimirSenha("SenhaPrioridade80", this, setorSelecionado);
                        aguarde.ShowDialog();
                    }
                    else
                    {
                        EscolherSetor setor = new EscolherSetor(this);
                        setor.Owner = this;
                        setor.ShowDialog();

                        if (setorSelecionado < 5)
                        {
                            FormImprimirSenha aguarde = new FormImprimirSenha("SenhaPrioridade80", this, setorSelecionado);
                            aguarde.ShowDialog();
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
