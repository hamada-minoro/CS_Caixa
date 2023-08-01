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
using CS_Caixa.Models;
using CS_Caixa.Controls;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using CS_Caixa.Repositorios;
using System.Management;


namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinLogin.xaml
    /// </summary>
    public partial class WinLogin : Window
    {
        ClassUsuario classUsuario = new ClassUsuario();

        List<Usuario> Usuarios = new List<Usuario>();

        Usuario usuarioLogado = new Usuario();

        public DateTime dataSistema;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();

        public Parametro parametros = new Parametro();

        public ClassServidor mainServidor;
        public bool atualizarParametros = false;
        public bool atualizar = false;
        public Cadastro_Pc meuPc = new Cadastro_Pc();
        RepositorioCadastro_Pc repositorioCadastroPc = new RepositorioCadastro_Pc();

        public WinLogin()
        {
            InitializeComponent();
        }

        string _destino;
        Apresentacao _inicio;

        public WinLogin(string destino, Apresentacao inicio)
        {
            _inicio = inicio;
            _destino = destino;
            InitializeComponent();
        }

        public WinLogin(string destino)
        {
            _destino = destino;
            InitializeComponent();
        }

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClassAtualizaSite atualiza = new ClassAtualizaSite();

            ClassVerificaBackup backup = new ClassVerificaBackup();

            try
            {
                parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();

                string nomeMaquina = repositorioCadastroPc.ObterNomeMaquina();

                meuPc = repositorioCadastroPc.ObterPorNomePc(nomeMaquina);

                dataSistema = DateTime.Now;

                dataSistema = GetRemoteDateTime("Servidor", "Administrator", "P@$$w0rd");

                dataSistema = PegarDtHoraAtualizada("servidor");

                lblData.Content = string.Format("Data e Hora: {0}", dataSistema);
                               
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível obter a data atual do Servidor. Será necessário a senha de um usuário Master.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            VerificaBackup ultimaVer = backup.ObterUltimaVerificacao();

            if (ultimaVer == null)
            {
                VerificaBackup vb = new VerificaBackup();

                vb.DataVerificacao = dataSistema;

                vb.HoraVerificacao = DateTime.Now.ToLongTimeString();

                vb.MaquinaVerificou = Environment.MachineName;

                vb.Status = "Validando backups";

                ClassVerificaBackup classVerificaBackup = new ClassVerificaBackup();

                var vbNovo = classVerificaBackup.AdicionarVerificaBackup(vb);

                WinAguardeVerificaBackup verifica = new WinAguardeVerificaBackup(dataSistema, vbNovo.VerificaBackupId);
                verifica.Owner = this;
                verifica.ShowDialog();
            }
            else
            {

                if (ultimaVer.DataVerificacao.Value.ToShortDateString() != DateTime.Now.ToShortDateString())
                {

                    VerificaBackup vb = new VerificaBackup();

                    vb.DataVerificacao = dataSistema;

                    vb.HoraVerificacao = DateTime.Now.ToLongTimeString();

                    vb.MaquinaVerificou = Environment.MachineName;

                    vb.Status = "Validando backups";

                    ClassVerificaBackup classVerificaBackup = new ClassVerificaBackup();

                    var vbNovo = classVerificaBackup.AdicionarVerificaBackup(vb);

                    WinAguardeVerificaBackup verifica = new WinAguardeVerificaBackup(dataSistema, vbNovo.VerificaBackupId);
                    verifica.Owner = this;
                    verifica.ShowDialog();
                }


            }

            Usuarios = classUsuario.ListaUsuarios();

            cmbLogin.ItemsSource = Usuarios.Select(p => p.NomeUsu);

            usuarioLogado = (Usuario)Usuarios[cmbLogin.SelectedIndex];

            cmbLogin.Focus();


            if (meuPc != null)
            {
                if (meuPc.Tipo_Atendimento == "GUICHÊ")
                {
                    // Analisa o endereço IP do servidor informado no textbox
                    IPAddress enderecoIP = IPAddress.Parse(meuPc.Ip_Pc);
                    // Cria uma nova instância do objeto ChatServidor
                    mainServidor = new ClassServidor(enderecoIP, meuPc.Porta_Pc);
                    // Vincula o tratamento de evento StatusChanged a mainServer_StatusChanged
                    ClassServidor.StatusChanged += new StatusChangedEventHandler(mainServidor_StatusChanged);
                    // Inicia o atendimento das conexões
                    mainServidor.IniciaAtendimento();
                }
            }

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }


        public void mainServidor_StatusChanged(object sender, ClassStatusChangedEventArgs e)
        {

            if (e.EventMessage == "Atualizar DataGrids")
                atualizar = true;

            if (e.EventMessage == "Parametros")
                atualizarParametros = true;

        }


        private void ConfirmaLoginMaster()
        {
            var confirmarLogin = new WinConfirmarData(this);
            confirmarLogin.Owner = this;
            confirmarLogin.ShowDialog();

        }




        public static DateTime GetNetworkTime()
        {
            //Servidor nacional para melhor latência
            const string ntpServer = "a.ntp.br";

            // Tamanho da mensagem NTP - 16 bytes (RFC 2030)
            var ntpData = new byte[48];

            //Indicador de Leap (ver RFC), Versão e Modo
            ntpData[0] = 0x1B; //LI = 0 (sem warnings), VN = 3 (IPv4 apenas), Mode = 3 (modo cliente)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;

            //123 é a porta padrão do NTP
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            //NTP usa UDP
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);

            //Caso NTP esteja bloqueado, ao menos nao trava o app
            socket.ReceiveTimeout = 3000;

            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            //Offset para chegar no campo "Transmit Timestamp" (que é
            //o do momento da saída do servidor, em formato 64-bit timestamp
            const byte serverReplyTime = 40;

            //Pegando os segundos
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //e a fração de segundos
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Passando de big-endian pra little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //Tempo em **UTC**
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }

        // stackoverflow.com/a/3294698/162671
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                           ((x & 0x0000ff00) << 8) +
                           ((x & 0x00ff0000) >> 8) +
                           ((x & 0xff000000) >> 24));
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (atualizarParametros == true)
            {
                parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();
                atualizarParametros = false;
            }


            if (parametros.DesligarEstacao == true)
            {
                if (parametros.HoraDesligarEstacao == DateTime.Now.ToLongTimeString())
                {
                    Process.Start("Shutdown", "-s -f -t 00");
                }
            }



        }


        public static DateTime GetRemoteDateTime(string serverName, string userName, string password)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Username = userName;
            options.Password = password;
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;
            options.EnablePrivileges = true;

            ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", serverName), options);
            scope.Connect();

            // faz a query para buscar a hora do sistema...
            SelectQuery selectQuery = new SelectQuery("select * from win32_localtime");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, selectQuery);

            // pega a hora...
            foreach (ManagementObject mo in searcher.Get())
            {
                DateTime dateTime = new DateTime(
                 Convert.ToInt32(mo["Year"]),
                 Convert.ToInt32(mo["Month"]),
                 Convert.ToInt32(mo["Day"]),
                 Convert.ToInt32(mo["Hour"]),
                 Convert.ToInt32(mo["Minute"]),
                 Convert.ToInt32(mo["Second"]));

                return dateTime;
            }

            // a query não retornou nenhum valor...
            return DateTime.MinValue;
        }

        public static DateTime PegarDtHoraAtualizada(string ntpServer)
        {
            var ntpData = new byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            //somente IPV4
            var addresses = Dns.GetHostEntry(ntpServer).AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);

            var ipEndPoint = new IPEndPoint(addresses, 123);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.ReceiveTimeout = 5000; //5 segundos timeout
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds).ToLocalTime();

            //ajustando para hora brasil teste ajuste independente do fuso horario
            //networkDateTime = DateTime.ParseExact(networkDateTime.ToString(), "dd/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            return networkDateTime;
        }


        private void cmbLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usuarioLogado = (Usuario)Usuarios[cmbLogin.SelectedIndex];
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

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {

            string senha = ClassCriptografia.Encrypt(txtSenha.Password);

            bool autorizado = classUsuario.VerificaAutenticacao(usuarioLogado.Id_Usuario, senha);

            if (autorizado)
            {

                if (dataSistema.Year == 1)
                {
                    ConfirmaLoginMaster();

                    if (dataSistema.Year == 1)
                    {
                        return;
                    }
                }

                switch (_destino)
                {
                    case "Configuracoes":
                        WinParametros configuracoes = new WinParametros(usuarioLogado, _inicio, _destino, this);
                        configuracoes.Owner = this;
                        this.Visibility = Visibility.Hidden;
                        configuracoes.ShowDialog();
                        break;

                    case "Estacao":
                        WinPrincipal Principal = new WinPrincipal(usuarioLogado, dataSistema, this);
                        Principal.Owner = this;
                        this.Visibility = Visibility.Hidden;
                        Principal.ShowDialog();
                        break;

                    default:
                        WinParametros configuracoes2 = new WinParametros(usuarioLogado, _inicio, _destino, this);
                        configuracoes2.Owner = this;
                        this.Visibility = Visibility.Hidden;
                        configuracoes2.ShowDialog();
                        break;
                }


            }
            else
            {
                MessageBox.Show("Senha Inválida.", "Senha", MessageBoxButton.OK, MessageBoxImage.Stop);
                txtSenha.Focus();
                txtSenha.SelectAll();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(Environment.ExitCode);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                ConfirmaLoginMaster();
            }
                       
        }
    }
}
