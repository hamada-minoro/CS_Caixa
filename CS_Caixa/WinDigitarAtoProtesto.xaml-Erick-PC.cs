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
using CS_Caixa.RelatoriosForms;
using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinDigitarAtoProtesto.xaml
    /// </summary>
    public partial class WinDigitarAtoProtesto : Window
    {

        WinPrincipal Principal;
        List<CadMensalista> listaMensalista = new List<CadMensalista>();
        List<Usuario> listaNomes = new List<Usuario>();
        Usuario usuarioLogado;
        public List<CustasProtesto> listaCustas = new List<CustasProtesto>();
        public List<ItensCustasProtesto> listaItens = new List<ItensCustasProtesto>();
        public List<CustasProtesto> listaCustasItens = new List<CustasProtesto>();
        List<Portador> portador = new List<Portador>();
        public CustasProtesto emolumentos;
        public decimal emolLista;
        public int ano = DateTime.Now.Year;
        public decimal mutua = 0;
        public decimal acoterj = 0;
        string status;
        Ato atoSelecionado = new Ato();
        bool passou = false;
        string faixa;
        decimal tarifa = 0;
        public decimal porcentagemIss;


        decimal emolConv;
        decimal fetj_20Conv;
        decimal fundperj_5Conv;
        decimal funarpen_4Conv;
        decimal pmcmv_2Conv;
        decimal issConv;

        decimal CapaSerasaEmol;
        decimal CapaSerasa20;
        decimal CapaSerasa5;
        decimal CapaSerasa5_2;
        decimal CapaSerasa4;
        decimal CapaSerasa2;
        decimal CapaSerasaIss;
        decimal CapaSerasa;


        decimal NomeSerasEmol;
        decimal NomeSeras20;
        decimal NomeSeras5;
        decimal NomeSeras5_2;
        decimal NomeSeras4;
        decimal NomeSeras2;
        decimal NomeSerasIss;
        decimal NomeSeras;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        public decimal apontCancelamento = 0;

        string totalInicial = string.Empty;
        bool masterParaSalvar = false;
        public bool senhaConfirmada = false;
        DateTime dataAto = new DateTime();
        DateTime dataPagamento = new DateTime();

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public WinDigitarAtoProtesto(WinPrincipal Principal, Usuario usuarioLogado, string status)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            InitializeComponent();
        }

        public WinDigitarAtoProtesto(WinPrincipal Principal, Usuario usuarioLogado, string status, Ato atoSelecionado, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            this.atoSelecionado = atoSelecionado;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this.Title = Principal.TipoAto;

            datePickerDataAto.SelectedDate = DateTime.Now.Date;
            datePickerDataPagamento.SelectedDate = DateTime.Now.Date;

            var bc = new BrushConverter();


            if (Principal.TipoAto == "APONTAMENTO")
                this.Background = (Brush)bc.ConvertFrom("#FFD3E9EF");

            if (Principal.TipoAto == "CANCELAMENTO")
                this.Background = (Brush)bc.ConvertFrom("#FFE2E0BA");

            if (Principal.TipoAto == "PAGAMENTO")
                this.Background = (Brush)bc.ConvertFrom("#FF7FD3C0");

            if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                this.Background = (Brush)bc.ConvertFrom("#FFEBDED0");

            ClassMensalista classMensalista = new ClassMensalista();
            ClassUsuario classUsuario = new ClassUsuario();
            ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();
            ClassPortador classPortador = new ClassPortador();
            portador = classPortador.ListaPortador();
            cmbPortador.ItemsSource = portador.Select(p => p.NOME);
            listaMensalista = classMensalista.ListaMensalistas();
            listaNomes = classUsuario.ListaUsuarios();
            cmbNomes.ItemsSource = listaNomes.Select(p => p.NomeUsu);
            cmbNomes.SelectedItem = usuarioLogado.NomeUsu;
            listaCustas = classCustasProtesto.ListaCustas();
            listaCustasItens = classCustasProtesto.ListaCustas();

            if (Principal.TipoAto == "PAGAMENTO")
                tarifa = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "TARIFA BANCÁRIA" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());



            List<CustasProtesto> atulizado = listaCustas.Where(p => p.ANO == ano).ToList();
            if (status == "novo")
            {
                if (atulizado.Count > 0)
                {
                    if (DateTime.Now.Month == 1 && DateTime.Now.Day < 3)
                    {
                        if (MessageBox.Show("Deseja utilizar as custas do ano passado?", "Custas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            ano = listaCustas.Max(p => p.ANO).Value - 1;
                        }
                    }
                }
                else
                {
                    ano = listaCustas.Max(p => p.ANO).Value;

                }
            }

            if (status == "alterar")
            {

                if (DateTime.Now.Year != atoSelecionado.DataPagamento.Year)
                {
                    if (MessageBox.Show("Deseja utilizar as custas do ano deste ato?", "Custas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        ano = atoSelecionado.DataPagamento.Year;
                    }

                }
                btnImportar.IsEnabled = false;
            }


            mutua = Convert.ToDecimal(listaCustas.Where(p => p.TEXTO == "MUTUA" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
            acoterj = Convert.ToDecimal(listaCustas.Where(p => p.TEXTO == "ACOTERJ" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
            porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());



            if (Principal.TipoAto == "APONTAMENTO")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "P").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                
                lblProtRecibo.Content = "Protocolo:";
            }

            if (Principal.TipoAto == "CANCELAMENTO")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano).OrderBy(p => p.ORDEM).Select(p => p).ToList();
                listaCustas = listaCustas.Where(p => p.TIPO == "P" || p.TIPO == "C").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                btnImportar.IsEnabled = false;
                lblProtRecibo.Content = "Recibo:";
                txtVrTitulo.IsEnabled = false;


            }

            if (Principal.TipoAto == "PAGAMENTO")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "P").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                lblTarifa.Visibility = Visibility.Visible;
                txtTarifa.Visibility = Visibility.Visible;
            }

            if (Principal.TipoAto == "CERTIDÃO PROTESTO")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "I").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                btnImportar.IsEnabled = false;
                lblProtRecibo.Content = "Recibo:";
                btnSalvar.IsEnabled = true;

                cmbPortador.IsEnabled = false;
                txtletraFaixa.IsEnabled = false;
                txtFaixa.IsEnabled = false;
                txtConvenio.IsEnabled = false;
                cmbNatureza.IsEnabled = true;
            }

            groupBox1.Content = Principal.TipoAto;
            listaCustasItens = listaCustasItens.Where(p => p.ANO == ano && p.TIPO == "I").OrderBy(p => p.ORDEM).Select(p => p).ToList();


            if (Principal.TipoAto != "CERTIDÃO PROTESTO")
            {
                cmbNatureza.Items.Add(Principal.TipoAto);
                cmbNatureza.SelectedIndex = 0;
            }
            else
            {
                cmbNatureza.Items.Add(Principal.TipoAto);
                cmbNatureza.Items.Add("FOLHA EXCEDENTE");
                cmbNatureza.Items.Add("CERTIDÃO SERASA");
                cmbNatureza.Items.Add("CERTIDÃO BOA VISTA");
                if (status == "novo")
                    cmbNatureza.SelectedIndex = 0;
            }

            if (Principal.TipoAto == "APONTAMENTO")
            {
                cmbNatureza.IsEnabled = true;
                cmbNatureza.Items.Add("APONTAMENTO CANCELAMENTO");
                cmbNatureza.Items.Add("APONTAMENTO RETIRADO");
                cmbNatureza.SelectedIndex = 0;
            }

            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                cmbNomes.IsEnabled = true;

            }
            else
            {
                cmbNomes.IsEnabled = false;
            }

            cmbMensalista.ItemsSource = listaMensalista.Select(p => p.Nome);

            if (status == "alterar")
            {
                if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
                {
                    string nat = atoSelecionado.Natureza;

                    if (nat.Contains("APONTAMENTO"))
                    {
                        nat = atoSelecionado.Natureza.Replace("APONTAMENTO - ", "");
                    }

                    if (nat.Contains("PAGAMENTO"))
                    {
                        nat = atoSelecionado.Natureza.Replace("PAGAMENTO - ", "");
                    }

                    if (nat.Contains("RETIRADO"))
                    {
                        nat = atoSelecionado.Natureza.Replace("RETIRADO - ", "");
                    }

                    listaItens = classCustasProtesto.ListarItensCustas(atoSelecionado.Id_Ato);


                    emolumentos = listaCustas.Where(p => p.DESCR == nat).FirstOrDefault();

                }

                if (Principal.TipoAto == "CANCELAMENTO")
                {
                    listaItens = classCustasProtesto.ListarItensCustas(atoSelecionado.Id_Ato);
                    emolumentos = listaCustas.Where(p => p.DESCR == "CANCELAMENTO DO PROTESTO/AVERBAÇÃO DA SUSTAÇÃO").FirstOrDefault();
                }

                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    listaItens = classCustasProtesto.ListarItensCustas(atoSelecionado.Id_Ato);
                    cmbNatureza.Text = atoSelecionado.Natureza;
                }

                if (listaItens.Count > 0)
                {
                    btnCustas.IsEnabled = true;
                }
                else
                {
                    btnCustas.IsEnabled = false;
                }

                CarregaCamposAlterar();


            }

            datePickerDataAto.Focus();
            passou = true;


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



            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();
        }

        private void CarregaCamposAlterar()
        {

            if (Principal.TipoAto == "APONTAMENTO")
            {
                datePickerDataAto.SelectedDate = atoSelecionado.DataAto;

                cmbNomes.SelectedItem = atoSelecionado.Escrevente;
                cmbTipoCustas.Text = atoSelecionado.TipoCobranca;
                cmbPortador.Text = atoSelecionado.Portador;
                txtletraFaixa.Text = atoSelecionado.Faixa;
                txtFaixa.Text = atoSelecionado.Natureza;
                txtEmol.Text = string.Format("{0:n2}", atoSelecionado.Emolumentos);
                txtFetj.Text = string.Format("{0:n2}", atoSelecionado.Fetj);
                txtFundperj.Text = string.Format("{0:n2}", atoSelecionado.Fundperj);
                txtFunperj.Text = string.Format("{0:n2}", atoSelecionado.Funperj);
                txtFunarpen.Text = string.Format("{0:n2}", atoSelecionado.Funarpen);
                txtPmcmv.Text = string.Format("{0:n2}", atoSelecionado.Pmcmv);
                txtIss.Text = string.Format("{0:n2}", atoSelecionado.Iss);
                txtMutua.Text = string.Format("{0:n2}", atoSelecionado.Mutua);
                txtAcoterj.Text = string.Format("{0:n2}", atoSelecionado.Acoterj);
                txtTotal.Text = string.Format("{0:n2}", atoSelecionado.Total);

                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

                cmbNatureza.Text = atoSelecionado.TipoAto;

                txtProtRecibo.Text = string.Format("{0}", atoSelecionado.Protocolo);

                txtVrTitulo.Text = string.Format("{0:n2}", atoSelecionado.ValorTitulo);

                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;
                }
                if (cmbTipoPagamento.Text == "MENSALISTA")
                {
                    cmbMensalista.Text = atoSelecionado.Mensalista;
                    txtRequisicao.Text = atoSelecionado.NumeroRequisicao.ToString();
                }



                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;


                ClassAto classAto = new ClassAto();

                var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);


                checkBoxPago.IsChecked = atoSelecionado.Pago;


                txtConvenio.Text = atoSelecionado.Convenio;


                cmbNatureza.SelectedItem = atoSelecionado.Natureza;

                if (valorPago != null)
                {
                    txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                    txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                    txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                    txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                    txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                    txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
                }
            }

            if (Principal.TipoAto == "CANCELAMENTO")
            {
                datePickerDataAto.SelectedDate = atoSelecionado.DataAto;

                cmbNomes.SelectedItem = atoSelecionado.Escrevente;
                cmbTipoCustas.Text = atoSelecionado.TipoCobranca;
                cmbPortador.Text = atoSelecionado.Portador;
                txtletraFaixa.Text = atoSelecionado.Faixa;
                txtFaixa.Text = atoSelecionado.Natureza;
                faixa = txtFaixa.Text;

                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

                cmbNatureza.Text = atoSelecionado.TipoAto;

                txtProtRecibo.Text = string.Format("{0}", atoSelecionado.Recibo);

                txtVrTitulo.Text = string.Format("{0:n2}", atoSelecionado.ValorTitulo);

                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;
                }


                if (cmbTipoPagamento.Text == "MENSALISTA")
                {
                    cmbMensalista.Text = atoSelecionado.Mensalista;
                    txtRequisicao.Text = atoSelecionado.NumeroRequisicao.ToString();
                }



                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

                ClassAto classAto = new ClassAto();

                var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);


                checkBoxPago.IsChecked = atoSelecionado.Pago;


                txtConvenio.Text = atoSelecionado.Convenio;


                cmbNatureza.SelectedItem = atoSelecionado.Natureza;

                if (valorPago != null)
                {
                    txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                    txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                    txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                    txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                    txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                    txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
                }
            }

            if (Principal.TipoAto == "PAGAMENTO")
            {
                datePickerDataAto.SelectedDate = atoSelecionado.DataAto;

                cmbNomes.SelectedItem = atoSelecionado.Escrevente;
                cmbTipoCustas.Text = atoSelecionado.TipoCobranca;
                cmbPortador.Text = atoSelecionado.Portador;
                txtletraFaixa.Text = atoSelecionado.Faixa;
                txtFaixa.Text = atoSelecionado.Natureza;

                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

                cmbNatureza.Text = atoSelecionado.TipoAto;

                txtProtRecibo.Text = string.Format("{0}", atoSelecionado.Protocolo);

                txtVrTitulo.Text = string.Format("{0:n2}", atoSelecionado.ValorTitulo);

                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;
                }
                if (cmbTipoPagamento.Text == "MENSALISTA")
                {
                    cmbMensalista.Text = atoSelecionado.Mensalista;
                    txtRequisicao.Text = atoSelecionado.NumeroRequisicao.ToString();
                }



                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

                ClassAto classAto = new ClassAto();

                var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);



                checkBoxPago.IsChecked = atoSelecionado.Pago;


                txtConvenio.Text = atoSelecionado.Convenio;


                cmbNatureza.SelectedItem = atoSelecionado.Natureza;

                if (valorPago != null)
                {
                    txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                    txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                    txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                    txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                    txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                    txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
                }



            }

            if (Principal.TipoAto == "CERTIDÃO PROTESTO")
            {

                datePickerDataAto.SelectedDate = atoSelecionado.DataAto;

                cmbNomes.SelectedItem = atoSelecionado.Escrevente;
                cmbTipoCustas.Text = atoSelecionado.TipoCobranca;

                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;


                txtProtRecibo.Text = string.Format("{0}", atoSelecionado.Recibo);


                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;
                }
                if (cmbTipoPagamento.Text == "MENSALISTA")
                {
                    cmbMensalista.Text = atoSelecionado.Mensalista;
                    txtRequisicao.Text = atoSelecionado.NumeroRequisicao.ToString();
                }



                datePickerDataPagamento.SelectedDate = atoSelecionado.DataPagamento;

                ClassAto classAto = new ClassAto();

                var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);


                checkBoxPago.IsChecked = atoSelecionado.Pago;


                txtQtdNomes.Text = atoSelecionado.Faixa;
                cmbNatureza.SelectedItem = atoSelecionado.Natureza;

                if (cmbNatureza.SelectedIndex >= 2)
                {
                    CalcularCertidaoSerasaBoaVista();

                }

                if (valorPago != null)
                {
                    txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                    txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                    txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                    txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                    txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);
                    txtValorPagoCartaoCredito.Text = string.Format("{0:n2}", valorPago.CartaoCredito);
                }

            }

            totalInicial = string.Format("{0:n2}", atoSelecionado.Total);
            masterParaSalvar = false;
            dataAto = atoSelecionado.DataAto;
            dataPagamento = atoSelecionado.DataPagamento;
        }

        private void CalcularCertidaoSerasaBoaVista()
        {
            emolumentos = listaCustas.Where(p => p.DESCR == "CERTIDÃO FORNECIDA A CADA ENTIDADE REQUERENTE").FirstOrDefault();

            CalcularValoresCertidaoSerasa(Convert.ToDecimal(emolumentos.VALOR), emolumentos.DESCR);


            listaItens = new List<ItensCustasProtesto>();
            ItensCustasProtesto novoIten = new ItensCustasProtesto();
            novoIten.Item = emolumentos.ITEM;
            novoIten.SubItem = emolumentos.SUB;
            novoIten.Tabela = emolumentos.TAB;
            novoIten.Descricao = emolumentos.TEXTO;
            novoIten.Quantidade = "1";
            novoIten.Valor = emolumentos.VALOR;
            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
            listaItens.Add(novoIten);


            emolumentos = listaCustas.Where(p => p.DESCR == "A CADA NOME/DOCUMENTO RELACIONADO AO ITEM 24.3.1").FirstOrDefault();


            CalcularValoresCertidaoSerasa(Convert.ToDecimal(emolumentos.VALOR), emolumentos.DESCR);

            novoIten = new ItensCustasProtesto();
            novoIten.Item = emolumentos.ITEM;
            novoIten.SubItem = emolumentos.SUB;
            novoIten.Tabela = emolumentos.TAB;
            novoIten.Descricao = emolumentos.TEXTO;
            novoIten.Quantidade = txtQtdNomes.Text;
            novoIten.Valor = emolumentos.VALOR;
            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
            listaItens.Add(novoIten);


            btnCustas.IsEnabled = false;

        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

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
                }
                else
                {
                    txtRequisicao.Text = "";
                    cmbMensalista.SelectedIndex = -1;
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



        private void Window_Closed(object sender, EventArgs e)
        {

            //WinAtoProtesto apontamento = new WinAtoProtesto(usuarioLogado, Principal, atoSelecionado, dataInicioConsulta, dataFimConsulta);
            //apontamento.Owner = Principal;
            //apontamento.ShowDialog();


        }



        private void CalcularValoresApontamento()
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


            try
            {

                emol = Convert.ToDecimal(listaItens.Sum(p => p.Total));

                if (emol != 0)
                    if (cmbTipoCustas.SelectedIndex <= 1)
                    {
                        fetj_20 = emol * 20 / 100;
                        fundperj_5 = emol * 5 / 100;
                        funperj_5 = emol * 5 / 100;
                        funarpen_4 = emol * 4 / 100;

                        //iss = (100 - porcentagemIss) / 100;
                        //iss = emol / iss - emol;

                        iss = emol * porcentagemIss / 100;

                        pmcmv_2 = Convert.ToDecimal(emolumentos.VALOR * 2) / 100;

                        if (cmbTipoCustas.SelectedIndex == 0)
                        {
                            Semol = Convert.ToString(emol);
                        }
                        Sfetj_20 = Convert.ToString(fetj_20);
                        Sfundperj_5 = Convert.ToString(fundperj_5);
                        Sfunperj_5 = Convert.ToString(funperj_5);
                        Sfunarpen_4 = Convert.ToString(funarpen_4);
                        Spmcmv_2 = Convert.ToString(pmcmv_2);
                        Siss = Convert.ToString(iss);

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

                txtEmol.Text = Semol;
                txtFetj.Text = Sfetj_20;
                txtFundperj.Text = Sfundperj_5;
                txtFunperj.Text = Sfunperj_5;
                txtFunarpen.Text = Sfunarpen_4;
                txtPmcmv.Text = Spmcmv_2;
                txtIss.Text = Siss;


                if (txtConvenio.Text == "N")
                {
                    if (cmbTipoCustas.SelectedIndex <= 1)
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


                    if (cmbTipoCustas.SelectedIndex <= 1)
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


                txtTotal.Text = CalcularTotal(txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
                lblTotal.Content = txtTotal.Text;

                CalculaTroco();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



        private void CalcularValoresPagamento()
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


            try
            {
                emol = Convert.ToDecimal(listaItens.Sum(p => p.Total));
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    //iss = (100 - porcentagemIss) / 100;
                    //iss = emol / iss - emol;

                    iss = emol * porcentagemIss / 100;

                    pmcmv_2 = Convert.ToDecimal(emolumentos.VALOR * 2) / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                    Siss = Convert.ToString(iss);
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


                txtEmol.Text = Semol;
                txtFetj.Text = Sfetj_20;
                txtFundperj.Text = Sfundperj_5;
                txtFunperj.Text = Sfunperj_5;
                txtFunarpen.Text = Sfunarpen_4;
                txtPmcmv.Text = Spmcmv_2;
                txtIss.Text = Siss;


                if (txtConvenio.Text == "N")
                {
                    if (cmbTipoCustas.SelectedIndex <= 1)
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


                    if (cmbTipoCustas.SelectedIndex <= 1)
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

                txtTarifa.Text = string.Format("{0:n2}", tarifa);
                txtTotal.Text = CalcularTotal(txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
                lblTotal.Content = txtTotal.Text;

                CalculaTroco();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CalcularValoresCancelamento()
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


            try
            {

                emol = Convert.ToDecimal(listaItens.Sum(p => p.Total));
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    //iss = (100 - porcentagemIss) / 100;
                    //iss = emol / iss - emol;

                    iss = emol * porcentagemIss / 100;

                    pmcmv_2 = Convert.ToDecimal(emolumentos.VALOR * 2) / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                    Siss = Convert.ToString(iss);
                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {
                    apontCancelamento = 0;
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


                txtEmol.Text = Semol;
                txtFetj.Text = Sfetj_20;
                txtFundperj.Text = Sfundperj_5;
                txtFunperj.Text = Sfunperj_5;
                txtFunarpen.Text = Sfunarpen_4;
                txtPmcmv.Text = Spmcmv_2;
                txtIss.Text = Siss;

                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    if (txtConvenio.Text == "S")
                    {
                        txtMutua.Text = string.Format("{0:n2}", mutua * 2);
                        txtAcoterj.Text = string.Format("{0:n2}", acoterj * 2);
                    }
                    else
                    {
                        txtMutua.Text = string.Format("{0:n2}", mutua);
                        txtAcoterj.Text = string.Format("{0:n2}", acoterj);
                    }
                }
                else
                {
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }




                txtTotal.Text = CalcularTotal(txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
                lblTotal.Content = txtTotal.Text;

                CalculaTroco();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



        private decimal CalcularValoresApontCancelamento()
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





            emol = Convert.ToDecimal(listaItens.Sum(p => p.Total));


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

                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        pmcmv_2 = Convert.ToDecimal(emolumentos.VALOR * 2) / 100;
                    else
                        pmcmv_2 = 0;



                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        Spmcmv_2 = Convert.ToString(pmcmv_2);



                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;
                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        pmcmv_2 = 0;

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

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {
                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);
                }




                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                    txtPmcmv.Text = Spmcmv_2;


                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {

                    if (cmbTipoCustas.SelectedIndex <= 1)
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
                emolConv = Convert.ToDecimal(Semol);
                fetj_20Conv = Convert.ToDecimal(Sfetj_20);
                fundperj_5Conv = Convert.ToDecimal(Sfundperj_5);
                funarpen_4Conv = Convert.ToDecimal(Sfunarpen_4);
                pmcmv_2Conv = Convert.ToDecimal(Spmcmv_2);
                issConv = Convert.ToDecimal(Siss);

                return Convert.ToDecimal(Semol) + Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Spmcmv_2) + Convert.ToDecimal(Siss) + mutua + acoterj;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }



        private decimal CalcularValoresApontCancelamento(string letraFaixa)
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





            emol = Convert.ToDecimal(listaCustas.Where(p => p.SUB == letraFaixa).Select(p => p.VALOR).FirstOrDefault());


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

                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        pmcmv_2 = Convert.ToDecimal(emol * 2) / 100;
                    else
                        pmcmv_2 = 0;



                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        Spmcmv_2 = Convert.ToString(pmcmv_2);



                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;
                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                        pmcmv_2 = 0;

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

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);


                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {
                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);
                }




                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                    txtPmcmv.Text = Spmcmv_2;


                if (txtConvenio.Text == "S")
                {
                    txtMutua.Text = string.Format("{0:n2}", mutua * 2);
                    txtAcoterj.Text = string.Format("{0:n2}", acoterj * 2);
                }
                else
                {
                    txtMutua.Text = string.Format("{0:n2}", mutua);
                    txtAcoterj.Text = string.Format("{0:n2}", acoterj);
                }

                emolConv = Convert.ToDecimal(Semol);
                fetj_20Conv = Convert.ToDecimal(Sfetj_20);
                fundperj_5Conv = Convert.ToDecimal(Sfundperj_5);
                funarpen_4Conv = Convert.ToDecimal(Sfunarpen_4);
                pmcmv_2Conv = Convert.ToDecimal(Spmcmv_2);
                issConv = Convert.ToDecimal(Siss);

                return Convert.ToDecimal(Semol) + Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Spmcmv_2) + Convert.ToDecimal(Siss) + mutua + acoterj;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }


        private void CalcularValoresCertidao()
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal iss = 0;

            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Siss = "0,00";
            int index;


            try
            {

                emol = Convert.ToDecimal(listaItens.Sum(p => p.Total));
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    //iss = (100 - porcentagemIss) / 100;
                    //iss = emol / iss - emol;

                    iss = emol * porcentagemIss / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);

                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {
                    apontCancelamento = 0;
                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
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

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);


                txtEmol.Text = Semol;
                txtFetj.Text = Sfetj_20;
                txtFundperj.Text = Sfundperj_5;
                txtFunperj.Text = Sfunperj_5;
                txtFunarpen.Text = Sfunarpen_4;
                txtIss.Text = Siss;
                txtPmcmv.Text = "0,00";




                txtMutua.Text = "0,00";
                txtAcoterj.Text = "0,00";


                txtTotal.Text = CalcularTotal(txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
                lblTotal.Content = txtTotal.Text;

                CalculaTroco();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void CalcularValoresCertidaoSerasa(decimal valor, string descricao)
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


            try
            {

                emol = valor;
                if (cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;

                    iss = (100 - porcentagemIss) / 100;
                    iss = emol / iss - emol;

                    if (cmbNatureza.SelectedIndex == 2)
                        iss = emol * 5 / 100;
                    else
                        iss = emol * porcentagemIss / 100;

                    pmcmv_2 = Convert.ToDecimal(emol * 2) / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }
                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                    Siss = Convert.ToString(iss);
                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {
                    apontCancelamento = 0;
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

                if (emolumentos.DESCR == "CERTIDÃO FORNECIDA A CADA ENTIDADE REQUERENTE")
                {
                    CapaSerasaEmol = Convert.ToDecimal(Semol);
                    CapaSerasa20 = Convert.ToDecimal(Sfetj_20);
                    CapaSerasa5 = Convert.ToDecimal(Sfundperj_5);
                    CapaSerasa5_2 = Convert.ToDecimal(Sfunperj_5);
                    CapaSerasa4 = Convert.ToDecimal(Sfunarpen_4);
                    CapaSerasa2 = Convert.ToDecimal(Spmcmv_2);
                    CapaSerasaIss = Convert.ToDecimal(Siss);
                    CapaSerasa = Convert.ToDecimal(Semol) + Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Spmcmv_2) + Convert.ToDecimal(Siss); ;



                }
                else
                {
                    NomeSerasEmol = Convert.ToDecimal(Semol) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSeras20 = Convert.ToDecimal(Sfetj_20) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSeras5 = Convert.ToDecimal(Sfundperj_5) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSeras5_2 = Convert.ToDecimal(Sfunperj_5) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSeras4 = Convert.ToDecimal(Sfunarpen_4) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSeras2 = Convert.ToDecimal(Spmcmv_2) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSerasIss = Convert.ToDecimal(Siss) * Convert.ToInt32(txtQtdNomes.Text);
                    NomeSeras = Convert.ToDecimal(Semol) + Convert.ToDecimal(Sfetj_20) + Convert.ToDecimal(Sfundperj_5) + Convert.ToDecimal(Sfunperj_5) + Convert.ToDecimal(Sfunarpen_4) + Convert.ToDecimal(Spmcmv_2) + Convert.ToDecimal(Siss);
                }

                txtEmol.Text = string.Format("{0:n2}", CapaSerasaEmol + NomeSerasEmol);
                txtFetj.Text = string.Format("{0:n2}", CapaSerasa20 + NomeSeras20);
                txtFundperj.Text = string.Format("{0:n2}", CapaSerasa5 + NomeSeras5);
                txtFunperj.Text = string.Format("{0:n2}", CapaSerasa5_2 + NomeSeras5_2);
                txtFunarpen.Text = string.Format("{0:n2}", CapaSerasa4 + NomeSeras4);
                txtPmcmv.Text = string.Format("{0:n2}", CapaSerasa2 + NomeSeras2);
                txtIss.Text = string.Format("{0:n2}", CapaSerasaIss + NomeSerasIss);

                txtTotal.Text = string.Format("{0:n2}", CalcularTotal(txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text));
                lblTotal.Content = txtTotal.Text;

                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        public string CalcularTotal(string txtEmol, string txtFetj, string txtFundperj, string txtFunperj, string txtFunarpen, string txtPmcmv, string txtIss, string txtMutua, string txtAcoterj)
        {
            try
            {

                decimal emolumento = Convert.ToDecimal(txtEmol);
                decimal Fetj = Convert.ToDecimal(txtFetj);
                decimal Fundperj = Convert.ToDecimal(txtFundperj);
                decimal Funperj = Convert.ToDecimal(txtFunperj);
                decimal Funarpen = Convert.ToDecimal(txtFunarpen);
                decimal Pmcmv = Convert.ToDecimal(txtPmcmv);
                decimal Iss = Convert.ToDecimal(txtIss);
                decimal Mutua = Convert.ToDecimal(txtMutua);
                decimal Acoterj = Convert.ToDecimal(txtAcoterj);
                decimal Total = 0;


                Total = emolumento + Fetj + Fundperj + Funperj + Funarpen + Pmcmv + Iss + Mutua + Acoterj;


                string Stotal = Total.ToString();

                int index = Stotal.IndexOf(',');

                Stotal = Stotal.Substring(0, index + 3);

                return Stotal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void CalcularItensCustas()
        {
            if (status == "novo")
            {
                if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
                {
                    emolumentos = listaCustas.Where(p => p.DESCR == txtFaixa.Text).FirstOrDefault();
                    listaItens = new List<ItensCustasProtesto>();
                    ItensCustasProtesto novoIten = new ItensCustasProtesto();
                    novoIten.Item = emolumentos.ITEM;
                    novoIten.SubItem = emolumentos.SUB;
                    novoIten.Tabela = emolumentos.TAB;
                    novoIten.Descricao = emolumentos.TEXTO;
                    novoIten.Quantidade = "1";
                    novoIten.Valor = emolumentos.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    listaItens.Add(novoIten);
                }


                if (Principal.TipoAto == "CANCELAMENTO")
                {

                    emolumentos = listaCustas.Where(p => p.DESCR == "CANCELAMENTO DO PROTESTO/AVERBAÇÃO DA SUSTAÇÃO").FirstOrDefault();
                    listaItens = new List<ItensCustasProtesto>();
                    ItensCustasProtesto novoIten = new ItensCustasProtesto();
                    novoIten.Item = emolumentos.ITEM;
                    novoIten.SubItem = emolumentos.SUB;
                    novoIten.Tabela = emolumentos.TAB;
                    novoIten.Descricao = emolumentos.TEXTO;
                    novoIten.Quantidade = "1";
                    novoIten.Valor = emolumentos.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    listaItens.Add(novoIten);

                }

                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {

                    if (cmbNatureza.SelectedIndex == 0)
                    {
                        emolumentos = listaCustas.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS").FirstOrDefault();
                        listaItens = new List<ItensCustasProtesto>();
                        ItensCustasProtesto novoIten = new ItensCustasProtesto();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = emolumentos.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);

                        emolumentos = new CustasProtesto();

                        emolumentos = listaCustas.Where(p => p.DESCR == "BUSCAS EM LIVROS OU PAPÉIS").FirstOrDefault();
                        novoIten = new ItensCustasProtesto();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = emolumentos.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);

                        btnCustas.IsEnabled = true;
                    }

                    if (cmbNatureza.SelectedIndex == 1)
                    {
                        emolumentos = listaCustas.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS").FirstOrDefault();
                        listaItens = new List<ItensCustasProtesto>();
                        ItensCustasProtesto novoIten = new ItensCustasProtesto();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = emolumentos.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);

                        btnCustas.IsEnabled = true;
                    }

                    if (cmbNatureza.SelectedIndex == 2)
                    {
                        emolumentos = listaCustas.Where(p => p.DESCR == "CERTIDÃO FORNECIDA A CADA ENTIDADE REQUERENTE").FirstOrDefault();

                        CalcularValoresCertidaoSerasa(Convert.ToDecimal(emolumentos.VALOR), emolumentos.DESCR);


                        listaItens = new List<ItensCustasProtesto>();
                        ItensCustasProtesto novoIten = new ItensCustasProtesto();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = emolumentos.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);


                        emolumentos = listaCustas.Where(p => p.DESCR == "A CADA NOME/DOCUMENTO RELACIONADO AO ITEM 24.3.1").FirstOrDefault();


                        CalcularValoresCertidaoSerasa(Convert.ToDecimal(emolumentos.VALOR), emolumentos.DESCR);

                        novoIten = new ItensCustasProtesto();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = txtQtdNomes.Text;
                        novoIten.Valor = emolumentos.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);


                        btnCustas.IsEnabled = false;


                    }
                }
            }

        }





        private void cmbTipoCustas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbTipoCustas.Focus())
            {
                if (cmbNatureza.SelectedIndex > -1)
                {

                    if (cmbTipoCustas.Focus())
                    {
                        if (emolumentos != null && cmbPortador.SelectedIndex > -1)
                        {
                            if (Principal.TipoAto == "APONTAMENTO")
                            {
                                CalcularValoresApontamento();
                            }
                            if (Principal.TipoAto == "PAGAMENTO")
                            {
                                CalcularValoresPagamento();
                            }
                            if (Principal.TipoAto == "CANCELAMENTO")
                            {
                                CalcularItensCustas();
                                CalcularValoresCancelamento();
                            }
                        }

                        if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                        {
                            CalcularItensCustas();
                            CalcularValoresCertidao();
                        }
                    }
                }

                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {
                    if (cmbTipoCustas.SelectedIndex > 1)
                    {
                        cmbTipoPagamento.SelectedIndex = -1;
                        cmbTipoPagamento.IsEnabled = false;
                    }
                    else
                    {
                        cmbTipoPagamento.IsEnabled = true;

                        string codigoPortador = portador[cmbPortador.SelectedIndex].CODIGO;
                        if (codigoPortador == "000" || codigoPortador == "104")
                        {
                            cmbTipoPagamento.SelectedIndex = 0;
                            if (usuarioLogado.Caixa == true || usuarioLogado.Master == true)
                            {
                                checkBoxPago.IsEnabled = true;
                            }
                            else
                            {
                                checkBoxPago.IsEnabled = false;
                            }
                        }
                        else
                        {
                            cmbTipoPagamento.SelectedIndex = 1;
                        }
                    }

                }


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



        private void checkBoxPago_Click(object sender, RoutedEventArgs e)
        {

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

        private void txtValorPagoDinheiro_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPagoDinheiro.Text == "0,00")
            {
                txtValorPagoDinheiro.Text = "";
            }
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (Principal.TipoAto != "PAGAMENTO")
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

                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                    ProcSalvarCertidao();
                else
                    ProcSalvar();
            }
            else
            {


                if (dataPagamento != datePickerDataAto.SelectedDate || dataAto != datePickerDataPagamento.SelectedDate || totalInicial != txtTotal.Text)
                    masterParaSalvar = true;
                else
                    masterParaSalvar = false;


                if (masterParaSalvar == false)
                {
                    if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                        ProcSalvarCertidao();
                    else
                        ProcSalvar();
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
                            if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                                ProcSalvarCertidao();
                            else
                                ProcSalvar();
                        }
                        else
                            return;
                    }
                    else
                    {
                        if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                            ProcSalvarCertidao();
                        else
                            ProcSalvar();
                    }
                }

            }
        }


        private int ObterRecibo(string protocolo)
        {
            int retorno = 0;
            int recibo = 0;

            try
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando = string.Format("select id_ato from titulos where protocolo = '{0}'", protocolo);
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;


                        retorno = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }
                }

                using (FbConnection conn1 = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando1 = string.Format("select recibo from CERTIDOES where id_ato = {0}", retorno);
                    conn1.Open();

                    using (FbCommand cmdTotal1 = new FbCommand(comando1, conn1))
                    {
                        cmdTotal1.CommandType = CommandType.Text;


                        recibo = Convert.ToInt32(cmdTotal1.ExecuteScalar());
                    }
                }

                return recibo;
            }
            catch (Exception)
            {
                return recibo;
            }
        }

        private int ObterProtocolo(string recibo)
        {
            int retorno = 0;
            int protocolo = 0;

            try
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando = string.Format("select id_ato from certidoes where recibo = {0}", recibo);
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;


                        retorno = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }
                }

                using (FbConnection conn1 = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando1 = string.Format("select protocolo from titulos where id_ato = {0}", retorno);
                    conn1.Open();

                    using (FbCommand cmdTotal1 = new FbCommand(comando1, conn1))
                    {
                        cmdTotal1.CommandType = CommandType.Text;


                        protocolo = Convert.ToInt32(cmdTotal1.ExecuteScalar());
                    }
                }

                return protocolo;
            }
            catch (Exception)
            {
                return protocolo;
            }
        }


        private void ProcSalvar()
        {

            //if (txtConvenio.Text == "S" && txtVrTitulo.Text == "0,00")
            //{
            //    MessageBox.Show("Informe o Valor do Título.", "Valor do Título", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}



            Ato atoCorrente = new Ato();

            ClassAto classAto = new ClassAto();

            if (status == "alterar")
            {
                atoCorrente = atoSelecionado;
            }

            try
            {

                atoCorrente.DescricaoAto = "M";
                // data do pagamento
                if (datePickerDataPagamento.SelectedDate != null)
                {
                    atoCorrente.DataPagamento = datePickerDataPagamento.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data do Pagamento.", "Data do Pagamento", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataPagamento.Focus();
                    return;
                }

                // tipo de pagamento
                atoCorrente.TipoPagamento = cmbTipoPagamento.Text;


                // data do ato
                if (datePickerDataAto.SelectedDate != null)
                {
                    atoCorrente.DataAto = datePickerDataAto.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data do Ato.", "Data do Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataAto.Focus();
                    return;
                }


                // pago
                if (checkBoxPago.IsChecked == true)
                    atoCorrente.Pago = true;
                else
                    atoCorrente.Pago = false;


                // convenio
                atoCorrente.Convenio = txtConvenio.Text;


                atoCorrente.Faixa = txtletraFaixa.Text;



                if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
                {
                    if (txtProtRecibo.Text != "")
                    {
                        atoCorrente.Protocolo = Convert.ToInt32(txtProtRecibo.Text);

                        if (Principal.TipoAto == "APONTAMENTO")
                        {
                            if (cmbNatureza.SelectedIndex == 1)
                                atoCorrente.Recibo = ObterRecibo(txtProtRecibo.Text);
                            else
                                atoCorrente.Recibo = Convert.ToInt32(txtProtRecibo.Text);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Informe o Número do Protocolo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtProtRecibo.Focus();
                        return;
                    }

                }

                if (Principal.TipoAto == "CANCELAMENTO")
                {
                    if (txtProtRecibo.Text != "")
                    {
                        atoCorrente.Recibo = Convert.ToInt32(txtProtRecibo.Text);
                        atoCorrente.Protocolo = ObterProtocolo(txtProtRecibo.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe o Número do Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtProtRecibo.Focus();
                        return;
                    }
                }

                if (txtVrTitulo.Text != "")
                    atoCorrente.ValorTitulo = Convert.ToDecimal(txtVrTitulo.Text);
                else
                    atoCorrente.ValorTitulo = 0;

                if (status == "novo")
                {


                    if (usuarioLogado.Caixa == true || usuarioLogado.Master == true)
                    {
                        // IdUsuario
                        atoCorrente.IdUsuario = listaNomes.Where(p => p.NomeUsu == cmbNomes.Text).FirstOrDefault().Id_Usuario;

                        // Usuario
                        atoCorrente.Usuario = cmbNomes.Text;
                    }
                    else
                    {
                        // IdUsuario
                        atoCorrente.IdUsuario = usuarioLogado.Id_Usuario;

                        // Usuario
                        atoCorrente.Usuario = usuarioLogado.NomeUsu;
                    }

                }

                // Atribuiçao
                atoCorrente.Atribuicao = "PROTESTO";

                // Mensalista
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    if (cmbMensalista.SelectedIndex >= 0)
                    {
                        atoCorrente.Mensalista = cmbMensalista.Text;
                    }
                    else
                    {
                        MessageBox.Show("Informe o Nome do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                        cmbMensalista.Focus();
                        return;
                    }

                }

                //idReciboBalcao
                atoCorrente.IdReciboBalcao = 0;

                // Recibo Balcao
                atoCorrente.ReciboBalcao = 0;


                // TipoAto
                atoCorrente.TipoAto = Principal.TipoAto;

                if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
                {
                    if (Principal.TipoAto == "APONTAMENTO")
                    {
                        atoCorrente.TipoAto = cmbNatureza.Text;
                    }

                    // Natureza
                    if (cmbNatureza.SelectedIndex >= 0)
                        atoCorrente.Natureza = txtFaixa.Text;
                }
                if (Principal.TipoAto == "CANCELAMENTO")
                {
                    // Natureza
                    if (txtConvenio.Text == "S")
                    {

                        if (status == "novo")
                            atoCorrente.Natureza = "CANCELAMENTO " + faixa;
                        else
                        {
                            atoCorrente.Natureza = faixa;
                        }
                    }
                    else
                        atoCorrente.Natureza = Principal.TipoAto;
                }

                // Escrevente
                atoCorrente.Escrevente = cmbNomes.Text;

                // TipoCobranca
                atoCorrente.TipoCobranca = cmbTipoCustas.Text;


                //Emolumentos
                atoCorrente.Emolumentos = Convert.ToDecimal(txtEmol.Text);


                //Fetj
                atoCorrente.Fetj = Convert.ToDecimal(txtFetj.Text);

                //Fundperj
                atoCorrente.Fundperj = Convert.ToDecimal(txtFundperj.Text);


                //Funperj
                atoCorrente.Funperj = Convert.ToDecimal(txtFunperj.Text);


                //Funarpen
                atoCorrente.Funarpen = Convert.ToDecimal(txtFunarpen.Text);

                // Pmcmv
                atoCorrente.Pmcmv = Convert.ToDecimal(txtPmcmv.Text);

                //ISS
                atoCorrente.Iss = Convert.ToDecimal(txtIss.Text);


                // Mutua
                atoCorrente.Mutua = Convert.ToDecimal(txtMutua.Text);


                // Acoterj
                atoCorrente.Acoterj = Convert.ToDecimal(txtAcoterj.Text);


                // portador
                if (cmbPortador.SelectedIndex > -1)
                {
                    atoCorrente.Portador = cmbPortador.Text;
                }
                else
                {
                    MessageBox.Show("Informe o Portador.", "Portador", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbPortador.Focus();
                    return;
                }


                //NumeroRequisicao
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    if (txtRequisicao.Text != "")
                    {
                        atoCorrente.NumeroRequisicao = Convert.ToInt32(txtRequisicao.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe o Numero da requisição do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtRequisicao.Focus();
                        return;
                    }

                }

                // Tarifa
                atoCorrente.Bancaria = tarifa;


                // Total
                atoCorrente.Total = Convert.ToDecimal(txtTotal.Text);



                int idAto = classAto.SalvarAto(atoCorrente, status);

                SalvarItensCustas(idAto);
                var valorPago = new ValorPago();
                atoCorrente.Id_Ato = idAto;


                    if (status == "novo")
                    {
                        var valorPg = new ValorPago()
                        {
                            Data = atoCorrente.DataPagamento,
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

                        valorPg.IdAto = idAto;
                        valorPg.IdReciboBalcao = 0;

                        if (cmbTipoPagamento.SelectedIndex == 2)
                            valorPg.Mensalista = atoCorrente.Total;
                        else
                            valorPg.Mensalista = 0M;

                        classAto.SalvarValorPago(valorPg, status, "IdAto");
                    }
                    else
                    {
                        if (usuarioLogado.Caixa == true || usuarioLogado.Master == true || usuarioLogado.Protesto == true)
                        {
                            var valorPg = new ValorPago()
                            {
                                Data = atoCorrente.DataPagamento,
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

                            valorPg.IdAto = idAto;
                            valorPg.IdReciboBalcao = 0;

                            if (cmbTipoPagamento.SelectedIndex == 2)
                                valorPg.Mensalista = atoCorrente.Total;
                            else
                                valorPg.Mensalista = 0M;

                            classAto.SalvarValorPago(valorPg, status, "IdAto");
                        }
                    }

                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado, por favor verifique se o registro foi salvo. Se não foi salvo tente novamente. >>>" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }


        

        private void ProcSalvarCertidao()
        {
            Ato atoCorrente = new Ato();

            ClassAto classAto = new ClassAto();

            if (status == "alterar")
            {
                atoCorrente = atoSelecionado;
            }

            try
            {



                // data do pagamento
                if (datePickerDataPagamento.SelectedDate != null)
                {
                    atoCorrente.DataPagamento = datePickerDataPagamento.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data do Pagamento.", "Data do Pagamento", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataPagamento.Focus();
                    return;
                }

                // tipo de pagamento
                if (cmbNatureza.SelectedIndex >= 2)
                {
                    atoCorrente.TipoPagamento = "DEPÓSITO";
                }
                else
                {
                    atoCorrente.TipoPagamento = cmbTipoPagamento.Text;
                }

                // data do ato
                if (datePickerDataAto.SelectedDate != null)
                {
                    atoCorrente.DataAto = datePickerDataAto.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data do Ato.", "Data do Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataAto.Focus();
                    return;
                }


                if (cmbNatureza.SelectedIndex >= 2)
                {
                    atoCorrente.Pago = true;
                }
                else
                {
                    // pago
                    if (checkBoxPago.IsChecked == true)
                        atoCorrente.Pago = true;
                    else
                        atoCorrente.Pago = false;
                }

                //// convenio
                //atoCorrente.Convenio = txtConvenio.Text;

                atoCorrente.Faixa = txtQtdNomes.Text;

                atoCorrente.Protocolo = 0;

                if (txtProtRecibo.Text != "")
                {
                    atoCorrente.Recibo = Convert.ToInt32(txtProtRecibo.Text);
                }
                else
                {
                    MessageBox.Show("Informe o Número do Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtProtRecibo.Focus();
                    return;
                }


                // atoCorrente.ValorTitulo = Convert.ToDecimal(txtVrTitulo.Text);



                if (status == "novo")
                {


                    if (usuarioLogado.Caixa == true || usuarioLogado.Master == true)
                    {
                        // IdUsuario
                        atoCorrente.IdUsuario = listaNomes.Where(p => p.NomeUsu == cmbNomes.Text).FirstOrDefault().Id_Usuario;

                        // Usuario
                        atoCorrente.Usuario = cmbNomes.Text;
                    }
                    else
                    {
                        // IdUsuario
                        atoCorrente.IdUsuario = usuarioLogado.Id_Usuario;

                        // Usuario
                        atoCorrente.Usuario = usuarioLogado.NomeUsu;
                    }

                }

                // Atribuiçao
                atoCorrente.Atribuicao = "PROTESTO";

                // Mensalista
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    if (cmbMensalista.SelectedIndex >= 0)
                    {
                        atoCorrente.Mensalista = cmbMensalista.Text;
                    }
                    else
                    {
                        MessageBox.Show("Informe o Nome do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                        cmbMensalista.Focus();
                        return;
                    }

                }

                //idReciboBalcao
                atoCorrente.IdReciboBalcao = 0;

                // Recibo Balcao
                atoCorrente.ReciboBalcao = 0;


                // TipoAto
                atoCorrente.TipoAto = Principal.TipoAto;


                // Natureza
                if (cmbNatureza.SelectedIndex >= 0)
                    atoCorrente.Natureza = cmbNatureza.Text;

                atoCorrente.Convenio = "N";

                // Escrevente
                atoCorrente.Escrevente = cmbNomes.Text;

                // TipoCobranca
                atoCorrente.TipoCobranca = cmbTipoCustas.Text;


                //Emolumentos
                atoCorrente.Emolumentos = Convert.ToDecimal(txtEmol.Text);


                //Fetj
                atoCorrente.Fetj = Convert.ToDecimal(txtFetj.Text);

                //Fundperj
                atoCorrente.Fundperj = Convert.ToDecimal(txtFundperj.Text);


                //Funperj
                atoCorrente.Funperj = Convert.ToDecimal(txtFunperj.Text);


                //Funarpen
                atoCorrente.Funarpen = Convert.ToDecimal(txtFunarpen.Text);

                // Pmcmv
                atoCorrente.Pmcmv = Convert.ToDecimal(txtPmcmv.Text);


                // Mutua
                atoCorrente.Mutua = Convert.ToDecimal(txtMutua.Text);


                // Acoterj
                atoCorrente.Acoterj = Convert.ToDecimal(txtAcoterj.Text);


                atoCorrente.Iss = Convert.ToDecimal(txtIss.Text);


                //NumeroRequisicao
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    if (txtRequisicao.Text != "")
                    {
                        atoCorrente.NumeroRequisicao = Convert.ToInt32(txtRequisicao.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe o Numero da requisição do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtRequisicao.Focus();
                        return;
                    }

                }


                // Total
                atoCorrente.Total = Convert.ToDecimal(txtTotal.Text);


                int idAto = classAto.SalvarAto(atoCorrente, status);

                SalvarItensCustas(idAto);

                atoCorrente.Id_Ato = idAto;

                if (status == "novo")
                {
                    var valorPago = new ValorPago()
                    {
                        Data = atoCorrente.DataPagamento,
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
                        valorPago.Mensalista = atoCorrente.Total;
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
                            Data = atoCorrente.DataPagamento,
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
                            valorPago.Mensalista = atoCorrente.Total;
                        else
                            valorPago.Mensalista = 0M;

                        classAto.SalvarValorPago(valorPago, status, "IdAto");
                    }

                    if (cmbNatureza.SelectedIndex >= 2)
                    {
                        if (MessageBox.Show("Deseja Imprimir o Recibo?", "Recibo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            FrmRecibo frmRecibo = new FrmRecibo(txtProtRecibo.Text, cmbNatureza.Text, datePickerDataAto.SelectedDate.Value.ToShortDateString(), txtQtdNomes.Text, txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtTotal.Text, cmbNomes.Text);
                            frmRecibo.ShowDialog();
                            frmRecibo.Dispose();
                        }
                    }
                    
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado, por favor verifique se o registro foi salvo. Se não foi salvo tente novamente. >>>" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

        }


        private void SalvarItensCustas(int idAto)
        {
            ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();

            if (status == "alterar")
            {
                classCustasProtesto.RemoverItensCustas(idAto);
            }


            for (int cont = 0; cont <= listaItens.Count - 1; cont++)
            {
                ItensCustasProtesto item = new ItensCustasProtesto();

                item.Id_Ato = idAto;

                item.Tabela = listaItens[cont].Tabela;

                item.Item = listaItens[cont].Item;

                item.SubItem = listaItens[cont].SubItem;

                item.Quantidade = listaItens[cont].Quantidade;

                item.Valor = listaItens[cont].Valor;

                item.Total = listaItens[cont].Total;

                item.Descricao = listaItens[cont].Descricao;

                classCustasProtesto.SalvarItensLista(item);

            }

        }

        private void txtLetraSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
        }



        private void txtVrTitulo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtVrTitulo.Text == "0,00")
                txtVrTitulo.Text = "";

        }

        private void txtVrTitulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtVrTitulo.Text != "")
                txtVrTitulo.Text = string.Format("{0:n2}", Convert.ToDecimal(txtVrTitulo.Text));
            else
                txtVrTitulo.Text = "0,00";

            if (txtVrTitulo.Text != "" && txtVrTitulo.Text != "0,00")
            {
                txtletraFaixa.Text = Faixa(Convert.ToDouble(txtVrTitulo.Text));

                if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
                {

                    CalcularItensCustas();

                    if (Principal.TipoAto == "APONTAMENTO")
                    {
                        CalcularValoresApontamento();
                    }
                    if (Principal.TipoAto == "PAGAMENTO")
                    {
                        CalcularValoresPagamento();
                    }

                    btnSalvar.IsEnabled = true;


                    if (listaItens.Count > 0)
                    {
                        btnCustas.IsEnabled = true;
                    }
                    else
                    {
                        btnCustas.IsEnabled = false;
                    }

                }

                HabilitaTipoPagamento();
            }

        }

        private void txtVrTitulo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;



            if (txtVrTitulo.Text.Length > 0)
            {
                if (txtVrTitulo.Text.Contains(","))
                {
                    int index = txtVrTitulo.Text.IndexOf(",");

                    if (txtVrTitulo.Text.Length == index + 3)
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


        private void cmbPortador_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
            {
                Portador convenio = (Portador)portador[cmbPortador.SelectedIndex];

                txtConvenio.Text = convenio.CONVENIO;


                if (cmbPortador.SelectedIndex > -1)
                {
                    txtVrTitulo.IsEnabled = true;
                }
                else
                {
                    txtVrTitulo.IsEnabled = false;
                }

                tabItemCustas.IsSelected = true;

                string codigoPortador = portador[cmbPortador.SelectedIndex].CODIGO;
                if (status == "novo")
                {
                    if (codigoPortador == "000" || codigoPortador == "104")
                    {
                        cmbTipoPagamento.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbTipoPagamento.SelectedIndex = 1;
                    }


                    if (txtVrTitulo.Text != "0,00" && txtVrTitulo.Text != "")
                    {
                        CalcularItensCustas();
                        if (Principal.TipoAto == "APONTAMENTO")
                        {
                            CalcularValoresApontamento();
                        }
                        if (Principal.TipoAto == "PAGAMENTO")
                        {
                            CalcularValoresPagamento();
                        }
                        if (Principal.TipoAto == "CANCELAMENTO")
                        {
                            CalcularValoresCancelamento();

                        }
                    }
                }
                else
                {


                    CalcularItensCustas();
                    if (Principal.TipoAto == "APONTAMENTO")
                    {
                        CalcularValoresApontamento();
                    }
                    if (Principal.TipoAto == "PAGAMENTO")
                    {
                        CalcularValoresPagamento();
                    }
                    if (Principal.TipoAto == "CANCELAMENTO")
                    {
                        CalcularValoresCancelamento();
                    }
                }
            }


            if (Principal.TipoAto == "CANCELAMENTO")
            {
                Portador convenio = (Portador)portador[cmbPortador.SelectedIndex];

                txtConvenio.Text = convenio.CONVENIO;

                btnCustas.IsEnabled = true;

                btnSalvar.IsEnabled = true;

                if (txtConvenio.Text == "N")
                {
                    txtVrTitulo.IsEnabled = false;
                    txtVrTitulo.Text = "0,00";
                    txtletraFaixa.Text = "";
                    txtFaixa.Text = "";
                }
                else
                {
                    txtVrTitulo.IsEnabled = true;
                    txtletraFaixa.Text = "";
                    txtFaixa.Text = "";
                }

                tabItemCustas.IsSelected = true;

                CalcularItensCustas();

                if (Principal.TipoAto == "APONTAMENTO")
                {
                    CalcularValoresApontamento();
                }
                if (Principal.TipoAto == "PAGAMENTO")
                {
                    CalcularValoresPagamento();
                }

                if (Principal.TipoAto == "CANCELAMENTO")
                {
                    CalcularValoresCancelamento();
                }

            }

            HabilitaTipoPagamento();
        }


        public string Faixa(double Valor)
        {
            string letra = string.Empty;

            if (Valor <= 50.00)
            {
                letra = "A";
            }
            else if (Valor <= 100.00)
            {
                letra = "B";
            }
            else if (Valor <= 150.00)
            {
                letra = "C";
            }
            else if (Valor <= 200.00)
            {
                letra = "D";
            }
            else if (Valor <= 250.00)
            {
                letra = "E";
            }
            else if (Valor <= 300.00)
            {
                letra = "F";
            }
            else if (Valor <= 350.00)
            {
                letra = "G";
            }
            else if (Valor <= 400.00)
            {
                letra = "H";
            }
            else if (Valor <= 450.00)
            {
                letra = "I";
            }
            else if (Valor <= 500.00)
            {
                letra = "J";
            }
            else if (Valor <= 600.00)
            {
                letra = "K";
            }
            else if (Valor <= 700.00)
            {
                letra = "L";
            }
            else if (Valor <= 800.00)
            {
                letra = "M";
            }
            else if (Valor <= 900.00)
            {
                letra = "N";
            }
            else if (Valor <= 1000.00)
            {
                letra = "O";
            }
            else if (Valor <= 1500.00)
            {
                letra = "P";
            }
            else if (Valor <= 2000.00)
            {
                letra = "Q";
            }
            else if (Valor <= 2500.00)
            {
                letra = "R";
            }
            else if (Valor <= 3000.00)
            {
                letra = "S";
            }
            else if (Valor <= 3500.00)
            {
                letra = "T";
            }
            else if (Valor <= 4000.00)
            {
                letra = "U";
            }
            else if (Valor <= 4500.00)
            {
                letra = "V";
            }
            else if (Valor <= 5000.00)
            {
                letra = "W";
            }
            else if (Valor <= 7500.00)
            {
                letra = "X";
            }
            else if (Valor <= 10000.00)
            {
                letra = "Y";
            }
            else if (Valor > 10000.00)
            {
                letra = "Z";
            }

            return letra;
        }

        private void txtletraFaixa_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtFaixa.Text = listaCustas.Where(p => p.SUB == txtletraFaixa.Text).Select(p => p.DESCR).FirstOrDefault();
        }


        private void txtVrTitulo_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            txtVrTitulo.Background = Brushes.White;

            if (txtVrTitulo.Text.Length > 0)
            {
                if (txtVrTitulo.Text.Contains(","))
                {
                    int index = txtVrTitulo.Text.IndexOf(",");

                    if (txtVrTitulo.Text.Length == index + 3)
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


        private void txtProtRecibo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void btnCustas_Click(object sender, RoutedEventArgs e)
        {
            WinCustasProtesto winCustasNotas = new WinCustasProtesto(this, Principal);
            winCustasNotas.Owner = this;

            winCustasNotas.ShowDialog();

            HabilitaTipoPagamento();
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbNatureza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNatureza.Focus() == true)
            {
                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    btnCustas.IsEnabled = true;

                    if (cmbNatureza.SelectedIndex >= 2)
                    {
                        stackPanelQdtNomes.Visibility = Visibility.Visible;

                        CalcularCertidaoSerasaBoaVista();
                    }
                    else
                    {
                        stackPanelQdtNomes.Visibility = Visibility.Hidden;
                        CalcularItensCustas();
                        CalcularValoresCertidao();
                    }
                    HabilitaTipoPagamento();
                }


                if (Principal.TipoAto == "APONTAMENTO" || Principal.TipoAto == "PAGAMENTO")
                {

                    if (txtVrTitulo.Text != "" && txtVrTitulo.Text != "0,00")
                    {
                        CalcularItensCustas();

                        if (Principal.TipoAto == "APONTAMENTO")
                        {
                            CalcularValoresApontamento();
                        }
                        if (Principal.TipoAto == "PAGAMENTO")
                        {
                            CalcularValoresPagamento();
                        }

                        btnSalvar.IsEnabled = true;


                        if (listaItens.Count > 0)
                        {
                            btnCustas.IsEnabled = true;
                        }
                        else
                        {
                            btnCustas.IsEnabled = false;
                        }

                    }
                    else
                    {
                        txtVrTitulo.Focus();
                    }

                }

                if (Principal.TipoAto == "CANCELAMENTO")
                {

                    CalcularItensCustas();
                    CalcularValoresCancelamento();
                }

                if (listaItens.Count > 0)
                {
                    btnCustas.IsEnabled = true;
                }
                else
                {
                    btnCustas.IsEnabled = false;
                }



                HabilitaTipoPagamento();
            }
        }

        private void txtQtd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdNomes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdNomes_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdNomes.Text == "0")
            {
                txtQtdNomes.Text = "";
            }
        }

        private void txtQtdNomes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdNomes.Text == "")
            {
                txtQtdNomes.Text = "0";
            }
            CalcularCertidaoSerasaBoaVista();

            HabilitaTipoPagamento();
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
                    if (Convert.ToDecimal(lblTotal.Content) != (Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text) + Convert.ToDecimal(txtValorPagoCartaoCredito.Text)))
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

        private void Grid_PreviewKeyDown_1(object sender, KeyEventArgs e)
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



    }
}
