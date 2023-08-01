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
using CS_Caixa.RelatoriosForms;
using System.Collections;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.IO;
using IWshRuntimeLibrary;
using System.Data;
using MySql.Data.MySqlClient;
using CS_Caixa.Repositorios;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinPrincipal.xaml
    /// </summary>
    public partial class WinPrincipal : Window
    {
        public Usuario usuarioLogado = new Usuario();

        public string TipoAto;

        public string Atribuicao;

        public DateTime _dataSistema;

        VerificaBackup backup;

        List<Atendimento> ListaFilaNotasVinculados = new List<Atendimento>();


        RepositorioSenha _AppServicoSenha = new RepositorioSenha();
        RepositorioParametros _AppServicoParametros = new RepositorioParametros();
        RepositorioPc_Painel _AppServicoPc_Painel = new RepositorioPc_Painel();
        RepositorioCadastro_Pc _AppServicoCadastro_Pc = new RepositorioCadastro_Pc();
        RepositorioCadastro_Painel _AppServicoCadastro_Painel = new RepositorioCadastro_Painel();
        RepositorioUsuario _AppServicoUsuario = new RepositorioUsuario();



        public Parametro parametros = new Parametro();
        Cadastro_Pc meuPc = new Cadastro_Pc();
        List<Senha> senhas = new List<Senha>();
        bool boletoSolicitado = false;

        ClassAtendimento classAtendimento = new ClassAtendimento();

        ClassVerificaBackup verificaBackup = new ClassVerificaBackup();

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        RepositorioCadastro_Pc repositorioCadastroPc = new RepositorioCadastro_Pc();

        string idMaquina = string.Empty;
        public string acao = string.Empty;

        public Senha chamarSenha;

        List<Cadastro_Painel> paineis = new List<Cadastro_Painel>();
        List<Cadastro_Pc> estacoes = new List<Cadastro_Pc>();
        List<Pc_Painel> relacoesPcPainal = new List<Pc_Painel>();
        List<Cadastro_Pc> maquinasEstacao = new List<Cadastro_Pc>();

        WinLogin _login;

        public WinPrincipal(Usuario usuarioLogado, DateTime dataSistema, WinLogin login)
        {
            this.usuarioLogado = usuarioLogado;
            _dataSistema = dataSistema;
            _login = login;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            statusBarNomeUsu.Content = usuarioLogado.NomeUsu;
            lblDataSistema.Content = string.Format("Data e Hora: {0}", _dataSistema);


            ClassDivida divida = new ClassDivida();
            var parcelasPendentes = divida.ObterParcelasPendentesPorUsuario(usuarioLogado.Id_Usuario, _dataSistema.Date);

            if (parcelasPendentes.Count > 0)
            {
                MessageBox.Show("Parcela(s) pendente(s) para este usuário. \n\n Acesse a aba: Arquivo > Dívidas/Descontos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            string nomeMaquina = repositorioCadastroPc.ObterNomeMaquina();

            meuPc = repositorioCadastroPc.ObterPorNomePc(nomeMaquina);
            idMaquina = meuPc.Identificador_Pc;
            parametros = _AppServicoParametros.ObterTodos().FirstOrDefault();
            maquinasEstacao = _AppServicoCadastro_Pc.ObterTodos().Where(p => p.Tipo_Entrada == 1).ToList();
            paineis = _AppServicoCadastro_Painel.ObterTodos().ToList();
            estacoes = _AppServicoCadastro_Pc.ObterTodos().ToList();
            relacoesPcPainal = _AppServicoPc_Painel.ObterTodos().Where(p => p.Cadastro_Pc_Id == meuPc.Identificador_Pc).ToList();


            backup = verificaBackup.ObterUltimaVerificacao();

            lblDataBackup.Content = string.Format("Data: {0}", backup.DataVerificacao.Value.ToShortDateString());
            lblHoraBackup.Content = string.Format("Hora: {0}", backup.HoraVerificacao);
            lblMaquinaBackup.Content = string.Format("Máquina: {0}", backup.MaquinaVerificou);
            lblStatusBackup.Content = string.Format("Status: {0}", backup.Status);


            if (backup.Status == "Backups desatualizados.")
            {
                lblDataBackup.Foreground = Brushes.Red;
                lblHoraBackup.Foreground = Brushes.Red;
                lblMaquinaBackup.Foreground = Brushes.Red;
                lblStatusBackup.Foreground = Brushes.Red;
            }
            else
            {
                lblDataBackup.Foreground = Brushes.LightSlateGray;
                lblHoraBackup.Foreground = Brushes.LightSlateGray;
                lblMaquinaBackup.Foreground = Brushes.LightSlateGray;
                lblStatusBackup.Foreground = Brushes.LightSlateGray;
            }

            if (usuarioLogado.Master == false)
                MenuItemVerificarBackup.IsEnabled = false;

            if (usuarioLogado.Protesto == true || usuarioLogado.Master == true)
                ConsultarSolicitacaoBoletos();


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private IDbConnection AbrirConexao()
        {
            return new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
        }


        public void ConsultarSolicitacaoBoletos()
        {
            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = string.Format("select * from ConsultaApontado where SolicitarBoleto = true");
                    var reader = comm.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);

                    if (table.Rows.Count > 0)
                        boletoSolicitado = true;
                    else
                        boletoSolicitado = false;

                }
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Minute % 2 == 0 && DateTime.Now.Second == 0)
            {

                backup = verificaBackup.ObterUltimaVerificacao();

                lblDataBackup.Content = string.Format("Data: {0}", backup.DataVerificacao.Value.ToShortDateString());
                lblHoraBackup.Content = string.Format("Hora: {0}", backup.HoraVerificacao);
                lblMaquinaBackup.Content = string.Format("Máquina: {0}", backup.MaquinaVerificou);
                lblStatusBackup.Content = string.Format("Status: {0}", backup.Status);

                if (boletoSolicitado == true && (usuarioLogado.Protesto == true || usuarioLogado.Master == true))
                    ConsultarSolicitacaoBoletos();

                if (VerificacaoCaixa() == "S")
                {
                    MessageBox.Show("O sistema está bloqueado. O Windows será desligado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    Application.Current.Shutdown();
                }

            }

            if (boletoSolicitado == true && (usuarioLogado.Protesto == true || usuarioLogado.Master == true))
            {
                if (lblAlertaBoletoSolicitado.Visibility == Visibility.Hidden)
                    lblAlertaBoletoSolicitado.Visibility = Visibility.Visible;
                else
                    lblAlertaBoletoSolicitado.Visibility = Visibility.Hidden;
            }
            else
            {
                lblAlertaBoletoSolicitado.Visibility = Visibility.Hidden;
            }



            if (backup.Status != "Backups validados com sucesso.")
            {
                lblDataBackup.Foreground = Brushes.Red;
                lblHoraBackup.Foreground = Brushes.Red;
                lblMaquinaBackup.Foreground = Brushes.Red;
                lblStatusBackup.Foreground = Brushes.Red;
            }
            else
            {
                lblDataBackup.Foreground = Brushes.LightSlateGray;
                lblHoraBackup.Foreground = Brushes.LightSlateGray;
                lblMaquinaBackup.Foreground = Brushes.LightSlateGray;
                lblStatusBackup.Foreground = Brushes.LightSlateGray;
            }

        }

        private string VerificacaoCaixa()
        {
            try
            {
                string result = string.Empty;

                SqlConnection con = new SqlConnection("Data Source=servidor;Initial Catalog=CS_CAIXA_DB;Persist Security Info=True;User ID=sa;Password=P@$$w0rd");
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Valor FROM VerificacaoCaixa WHERE VerificacaoCaixaId = 1", con);

                cmd.CommandType = CommandType.Text;

                result = ClassCriptografia.Decrypt(cmd.ExecuteScalar().ToString());

                con.Close();

                return result;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        private void imgEscritura_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEscritura.Height = 52;
            imgEscritura.Width = 72;
        }

        private void imgEscritura_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEscritura.Height = 48;
            imgEscritura.Width = 62;
        }

        private void imgEscritura_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Notas == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "NOTAS";

                TipoAto = "ESCRITURA";


                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);

                WinAtosNotas escritura = new WinAtosNotas(usuarioLogado, this, dataInicio, dataFim);
                escritura.Owner = this;
                escritura.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Escritura.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imgProcuracao_MouseEnter(object sender, MouseEventArgs e)
        {
            imgProcuracao.Height = 52;
            imgProcuracao.Width = 72;
        }

        private void imgProcuracao_MouseLeave(object sender, MouseEventArgs e)
        {
            imgProcuracao.Height = 48;
            imgProcuracao.Width = 62;
        }

        private void imgProcuracao_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Notas == true || usuarioLogado.Caixa == true)
            {
                Atribuicao = "NOTAS";

                TipoAto = "PROCURAÇÃO";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);



                WinAtosNotas Procuracao = new WinAtosNotas(usuarioLogado, this, dataInicio, dataFim);
                Procuracao.Owner = this;
                Procuracao.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Procuração.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(Environment.ExitCode);
        }

        private void imgTestamento_MouseEnter(object sender, MouseEventArgs e)
        {
            imgTestamento.Height = 52;
            imgTestamento.Width = 72;
        }

        private void imgTestamento_MouseLeave(object sender, MouseEventArgs e)
        {
            imgTestamento.Height = 48;
            imgTestamento.Width = 62;
        }

        private void imgTestamento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Notas == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "NOTAS";

                TipoAto = "TESTAMENTO";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);


                WinAtosNotas Procuracao = new WinAtosNotas(usuarioLogado, this, dataInicio, dataFim);
                Procuracao.Owner = this;
                Procuracao.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Testamento.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        private void imageCertidaoNotas_MouseEnter(object sender, MouseEventArgs e)
        {
            imgCertidaoNotas.Height = 52;
            imgCertidaoNotas.Width = 72;
        }

        private void imageCertidaoNotas_MouseLeave(object sender, MouseEventArgs e)
        {
            imgCertidaoNotas.Height = 48;
            imgCertidaoNotas.Width = 62;
        }

        private void imageCertidaoNotas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Notas == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "NOTAS";

                TipoAto = "CERTIDÃO NOTAS";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);

                WinAtosNotas Notas = new WinAtosNotas(usuarioLogado, this, dataInicio, dataFim);
                Notas.Owner = this;
                Notas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Certidão Notas.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imageApontamento_MouseEnter(object sender, MouseEventArgs e)
        {
            imgApontamento.Height = 52;
            imgApontamento.Width = 72;
        }

        private void imageApontamento_MouseLeave(object sender, MouseEventArgs e)
        {
            imgApontamento.Height = 48;
            imgApontamento.Width = 62;
        }

        private void imageApontamento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Protesto == true || usuarioLogado.Caixa == true)
            {


                Atribuicao = "PROTESTO";

                TipoAto = "APONTAMENTO";


                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);


                WinAtoProtesto Protesto = new WinAtoProtesto(usuarioLogado, this, dataInicio, dataFim);
                Protesto.Owner = this;
                Protesto.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Apontamento.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        private void imgCancelamento_MouseLeave(object sender, MouseEventArgs e)
        {
            imgCancelamento.Height = 48;
            imgCancelamento.Width = 62;
        }

        private void imgCancelamento_MouseEnter(object sender, MouseEventArgs e)
        {
            imgCancelamento.Height = 52;
            imgCancelamento.Width = 72;
        }

        private void imgCancelamento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Protesto == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "PROTESTO";

                TipoAto = "CANCELAMENTO";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);


                WinAtoProtesto Protesto = new WinAtoProtesto(usuarioLogado, this, dataInicio, dataFim);
                Protesto.Owner = this;
                Protesto.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Cancelamento.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        private void imagePagamento_MouseEnter(object sender, MouseEventArgs e)
        {
            imgPagamento.Height = 52;
            imgPagamento.Width = 72;
        }

        private void imagePagamento_MouseLeave(object sender, MouseEventArgs e)
        {
            imgPagamento.Height = 48;
            imgPagamento.Width = 62;
        }

        private void imagePagamento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Protesto == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "PROTESTO";

                TipoAto = "PAGAMENTO";


                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);


                WinAtoProtesto Protesto = new WinAtoProtesto(usuarioLogado, this, dataInicio, dataFim);
                Protesto.Owner = this;
                Protesto.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Pagamento.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imageCertidaoProtesto_MouseEnter(object sender, MouseEventArgs e)
        {
            imgCertidaoProtesto.Height = 52;
            imgCertidaoProtesto.Width = 72;
        }

        private void imageCertidaoProtesto_MouseLeave(object sender, MouseEventArgs e)
        {
            imgCertidaoProtesto.Height = 48;
            imgCertidaoProtesto.Width = 62;
        }

        private void imageCertidaoProtesto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Protesto == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "PROTESTO";

                TipoAto = "CERTIDÃO PROTESTO";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);


                WinAtoProtesto Protesto = new WinAtoProtesto(usuarioLogado, this, dataInicio, dataFim);
                Protesto.Owner = this;
                Protesto.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Certidão Protesto.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imageRegistro_MouseEnter(object sender, MouseEventArgs e)
        {
            imgRegistro.Height = 52;
            imgRegistro.Width = 72;
        }

        private void imageRegistro_MouseLeave(object sender, MouseEventArgs e)
        {
            imgRegistro.Height = 48;
            imgRegistro.Width = 62;
        }

        private void imageRegistro_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (usuarioLogado.Master == true || usuarioLogado.Rgi == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "RGI";

                TipoAto = "REGISTRO";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);

                WinAtosRgi rgi = new WinAtosRgi(usuarioLogado, this, dataInicio, dataFim);
                rgi.Owner = this;
                rgi.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Registro.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imageAverbacao_MouseEnter(object sender, MouseEventArgs e)
        {
            imgAverbacao.Height = 52;
            imgAverbacao.Width = 72;
        }

        private void imageAverbacao_MouseLeave(object sender, MouseEventArgs e)
        {
            imgAverbacao.Height = 48;
            imgAverbacao.Width = 62;
        }

        private void imageAverbacao_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Rgi == true || usuarioLogado.Caixa == true)
            {
                ClassAto classAto = new ClassAto();

                Atribuicao = "RGI";

                TipoAto = "AVERBAÇÃO";
                List<Ato> ListaAtos = new List<Ato>();

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    ListaAtos = classAto.ListarAtoData(dataInicio, dataFim, TipoAto, Atribuicao);
                }
                else
                {
                    ListaAtos = classAto.ListarAtoNome(TipoAto, Atribuicao, usuarioLogado.Id_Usuario);
                }


                WinAtosRgi rgi = new WinAtosRgi(usuarioLogado, this, dataInicio, dataFim);
                rgi.Owner = this;
                rgi.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Averbação.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imageCertidaoRgi_MouseEnter(object sender, MouseEventArgs e)
        {
            imgCertidaoRgi.Height = 52;
            imgCertidaoRgi.Width = 72;
        }

        private void imageCertidaoRgi_MouseLeave(object sender, MouseEventArgs e)
        {
            imgCertidaoRgi.Height = 48;
            imgCertidaoRgi.Width = 62;
        }

        private void imageCertidaoRgi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Rgi == true || usuarioLogado.Caixa == true)
            {
                ClassAto classAto = new ClassAto();

                Atribuicao = "RGI";

                TipoAto = "CERTIDÃO RGI";
                List<Ato> ListaAtos = new List<Ato>();

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    ListaAtos = classAto.ListarAtoData(dataInicio, dataFim, TipoAto, Atribuicao);
                }
                else
                {
                    ListaAtos = classAto.ListarAtoNome(TipoAto, Atribuicao, usuarioLogado.Id_Usuario);
                }


                WinAtosRgi rgi = new WinAtosRgi(usuarioLogado, this, dataInicio, dataFim);
                rgi.Owner = this;
                rgi.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Averbação.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }



        private void MenuItemCadUsuarios_Click(object sender, RoutedEventArgs e)
        {
            WinCadUsuario cadUsu = new WinCadUsuario(usuarioLogado);
            cadUsu.Owner = this;
            cadUsu.ShowDialog();
            statusBarNomeUsu.Content = usuarioLogado.NomeUsu;
        }


        private void MenuItemTrocarUsuario_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Apresentacao apresentacao = new Apresentacao();
            apresentacao.Show();
        }

        private void MenuItemIncluirCaixa_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                WinAdicionarCaixa adicionarCaixa = new WinAdicionarCaixa(usuarioLogado, _dataSistema);
                adicionarCaixa.Owner = this;
                adicionarCaixa.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemRetiradaCaixa_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                WinRetiradaCaixa retiradaCaixa = new WinRetiradaCaixa(usuarioLogado);
                retiradaCaixa.Owner = this;
                retiradaCaixa.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemSair_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemCadCheques_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                WinCadCheque cadCheque = new WinCadCheque(usuarioLogado);
                cadCheque.Owner = this;
                cadCheque.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemFechamentoCaixa_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                WinRelatorioFechamentoCaixaGeral relatorio = new WinRelatorioFechamentoCaixaGeral("Relatório de Fechamento de Caixa");
                relatorio.Owner = this;
                relatorio.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemRelatorioProtestoFechamentoCaixa_Click(object sender, RoutedEventArgs e)
        {
            WinRelatorioFechamentoCaixaGeral relatorio = new WinRelatorioFechamentoCaixaGeral("Relatório de Fechamento de Caixa Protesto");
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioNotasFechamentoCaixa_Click(object sender, RoutedEventArgs e)
        {
            WinRelatorioFechamentoCaixaGeral relatorio = new WinRelatorioFechamentoCaixaGeral("Relatório de Fechamento de Caixa Notas");
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioRgiFechamentoCaixa_Click(object sender, RoutedEventArgs e)
        {
            WinRelatorioFechamentoCaixaGeral relatorio = new WinRelatorioFechamentoCaixaGeral("Relatório de Fechamento de Caixa Rgi");
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioBalcaoFechamentoCaixa_Click(object sender, RoutedEventArgs e)
        {
            WinRelatorioFechamentoCaixaGeral relatorio = new WinRelatorioFechamentoCaixaGeral("Relatório de Fechamento de Caixa Balcão");
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioProtestoMovimentoDiario_Click(object sender, RoutedEventArgs e)
        {

            WinMovimentoDiario relatorio = new WinMovimentoDiario("Movimento Diário Protesto", usuarioLogado);
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioNotasMovimentoDiario_Click(object sender, RoutedEventArgs e)
        {
            WinMovimentoDiario relatorio = new WinMovimentoDiario("Movimento Diário Notas", usuarioLogado);
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioRgiMovimentoDiario_Click(object sender, RoutedEventArgs e)
        {
            WinMovimentoDiario relatorio = new WinMovimentoDiario("Movimento Diário Rgi", usuarioLogado);
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioBalcaoMovimentoDiario_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                WinMovimentoDiario relatorio = new WinMovimentoDiario("Movimento Diário Balcão", usuarioLogado);
                relatorio.Owner = this;
                relatorio.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemRepasseEscrevente_Click(object sender, RoutedEventArgs e)
        {
            WinMovimentoDiario relatorio = new WinMovimentoDiario("Relatório de Repasse dos Escreventes", usuarioLogado);
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemControleAtosEscritura_Click(object sender, RoutedEventArgs e)
        {
            WinControleDiario relatorio = new WinControleDiario("Controle Diário Escritura", usuarioLogado);
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemControleAtosProcuracao_Click(object sender, RoutedEventArgs e)
        {
            WinControleDiario relatorio = new WinControleDiario("Controle Diário Procuração", usuarioLogado);
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemRelatorioMensalisas_Click(object sender, RoutedEventArgs e)
        {
            WinRelatorioMensalista relatorio = new WinRelatorioMensalista();
            relatorio.Owner = this;
            relatorio.ShowDialog();
        }

        private void MenuItemIndisponibilidade_Click(object sender, RoutedEventArgs e)
        {
            WinIndisponibilidade indisp = new WinIndisponibilidade();
            indisp.Owner = this;
            indisp.ShowDialog();
        }

        private void MenuItemCredito_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true)
            {
                WinControlePagamento credito = new WinControlePagamento("credito", usuarioLogado);
                credito.Owner = this;
                credito.ShowDialog();

            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemDebito_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true)
            {
                WinControlePagamento credito = new WinControlePagamento("Debito", usuarioLogado);
                credito.Owner = this;
                credito.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemControle_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true)
            {
                //WinControlePagamentoControle controle = new WinControlePagamentoControle();
                //controle.Owner = this;
                //controle.ShowDialog();
                WinSaldoControle controle = new WinSaldoControle();
                controle.Owner = this;
                controle.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemIndiceEscritura_Click(object sender, RoutedEventArgs e)
        {
            WinIndiceEscritura indEscritura = new WinIndiceEscritura();
            indEscritura.Owner = this;
            indEscritura.ShowDialog();
        }

        private void MenuItemIndiceProcuracao_Click(object sender, RoutedEventArgs e)
        {
            WinIndiceProcuracao indProcuracao = new WinIndiceProcuracao();
            indProcuracao.Owner = this;
            indProcuracao.ShowDialog();
        }

        private void MenuItemIndiceRgi_Click(object sender, RoutedEventArgs e)
        {
            WinIndiceRgi win = new WinIndiceRgi(usuarioLogado);
            win.Owner = this;
            win.ShowDialog();
        }

        private void MenuItemControleFolhas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemControleNotas_Click(object sender, RoutedEventArgs e)
        {

            Atribuicao = "NOTAS";

            TipoAto = "";


            DateTime dataFim = DateTime.Now.Date;

            DateTime dataInicio = dataFim.AddDays(-10);

            WinControleAtosNotas escritura = new WinControleAtosNotas(usuarioLogado, this, dataInicio, dataFim);
            escritura.Owner = this;
            escritura.ShowDialog();
        }


        private void MenuItemControleProtesto_Click(object sender, RoutedEventArgs e)
        {
            Atribuicao = "PROTESTO";

            TipoAto = "";


            DateTime dataFim = DateTime.Now.Date;

            DateTime dataInicio = dataFim.AddDays(-10);

            WinControleAtosProtesto escritura = new WinControleAtosProtesto(usuarioLogado, this, dataInicio, dataFim);
            escritura.Owner = this;
            escritura.ShowDialog();
        }

        private void MenuItemControleRgi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemCustas_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Atualizar Custas 2021?", "Custas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var custas = new WinAguardeAtualizacaoCustas();
                custas.Owner = this;
                custas.ShowDialog();
            }
        }

        private void MenuItemEtiquetaCertidao_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemImportarMas_Click(object sender, RoutedEventArgs e)
        {
            var ImportarMas = new ImportarMas();
            ImportarMas.Owner = this;
            ImportarMas.ShowDialog();
        }



        private void MenuItemChamarSenhas_Click(object sender, RoutedEventArgs e)
        {
            var senhas = new Chamada(usuarioLogado, idMaquina, this, _login);
            senhas.Owner = this;
            senhas.ShowDialog();
        }



        private void imgApostilamentoHaia_MouseEnter(object sender, MouseEventArgs e)
        {
            imgApostilamentoHaia.Height = 52;
            imgApostilamentoHaia.Width = 72;
        }

        private void imgApostilamentoHaia_MouseLeave(object sender, MouseEventArgs e)
        {
            imgApostilamentoHaia.Height = 48;
            imgApostilamentoHaia.Width = 62;
        }

        private void imgApostilamentoHaia_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Notas == true || usuarioLogado.Caixa == true)
            {

                Atribuicao = "NOTAS";

                TipoAto = "APOSTILAMENTO HAIA";

                DateTime dataFim = DateTime.Now.Date;

                DateTime dataInicio = dataFim.AddDays(-10);

                WinAtosNotas Notas = new WinAtosNotas(usuarioLogado, this, dataInicio, dataFim);
                Notas.Owner = this;
                Notas.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar Certidão Notas.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void imgPagament_MouseEnter(object sender, MouseEventArgs e)
        {
            imgPagament.Height = 52;
            imgPagament.Width = 72;
        }

        private void imgPagament_MouseLeave(object sender, MouseEventArgs e)
        {
            imgPagament.Height = 48;
            imgPagament.Width = 62;
        }


        private void imgPagament_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var winNaoPagos = new WinNaoPagos(usuarioLogado, this);
            winNaoPagos.Owner = this;
            winNaoPagos.ShowDialog();
        }

        private void MenuItemEnviarSite_Click(object sender, RoutedEventArgs e)
        {
            WinAguardeEnviarSite enviar = new WinAguardeEnviarSite(_dataSistema);
            enviar.Owner = this;
            enviar.ShowDialog();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }


        private void imageBalcao_MouseEnter(object sender, MouseEventArgs e)
        {
            imgBalcao.Height = 52;
            imgBalcao.Width = 72;
        }

        private void imageBalcao_MouseLeave(object sender, MouseEventArgs e)
        {
            imgBalcao.Height = 48;
            imgBalcao.Width = 62;
        }

        public string senhaAtual;

        public void imageBalcao_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            chamarSenha = new Senha();

            acao = "";

            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                if (MessageBox.Show("Deseja lançar novo recibo? Se deseja apenas alterar um recibo existente clique 'Não'", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    WinBalcaoNovo balcao = new WinBalcaoNovo(this, false);
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

                        if (parametros.Tipo_Senha == 0)
                            senha = _AppServicoSenha.OberProximaSenha(parametros.ZerarSenhaDiaSeguinte, meuPc.SetorId, senhatipo, out numSequecia, parametros.Qtd_Caracteres_Senha);
                        else
                        {
                            do
                            {
                                senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(parametros.Qtd_Caracteres_Senha + 3));
                            } while (senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);

                        }


                        chamarSenha.Data = DateTime.Now.Date;


                        if (parametros.Utilizar_Aleatorio == true)
                        {
                            do
                            {
                                chamarSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(parametros.Qtd_Caracteres_Senha + 3);
                            }
                            while (chamarSenha.Aleatorio_Confirmacao == senha.ToString() || chamarSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                        }

                        chamarSenha.Hora_Retirada = HoraRetiradaSenha;
                        chamarSenha.Numero_Senha = senha;
                        chamarSenha.Tipo = tipoSenha;
                        chamarSenha.SenhaTipo = senhatipo;
                        chamarSenha.Status = status;
                        chamarSenha.SetorId = meuPc.SetorId;



                        if (parametros.ZerarSenhaDiaSeguinte == true)
                            chamarSenha.ModoSequencial = false;
                        else
                            chamarSenha.ModoSequencial = true;

                        chamarSenha.QtdCaracteres = parametros.Qtd_Caracteres_Senha;

                        chamarSenha.NumeroSequencia = numSequecia;



                        switch (meuPc.SetorId)
                        {
                            case -1:

                                switch (chamarSenha.SenhaTipo)
                                {
                                    case 1:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = parametros.Voz_Botao_1;
                                        break;

                                    case 2:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = parametros.Voz_Botao_2;
                                        break;

                                    case 3:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = parametros.Voz_Botao_3;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 0:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_1;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_1;
                                chamarSenha.Voz = parametros.Voz_Setor_1;
                                break;

                            case 1:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_2;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_2;
                                chamarSenha.Voz = parametros.Voz_Setor_2;
                                break;

                            case 2:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_3;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_3;
                                chamarSenha.Voz = parametros.Voz_Setor_3;
                                break;

                            case 3:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_4;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_4;
                                chamarSenha.Voz = parametros.Voz_Setor_4;
                                break;

                            default:
                                break;
                        }

                        _AppServicoSenha.Adicionar(chamarSenha);


                        WinBalcaoNovo balcao = new WinBalcaoNovo(this, true);
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
                    if (usuarioLogado.Balcao == true)
                    {
                        int senha = 0;
                        string tipoSenha = "";
                        int senhatipo = 5;
                        string HoraRetiradaSenha = DateTime.Now.ToLongTimeString();
                        string status = "ATENDENDO";

                        int numSequecia = 0;

                        if (parametros.Tipo_Senha == 0)
                            senha = _AppServicoSenha.OberProximaSenha(parametros.ZerarSenhaDiaSeguinte, meuPc.SetorId, senhatipo, out numSequecia, parametros.Qtd_Caracteres_Senha);
                        else
                        {
                            do
                            {
                                senha = Convert.ToInt32(ClassGerarAleatorio.NumerosAleatorias(parametros.Qtd_Caracteres_Senha + 3));
                            } while (senhas.Where(p => p.Numero_Senha == senha).FirstOrDefault() != null);

                        }



                        chamarSenha.Data = DateTime.Now.Date;


                        if (parametros.Utilizar_Aleatorio == true)
                        {
                            do
                            {
                                chamarSenha.Aleatorio_Confirmacao = ClassGerarAleatorio.NumerosAleatorias(parametros.Qtd_Caracteres_Senha + 3);
                            }
                            while (chamarSenha.Aleatorio_Confirmacao == senha.ToString() || chamarSenha.Aleatorio_Confirmacao.Substring(0, 1) == "0");
                        }

                        chamarSenha.Hora_Retirada = HoraRetiradaSenha;
                        chamarSenha.Numero_Senha = senha;
                        chamarSenha.Tipo = tipoSenha;
                        chamarSenha.SenhaTipo = senhatipo;
                        chamarSenha.Status = status;
                        chamarSenha.SetorId = meuPc.SetorId;



                        if (parametros.ZerarSenhaDiaSeguinte == true)
                            chamarSenha.ModoSequencial = false;
                        else
                            chamarSenha.ModoSequencial = true;

                        chamarSenha.QtdCaracteres = parametros.Qtd_Caracteres_Senha;

                        chamarSenha.NumeroSequencia = numSequecia;



                        switch (meuPc.SetorId)
                        {
                            case -1:

                                switch (chamarSenha.SenhaTipo)
                                {
                                    case 1:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = parametros.Voz_Botao_1;
                                        break;

                                    case 2:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = parametros.Voz_Botao_2;
                                        break;

                                    case 3:
                                        chamarSenha.LetraSetor = "";
                                        chamarSenha.Voz = parametros.Voz_Botao_3;
                                        break;

                                    default:
                                        break;
                                }

                                break;

                            case 0:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_1;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_1;
                                chamarSenha.Voz = parametros.Voz_Setor_1;
                                break;

                            case 1:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_2;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_2;
                                chamarSenha.Voz = parametros.Voz_Setor_2;
                                break;

                            case 2:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_3;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_3;
                                chamarSenha.Voz = parametros.Voz_Setor_3;
                                break;

                            case 3:
                                chamarSenha.LetraSetor = parametros.Letra_Setor_4;
                                chamarSenha.NomeSetor = parametros.Nome_Setor_4;
                                chamarSenha.Voz = parametros.Voz_Setor_4;
                                break;

                            default:
                                break;
                        }

                        _AppServicoSenha.Adicionar(chamarSenha);


                        WinBalcaoNovo balcao = new WinBalcaoNovo(this, true);
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
                    else
                    {
                        MessageBox.Show("Você não tem permissão para acessar Balcão.", "Erro", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível obter a senha.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        public Senha AtualizarStatusSenha(Senha senha, string status)
        {
            senha.Status = status;
            senha.Caracter_Atendimento = meuPc.Caracter;
            senha.Identificador_Pc = meuPc.Identificador_Pc;
            senha.Usuario_Id = usuarioLogado.Id_Usuario;
            senha.Nome_Usuario = usuarioLogado.NomeUsu;
            senha.FalaOutros = meuPc.FalaOutros;

            if (parametros.ModoRetiradaSenhaManual == false)
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


        private void MenuItemQrCodeNotas_Click(object sender, RoutedEventArgs e)
        {
            WinGerarQrCode gerar = new WinGerarQrCode();
            gerar.Owner = this;
            gerar.ShowDialog();
        }

        private void MenuItemQrCodeRgi_Click(object sender, RoutedEventArgs e)
        {
            WinGerarQrCodeRgi gerar = new WinGerarQrCodeRgi();
            gerar.Owner = this;
            gerar.ShowDialog();
        }

        private void MenuItemFirmas_Click(object sender, RoutedEventArgs e)
        {
            WinTotal total = new WinTotal();
            total.Owner = this;
            total.ShowDialog();
        }

        private void urlShortcutToDesktop(string linkName, string linkUrl)
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + linkUrl);
                writer.Flush();
            }
        }

        private void CreateShortcut(string nomeAtalho, string iniciar, string destino, string descricao, string hotkey)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + nomeAtalho + ".lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = descricao;
            shortcut.WorkingDirectory = iniciar;
            shortcut.Hotkey = hotkey;
            shortcut.TargetPath = destino;
            shortcut.Save();
        }

        private void MenuItemAtalhoFirmas_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho Firmas", @"\\SERVIDOR\Total\firmas", @"\\SERVIDOR\Total\firmas\Firmas.exe", "Total Firmas", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoNotas_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho Notas", @"\\SERVIDOR\Total\Notas", @"\\SERVIDOR\Total\Notas\Notas.exe", "Total Notas", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoRgi_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho Rgi", @"\\SERVIDOR\Total\RGI", @"\\SERVIDOR\Total\RGI\TotalRI.exe", "Total Rgi", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoProtesto_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho Protesto", @"\\SERVIDOR\Total\Protesto", @"\\SERVIDOR\Total\Protesto\Protesto.exe", "Total Protesto", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoEcertidao_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho eCertidao", @"\\SERVIDOR\Total\eCertidao", @"\\SERVIDOR\Total\eCertidao\eCertidao.exe", "Total eCertidao", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoReciboNotas_Click(object sender, RoutedEventArgs e)
        {
            DirectoryCopy(@"\\SERVIDOR\Total\Recibo", @"C:\Total\Recibo", true);
            CreateShortcut(@"\Atalho Recibo Notas", @"\\SERVIDOR\Total\Recibo_Notas", @"\\SERVIDOR\Total\Recibo_Notas\TotalRecibo.exe", "Total Recibo Notas", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoReciboRgi_Click(object sender, RoutedEventArgs e)
        {
            DirectoryCopy(@"\\SERVIDOR\Total\Recibo", @"C:\Total\Recibo", true);
            CreateShortcut(@"\Atalho Recibo Rgi", @"\\SERVIDOR\Total\ReciboNovo", @"\\SERVIDOR\Total\ReciboNovo\TotalRecibo.exe", "Total Recibo Rgi", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoMas_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho MAS", @"\\Servidor\cgj-rj\ModuloApoioServico", @"\\Servidor\cgj-rj\ModuloApoioServico\MAS_COPIADOR.EXE", "MAS - Módulo de Apoio ao Serviço", "Ctrl+Shift+F");
            MessageBox.Show("Atalho criado na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuItemAtalhoTodos_Click(object sender, RoutedEventArgs e)
        {
            CreateShortcut(@"\Atalho Firmas", @"\\SERVIDOR\Total\firmas", @"\\SERVIDOR\Total\firmas\Firmas.exe", "Total Firmas", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho Notas", @"\\SERVIDOR\Total\Notas", @"\\SERVIDOR\Total\Notas\Notas.exe", "Total Notas", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho Rgi", @"\\SERVIDOR\Total\RGI", @"\\SERVIDOR\Total\RGI\TotalRI.exe", "Total Rgi", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho Protesto", @"\\SERVIDOR\Total\Protesto", @"\\SERVIDOR\Total\Protesto\Protesto.exe", "Total Protesto", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho eCertidao", @"\\SERVIDOR\Total\eCertidao", @"\\SERVIDOR\Total\eCertidao\eCertidao.exe", "Total eCertidao", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho Recibo Notas", @"\\SERVIDOR\Total\Recibo_Notas", @"\\SERVIDOR\Total\Recibo_Notas\TotalRecibo.exe", "Total Recibo Notas", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho Recibo Rgi", @"\\SERVIDOR\Total\ReciboNovo", @"\\SERVIDOR\Total\ReciboNovo\TotalRecibo.exe", "Total Recibo Rgi", "Ctrl+Shift+F");
            CreateShortcut(@"\Atalho MAS", @"\\Servidor\cgj-rj\ModuloApoioServico", @"\\Servidor\cgj-rj\ModuloApoioServico\MAS_COPIADOR.EXE", "MAS - Módulo de Apoio ao Serviço", "Ctrl+Shift+F");
            MessageBox.Show("Atalhos criados na área de trabalho.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();
            // Se o diretório de origem não existe, lançar uma exceção.
            if (!dir.Exists)
            {
                dir.Create();

            }
            // Se o diretório de destino não existe, criá-lo.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            // Receba o conteúdo do arquivo do diretório para copiar.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                // Crie o caminho para a nova cópia do arquivo.
                var temppath = System.IO.Path.Combine(destDirName, file.Name);
                // Copie o arquivo.
                file.CopyTo(temppath, true);
            }
            // Se copySubDirs é verdade, copiar os subdiretórios.
            if (!copySubDirs) return;
            foreach (var subdir in dirs)
            {
                // Criar o subdiretório.
                var temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                // Copiar os subdiretórios.
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }

        private void MenuItemVerificarBackup_Click(object sender, RoutedEventArgs e)
        {
            VerificaBackup vb = new VerificaBackup();

            vb.DataVerificacao = _dataSistema;

            vb.HoraVerificacao = DateTime.Now.ToLongTimeString();

            vb.MaquinaVerificou = Environment.MachineName;

            vb.Status = "Validando backups";

            ClassVerificaBackup classVerificaBackup = new ClassVerificaBackup();

            var vbNovo = classVerificaBackup.AdicionarVerificaBackup(vb);



            WinAguardeVerificaBackup verifica = new WinAguardeVerificaBackup(_dataSistema, vbNovo.VerificaBackupId);
            verifica.Owner = this;
            verifica.ShowDialog();


            backup = verificaBackup.ObterUltimaVerificacao();

            lblDataBackup.Content = string.Format("Data: {0}", backup.DataVerificacao.Value.ToShortDateString());
            lblHoraBackup.Content = string.Format("Hora: {0}", backup.HoraVerificacao);
            lblMaquinaBackup.Content = string.Format("Máquina: {0}", backup.MaquinaVerificou);
            lblStatusBackup.Content = string.Format("Status: {0}", backup.Status);
        }

        private void MenuItemVerificaCenprot_Click(object sender, RoutedEventArgs e)
        {
            WinCenprot cenprot = new WinCenprot();
            cenprot.Owner = this;
            cenprot.ShowDialog();
        }

        private void MenuItemConsultaFirmas_Click(object sender, RoutedEventArgs e)
        {
            WinConsultaReconhecimento consultaReconhecimento = new WinConsultaReconhecimento();
            consultaReconhecimento.Owner = this;
            consultaReconhecimento.ShowDialog();
        }

        private void MenuItemQrCodePrenotacao_Click(object sender, RoutedEventArgs e)
        {
            var etiquetaPrenotacao = new WinEtiquetaPrenotacao();
            etiquetaPrenotacao.Owner = this;
            etiquetaPrenotacao.ShowDialog();
        }

        private void MenuItemEnviarBoleto_Click(object sender, RoutedEventArgs e)
        {
            var enviar = new WinEnviarBoleto(this);
            enviar.Owner = this;
            enviar.ShowDialog();
        }

        private void MenuItemCadSite_Click(object sender, RoutedEventArgs e)
        {
            var site = new WinCadastroSite();
            site.Owner = this;
            site.ShowDialog();
        }

        private void MenuItemDivida_Click(object sender, RoutedEventArgs e)
        {
            var dividas = new WinDividasCartorio(usuarioLogado);
            dividas.Owner = this;
            dividas.ShowDialog();
        }

        private void MenuItemRepasseCaixa_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                var repasse = new WinRepasse(_dataSistema.Date);
                repasse.Owner = this;
                repasse.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuário logado não tem permissão para acessar esta tela.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemAgendaEletronica_Click(object sender, RoutedEventArgs e)
        {
            var agenda = new AgendaEletronica(usuarioLogado);
            agenda.Owner = this;
            agenda.ShowDialog();
        }



        private void MenuItemReciboCartao_Click(object sender, RoutedEventArgs e)
        {
            var recibo = new WinImprimirReciboCartao();
            recibo.Owner = this;
            recibo.ShowDialog();
        }

        private void Mensalista_Click(object sender, RoutedEventArgs e)
        {
            var mensalista = new WinCadMensalista();
            mensalista.Owner = this;
            mensalista.ShowDialog();
        }

        private void MenuItemReciboNotas_Click(object sender, RoutedEventArgs e)
        {
             var reciboNotas = new WinReciboNotasControle(usuarioLogado);
             reciboNotas.Owner = this;
             reciboNotas.ShowDialog();
        }

        private void MenuItemRecolhimento_Click(object sender, RoutedEventArgs e)
        {
            var recolhimento = new WinRecolhimento();
            recolhimento.Owner = this;
            recolhimento.ShowDialog();
        }
    }
}
