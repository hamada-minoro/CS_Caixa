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
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinDigitarAtoRgi.xaml
    /// </summary>
    public partial class WinDigitarAtoRgi : Window
    {
        WinPrincipal Principal;
        Usuario usuarioLogado;
        string status;
        ClassMensalista mensalistas = new ClassMensalista();
        ClassCustasRgi ClassCustasRgi = new ClassCustasRgi();
        ClassAto classAto = new ClassAto();
        List<CustasRgi> custasRgi = new List<CustasRgi>();
        public List<CustasRgi> itensCustasRgi = new List<CustasRgi>();
        public ItensAtoRgi atoAlterar;
        List<Usuario> listaNomes = new List<Usuario>();
        decimal emolumentoPercente = 0;




        int anoAtual = DateTime.Now.Date.Year;
        public CustasRgi emolumentos = new CustasRgi();
        public CustasRgi emolumentosMetade = new CustasRgi();

        // lista de itens para calcular os valores que serao passados para a listaItensCustas no momento que for adicionado para o Grid
        public List<ItensCustasRgi> listaItensCalculo = new List<ItensCustasRgi>();
        public List<CustasDistribuicao> listaDistribuicao = new List<CustasDistribuicao>();

        // ITENS A SEREM SALVOS
        public List<ItensCustasRgi> listaItensCustas = new List<ItensCustasRgi>();
        List<ItensAtoRgi> listaItensAtos = new List<ItensAtoRgi>();
        Ato Ato = new Ato();
        Ato atoSelecionado;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        WinAtosRgi _atosRgi;

        public decimal mutua = 0;
        public decimal acoterj = 0;
        decimal prenotacao = 0;
        public decimal indisponibilidade = 0;
        public decimal porcentagemIss;

        string totalInicial = string.Empty;
        bool masterParaSalvar = false;
        public bool senhaConfirmada = false;
        DateTime dataAto = new DateTime();
        DateTime dataPagamento = new DateTime();

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();



        public WinDigitarAtoRgi(WinPrincipal Principal, Usuario usuarioLogado, string status, Ato atoSelecionado)
        {
            InitializeComponent();
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            this.atoSelecionado = atoSelecionado;
            grid1.IsEnabled = false;
            this.dataInicioConsulta = atoSelecionado.DataAto;
            this.dataFimConsulta = atoSelecionado.DataAto;
        }

        public WinDigitarAtoRgi(WinPrincipal Principal, Usuario usuarioLogado, string status, WinAtosRgi atosRgi, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            this.atoSelecionado = atosRgi.atoSelecionado;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            _atosRgi = atosRgi;
            InitializeComponent();
        }


        bool ativo = true;


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (Convert.ToDecimal(lblTroco.Content) > 0M)
            {
                if (ativo == true)
                {
                    label39.Foreground = new SolidColorBrush(Colors.Red);
                    lblTroco.Foreground = new SolidColorBrush(Colors.Red);
                    ativo = false;
                }
                else
                {
                    label39.Foreground = new SolidColorBrush(Colors.Transparent);
                    lblTroco.Foreground = new SolidColorBrush(Colors.Transparent);
                    ativo = true;
                }
            }
            else
            {
                label39.Foreground = new SolidColorBrush(Colors.Red);
                lblTroco.Foreground = new SolidColorBrush(Colors.Red);
            }

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrimeiroCarregamento();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();

        }

        /// <summary>
        /// PRIMEIRO CARREGAMENTO DO FORM
        /// </summary>
        private void PrimeiroCarregamento()
        {
            // TITULO DO FORM
            this.Title = Principal.TipoAto;

            // DATA DE PAGAMENTO E ATO
            datePickerDataAto.SelectedDate = DateTime.Now.Date;
            datePickerDataPagamento.SelectedDate = DateTime.Now.Date;


            // COR DO FORM
            var bc = new BrushConverter();

            if (Principal.TipoAto == "REGISTRO")
                this.Background = (Brush)bc.ConvertFrom("#FFD3E9EF");

            if (Principal.TipoAto == "AVERBAÇÃO")
                this.Background = (Brush)bc.ConvertFrom("#FFE2E0BA");

            if (Principal.TipoAto == "CERTIDÃO RGI")
                this.Background = (Brush)bc.ConvertFrom("#FF7FD3C0");



            // PREENCHER O COMBO BOX DO NOME USUARIO
            ClassUsuario carregaNomesUsuarios = new ClassUsuario();
            listaNomes = carregaNomesUsuarios.ListaUsuarios();
            cmbNomes.ItemsSource = listaNomes.Select(p => p.NomeUsu);
            cmbNomes.SelectedItem = usuarioLogado.NomeUsu;




            // CARREGA CUSTAS RGI ANO
            custasRgi = ClassCustasRgi.ListaCustas().ToList();
            List<CustasRgi> atulizado = custasRgi.Where(p => p.ANO == anoAtual).ToList();
            if (status == "novo")
            {
                if (atulizado.Count > 0)
                {
                    if (DateTime.Now.Month == 1 && DateTime.Now.Day < 3)
                    {
                        if (MessageBox.Show("Deseja utilizar as custas do ano passado?", "Custas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            anoAtual = custasRgi.Max(p => p.ANO).Value - 1;
                        }
                    }
                }
                else
                {
                    anoAtual = custasRgi.Max(p => p.ANO).Value;

                }
            }
            if (status == "alterar")
            {

                if (DateTime.Now.Year != atoSelecionado.DataPagamento.Year)
                {
                    if (MessageBox.Show("Deseja utilizar as custas do ano deste ato?", "Custas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        anoAtual = atoSelecionado.DataPagamento.Year;
                    }

                }



                listaItensAtos = classAto.CarregaGridItensAto(atoSelecionado.Id_Ato);
                dataGridItensAtoRgi.ItemsSource = listaItensAtos;
                dataGridItensAtoRgi.Items.Refresh();


                if (listaItensAtos.Count > 0)
                    cont = Convert.ToInt32(listaItensAtos.Max(p => p.Cont + 1));

                if (listaItensAtos.Count > 0)
                {
                    cmbPrenotacao.IsEnabled = false;

                    cmbTipoCustas.IsEnabled = false;

                    cmbTipoPagamento.IsEnabled = false;

                    btnSalvar.IsEnabled = true;
                }

                for (int i = 0; i < listaItensAtos.Count; i++)
                {
                    List<ItensCustasRgi> itensAdicionar = new List<ItensCustasRgi>();
                    int idAtoRgi = listaItensAtos[i].Id_AtoRgi;
                    itensAdicionar = classAto.CarregaItensCustasAlterar(idAtoRgi);


                    for (int c = 0; c < itensAdicionar.Count; c++)
                    {
                        listaItensCustas.Add(itensAdicionar[c]);
                    }
                }

                CarregaAlteracao();
            }

            custasRgi = custasRgi.Where(p => p.ANO == anoAtual).ToList();
            itensCustasRgi = custasRgi.Where(p => p.TIPO == "I" && p.ANO == anoAtual).ToList();

            mutua = Convert.ToDecimal(custasRgi.Where(p => p.DESCR == "MUTUA").Select(p => p.VALOR).FirstOrDefault());
            acoterj = Convert.ToDecimal(custasRgi.Where(p => p.DESCR == "ACOTERJ").Select(p => p.VALOR).FirstOrDefault());
            listaDistribuicao = ClassCustasRgi.ListaCustasDistribuicao();
            listaDistribuicao = listaDistribuicao.Where(p => p.Ano == anoAtual).OrderBy(p => p.Quant_Exced).ToList();
            prenotacao = Convert.ToDecimal(custasRgi.Where(p => p.DESCR == "PRENOTAÇÃO").Select(p => p.VALOR).FirstOrDefault());
            indisponibilidade = Convert.ToDecimal(custasRgi.Where(p => p.TEXTO == "INDISPONIBILIDADE").Select(p => p.VALOR).FirstOrDefault());
            porcentagemIss = Convert.ToDecimal(custasRgi.Where(p => p.DESCR == "PORCENTAGEM ISS").Select(p => p.VALOR).FirstOrDefault());



            if (Principal.TipoAto == "REGISTRO")
            {

                custasRgi = custasRgi.Where(p => p.TIPO == "R" && p.ANO == anoAtual).ToList();
                cmbNatureza.ItemsSource = custasRgi.Select(p => p.DESCR);

                groupBox1.Header = "Dados do Registro";
            }

            if (Principal.TipoAto == "AVERBAÇÃO")
            {
                custasRgi = custasRgi.Where(p => p.TIPO == "A" && p.ANO == anoAtual).ToList();
                cmbNatureza.ItemsSource = custasRgi.Select(p => p.DESCR);
                cmbPrenotacao.Items.Add("SEM PRENOTAÇÃO");
                cmbPrenotacao.SelectedIndex = 3;
                groupBox1.Header = "Dados da Averbação";
            }
            if (Principal.TipoAto == "CERTIDÃO RGI")
            {
                custasRgi = custasRgi.Where(p => p.TIPO == "C" && p.ANO == anoAtual).ToList();
                cmbNatureza.ItemsSource = custasRgi.Select(p => p.DESCR);
                cmbPrenotacao.Items.Add("SEM PRENOTAÇÃO");
                cmbPrenotacao.SelectedIndex = 3;
                cmbPrenotacao.IsEnabled = false;
                groupBox1.Header = "Dados da Certidão";
                txtQtdDistrib.IsEnabled = false;
                txtQtdExced.IsEnabled = false;
                txtQtdIndisp.IsEnabled = false;
                lblProtocoloRecibo.Content = "Recibo:";
            }


            // PERMISSAO OU NÃO PARA USUARIO 
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                cmbNomes.IsEnabled = true;
                checkBoxPago.IsEnabled = true;
            }
            else
            {
                cmbNomes.IsEnabled = false;
                checkBoxPago.IsEnabled = false;
            }

        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CarregaAlteracao()
        {
            datePickerDataAto.SelectedDate = atoSelecionado.DataAto;
            txtProtocolo.Text = string.Format("{0}", atoSelecionado.Protocolo);
            txtAdicionar.Text = string.Format("{0:n2}", atoSelecionado.ValorAdicionar);
            txtDesconto.Text = string.Format("{0:n2}", atoSelecionado.ValorDesconto);
            cmbNomes.Text = atoSelecionado.Escrevente;
            cmbTipoCustas.Text = atoSelecionado.TipoCobranca;
            cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;

            if (Principal.TipoAto == "CERTIDÃO RGI")
                txtProtocolo.Text = atoSelecionado.Recibo.ToString();

            if (cmbTipoPagamento.SelectedIndex == 2)
            {

                txtRequisicao.IsEnabled = true;
                cmbMensalista.ItemsSource = mensalistas.ListaMensalistas().Select(p => p.Nome);
                txtRequisicao.IsEnabled = false;
            }

            cmbMensalista.Text = atoSelecionado.Mensalista;
            txtRequisicao.Text = string.Format("{0}", atoSelecionado.NumeroRequisicao);
            txtQtdIndisp.Text = string.Format("{0}", atoSelecionado.QuantIndisp);
            cmbPrenotacao.Text = atoSelecionado.TipoPrenotacao;
            txtPrenotacao.Text = string.Format("{0:n2}", atoSelecionado.Prenotacao);
            txtIndisp.Text = string.Format("{0:n2}", atoSelecionado.Indisponibilidade);
            datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;
            txtEmolTotal.Text = string.Format("{0:n2}", atoSelecionado.Emolumentos);
            txtFetjTotal.Text = string.Format("{0:n2}", atoSelecionado.Fetj);
            txtFundperjTotal.Text = string.Format("{0:n2}", atoSelecionado.Fundperj);
            txtFunperjTotal.Text = string.Format("{0:n2}", atoSelecionado.Funperj);
            txtFunarpenTotal.Text = string.Format("{0:n2}", atoSelecionado.Funarpen);
            txtIssTotal.Text = string.Format("{0:n2}", atoSelecionado.Iss);
            txtPmcmvTotal.Text = string.Format("{0:n2}", atoSelecionado.Pmcmv);
            txtMutuaTotal.Text = string.Format("{0:n2}", atoSelecionado.Mutua);
            txtAcoterjTotal.Text = string.Format("{0:n2}", atoSelecionado.Acoterj);
            txtDistribuicaoTotal.Text = string.Format("{0:n2}", atoSelecionado.Distribuicao);
            txtTotalTotal.Text = string.Format("{0:n2}", atoSelecionado.Total);

            checkBoxPago.IsChecked = atoSelecionado.Pago;
            ClassAto classAto = new ClassAto();

            var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);

            if (valorPago != null)
            {
                txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
            }

            totalInicial = string.Format("{0:n2}", atoSelecionado.Total);
            masterParaSalvar = false;
            dataAto = atoSelecionado.DataAto;
            dataPagamento = atoSelecionado.DataPagamento;
        }

        private void groupBox1_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void tabControl1_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void cmbTipoPagamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoPagamento.Focus())
            {
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    cmbMensalista.IsEnabled = true;
                    txtRequisicao.IsEnabled = true;
                    cmbMensalista.ItemsSource = mensalistas.ListaMensalistas().Select(p => p.Nome);
                }
                else
                {
                    cmbMensalista.SelectedIndex = -1;
                    txtRequisicao.Text = "";
                    cmbMensalista.IsEnabled = false;
                    txtRequisicao.IsEnabled = false;
                }

                HabilitaTipoPagamento();
            }

        }

        private void txtFlsInicial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtFlsFinal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtNumeroSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdIndisp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void QtdExced_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtRequisicao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
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
            if (txtDesconto.Text == "")
                txtDesconto.Text = "0,00";
            else
                txtDesconto.Text = string.Format("{0:n2}", Convert.ToDecimal(txtDesconto.Text));

            CalcularTotalValoresEmolGeral();

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
            if (txtAdicionar.Text == "")
                txtAdicionar.Text = "0,00";
            else
                txtAdicionar.Text = string.Format("{0:n2}", Convert.ToDecimal(txtAdicionar.Text));

            CalcularTotalValoresEmolGeral();

        }


        private void Window_Closed(object sender, EventArgs e)
        {

            //WinAtosRgi rgi = new WinAtosRgi(usuarioLogado, Principal, atoSelecionado, dataInicioConsulta, dataFimConsulta);
            //rgi.Owner = Principal;
            //rgi.ShowDialog();
        }

        private void cmbNatureza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNatureza.Focus() == true)
            {
                CarregarItensNaListaCustas();
                CalcularValoresEmolumentos();

                if (cmbNatureza.SelectedIndex > -1)
                {
                    btnCustas.IsEnabled = true;
                    btnAdicionar.IsEnabled = true;
                }
                else
                {
                    btnCustas.IsEnabled = false;
                    btnAdicionar.IsEnabled = false;
                }

                HabilitaTipoPagamento();
            }
        }

        /// <summary>
        /// CALCULAR OS VALORES DE CUSTAS APARTIR DOS EMOLUMENTOS DA LISTA CALCULAR
        /// </summary>
        private void CalcularValoresEmolumentos()
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss = 0;

            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Spmcmv_2 = "0,00";
            string Siss = "0,00";
            int index;



            emol = Convert.ToDecimal(listaItensCalculo.Sum(p => p.Total));


            try
            {
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    //iss = (100 - porcentagemIss) / 100;
                    //iss = emol / iss - emol;

                    iss = emol * porcentagemIss / 100;

                    pmcmv_2 = Convert.ToDecimal(emolumentoPercente * 2) / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                        Semol = Convert.ToString(emol);

                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    pmcmv_2 = 0;
                    iss = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Spmcmv_2 = "0,00";
                    Siss = "0,00";
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


                index = Spmcmv_2.IndexOf(',');
                Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                if (cmbPrenotacao.SelectedIndex <= 1 || cmbPrenotacao.SelectedIndex == 3)
                {
                    txtEmol.Text = Semol;
                    txtFetj.Text = Sfetj_20;
                    txtFundperj.Text = Sfundperj_5;
                    txtFunperj.Text = Sfunperj_5;
                    txtFunarpen.Text = Sfunarpen_4;
                    txtPmcmv.Text = Spmcmv_2;
                    txtIss.Text = Siss;
                    if (cmbTipoCustas.SelectedIndex <= 1)
                    {
                        if (Principal.TipoAto != "CERTIDÃO RGI")
                        {
                            txtMutua.Text = string.Format("{0:n2}", mutua);
                            txtAcoterj.Text = string.Format("{0:n2}", acoterj);
                        }
                        else
                        {
                            txtMutua.Text = "0,00";
                            txtAcoterj.Text = "0,00";
                        }
                    }
                    else
                    {
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }
                }
                else
                {
                    txtEmol.Text = Semol;
                    txtFetj.Text = "0,00";
                    txtFundperj.Text = "0,00";
                    txtFunperj.Text = "0,00";
                    txtFunarpen.Text = "0,00";
                    txtPmcmv.Text = "0,00";
                    txtIss.Text = "0,00";
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }

                CalcularPrenotacao();
                CalcularDistribuicao();
                CalcularIndisponibilidade();
                CalcularTotalValoresEmol();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        /// <summary>
        /// CALCULAR OS VALORES DE CUSTAS APARTIR DOS EMOLUMENTOS DA LISTA CALCULAR DEPOIS QUE ALTERAR
        /// </summary>
        private ItensAtoRgi CalcularValoresEmolumentosAlterar(ItensAtoRgi itensListaAtoAlterar)
        {

            ItensAtoRgi itensListaAtoAlterado = new ItensAtoRgi();
            itensListaAtoAlterado = itensListaAtoAlterar;

            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss = 0;

            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Spmcmv_2 = "0,00";
            string Siss = "0,00";
            int index;



            emol = Convert.ToDecimal(listaItensCalculo.Sum(p => p.Total));

            try
            {
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;
                    pmcmv_2 = Convert.ToDecimal(emolumentoPercente * 2) / 100;


                    //iss = (100 - porcentagemIss) / 100;
                    //iss = emol / iss - emol;

                    iss = emol * porcentagemIss / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                        Semol = Convert.ToString(emol);

                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    pmcmv_2 = 0;
                    iss = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Spmcmv_2 = "0,00";
                    Siss = "0,00";
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


                index = Spmcmv_2.IndexOf(',');
                Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                if (cmbPrenotacao.SelectedIndex <= 1 || cmbPrenotacao.SelectedIndex == 3)
                {
                    itensListaAtoAlterado.Emolumentos = Convert.ToDecimal(Semol);
                    itensListaAtoAlterado.Fetj = Convert.ToDecimal(Sfetj_20);
                    itensListaAtoAlterado.Fundperj = Convert.ToDecimal(Sfundperj_5);
                    itensListaAtoAlterado.Funperj = Convert.ToDecimal(Sfunperj_5);
                    itensListaAtoAlterado.Funarpen = Convert.ToDecimal(Sfunarpen_4);
                    itensListaAtoAlterado.Pmcmv = Convert.ToDecimal(Spmcmv_2);
                    itensListaAtoAlterado.Iss = Convert.ToDecimal(Siss);
                    if (cmbTipoCustas.SelectedIndex <= 1)
                    {
                        if (Principal.TipoAto != "CERTIDÃO RGI")
                        {
                            itensListaAtoAlterado.Mutua = mutua;
                            itensListaAtoAlterado.Acoterj = acoterj;
                        }
                        else
                        {
                            itensListaAtoAlterado.Mutua = 0;
                            itensListaAtoAlterado.Acoterj = 0;
                        }
                    }
                    else
                    {
                        itensListaAtoAlterado.Mutua = 0;
                        itensListaAtoAlterado.Acoterj = 0;
                    }
                }
                else
                {
                    itensListaAtoAlterado.Emolumentos = Convert.ToDecimal(Semol);
                    itensListaAtoAlterado.Fetj = 0;
                    itensListaAtoAlterado.Fundperj = 0;
                    itensListaAtoAlterado.Funperj = 0;
                    itensListaAtoAlterado.Funarpen = 0;
                    itensListaAtoAlterado.Pmcmv = 0;
                    itensListaAtoAlterado.Iss = 0;
                    itensListaAtoAlterado.Mutua = 0;
                    itensListaAtoAlterado.Acoterj = 0;
                }

                itensListaAtoAlterado.Total = itensListaAtoAlterado.Emolumentos + itensListaAtoAlterado.Fetj + itensListaAtoAlterado.Fundperj + itensListaAtoAlterado.Funperj + itensListaAtoAlterado.Funarpen + itensListaAtoAlterado.Pmcmv + itensListaAtoAlterado.Iss + itensListaAtoAlterado.Mutua + itensListaAtoAlterado.Acoterj + itensListaAtoAlterado.Distribuicao;

                return itensListaAtoAlterado;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }



        /// <summary>
        /// CALCULAR VALOR PRENOTAÇÃO 25% OU 50%
        /// </summary>
        private void CalcularPrenotacao()
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss = 0;

            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Spmcmv_2 = "0,00";
            string Siss = "0,00";
            int index;



            emol = prenotacao;

            try
            {
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    if (cmbPrenotacao.SelectedIndex == 1)
                    {
                        emol = emol * 75 / 100;
                    }

                    if (cmbPrenotacao.SelectedIndex == 2)
                    {
                        emol = emol / 2;
                    }

                    if (cmbPrenotacao.SelectedIndex < 2)
                    {
                        fetj_20 = emol * 20 / 100;
                        fundperj_5 = emol * 5 / 100;
                        funperj_5 = emol * 5 / 100;
                        funarpen_4 = emol * 4 / 100;
                        pmcmv_2 = emol * 2 / 100;

                        //iss = (100 - porcentagemIss) / 100;
                        //iss = emol / iss - emol;

                        iss = emol * porcentagemIss / 100;
                    }

                    if (cmbTipoCustas.SelectedIndex == 0)
                        Semol = Convert.ToString(emol);

                    if (cmbPrenotacao.SelectedIndex < 2)
                    {
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);
                        Siss = Convert.ToString(iss);
                    }
                }

                if (cmbTipoCustas.SelectedIndex > 1 || cmbPrenotacao.SelectedIndex == 3)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    pmcmv_2 = 0;
                    iss = 0;


                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Spmcmv_2 = "0,00";
                    Siss = "0,00";
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


                index = Spmcmv_2.IndexOf(',');
                Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);


                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                emol = Convert.ToDecimal(Semol);

                fetj_20 = Convert.ToDecimal(Sfetj_20);
                fundperj_5 = Convert.ToDecimal(Sfundperj_5);
                funperj_5 = Convert.ToDecimal(Sfunperj_5);
                funarpen_4 = Convert.ToDecimal(Sfunarpen_4);
                pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
                iss = Convert.ToDecimal(Siss);

                decimal total = emol + fetj_20 + fundperj_5 + funperj_5 + funarpen_4 + pmcmv_2 + iss;

                txtPrenotacao.Text = string.Format("{0:n2}", total);
                CalcularTotalValoresEmolGeral();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// CARREGAR ITENS PARA A LISTA DE CALCULO
        /// </summary>
        public void CarregarItensNaListaCustas()
        {
            if (cmbNatureza.SelectedIndex > -1)
            {
                if (Principal.TipoAto == "REGISTRO")
                {
                    try
                    {
                        emolumentos = (CustasRgi)custasRgi[cmbNatureza.SelectedIndex];
                        listaItensCalculo = new List<ItensCustasRgi>();
                        ItensCustasRgi novoIten = new ItensCustasRgi();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(emolumentos.VALOR), 1));
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItensCalculo.Add(novoIten);

                        emolumentoPercente = Convert.ToDecimal(novoIten.Valor);

                        novoIten = new ItensCustasRgi();
                        var arqrivDesarquiv = (CustasRgi)itensCustasRgi.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").FirstOrDefault();
                        novoIten.Item = arqrivDesarquiv.ITEM;
                        novoIten.SubItem = arqrivDesarquiv.SUB;
                        novoIten.Tabela = arqrivDesarquiv.TAB;
                        novoIten.Descricao = arqrivDesarquiv.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(arqrivDesarquiv.VALOR), 1));
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItensCalculo.Add(novoIten);


                        novoIten = new ItensCustasRgi();
                        var expedicao_emissao = (CustasRgi)itensCustasRgi.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES").FirstOrDefault();
                        novoIten.Item = expedicao_emissao.ITEM;
                        novoIten.SubItem = expedicao_emissao.SUB;
                        novoIten.Tabela = expedicao_emissao.TAB;
                        novoIten.Descricao = expedicao_emissao.TEXTO;
                        novoIten.Quantidade = "3";
                        novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(expedicao_emissao.VALOR), 1));
                        novoIten.Total = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(expedicao_emissao.VALOR), 3));
                        listaItensCalculo.Add(novoIten);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


                if (Principal.TipoAto == "AVERBAÇÃO")
                {
                    try
                    {
                        emolumentos = (CustasRgi)custasRgi[cmbNatureza.SelectedIndex];
                        listaItensCalculo = new List<ItensCustasRgi>();
                        ItensCustasRgi novoIten = new ItensCustasRgi();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(emolumentos.VALOR), 1));
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItensCalculo.Add(novoIten);

                        emolumentoPercente = Convert.ToDecimal(novoIten.Valor);

                        novoIten = new ItensCustasRgi();
                        var arqrivDesarquiv = (CustasRgi)itensCustasRgi.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").FirstOrDefault();
                        novoIten.Item = arqrivDesarquiv.ITEM;
                        novoIten.SubItem = arqrivDesarquiv.SUB;
                        novoIten.Tabela = arqrivDesarquiv.TAB;
                        novoIten.Descricao = arqrivDesarquiv.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(arqrivDesarquiv.VALOR), 1));
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItensCalculo.Add(novoIten);


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (Principal.TipoAto == "CERTIDÃO RGI")
                {
                    try
                    {


                        emolumentos = (CustasRgi)custasRgi[cmbNatureza.SelectedIndex];
                        if (cmbNatureza.SelectedIndex == 1)
                        {
                            emolumentos = (CustasRgi)custasRgi[0];
                            emolumentosMetade = (CustasRgi)custasRgi[1];
                            emolumentosMetade.VALOR = emolumentos.VALOR / 2;

                            listaItensCalculo = new List<ItensCustasRgi>();
                            ItensCustasRgi novoIten = new ItensCustasRgi();
                            novoIten.Item = emolumentosMetade.ITEM;
                            novoIten.SubItem = emolumentosMetade.SUB;
                            novoIten.Tabela = emolumentosMetade.TAB;
                            novoIten.Descricao = emolumentosMetade.TEXTO;
                            novoIten.Quantidade = "1";
                            novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(emolumentosMetade.VALOR), 1));
                            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                            listaItensCalculo.Add(novoIten);

                            emolumentoPercente = Convert.ToDecimal(novoIten.Valor);
                        }
                        else
                        {
                            listaItensCalculo = new List<ItensCustasRgi>();
                            ItensCustasRgi novoIten = new ItensCustasRgi();
                            novoIten.Item = emolumentos.ITEM;
                            novoIten.SubItem = emolumentos.SUB;
                            novoIten.Tabela = emolumentos.TAB;
                            novoIten.Descricao = emolumentos.TEXTO;
                            novoIten.Quantidade = "1";
                            novoIten.Valor = Convert.ToDecimal(CalcularItensPorcentagem(Convert.ToDecimal(emolumentos.VALOR), 1));
                            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                            listaItensCalculo.Add(novoIten);

                            emolumentoPercente = Convert.ToDecimal(novoIten.Valor);
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// CALCULAR TODOS OS VALORES PARA ADICIONAR NO GRID
        /// </summary>
        private void CalcularTotalValoresEmol()
        {
            decimal Total;


            try
            {
                Total = Convert.ToDecimal(txtEmol.Text) + Convert.ToDecimal(txtFetj.Text) + Convert.ToDecimal(txtFundperj.Text) + Convert.ToDecimal(txtFunperj.Text) + Convert.ToDecimal(txtFunarpen.Text) + Convert.ToDecimal(txtPmcmv.Text) + Convert.ToDecimal(txtIss.Text) + Convert.ToDecimal(txtMutua.Text) + Convert.ToDecimal(txtAcoterj.Text) + Convert.ToDecimal(txtDistribuicao.Text);
                txtTotal.Text = string.Format("{0:n2}", Total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }




        /// <summary>
        /// CALCULAR TODOS OS VALORES TROCO
        /// </summary>
        private void CalcularTotalValoresEmolGeral()
        {

            txtEmolTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Emolumentos));
            txtFetjTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Fetj));
            txtFundperjTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Fundperj));
            txtFunperjTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Funperj));
            txtFunarpenTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Funarpen));
            txtPmcmvTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Pmcmv));
            txtIssTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Iss));
            if (Principal.TipoAto != "CERTIDÃO RGI")
            {
                txtMutuaTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Mutua));
                txtAcoterjTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Acoterj));
                txtDistribuicaoTotal.Text = string.Format("{0:n2}", listaItensAtos.Sum(p => p.Distribuicao));
            }
            else
            {
                txtMutuaTotal.Text = string.Format("{0:n2}", 0);
                txtAcoterjTotal.Text = string.Format("{0:n2}", 0);
                txtDistribuicaoTotal.Text = string.Format("{0:n2}", 0);
            }
            decimal Total = 0;


            try
            {
                Total = Convert.ToDecimal(txtEmolTotal.Text) + Convert.ToDecimal(txtFetjTotal.Text) + Convert.ToDecimal(txtFundperjTotal.Text) + Convert.ToDecimal(txtFunperjTotal.Text) + Convert.ToDecimal(txtFunarpenTotal.Text) + Convert.ToDecimal(txtPmcmvTotal.Text) + Convert.ToDecimal(txtIssTotal.Text) + Convert.ToDecimal(txtMutuaTotal.Text) + Convert.ToDecimal(txtAcoterjTotal.Text) + Convert.ToDecimal(txtDistribuicaoTotal.Text) + Convert.ToDecimal(txtIndisp.Text) + Convert.ToDecimal(txtPrenotacao.Text) + Convert.ToDecimal(txtAdicionar.Text) - Convert.ToDecimal(txtDesconto.Text);
                txtTotalTotal.Text = string.Format("{0:n2}", Total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        /// <summary>
        /// CALCULAR VALOR PORCENTAGEM PARA CADA ITEM (25% E 50%)
        /// </summary>
        /// <param name="valor">VALOR DO ITEM PARA CALCULAR</param>
        /// <returns>VALOR CALCULADO</returns>
        private decimal CalcularItensPorcentagem(decimal valor, int qtd)
        {
            decimal emol = 0;
            //int index;

            string Semol = "0,00";
            try
            {
                if (cmbPrenotacao.SelectedIndex == 0 || cmbPrenotacao.SelectedIndex == 3)
                {
                    emol = valor;
                }

                if (cmbPrenotacao.SelectedIndex == 1)
                {

                    emol = valor * 75 / 100;
                }

                if (cmbPrenotacao.SelectedIndex == 2)
                {
                    emol = valor / 2;
                }

                emol = emol * qtd;
                Semol = Convert.ToString(emol);

                //index = Semol.IndexOf(',');
                //Semol = Semol.Substring(0, index + 3);

                emol = Convert.ToDecimal(Semol);

                return emol;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

        }


        private void txtQtdIndisp_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtQtdIndisp.Text == "")
            {
                txtQtdIndisp.Text = "0";
            }

            CalcularIndisponibilidade();
            CalcularTotalValoresEmolGeral();

        }


        /// <summary>
        /// CALCULAR INDISPONIBILIDADE
        /// </summary>
        private void CalcularIndisponibilidade()
        {
            if (cmbTipoCustas.SelectedIndex <= 1)
                txtIndisp.Text = string.Format("{0:n2}", Convert.ToInt32(txtQtdIndisp.Text) * indisponibilidade);
            else
                txtIndisp.Text = "0,00";
        }


        private void txtQtdIndisp_GotFocus(object sender, RoutedEventArgs e)
        {

            if (txtQtdIndisp.Text == "0")
            {
                txtQtdIndisp.Text = "";
            }

        }

        private void QtdExced_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdExced.Text == "")
            {
                txtQtdExced.Text = "0";
            }

            CalcularDistribuicao();
            CalcularTotalValoresEmol();
        }

        private void QtdExced_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdExced.Text == "0")
            {
                txtQtdExced.Text = "";
            }
        }

        private void txtQtdIndisp_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void QtdExced_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbTipoCustas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoCustas.Focus())
            {
                if (cmbNatureza.SelectedIndex > -1)
                {
                    txtQtdExced.IsEnabled = true;
                    txtQtdIndisp.IsEnabled = true;
                    CalcularValoresEmolumentos();

                }
                if (cmbTipoCustas.SelectedIndex > 1)
                {
                    cmbTipoPagamento.SelectedIndex = -1;
                    cmbTipoPagamento.IsEnabled = false;
                }
                else
                {
                    cmbTipoPagamento.IsEnabled = true;
                    cmbTipoPagamento.Text = "DINHEIRO";
                }

            }
        }

        private void btnCustas_Click(object sender, RoutedEventArgs e)
        {
            WinCustasRgi winCustasRgi = new WinCustasRgi(this, Principal, "nao");
            winCustasRgi.Owner = this;

            winCustasRgi.ShowDialog();

            CalcularValoresEmolumentos();
        }

        private void checkBoxPago_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkBoxPago_Click(object sender, RoutedEventArgs e)
        {

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




        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {

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

            if (status == "novo")
            {
                if (datePickerDataPagamento.SelectedDate.Value < Principal._dataSistema.Date)
                {
                    if (usuarioLogado.Master == false)
                    {
                        MessageBox.Show("Apenas usuário Master tem permissão para incluir atos com data de pagamento diferente da data do sistema. Solicite a um usuário Master para incluir o ato.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }

                }

                SalvarAto();
            }
            else
            {
                if (dataPagamento != datePickerDataAto.SelectedDate || dataAto != datePickerDataPagamento.SelectedDate || totalInicial != txtTotalTotal.Text)
                    masterParaSalvar = true;
                else
                    masterParaSalvar = false;

                if (masterParaSalvar == false)
                    SalvarAto();
                else
                {
                    if (usuarioLogado.Master == false)
                    {
                        var confirmaMaster = new WinConfirmaSenhaMaster(this);
                        confirmaMaster.Owner = this;
                        confirmaMaster.ShowDialog();
                        if (senhaConfirmada == true)
                            SalvarAto();
                        else
                            return;
                    }
                    else
                        SalvarAto();
                }
            }

        }



        private int ObterRecibo(int? protocolo)
        {
            using (FbConnection conn2 = new FbConnection(Properties.Settings.Default.SettingReciboRgi))
            {

                string comando2 = string.Format("select RECIBO from ATOS where PROTOCOLO = {0}", protocolo);
                conn2.Open();

                using (FbCommand cmdTotal2 = new FbCommand(comando2, conn2))
                {

                    cmdTotal2.CommandType = CommandType.Text;


                    return Convert.ToInt32(cmdTotal2.ExecuteScalar());

                }

            }

        }

        private void SalvarAto()
        {
            try
            {


                ClassAto classAto = new ClassAto();

                // data do pagamento
                if (datePickerDataPagamento.SelectedDate != null)
                {
                    Ato.DataPagamento = datePickerDataPagamento.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data do Pagamento.", "Data do Pagamento", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataPagamento.Focus();
                    return;
                }


                Ato.TipoPagamento = cmbTipoPagamento.Text;


                // data do ato
                if (datePickerDataAto.SelectedDate != null)
                {
                    Ato.DataAto = datePickerDataAto.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data do Ato.", "Data do Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataAto.Focus();
                    return;
                }


                Ato.Pago = checkBoxPago.IsChecked.Value;



                if (status == "novo")
                {


                    if (usuarioLogado.Caixa == true || usuarioLogado.Master == true)
                    {
                        // IdUsuario
                        Ato.IdUsuario = listaNomes.Where(p => p.NomeUsu == cmbNomes.Text).FirstOrDefault().Id_Usuario;

                        // Usuario
                        Ato.Usuario = cmbNomes.Text;
                    }
                    else
                    {
                        // IdUsuario
                        Ato.IdUsuario = usuarioLogado.Id_Usuario;

                        // Usuario
                        Ato.Usuario = usuarioLogado.NomeUsu;
                    }

                }

                Ato.Atribuicao = "RGI";
                Ato.ValorAdicionar = Convert.ToDecimal(txtAdicionar.Text);
                Ato.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);


                // Mensalista
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    if (cmbMensalista.SelectedIndex >= 0)
                    {
                        Ato.Mensalista = cmbMensalista.Text;
                    }
                    else
                    {
                        MessageBox.Show("Informe o Nome do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                        cmbMensalista.Focus();
                        return;
                    }

                }


                Ato.Faixa = listaItensAtos.Select(p => p.Natureza).FirstOrDefault();

                if (txtProtocolo.Text != "")
                {
                    Ato.Protocolo = Convert.ToInt32(txtProtocolo.Text);

                    if (Principal.TipoAto != "CERTIDÃO RGI")
                        Ato.Recibo = ObterRecibo(Ato.Protocolo);
                    else
                        Ato.Recibo = Convert.ToInt32(txtProtocolo.Text);


                    if (Ato.Recibo == 0)
                    {
                        if (Principal.TipoAto != "CERTIDÃO RGI")
                            MessageBox.Show("Número do Protocolo inexistente.", "Protocolo", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Número do Recibo inexistente.", "Recibo", MessageBoxButton.OK, MessageBoxImage.Information);

                        txtProtocolo.Focus();
                        return;
                    }


                }
                else
                {
                    if (Principal.TipoAto != "CERTIDÃO RGI")
                        MessageBox.Show("Informe o Número do Protocolo.", "Protocolo", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Informe o Número do Recibo.", "Recibo", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtProtocolo.Focus();
                    return;
                }


                if (status == "novo")
                    if (classAto.ListarAtoPorReciboRgi(Ato.Recibo).Count > 0)
                    {
                        MessageBox.Show("Ato já lançado no Caixa.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }


                Ato.TipoAto = Principal.TipoAto;
                Ato.Natureza = Ato.Faixa;
                Ato.Escrevente = cmbNomes.Text;
                Ato.TipoCobranca = cmbTipoCustas.Text;
                Ato.Emolumentos = Convert.ToDecimal(txtEmolTotal.Text);
                Ato.Fetj = Convert.ToDecimal(txtFetjTotal.Text);
                Ato.Fundperj = Convert.ToDecimal(txtFundperjTotal.Text);
                Ato.Funperj = Convert.ToDecimal(txtFunperjTotal.Text);
                Ato.Funarpen = Convert.ToDecimal(txtFunarpenTotal.Text);
                Ato.Pmcmv = Convert.ToDecimal(txtPmcmvTotal.Text);
                Ato.Iss = Convert.ToDecimal(txtIssTotal.Text);
                Ato.Mutua = Convert.ToDecimal(txtMutuaTotal.Text);
                Ato.Acoterj = Convert.ToDecimal(txtAcoterjTotal.Text);
                Ato.Distribuicao = Convert.ToDecimal(txtDistribuicaoTotal.Text);
                Ato.Indisponibilidade = Convert.ToDecimal(txtIndisp.Text);
                Ato.TipoPrenotacao = cmbPrenotacao.Text;
                Ato.Prenotacao = Convert.ToDecimal(txtPrenotacao.Text);
                Ato.QuantIndisp = Convert.ToInt32(txtQtdIndisp.Text);
                Ato.QtdAtos = listaItensAtos.Count;
                //NumeroRequisicao
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    if (txtRequisicao.Text != "")
                    {
                        Ato.NumeroRequisicao = Convert.ToInt32(txtRequisicao.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe o Numero da requisição do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtRequisicao.Focus();
                        return;
                    }

                }


                Ato.Total = Convert.ToDecimal(txtTotalTotal.Text);

                if (status == "alterar")
                    Ato.Id_Ato = atoSelecionado.Id_Ato;

                int idAto = classAto.SalvarAto(Ato, status);

                SalvarItemAto(idAto);



                if (status == "novo")
                {
                    var valorPago = new ValorPago()
                    {
                        Data = Ato.DataPagamento,
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

                    valorPago.IdAto = idAto;
                    valorPago.IdReciboBalcao = 0;

                    if (cmbTipoPagamento.SelectedIndex == 2)
                        valorPago.Mensalista = Ato.Total;
                    else
                        valorPago.Mensalista = 0M;

                    classAto.SalvarValorPago(valorPago, status, "IdAto");
                }
                else
                {
                    if (usuarioLogado.Caixa == true || usuarioLogado.Master == true)
                    {
                        var valorPago = new ValorPago()
                        {
                            Data = Ato.DataPagamento,
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

                        valorPago.IdAto = idAto;
                        valorPago.IdReciboBalcao = 0;

                        if (cmbTipoPagamento.SelectedIndex == 2)
                            valorPago.Mensalista = Ato.Total;
                        else
                            valorPago.Mensalista = 0M;

                        classAto.SalvarValorPago(valorPago, status, "IdAto");
                    }
                }

                _atosRgi.idAtoNovo = idAto;

                _atosRgi.atoSelecionado = Ato;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o registro. " + ex.Message);
            }
        }

        private void SalvarItemAto(int idAto)
        {
            try
            {
                if (status == "alterar")
                {
                    List<ItensCustasRgi> itensExcluir = new List<ItensCustasRgi>();
                    itensExcluir = ClassCustasRgi.ListarItensCustasIdAto(idAto);


                    ClassCustasRgi.RemoverItensCustas(idAto);


                    List<ItensAtoRgi> itensAtoExcluir = new List<ItensAtoRgi>();
                    itensAtoExcluir = ClassCustasRgi.ListarAtoRgi(idAto);

                    for (int i = 0; i < itensAtoExcluir.Count; i++)
                    {
                        ClassCustasRgi.RemoverItensAto(itensAtoExcluir[i].Id_AtoRgi);
                    }

                }


                ItensAtoRgi itemSalvar;
                int idAtoRgi = 0;

                for (int i = 0; i < listaItensAtos.Count; i++)
                {
                    itemSalvar = new ItensAtoRgi();
                    itemSalvar.Id_Ato = idAto;
                    itemSalvar.Cont = listaItensAtos[i].Cont;
                    itemSalvar.Protocolo = listaItensAtos[i].Protocolo;
                    itemSalvar.Recibo = listaItensAtos[i].Recibo;
                    itemSalvar.TipoAto = listaItensAtos[i].TipoAto;
                    itemSalvar.Natureza = listaItensAtos[i].Natureza;
                    itemSalvar.Emolumentos = listaItensAtos[i].Emolumentos;
                    itemSalvar.Fetj = listaItensAtos[i].Fetj;
                    itemSalvar.Fundperj = listaItensAtos[i].Fundperj;
                    itemSalvar.Funperj = listaItensAtos[i].Funperj;
                    itemSalvar.Funarpen = listaItensAtos[i].Funarpen;
                    itemSalvar.Pmcmv = listaItensAtos[i].Pmcmv;
                    itemSalvar.Iss = listaItensAtos[i].Iss;
                    itemSalvar.Mutua = listaItensAtos[i].Mutua;
                    itemSalvar.Acoterj = listaItensAtos[i].Acoterj;
                    itemSalvar.Distribuicao = listaItensAtos[i].Distribuicao;
                    itemSalvar.QuantDistrib = listaItensAtos[i].QuantDistrib;
                    itemSalvar.Total = listaItensAtos[i].Total;

                    idAtoRgi = ClassCustasRgi.SalvarItensListaAto(itemSalvar);
                    SalvarItemCustas(idAtoRgi, idAto, Convert.ToInt32(itemSalvar.Cont));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o registro. " + ex.Message);
            }
        }

        private void SalvarItemCustas(int idAtoRgi, int idAto, int Cont)
        {
            try
            {
                if (status == "alterar")
                {
                    ClassCustasRgi.RemoverItensCustas(idAtoRgi);
                }



                for (int cont = 0; cont <= listaItensCustas.Count - 1; cont++)
                {
                    if (listaItensCustas[cont].Cont == Cont)
                    {

                        ItensCustasRgi item = new ItensCustasRgi();

                        item.Id_AtoRgi = idAtoRgi;

                        item.Id_Ato = idAto;

                        item.Cont = listaItensCustas[cont].Cont;

                        item.Tabela = listaItensCustas[cont].Tabela;

                        item.Item = listaItensCustas[cont].Item;

                        item.SubItem = listaItensCustas[cont].SubItem;

                        item.Quantidade = listaItensCustas[cont].Quantidade;

                        item.Valor = listaItensCustas[cont].Valor;

                        item.Total = listaItensCustas[cont].Total;

                        item.Descricao = listaItensCustas[cont].Descricao;

                        ClassCustasRgi.SalvarItensLista(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o registro. " + ex.Message);
            }
        }

        private void txtLetraSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
        }



        private void txtQtdDistrib_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdDistrib_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtQtdDistrib.Text == "")
            {
                txtQtdDistrib.Text = "0";
            }
            CalcularDistribuicao();
            CalcularTotalValoresEmol();
        }

        private void CalcularDistribuicao()
        {
            decimal dist = Convert.ToDecimal(listaDistribuicao.Where(p => p.Quant_Exced == 0).Select(p => p.Total).FirstOrDefault());

            decimal exced = Convert.ToDecimal(listaDistribuicao.Where(p => p.Quant_Exced == Convert.ToDecimal(txtQtdExced.Text)).Select(p => p.Total).FirstOrDefault());

            exced = exced - dist;

            dist = dist * Convert.ToInt32(txtQtdDistrib.Text);

            if (cmbTipoCustas.SelectedIndex <= 1)
                txtDistribuicao.Text = string.Format("{0:n2}", dist + exced);
            else
                txtDistribuicao.Text = "0,00";
        }


        private void txtQtdDistrib_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdDistrib.Text == "0")
            {
                txtQtdDistrib.Text = "";
            }

        }


        private void txtDesconto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDesconto.Text == "0,00")
            {
                txtDesconto.Text = "";
            }
        }

        private void txtAdicionar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtAdicionar.Text == "0,00")
            {
                txtAdicionar.Text = "";
            }
        }

        private void txtProtocolo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void cmbPrenotacao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPrenotacao.Focus())
            {
                if (cmbNatureza.SelectedIndex > -1)
                {
                    CarregarItensNaListaCustas();
                    CalcularValoresEmolumentos();

                    HabilitaTipoPagamento();
                }


            }
        }


        int cont = 1;

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {




            for (int p = 0; p < Convert.ToInt16(txtQtdAtos.Text); p++)
            {

                ItensAtoRgi novoAto = new ItensAtoRgi();

                novoAto.Natureza = cmbNatureza.Text;
                novoAto.Cont = cont;
                novoAto.Emolumentos = Convert.ToDecimal(txtEmol.Text);
                novoAto.Fetj = Convert.ToDecimal(txtFetj.Text);
                novoAto.Funperj = Convert.ToDecimal(txtFunperj.Text);
                novoAto.Fundperj = Convert.ToDecimal(txtFundperj.Text);
                novoAto.Funarpen = Convert.ToDecimal(txtFunarpen.Text);
                novoAto.Pmcmv = Convert.ToDecimal(txtPmcmv.Text);
                novoAto.Iss = Convert.ToDecimal(txtIss.Text);
                novoAto.Mutua = Convert.ToDecimal(txtMutua.Text);
                novoAto.Acoterj = Convert.ToDecimal(txtAcoterj.Text);
                novoAto.Distribuicao = Convert.ToDecimal(txtDistribuicao.Text);
                novoAto.TipoAto = "REGISTRO";
                novoAto.Total = Convert.ToDecimal(txtTotal.Text);

                listaItensAtos.Add(novoAto);



                for (int i = 0; i < listaItensCalculo.Count; i++)
                {

                    var novoItem = new ItensCustasRgi();

                    novoItem.Ato = listaItensCalculo[i].Ato;

                    novoItem.Complemento = listaItensCalculo[i].Complemento;

                    novoItem.Cont = novoAto.Cont;

                    novoItem.Descricao = listaItensCalculo[i].Descricao;

                    novoItem.Excessao = listaItensCalculo[i].Excessao;

                    novoItem.Id_Ato = listaItensCalculo[i].Id_Ato;

                    novoItem.Id_AtoRgi = listaItensCalculo[i].Id_AtoRgi;

                    novoItem.Id_Custa = listaItensCalculo[i].Id_Custa;

                    novoItem.Item = listaItensCalculo[i].Item;

                    novoItem.Quantidade = listaItensCalculo[i].Quantidade;

                    novoItem.SubItem = listaItensCalculo[i].SubItem;

                    novoItem.Tabela = listaItensCalculo[i].Tabela;

                    novoItem.Total = listaItensCalculo[i].Total;

                    novoItem.Valor = listaItensCalculo[i].Valor;

                    listaItensCustas.Add(novoItem);
                }


                cont++;
            }





            cmbNatureza.SelectedIndex = -1;
            LimpaCamposValores();

            dataGridItensAtoRgi.ItemsSource = listaItensAtos;
            dataGridItensAtoRgi.Items.Refresh();

            cmbTipoCustas.IsEnabled = false;

            cmbTipoPagamento.IsEnabled = false;

            btnSalvar.IsEnabled = true;

            cmbPrenotacao.IsEnabled = false;

            CalcularTotalValoresEmolGeral();

            HabilitaTipoPagamento();
        }



        private void LimpaCamposValores()
        {
            txtEmol.Text = "0,00";
            txtFetj.Text = "0,00";
            txtFundperj.Text = "0,00";
            txtFunperj.Text = "0,00";
            txtFunarpen.Text = "0,00";
            txtPmcmv.Text = "0,00";
            txtIss.Text = "0,00";
            txtDistribuicao.Text = "0,00";
            txtQtdAtos.Text = "1";
            txtQtdDistrib.Text = "0";
            txtQtdExced.Text = "0";
            txtMutua.Text = "0,00";
            txtAcoterj.Text = "0,00";
            txtTotal.Text = "0,00";
        }






        private void dataGridItensAtoRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (dataGridItensAtoRgi.Items.Count > 0)
                {
                    if (dataGridItensAtoRgi.SelectedIndex > -1)
                    {
                        ItensAtoRgi atoRemover = new ItensAtoRgi();
                        atoRemover = (ItensAtoRgi)dataGridItensAtoRgi.SelectedItem;

                        List<ItensCustasRgi> custasRemover = new List<ItensCustasRgi>();

                        int contAtoRemover = Convert.ToInt32(atoRemover.Cont);

                        custasRemover = listaItensCustas.Where(p => p.Cont == contAtoRemover).ToList();

                        for (int i = 0; i < custasRemover.Count; i++)
                        {
                            listaItensCustas.Remove(custasRemover[i]);
                        }

                        listaItensAtos.Remove(atoRemover);

                        dataGridItensAtoRgi.ItemsSource = listaItensAtos;
                        dataGridItensAtoRgi.Items.Refresh();


                        CalcularTotalValoresEmolGeral();

                        HabilitaTipoPagamento();
                    }
                }
                if (listaItensAtos.Count == 0)
                {
                    cmbTipoCustas.IsEnabled = true;

                    cmbTipoPagamento.IsEnabled = true;

                    cmbPrenotacao.IsEnabled = true;

                    btnSalvar.IsEnabled = false;
                }
                else
                {
                    dataGridItensAtoRgi.Focus();
                    dataGridItensAtoRgi.SelectedIndex = 0;
                }
            }

        }


        private void dataGridItensAtoRgi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dataGridItensAtoRgi.Items.Count > 0)
                {
                    atoAlterar = (ItensAtoRgi)dataGridItensAtoRgi.SelectedItem;


                    listaItensCalculo = new List<ItensCustasRgi>();

                    listaItensCalculo = listaItensCustas.Where(p => p.Cont == atoAlterar.Cont).ToList();


                    WinCustasRgi winCustasRgi = new WinCustasRgi(this, Principal, "sim", atoAlterar);
                    winCustasRgi.Owner = this;
                    winCustasRgi.ShowDialog();

                    for (int i = listaItensCustas.Count - 1; i >= 0; i--)
                    {
                        if (listaItensCustas[i].Cont == atoAlterar.Cont)
                            listaItensCustas.Remove(listaItensCustas[i]);
                    }

                    for (int i = 0; i < listaItensCalculo.Count; i++)
                    {
                        if (i == 0)
                            emolumentoPercente = Convert.ToDecimal(listaItensCalculo[i].Valor);

                        listaItensCalculo[i].Cont = atoAlterar.Cont;
                        listaItensCustas.Add(listaItensCalculo[i]);
                    }

                    atoAlterar = CalcularValoresEmolumentosAlterar(atoAlterar);

                    listaItensAtos[dataGridItensAtoRgi.SelectedIndex] = atoAlterar;

                    CalcularTotalValoresEmolGeral();

                    dataGridItensAtoRgi.ItemsSource = listaItensAtos;
                    dataGridItensAtoRgi.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridItensAtoRgi_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }



        private void txtTotalTotal_TextChanged(object sender, TextChangedEventArgs e)
        {

            lblTotal.Content = string.Format("{0}", txtTotalTotal.Text);
            HabilitaTipoPagamento();
        }

        private void txtQtdAtos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdAtos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdAtos.Text == "")
            {
                txtQtdAtos.Text = "1";
            }
        }

        private void txtQtdAtos_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdAtos.Text == "1")
            {
                txtQtdAtos.Text = "";
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

        private void HabilitaTipoPagamento()
        {
            GridValoresRecibo.IsEnabled = true;

            lblTroco.Content = "0,00";

            txtValorPagoDinheiro.Text = "0,00";

            txtValorPagoDeposito.Text = "0,00";

            txtValorPagoCheque.Text = "0,00";

            txtValorPagoChequePre.Text = "0,00";

            txtValorPagoBoleto.Text = "0,00";

            txtValorPagoCartaoCredito.Text = "0,00";

            txtValorPagoDinheiro.Text = "0,00";

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
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
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
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
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
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
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
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
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
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
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
                        mensagem = "O valor pago não pode ser menor que o total, favor verifique.";
                        tabControl1.SelectedIndex = 1;
                    }
                    break;

                case 7:
                    if (Convert.ToDecimal(lblTotal.Content) != (Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text)))
                    {
                        mensagem = "Os valores pagos somados devem corresponder exatamente ao total. O troco deve estar igual a '0,00', favor verifique.";
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
    }
}
