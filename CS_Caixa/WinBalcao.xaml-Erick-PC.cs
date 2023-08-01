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
    /// Interaction logic for WinBalcao.xaml
    /// </summary>
    public partial class WinBalcao : Window
    {
        private WinPrincipal winPrincipal;

        public WinBalcao()
        {
            InitializeComponent();
        }

        public WinBalcao(WinPrincipal winPrincipal)
        {
            // TODO: Complete member initialization
            this.winPrincipal = winPrincipal;
            InitializeComponent();
        }

        public int idRecibo;
        string calcularAto = string.Empty;
        int qtdCopias = 0;
        int totalCopias = 0;
        public Usuario usuarioLogado = new Usuario();
        ReciboBalcao reciboBalcao = new ReciboBalcao();
        public List<Ato> listaSelos = new List<Ato>();
        public List<Ato> listaSelosConsultaSelos = new List<Ato>();
        public List<Ato> listaSelosConsultaPendente = new List<Ato>();

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            usuarioLogado = winPrincipal.usuarioLogado;


            IniciaForm();

            btnNovo.Focus();

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

                txtLetraSelo.Text = classBalcao.LetraSeloAtual().Letra;

                btnNovo.IsEnabled = false;

                btnLimpar.IsEnabled = true;

                btnCancelar.IsEnabled = true;

                btnAdicionar.IsEnabled = true;

                groupBoxDadosRecibo.IsEnabled = true;

                groupBoxSelos.IsEnabled = true;

                lblNomeRecibo.Visibility = Visibility.Visible;

                lblRecibo.Visibility = Visibility.Visible;

                idRecibo = classBalcao.ProximoReciboBalcao();

                reciboBalcao = classBalcao.ReciboBalcao(idRecibo);

                lblRecibo.Content = reciboBalcao.IdReciboBalcao;

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


        private void IniciaForm()
        {
            try
            {
                int ano = DateTime.Now.Date.Year;

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


                custas = classCustasNotas.ListaCustas().Where(p => p.ANO == ano).ToList();

                porcentagemIss = Convert.ToDecimal(custas.Where(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());

                DateTime data = DateTime.Now.Date;

                datePickerData.SelectedDate = data;

                datePickerDataConsulta.SelectedDate = data;

                datePickerDataConsultaSelo.SelectedDate = data;

                datePickerDataConsultaPendentes.SelectedDate = data;


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

                txtLetraSelo.Text = classBalcao.LetraSeloAtual().Letra;

                listaUsuarios = classUsuario.ListaUsuarios();

                cmbFuncionario.ItemsSource = listaUsuarios.Select(p => p.NomeUsu);

                cmbFuncionario.SelectedItem = winPrincipal.usuarioLogado.NomeUsu;

                listaMensalista = classMensalista.ListaMensalistas();

                cmbMensalista.ItemsSource = listaMensalista.Select(p => p.Nome);

                txtRequisicao.Text = "";

                txtDesconto.Text = "0,00";

                txtAdicionar.Text = "0,00";

                lblNomeRecibo.Visibility = Visibility.Hidden;

                lblRecibo.Visibility = Visibility.Hidden;

                lblQtdCopia.Content = "0";

                lblQtdSeloAbert.Content = "0";

                lblQtdSeloAut.Content = "0";

                lblQtdSeloRecAut.Content = "0";

                lblQtdSeloRecSem.Content = "0";

                txtQtdAut.Text = "0";

                txtQtdAbert.Text = "0";

                txtQtdCopia.Text = "0";

                txtQtdRecAut.Text = "0";

                txtQtdRecSem.Text = "0";

                txtSeloInicialAut.Text = "";

                txtSeloFinalAut.Text = "";

                txtSeloInicialAbert.Text = "";

                txtSeloFinalAbert.Text = "";

                txtSeloInicialRecAut.Text = "";

                txtSeloFinalRecAut.Text = "";

                txtSeloInicialRecSem.Text = "";

                txtSeloFinalRecSem.Text = "";

                lblTotalAut.Content = "0,00";

                lblTotalAbert.Content = "0,00";

                lblTotalRecAut.Content = "0,00";

                lblTotalRecSem.Content = "0,00";

                lblTotal.Content = "0,00";

                lblTroco.Content = "0,00";

                txtValorPago.Text = "0,00";

                groupBoxDadosRecibo.IsEnabled = false;

                btnNovo.IsEnabled = true;

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
                }
                else
                {
                    txtRequisicao.Text = "";
                    cmbMensalista.SelectedIndex = -1;
                    cmbMensalista.IsEnabled = false;
                    txtRequisicao.IsEnabled = false;
                }
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {

            ProcNovo();
            status = "novo";
            cmbFuncionario.Focus();
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


                if (calcularAto == "autenticação")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "AUTENTICAÇÃO POR DOCUMENTO OU PÁGINA").Select(p => p.VALOR).FirstOrDefault());

                if (calcularAto == "abertura")
                {
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "ABERTURA DE FIRMA").Select(p => p.VALOR).FirstOrDefault());
                    arquivamento = Convert.ToDecimal(custas.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").Select(p => p.VALOR).FirstOrDefault());
                }

                if (calcularAto == "rec autenticidade")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p.VALOR).FirstOrDefault());

                if (calcularAto == "rec semelhança")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR SEMELHANÇA OU CHANCELA").Select(p => p.VALOR).FirstOrDefault());

                if (calcularAto == "materializacao")
                    emolumentos = Convert.ToDecimal(custas.Where(p => p.DESCR == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Select(p => p.VALOR).FirstOrDefault());

                if (cmbTipoCustas.SelectedIndex <= 1)
                {

                    emol = emolumentos + arquivamento;
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;
                    pmcmv_2 = emolumentos * 2 / 100;

                    //iss = (100 - porcentagemIss) / 100;
                    //iss = (emol + pmcmv_2) / iss - emol;


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
                    pmcmv_2 = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Siss = "0,00";
                    Spmcmv_2 = "0,00";


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

                return Convert.ToDecimal(Semol) * qtd + Convert.ToDecimal(Sfetj_20) * qtd + Convert.ToDecimal(Sfundperj_5) * qtd + Convert.ToDecimal(Sfunperj_5) * qtd + Convert.ToDecimal(Sfunarpen_4) * qtd + Convert.ToDecimal(Siss) * qtd + Convert.ToDecimal(Spmcmv_2) * qtd + (valorCopias * qtdCopia);

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
            catch (Exception)
            {

            }

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
            catch (Exception)
            {

            }

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
                catch (Exception)
                {

                }
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
            catch (Exception)
            {

            }
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
                catch (Exception)
                {

                }

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
                        e.Handled = !(key == 2 || key == 3);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key >= 23 && key <= 25);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
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
                        e.Handled = !(key == 2 || key == 3);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key >= 23 && key <= 25);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
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
                if ((txtQtdAut.Text != "" && txtQtdAut.Text != "0") || (txtQtdAbert.Text != "" && txtQtdAbert.Text != "0") || (txtQtdRecAut.Text != "" && txtQtdRecAut.Text != "0") || (txtQtdRecSem.Text != "" && txtQtdRecSem.Text != "0") || (txtQtdCopia.Text != "" && txtQtdCopia.Text != "0"|| (txtQtdMaterializacao.Text != "" && txtQtdMaterializacao.Text != "0")))
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


                            if (listaSelos.Count > 0)
                                GridValoresRecibo.IsEnabled = true;

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



                    }
                    else
                    {
                        MessageBox.Show("Informe a Data.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                txtValorPago.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                lblQtdSeloRecAut.Content = listaSelos.Where(p => p.TipoAto == "REC AUTENTICIDADE").Count();

                lblQtdSeloRecSem.Content = listaSelos.Where(p => p.TipoAto == "REC SEMELHANÇA").Count();
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
                    txtValorPago.Text = "0,00";
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

            IniciaForm();
            try
            {
                if (status == "novo")
                    classBalcao.RetornaReciboLivre(reciboBalcao);
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

        }

        private void txtValorPago_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPago.Text == "0,00")
            {
                txtValorPago.Text = "";
            }
        }


        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


        private void txtValorPago_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtValorPago.Text != "")
                {
                    try
                    {
                        txtValorPago.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorPago.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtValorPago.Text = "0,00";
                }
                if (txtValorPago.Text == "0,00")
                {
                    lblTroco.Content = "0,00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        bool ativo = true;


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (txtValorPago.Text != "0,00" && txtValorPago.Text != "")
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
                if (txtValorPago.Text != "")
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
                if (txtValorPago.Text == "0,00")
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
                if (txtValorPago.Text == "0,00")
                {
                    lblTroco.Content = "0,00";
                }
                else
                {
                    if (txtValorPago.Text == "")
                    {
                        txtValorPago.Text = "0,00";
                    }
                    lblTroco.Content = string.Format("{0:n2}", Convert.ToDecimal(txtValorPago.Text) - Convert.ToDecimal(lblTotal.Content));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtValorPago_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorPago.Text.Length > 0)
            {
                if (txtValorPago.Text.Contains(","))
                {
                    int index = txtValorPago.Text.IndexOf(",");

                    if (txtValorPago.Text.Length == index + 3)
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

        private void grid1_PreviewKeyDown(object sender, KeyEventArgs e)
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

                    if (status == "alterar")
                    {
                        classBalcao.RemoverSelosExistentes(Convert.ToInt32(selo.IdReciboBalcao));
                    }

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

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FinalizarRecibo();
                IniciaForm();
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
                
               
                for (int i = 0; i < listaSelos.Count; i++)
                {

                    listaSelos[i].IdReciboBalcao = reciboBalcao.IdReciboBalcao;
                    listaSelos[i].ReciboBalcao = reciboBalcao.NumeroRecibo;
                    listaSelos[i].Recibo = reciboBalcao.NumeroRecibo;
                    listaSelos[i].Pago = checkBoxPago.IsChecked.Value;


                    salvarSelo.SalvarAto(listaSelos[i], "novo");
                    

                }

                reciboBalcao.Data = datePickerData.SelectedDate;

                reciboBalcao.IdUsuario = listaUsuarios[cmbFuncionario.SelectedIndex].Id_Usuario;
                reciboBalcao.Usuario = listaUsuarios[cmbFuncionario.SelectedIndex].NomeUsu;
                reciboBalcao.Status = "UTILIZADO";
                reciboBalcao.Pago = checkBoxPago.IsChecked.Value;
                reciboBalcao.TipoPagamento = cmbTipoPagamento.Text;
                reciboBalcao.TipoCustas = cmbTipoCustas.Text;
                reciboBalcao.QuantAut = listaSelos.Where(p => p.TipoAto == "AUTENTICAÇÃO").Count();
                reciboBalcao.QuantAbert = listaSelos.Where(p => p.TipoAto == "ABERTURA DE FIRMAS").Count();
                reciboBalcao.QuantRecAut = listaSelos.Where(p => p.TipoAto == "REC AUTENTICIDADE").Count();
                reciboBalcao.QuantRecSem = listaSelos.Where(p => p.TipoAto == "REC SEMELHANÇA").Count();
                reciboBalcao.QuantMaterializacao = listaSelos.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS").Count();
                reciboBalcao.QuantCopia = totalCopias;
                reciboBalcao.ValorAdicionar = Convert.ToDecimal(txtAdicionar.Text);
                reciboBalcao.ValorDesconto = Convert.ToDecimal(txtDesconto.Text);
                reciboBalcao.Mensalista = cmbMensalista.Text;

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

                if (txtValorPago.Text != "")
                    reciboBalcao.ValorPago = Convert.ToDecimal(txtValorPago.Text);

                reciboBalcao.ValorTroco = Convert.ToDecimal(lblTroco.Content);

                classBalcao.SalvarRecibo(reciboBalcao);


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
                if (cmbTipoConsulta.SelectedIndex == 0)
                {
                    datePickerDataConsulta.Visibility = Visibility.Visible;
                    txtConsulta.Visibility = Visibility.Hidden;

                }
                else
                {
                    datePickerDataConsulta.Visibility = Visibility.Hidden;
                    txtConsulta.Visibility = Visibility.Visible;
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

        }

        private void VerificaMenu()
        {
            try
            {
                if (dataGridReciboBalcao.Items.Count > 0)
                {
                    reciboBalcao = (ReciboBalcao)dataGridReciboBalcao.SelectedItem;

                    if (usuarioLogado.Master == true || usuarioLogado.AlterarAtos == true || usuarioLogado.ExcluirAtos == true)
                    {
                        menuRecibo.Visibility = Visibility.Visible;

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
                    else
                    {

                        menuRecibo.Visibility = Visibility.Hidden;

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

                    SelosPendentes(listaSelosConsultaPendente);

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
                lblRecibo.Content = reciboBalcao.NumeroRecibo;

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

                lblTroco.Content = string.Format("{0:n2}", reciboBalcao.ValorTroco);

                txtValorPago.Text = string.Format("{0:n2}", reciboBalcao.ValorPago);

                totalCopias = Convert.ToInt32(reciboBalcao.QuantCopia);

                groupBoxSelos.IsEnabled = true;



                dataGridSelosAdicionados.IsEnabled = true;




                listaSelos = classBalcao.ListarSelosIdRecibo(reciboBalcao.IdReciboBalcao);

                dataGridSelosAdicionados.IsEnabled = true;
                dataGridSelosAdicionados.ItemsSource = listaSelos;
                dataGridSelosAdicionados.Items.Refresh();


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

                lblNomeRecibo.Visibility = Visibility.Visible;

                lblRecibo.Visibility = Visibility.Visible;

                GridValoresRecibo.IsEnabled = true;

                status = "alterar";
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


    }
}