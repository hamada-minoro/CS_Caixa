using CS_Caixa.Controls;
using CS_Caixa.Models;
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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinDigitarControleAtosNotas.xaml
    /// </summary>
    public partial class WinDigitarControleAtosNotas : Window
    {

        WinPrincipal Principal;
        Usuario usuarioLogado;
        public List<CustasNota> listaCustas = new List<CustasNota>();
        public List<CustasNota> listaCustasAtosCorrente = new List<CustasNota>();
        public List<ItensCustasControleAtosNota> listaItens = new List<ItensCustasControleAtosNota>();
        public List<CustasNota> listaCustasItens = new List<CustasNota>();
        public CustasNota emolumentos;
        public decimal emolLista;
        public int ano = DateTime.Now.Year;
        public decimal mutua = 0;
        public decimal acoterj = 0;
        public decimal indisponibilidade = 0;
        string status;
        ControleAto atoSelecionado = new ControleAto();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public decimal porcentagemIss;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        public WinDigitarControleAtosNotas(WinPrincipal Principal, Usuario usuarioLogado, string status)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            InitializeComponent();
        }

        public WinDigitarControleAtosNotas(WinPrincipal Principal, Usuario usuarioLogado, string status, ControleAto atoSelecionado, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.Principal = Principal;
            this.usuarioLogado = usuarioLogado;
            this.status = status;
            this.atoSelecionado = atoSelecionado;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                datePickerDataAto.SelectedDate = DateTime.Now.Date;


                ClassUsuario classUsuario = new ClassUsuario();
                ClassCustasNotas classCustasNotas = new ClassCustasNotas();
                listaCustas = classCustasNotas.ListaCustas();
                listaCustasItens = classCustasNotas.ListaCustas();



                List<CustasNota> atulizado = listaCustas.Where(p => p.ANO == ano).ToList();
                if (status == "novo")
                {
                    if (atulizado.Count > 0)
                    {
                        if (DateTime.Now.Month == 1 && DateTime.Now.Day < 7)
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

                    if (DateTime.Now.Year != atoSelecionado.DataAto.Year)
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


                cmbTiposAtos.Text = "ESCRITURA";

                CarregaComboTipoAto();

                if (status == "alterar")
                {

                    listaItens = classCustasNotas.ListarItensControleAtosCustas(atoSelecionado.Id_ControleAtos);
                    emolumentos = new CustasNota();
                    emolumentos.ITEM = listaItens[0].Item;
                    emolumentos.SUB = listaItens[0].SubItem;
                    emolumentos.DESCR = listaItens[0].Descricao;
                    emolumentos.TEXTO = listaItens[0].Descricao;
                    emolumentos.VALOR = listaItens[0].Total;
                    CarregaCamposAlterar();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CarregaCamposAlterar()
        {
            datePickerDataAto.SelectedDate = atoSelecionado.DataAto;
            txtLivro.Text = atoSelecionado.Livro;
            txtFlsInicial.Text = string.Format("{0:000}", atoSelecionado.FolhaInical);
            txtFlsFinal.Text = string.Format("{0:000}", atoSelecionado.FolhaFinal);
            txtAto.Text = string.Format("{0:000}", atoSelecionado.NumeroAto);
            txtLetraSelo.Text = atoSelecionado.LetraSelo;
            txtNumeroSelo.Text = string.Format("{0:00000}", atoSelecionado.NumeroSelo);
            txtBaseCalculo.Text = string.Format("{0:n2}", atoSelecionado.Faixa);

            if (txtBaseCalculo.Text == "0,000")
                txtBaseCalculo.Text = "0,00";

            cmbTiposAtos.Text = atoSelecionado.TipoAto;
            cmbNatureza.Text = atoSelecionado.Natureza;
            txtQtdAtos.Text = atoSelecionado.QtdAtos.ToString();
            ckbGratuito.IsEnabled = true;
            if (atoSelecionado.AtoGratuito == 1)
            {
                ckbGratuito.IsChecked = true;
            }
            if (atoSelecionado.AtoGratuito == 0)
            {
                ckbGratuito.IsChecked = false;
            }
            txtQtdAtos.Text = string.Format("{0}", atoSelecionado.QtdAtos);

            //CalcularValores();
        }


        private void CarregaComboTipoAto()
        {
            try
            {


                if (cmbTiposAtos.SelectedIndex == 0)
                {
                    listaCustasAtosCorrente = listaCustas.Where(p => p.ANO == ano && p.TIPO == "E").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                    txtBaseCalculo.IsEnabled = true;
                    txtQtdAtos.IsEnabled = true;
                    txtQtdAtos.Text = "1";
                }

                if (cmbTiposAtos.SelectedIndex == 1)
                {
                    listaCustasAtosCorrente = listaCustas.Where(p => p.ANO == ano && p.TIPO == "P").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                    txtBaseCalculo.Text = "0,00";
                    txtBaseCalculo.IsEnabled = false;
                    txtQtdAtos.IsEnabled = true;
                    txtQtdAtos.Text = "1";
                }

                if (cmbTiposAtos.SelectedIndex == 2)
                {
                    listaCustasAtosCorrente = listaCustas.Where(p => p.ANO == ano && p.TIPO == "T").OrderBy(p => p.ORDEM).Select(p => p).ToList();
                    txtBaseCalculo.Text = "0,00";
                    txtBaseCalculo.IsEnabled = false;
                    txtQtdAtos.IsEnabled = true;
                    txtQtdAtos.Text = "1";
                }

                if (cmbTiposAtos.SelectedIndex >= 3)
                {
                    txtBaseCalculo.Text = "0,00";
                    txtBaseCalculo.IsEnabled = false;
                    txtQtdAtos.Text = "0";
                }

                listaCustasItens = listaCustasItens.Where(p => p.ANO == ano && p.VAI == "S").OrderBy(p => p.ORDEM).Select(p => p).ToList();

                if (cmbTiposAtos.SelectedIndex < 3)
                {
                    cmbNatureza.ItemsSource = listaCustasAtosCorrente.Select(p => p.DESCR).ToList();
                }
                else
                {
                    if (cmbTiposAtos.SelectedIndex == 3)
                    {
                        var itensCertidao = new List<string>();
                        itensCertidao.Add("CERTIDÃO DE ESCRITURA");
                        itensCertidao.Add("CERTIDÃO DE PROCURAÇÃO");
                        itensCertidao.Add("CERTIDÃO DE TESTAMENTO");

                        cmbNatureza.ItemsSource = itensCertidao;
                    }
                    if (cmbTiposAtos.SelectedIndex == 4)
                    {
                        var itensBalcao = new List<string>();
                        itensBalcao.Add("ABERTURA DE FIRMAS");
                        itensBalcao.Add("REC AUTENTICIDADE");
                        itensBalcao.Add("REC SEMELHANÇA");
                        itensBalcao.Add("AUTENTICAÇÃO");
                        cmbNatureza.ItemsSource = itensBalcao;
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
                WinControleAtosNotas escritura = new WinControleAtosNotas(usuarioLogado, Principal, atoSelecionado, dataInicioConsulta, dataFimConsulta);
                escritura.Owner = Principal;
                escritura.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }


        private void txtFlsInicial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            ProximoComponente(sender, e);
        }

        private void txtFlsFinal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            ProximoComponente(sender, e);
        }

        private void txtAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            ProximoComponente(sender, e);
        }

        private void txtLetraSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);

        }

        private void txtNumeroSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            ProximoComponente(sender, e);
        }

        private void txtNumeroSelo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumeroSelo.Text != "")
                txtNumeroSelo.Text = string.Format("{0:00000}", Convert.ToInt32(txtNumeroSelo.Text));
        }

        private void txtBaseCalculo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;


            if (cmbTiposAtos.SelectedIndex == 0)
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
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }

            ProximoComponente(sender, e);
        }

        private void txtBaseCalculo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbTiposAtos.SelectedIndex == 0)
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

        private void txtQtdAtos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            ProximoComponente(sender, e);
        }

        private void txtQtdAtos_GotFocus(object sender, RoutedEventArgs e)
        {

            if (txtQtdAtos.Text == "0" || txtQtdAtos.Text == "1")
            {
                txtQtdAtos.Text = "";
            }

        }

        private void txtQtdAtos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdAtos.Text == "")
            {
                txtQtdAtos.Text = "0";
            }
            if (cmbNatureza.SelectedIndex > -1)
            {
                CalcularValores();
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ProcSalvar();
        }

        private void ProcSalvar()
        {
            var atoCorrente = new ControleAto();

            if (status == "alterar")
            {
                atoCorrente.Id_ControleAtos = atoSelecionado.Id_ControleAtos;
            }

            try
            {

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

                // gratuito
                if (ckbGratuito.IsChecked == true)
                {
                    atoCorrente.AtoGratuito = 1;
                    atoCorrente.AtoNaoGratuito = 0;
                }
                else
                {
                    atoCorrente.AtoGratuito = 0;
                    atoCorrente.AtoNaoGratuito = 1;
                }

                if (status == "novo")
                {
                    // IdUsuario
                    atoCorrente.IdUsuario = usuarioLogado.Id_Usuario;



                    // Usuario
                    atoCorrente.Usuario = usuarioLogado.NomeUsu;
                }

                // Atribuiçao
                atoCorrente.Atribuicao = "NOTAS";


                // Letra Selo
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


                // Numero Selo
                if (txtNumeroSelo.Text != "" && txtLetraSelo.Text.Length == 4)
                {
                    atoCorrente.NumeroSelo = Convert.ToInt32(txtNumeroSelo.Text);
                }
                if (txtNumeroSelo.Text == "" && txtLetraSelo.Text.Length == 4)
                {
                    MessageBox.Show("Campo Número do selo inválido, favor verifique.", "Número Selo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtNumeroSelo.Focus();
                    return;
                }



                // Faixa

                atoCorrente.Faixa = txtBaseCalculo.Text;


                atoCorrente.QtdAtos = Convert.ToInt32(txtQtdAtos.Text);


                // Livro
                if (txtLivro.Text != "" || cmbTiposAtos.SelectedIndex == 4)
                {
                    atoCorrente.Livro = txtLivro.Text;
                }
                else
                {
                    MessageBox.Show("Informe o livro.", "Livro", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtLivro.Focus();
                    return;
                }


                // FolhaInical
                if (txtFlsInicial.Text != "" || cmbTiposAtos.SelectedIndex == 4)
                {

                    if (txtFlsInicial.Text == "")
                        txtFlsInicial.Text = "0";
                    atoCorrente.FolhaInical = Convert.ToInt32(txtFlsInicial.Text);
                }
                else
                {
                    MessageBox.Show("Informe a Folha Inicial.", "Folha Inicial", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtFlsInicial.Focus();
                    return;
                }

                // FolhaFinal
                if (txtFlsFinal.Text != "" || cmbTiposAtos.SelectedIndex == 4)
                {
                    if (txtFlsFinal.Text == "")
                        txtFlsFinal.Text = "0";
                    atoCorrente.FolhaFinal = Convert.ToInt32(txtFlsFinal.Text);
                }
                else
                {
                    MessageBox.Show("Informe a Folha Final.", "Folha Final", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtFlsFinal.Focus();
                    return;
                }

                // Numero Ato
                if (txtAto.Text != "" || cmbTiposAtos.SelectedIndex == 4)
                {
                    if (txtAto.Text == "")
                        txtAto.Text = "0";
                    atoCorrente.NumeroAto = Convert.ToInt32(txtAto.Text);
                }
                else
                {
                    MessageBox.Show("Informe o Número do Ato.", "Número do Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtAto.Focus();
                    return;
                }




                // TipoAto
                atoCorrente.TipoAto = cmbTiposAtos.Text;



                // Natureza
                if (cmbNatureza.SelectedIndex >= 0)
                    atoCorrente.Natureza = cmbNatureza.Text;


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

                var classAto = new ClassControleAto();

                int idAto = classAto.SalvarAto(atoCorrente, status);

                SalvarItensCustas(idAto);
                atoSelecionado = atoCorrente;
                dataInicioConsulta = atoCorrente.DataAto;
                dataFimConsulta = atoCorrente.DataAto;
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void SalvarItensCustas(int idAto)
        {
            var classCustasNotas = new ClassCustasNotas();

            if (status == "alterar")
            {
                classCustasNotas.RemoverItensCustasControleAtos(idAto);
            }


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


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCustas_Click(object sender, RoutedEventArgs e)
        {
            WinCustasNotas winCustasNotas = new WinCustasNotas(this, Principal);
            winCustasNotas.Owner = this;

            winCustasNotas.ShowDialog();
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

        private void txtLetraSelo_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbTiposAtos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarregaComboTipoAto();
        }


        private void cmbNatureza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
                
            CalcularItensCustas();
             


            if (cmbNatureza.SelectedIndex > -1)
            {
                btnSalvar.IsEnabled = true;

                btnCustas.IsEnabled = true;

                ckbGratuito.IsEnabled = true;

            }
            else
            {
                btnSalvar.IsEnabled = false;

                btnCustas.IsEnabled = false;

                ckbGratuito.IsEnabled = false;
            }
        }

        private void CalcularItensCustas()
        {
            try
            {
                if (status == "novo")
                {
                    if (cmbTiposAtos.SelectedIndex < 3)
                    {
                        if (cmbNatureza.SelectedIndex > -1)
                        {
                            emolumentos = (CustasNota)listaCustasAtosCorrente[cmbNatureza.SelectedIndex];
                            listaItens = new List<ItensCustasControleAtosNota>();
                            ItensCustasControleAtosNota novoIten = new ItensCustasControleAtosNota();
                            novoIten.Item = emolumentos.ITEM;
                            novoIten.SubItem = emolumentos.SUB;
                            novoIten.Tabela = emolumentos.TAB;
                            novoIten.Descricao = emolumentos.TEXTO;
                            novoIten.Quantidade = "1";
                            novoIten.Valor = emolumentos.VALOR;
                            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                            listaItens.Add(novoIten);


                            novoIten = new ItensCustasControleAtosNota();
                            var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO" && p.ANO == ano).FirstOrDefault();
                            novoIten.Item = arqrivDesarquiv.ITEM;
                            novoIten.SubItem = arqrivDesarquiv.SUB;
                            novoIten.Tabela = arqrivDesarquiv.TAB;
                            novoIten.Descricao = arqrivDesarquiv.TEXTO;
                            novoIten.Quantidade = "1";
                            novoIten.Valor = arqrivDesarquiv.VALOR;
                            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                            listaItens.Add(novoIten);


                            novoIten = new ItensCustasControleAtosNota();
                            var expedicao_emissao = (CustasNota)listaCustasItens.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES" && p.ANO == ano).FirstOrDefault();
                            novoIten.Item = expedicao_emissao.ITEM;
                            novoIten.SubItem = expedicao_emissao.SUB;
                            novoIten.Tabela = expedicao_emissao.TAB;
                            novoIten.Descricao = expedicao_emissao.TEXTO;

                            if (cmbTiposAtos.SelectedIndex == 0)
                                novoIten.Quantidade = "4";

                            if (cmbTiposAtos.SelectedIndex == 1)
                                novoIten.Quantidade = "2";

                            if (cmbTiposAtos.SelectedIndex == 2)
                                novoIten.Quantidade = "2";

                            novoIten.Valor = Convert.ToDecimal(expedicao_emissao.VALOR);
                            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                            listaItens.Add(novoIten);

                            CalcularValores();

                            ckbGratuito.IsEnabled = true;
                        }

                    }
                    else
                    {
                        if (cmbTiposAtos.SelectedIndex == 3)
                        {
                            if (cmbNatureza.SelectedIndex > -1)
                            {


                                ckbGratuito.IsEnabled = true;
                                listaItens = new List<ItensCustasControleAtosNota>();
                                ItensCustasControleAtosNota novoIten;


                                novoIten = new ItensCustasControleAtosNota();
                                var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS" && p.ANO == ano).FirstOrDefault();
                                novoIten.Item = arqrivDesarquiv.ITEM;
                                novoIten.SubItem = arqrivDesarquiv.SUB;
                                novoIten.Tabela = arqrivDesarquiv.TAB;
                                novoIten.Descricao = arqrivDesarquiv.TEXTO;
                                novoIten.Quantidade = "1";
                                novoIten.Valor = arqrivDesarquiv.VALOR;
                                novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                                listaItens.Add(novoIten);


                                novoIten = new ItensCustasControleAtosNota();
                                var expedicao_emissao = (CustasNota)listaCustasItens.Where(p => p.DESCR == "BUSCAS EM LIVROS OU PAPÉIS" && p.ANO == ano).FirstOrDefault();
                                novoIten.Item = expedicao_emissao.ITEM;
                                novoIten.SubItem = expedicao_emissao.SUB;
                                novoIten.Tabela = expedicao_emissao.TAB;
                                novoIten.Descricao = expedicao_emissao.TEXTO;
                                novoIten.Quantidade = "1";
                                novoIten.Valor = Convert.ToDecimal(expedicao_emissao.VALOR);
                                novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                                listaItens.Add(novoIten);

                                novoIten = new ItensCustasControleAtosNota();
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


                        if (cmbTiposAtos.SelectedIndex == 4)
                        {
                            if (cmbNatureza.SelectedIndex == 0)
                            {
                                ckbGratuito.IsEnabled = true;

                                listaItens = new List<ItensCustasControleAtosNota>();
                                ItensCustasControleAtosNota novoIten;


                                novoIten = new ItensCustasControleAtosNota();
                                var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "ABERTURA DE FIRMA" && p.ANO == ano).FirstOrDefault();
                                novoIten.Item = arqrivDesarquiv.ITEM;
                                novoIten.SubItem = arqrivDesarquiv.SUB;
                                novoIten.Tabela = arqrivDesarquiv.TAB;
                                novoIten.Descricao = arqrivDesarquiv.TEXTO;
                                novoIten.Quantidade = "1";
                                novoIten.Valor = arqrivDesarquiv.VALOR;
                                novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                                listaItens.Add(novoIten);
                                emolumentos = arqrivDesarquiv;

                                novoIten = new ItensCustasControleAtosNota();
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

                            if (cmbNatureza.SelectedIndex == 1)
                            {
                                ckbGratuito.IsEnabled = true;
                                listaItens = new List<ItensCustasControleAtosNota>();
                                ItensCustasControleAtosNota novoIten;


                                novoIten = new ItensCustasControleAtosNota();
                                var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE" && p.ANO == ano).FirstOrDefault();
                                novoIten.Item = arqrivDesarquiv.ITEM;
                                novoIten.SubItem = arqrivDesarquiv.SUB;
                                novoIten.Tabela = arqrivDesarquiv.TAB;
                                novoIten.Descricao = arqrivDesarquiv.TEXTO;
                                novoIten.Quantidade = "1";
                                novoIten.Valor = arqrivDesarquiv.VALOR;
                                novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                                listaItens.Add(novoIten);
                                emolumentos = arqrivDesarquiv;
                            }

                            if (cmbNatureza.SelectedIndex == 2)
                            {
                                ckbGratuito.IsEnabled = true;
                                listaItens = new List<ItensCustasControleAtosNota>();
                                ItensCustasControleAtosNota novoIten;


                                novoIten = new ItensCustasControleAtosNota();
                                var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR SEMELHANÇA OU CHANCELA" && p.ANO == ano).FirstOrDefault();
                                novoIten.Item = arqrivDesarquiv.ITEM;
                                novoIten.SubItem = arqrivDesarquiv.SUB;
                                novoIten.Tabela = arqrivDesarquiv.TAB;
                                novoIten.Descricao = arqrivDesarquiv.TEXTO;
                                novoIten.Quantidade = "1";
                                novoIten.Valor = arqrivDesarquiv.VALOR;
                                novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                                listaItens.Add(novoIten);
                                emolumentos = arqrivDesarquiv;
                            }

                            if (cmbNatureza.SelectedIndex == 3)
                            {
                                ckbGratuito.IsEnabled = true;
                                listaItens = new List<ItensCustasControleAtosNota>();
                                ItensCustasControleAtosNota novoIten;


                                novoIten = new ItensCustasControleAtosNota();
                                var arqrivDesarquiv = (CustasNota)listaCustasItens.Where(p => p.DESCR == "AUTENTICAÇÃO POR DOCUMENTO OU PÁGINA" && p.ANO == ano).FirstOrDefault();
                                novoIten.Item = arqrivDesarquiv.ITEM;
                                novoIten.SubItem = arqrivDesarquiv.SUB;
                                novoIten.Tabela = arqrivDesarquiv.TAB;
                                novoIten.Descricao = arqrivDesarquiv.TEXTO;
                                novoIten.Quantidade = "1";
                                novoIten.Valor = arqrivDesarquiv.VALOR;
                                novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                                listaItens.Add(novoIten);
                                emolumentos = arqrivDesarquiv;
                            }



                        }

                    }
                              

                }

                CalcularValores();        

                if (cmbNatureza.SelectedIndex == -1)
                {
                    ckbGratuito.IsEnabled = false;

                    txtEmol.Text = "0,00";
                    txtFetj.Text = "0,00";
                    txtFundperj.Text = "0,00";
                    txtFunperj.Text = "0,00";
                    txtFunarpen.Text = "0,00";
                    txtPmcmv.Text = "0,00";
                    txtIss.Text = "0,00";

                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                fetj_20 = emol * 20 / 100;
                fundperj_5 = emol * 5 / 100;
                funperj_5 = emol * 5 / 100;
                funarpen_4 = emol * 4 / 100;

                if (cmbTiposAtos.SelectedIndex != 4)
                {
                    iss = (100 - porcentagemIss) / 100;
                    iss = emol / iss - emol;
                    iss = emol * porcentagemIss / 100;
                }

                if (cmbTiposAtos.SelectedIndex != 3)
                pmcmv_2 = Convert.ToDecimal(emolumentos.VALOR * 2) / 100;


                if (ckbGratuito.IsChecked == false)
                {
                    Semol = Convert.ToString(emol);
                }
                else
                {
                    Semol = "0,00";
                }


                Sfetj_20 = Convert.ToString(fetj_20);
                Sfundperj_5 = Convert.ToString(fundperj_5);
                Sfunperj_5 = Convert.ToString(funperj_5);
                Sfunarpen_4 = Convert.ToString(funarpen_4);
                Siss = Convert.ToString(iss);
                Spmcmv_2 = Convert.ToString(pmcmv_2);






                if (ckbGratuito.IsChecked == true)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;
                    if (cmbTiposAtos.SelectedIndex != 3)
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

                if (cmbTiposAtos.SelectedIndex != 4)
                {
                    index = Siss.IndexOf(',');
                    Siss = Siss.Substring(0, index + 3);
                }
                else
                {
                    Siss = "0,00";
                }

                if (cmbTiposAtos.SelectedIndex != 3)
                {
                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);
                }
                else
                {
                    Spmcmv_2 = "0,00";
                }



                txtEmol.Text = Semol;
                txtFetj.Text = Sfetj_20;
                txtFundperj.Text = Sfundperj_5;
                txtFunperj.Text = Sfunperj_5;
                txtFunarpen.Text = Sfunarpen_4;
                txtPmcmv.Text = Spmcmv_2;
                txtIss.Text = Siss;

                if (ckbGratuito.IsChecked == false)
                {
                    txtMutua.Text = string.Format("{0:N2}", (Convert.ToInt16(txtQtdAtos.Text) * mutua));
                    txtAcoterj.Text = string.Format("{0:N2}", (Convert.ToInt16(txtQtdAtos.Text) * acoterj));
                }
                else
                {
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ckbGratuito_Checked(object sender, RoutedEventArgs e)
        {
            CalcularValores();
        }

        private void ckbGratuito_Unchecked(object sender, RoutedEventArgs e)
        {
            CalcularValores();
        }

        private void datePickerDataAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ProximoComponente(sender, e);
        }

        private void ProximoComponente(object sender, KeyEventArgs e)
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

        private void txtLivro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ProximoComponente(sender, e);
        }

        private void cmbTiposAtos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ProximoComponente(sender, e);
        }

        private void cmbNatureza_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ProximoComponente(sender, e);
        }
    }
}
