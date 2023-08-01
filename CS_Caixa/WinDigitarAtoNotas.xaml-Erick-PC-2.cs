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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinDigitarEscritura.xaml
    /// </summary>
    public partial class WinDigitarAtoNotas : Window
    {
        WinPrincipal Principal;
        List<CadMensalista> listaMensalista = new List<CadMensalista>();
        List<Usuario> listaNomes = new List<Usuario>();
        Usuario usuarioLogado;
        public List<CustasNota> listaCustas = new List<CustasNota>();
        public List<ItensCustasNota> listaItens = new List<ItensCustasNota>();
        public List<CustasNota> listaCustasItens = new List<CustasNota>();
        public List<CustasDistribuicao> listaDistribuicao = new List<CustasDistribuicao>();
        public CustasNota emolumentos;
        public decimal emolLista;
        public int ano = DateTime.Now.Year;
        public decimal mutua = 0;
        public decimal acoterj = 0;
        public decimal indisponibilidade = 0;
        string status;
        Ato atoSelecionado = new Ato();
        WinAtosNotas _digitarNotas;
        ClassEnotariado enot = new ClassEnotariado();
        Enotariado enotariadoAlterar;
        string totalInicial = string.Empty;
        bool masterParaSalvar = false;
        public bool senhaConfirmada = false;
        DateTime dataAto = new DateTime();
        DateTime dataPagamento = new DateTime();

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public decimal porcentagemIss;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        public WinDigitarAtoNotas(WinPrincipal Principal, Usuario usuarioLogado, string status)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            InitializeComponent();
        }


        public WinDigitarAtoNotas(WinPrincipal Principal, Usuario usuarioLogado, string status, WinAtosNotas digitarAtoNotas, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            this.atoSelecionado = digitarAtoNotas.atoSelecionado;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            _digitarNotas = digitarAtoNotas;
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

            if (Principal.TipoAto == "ESCRITURA")
                this.Background = (Brush)bc.ConvertFrom("#FFD3E9EF");

            if (Principal.TipoAto == "PROCURAÇÃO")
                this.Background = (Brush)bc.ConvertFrom("#FFE2E0BA");

            if (Principal.TipoAto == "TESTAMENTO")
                this.Background = (Brush)bc.ConvertFrom("#FF7FD3C0");

            if (Principal.TipoAto == "CERTIDÃO NOTAS")
                this.Background = (Brush)bc.ConvertFrom("#FFEBDED0");

            if (Principal.TipoAto == "APOSTILAMENTO HAIA")
                this.Background = (Brush)bc.ConvertFrom("#FFC8EED2");

            ClassMensalista classMensalista = new ClassMensalista();
            ClassUsuario classUsuario = new ClassUsuario();
            ClassCustasNotas classCustasNotas = new ClassCustasNotas();
            listaMensalista = classMensalista.ListaMensalistas();
            listaNomes = classUsuario.ListaUsuarios();
            cmbNomes.ItemsSource = listaNomes.Select(p => p.NomeUsu);

            cmbNomes.SelectedItem = usuarioLogado.NomeUsu;
            listaCustas = classCustasNotas.ListaCustas();
            listaCustasItens = classCustasNotas.ListaCustas();

            listaDistribuicao = classCustasNotas.ListaCustasDistribuicao();



            List<CustasNota> atulizado = listaCustas.Where(p => p.ANO == ano).ToList();
            if (status == "novo")
            {
                if (atulizado.Count > 0)
                {
                    if (DateTime.Now.Month == 1 && DateTime.Now.Day < 10)
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

                if (DateTime.Now.Month == 1 && DateTime.Now.Day < 10)
                {
                    if (MessageBox.Show("Deseja utilizar as custas do ano deste ato?", "Custas", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        ano = atoSelecionado.DataAto.Year;
                    }

                }

            }

            porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
            mutua = Convert.ToDecimal(listaCustas.Where(p => p.TEXTO == "MUTUA" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
            acoterj = Convert.ToDecimal(listaCustas.Where(p => p.TEXTO == "ACOTERJ" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
            indisponibilidade = Convert.ToDecimal(listaCustas.Where(p => p.TEXTO == "INDISPONIBILIDADE" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
            listaDistribuicao = listaDistribuicao.Where(p => p.Ano == ano).OrderBy(p => p.Quant_Exced).ToList();


            if (Principal.TipoAto == "ESCRITURA")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "E").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                txtQtdAtos.IsEnabled = true;
                txtQtdAtos.Text = "1";
                label34.Visibility = Visibility.Visible;
                txtNatureza.Visibility = Visibility.Visible;
                ckboxEnotariado.IsEnabled = true;
            }

            if (Principal.TipoAto == "PROCURAÇÃO")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "P").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                txtVrCorretor.IsEnabled = false;
                txtVrEscrevente.IsEnabled = false;
                txtBaseCalculo.IsEnabled = false;
                txtQtdAtos.IsEnabled = true;
                txtQtdAtos.Text = "1";
                ckboxEnotariado.IsEnabled = true;
            }

            if (Principal.TipoAto == "TESTAMENTO")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "T").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                txtVrCorretor.IsEnabled = false;
                txtVrEscrevente.IsEnabled = false;
                txtBaseCalculo.IsEnabled = false;
                txtQtdAtos.IsEnabled = true;
                txtQtdAtos.Text = "1";
            }

            if (Principal.TipoAto == "CERTIDÃO NOTAS")
            {
                label22.Content = "Recibo:";
                txtVrCorretor.IsEnabled = false;
                txtVrEscrevente.IsEnabled = false;
                txtBaseCalculo.Text = "0";
                txtQtdExced.IsEnabled = false;
                txtQtdIndisp.IsEnabled = false;
                txtQtdAtos.Text = "0";
            }


            if (Principal.TipoAto == "APOSTILAMENTO HAIA")
            {
                listaCustas = listaCustas.Where(p => p.ANO == ano && p.DESCR == "APOSTILAMENTO DE HAIA").Select(p => p).ToList();
                label22.Content = "Recibo:";
                txtVrCorretor.IsEnabled = false;
                txtVrEscrevente.IsEnabled = false;
                txtBaseCalculo.Text = "0";
                txtQtdExced.IsEnabled = false;
                txtQtdIndisp.IsEnabled = false;
                txtQtdAtos.Text = "0";
                txtLivro.IsEnabled = false;
                txtFlsInicial.IsEnabled = false;
                txtFlsFinal.IsEnabled = false;
                txtAto.IsEnabled = false;
                txtLetraSelo.IsEnabled = false;
                txtNumeroSelo.IsEnabled = false;
                txtAleatorio.IsEnabled = false;
                txtBaseCalculo.IsEnabled = false;


                txtFlsApostilamento.Visibility = Visibility.Visible;
                lblFolhaApostilamento.Visibility = Visibility.Visible;
            }

            groupBox1.Content = Principal.TipoAto;

            listaCustasItens = listaCustasItens.Where(p => p.ANO == ano && p.VAI == "S").OrderBy(p => p.ORDEM).Select(p => p).ToList();

            if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
            {
                cmbNatureza.ItemsSource = listaCustas.Select(p => p.DESCR).ToList();
            }
            else
            {
                if (Principal.TipoAto == "APOSTILAMENTO HAIA")
                {
                    cmbNatureza.Items.Add("APOSTILAMENTO DE HAIA");
                    if (status == "novo")
                        cmbNatureza.SelectedIndex = 0;
                }
                else
                {
                    cmbNatureza.Items.Add("CERTIDÃO DE ESCRITURA");
                    cmbNatureza.Items.Add("CERTIDÃO DE PROCURAÇÃO");
                    cmbNatureza.Items.Add("CERTIDÃO DE TESTAMENTO");
                    cmbNatureza.Items.Add("CERTIDÃO NOTAS");
                }
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

            cmbMensalista.ItemsSource = listaMensalista.Select(p => p.Nome);

            if (status == "alterar")
            {

                listaItens = classCustasNotas.ListarItensCustas(atoSelecionado.Id_Ato);

                if (listaItens.Count() > 0)
                {
                    emolumentos = new CustasNota();
                    emolumentos.ITEM = listaItens[0].Item;
                    emolumentos.SUB = listaItens[0].SubItem;
                    emolumentos.DESCR = listaItens[0].Descricao;
                    emolumentos.TEXTO = listaItens[0].Descricao;
                    emolumentos.VALOR = listaItens[0].Total;
                }
                else
                {
                    if (Principal.TipoAto == "CERTIDÃO NOTAS")
                    {
                        listaItens = new List<ItensCustasNota>();
                        ItensCustasNota novoIten = new ItensCustasNota();


                        novoIten = new ItensCustasNota();
                        var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS" && p.ANO == ano).FirstOrDefault();
                        novoIten.Item = arqrivDesarquiv.ITEM;
                        novoIten.SubItem = arqrivDesarquiv.SUB;
                        novoIten.Tabela = arqrivDesarquiv.TAB;
                        novoIten.Descricao = arqrivDesarquiv.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = arqrivDesarquiv.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);


                        novoIten = new ItensCustasNota();
                        var expedicao_emissao = (CustasNota)listaCustasItens.Where(p => p.DESCR == "BUSCAS EM LIVROS OU PAPÉIS" && p.ANO == ano).FirstOrDefault();
                        novoIten.Item = expedicao_emissao.ITEM;
                        novoIten.SubItem = expedicao_emissao.SUB;
                        novoIten.Tabela = expedicao_emissao.TAB;
                        novoIten.Descricao = expedicao_emissao.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(expedicao_emissao.VALOR);
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);

                        novoIten = new ItensCustasNota();
                        var arquivamento = (CustasNota)listaCustasItens.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO" && p.ANO == ano).FirstOrDefault();
                        novoIten.Item = arquivamento.ITEM;
                        novoIten.SubItem = arquivamento.SUB;
                        novoIten.Tabela = arquivamento.TAB;
                        novoIten.Descricao = arquivamento.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(arquivamento.VALOR);
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);
                    }
                }
                CarregaCamposAlterar();

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

            //HabilitaTipoPagamento();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();
        }

        private void CarregaCamposAlterar()
        {
            datePickerDataAto.SelectedDate = atoSelecionado.DataAto;
            txtLivro.Text = atoSelecionado.Livro;
            txtFlsInicial.Text = string.Format("{0:000}", atoSelecionado.FolhaInical);
            txtFlsFinal.Text = string.Format("{0:000}", atoSelecionado.FolhaFinal);

            if (Principal.TipoAto == "APOSTILAMENTO HAIA")
                txtFlsApostilamento.Text = atoSelecionado.DescricaoAto;


            txtAto.Text = string.Format("{0:000}", atoSelecionado.NumeroAto);
            txtLetraSelo.Text = atoSelecionado.LetraSelo;
            txtNumeroSelo.Text = string.Format("{0:00000}", atoSelecionado.NumeroSelo);
            txtAleatorio.Text = atoSelecionado.Aleatorio;
            cmbNomes.SelectedItem = atoSelecionado.Escrevente;
            cmbTipoCustas.Text = atoSelecionado.TipoCobranca;
            if (cmbTipoCustas.SelectedIndex <= 1)
            {
                cmbTipoPagamento.Text = atoSelecionado.TipoPagamento;
            }
            if (cmbTipoPagamento.Text == "MENSALISTA")
            {
                cmbMensalista.Text = atoSelecionado.Mensalista;
                txtRequisicao.Text = atoSelecionado.NumeroRequisicao.ToString();
            }

            if (atoSelecionado.ValorEscrevente.ToString() != "")
                txtVrEscrevente.Text = string.Format("{0:n2}", atoSelecionado.ValorEscrevente);
            if (atoSelecionado.ValorCorretor.ToString() != "")
                txtVrCorretor.Text = string.Format("{0:n2}", atoSelecionado.ValorCorretor);
            if (atoSelecionado.ValorDesconto.ToString() != "")
                txtDesconto.Text = string.Format("{0:n2}", atoSelecionado.ValorDesconto);
            if (atoSelecionado.ValorAdicionar.ToString() != "")
                txtAdicionar.Text = string.Format("{0:n2}", atoSelecionado.ValorAdicionar);

            if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                txtBaseCalculo.Text = string.Format("{0:n2}", atoSelecionado.ValorTitulo);
            else
                txtBaseCalculo.Text = atoSelecionado.Recibo.ToString();



            enotariadoAlterar = enot.ObterPorIdAto(atoSelecionado.Id_Ato);

            if (enotariadoAlterar != null)
            {


                if (enotariadoAlterar.Valor > 0)
                {
                    ckboxEnotariado.IsChecked = true;
                    txtEnotariado.IsEnabled = true;
                    txtEnotariado.Text = string.Format("{0:n2}", enotariadoAlterar.Valor);
                }
                else
                {
                    ckboxEnotariado.IsChecked = false;
                    txtEnotariado.IsEnabled = false;

                }
            }
            else
            {
                txtEnotariado.Text = "0,00";
            }

            txtNatureza.Text = atoSelecionado.Natureza;

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

            if (atoSelecionado.QuantIndisp.ToString() != "")
                txtQtdIndisp.Text = atoSelecionado.QuantIndisp.ToString();

            if (atoSelecionado.QuantDistrib.ToString() != "")
                txtQtdExced.Text = atoSelecionado.QuantDistrib.ToString();

            ClassAto classAto = new ClassAto();

            var valorPago = classAto.ObterValorPagoPorIdAto(atoSelecionado.Id_Ato);



            checkBoxPago.IsChecked = atoSelecionado.Pago;

            txtQtdAtos.Text = string.Format("{0}", atoSelecionado.QtdAtos);


            if (Principal.TipoAto == "ESCRITURA")
                cmbNatureza.SelectedItem = atoSelecionado.Faixa;
            else
                cmbNatureza.SelectedItem = atoSelecionado.Natureza;




            if (valorPago != null)
            {
                txtValorPagoDinheiro.Text = string.Format("{0:n2}", valorPago.Dinheiro);
                txtValorPagoDeposito.Text = string.Format("{0:n2}", valorPago.Deposito);
                txtValorPagoCheque.Text = string.Format("{0:n2}", valorPago.Cheque);
                txtValorPagoChequePre.Text = string.Format("{0:n2}", valorPago.ChequePre);
                txtValorPagoBoleto.Text = string.Format("{0:n2}", valorPago.Boleto);

            }

            totalInicial = txtTotal.Text;
            masterParaSalvar = false;
            dataAto = atoSelecionado.DataAto;
            dataPagamento = atoSelecionado.DataPagamento;
        }

       

        private void tabControl1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassaUmControleParaOutro(sender, e);
        }

        private void PassaUmControleParaOutro(object sender, KeyEventArgs e)
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

        private void txtVrEscrevente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;


            if (txtVrEscrevente.Text.Length > 0)
            {
                if (txtVrEscrevente.Text.Contains(","))
                {
                    int index = txtVrEscrevente.Text.IndexOf(",");

                    if (txtVrEscrevente.Text.Length == index + 3)
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

        private void txtVrEscrevente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtVrEscrevente.Text != "")
            {
                try
                {
                    txtVrEscrevente.Text = string.Format("{0:n2}", Convert.ToDecimal(txtVrEscrevente.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtVrEscrevente.Text = "0,00";
            }

            if (cmbNatureza.SelectedIndex > -1)
            {
                txtQtdExced.IsEnabled = true;
                txtQtdIndisp.IsEnabled = true;


                CalcularValores();

            }
        }

        
        private void txtVrCorretor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtVrCorretor.Text.Length > 0)
            {
                if (txtVrCorretor.Text.Contains(","))
                {
                    int index = txtVrCorretor.Text.IndexOf(",");

                    if (txtVrCorretor.Text.Length == index + 3)
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

        private void txtVrCorretor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtVrCorretor.Text != "")
            {
                try
                {
                    txtVrCorretor.Text = string.Format("{0:n2}", Convert.ToDecimal(txtVrCorretor.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtVrCorretor.Text = "0,00";
            }

            if (cmbNatureza.SelectedIndex > -1)
            {
                txtQtdExced.IsEnabled = true;
                txtQtdIndisp.IsEnabled = true;

                CalcularValores();


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

            if (cmbNatureza.SelectedIndex > -1)
            {
                txtQtdExced.IsEnabled = true;
                txtQtdIndisp.IsEnabled = true;

                CalcularValores();


            }

            HabilitaTipoPagamento();
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

            if (cmbNatureza.SelectedIndex > -1)
            {
                txtQtdExced.IsEnabled = true;
                txtQtdIndisp.IsEnabled = true;

                CalcularValores();


            }

            HabilitaTipoPagamento();
        }

        private void txtBaseCalculo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;


            if (Principal.TipoAto != "CERTIDÃO NOTAS")
            {
                if (txtBaseCalculo.Text.Length > 0)
                {
                    if (txtBaseCalculo.Text.Contains(","))
                    {
                        int index = txtBaseCalculo.Text.IndexOf(",");

                        if (txtBaseCalculo.Text.Length == index + 3)
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
        }

        private void txtBaseCalculo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Principal.TipoAto != "CERTIDÃO NOTAS")
            {
                if (txtBaseCalculo.Text != "")
                {
                    try
                    {
                        txtBaseCalculo.Text = string.Format("{0:n2}", Convert.ToDecimal(txtBaseCalculo.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtBaseCalculo.Text = "0,00";
                }
            }
            else
            {
                if (txtBaseCalculo.Text == "")
                {
                    txtBaseCalculo.Text = "0";
                }
            }
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            //WinAtosNotas escritura = new WinAtosNotas(usuarioLogado, Principal, atoSelecionado, dataInicioConsulta, dataFimConsulta);
            //escritura.Owner = Principal;
            //escritura.ShowDialog();
        }

        private void cmbNatureza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNatureza.Focus())
            {
                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    CalcularItensCustas();
                }
                else
                {
                    if (btnCustas.IsEnabled == false && status == "novo")
                    {
                        CalcularItensCustas();
                    }
                    else
                        CalcularValores();

                }

                HabilitaTipoPagamento();
            }

            if (cmbNatureza.SelectedIndex > -1)
            {
                btnSalvar.IsEnabled = true;

                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {

                    txtQtdExced.IsEnabled = true;
                    txtQtdIndisp.IsEnabled = true;
                }
                btnCustas.IsEnabled = true;

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {

                    checkBoxPago.IsEnabled = true;
                }
                else
                {

                    checkBoxPago.IsEnabled = false;
                }


            }

        }

        private void CalcularValores()
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss = 0;
            decimal indisp = 0;

            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Spmcmv_2 = "0,00";
            string Siss = "0,00";
            int index;





            var distrib = listaDistribuicao.Where(p => p.Quant_Exced == Convert.ToDecimal(txtQtdExced.Text)).Select(p => p.Total).FirstOrDefault();
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

                    if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                        pmcmv_2 = Convert.ToDecimal(emolumentos.VALOR * 2) / 100;

                    if (cmbTipoCustas.SelectedIndex == 0)
                    {
                        Semol = Convert.ToString(emol);
                    }


                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Siss = Convert.ToString(iss);
                    if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                        Spmcmv_2 = Convert.ToString(pmcmv_2);



                    indisp = indisponibilidade * Convert.ToInt16(txtQtdIndisp.Text);

                }

                if (cmbTipoCustas.SelectedIndex > 1)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;
                    if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                        pmcmv_2 = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Spmcmv_2 = "0,00";
                    Siss = "0,00";

                    indisp = 0;

                    distrib = 0;


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

                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);
                }

                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    txtEmol.Text = Semol;
                    txtFetj.Text = Sfetj_20;
                    txtFundperj.Text = Sfundperj_5;
                    txtFunperj.Text = Sfunperj_5;
                    txtFunarpen.Text = Sfunarpen_4;
                    txtIss.Text = Siss;
                }
                else
                {
                    txtEmol.Text = "0,00";
                    txtFetj.Text = "0,00";
                    txtFundperj.Text = "0,00";
                    txtFunperj.Text = "0,00";
                    txtFunarpen.Text = "0,00";
                    txtIss.Text = Siss;
                }

                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                    txtPmcmv.Text = Spmcmv_2;

                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    txtIndisp.Text = string.Format("{0:n2}", indisp);
                    txtDistribuicao.Text = string.Format("{0:n2}", distrib);
                }
                else
                {
                    txtIndisp.Text = "0,00";
                    txtDistribuicao.Text = "0,00";
                }

                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (cmbTipoCustas.SelectedIndex <= 1)
                    {
                        txtMutua.Text = string.Format("{0:n2}", mutua * Convert.ToInt32(txtQtdAtos.Text));
                        txtAcoterj.Text = string.Format("{0:n2}", acoterj * Convert.ToInt32(txtQtdAtos.Text));
                    }
                    else
                    {
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }
                }

                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    txtTotal.Text = CalcularTotal(txtAdicionar.Text, txtDesconto.Text, txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, txtEnotariado.Text);
                    lblTotal.Content = string.Format("{0}", txtTotal.Text);
                    lblTotal1.Content = lblTotal.Content;
                }
                else
                {
                    txtTotal.Text = CalcularTotal(txtAdicionar.Text, txtDesconto.Text, Semol, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, txtEnotariado.Text);
                    lblTotal.Content = string.Format("{0}", txtTotal.Text);
                    lblTotal1.Content = lblTotal.Content;
                }
                HabilitaTipoPagamento();
                CalculaTroco();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public string CalcularTotal(string txtAdicionar, string txtDesconto, string txtEmol, string txtFetj, string txtFundperj, string txtFunperj, string txtFunarpen, string txtPmcmv, string txtIss, string txtIndisp, string txtDistribuicao, string txtMutua, string txtAcoterj, string txtEnotariado)
        {
            try
            {
                decimal Adicionar = Convert.ToDecimal(txtAdicionar);
                decimal Desconto = Convert.ToDecimal(txtDesconto);
                decimal emolumento = Convert.ToDecimal(txtEmol);
                decimal Fetj = Convert.ToDecimal(txtFetj);
                decimal Fundperj = Convert.ToDecimal(txtFundperj);
                decimal Funperj = Convert.ToDecimal(txtFunperj);
                decimal Funarpen = Convert.ToDecimal(txtFunarpen);
                decimal Pmcmv = Convert.ToDecimal(txtPmcmv);
                decimal Iss = Convert.ToDecimal(txtIss);
                decimal Indisp = Convert.ToDecimal(txtIndisp);
                decimal Distribuicao = Convert.ToDecimal(txtDistribuicao);
                decimal Mutua = Convert.ToDecimal(txtMutua);
                decimal Acoterj = Convert.ToDecimal(txtAcoterj);
                decimal Enotariado = Convert.ToDecimal(txtEnotariado);
                decimal Total = (Adicionar + emolumento + Fetj + Fundperj + Funperj + Funarpen + Pmcmv + Iss + Indisp + Distribuicao + Mutua + Acoterj + Enotariado) - Desconto;

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
                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    emolumentos = (CustasNota)listaCustas[cmbNatureza.SelectedIndex];
                    listaItens = new List<ItensCustasNota>();
                    ItensCustasNota novoIten = new ItensCustasNota();
                    novoIten.Item = emolumentos.ITEM;
                    novoIten.SubItem = emolumentos.SUB;
                    novoIten.Tabela = emolumentos.TAB;
                    novoIten.Descricao = emolumentos.TEXTO;
                    novoIten.Quantidade = "1";
                    novoIten.Valor = emolumentos.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    listaItens.Add(novoIten);


                    novoIten = new ItensCustasNota();
                    var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO" && p.ANO == ano).FirstOrDefault();
                    novoIten.Item = arqrivDesarquiv.ITEM;
                    novoIten.SubItem = arqrivDesarquiv.SUB;
                    novoIten.Tabela = arqrivDesarquiv.TAB;
                    novoIten.Descricao = arqrivDesarquiv.TEXTO;
                    novoIten.Quantidade = "1";
                    novoIten.Valor = arqrivDesarquiv.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    listaItens.Add(novoIten);


                    novoIten = new ItensCustasNota();
                    var expedicao_emissao = (CustasNota)listaCustasItens.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES" && p.ANO == ano).FirstOrDefault();
                    novoIten.Item = expedicao_emissao.ITEM;
                    novoIten.SubItem = expedicao_emissao.SUB;
                    novoIten.Tabela = expedicao_emissao.TAB;
                    novoIten.Descricao = expedicao_emissao.TEXTO;

                    if (Principal.TipoAto == "ESCRITURA")
                        novoIten.Quantidade = "4";

                    if (Principal.TipoAto == "PROCURAÇÃO")
                        novoIten.Quantidade = "2";

                    if (Principal.TipoAto == "TESTAMENTO")
                        novoIten.Quantidade = "2";

                    novoIten.Valor = Convert.ToDecimal(expedicao_emissao.VALOR);
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    listaItens.Add(novoIten);
                }
                else
                {
                    if (Principal.TipoAto == "CERTIDÃO NOTAS")
                    {
                        listaItens = new List<ItensCustasNota>();
                        ItensCustasNota novoIten = new ItensCustasNota();


                        novoIten = new ItensCustasNota();
                        var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS" && p.ANO == ano).FirstOrDefault();
                        novoIten.Item = arqrivDesarquiv.ITEM;
                        novoIten.SubItem = arqrivDesarquiv.SUB;
                        novoIten.Tabela = arqrivDesarquiv.TAB;
                        novoIten.Descricao = arqrivDesarquiv.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = arqrivDesarquiv.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);


                        novoIten = new ItensCustasNota();
                        var expedicao_emissao = (CustasNota)listaCustasItens.Where(p => p.DESCR == "BUSCAS EM LIVROS OU PAPÉIS" && p.ANO == ano).FirstOrDefault();
                        novoIten.Item = expedicao_emissao.ITEM;
                        novoIten.SubItem = expedicao_emissao.SUB;
                        novoIten.Tabela = expedicao_emissao.TAB;
                        novoIten.Descricao = expedicao_emissao.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(expedicao_emissao.VALOR);
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);

                        novoIten = new ItensCustasNota();
                        var arquivamento = (CustasNota)listaCustasItens.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO" && p.ANO == ano).FirstOrDefault();
                        novoIten.Item = arquivamento.ITEM;
                        novoIten.SubItem = arquivamento.SUB;
                        novoIten.Tabela = arquivamento.TAB;
                        novoIten.Descricao = arquivamento.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = Convert.ToDecimal(arquivamento.VALOR);
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);
                    }
                    else
                    {
                        listaItens = new List<ItensCustasNota>();

                        emolumentos = listaCustas.FirstOrDefault();
                        listaItens = new List<ItensCustasNota>();
                        ItensCustasNota novoIten = new ItensCustasNota();
                        novoIten.Item = emolumentos.ITEM;
                        novoIten.SubItem = emolumentos.SUB;
                        novoIten.Tabela = emolumentos.TAB;
                        novoIten.Descricao = emolumentos.TEXTO;
                        novoIten.Quantidade = "1";
                        novoIten.Valor = emolumentos.VALOR;
                        novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                        listaItens.Add(novoIten);

                    }
                }
            }
            CalcularValores();
        }

        private void txtVrEscrevente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtVrEscrevente.Text == "0,00")
            {
                txtVrEscrevente.Text = "";
            }

        }

        private void txtVrCorretor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtVrCorretor.Text == "0,00")
            {
                txtVrCorretor.Text = "";
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

        private void txtBaseCalculo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Principal.TipoAto != "CERTIDÃO NOTAS")
            {
                if (txtBaseCalculo.Text == "0,00")
                {
                    txtBaseCalculo.Text = "";
                }
            }
            else
            {
                if (txtBaseCalculo.Text == "0")
                {
                    txtBaseCalculo.Text = "";
                }
            }
        }

        private void txtQtdIndisp_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbNatureza.SelectedIndex > -1)
            {
                if (txtQtdIndisp.Text == "")
                {
                    txtQtdIndisp.Text = "0";
                }
                CalcularValores();
            }
        }

        private void txtQtdIndisp_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbNatureza.SelectedIndex > -1)
            {
                if (txtQtdIndisp.Text == "0")
                {
                    txtQtdIndisp.Text = "";
                }
            }
        }

        private void QtdExced_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdExced.Text == "")
            {
                txtQtdExced.Text = "0";
            }
            CalcularValores();
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

                if (Principal.TipoAto == "APOSTILAMENTO HAIA")
                {
                    if (cmbTipoCustas.SelectedIndex == 1)
                        cmbTipoCustas.SelectedIndex = 0;
                }

                if (cmbNatureza.SelectedIndex > -1)
                {
                    txtQtdExced.IsEnabled = true;
                    txtQtdIndisp.IsEnabled = true;
                    if (cmbTipoCustas.Focus())
                    {

                        CalcularValores();
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

        private void btnCustas_Click(object sender, RoutedEventArgs e)
        {
            WinCustasNotas winCustasNotas = new WinCustasNotas(this, Principal);
            winCustasNotas.Owner = this;

            winCustasNotas.ShowDialog();
        }

        private void checkBoxPago_Checked(object sender, RoutedEventArgs e)
        {

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
                if (txtValorPagoDinheiro != null && txtValorPagoDeposito != null && txtValorPagoCheque != null && txtValorPagoChequePre != null && txtValorPagoBoleto != null)
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

                ProcSalvar();
            }
            else
            {
                if (dataPagamento != datePickerDataAto.SelectedDate || dataAto != datePickerDataPagamento.SelectedDate || totalInicial != txtTotal.Text)
                    masterParaSalvar = true;
                else
                    masterParaSalvar = false;


                if (masterParaSalvar == false)
                    ProcSalvar();
                else
                {
                    if (usuarioLogado.Master == false)
                    {
                        var confirmaMaster = new WinConfirmaSenhaMaster(this);
                        confirmaMaster.Owner = this;
                        confirmaMaster.ShowDialog();
                        if (senhaConfirmada == true)
                            ProcSalvar();
                        else
                            return;
                    }
                    else
                        ProcSalvar();
                }
            }
        }




        private void SalvarItensCustasControleAtos(int idAto)
        {

            var classCustasNotas = new ClassCustasNotas();

            for (int cont = 0; cont <= listaItens.Count - 1; cont++)
            {
                var item = new ItensCustasControleAtosNota();

                item.Id_ControleAto = idAto;

                item.Tabela = listaItens[cont].Tabela;

                item.Item = listaItens[cont].Item;

                item.SubItem = listaItens[cont].SubItem;

                item.Quantidade = listaItens[cont].Quantidade;

                item.Valor = listaItens[cont].Valor;

                item.Total = listaItens[cont].Total;

                item.Descricao = listaItens[cont].Descricao;

                classCustasNotas.SalvarItensListaControleAtos(item);

            }


        }




        private void ProcSalvar()
        {



            if (Principal.TipoAto == "APOSTILAMENTO HAIA")
            {
                if (txtFlsApostilamento.Text.Trim() == "")
                {
                    MessageBox.Show("Número de série da folha é obrigatório.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtFlsApostilamento.Focus();
                    return;
                }
            }

            Ato atoCorrente = new Ato();

            if (status == "alterar")
            {
                atoCorrente.Id_Ato = atoSelecionado.Id_Ato;
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

                if (datePickerDataAto.SelectedDate.Value > datePickerDataPagamento.SelectedDate.Value)
                {
                    MessageBox.Show("Data do Ato não pode ser maior que a data do Pagamento.", "Data do Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                    datePickerDataAto.Focus();
                    return;
                }

                // pago
                if (checkBoxPago.IsChecked == true)
                    atoCorrente.Pago = true;
                else
                    atoCorrente.Pago = false;




                // IdUsuario
                atoCorrente.IdUsuario = listaNomes.Where(p => p.NomeUsu == cmbNomes.Text).FirstOrDefault().Id_Usuario;

                // Usuario
                atoCorrente.Usuario = cmbNomes.Text;

                // Atribuiçao
                atoCorrente.Atribuicao = "NOTAS";


                // Letra Selo
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtLetraSelo.Text.Length == 4)
                    {
                        atoCorrente.LetraSelo = txtLetraSelo.Text;
                    }
                    if (txtLetraSelo.Text.Length < 4 && txtLetraSelo.Text.Length > 0)
                    {
                        MessageBox.Show("Campo Letra do selo inválido, favor verifique.", "Letra Selo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtLetraSelo.Focus();
                        return;
                    }
                }

                // Numero Selo
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtNumeroSelo.Text.Length == 5 && txtLetraSelo.Text.Length == 4)
                    {
                        atoCorrente.NumeroSelo = Convert.ToInt32(txtNumeroSelo.Text);
                    }
                    if ((txtNumeroSelo.Text.Length > 0 && txtNumeroSelo.Text.Length < 5) && (txtLetraSelo.Text.Length == 4))
                    {
                        MessageBox.Show("Campo Número do selo inválido, favor verifique.", "Número Selo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtNumeroSelo.Focus();
                        return;
                    }
                }

                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtAleatorio.Text != "" && txtAleatorio.Text.Length == 3)
                    {
                        atoCorrente.Aleatorio = txtAleatorio.Text;
                    }
                    if ((txtAleatorio.Text.Length > 0 && txtAleatorio.Text.Length < 3) && (txtNumeroSelo.Text.Length == 5 && txtLetraSelo.Text.Length == 4))
                    {
                        MessageBox.Show("Campo Aleatório inválido, favor verifique.", "Aleatório", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtAleatorio.Focus();
                        return;
                    }
                }

                // valor escrevente
                atoCorrente.ValorEscrevente = Convert.ToDecimal(txtVrEscrevente.Text);


                // valor adicionar
                atoCorrente.ValorAdicionar = Convert.ToDecimal(txtAdicionar.Text);


                // Valor Desconto
                atoCorrente.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);


                if (Principal.TipoAto == "APOSTILAMENTO HAIA")
                {
                    atoCorrente.DescricaoAto = txtFlsApostilamento.Text;
                }


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

                // ValorCorretor
                atoCorrente.ValorCorretor = Convert.ToDecimal(txtVrCorretor.Text);



                if (Principal.TipoAto == "ESCRITURA")
                {
                    // Faixa
                    if (cmbNatureza.SelectedIndex >= 0)
                        atoCorrente.Faixa = cmbNatureza.Text;

                    // base calculo
                    if (txtBaseCalculo.Text != "")
                        atoCorrente.ValorTitulo = Convert.ToDecimal(txtBaseCalculo.Text);
                }


                atoCorrente.QtdAtos = Convert.ToInt32(txtQtdAtos.Text);


                // Livro
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtLivro.Text != "")
                    {
                        atoCorrente.Livro = txtLivro.Text;
                    }
                    else
                    {
                        MessageBox.Show("Informe o livro.", "Livro", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtLivro.Focus();
                        return;
                    }
                }

                // FolhaInical
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtFlsInicial.Text != "")
                    {
                        atoCorrente.FolhaInical = Convert.ToInt32(txtFlsInicial.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe a Folha Inicial.", "Folha Inicial", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtFlsInicial.Focus();
                        return;
                    }
                }

                // FolhaFinal
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtFlsFinal.Text != "")
                    {
                        atoCorrente.FolhaFinal = Convert.ToInt32(txtFlsFinal.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe a Folha Final.", "Folha Final", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtFlsFinal.Focus();
                        return;
                    }
                }

                // Numero Ato
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    if (txtAto.Text != "")
                    {
                        atoCorrente.NumeroAto = Convert.ToInt32(txtAto.Text);
                    }
                    else
                    {
                        MessageBox.Show("Informe o Número do Ato.", "Número do Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtAto.Focus();
                        return;
                    }
                }

                if (Principal.TipoAto == "CERTIDÃO NOTAS")
                {
                    if (txtBaseCalculo.Text == "0")
                    {
                        MessageBox.Show("Informe o Número do Recibo.", "Recibo", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtBaseCalculo.Focus();
                        return;
                    }
                    else
                    {
                        atoCorrente.Recibo = Convert.ToInt32(txtBaseCalculo.Text);
                    }
                }
                else
                {
                    atoCorrente.Recibo = 0;
                }


                //idReciboBalcao
                atoCorrente.IdReciboBalcao = 0;

                // Recibo Balcao
                atoCorrente.ReciboBalcao = 0;


                // TipoAto
                atoCorrente.TipoAto = Principal.TipoAto;

                if (Principal.TipoAto == "ESCRITURA")
                {
                    // Natureza
                    if (txtNatureza.Text != "")
                    {
                        atoCorrente.Natureza = txtNatureza.Text.Trim();
                    }
                    else
                    {
                        MessageBox.Show("Informe a Natureza.", "Natureza", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNatureza.Focus();
                        return;
                    }
                }
                else
                {
                    // Natureza
                    if (cmbNatureza.SelectedIndex >= 0)
                        atoCorrente.Natureza = cmbNatureza.Text;
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


                // ISS
                atoCorrente.Iss = Convert.ToDecimal(txtIss.Text);


                // Mutua
                atoCorrente.Mutua = Convert.ToDecimal(txtMutua.Text);


                // Acoterj
                atoCorrente.Acoterj = Convert.ToDecimal(txtAcoterj.Text);


                // Distribuicao
                atoCorrente.Distribuicao = Convert.ToDecimal(txtDistribuicao.Text);


                // Indisponibilidade
                atoCorrente.Indisponibilidade = Convert.ToDecimal(txtIndisp.Text);


                // QuantIndisp
                atoCorrente.QuantIndisp = Convert.ToInt32(txtQtdIndisp.Text);


                // QuantDistrib
                atoCorrente.QuantDistrib = Convert.ToInt32(txtQtdExced.Text);


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
                atoCorrente.ValorTroco = Convert.ToDecimal(lblTroco.Content);

                // Total
                atoCorrente.Total = Convert.ToDecimal(txtTotal.Text);

                ClassAto classAto = new ClassAto();

                int idAtoPrincipal = classAto.SalvarAto(atoCorrente, status);

                SalvarItensCustas(idAtoPrincipal);

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

                    valorPago.IdAto = idAtoPrincipal;
                    valorPago.IdReciboBalcao = 0;

                    if (cmbTipoPagamento.SelectedIndex == 2)
                        valorPago.Mensalista = atoCorrente.Total;
                    else
                        valorPago.Mensalista = 0M;


                    classAto.SalvarValorPago(valorPago, status, "IdAto");

                    Enotariado enotariado = new Enotariado();
                    enotariado.Data = atoCorrente.DataPagamento;
                    enotariado.IdAto = idAtoPrincipal;
                    enotariado.Valor = Convert.ToDecimal(txtEnotariado.Text);

                    enot.SalvaEnotariado(enotariado, "novo");

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
                            Troco = Convert.ToDecimal(lblTroco.Content),
                            DataModificado = DateTime.Now.ToShortDateString(),
                            HoraModificado = DateTime.Now.ToLongTimeString(),
                            IdUsuario = usuarioLogado.Id_Usuario,
                            NomeUsuario = usuarioLogado.NomeUsu           

                        };

                        valorPago.IdAto = idAtoPrincipal;
                        valorPago.IdReciboBalcao = 0;

                        if (cmbTipoPagamento.SelectedIndex == 2)
                            valorPago.Mensalista = atoCorrente.Total;
                        else
                            valorPago.Mensalista = 0M;


                        classAto.SalvarValorPago(valorPago, status, "IdAto");

                        if (enotariadoAlterar != null)
                        {
                            enotariadoAlterar.Data = atoCorrente.DataAto;
                            enotariadoAlterar.Valor = Convert.ToDecimal(txtEnotariado.Text);

                            enot.SalvaEnotariado(enotariadoAlterar, "alterar");
                        }
                        else
                        {
                            Enotariado enotariado = new Enotariado();
                            enotariado.Data = atoCorrente.DataPagamento;
                            enotariado.IdAto = idAtoPrincipal;
                            enotariado.Valor = Convert.ToDecimal(txtEnotariado.Text);

                            enot.SalvaEnotariado(enotariado, "novo");
                        }

                    }
                }

                _digitarNotas.datePickerdataConsulta.SelectedDate = dataInicioConsulta;
                _digitarNotas.datePickerdataConsultaFim.SelectedDate = dataFimConsulta;
                _digitarNotas.ConsultaData();

                _digitarNotas.idAtoNovo = idAtoPrincipal;

                _digitarNotas.atoSelecionado = atoCorrente;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void SalvarItensCustas(int idAto)
        {
            ClassCustasNotas classCustasNotas = new ClassCustasNotas();

            if (status == "alterar")
            {
                classCustasNotas.RemoverItensCustas(idAto);
            }


            for (int cont = 0; cont <= listaItens.Count - 1; cont++)
            {
                ItensCustasNota item = new ItensCustasNota();

                item.Id_Ato = idAto;

                item.Tabela = listaItens[cont].Tabela;

                item.Item = listaItens[cont].Item;

                item.SubItem = listaItens[cont].SubItem;

                item.Quantidade = listaItens[cont].Quantidade;

                item.Valor = listaItens[cont].Valor;

                item.Total = listaItens[cont].Total;

                item.Descricao = listaItens[cont].Descricao;

                classCustasNotas.SalvarItensLista(item);

            }

        }

        private void txtLetraSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
        }

        private void txtLetraSelo_LostFocus(object sender, RoutedEventArgs e)
        {

        }



        private void txtNumeroSelo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumeroSelo.Text != "")
                txtNumeroSelo.Text = string.Format("{0:00000}", Convert.ToInt32(txtNumeroSelo.Text));
        }

        private void txtLetraSelo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLetraSelo.Text.Length == 4)
            {
                txtNumeroSelo.IsEnabled = true;
                txtNumeroSelo.Focus();
            }
            else
            {
                txtNumeroSelo.IsEnabled = false;
                txtNumeroSelo.Text = "";
            }
        }

        private void txtQtdAtos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdAtos_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbNatureza.SelectedIndex > -1)
            {
                if (txtQtdAtos.Text == "0")
                {
                    txtQtdAtos.Text = "";
                }
            }
        }

        private void txtQtdAtos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbNatureza.SelectedIndex > -1)
            {
                if (txtQtdAtos.Text == "")
                {
                    txtQtdAtos.Text = "0";
                }
                CalcularValores();
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

            txtValorPagoDinheiro.Text = "0,00";

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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
                    }
                    break;

                case 7:
                    if (Convert.ToDecimal(lblTotal.Content) != (Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text) + Convert.ToDecimal(txtValorPagoCartaoCredito.Text)))
                    {
                        mensagem = "Os valores pagos somados devem corresponder exatamente ao total. O troco deve estar igual a '0,00', favor verifique.";
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
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
                        tabControl1.SelectedIndex = 2;
                    }
                    break;

                case 7:
                    if (Convert.ToDecimal(lblTotal.Content) > (Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text) + Convert.ToDecimal(txtValorPagoCartaoCredito.Text)))
                    {
                        mensagem = "Os valores pagos somados devem ser igual ou maior que o total. O troco deve estar igual a '0,00' ou valor positivo, favor verifique.";
                        tabControl1.SelectedIndex = 2;
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

        private void txtEnotariado_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;


            if (txtEnotariado.Text.Length > 0)
            {
                if (txtEnotariado.Text.Contains(","))
                {
                    int index = txtEnotariado.Text.IndexOf(",");

                    if (txtEnotariado.Text.Length == index + 3)
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

        private void txtEnotariado_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEnotariado.Text != "")
            {
                try
                {
                    txtEnotariado.Text = string.Format("{0:n2}", Convert.ToDecimal(txtEnotariado.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtEnotariado.Text = "0,00";
            }

            if (cmbNatureza.SelectedIndex > -1)
            {
                txtQtdExced.IsEnabled = true;
                txtQtdIndisp.IsEnabled = true;


                CalcularValores();

            }
        }

        private void txtEnotariado_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEnotariado.Text == "0,00")
            {
                txtEnotariado.Text = "";
            }
        }

        private void ckboxEnotariado_Checked(object sender, RoutedEventArgs e)
        {
            txtEnotariado.IsEnabled = true;
            txtEnotariado.Text = "";
            txtEnotariado.Focus();
        }

        private void ckboxEnotariado_Unchecked(object sender, RoutedEventArgs e)
        {
            txtEnotariado.IsEnabled = false;
            txtEnotariado.Text = "0,00";
            CalcularValores();
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

        private void gridDados_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassaUmControleParaOutro(sender, e);
        }


    }
}
