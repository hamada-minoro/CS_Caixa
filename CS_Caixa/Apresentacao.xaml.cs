using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Objetos_de_Valor;
using CS_Caixa.Repositorios;
using FirebirdSql.Data.FirebirdClient;
using IWshRuntimeLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security;
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
    /// Lógica interna para Apresentacao.xaml
    /// </summary>
    public partial class Apresentacao : Window
    {
        BackgroundWorker worker;

        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();
        RepositorioUsuario _AppServicoUsuario = new RepositorioUsuario();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioControle_Uso _AppServicoControle_Uso = new RepositorioControle_Uso();
        Cadastro_Painel cadastroPainel;
        Cadastro_Pc cadastroPc;
        List<Controle_Uso> controles = new List<Controle_Uso>();
        Controle_Uso controle;
        Parametro parametros = new Parametro();
        string erroVerificacao = string.Empty;
        string IpMaquina;
        string destino = string.Empty;
        bool config = false;
        bool cancelado = false;
        List<Usuario> usuarios = new List<Usuario>();
        public string idMaquina;
        string nomeMaquina;
        DataTable dtTotalApontamento = new DataTable();

        public Apresentacao()
        {
            InitializeComponent();
        }


        public void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {                   
                        ExecuteAsAdmin(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\Atualização\Hora_Servidor - Atalho");                

                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível obter a data e hora do servidor.");
                }
                
                AtualizarSiteVerificaBackup();

                try
                {
                    SincronizarUsuariosSite();
                }
                catch (Exception)
                {

                }

                parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();

                controles = _AppServicoControle_Uso.ObterTodos().ToList();

                if (parametros != null)
                {
                    if (parametros.CodigoInstalacao != "")
                        lblCodigo.Content = string.Format("Código: {0}", ClassCriptografia.Decrypt(parametros.CodigoInstalacao));
                    else
                    {
                        lblCodigo.Content = string.Format("Código: {0}", "Código Removido");
                        config = true;
                    }
                }
                else
                {
                    lblCodigo.Content = "";
                    config = true;
                }

                if (controles.Count() > 0)
                {
                    controle = controles.Where(p => p.AtivacaoUso == ClassCriptografia.Encrypt("V")).FirstOrDefault();

                    if (controle != null)
                    {
                        string versao = ClassCriptografia.Decrypt(controle.Versao);

                        lblVersao.Content = string.Format("Versão: {0}", versao);

                        if (!versao.Contains("Demo"))
                            lblDataAtivacao.Content = string.Format("Ativado em: {0}", ClassCriptografia.Decrypt(controle.DataAtivacao));
                    }
                    else
                        config = true;
                }
                else
                    config = true;


                string nomePC = _AppServicoCadastro_Pc.ObterNomeMaquina().ToLower();

                if (nomePC.Contains("balcao"))
                {
                    Process processoAtual = Process.GetCurrentProcess();

                    var processoRodando = (from proc in Process.GetProcesses()
                                           where proc.Id != processoAtual.Id &&
                                                 proc.ProcessName == processoAtual.ProcessName
                                           select proc).FirstOrDefault();

                    if (processoRodando != null)
                    {
                        MessageBox.Show("Uma instância da aplicação já está sendo executada.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        this.Close();
                    }

                }

                cadastroPainel = new Cadastro_Painel();
                cadastroPc = new Cadastro_Pc();
                this.Activate();
                destino = string.Empty;
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao tentar obter as configurações. Favor imformar ao Suporte." + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }
        private void VerificarPcPainel()
        {
            Cadastro_Painel cadastroPainel = _AppServicoCadastro_Painel.ObterPorIdentificador(idMaquina);

            if (cadastroPainel != null)
            {
                cadastroPainel.Ip_Pc = IpMaquina;

                _AppServicoCadastro_Painel.Update(cadastroPainel);
            }


            Cadastro_Pc cadastroPc = _AppServicoCadastro_Pc.ObterPorIdentificadorPc(idMaquina);

            if (cadastroPc != null)
            {
                cadastroPc.Ip_Pc = IpMaquina;

                _AppServicoCadastro_Pc.Update(cadastroPc);
            }
        }



        private DataTable VerificarMaisCincoDias()
        {
            string data = string.Empty;
            string ano = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.AddDays(-5).Day.ToString();

            data = string.Format("{0}-{1}-{2}", ano, mes, dia);

            var table = new DataTable();
            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "select * from Boletos where data_envio <= '" + data + "'";

                    var reader = comm.ExecuteReader();

                    table.Load(reader);
                    if (table.Rows.Count > 0)
                        return table;

                }
            }

            return table;
        }

        private void DeletarMaisCincoDias(DataTable excluir)
        {

            for (int i = 0; i < excluir.Rows.Count; i++)
            {
                using (var conn = AbrirConexao())
                {
                    conn.Open();
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandText = "delete from Boletos where Protocolo = '" + excluir.Rows[i]["Protocolo"].ToString() + "'";

                        comm.ExecuteNonQuery();

                    }
                }
            }
        }

        private IDbConnection AbrirConexao()
        {
            return new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
        }


        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(50);
                worker.ReportProgress(i);

                if (cancelado == true)
                    return;


                if (i == 11)
                {
                    try
                    {
                        DataTable excluir = VerificarMaisCincoDias();

                        if (excluir.Rows.Count > 0)
                            DeletarMaisCincoDias(excluir);
                    }
                    catch (Exception)
                    {

                    }

                }

                if (i == 35)
                {

                    try
                    {
                        nomeMaquina = _AppServicoCadastro_Pc.ObterNomeMaquina();

                        idMaquina = _AppServicoCadastro_Pc.ObterPorNomePc(nomeMaquina).Identificador_Pc;

                        if (idMaquina == null || idMaquina == "")
                        {
                            cadastroPainel = _AppServicoCadastro_Painel.ObterPorNome(nomeMaquina);
                        }

                        IpMaquina = _AppServicoCadastro_Pc.ObterIpMaquina();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            idMaquina = _AppServicoCadastro_Painel.ObterPorNome(nomeMaquina).Identificador_Pc;
                            IpMaquina = _AppServicoCadastro_Pc.ObterIpMaquina();
                        }
                        catch (Exception)
                        {

                        }

                    }
                }


                if (i == 50)
                    VerificacaoControle();

                if (i == 63)
                {
                    if (idMaquina != null)
                        VerificarPcPainel();

                    cadastroPainel = _AppServicoCadastro_Painel.ObterPorIdentificador(idMaquina);

                    usuarios = _AppServicoUsuario.ObterTodos().ToList();

                    cadastroPc = _AppServicoCadastro_Pc.ObterPorIdentificadorPc(idMaquina);
                }



            }



            if (usuarios.Count() < 1)
            {
                if (cadastroPc == null)
                {
                    MessageBox.Show("É necessário cadastrar essa máquina e adicionar um usuário. Você será direcinado para as configurações.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("É necessário cadastrar um usuário. Você será direcinado para as configurações.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                destino = "Configuracoes";
                return;
            }

            if (cadastroPainel != null)
            {
                destino = "Painel";
            }
            else
            {

                if (cadastroPc != null)
                {
                    switch (cadastroPc.Tipo_Entrada)
                    {
                        case 0:
                            destino = "Retirada";
                            break;
                        case 1:
                            destino = "LoginEstacao";
                            break;

                        default:
                            destino = "LoginConfiguracoes";
                            break;
                    }


                }
                else
                {
                    MessageBox.Show("Esta máquina não consta no cadastro, você será direcinado para as configurações.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    destino = "LoginConfiguracoes";
                }
            }


        }



        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblContador.Content = string.Format("{0}%", e.ProgressPercentage);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Visibility = System.Windows.Visibility.Hidden;


            if (cancelado == true)
            {
                this.Close();
                Application.Current.Shutdown();
                return;
            }
            if (config == false)
                if (parametros != null)
                {
                    if (parametros.InicioFimExpediente == true)
                        switch (DateTime.Now.Date.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                if (parametros.Segunda == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            case DayOfWeek.Tuesday:
                                if (parametros.Terca == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            case DayOfWeek.Wednesday:
                                if (parametros.Quarta == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            case DayOfWeek.Thursday:
                                if (parametros.Quinta == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            case DayOfWeek.Friday:
                                if (parametros.Sexta == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            case DayOfWeek.Saturday:
                                if (parametros.Sabado == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            case DayOfWeek.Sunday:
                                if (parametros.Domingo == false)
                                    Process.Start("Shutdown", "-s -f -t 00");
                                break;

                            default:
                                break;
                        }
                }

            if (config == true)
            {
                if (usuarios.Count() > 0)
                {
                    WinLogin configuracoes = new WinLogin("Configuracoes", this);
                    configuracoes.Owner = this;
                    configuracoes.ShowDialog();
                    return;
                }
                else
                {
                    WinParametros configuracoes = new WinParametros(this);
                    configuracoes.Owner = this;
                    configuracoes.ShowDialog();
                    return;
                }
            }


            if (erroVerificacao != string.Empty)
            {
                MessageBox.Show(erroVerificacao, "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);

                if (erroVerificacao.Contains("Ocorreu"))
                    Application.Current.Shutdown();
            }


            switch (destino)
            {
                case "Configuracoes":
                    WinParametros configuracoes = new WinParametros(this);
                    configuracoes.Owner = this;
                    configuracoes.ShowDialog();
                    break;

                case "LoginConfiguracoes":
                    WinLogin LoginConfiguracoes = new WinLogin("Configuracoes", this);
                    LoginConfiguracoes.Owner = this;
                    LoginConfiguracoes.ShowDialog();
                    break;

                case "Painel":
                    WinPainelSenhas painelChamdaSenha = new WinPainelSenhas(idMaquina);
                    painelChamdaSenha.Owner = this;
                    painelChamdaSenha.ShowDialog();
                    break;

                case "Retirada":
                    RetiradaSenha RetiradaSenha = new RetiradaSenha(idMaquina);
                    RetiradaSenha.Owner = this;
                    RetiradaSenha.ShowDialog();
                    break;

                case "LoginEstacao":
                    WinLogin LoginEstacao = new WinLogin("Estacao", this);
                    LoginEstacao.Owner = this;
                    LoginEstacao.ShowDialog();
                    break;

                default:
                    break;
            }



        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                config = true;
                label1.Content = "Parâmetros do Sistema";
            }

            if (e.Key == Key.Escape)
            {
                cancelado = true;
                label1.Content = "Inicialização Cancelada";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void VerificacaoControle()
        {

            if (controle != null)
            {
                DateTime dataFim = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataValidadeFim));
                DateTime dataInicio = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataValidadeInicio));
                DateTime dataUltimoUso = Convert.ToDateTime(ClassCriptografia.Decrypt(controle.DataUltimaUtilizacao));

                if (dataUltimoUso <= DateTime.Now.Date)
                {
                    if (dataInicio <= DateTime.Now.Date && dataFim >= DateTime.Now.Date)
                    {

                        if (dataUltimoUso != DateTime.Now.Date)
                            controle.DiasUtilizado = controle.DiasUtilizado + 1;

                        controle.DataUltimaUtilizacao = ClassCriptografia.Encrypt(DateTime.Now.ToShortDateString());

                        _AppServicoControle_Uso.Update(controle);
                    }
                    else
                    {
                        Controle_Uso novoControle = controles.Where(parametros => parametros.ControleId > controle.ControleId).FirstOrDefault();

                        if (novoControle != null)
                        {
                            novoControle.AtivacaoUso = ClassCriptografia.Encrypt("V");
                            novoControle.DataUltimaUtilizacao = ClassCriptografia.Encrypt(DateTime.Now.ToShortDateString());
                            novoControle.DiasUtilizado = 1;
                            _AppServicoControle_Uso.Update(novoControle);


                            controle.AtivacaoUso = ClassCriptografia.Encrypt("F");
                            _AppServicoControle_Uso.Update(controle);

                        }
                        else
                        {


                            if (ClassCriptografia.Decrypt(controle.Versao) == "Demo")
                                erroVerificacao = "Sua licença de Demonstração expirou, favor solicitar nova licença.";
                            else
                                erroVerificacao = "Sua licença atual expirou, favor solicitar nova licença.";

                            controle.AtivacaoUso = ClassCriptografia.Encrypt("F");
                            _AppServicoControle_Uso.Update(controle);

                            config = true;

                        }

                    }
                }
                else
                {
                    erroVerificacao = "Ocorreu um problema referente a data, entre em contato com o suporte. O sistema será encerrado.";

                }

            }


            string curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());


            FileInfo arquivoVerificar = new FileInfo(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\SysConf.xml");

            if (controles.Count > 0)
            {
                if (!arquivoVerificar.Exists)
                {
                    erroVerificacao = "Não foi possível encontrar o arquivo de configuração.";
                    config = true;
                }
                else
                {
                    bool result = _AppServicoParametros.VerificarArquivoXml(arquivoVerificar.FullName, parametros.CodigoInstalacao);

                    if (result == false)
                    {
                        erroVerificacao = "Erro de Código de Instalação. Para solucionar o problema é necessário solicitar uma nova licença.";
                        config = true;
                    }
                }
            }


        }


        public DateTime dataSistema;

        private void AtualizarSiteVerificaBackup()
        {
            ClassAtualizaSite atualiza = new ClassAtualizaSite();

            ClassVerificaBackup backup = new ClassVerificaBackup();

            try
            {
                dataSistema = DateTime.Now;

                dataSistema = GetRemoteDateTime("Servidor", "Administrator", "P@$$w0rd");

                dataSistema = PegarDtHoraAtualizada("servidor");
                //"a.ntp.br"

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

            if (atualiza.VerificaAtualizar(dataSistema))
            {
                var atu = atualiza.ObterAtualizaSite();

                atu.Status = "ATUALIZANDO";
                atu.DataAtualizacao = dataSistema.ToShortDateString();
                atu.HoraAtualizacao = DateTime.Now.ToLongTimeString();

                atualiza.SalvarAtualizar(atu);

                WinAguardeEnviarSite enviar = new WinAguardeEnviarSite(dataSistema);
                enviar.Owner = this;
                enviar.ShowDialog();


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



        private void SincronizarUsuariosSite()
        {
            var Usuarios = ObterUsuariosSite();

        }

        private List<UsuariosSite> ObterUsuariosSite()
        {
            MySqlDataReader dr;
            DataTable dt = new DataTable();
            var usuarios = new List<UsuariosSite>();
            UsuariosSite usu;
            using (MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477"))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("select * FROM AspNetUsers WHERE ConfirmacaoPresencial = 1", con);

                cmd.CommandType = CommandType.Text;

                dr = cmd.ExecuteReader();

                dt.Load(dr);

                con.Close();
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    usu = new UsuariosSite();

                    usu.AtosFirmas = Convert.ToInt32(dt.Rows[i]["AtosFirmas"]);
                }


            }

            return usuarios;
        }
    }
}
