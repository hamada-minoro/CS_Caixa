
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
using CS_Caixa.Controls;
using CS_Caixa.Models;
using System.Net;
using System.Net.Sockets;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using CS_Caixa.Repositorios;
using System.Data.SqlClient;



namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinBalcao.xaml
    /// </summary>
    public partial class WinBalcaoNovo : Window
    {
        private WinPrincipal winPrincipal;

        WinChamarSenhas _chamadaSenhas;

        Chamada _chamada;

        bool salvarAtendimento = false;

        bool salvar = false;

        public bool automatico = false;

        bool btnStatusNovo = true;

        RepositorioCadastro_Pc cadastroPc = new RepositorioCadastro_Pc();


        ConexaoPainelSenha conexao = new ConexaoPainelSenha();
        string nomeMaquina = Environment.MachineName.Substring(Environment.MachineName.Length - 2, 2);
        ClassAtendimento classAtendimento = new ClassAtendimento();

        ConfirmarChamadaSenha _confirmaChamada;
        string _acao;


        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        int ano = DateTime.Now.Date.Year;
        public int idRecibo;
        string calcularAto = string.Empty;
        public int qtdCopias = 0;
        int totalCopias = 0;
        public Usuario usuarioLogado = new Usuario();
        public ReciboBalcao reciboBalcao = new ReciboBalcao();
        public List<Ato> listaSelos = new List<Ato>();
        public List<Ato> listaSelosConsultaSelos = new List<Ato>();
        public List<Ato> listaSelosConsultaPendente = new List<Ato>();
        DateTime data = DateTime.Now.Date;
        ClassUsuario classUsuario = new ClassUsuario();
        public List<Usuario> listaUsuarios = new List<Usuario>();
        ClassMensalista classMensalista = new ClassMensalista();
        List<CadMensalista> listaMensalista = new List<CadMensalista>();
        ClassBalcao classBalcao = new ClassBalcao();
        ClassCustasNotas classCustasNotas = new ClassCustasNotas();
        public List<CustasNota> custas = new List<CustasNota>();
        List<ReciboBalcao> listaRecibosConsulta = new List<ReciboBalcao>();
        string status = "pronto";
        public decimal porcentagemIss;

        string totalInicial = string.Empty;
        bool masterParaSalvar = false;
        public bool senhaConfirmada = false;


        public WinBalcaoNovo()
        {
            InitializeComponent();
        }

        public WinBalcaoNovo(WinChamarSenhas chamadaSenhas, WinPrincipal principal)
        {
            _chamadaSenhas = chamadaSenhas;
            winPrincipal = principal;
            salvarAtendimento = true;
            InitializeComponent();
        }

        public WinBalcaoNovo(Chamada chamadaSenhas, WinPrincipal principal, ConfirmarChamadaSenha confirmaChamada)
        {
            _chamada = chamadaSenhas;
            salvarAtendimento = true;
            winPrincipal = principal;
            _confirmaChamada = confirmaChamada;
            InitializeComponent();
        }

        public WinBalcaoNovo(Chamada chamadaSenhas, WinPrincipal principal)
        {
            _chamada = chamadaSenhas;
            salvarAtendimento = true;
            winPrincipal = principal;
            InitializeComponent();
        }

        public WinBalcaoNovo(WinPrincipal winPrincipal, bool btnNovoStatus)
        {
            // TODO: Complete member initialization
            this.winPrincipal = winPrincipal;
            _acao = winPrincipal.acao;
            InitializeComponent();
            btnStatusNovo = btnNovoStatus;
        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            txtLetraSelo.Text = classBalcao.LetraSeloAtual().Letra;

            usuarioLogado = winPrincipal.usuarioLogado;

            listaUsuarios = classUsuario.ListaUsuarios();

            cmbFuncionario.ItemsSource = listaUsuarios;
            cmbFuncionario.DisplayMemberPath = "NomeUsu";
            cmbFuncionario.SelectedValuePath = "Id_Usuario";

            listaMensalista = classMensalista.ListaMensalistas();

            cmbMensalista.ItemsSource = listaMensalista.Select(p => p.Nome);

            custas = classCustasNotas.ListaCustas().Where(p => p.ANO == ano).ToList();

            porcentagemIss = Convert.ToDecimal(custas.Where(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());

            IniciaForm();

            btnNovo.IsEnabled = btnStatusNovo;

            btnNovo.Focus();

            ConsultarSelosDuplicados();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();

        }



        private void ProcNovo()
        {
            try
            {

                tabControl1.SelectedIndex = 0;

                TabItemConsultaRecibo.IsEnabled = false;
                TabItemConsultaSelos.IsEnabled = false;
                TabItemSequenciaSelos.IsEnabled = false;
                TabItemControleSelos.IsEnabled = false;
                //TabItemDut.IsEnabled = false;
                TabItemDuplicidade.IsEnabled = false;

                btnNovo.IsEnabled = false;

                btnLimpar.IsEnabled = true;

                btnCancelar.IsEnabled = true;

                btnAdicionar.IsEnabled = true;

                groupBoxDadosRecibo.IsEnabled = true;

                groupBoxSelos.IsEnabled = true;


                lblNumeroRecibo.Visibility = Visibility.Visible;

                classBalcao = new ClassBalcao();

                idRecibo = classBalcao.ProximoReciboBalcao();

                reciboBalcao = classBalcao.ReciboBalcao(idRecibo);

                lblNumeroRecibo.Content = string.Format("RECIBO: {0}", reciboBalcao.NumeroRecibo);

                if (winPrincipal.usuarioLogado.Caixa == true || winPrincipal.usuarioLogado.Master == true)
                {
                    checkBoxPago.IsEnabled = true;
                    cmbFuncionario.IsEnabled = true;
                }
                else
                {
                    checkBoxPago.IsEnabled = false;
                    cmbFuncionario.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void IniciarNovoRecibo()
        {
            listaSelos = new List<Ato>();
            dataGridSelosAdicionados.ItemsSource = listaSelos;
            dataGridSelosAdicionados.Items.Refresh();
            cmbTipoCustas.SelectedIndex = 0;
            cmbTipoPagamento.SelectedIndex = 0;
            cmbMensalista.SelectedIndex = -1;

            cmbFuncionario.Text = usuarioLogado.NomeUsu;

            datePickerData.SelectedDate = data;

            if (usuarioLogado.Master == true)
                checkBoxPago.IsChecked = false;
            else
                checkBoxPago.IsChecked = false;



            txtRequisicao.Text = "";

            txtDesconto.Text = "0,00";

            txtAdicionar.Text = "0,00";

            lblQtdCopia.Content = "0";

            lblQtdSeloAbert.Content = "0";

            lblQtdSeloAut.Content = "0";

            lblQtdSeloRecAut.Content = "0";

            lblQtdSeloRecSem.Content = "0";

            lblQtdSeloMaterializacao.Content = "0";

            txtQtdAut.Text = "0";

            txtQtdAbert.Text = "0";

            txtQtdCopia.Text = "0";

            txtQtdRecAut.Text = "0";

            txtQtdRecAutDut.Text = "0";

            txtQtdRecSem.Text = "0";

            txtQtdMaterializacao.Text = "0";

            txtSeloInicialAut.Text = "";

            txtSeloFinalAut.Text = "";

            txtSeloInicialAbert.Text = "";

            txtSeloFinalAbert.Text = "";

            txtSeloInicialRecAut.Text = "";

            txtSeloFinalRecAut.Text = "";

            txtSeloInicialRecAutDut.Text = "";

            txtSeloFinalRecAutDut.Text = "";

            txtSeloInicialRecSem.Text = "";

            txtSeloFinalRecSem.Text = "";

            txtSeloInicialMaterializacao.Text = "";

            txtSeloFinalMaterializacao.Text = "";

            lblTotalAut.Content = "0,00";

            lblTotalAbert.Content = "0,00";

            lblTotalRecAut.Content = "0,00";

            lblTotalRecAutDut.Content = "0,00";

            lblTotalRecSem.Content = "0,00";

            lblTotalMaterializacao.Content = "0,00";

            lblTotal.Content = "0,00";

            lblTroco.Content = "0,00";

            txtValorPagoDinheiro.Text = "0,00";

            txtValorPagoDeposito.Text = "0,00";

            txtValorPagoCheque.Text = "0,00";

            txtValorPagoChequePre.Text = "0,00";

            txtValorPagoBoleto.Text = "0,00";

            txtValorPagoDinheiro.Text = "0,00";

            txtValorPagoCartaoCredito.Text = "0,00";

            txtValorPagoDinheiro.Background = Brushes.White;
            txtValorPagoDeposito.Background = Brushes.White;
            txtValorPagoCheque.Background = Brushes.White;
            txtValorPagoChequePre.Background = Brushes.White;
            txtValorPagoBoleto.Background = Brushes.White;
            txtValorPagoCartaoCredito.Background = Brushes.White;

            totalCopias = 0;

            qtdCopias = 0;
        }

        private void IniciaForm()
        {
            try
            {
                listaSelos = new List<Ato>();
                dataGridSelosAdicionados.ItemsSource = listaSelos;
                dataGridSelosAdicionados.Items.Refresh();
                cmbTipoCustas.SelectedIndex = 0;
                cmbTipoPagamento.SelectedIndex = 0;
                cmbMensalista.SelectedIndex = -1;

                TabItemConsultaRecibo.IsEnabled = true;
                TabItemConsultaSelos.IsEnabled = true;
                TabItemSequenciaSelos.IsEnabled = true;
                TabItemControleSelos.IsEnabled = true;
                TabItemDuplicidade.IsEnabled = true;

                cmbFuncionario.SelectedItem = listaUsuarios.Where(p => p.Id_Usuario == winPrincipal.usuarioLogado.Id_Usuario).FirstOrDefault();

                datePickerData.SelectedDate = data;

                datePickerDataConsulta.SelectedDate = data;

                datePickerDataConsultaSelo.SelectedDate = data;

                datePickerDataConsultaPendentes.SelectedDate = data;

                datePickerDataConsultaDuplicado.SelectedDate = data;

                if (usuarioLogado.Master == true)
                    datePickerData.IsEnabled = true;
                else
                    datePickerData.IsEnabled = false;

                if (listaRecibosConsulta.Count > 0)
                {
                    dataGridReciboBalcao.IsEnabled = true;
                }
                else
                {
                    dataGridReciboBalcao.IsEnabled = false;
                }

                if (usuarioLogado.Caixa == true)
                {
                    checkBoxPago.IsChecked = true;
                }
                else
                {
                    checkBoxPago.IsChecked = false;
                }

                txtRequisicao.Text = "";

                txtDesconto.Text = "0,00";

                txtAdicionar.Text = "0,00";

                lblNumeroRecibo.Visibility = Visibility.Hidden;

                lblQtdCopia.Content = "0";

                lblQtdSeloAbert.Content = "0";

                lblQtdSeloAut.Content = "0";

                lblQtdSeloRecAut.Content = "0";

                lblQtdSeloRecSem.Content = "0";

                lblQtdSeloMaterializacao.Content = "0";

                txtQtdAut.Text = "0";

                txtQtdAbert.Text = "0";

                txtQtdCopia.Text = "0";

                txtQtdRecAut.Text = "0";

                txtQtdRecAutDut.Text = "0";

                txtQtdRecSem.Text = "0";

                txtQtdMaterializacao.Text = "0";

                txtSeloInicialAut.Text = "";

                txtSeloFinalAut.Text = "";

                txtSeloInicialAbert.Text = "";

                txtSeloFinalAbert.Text = "";

                txtSeloInicialRecAut.Text = "";

                txtSeloFinalRecAut.Text = "";

                txtSeloInicialRecAutDut.Text = "";

                txtSeloFinalRecAutDut.Text = "";

                txtSeloInicialRecSem.Text = "";

                txtSeloFinalRecSem.Text = "";

                txtSeloInicialMaterializacao.Text = "";

                txtSeloFinalMaterializacao.Text = "";

                lblTotalAut.Content = "0,00";

                lblTotalAbert.Content = "0,00";

                lblTotalRecAut.Content = "0,00";

                lblTotalRecAutDut.Content = "0,00";

                lblTotalRecSem.Content = "0,00";

                lblTotalMaterializacao.Content = "0,00";

                lblTotal.Content = "0,00";

                lblTroco.Content = "0,00";

                txtValorPagoDinheiro.Text = "0,00";

                txtValorPagoDeposito.Text = "0,00";

                txtValorPagoCheque.Text = "0,00";

                txtValorPagoChequePre.Text = "0,00";

                txtValorPagoBoleto.Text = "0,00";

                txtValorPagoDinheiro.Text = "0,00";

                txtValorPagoCartaoCredito.Text = "0,00";

                txtValorPagoDinheiro.Background = Brushes.White;
                txtValorPagoDeposito.Background = Brushes.White;
                txtValorPagoCheque.Background = Brushes.White;
                txtValorPagoChequePre.Background = Brushes.White;
                txtValorPagoBoleto.Background = Brushes.White;
                txtValorPagoCartaoCredito.Background = Brushes.White;

                groupBoxDadosRecibo.IsEnabled = false;

                btnNovo.IsEnabled = btnStatusNovo;

                btnLimpar.IsEnabled = false;

                btnCancelar.IsEnabled = false;

                btnAdicionar.IsEnabled = false;

                groupBoxSelos.IsEnabled = false;

                dataGridSelosAdicionados.IsEnabled = false;

                GridValoresRecibo.IsEnabled = false;

                txtConsulta.Visibility = Visibility.Hidden;

                totalCopias = 0;

                qtdCopias = 0;

                ConsultaTodos();

                datePickerData.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbTipoPagamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoPagamento.Focus())
            {
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    cmbMensalista.IsEnabled = true;
                    txtRequisicao.IsEnabled = true;
                    txtValorPagoDinheiro.Text = "0,00";
                    txtValorPagoDeposito.Text = "0,00";
                    txtValorPagoBoleto.Text = "0,00";
                    txtValorPagoCheque.Text = "0,00";
                    txtValorPagoChequePre.Text = "0,00";
                    txtValorPagoCartaoCredito.Text = "0,00";
                }
                else
                {
                    txtRequisicao.Text = "";
                    cmbMensalista.SelectedIndex = -1;
                    cmbMensalista.IsEnabled = false;
                    txtRequisicao.IsEnabled = false;
                }

                MudarTipoPagamento(cmbTipoPagamento.SelectedIndex);
            }
        }

        private void MudarTipoPagamento(int index)
        {
            txtValorPagoDinheiro.IsEnabled = false;
            txtValorPagoDinheiro.Text = "0,00";

            txtValorPagoDeposito.IsEnabled = false;
            txtValorPagoDeposito.Text = "0,00";

            txtValorPagoCheque.IsEnabled = false;
            txtValorPagoCheque.Text = "0,00";

            txtValorPagoChequePre.IsEnabled = false;
            txtValorPagoChequePre.Text = "0,00";

            txtValorPagoBoleto.IsEnabled = false;
            txtValorPagoBoleto.Text = "0,00";

            txtValorPagoCartaoCredito.IsEnabled = false;
            txtValorPagoCartaoCredito.Text = "0,00";

            switch (index)
            {
                case 0:
                    txtValorPagoDinheiro.IsEnabled = true;
                    txtValorPagoDinheiro.Text = lblTotal.Content.ToString();
                    break;
                case 1:
                    txtValorPagoDeposito.IsEnabled = true;
                    txtValorPagoDeposito.Text = lblTotal.Content.ToString();
                    break;
                case 3:
                    txtValorPagoCheque.IsEnabled = true;
                    txtValorPagoCheque.Text = lblTotal.Content.ToString();
                    break;
                case 4:
                    txtValorPagoChequePre.IsEnabled = true;
                    txtValorPagoChequePre.Text = lblTotal.Content.ToString();
                    break;
                case 5:
                    txtValorPagoBoleto.IsEnabled = true;
                    txtValorPagoBoleto.Text = lblTotal.Content.ToString();
                    break;

                case 6:
                    txtValorPagoCartaoCredito.IsEnabled = true;
                    txtValorPagoCartaoCredito.Text = lblTotal.Content.ToString();
                    break;

                case 7:
                    txtValorPagoDinheiro.IsEnabled = true;
                    txtValorPagoDinheiro.Text = "0,00";

                    txtValorPagoDeposito.IsEnabled = true;
                    txtValorPagoDeposito.Text = "0,00";

                    txtValorPagoCheque.IsEnabled = true;
                    txtValorPagoCheque.Text = "0,00";

                    txtValorPagoChequePre.IsEnabled = true;
                    txtValorPagoChequePre.Text = "0,00";

                    txtValorPagoBoleto.IsEnabled = true;
                    txtValorPagoBoleto.Text = "0,00";

                    txtValorPagoCartaoCredito.IsEnabled = true;
                    txtValorPagoCartaoCredito.Text = "0,00";
                    break;


                default:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDinheiro.Text = "0,00";

                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoDeposito.Text = "0,00";

                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoCheque.Text = "0,00";

                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoChequePre.Text = "0,00";

                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoBoleto.Text = "0,00";

                    txtValorPagoCartaoCredito.IsEnabled = false;
                    txtValorPagoCartaoCredito.Text = "0,00";
                    break;
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IniciarNovoRecibo();
                ProcNovo();
                status = "novo";
                cmbFuncionario.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void txtQtdAut_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdAut.Text == "0")
            {
                txtQtdAut.Text = "";
            }
            else
            {
                txtQtdAut.Select(0, txtQtdAut.Text.Length);
            }

        }

        private void txtQtdAut_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtQtdAut.Text == "" || txtQtdAut.Text == "0")
            {
                txtLetraSeloAut.Text = "";
                txtQtdAut.Text = "0";
                txtQtdCopia.Text = "0";
                txtSeloInicialAut.Text = "";
                txtSeloFinalAut.Text = "";
                txtSeloInicialAut.IsReadOnly = true;
                CalcularValorAutenticacao();
            }
            else
            {
                try
                {
                    txtSeloInicialAut.IsReadOnly = false;

                    if (txtSeloInicialAut.Text == "")
                    {
                        txtLetraSeloAut.Text = txtLetraSelo.Text;
                        txtSeloInicialAut.Text = string.Format("{0:00000}", classBalcao.LetraSeloAtual().Numero);

                    }

                    if (txtSeloInicialAut.Text.Length == 5)
                    {
                        int numero = Convert.ToInt32(txtSeloInicialAut.Text);
                        txtSeloFinalAut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdAut.Text) - 1));
                        CalcularValorAutenticacao();
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        private void txtQtdAut_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }


        private void txtQtdAut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtQtdCopia_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdCopia.Text == "0")
            {
                txtQtdCopia.Text = "";
            }
            else
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void txtQtdCopia_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtQtdCopia.Text == "")
                {
                    txtQtdCopia.Text = "0";
                }

                qtdCopias = Convert.ToInt32(txtQtdCopia.Text);

                CalcularValorAutenticacao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtQtdCopia_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtQtdCopia_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }


        // -------------------- STATIC ---------------------------------------------
        static void SelosInicialFinal(TextBox txt, KeyEventArgs e)
        {

            int key = (int)e.Key;



            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

        }

        static void textBoxSelo_LostFocus(TextBox txt, RoutedEventArgs e)
        {
            if (txt.Text.Length != 5)
            {
                if (txt.Text.Length >= 5)
                {
                    int numero = Convert.ToInt32(txt.Text.Substring(4, txt.Text.Length - 4));

                    txt.Text = string.Format("{0:00000}", numero);

                }

            }
        }


        // --------------------------------------------------------------------------
        private void txtSeloInicialAut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                SelosInicialFinal(txtSeloInicialAut, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSeloInicialAut_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxSelo_LostFocus(txtSeloInicialAut, e);


                if (txtSeloInicialAut.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialAut.Text);
                    txtSeloFinalAut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdAut.Text) - 1));
                }


            }
            catch (Exception)
            {

            }

        }

        private void txtSeloInicialAut_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtSeloInicialAut.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialAut.Text);
                    txtSeloFinalAut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdAut.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }

        private void CalcularValorAutenticacao()
        {
            try
            {
                lblTotalAut.Content = string.Format("{0:n2}", CalcularValores("autenticação", Convert.ToInt32(txtQtdAut.Text), Convert.ToInt32(txtQtdCopia.Text)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private decimal CalcularValores(string calcularAto, int qtd, int qtdCopia)
        {
            try
            {
                decimal emolumentos = 0;
                decimal emol = 0;
                decimal fetj_20 = 0;
                decimal fundperj_5 = 0;
                decimal funperj_5 = 0;
                decimal funarpen_4 = 0;
                decimal pmcmv_2 = 0;
                decimal iss = 0;
                decimal valorCopias = 0;
                decimal arquivamento = 0;

                decimal arquivEmol = 0;
                decimal arquiv20 = 0;
                decimal arquiv5 = 0;
                decimal arquiv4 = 0;
                decimal arquivIss = 0;


                decimal valorDut = 0;


                if (calcularAto == "autenticação")
                    valorCopias = Convert.ToDecimal(custas.Where(p => p.DESCR == "Cópia").Select(p => p.VALOR).FirstOrDefault());




                string Semol = "0,00";
                string Sfetj_20 = "0,00";
                string Sfundperj_5 = "0,00";
                string Sfunperj_5 = "0,00";
                string Sfunarpen_4 = "0,00";
                string Siss = "0,00";
                string Spmcmv_2 = "0,00";
                int index;

                string SemolArquiv = "0,00";
                string Sfetj_20Arquiv = "0,00";
                string Sfundperj_5Arquiv = "0,00";
                string Sfunperj_5Arquiv = "0,00";
                string Sfunarpen_4Arquiv = "0,00";
                string SissArquiv = "0,00";

                if (calcularAto == "autenticação")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "AUTENTICAÇÃO POR DOCUMENTO OU PÁGINA").Select(p => p.VALOR).FirstOrDefault());

                if (calcularAto == "abertura")
                {
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "ABERTURA DE FIRMA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(custas.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").Select(p => p.VALOR).FirstOrDefault());
                }

                if (calcularAto == "rec autenticidade")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());

                if (calcularAto == "dut")
                {
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(custas.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES").Select(p => p.VALOR).FirstOrDefault());

                    valorDut = Convert.ToDecimal(custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE (DUT)").Select(p => p.VALOR).FirstOrDefault());
                }

                if (calcularAto == "rec semelhança")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR SEMELHANÇA OU CHANCELA").Select(p => p.VALOR).FirstOrDefault());

                if (calcularAto == "materializacao")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Select(p => p.VALOR).FirstOrDefault());

                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    if (calcularAto == "abertura")
                    {
                        emol = emolumentos + arquivamento;
                        fetj_20 = emol * 20 / 100;
                        fundperj_5 = emol * 5 / 100;
                        funperj_5 = emol * 5 / 100;
                        funarpen_4 = emol * 4 / 100;
                        pmcmv_2 = emolumentos * 2 / 100;
                        iss = emol * porcentagemIss / 100;
                    }
                    else
                    {
                        emol = emolumentos;
                        fetj_20 = emolumentos * 20 / 100;
                        fundperj_5 = emolumentos * 5 / 100;
                        funperj_5 = emolumentos * 5 / 100;
                        funarpen_4 = emolumentos * 4 / 100;
                        pmcmv_2 = emolumentos * 2 / 100;

                        arquivEmol = arquivamento;
                        arquiv20 = arquivamento * 20 / 100;
                        arquiv5 = arquivamento * 5 / 100;
                        arquiv4 = arquivamento * 4 / 100;


                        //iss = (100 - porcentagemIss) / 100;
                        //iss = (emol + pmcmv_2) / iss - emol;


                        iss = emol * porcentagemIss / 100;
                        arquivIss = arquivamento * porcentagemIss / 100;
                    }


                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);

                        if (arquivEmol > 0)
                            SemolArquiv = Convert.ToString(arquivEmol);

                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);

                    if (arquivEmol > 0)
                    {
                        Sfetj_20Arquiv = Convert.ToString(arquiv20);
                        Sfundperj_5Arquiv = Convert.ToString(arquiv5);
                        Sfunperj_5Arquiv = Convert.ToString(arquiv5);
                        Sfunarpen_4Arquiv = Convert.ToString(arquiv4);
                        SissArquiv = Convert.ToString(arquivIss);
                    }

                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {

                    valorDut = 0;
                    valorCopias = 0;

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;
                    pmcmv_2 = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Siss = "0,00";
                    Spmcmv_2 = "0,00";


                    arquivEmol = 0;
                    arquiv20 = 0;
                    arquiv5 = 0;
                    arquiv4 = 0;

                    SemolArquiv = "0,00";
                    Sfetj_20Arquiv = "0,00";
                    Sfundperj_5Arquiv = "0,00";
                    Sfunperj_5Arquiv = "0,00";
                    Sfunarpen_4Arquiv = "0,00";
                    SissArquiv = "0,00";

                }


                index = Semol.IndexOf(',');
                Semol = Semol.Substring(0, index + 3);

                index = Sfetj_20.IndexOf(',');
                Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                index = Sfundperj_5.IndexOf(',');
                Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                index = Sfunperj_5.IndexOf(',');
                Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                index = Sfunarpen_4.IndexOf(',');
                Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                index = Spmcmv_2.IndexOf(',');
                Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                //--------------


                index = SemolArquiv.IndexOf(',');
                SemolArquiv = SemolArquiv.Substring(0, index + 3);

                index = Sfetj_20Arquiv.IndexOf(',');
                Sfetj_20Arquiv = Sfetj_20Arquiv.Substring(0, index + 3);


                index = Sfundperj_5Arquiv.IndexOf(',');
                Sfundperj_5Arquiv = Sfundperj_5Arquiv.Substring(0, index + 3);


                index = Sfunperj_5Arquiv.IndexOf(',');
                Sfunperj_5Arquiv = Sfunperj_5Arquiv.Substring(0, index + 3);


                index = Sfunarpen_4Arquiv.IndexOf(',');
                Sfunarpen_4Arquiv = Sfunarpen_4Arquiv.Substring(0, index + 3);

                index = SissArquiv.IndexOf(',');
                SissArquiv = SissArquiv.Substring(0, index + 3);

                if (calcularAto == "abertura")
                {
                    emol = Convert.ToDecimal(Semol);
                    fetj_20 = Convert.ToDecimal(Sfetj_20);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss);
                }
                else
                {
                    emol = Convert.ToDecimal(Semol) + Convert.ToDecimal(SemolArquiv);
                    fetj_20 = Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfetj_20Arquiv);
                    fundperj_5 = Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfundperj_5Arquiv);
                    funperj_5 = Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunperj_5Arquiv);
                    funarpen_4 = Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Sfunarpen_4Arquiv);
                    pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                    iss = Convert.ToDecimal(Siss) + Convert.ToDecimal(SissArquiv);
                }

                return (emol * qtd) + (fetj_20 * qtd) + (fundperj_5 * qtd) + (funperj_5 * qtd) + (funarpen_4 * qtd) + (iss * qtd) + (pmcmv_2 * qtd) + (valorCopias * qtdCopia) + (valorDut * qtd);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void groupBoxSelos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void groupBoxDadosRecibo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void txtSeloFinalAut_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalAut.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalAut.Text.Substring(4, txtSeloFinalAut.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        // -------------------------------------ABERTURA DE FIRMAS ------------------------------------------------------



        private void CalcularValorAbertura()
        {
            try
            {
                lblTotalAbert.Content = string.Format("{0:n2}", CalcularValores("abertura", Convert.ToInt32(txtQtdAbert.Text), 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void txtQtdAbert_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtQtdAbert_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtQtdAbert.Text == "" || txtQtdAbert.Text == "0")
            {
                txtLetraSeloAbert.Text = "";
                txtQtdAbert.Text = "0";
                txtSeloInicialAbert.Text = "";
                txtSeloFinalAbert.Text = "";
                txtSeloInicialAbert.IsReadOnly = true;
                CalcularValorAbertura();
            }
            else
            {
                try
                {
                    txtSeloInicialAbert.IsReadOnly = false;

                    if (txtSeloInicialAbert.Text == "")
                    {
                        txtLetraSeloAbert.Text = txtLetraSelo.Text;
                        txtSeloInicialAbert.Text = string.Format("{0:00000}", classBalcao.LetraSeloAtual().Numero);

                    }

                    if (txtSeloInicialAbert.Text.Length == 5)
                    {
                        int numero = Convert.ToInt32(txtSeloInicialAbert.Text);
                        txtSeloFinalAbert.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdAbert.Text) - 1));
                        CalcularValorAbertura();
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        private void txtQtdAbert_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdAbert.Text == "0")
            {
                txtQtdAbert.Text = "";
            }
            else
            {
                txtQtdAbert.Select(0, txtQtdAbert.Text.Length);
            }
        }

        private void txtSeloInicialAbert_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelosInicialFinal(txtSeloInicialAbert, e);
        }

        private void txtSeloInicialAbert_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxSelo_LostFocus(txtSeloInicialAbert, e);

                if (txtSeloInicialAbert.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialAbert.Text);
                    txtSeloFinalAbert.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdAbert.Text) - 1));
                }



            }
            catch (Exception) { }
        }

        private void txtSeloInicialAbert_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtSeloInicialAbert.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialAbert.Text);
                    txtSeloFinalAbert.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdAbert.Text) - 1));
                }
            }
            catch (Exception) { }
        }

        private void txtSeloFinalAbert_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalAbert.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalAbert.Text.Substring(4, txtSeloFinalAbert.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //---------------------------------------REC AUTENTICIDADE --------------------------------------------------

        private void CalcularValorRecAutenticidade()
        {
            try
            {
                lblTotalRecAut.Content = string.Format("{0:n2}", CalcularValores("rec autenticidade", Convert.ToInt32(txtQtdRecAut.Text), 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtQtdRecAut_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdRecAut.Text == "0")
            {
                txtQtdRecAut.Text = "";
            }
            else
            {
                txtQtdRecAut.Select(0, txtQtdRecAut.Text.Length);
            }
        }

        private void txtQtdRecAut_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdRecAut.Text == "" || txtQtdRecAut.Text == "0")
            {
                txtLetraSeloRecAut.Text = "";
                txtQtdRecAut.Text = "0";
                txtSeloInicialRecAut.Text = "";
                txtSeloFinalRecAut.Text = "";
                txtSeloInicialRecAut.IsReadOnly = true;
                CalcularValorRecAutenticidade();
            }
            else
            {
                try
                {
                    txtSeloInicialRecAut.IsReadOnly = false;

                    if (txtSeloInicialRecAut.Text == "")
                    {
                        txtLetraSeloRecAut.Text = txtLetraSelo.Text;
                        txtSeloInicialRecAut.Text = string.Format("{0:00000}", classBalcao.LetraSeloAtual().Numero);
                    }

                    if (txtSeloInicialRecAut.Text.Length == 5)
                    {
                        int numero = Convert.ToInt32(txtSeloInicialRecAut.Text);
                        txtSeloFinalRecAut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecAut.Text) - 1));
                        CalcularValorRecAutenticidade();
                    }
                }
                catch (Exception) { }
            }
        }

        private void txtQtdRecAut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtSeloInicialRecAut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelosInicialFinal(txtSeloInicialRecAut, e);
        }

        private void txtSeloInicialRecAut_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxSelo_LostFocus(txtSeloInicialRecAut, e);

                if (txtSeloInicialRecAut.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialRecAut.Text);
                    txtSeloFinalRecAut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecAut.Text) - 1));
                }
            }
            catch (Exception) { }
        }

        private void txtSeloInicialRecAut_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtSeloInicialRecAut.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialRecAut.Text);
                    txtSeloFinalRecAut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecAut.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtSeloInicialRecAut_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalRecAut.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalRecAut.Text.Substring(4, txtSeloFinalRecAut.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        //---------------------------------------REC AUTENTICIDADE DUT--------------------------------------------------

        private void CalcularValorRecAutenticidadeDut()
        {
            try
            {
                lblTotalRecAutDut.Content = string.Format("{0:n2}", CalcularValores("dut", Convert.ToInt32(txtQtdRecAutDut.Text), 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtQtdRecAutDut_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdRecAutDut.Text == "0")
            {
                txtQtdRecAutDut.Text = "";
            }
            else
            {
                txtQtdRecAutDut.Select(0, txtQtdRecAutDut.Text.Length);
            }
        }

        private void txtQtdRecAutDut_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdRecAutDut.Text == "" || txtQtdRecAutDut.Text == "0")
            {
                txtLetraSeloRecAutDut.Text = "";
                txtQtdRecAutDut.Text = "0";
                txtSeloInicialRecAutDut.Text = "";
                txtSeloFinalRecAutDut.Text = "";
                txtSeloInicialRecAutDut.IsReadOnly = true;
                CalcularValorRecAutenticidadeDut();
            }
            else
            {
                try
                {
                    txtSeloInicialRecAutDut.IsReadOnly = false;

                    if (txtSeloInicialRecAutDut.Text == "")
                    {
                        txtLetraSeloRecAutDut.Text = txtLetraSelo.Text;
                        txtSeloInicialRecAutDut.Text = string.Format("{0:00000}", classBalcao.LetraSeloAtual().Numero);
                    }

                    if (txtSeloInicialRecAutDut.Text.Length == 5)
                    {
                        int numero = Convert.ToInt32(txtSeloInicialRecAutDut.Text);
                        txtSeloFinalRecAutDut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecAutDut.Text) - 1));
                        CalcularValorRecAutenticidadeDut();
                    }
                }
                catch (Exception)
                {

                }
            }



        }

        private void txtQtdRecAutDut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtSeloInicialRecAutDut_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelosInicialFinal(txtSeloInicialRecAutDut, e);
        }

        private void txtSeloInicialRecAutDut_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxSelo_LostFocus(txtSeloInicialRecAutDut, e);

                if (txtSeloInicialRecAutDut.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialRecAutDut.Text);
                    txtSeloFinalRecAutDut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecAutDut.Text) - 1));
                }
            }
            catch (Exception) { }
        }

        private void txtSeloInicialRecAutDut_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtSeloInicialRecAutDut.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialRecAutDut.Text);
                    txtSeloFinalRecAutDut.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecAutDut.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtSeloInicialRecAutDut_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalRecAutDut.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalRecAutDut.Text.Substring(4, txtSeloFinalRecAutDut.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        //--------------------------------------------REC SEMELHANÇA ------------------------------------------------------------


        private void CalcularValorRecSemelhanca()
        {
            try
            {
                lblTotalRecSem.Content = string.Format("{0:n2}", CalcularValores("rec semelhança", Convert.ToInt32(txtQtdRecSem.Text), 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void txtQtdRecSem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdRecSem.Text == "0")
            {
                txtQtdRecSem.Text = "";
            }
            else
            {
                txtQtdRecSem.Select(0, txtQtdRecSem.Text.Length);
            }
        }

        private void txtQtdRecSem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdRecSem.Text == "" || txtQtdRecSem.Text == "0")
            {
                txtLetraSeloRecSem.Text = "";
                txtQtdRecSem.Text = "0";
                txtSeloInicialRecSem.Text = "";
                txtSeloFinalRecSem.Text = "";
                txtSeloInicialRecSem.IsReadOnly = true;
                CalcularValorRecSemelhanca();
            }
            else
            {
                try
                {
                    txtSeloInicialRecSem.IsReadOnly = false;

                    if (txtSeloInicialRecSem.Text == "")
                    {
                        txtLetraSeloRecSem.Text = txtLetraSelo.Text;
                        txtSeloInicialRecSem.Text = string.Format("{0:00000}", classBalcao.LetraSeloAtual().Numero);
                    }

                    if (txtSeloInicialRecSem.Text.Length == 5)
                    {
                        int numero = Convert.ToInt32(txtSeloInicialRecSem.Text);
                        txtSeloFinalRecSem.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecSem.Text) - 1));
                        CalcularValorRecSemelhanca();
                    }
                }
                catch (Exception) { }
            }
        }

        private void txtQtdRecSem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtSeloInicialRecSem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelosInicialFinal(txtSeloInicialRecSem, e);
        }

        private void txtSeloInicialRecSem_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxSelo_LostFocus(txtSeloInicialRecSem, e);

                if (txtSeloInicialRecSem.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialRecSem.Text);
                    txtSeloFinalRecSem.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecSem.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtSeloInicialRecSem_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtSeloInicialRecSem.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialRecSem.Text);
                    txtSeloFinalRecSem.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdRecSem.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }


        private void txtSeloFinalRecAut_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalRecAut.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalRecAut.Text.Substring(4, txtSeloFinalRecAut.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSeloFinalRecSem_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalRecSem.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalRecSem.Text.Substring(4, txtSeloFinalRecSem.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDesconto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtDesconto.Text.Length > 0)
            {
                if (txtDesconto.Text.Contains(","))
                {
                    int index = txtDesconto.Text.IndexOf(",");

                    if (txtDesconto.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtDesconto_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDesconto.Text != "")
                {
                    try
                    {
                        txtDesconto.Text = string.Format("{0:n2}", Convert.ToDecimal(txtDesconto.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtDesconto.Text = "0,00";
                }

                if (listaSelos.Count > 0)
                {
                    CalculaTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtDesconto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDesconto.Text == "0,00")
            {
                txtDesconto.Text = "";
            }
        }

        private void txtAdicionar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtAdicionar.Text.Length > 0)
            {
                if (txtAdicionar.Text.Contains(","))
                {
                    int index = txtAdicionar.Text.IndexOf(",");

                    if (txtAdicionar.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtAdicionar_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAdicionar.Text != "")
                {
                    try
                    {
                        txtAdicionar.Text = string.Format("{0:n2}", Convert.ToDecimal(txtAdicionar.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtAdicionar.Text = "0,00";
                }

                if (listaSelos.Count > 0)
                {
                    CalculaTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtAdicionar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtAdicionar.Text == "0,00")
            {
                txtAdicionar.Text = "";
            }
        }

        private void cmbTipoCustas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbTipoCustas.Focus())
                {
                    if (txtQtdAut.Text != "" && txtQtdAut.Text != "0")
                    {
                        CalcularValorAutenticacao();
                    }

                    if (txtQtdAbert.Text != "" && txtQtdAbert.Text != "0")
                    {
                        CalcularValorAbertura();
                    }

                    if (txtQtdRecAut.Text != "" && txtQtdRecAut.Text != "0")
                    {
                        CalcularValorRecAutenticidade();
                    }

                    if (txtQtdRecSem.Text != "" && txtQtdRecSem.Text != "0")
                    {
                        CalcularValorRecSemelhanca();
                    }

                    if (txtQtdMaterializacao.Text != "" && txtQtdMaterializacao.Text != "0")
                        CalcularValorMaterializacao();

                    if (txtQtdRecAutDut.Text != "" && txtQtdRecAutDut.Text != "0")
                        CalcularValorRecAutenticidadeDut();

                    if (cmbTipoCustas.SelectedIndex > 1)
                    {
                        cmbTipoPagamento.SelectedIndex = -1;
                        cmbTipoPagamento.IsEnabled = false;
                    }
                    else
                    {
                        cmbTipoPagamento.SelectedIndex = 0;
                        cmbTipoPagamento.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtQtdAut.Text == "")
                    txtQtdAut.Text = "0";

                if (txtQtdAbert.Text == "")
                    txtQtdAbert.Text = "0";

                if (txtQtdRecAut.Text == "")
                    txtQtdRecAut.Text = "0";

                if (txtQtdRecSem.Text == "")
                    txtQtdRecSem.Text = "0";

                if (txtQtdCopia.Text == "")
                    txtQtdCopia.Text = "0";

                if (txtQtdMaterializacao.Text == "")
                    txtQtdMaterializacao.Text = "0";

                if (txtQtdRecAutDut.Text == "")
                    txtQtdRecAutDut.Text = "0";

                if ((txtQtdAut.Text == "0") && (txtQtdAbert.Text == "0") && (txtQtdRecAut.Text == "0") && (txtQtdRecSem.Text == "0") && (txtQtdCopia.Text == "0") && (txtQtdMaterializacao.Text == "0") && (txtQtdRecAutDut.Text == "0"))
                {
                    if (datePickerData.SelectedDate != null)
                    {
                        automatico = true;

                        btnLimpar_Click(sender, e);

                        WinAguardeBalcao aguarde = new WinAguardeBalcao(this);
                        aguarde.Owner = this;
                        aguarde.ShowDialog();
                        automatico = false;

                        if (listaSelos.Count > 0 || totalCopias > 0 || qtdCopias > 0)
                        {
                            dataGridSelosAdicionados.IsEnabled = true;
                            dataGridSelosAdicionados.ItemsSource = listaSelos;
                            dataGridSelosAdicionados.Items.Refresh();

                            groupBoxDadosRecibo.IsEnabled = false;
                            CarregaQtdGrid();

                        }
                        else
                        {
                            dataGridSelosAdicionados.IsEnabled = false;
                            GridValoresRecibo.IsEnabled = false;
                            groupBoxDadosRecibo.IsEnabled = true;
                        }

                        LimpaCamposSelos();
                        CalculaTotal();
                        if (listaSelos.Count > 0)
                            HabilitaTipoPagamento();
                    }
                    else
                    {
                        MessageBox.Show("Informe a Data.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                    return;
                }


                if ((txtQtdAut.Text != "" && txtQtdAut.Text != "0") || (txtQtdAbert.Text != "" && txtQtdAbert.Text != "0") || (txtQtdRecAut.Text != "" && txtQtdRecAut.Text != "0") || (txtQtdRecSem.Text != "" && txtQtdRecSem.Text != "0") || (txtQtdCopia.Text != "" && txtQtdCopia.Text != "0") || (txtQtdMaterializacao.Text != "" && txtQtdMaterializacao.Text != "0") || (txtQtdRecAutDut.Text != "" && txtQtdRecAutDut.Text != "0"))
                {
                    if (cmbTipoPagamento.SelectedIndex == 2)
                    {
                        if (cmbMensalista.SelectedIndex == -1)
                        {
                            MessageBox.Show("Informe o Nome do Mensalista.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            if (txtRequisicao.Text == "")
                            {
                                MessageBox.Show("Informe o número da Requisição.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }
                    }

                    if (datePickerData.SelectedDate != null)
                    {
                        WinAguardeBalcao aguarde = new WinAguardeBalcao(this);
                        aguarde.Owner = this;
                        aguarde.ShowDialog();


                        if (listaSelos.Count > 0 || totalCopias > 0 || qtdCopias > 0)
                        {
                            dataGridSelosAdicionados.IsEnabled = true;
                            dataGridSelosAdicionados.ItemsSource = listaSelos;
                            dataGridSelosAdicionados.Items.Refresh();

                            groupBoxDadosRecibo.IsEnabled = false;
                            CarregaQtdGrid();
                        }
                        else
                        {
                            dataGridSelosAdicionados.IsEnabled = false;
                            GridValoresRecibo.IsEnabled = false;
                            groupBoxDadosRecibo.IsEnabled = true;
                        }

                        LimpaCamposSelos();

                        CalculaTotal();

                        if (listaSelos.Count > 0)
                            HabilitaTipoPagamento();
                    }
                    else
                    {
                        MessageBox.Show("Informe a Data.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                txtValorPagoDinheiro.Focus();

                this.Activate();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HabilitaTipoPagamento()
        {
            GridValoresRecibo.IsEnabled = true;

            switch (cmbTipoPagamento.SelectedIndex)
            {
                case 0:
                    txtValorPagoDinheiro.IsEnabled = true;
                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoCartaoCredito.IsEnabled = false;
                    txtValorPagoDinheiro.Text = lblTotal.Content.ToString();
                    txtValorPagoDinheiro.SelectAll();
                    break;

                case 1:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDeposito.IsEnabled = true;
                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoCartaoCredito.IsEnabled = false;
                    txtValorPagoDeposito.Text = lblTotal.Content.ToString();
                    txtValorPagoDeposito.SelectAll();
                    break;


                case 3:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoCheque.IsEnabled = true;
                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoCartaoCredito.IsEnabled = false;
                    txtValorPagoCheque.Text = lblTotal.Content.ToString();
                    txtValorPagoCheque.SelectAll();
                    break;

                case 4:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoChequePre.IsEnabled = true;
                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoCartaoCredito.IsEnabled = false;
                    txtValorPagoChequePre.Text = lblTotal.Content.ToString();
                    txtValorPagoChequePre.SelectAll();
                    break;

                case 5:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoBoleto.IsEnabled = true;
                    txtValorPagoCartaoCredito.IsEnabled = false;
                    txtValorPagoBoleto.Text = lblTotal.Content.ToString();
                    txtValorPagoBoleto.SelectAll();
                    break;

                case 6:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoCartaoCredito.IsEnabled = true;
                    txtValorPagoCartaoCredito.Text = lblTotal.Content.ToString();
                    txtValorPagoCartaoCredito.SelectAll();
                    break;


                case 7:
                    txtValorPagoDinheiro.IsEnabled = true;
                    txtValorPagoDeposito.IsEnabled = true;
                    txtValorPagoCheque.IsEnabled = true;
                    txtValorPagoChequePre.IsEnabled = true;
                    txtValorPagoBoleto.IsEnabled = true;
                    txtValorPagoCartaoCredito.IsEnabled = true;
                    txtValorPagoDinheiro.Focus();
                    break;

                default:
                    txtValorPagoDinheiro.IsEnabled = false;
                    txtValorPagoDeposito.IsEnabled = false;
                    txtValorPagoCheque.IsEnabled = false;
                    txtValorPagoChequePre.IsEnabled = false;
                    txtValorPagoBoleto.IsEnabled = false;
                    txtValorPagoCartaoCredito.IsEnabled = false;
                    break;
            }
        }

        private void CarregaQtdGrid()
        {
            try
            {
                lblQtdSeloAut.Content = listaSelos.Where(p => p.TipoAto == "AUTENTICAÇÃO").Count();

                totalCopias = totalCopias + qtdCopias;
                lblQtdCopia.Content = totalCopias;

                lblQtdSeloAbert.Content = listaSelos.Where(p => p.TipoAto == "ABERTURA DE FIRMAS").Count();

                lblQtdSeloRecAut.Content = listaSelos.Where(p => p.TipoAto == "REC AUTENTICIDADE" || p.TipoAto == "REC AUTENTICIDADE (DUT)").Count();

                lblQtdSeloRecSem.Content = listaSelos.Where(p => p.TipoAto == "REC SEMELHANÇA" || p.TipoAto == "SINAL PÚBLICO").Count();

                lblQtdSeloMaterializacao.Content = listaSelos.Where(p => p.TipoAto == "MATERIALIZAÇÃO").Count();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CalculaTotal()
        {
            try
            {

                decimal valorCopias;
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    valorCopias = Convert.ToDecimal(custas.Where(p => p.DESCR == "Cópia").Select(p => p.VALOR).FirstOrDefault());
                }
                else
                {
                    valorCopias = 0;
                }
                decimal desconto = Convert.ToDecimal(txtDesconto.Text);
                decimal adicionar = Convert.ToDecimal(txtAdicionar.Text);
                decimal total = Convert.ToDecimal(listaSelos.Sum(p => p.Total));


                total = total + adicionar + (totalCopias * valorCopias) - desconto;

                lblTotal.Content = string.Format("{0:n2}", total);

                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LimpaCamposSelos()
        {
            txtQtdAut.Text = "0";
            txtQtdCopia.Text = "0";
            txtLetraSeloAut.Text = "";
            txtSeloInicialAut.Text = "";
            txtSeloFinalAut.Text = "";
            lblTotalAut.Content = "0,00";

            txtQtdAbert.Text = "0";
            txtLetraSeloAbert.Text = "";
            txtSeloInicialAbert.Text = "";
            txtSeloFinalAbert.Text = "";
            lblTotalAbert.Content = "0,00";

            txtQtdRecAut.Text = "0";
            txtLetraSeloRecAut.Text = "";
            txtSeloInicialRecAut.Text = "";
            txtSeloFinalRecAut.Text = "";
            lblTotalRecAut.Content = "0,00";

            txtQtdRecAutDut.Text = "0";
            txtLetraSeloRecAutDut.Text = "";
            txtSeloInicialRecAutDut.Text = "";
            txtSeloFinalRecAutDut.Text = "";
            lblTotalRecAutDut.Content = "0,00";

            txtQtdRecSem.Text = "0";
            txtLetraSeloRecSem.Text = "";
            txtSeloInicialRecSem.Text = "";
            txtSeloFinalRecSem.Text = "";
            lblTotalRecSem.Content = "0,00";

            txtQtdMaterializacao.Text = "0";
            txtLetraSeloMaterializacao.Text = "";
            txtSeloInicialMaterializacao.Text = "";
            txtSeloFinalMaterializacao.Text = "";
            lblTotalMaterializacao.Content = "0,00";

            qtdCopias = 0;

        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listaSelos.Count > 0 || qtdCopias > 0)
                {
                    listaSelos = new List<Ato>();
                    dataGridSelosAdicionados.ItemsSource = listaSelos;
                    dataGridSelosAdicionados.Items.Refresh();
                    LimpaCamposSelos();
                    lblQtdCopia.Content = "0";
                    lblQtdSeloAbert.Content = "0";
                    lblQtdSeloAut.Content = "0";
                    lblQtdSeloRecAut.Content = "0";
                    lblQtdSeloRecSem.Content = "0";
                    lblQtdSeloMaterializacao.Content = "0";
                    totalCopias = 0;
                    qtdCopias = 0;

                    lblTotal.Content = "0,00";
                    txtValorPagoDinheiro.Text = "0,00";
                    txtValorPagoDeposito.Text = "0,00";
                    txtValorPagoCheque.Text = "0,00";
                    txtValorPagoChequePre.Text = "0,00";
                    txtValorPagoBoleto.Text = "0,00";
                    txtValorPagoCartaoCredito.Text = "0,00";

                    lblTroco.Content = "0,00";
                    dataGridSelosAdicionados.IsEnabled = false;
                    groupBoxDadosRecibo.IsEnabled = true;

                    if (status == "alterar")
                    {
                        classBalcao.RemoverSelosExistentes(reciboBalcao.IdReciboBalcao);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                if (status == "novo" && reciboBalcao != null)
                    classBalcao.RetornaReciboLivre(reciboBalcao);

                IniciaForm();
                LimpaCamposSelos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (btnNovo.IsEnabled == false)
                {
                    if (status == "novo")
                    {
                        MessageBox.Show("Você fechou a tela sem finalizar o Recibo. Este Recibo não será salvo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                        classBalcao.RetornaReciboLivre(reciboBalcao);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (salvarAtendimento == true && salvar == false)
                {
                    _chamada.acao = "cancelar";
                    _acao = "cancelar";
                }

                if (salvarAtendimento == true && salvar == true)
                {
                    _chamada.acao = "finalizar";
                    _acao = "finalizar";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message, "Erro na conexão com servidor", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        bool ativo = true;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (Convert.ToDecimal(lblTroco.Content) > 0M)
            {
                if (ativo == true)
                {
                    label36.Foreground = new SolidColorBrush(Colors.Red);
                    lblTroco.Foreground = new SolidColorBrush(Colors.Red);
                    ativo = false;
                }
                else
                {
                    label36.Foreground = new SolidColorBrush(Colors.Transparent);
                    lblTroco.Foreground = new SolidColorBrush(Colors.Transparent);
                    ativo = true;
                }
            }
            else
            {
                label36.Foreground = new SolidColorBrush(Colors.Red);
                lblTroco.Foreground = new SolidColorBrush(Colors.Red);
            }

        }

        private void txtValorPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtValorPagoDinheiro.Text != "")
                {
                    if (listaSelos.Count > 0)
                    {
                        CalculaTroco();
                    }
                }
                else
                {
                    lblTroco.Content = "0,00";
                }
                if (txtValorPagoDinheiro.Text == "0,00")
                {
                    lblTroco.Content = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CalculaTroco()
        {
            try
            {
                if (txtValorPagoDinheiro != null && txtValorPagoDeposito != null && txtValorPagoCheque != null && txtValorPagoChequePre != null && txtValorPagoBoleto != null && txtValorPagoCartaoCredito != null)
                {
                    decimal dinheiro, deposito, cheque, chequePre, boleto, cartaoCredito;

                    if (txtValorPagoDinheiro.Text == "")
                        dinheiro = 0M;
                    else
                        dinheiro = Convert.ToDecimal(txtValorPagoDinheiro.Text);

                    if (txtValorPagoDeposito.Text == "")
                        deposito = 0M;
                    else
                        deposito = Convert.ToDecimal(txtValorPagoDeposito.Text);

                    if (txtValorPagoCheque.Text == "")
                        cheque = 0M;
                    else
                        cheque = Convert.ToDecimal(txtValorPagoCheque.Text);

                    if (txtValorPagoChequePre.Text == "")
                        chequePre = 0M;
                    else
                        chequePre = Convert.ToDecimal(txtValorPagoChequePre.Text);

                    if (txtValorPagoBoleto.Text == "")
                        boleto = 0M;
                    else
                        boleto = Convert.ToDecimal(txtValorPagoBoleto.Text);

                    if (txtValorPagoCartaoCredito.Text == "")
                        cartaoCredito = 0M;
                    else
                        cartaoCredito = Convert.ToDecimal(txtValorPagoCartaoCredito.Text);

                    var valoresPagos = dinheiro + deposito + cheque + chequePre + boleto + cartaoCredito;

                    if (valoresPagos != 0M)
                        lblTroco.Content = string.Format("{0:n2}", valoresPagos - Convert.ToDecimal(lblTotal.Content));
                    else
                        lblTroco.Content = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void MenuItemLimparCopias_Click(object sender, RoutedEventArgs e)
        {
            qtdCopias = 0;
            totalCopias = 0;
            CarregaQtdGrid();
            CalculaTotal();
        }

        private void MenuItemExcluirSelo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listaSelos.Count > 0)
                {
                    Ato selo = new Ato();
                    selo = (Ato)dataGridSelosAdicionados.SelectedItem;

                    listaSelos.Remove(selo);
                    dataGridSelosAdicionados.ItemsSource = listaSelos;
                    dataGridSelosAdicionados.Items.Refresh();

                    CarregaQtdGrid();
                    CalculaTotal();
                    CalculaTroco();
                }
                if (listaSelos.Count == 0)
                {
                    dataGridSelosAdicionados.IsEnabled = false;
                    groupBoxDadosRecibo.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtQtdAut_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private bool VerificaPagamento(out string mensagem)
        {
            bool resulado = false;
            mensagem = string.Empty;

            switch (cmbTipoPagamento.SelectedIndex)
            {
                case 0:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoDinheiro.Text))
                    {
                        txtValorPagoDinheiro.Text = lblTotal.Content.ToString();
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode estar menor que o total, favor verifique.";
                    }
                    break;

                case 1:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoDeposito.Text))
                    {
                        txtValorPagoDeposito.Text = lblTotal.Content.ToString();
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode estar menor que o total, favor verifique.";
                    }
                    break;


                case 3:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoCheque.Text))
                    {
                        txtValorPagoCheque.Text = lblTotal.Content.ToString();
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode estar menor que o total, favor verifique.";
                    }
                    break;

                case 4:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoChequePre.Text))
                    {
                        txtValorPagoChequePre.Text = lblTotal.Content.ToString();
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode estar menor que o total, favor verifique.";
                    }
                    break;

                case 5:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoBoleto.Text))
                    {
                        txtValorPagoBoleto.Text = lblTotal.Content.ToString();
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode estar menor que o total, favor verifique.";
                    }
                    break;

                case 6:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoCartaoCredito.Text))
                    {
                        txtValorPagoCartaoCredito.Text = lblTotal.Content.ToString();
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode estar menor que o total, favor verifique.";
                    }
                    break;

                case 7:
                    if (Convert.ToDecimal(lblTotal.Content) != (Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text) + Convert.ToDecimal(txtValorPagoCartaoCredito.Text)))
                        mensagem = "Os valores pagos somados devem corresponder exatamente ao total. O troco deve estar igual a '0,00', favor verifique.";
                    else
                        resulado = true;
                    break;
                default:
                    resulado = true;
                    break;
            }

            return resulado;
        }


        private bool VerificaPagamentoMaster(out string mensagem)
        {
            bool resulado = false;
            mensagem = string.Empty;

            switch (cmbTipoPagamento.SelectedIndex)
            {
                case 0:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoDinheiro.Text))
                    {
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 1:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoDeposito.Text))
                    {
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 3:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoCheque.Text))
                    {
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 4:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoChequePre.Text))
                    {
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 5:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoBoleto.Text))
                    {
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 6:
                    if (Convert.ToDecimal(lblTotal.Content) <= Convert.ToDecimal(txtValorPagoCartaoCredito.Text))
                    {
                        resulado = true;
                    }
                    else
                    {
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 7:
                    if (Convert.ToDecimal(lblTotal.Content) > (Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text) + Convert.ToDecimal(txtValorPagoCartaoCredito.Text)))
                    {
                        mensagem = "Os valores pagos somados devem ser igual ou maior que o total. O troco deve estar igual a '0,00' ou valor positivo, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    else
                        resulado = true;
                    break;
                default:

                    resulado = true;
                    break;
            }


            return resulado;
        }


        private List<string> VerificarSelosNaoLancados()
        {

            var classAto = new ClassAto();
            List<string> selosNaoLancados = new List<string>();

            List<Ato> selosCaixa = classAto.ListarAtoDataAto(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value).Where(p => p.Atribuicao == "BALCÃO").ToList();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {

                string data = datePickerData.SelectedDate.Value.ToShortDateString().Replace("/", ".");

                string comando = string.Format("select selo from atos where data = '{0}' and logado = '{1}' and status = 'XML'", data, usuarioLogado.NomeUsu);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();

                    while (dr.Read())
                    {
                        string selo = dr["SELO"].ToString();

                        var letra = selo.Substring(0, 4);

                        var numero = Convert.ToInt32(selo.Substring(4, 5));

                        if (selosCaixa.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero).Count() == 0 && listaSelos.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero).Count() == 0)
                            selosNaoLancados.Add(selo);
                    }
                }
            }

            return selosNaoLancados;
        }


        private List<string> VerificarTodosSelosNaoLancados()
        {
            var classAto = new ClassAto();
            List<string> selosNaoLancados = new List<string>();

            List<Ato> selosCaixa = classAto.ListarAtoDataAto(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value).Where(p => p.Atribuicao == "BALCÃO").ToList();


            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {

                string data = datePickerData.SelectedDate.Value.ToShortDateString().Replace("/", ".");

                string comando = string.Format("select selo from atos where data = '{0}' and status = 'XML'", data);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();

                    while (dr.Read())
                    {
                        string selo = dr["SELO"].ToString();

                        var letra = selo.Substring(0, 4);

                        var numero = Convert.ToInt32(selo.Substring(4, 5));

                        if (selosCaixa.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero).Count() == 0 && listaSelos.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero).Count() == 0)
                            selosNaoLancados.Add(selo);
                    }
                }
            }

            return selosNaoLancados;
        }


        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ok = true;

                if (status == "novo")
                {
                    ClassBalcao classBalcao = new ClassBalcao();

                    for (int i = 0; i < listaSelos.Count; i++)
                    {
                        var selosJaLancados = classBalcao.VerificaSeloExistente(listaSelos[i].LetraSelo, Convert.ToInt32(listaSelos[i].NumeroSelo));

                        if (selosJaLancados.Count > 0)
                        {
                            MessageBox.Show("Selo " + selosJaLancados[0].LetraSelo + selosJaLancados[0].NumeroSelo + " já foi utilizado no recibo " + selosJaLancados[0].ReciboBalcao + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            ok = false;
                        }
                    }
                }

                if (ok == false)
                    return;


                if (listaSelos.Count < 1)
                    return;
                else
                {
                    var selos = VerificarSelosNaoLancados();

                    if (selos.Count > 0)
                    {
                        string mensagemSelo = string.Empty;

                        foreach (var item in selos)
                        {
                            mensagemSelo = mensagemSelo += item + "\n";
                        }

                        if (MessageBox.Show("Selo(s) não lançado(s) foi(ram) encontrado(s).\n\n" + mensagemSelo + "\n\n Deseja retornar e verificar?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            return;
                    }
                }

                string mensagem;
                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    if (VerificaPagamentoMaster(out mensagem) == false)
                    {
                        MessageBox.Show(mensagem, "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }
                else
                {
                    if (VerificaPagamento(out mensagem) == false)
                    {
                        MessageBox.Show(mensagem, "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }

                if (status != "novo")
                {
                    if (totalInicial != lblTotal.Content.ToString())
                        masterParaSalvar = true;


                    if (masterParaSalvar == false)
                    {
                        FinalizarRecibo();

                        if (salvarAtendimento == true)
                        {
                            salvar = true;
                            btnNovo.IsEnabled = true;
                            this.Close();
                        }

                        IniciaForm();
                    }
                    else
                    {
                        if (usuarioLogado.Master == false)
                        {
                            var confirmaMaster = new WinConfirmaSenhaMaster(this);
                            confirmaMaster.Owner = this;
                            confirmaMaster.ShowDialog();
                            if (senhaConfirmada == true)
                            {
                                FinalizarRecibo();

                                if (salvarAtendimento == true)
                                {
                                    salvar = true;
                                    btnNovo.IsEnabled = true;
                                    this.Close();
                                }

                                IniciaForm();
                            }
                            else
                                return;
                        }
                        else
                        {
                            FinalizarRecibo();

                            if (salvarAtendimento == true)
                            {
                                salvar = true;
                                btnNovo.IsEnabled = true;

                                this.Close();
                            }

                            IniciaForm();
                        }
                    }
                }
                else
                {
                    FinalizarRecibo();

                    if (salvarAtendimento == true)
                    {
                        salvar = true;
                        btnNovo.IsEnabled = true;
                        this.Close();
                    }

                    IniciaForm();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FinalizarRecibo()
        {
            try
            {
                var salvarSelo = new ClassAto();

                reciboBalcao.Data = datePickerData.SelectedDate;

                reciboBalcao.IdUsuario = listaUsuarios[cmbFuncionario.SelectedIndex].Id_Usuario;
                reciboBalcao.Usuario = listaUsuarios[cmbFuncionario.SelectedIndex].NomeUsu;
                reciboBalcao.Status = "UTILIZADO";
                reciboBalcao.Pago = checkBoxPago.IsChecked.Value;

                reciboBalcao.TipoCustas = cmbTipoCustas.Text;

                if (cmbTipoCustas.SelectedIndex > 1)
                    reciboBalcao.TipoPagamento = cmbTipoCustas.Text;
                else
                    reciboBalcao.TipoPagamento = cmbTipoPagamento.Text;

                reciboBalcao.QuantAut = listaSelos.Where(p => p.TipoAto == "AUTENTICAÇÃO").Count();
                reciboBalcao.QuantAbert = listaSelos.Where(p => p.TipoAto == "ABERTURA DE FIRMAS").Count();
                reciboBalcao.QuantRecAut = listaSelos.Where(p => p.TipoAto == "REC AUTENTICIDADE").Count();
                reciboBalcao.QuantRecSem = listaSelos.Where(p => p.TipoAto == "REC SEMELHANÇA").Count();
                reciboBalcao.QuantMaterializacao = listaSelos.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Count();
                reciboBalcao.QuantCopia = totalCopias;
                reciboBalcao.ValorAdicionar = Convert.ToDecimal(txtAdicionar.Text);
                reciboBalcao.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);
                reciboBalcao.Mensalista = cmbMensalista.Text;
                if (salvarAtendimento == true)
                    reciboBalcao.IdAtendimento = _chamada.chamarSenha.Senha_Id;
                if (txtRequisicao.Text != "")
                    reciboBalcao.NumeroRequisicao = Convert.ToInt32(txtRequisicao.Text);

                reciboBalcao.Emolumentos = listaSelos.Sum(p => p.Emolumentos);

                reciboBalcao.Fetj = listaSelos.Sum(p => p.Fetj);

                reciboBalcao.Fundperj = listaSelos.Sum(p => p.Fundperj);

                reciboBalcao.Funperj = listaSelos.Sum(p => p.Funperj);

                reciboBalcao.Funarpen = listaSelos.Sum(p => p.Funarpen);

                reciboBalcao.Pmcmv = listaSelos.Sum(p => p.Pmcmv);

                reciboBalcao.Iss = listaSelos.Sum(p => p.Iss);

                reciboBalcao.Mutua = listaSelos.Sum(p => p.Mutua);

                reciboBalcao.Acoterj = listaSelos.Sum(p => p.Acoterj);

                reciboBalcao.Total = Convert.ToDecimal(lblTotal.Content);

                reciboBalcao.ValorTroco = Convert.ToDecimal(lblTroco.Content);

                classBalcao.SalvarRecibo(reciboBalcao);

                if (status == "alterar")
                {
                    var excluirSelos = new ClassBalcao();

                    salvarSelo.ExcluirAtoBalcao(reciboBalcao.IdReciboBalcao);
                }

                for (int i = 0; i < listaSelos.Count; i++)
                {

                    listaSelos[i].IdReciboBalcao = reciboBalcao.IdReciboBalcao;
                    listaSelos[i].ReciboBalcao = reciboBalcao.NumeroRecibo;
                    listaSelos[i].Recibo = reciboBalcao.NumeroRecibo;
                    listaSelos[i].Pago = checkBoxPago.IsChecked.Value;

                    if (cmbFuncionario.SelectedItem != null)
                        listaSelos[i].IdUsuario = ((Usuario)cmbFuncionario.SelectedItem).Id_Usuario;
                    listaSelos[i].Usuario = ((Usuario)cmbFuncionario.SelectedItem).NomeUsu;

                    listaSelos[i].Escrevente = cmbFuncionario.Text;

                    listaSelos[i].DataAto = datePickerData.SelectedDate.Value;
                    listaSelos[i].TipoCobranca = cmbTipoCustas.Text;
                    listaSelos[i].TipoPagamento = cmbTipoPagamento.Text;
                    listaSelos[i].Mensalista = cmbMensalista.Text;

                    if (txtRequisicao.Text != "")
                        listaSelos[i].NumeroRequisicao = Convert.ToInt32(txtRequisicao.Text);

                    if (txtDesconto.Text != "")
                        listaSelos[i].ValorDesconto = Convert.ToDecimal(txtDesconto.Text);

                    if (txtAdicionar.Text != "")
                        listaSelos[i].ValorAdicionar = Convert.ToDecimal(txtAdicionar.Text);

                    salvarSelo.SalvarAto(listaSelos[i], "novo");
                }

                if (status == "novo")
                {
                    var valorPago = new ValorPago()
                    {
                        Data = reciboBalcao.Data,
                        Boleto = Convert.ToDecimal(txtValorPagoBoleto.Text),
                        Cheque = Convert.ToDecimal(txtValorPagoCheque.Text),
                        ChequePre = Convert.ToDecimal(txtValorPagoChequePre.Text),
                        Deposito = Convert.ToDecimal(txtValorPagoDeposito.Text),
                        Dinheiro = Convert.ToDecimal(txtValorPagoDinheiro.Text),
                        CartaoCredito = Convert.ToDecimal(txtValorPagoCartaoCredito.Text),
                        Troco = Convert.ToDecimal(lblTroco.Content),
                        DataModificado = DateTime.Now.ToShortDateString(),
                        HoraModificado = DateTime.Now.ToLongTimeString(),
                        IdUsuario = usuarioLogado.Id_Usuario,
                        NomeUsuario = usuarioLogado.NomeUsu
                    };

                    valorPago.IdAto = 0;
                    valorPago.IdReciboBalcao = reciboBalcao.IdReciboBalcao;

                    if (cmbTipoPagamento.SelectedIndex == 2)
                        valorPago.Mensalista = reciboBalcao.Total;
                    else
                        valorPago.Mensalista = 0M;

                    salvarSelo.SalvarValorPago(valorPago, status, "IdReciboBalcao");
                }
                else
                {
                    if (usuarioLogado.Caixa == true || usuarioLogado.Master == true)
                    {
                        var valorPago = new ValorPago()
                        {
                            Data = reciboBalcao.Data,
                            Boleto = Convert.ToDecimal(txtValorPagoBoleto.Text),
                            Cheque = Convert.ToDecimal(txtValorPagoCheque.Text),
                            ChequePre = Convert.ToDecimal(txtValorPagoChequePre.Text),
                            Deposito = Convert.ToDecimal(txtValorPagoDeposito.Text),
                            Dinheiro = Convert.ToDecimal(txtValorPagoDinheiro.Text),
                            CartaoCredito = Convert.ToDecimal(txtValorPagoCartaoCredito.Text),
                            Troco = Convert.ToDecimal(lblTroco.Content),
                            DataModificado = DateTime.Now.ToShortDateString(),
                            HoraModificado = DateTime.Now.ToLongTimeString(),
                            IdUsuario = usuarioLogado.Id_Usuario,
                            NomeUsuario = usuarioLogado.NomeUsu
                        };

                        valorPago.IdAto = 0;
                        valorPago.IdReciboBalcao = reciboBalcao.IdReciboBalcao;

                        if (cmbTipoPagamento.SelectedIndex == 2)
                            valorPago.Mensalista = reciboBalcao.Total;
                        else
                            valorPago.Mensalista = 0M;

                        salvarSelo.SalvarValorPago(valorPago, status, "IdReciboBalcao");
                    }
                    else
                    {
                        var valorPago = new ValorPago()
                        {
                            Data = reciboBalcao.Data,
                            Boleto = Convert.ToDecimal(txtValorPagoBoleto.Text),
                            Cheque = Convert.ToDecimal(txtValorPagoCheque.Text),
                            ChequePre = Convert.ToDecimal(txtValorPagoChequePre.Text),
                            Deposito = Convert.ToDecimal(txtValorPagoDeposito.Text),
                            Dinheiro = Convert.ToDecimal(txtValorPagoDinheiro.Text),
                            CartaoCredito = Convert.ToDecimal(txtValorPagoCartaoCredito.Text),
                            Troco = 0M,
                            DataModificado = DateTime.Now.ToShortDateString(),
                            HoraModificado = DateTime.Now.ToLongTimeString(),
                            IdUsuario = usuarioLogado.Id_Usuario,
                            NomeUsuario = usuarioLogado.NomeUsu
                        };

                        valorPago.IdAto = 0;
                        valorPago.IdReciboBalcao = reciboBalcao.IdReciboBalcao;

                        if (cmbTipoPagamento.SelectedIndex == 2)
                            valorPago.Mensalista = reciboBalcao.Total;
                        else
                            valorPago.Mensalista = 0M;


                        salvarSelo.SalvarValorPago(valorPago, status, "IdReciboBalcao");
                    }
                }

                if (ckbRecibo.IsChecked == true)
                {
                    var imprimirRecibo = new WinImprimirRecibo(reciboBalcao, listaSelos);
                    imprimirRecibo.Owner = this;
                    imprimirRecibo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void cmbTipoConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbTipoConsulta.Focus())
            {
                datePickerDataConsulta.Visibility = Visibility.Hidden;
                txtConsulta.Visibility = Visibility.Visible;

                if (cmbTipoConsulta.SelectedIndex == 0 || cmbTipoConsulta.SelectedIndex == 3)
                {
                    datePickerDataConsulta.Visibility = Visibility.Visible;
                    txtConsulta.Visibility = Visibility.Hidden;
                }

                txtConsulta.Text = "";

                if (cmbTipoConsulta.SelectedIndex == 0)
                {
                    datePickerDataConsulta.Focus();
                }
                if (cmbTipoConsulta.SelectedIndex > 0)
                {
                    txtConsulta.Focus();
                }
            }
        }

        private void btnConsultarRecibo_Click(object sender, RoutedEventArgs e)
        {
            ConsultaRecibos();
        }

        private void ConsultaRecibos()
        {
            if (cmbTipoConsulta.SelectedIndex == 0)
            {
                try
                {
                    DateTime data;
                    if (datePickerDataConsulta.SelectedDate != null)
                    {
                        data = datePickerDataConsulta.SelectedDate.Value;

                        listaRecibosConsulta = classBalcao.ListaRecibosBalcaoData(data);

                        if (listaRecibosConsulta.Count > 0)
                        {
                            dataGridReciboBalcao.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao.IsEnabled = false;
                        }

                        dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    }
                    else
                    {
                        MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception)
                {

                }

            }


            if (cmbTipoConsulta.SelectedIndex == 1)
            {

                if (txtConsulta.Text != "")
                {
                    try
                    {
                        int recibo = Convert.ToInt32(txtConsulta.Text);

                        List<ReciboBalcao> listaRecibosConsulta = new List<ReciboBalcao>();

                        listaRecibosConsulta = classBalcao.ListaRecibosBalcaoRecibo(recibo);
                        if (listaRecibosConsulta.Count > 0)
                        {
                            dataGridReciboBalcao.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao.IsEnabled = false;
                        }
                        dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    }
                    catch (Exception)
                    {

                    }
                }
            }


            if (cmbTipoConsulta.SelectedIndex == 2)
            {
                try
                {
                    if (txtConsulta.Text.Length == 9)
                    {
                        string letra = txtConsulta.Text.Substring(0, 4);
                        int numero = Convert.ToInt32(txtConsulta.Text.Substring(4, 5));

                        List<ReciboBalcao> listaRecibosConsulta = new List<ReciboBalcao>();

                        listaRecibosConsulta = classBalcao.ListaRecibosBalcaoSelo(letra, numero);
                        if (listaRecibosConsulta.Count > 0)
                        {
                            dataGridReciboBalcao.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao.IsEnabled = false;
                        }
                        dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    }
                }
                catch (Exception)
                {

                }

            }

            if (cmbTipoConsulta.SelectedIndex == 3)
            {
                try
                {
                    DateTime data;
                    if (datePickerDataConsulta.SelectedDate != null)
                    {
                        data = datePickerDataConsulta.SelectedDate.Value;

                        listaRecibosConsulta = classBalcao.ListaRecibosBalcaoDataNaoPago(data);

                        if (listaRecibosConsulta.Count > 0)
                        {
                            dataGridReciboBalcao.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao.IsEnabled = false;
                        }

                        dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == false).Sum(p => p.Total));
                    }
                    else
                    {
                        MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception)
                {

                }

            }


            if (cmbTipoConsulta.SelectedIndex == 4)
            {
                try
                {
                    if (txtConsulta.Text != "")
                    {
                        listaRecibosConsulta = classBalcao.ListaRecibosBalcaoNumeroRecisicao(Convert.ToInt32(txtConsulta.Text));

                        if (listaRecibosConsulta.Count > 0)
                        {
                            dataGridReciboBalcao.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao.IsEnabled = false;
                        }

                        dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    }
                    else
                    {
                        MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception)
                {

                }

            }

            if (cmbTipoConsulta.SelectedIndex == 5)
            {
                try
                {
                    if (txtConsulta.Text != "")
                    {
                        listaRecibosConsulta = classBalcao.ListaRecibosBalcaoNomeMensalista(txtConsulta.Text);

                        if (listaRecibosConsulta.Count > 0)
                        {
                            dataGridReciboBalcao.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao.IsEnabled = false;
                        }

                        dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    }
                    else
                    {
                        MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception)
                {

                }
            }
        }


        private void txtConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (cmbTipoConsulta.SelectedIndex == 1)
            {
                int key = (int)e.Key;

                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
            }
            if (cmbTipoConsulta.SelectedIndex == 2)
            {
                int key = (int)e.Key;

                if (txtConsulta.Text.Length >= 4)
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

                if (txtConsulta.Text.Length <= 3)
                    e.Handled = !(key >= 44 && key <= 69 || key == 25 || key == 2 || key == 3 || key >= 23 && key <= 25);
            }
        }

        private void MenuItemPagar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridReciboBalcao.Items.Count > 0)
            {
                try
                {
                    reciboBalcao = (ReciboBalcao)dataGridReciboBalcao.SelectedItem;
                    reciboBalcao.Pago = true;
                    classBalcao.PagarSelo(reciboBalcao);
                    classBalcao.PagarRecibo(reciboBalcao);
                    dataGridReciboBalcao.Items.Refresh();
                    lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    ConsultaSelos();
                    ConsultaPendentes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                VerificaMenu();
            }
        }

        private void MenuItemNaoPagar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridReciboBalcao.Items.Count > 0)
            {
                try
                {
                    reciboBalcao = (ReciboBalcao)dataGridReciboBalcao.SelectedItem;
                    reciboBalcao.Pago = false;
                    classBalcao.PagarSelo(reciboBalcao);
                    classBalcao.PagarRecibo(reciboBalcao);
                    dataGridReciboBalcao.Items.Refresh();
                    lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));
                    ConsultaSelos();
                    ConsultaPendentes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                VerificaMenu();
            }
        }

        private void dataGridReciboBalcao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VerificaMenu();

            if (dataGridReciboBalcao.SelectedIndex > -1)
            {

                reciboBalcao = (ReciboBalcao)dataGridReciboBalcao.SelectedItem;

                lblNumeroRecibo.Content = string.Format("RECIBO: {0}", reciboBalcao.NumeroRecibo);

                datePickerData.SelectedDate = reciboBalcao.Data;

                checkBoxPago.IsChecked = reciboBalcao.Pago;

                cmbFuncionario.Text = reciboBalcao.Usuario;

                cmbTipoCustas.Text = reciboBalcao.TipoCustas;

                cmbTipoPagamento.Text = reciboBalcao.TipoPagamento;

                cmbMensalista.Text = reciboBalcao.Mensalista;

                txtRequisicao.Text = reciboBalcao.NumeroRequisicao.ToString();

                txtDesconto.Text = string.Format("{0:n2}", reciboBalcao.ValorDesconto);

                txtAdicionar.Text = string.Format("{0:n2}", reciboBalcao.ValorAdicionar);

                lblTotal.Content = string.Format("{0:n2}", reciboBalcao.Total);

                totalInicial = lblTotal.Content.ToString();

                lblNumeroRecibo.Visibility = Visibility.Visible;

                var valorPago = classBalcao.ObterValorPagoPorIdReciboBalcao(reciboBalcao.IdReciboBalcao);

                if (valorPago != null)
                {
                    txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                    txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                    txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                    txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                    txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                    txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
                }

                totalCopias = Convert.ToInt32(reciboBalcao.QuantCopia);

                groupBoxSelos.IsEnabled = true;

                menu.Visibility = Visibility.Hidden;

                listaSelos = classBalcao.ListarSelosIdRecibo(reciboBalcao.IdReciboBalcao);

                dataGridSelosAdicionados.IsEnabled = true;
                dataGridSelosAdicionados.ItemsSource = listaSelos;
                dataGridSelosAdicionados.Items.Refresh();


                CarregaQtdGrid();

                //HabilitaTipoPagamento();

                CalculaTroco();

                GridValoresRecibo.IsEnabled = false;
            }
            else
            {
                IniciaForm();
            }

        }

        private void VerificaMenu()
        {
            try
            {
                if (dataGridReciboBalcao.Items.Count > 0)
                {
                    reciboBalcao = (ReciboBalcao)dataGridReciboBalcao.SelectedItem;

                    if (reciboBalcao.TipoPagamento == "DINHEIRO")
                    {
                        MenuItemDinheiro.IsChecked = true;
                    }
                    else
                    {
                        MenuItemDinheiro.IsChecked = false;
                    }
                    if (reciboBalcao.TipoPagamento == "DEPÓSITO")
                    {
                        MenuItemDeposito.IsChecked = true;
                    }
                    else
                    {
                        MenuItemDeposito.IsChecked = false;
                    }

                    if (reciboBalcao.TipoPagamento == "MENSALISTA")
                    {
                        MenuItemMensalista.IsChecked = true;
                    }
                    else
                    {
                        MenuItemMensalista.IsChecked = false;
                    }

                    if (reciboBalcao.TipoPagamento == "CHEQUE")
                    {
                        MenuItemCheque.IsChecked = true;
                    }
                    else
                    {
                        MenuItemCheque.IsChecked = false;
                    }

                    if (reciboBalcao.TipoPagamento == "CHEQUE-PRÉ")
                    {
                        MenuItemPre.IsChecked = true;
                    }
                    else
                    {
                        MenuItemPre.IsChecked = false;
                    }

                    if (reciboBalcao.TipoPagamento == "BOLETO")
                    {
                        MenuItemBoleto.IsChecked = true;
                    }
                    else
                    {
                        MenuItemBoleto.IsChecked = false;
                    }
                    if (reciboBalcao.TipoPagamento == "CARTÃO CRÉDITO")
                    {
                        MenuItemCartaoCredito.IsChecked = true;
                    }
                    else
                    {
                        MenuItemCartaoCredito.IsChecked = false;
                    }

                    if (!(usuarioLogado.Master == true || usuarioLogado.AlterarAtos == true || usuarioLogado.ExcluirAtos == true))
                    {

                        if (usuarioLogado.ExcluirAtos == true || usuarioLogado.Master == true)
                        {
                            MenuItemExcluirRecibo.IsEnabled = true;
                        }
                        else
                        {
                            MenuItemExcluirRecibo.IsEnabled = false;
                        }

                        if (usuarioLogado.AlterarAtos == true || usuarioLogado.Master == true)
                        {
                            MenuItemAlterarRecibo.IsEnabled = true;
                        }
                        else
                        {
                            MenuItemAlterarRecibo.IsEnabled = false;
                        }

                        MenuItemPagar.IsEnabled = false;
                        MenuItemNaoPagar.IsEnabled = false;

                        if (usuarioLogado.AlterarAtos == true || usuarioLogado.Master == true)
                        {
                            MenuItemPagamento.IsEnabled = true;
                        }
                        else
                        {
                            MenuItemPagamento.IsEnabled = false;
                        }
                    }
                    else
                    {
                        if (reciboBalcao.Pago == true)
                        {
                            MenuItemPagar.IsEnabled = false;
                            MenuItemNaoPagar.IsEnabled = true;
                        }
                        else
                        {
                            MenuItemPagar.IsEnabled = true;
                            MenuItemNaoPagar.IsEnabled = false;
                        }

                        if (reciboBalcao.Pago == false)
                        {
                            MenuItemPagar.IsEnabled = true;
                            MenuItemNaoPagar.IsEnabled = false;
                        }
                        else
                        {
                            MenuItemPagar.IsEnabled = false;
                            MenuItemNaoPagar.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        private void grid2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void btnConsultaSelos_Click(object sender, RoutedEventArgs e)
        {
            ConsultaSelos();
        }


        private void Controle(DateTime data)
        {
            try
            {
                if (datePickerDataConsultaSelo.SelectedDate != null)
                {
                    listaSelosConsultaSelos = new List<Ato>();

                    listaSelosConsultaSelos = classBalcao.ListarSelo(data).Where(p => p.Atribuicao == "BALCÃO").OrderBy(p => p.LetraSelo).OrderBy(p => p.NumeroSelo).ToList();

                    listaRecibosConsulta = classBalcao.ListaRecibosBalcaoData(data);

                    lblQtdAutControle.Content = string.Format("{0}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "AUTENTICAÇÃO").Count());
                    lblValorAutControle.Content = string.Format("{0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "AUTENTICAÇÃO").Sum(p => p.Total));

                    lblQtdAbertControle.Content = string.Format("{0}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "ABERTURA DE FIRMAS").Count());
                    lblValorAbertControle.Content = string.Format("{0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "ABERTURA DE FIRMAS").Sum(p => p.Total));

                    lblQtdRecAutControle.Content = string.Format("{0}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "REC AUTENTICIDADE").Count());
                    lblValorRecAutControle.Content = string.Format("{0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "REC AUTENTICIDADE").Sum(p => p.Total));

                    lblQtdRecSemControle.Content = string.Format("{0}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "REC SEMELHANÇA").Count());
                    lblValorRecSemControle.Content = string.Format("{0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "REC SEMELHANÇA").Sum(p => p.Total));

                    lblQtdMaterializacao.Content = string.Format("{0}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Count());
                    lblValorMaterializacao.Content = string.Format("{0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true && p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Sum(p => p.Total));

                    lblQtdCopiasControle.Content = string.Format("{0}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.QuantCopia));
                    lblValorCopiasControle.Content = string.Format("{0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.QuantCopia) * Convert.ToDecimal(custas.Where(p => p.DESCR == "Cópia").Select(p => p.VALOR).FirstOrDefault()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConsultaTodos()
        {
            ConsultaRecibos();
            ConsultaSelos();
            ConsultaPendentes();
        }

        private void ConsultaSelos()
        {
            try
            {
                if (datePickerDataConsultaSelo.SelectedDate != null)
                {
                    DateTime data = datePickerDataConsultaSelo.SelectedDate.Value;

                    listaSelosConsultaSelos = new List<Ato>();

                    listaSelosConsultaSelos = classBalcao.ListarSelo(data).Where(p => p.Atribuicao == "BALCÃO").OrderBy(p => p.LetraSelo).OrderBy(p => p.NumeroSelo).ToList();

                    dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == true);
                    dataGridSelosPagos.Items.Refresh();

                    dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == false);
                    dataGridSelosNaoPagos.Items.Refresh();

                    lblTotalSelos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true).Sum(p => p.Total));
                    lblTotalNPagos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == false).Sum(p => p.Total));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ConsultaPendentes()
        {
            try
            {
                if (datePickerDataConsultaPendentes.SelectedDate != null)
                {
                    DateTime data = datePickerDataConsultaPendentes.SelectedDate.Value;

                    listaSelosConsultaPendente = classBalcao.ListarSelo(data).Where(p => p.Atribuicao == "BALCÃO").OrderBy(p => p.LetraSelo).OrderBy(p => p.NumeroSelo).ToList();

                    dataGridPendentes.ItemsSource = listaSelosConsultaPendente;
                    dataGridPendentes.Items.Refresh();

                    listBoxPendentes.ItemsSource = VerificarTodosSelosNaoLancados();

                    Controle(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnConsultaPendentes_Click(object sender, RoutedEventArgs e)
        {
            ConsultaPendentes();
        }



        private void SelosPendentes(List<Ato> listaSelos)
        {
            List<string> listaPendentes = new List<string>();
            List<string> listaLetras = new List<string>();
            List<Ato> listaAux = new List<Ato>();
            try
            {
                listaLetras = listaSelos.Select(p => p.LetraSelo).Distinct().ToList();

                for (int i = 0; i < listaLetras.Count; i++)
                {
                    listaAux = new List<Ato>();
                    listaAux = listaSelos.Where(p => p.LetraSelo == listaLetras[i]).Select(p => p).ToList();

                    int selo = listaAux.Min(p => p.NumeroSelo).Value;
                    for (int f = 0; f < listaAux.Count; f++)
                    {
                        if (selo != listaAux[f].NumeroSelo)
                        {
                            for (int v = selo; v < listaAux[f].NumeroSelo; v++)
                            {
                                listaPendentes.Add(listaLetras[i] + " " + v.ToString());

                                selo = v + 1;
                            }

                        }
                        selo++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listBoxPendentes.ItemsSource = listaPendentes;
        }

        private void MenuItemExcluirRecibo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir o recibo número: " + reciboBalcao.NumeroRecibo + "?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (reciboBalcao.IdAtendimento != null)
                    {
                        classAtendimento.DeletarAtendimento(reciboBalcao.IdAtendimento);
                    }

                    classBalcao.ExcluirRecibo(reciboBalcao);

                    listaRecibosConsulta.RemoveAt(dataGridReciboBalcao.SelectedIndex);

                    dataGridReciboBalcao.ItemsSource = listaRecibosConsulta;

                    dataGridReciboBalcao.Items.Refresh();

                    lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));

                    if (dataGridReciboBalcao.Items.Count > 0)
                    {
                        dataGridReciboBalcao.IsEnabled = true;
                    }
                    else
                    {
                        dataGridReciboBalcao.IsEnabled = false;
                    }

                    ConsultaSelos();
                    ConsultaPendentes();

                    MessageBox.Show("Recibo excluído com sucesso!", "Excluído", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MenuItemAlterarRecibo_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridReciboBalcao.SelectedIndex > -1)
            {
                lblNumeroRecibo.Content = string.Format("RECIBO: {0}", reciboBalcao.NumeroRecibo);

                reciboBalcao = (ReciboBalcao)dataGridReciboBalcao.SelectedItem;

                datePickerData.SelectedDate = reciboBalcao.Data;

                checkBoxPago.IsChecked = reciboBalcao.Pago;

                cmbFuncionario.Text = reciboBalcao.Usuario;

                cmbTipoCustas.Text = reciboBalcao.TipoCustas;

                cmbTipoPagamento.Text = reciboBalcao.TipoPagamento;

                cmbMensalista.Text = reciboBalcao.Mensalista;

                txtRequisicao.Text = reciboBalcao.NumeroRequisicao.ToString();

                txtDesconto.Text = string.Format("{0:n2}", reciboBalcao.ValorDesconto);

                txtAdicionar.Text = string.Format("{0:n2}", reciboBalcao.ValorAdicionar);

                lblTotal.Content = string.Format("{0:n2}", reciboBalcao.Total);

                totalInicial = lblTotal.Content.ToString();

                masterParaSalvar = false;

                var valorPago = classBalcao.ObterValorPagoPorIdReciboBalcao(reciboBalcao.IdReciboBalcao);

                if (valorPago != null)
                {
                    txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                    txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                    txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                    txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                    txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                    txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
                }

                totalCopias = Convert.ToInt32(reciboBalcao.QuantCopia);

                groupBoxSelos.IsEnabled = true;

                listaSelos = classBalcao.ListarSelosIdRecibo(reciboBalcao.IdReciboBalcao);

                dataGridSelosAdicionados.IsEnabled = true;
                dataGridSelosAdicionados.ItemsSource = listaSelos;
                dataGridSelosAdicionados.Items.Refresh();

                menu.Visibility = Visibility.Visible;

                CarregaQtdGrid();

                tabControl1.SelectedIndex = 0;

                TabItemConsultaRecibo.IsEnabled = false;
                TabItemConsultaSelos.IsEnabled = false;
                TabItemSequenciaSelos.IsEnabled = false;
                TabItemControleSelos.IsEnabled = false;

                btnNovo.IsEnabled = false;

                btnLimpar.IsEnabled = true;

                btnCancelar.IsEnabled = true;

                btnAdicionar.IsEnabled = true;

                lblNumeroRecibo.Visibility = Visibility.Visible;

                GridValoresRecibo.IsEnabled = true;

                status = "alterar";

                grid1.Focus();

                //HabilitaTipoPagamento();

                switch (cmbTipoPagamento.SelectedIndex)
                {
                    case 0:
                        txtValorPagoDinheiro.IsEnabled = true;
                        txtValorPagoDeposito.IsEnabled = false;
                        txtValorPagoCheque.IsEnabled = false;
                        txtValorPagoChequePre.IsEnabled = false;
                        txtValorPagoBoleto.IsEnabled = false;
                        txtValorPagoCartaoCredito.IsEnabled = false;
                        txtValorPagoDinheiro.SelectAll();
                        break;
                    case 1:
                        txtValorPagoDinheiro.IsEnabled = false;
                        txtValorPagoDeposito.IsEnabled = true;
                        txtValorPagoCheque.IsEnabled = false;
                        txtValorPagoChequePre.IsEnabled = false;
                        txtValorPagoBoleto.IsEnabled = false;
                        txtValorPagoCartaoCredito.IsEnabled = false;
                        txtValorPagoDeposito.SelectAll();
                        break;
                    case 3:
                        txtValorPagoDinheiro.IsEnabled = false;
                        txtValorPagoDeposito.IsEnabled = false;
                        txtValorPagoCheque.IsEnabled = true;
                        txtValorPagoChequePre.IsEnabled = false;
                        txtValorPagoBoleto.IsEnabled = false;
                        txtValorPagoCartaoCredito.IsEnabled = false;
                        txtValorPagoCheque.SelectAll();
                        break;
                    case 4:
                        txtValorPagoDinheiro.IsEnabled = false;
                        txtValorPagoDeposito.IsEnabled = false;
                        txtValorPagoCheque.IsEnabled = false;
                        txtValorPagoChequePre.IsEnabled = true;
                        txtValorPagoBoleto.IsEnabled = false;
                        txtValorPagoCartaoCredito.IsEnabled = false;
                        txtValorPagoChequePre.SelectAll();
                        break;
                    case 5:
                        txtValorPagoDinheiro.IsEnabled = false;
                        txtValorPagoDeposito.IsEnabled = false;
                        txtValorPagoCheque.IsEnabled = false;
                        txtValorPagoChequePre.IsEnabled = false;
                        txtValorPagoBoleto.IsEnabled = true;
                        txtValorPagoCartaoCredito.IsEnabled = false;
                        txtValorPagoBoleto.SelectAll();
                        break;
                    case 6:
                        txtValorPagoDinheiro.IsEnabled = false;
                        txtValorPagoDeposito.IsEnabled = false;
                        txtValorPagoCheque.IsEnabled = false;
                        txtValorPagoChequePre.IsEnabled = false;
                        txtValorPagoBoleto.IsEnabled = false;
                        txtValorPagoCartaoCredito.IsEnabled = true;
                        txtValorPagoCartaoCredito.SelectAll();
                        break;
                    case 7:
                        txtValorPagoDinheiro.IsEnabled = true;
                        txtValorPagoDeposito.IsEnabled = true;
                        txtValorPagoCheque.IsEnabled = true;
                        txtValorPagoChequePre.IsEnabled = true;
                        txtValorPagoBoleto.IsEnabled = true;
                        txtValorPagoCartaoCredito.IsEnabled = true;
                        txtValorPagoDinheiro.Focus();
                        break;
                    default:
                        txtValorPagoDinheiro.IsEnabled = false;
                        txtValorPagoDeposito.IsEnabled = false;
                        txtValorPagoCheque.IsEnabled = false;
                        txtValorPagoChequePre.IsEnabled = false;
                        txtValorPagoBoleto.IsEnabled = false;
                        txtValorPagoCartaoCredito.IsEnabled = false;
                        break;
                }

                CalculaTroco();

                groupBoxDadosRecibo.IsEnabled = false;

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                    groupBoxDadosRecibo.IsEnabled = true;
                else
                    groupBoxDadosRecibo.IsEnabled = false;
            }
        }

        private void btnAdicionar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAdicionar.Content = "Adicionar";
        }

        private void btnAdicionar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAdicionar.Content = "";
        }

        private void btnCancelar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCancelar.Content = "Cancelar";
        }

        private void btnCancelar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCancelar.Content = "";
        }

        private void btnLimpar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnLimpar.Content = "Limpar";
        }

        private void btnLimpar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnLimpar.Content = "";
        }

        private void btnNovo_MouseEnter(object sender, MouseEventArgs e)
        {
            btnNovo.Content = "Novo";
        }

        private void btnNovo_MouseLeave(object sender, MouseEventArgs e)
        {
            btnNovo.Content = "";
        }

        private void txtSeloInicialAut_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSeloInicialAut.Select(3, 2);
        }

        private void txtSeloInicialAbert_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSeloInicialAbert.Select(3, 2);
        }

        private void txtSeloInicialRecAut_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSeloInicialRecAut.Select(3, 2);
        }

        private void txtSeloInicialRecSem_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSeloInicialRecSem.Select(3, 2);
        }

        private void txtRequisicao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F1)
                {
                    txtQtdAut.Focus();
                    txtQtdAut.Select(0, txtQtdAut.Text.Length);
                }

                if (e.Key == Key.F2)
                {
                    txtQtdAbert.Focus();
                    txtQtdAbert.Select(0, txtQtdAbert.Text.Length);
                }

                if (e.Key == Key.F3)
                {
                    txtQtdRecAut.Focus();
                    txtQtdAbert.Select(0, txtQtdRecAut.Text.Length);
                }

                if (e.Key == Key.F4)
                {
                    txtQtdRecSem.Focus();
                    txtQtdRecSem.Select(0, txtQtdRecSem.Text.Length);
                }

                if (e.Key == Key.F5)
                {
                    txtQtdMaterializacao.Focus();
                    txtQtdMaterializacao.Select(0, txtQtdMaterializacao.Text.Length);
                }

                if (e.Key == Key.C && !txtLetraSelo.Focus())
                {
                    txtQtdCopia.Focus();
                    txtQtdCopia.Select(0, txtQtdCopia.Text.Length);
                }

                if (e.Key == Key.F10)
                {
                    if (btnLimpar.IsEnabled == true)
                        btnLimpar_Click(sender, e);
                }

                if (e.Key == Key.F11)
                {
                    if (btnCancelar.IsEnabled == true)
                        btnCancelar_Click(sender, e);
                }

                if (e.Key == Key.F12)
                {
                    if (btnAdicionar.IsEnabled == true)
                        btnAdicionar_Click(sender, e);
                }

                if (e.Key == Key.F && !txtLetraSelo.Focus())
                {
                    if (btnFinalizar.IsEnabled == true)
                        btnFinalizar_Click(sender, e);
                }

                if (e.Key == Key.Home)
                {
                    txtLetraSelo.Focusable = true;
                    txtLetraSelo.Text = "";
                    txtLetraSelo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F9)
                {
                    if (btnNovo.IsEnabled == true)
                        btnNovo_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtLetraSelo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtLetraSelo.Text.Length == 4)
                {
                    classBalcao.AlterarSerieSelo(txtLetraSelo.Text);
                    txtLetraSelo.Focusable = false;
                }
                else
                {
                    MessageBox.Show("A Série deve conter 4 letras");
                    txtLetraSelo.Text = classBalcao.PegaLetraAtual();
                    txtLetraSelo.Focusable = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtLetraSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbMensalista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMensalista.SelectedIndex > -1)
            {
                try
                {
                    var mensalista = listaMensalista[cmbMensalista.SelectedIndex];

                    if (mensalista.Nome != "ORIENTE")
                    {
                        txtRequisicao.Text = classBalcao.ProximoNumeroRequisicao(mensalista.Nome).ToString();
                    }
                    else
                    {
                        txtRequisicao.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível obter o número da requisição automaticamente. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void CalcularValorMaterializacao()
        {
            try
            {
                lblTotalMaterializacao.Content = string.Format("{0:n2}", CalcularValores("materializacao", Convert.ToInt32(txtQtdMaterializacao.Text), 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtQtdMaterializacao_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdMaterializacao.Text == "0")
            {
                txtQtdMaterializacao.Text = "";
            }
            else
            {
                txtQtdMaterializacao.Select(0, txtQtdMaterializacao.Text.Length);
            }
        }

        private void txtQtdMaterializacao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdMaterializacao.Text == "" || txtQtdMaterializacao.Text == "0")
            {
                txtLetraSeloMaterializacao.Text = "";
                txtQtdMaterializacao.Text = "0";
                txtSeloInicialMaterializacao.Text = "";
                txtSeloFinalMaterializacao.Text = "";
                txtSeloInicialMaterializacao.IsReadOnly = true;
                CalcularValorMaterializacao();
            }
            else
            {
                try
                {
                    txtSeloInicialMaterializacao.IsReadOnly = false;

                    if (txtSeloInicialMaterializacao.Text == "")
                    {
                        txtLetraSeloMaterializacao.Text = txtLetraSelo.Text;
                        txtSeloInicialMaterializacao.Text = string.Format("{0:00000}", classBalcao.LetraSeloAtual().Numero);
                    }

                    if (txtSeloInicialMaterializacao.Text.Length == 5)
                    {
                        int numero = Convert.ToInt32(txtSeloInicialMaterializacao.Text);
                        txtSeloFinalMaterializacao.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdMaterializacao.Text) - 1));
                        CalcularValorMaterializacao();
                    }
                }
                catch (Exception)
                {

                }

            }
        }

        private void txtQtdMaterializacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtSeloInicialMaterializacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelosInicialFinal(txtSeloInicialMaterializacao, e);
        }

        private void txtSeloInicialMaterializacao_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSeloInicialMaterializacao.Select(3, 2);
        }

        private void txtSeloInicialMaterializacao_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxSelo_LostFocus(txtSeloInicialMaterializacao, e);

                if (txtSeloInicialMaterializacao.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialMaterializacao.Text);
                    txtSeloFinalMaterializacao.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdMaterializacao.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtSeloInicialMaterializacao_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtSeloInicialMaterializacao.Text.Length == 5)
                {
                    int numero = Convert.ToInt32(txtSeloInicialMaterializacao.Text);
                    txtSeloFinalMaterializacao.Text = string.Format("{0}{1:00000}", txtLetraSelo.Text, (numero + Convert.ToInt32(txtQtdMaterializacao.Text) - 1));
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtSeloFinalMaterializacao_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalMaterializacao.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalMaterializacao.Text.Substring(4, txtSeloFinalMaterializacao.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItemDinheiro_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("DINHEIRO");
        }

        private void MenuItemDeposito_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("DEPÓSITO");
        }

        private void txtSeloInicialRecAutDut_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSeloInicialRecAutDut.Select(3, 2);
        }

        private void txtSeloFinalRecAutDut_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtSeloFinalRecAutDut.Text.Length == 9)
                    classBalcao.SalvarUltimoSelo(Convert.ToInt32(txtSeloFinalRecAutDut.Text.Substring(4, txtSeloFinalRecAutDut.Text.Length - 4)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoDinheiro_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoDinheiro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPagoDinheiro.Text != "")
                {
                    try
                    {
                        txtValorPagoDinheiro.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPagoDinheiro.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPagoDinheiro.Text = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void txtValorPagoDinheiro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoDinheiro.Text == "0,00")
            {
                txtValorPagoDinheiro.Text = "";
            }
        }

        private void txtValorPagoDinheiro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPagoDinheiro.Text.Length > 0)
            {
                if (txtValorPagoDinheiro.Text.Contains(","))
                {
                    int index = txtValorPagoDinheiro.Text.IndexOf(",");

                    if (txtValorPagoDinheiro.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }


        private void txtValorPagoDeposito_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoDeposito_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPagoDeposito.Text != "")
                {
                    try
                    {
                        txtValorPagoDeposito.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPagoDeposito.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPagoDeposito.Text = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoDeposito_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPagoDeposito.Text.Length > 0)
            {
                if (txtValorPagoDeposito.Text.Contains(","))
                {
                    int index = txtValorPagoDeposito.Text.IndexOf(",");

                    if (txtValorPagoDeposito.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtValorPagoDeposito_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoDeposito.Text == "0,00")
            {
                txtValorPagoDeposito.Text = "";
            }
        }


        private void txtValorPagoCheque_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoCheque_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPagoCheque.Text != "")
                {
                    try
                    {
                        txtValorPagoCheque.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPagoCheque.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPagoCheque.Text = "0,00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoCheque_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPagoCheque.Text.Length > 0)
            {
                if (txtValorPagoCheque.Text.Contains(","))
                {
                    int index = txtValorPagoCheque.Text.IndexOf(",");

                    if (txtValorPagoCheque.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtValorPagoCheque_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoCheque.Text == "0,00")
            {
                txtValorPagoCheque.Text = "";
            }
        }

        private void txtValorPagoChequePre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoChequePre_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPagoChequePre.Text != "")
                {
                    try
                    {
                        txtValorPagoChequePre.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPagoChequePre.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPagoChequePre.Text = "0,00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoChequePre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPagoChequePre.Text.Length > 0)
            {
                if (txtValorPagoChequePre.Text.Contains(","))
                {
                    int index = txtValorPagoChequePre.Text.IndexOf(",");

                    if (txtValorPagoChequePre.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtValorPagoChequePre_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoChequePre.Text == "0,00")
            {
                txtValorPagoChequePre.Text = "";
            }
        }


        private void txtValorPagoBoleto_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoBoleto_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPagoBoleto.Text != "")
                {
                    try
                    {
                        txtValorPagoBoleto.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPagoBoleto.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPagoBoleto.Text = "0,00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoBoleto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPagoBoleto.Text.Length > 0)
            {
                if (txtValorPagoBoleto.Text.Contains(","))
                {
                    int index = txtValorPagoBoleto.Text.IndexOf(",");

                    if (txtValorPagoBoleto.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtValorPagoBoleto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoBoleto.Text == "0,00")
            {
                txtValorPagoBoleto.Text = "";
            }
        }

        private void gridPgMisto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }

        }

        private void MenuItemCheque_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("CHEQUE");
        }


        private void AlterarTipoPagamento(string tipoPagamento)
        {
            var recibo = ((ReciboBalcao)dataGridReciboBalcao.SelectedItem);

            classBalcao.MudarTipoPagamento(recibo.NumeroRecibo, tipoPagamento);

            dataGridReciboBalcao.Items.Refresh();

            var pago = new ValorPago();

            ClassAto classAto = new ClassAto();

            pago = classAto.ObterValorPagoPorIdReciboBalcao(recibo.IdReciboBalcao);

            if (pago != null)
            {
                pago.Mensalista = 0M;

                if (tipoPagamento == "DINHEIRO")
                    pago.Dinheiro = pago.Total;
                else
                    pago.Dinheiro = 0M;

                if (tipoPagamento == "DEPÓSITO")
                    pago.Deposito = pago.Total;
                else
                    pago.Deposito = 0M;

                if (tipoPagamento == "CHEQUE")
                    pago.Cheque = pago.Total;
                else
                    pago.Cheque = 0M;

                if (tipoPagamento == "PIX BRADESCO")
                    pago.ChequePre = pago.Total;
                else
                    pago.ChequePre = 0M;

                if (tipoPagamento == "PIX NUBANK")
                    pago.Boleto = pago.Total;
                else
                    pago.Boleto = 0M;

                if (tipoPagamento == "CARTÃO CRÉDITO")
                    pago.CartaoCredito = pago.Total;
                else
                    pago.CartaoCredito = 0M;

                classAto.SalvarValorPago(pago, "alterar", "IdReciboBalcao");
            }

            VerificaMenu();
        }

        private void MenuItemChequePre_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("PIX BRADESCO");
        }

        private void MenuItemBoleto_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("PIX NUBANK");
        }


        private void listView_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnConsultaDuplicado_Click(object sender, RoutedEventArgs e)
        {
            ConsultarSelosDuplicados();
        }

        private void ConsultarSelosDuplicados()
        {
            try
            {
                if (datePickerDataConsultaDuplicado.SelectedDate != null)
                {
                    using (SqlConnection con = new SqlConnection(CS_Caixa.Properties.Settings.Default.CS_CAIXA_DBConnectionString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("SELECT LetraSelo, NumeroSelo, ReciboBalcao, TipoAto, Escrevente, Count(*) as qtd FROM ato where atribuicao = 'BALCÃO' AND dataato = '" + datePickerDataConsultaDuplicado.SelectedDate.Value.ToShortDateString() + "' GROUP BY LetraSelo, NumeroSelo, ReciboBalcao, TipoAto, Escrevente HAVING COUNT(NumeroSelo) > 1;", con))
                        {
                            cmd.CommandType = CommandType.Text;

                            SqlDataReader dr = cmd.ExecuteReader();

                            DataTable dt = new DataTable();

                            dt.Load(dr);

                            dataGridDuplicado.ItemsSource = dt.DefaultView;
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void MenuItemImprimirRecibo_Click(object sender, RoutedEventArgs e)
        {
            var imprimirRecibo = new WinImprimirRecibo(reciboBalcao, listaSelos);
            imprimirRecibo.Owner = this;
            imprimirRecibo.ShowDialog();
        }

        private void txtValorPagoCartaoCredito_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoCartaoCredito_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPagoCartaoCredito.Text != "")
                {
                    try
                    {
                        txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPagoCartaoCredito.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPagoCartaoCredito.Text = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPagoCartaoCredito_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPagoCartaoCredito.Text.Length > 0)
            {
                if (txtValorPagoCartaoCredito.Text.Contains(","))
                {
                    int index = txtValorPagoCartaoCredito.Text.IndexOf(",");

                    if (txtValorPagoCartaoCredito.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtValorPagoCartaoCredito_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoCartaoCredito.Text == "0,00")
            {
                txtValorPagoCartaoCredito.Text = "";
            }
        }

        private void MenuItemCartaoCredito_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("CARTÃO CRÉDITO");
        }

    }
}