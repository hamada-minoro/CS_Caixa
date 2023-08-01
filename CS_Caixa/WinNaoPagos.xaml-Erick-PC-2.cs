using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Objetos_de_Valor;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinNaoPagos.xaml
    /// </summary>
    public partial class WinNaoPagos : Window
    {

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        Usuario _usuario;

        List<ReciboBalcao> recibosBalcao = new List<ReciboBalcao>();

        List<CS_Caixa.Models.Ato> listaSelosConsultaSelos = new List<CS_Caixa.Models.Ato>();

        ClassBalcao classBalcao = new ClassBalcao();

        ClassAto classAto = new ClassAto();

        List<Usuario> listaFuncionarios = new List<Usuario>();

        List<CS_Caixa.Models.Ato> listaAtosNotas = new List<CS_Caixa.Models.Ato>();

        List<CS_Caixa.Models.Ato> listaAtosProtesto = new List<CS_Caixa.Models.Ato>();

        List<CS_Caixa.Models.Ato> listaAtosRgi = new List<CS_Caixa.Models.Ato>();

        ClassUsuario carregaNomesUsuarios = new ClassUsuario();

        List<ReciboBalcao> listaRecibosConsulta = new List<ReciboBalcao>();

        List<CS_Caixa.Models.Ato> listaSelosConsultaPendente = new List<CS_Caixa.Models.Ato>();
        decimal porcentagemIss = 0;
        WinPrincipal _principal;
        List<CS_Caixa.Models.Ato> atosNaoLancados = new List<CS_Caixa.Models.Ato>();
        List<CS_Caixa.Models.Ato> atosNaoLancadosRgi = new List<CS_Caixa.Models.Ato>();
        List<CS_Caixa.Models.Ato> atosNaoLancadosNotas = new List<CS_Caixa.Models.Ato>();
        ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();



        int ano = DateTime.Now.Year;

        List<CustasProtesto> listaCustas = new List<CustasProtesto>();

        public WinNaoPagos(Usuario usuario, WinPrincipal principal)
        {
            _usuario = usuario;
            _principal = principal;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            Inicio();
            VerificarAtosPendentes();
            ConsultaRecibos();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();
        }

        private void VerificarAtosPendentes()
        {
            if (dpData.SelectedDate == DateTime.Now.Date)
            {
                var listAtos = new List<CS_Caixa.Models.Ato>();

                if (dpData.SelectedDate != null)
                    listAtos = classAto.ListarAtoDataAto(dpData.SelectedDate.Value, dpData.SelectedDate.Value).Where(p => p.Pago == false).ToList(); ;


                if (listAtos.Count == 0)
                    lblAtosPendentes.Content = "";
                else
                    lblAtosPendentes.Content = "Atos Pendentes";
            }
            else
            {
                if (_usuario.Master == true || _usuario.Caixa == true)
                {
                    var listAtos = new List<CS_Caixa.Models.Ato>();

                    if (dpData.SelectedDate != null)
                        listAtos = classAto.ListarAtoDataAto(dpData.SelectedDate.Value, dpData.SelectedDate.Value).Where(p => p.Pago == false).ToList(); ;


                    if (listAtos.Count == 0)
                        lblAtosPendentes.Content = "";
                    else
                        lblAtosPendentes.Content = "Atos Pendentes";
                }
            }


        }


        private void Inicio()
        {

            classAto = new ClassAto();

            classBalcao = new ClassBalcao();

            dpData.SelectedDate = DateTime.Now.Date;

            datePickerDataConsulta.SelectedDate = DateTime.Now.Date;

            dpDataTotalProtesto.SelectedDate = DateTime.Now.Date;

            dpDataTotalRgi.SelectedDate = DateTime.Now.Date;

            dpDataTotalNotas.SelectedDate = DateTime.Now.Date;

            lblTotal.Content = "0,00";

            txtValorPago.Text = "0,00";

            lblTroco.Content = "0,00";

            lbltotalBalcao.Content = "Balcão: 0,00";

            lbltotalNotas.Content = "Notas: 0,00";

            lbltotalProtesto.Content = "Protesto: 0,00";

            lbltotalRgi.Content = "Rgi: 0,00";

            checkBalcaoTodos.IsChecked = false;
            checkNotasTodos.IsChecked = false;
            checkProtestoTodos.IsChecked = false;
            checkRgiTodos.IsChecked = false;


            CarregarComboBoxFuncionarios();


            cmbTipoAtoRgi.SelectedIndex = -1;

            cmbMensalista.SelectedIndex = -1;
            txtRequisicao.Text = "";

            cmbMensalistaRgi.SelectedIndex = -1;
            txtRequisicaoRgi.Text = "";
            checkBoxTroco.IsChecked = false;

            if (_usuario.Master == true || _usuario.Caixa == true)
            {
                cmbFuncionario.IsEnabled = true;
                cmbFuncionario.SelectedIndex = -1;
                cmbFuncionarioTotalProtesto.IsEnabled = true;
                cmbFuncionarioTotalProtesto.SelectedIndex = -1;
                cmbFuncionarioTotalRgi.IsEnabled = true;
                cmbFuncionarioTotalRgi.SelectedIndex = -1;
                cmbFuncionarioTotalNotas.IsEnabled = true;
                cmbFuncionarioTotalNotas.SelectedIndex = -1;
                btnPagar.IsEnabled = true;
                ckbTodos.IsEnabled = true;
                checkBoxTroco.Visibility = Visibility.Visible;
            }
            else
            {
                cmbFuncionario.IsEnabled = false;
                cmbFuncionarioTotalProtesto.IsEnabled = false;
                cmbFuncionarioTotalRgi.IsEnabled = false;
                cmbFuncionarioTotalNotas.IsEnabled = false;
                btnPagar.IsEnabled = false;
                ckbTodos.IsEnabled = false;
                checkBoxTroco.Visibility = Visibility.Hidden;
            }



            if (cmbFuncionario.SelectedItem != null && dpData.SelectedDate != null)
            {
                VerificarPendencias();
                MarcarTodosBalcao();
                MarcarTodosNotas();
                MarcarTodosProtesto();
                MarcarTodosRgi();
            }

        }

        private void VerificarPendencias()
        {
            try
            {

                VerificarAtosPendentes();

                Usuario usuarioSelecionado;

                classAto = new ClassAto();

                classBalcao = new ClassBalcao();

                if (cmbFuncionario.SelectedItem != null)
                {
                    usuarioSelecionado = (Usuario)cmbFuncionario.SelectedItem;



                    if (dpData.SelectedDate != null)
                    {

                        recibosBalcao = classBalcao.ListaRecibosBalcaoDataNomeNaoPago(dpData.SelectedDate.Value, usuarioSelecionado);

                        dataGridReciboBalcao.ItemsSource = recibosBalcao;
                        dataGridReciboBalcao.Items.Refresh();

                        dataGridReciboBalcao.SelectedIndex = 0;


                        DateTime dataIni, dataFim;

                        dataIni = dpData.SelectedDate.Value;
                        dataFim = dpData.SelectedDate.Value;



                        listaAtosNotas = classAto.ListarAtoDataNaoPago(dataIni, dataFim, "NOTAS", usuarioSelecionado);
                        dataGridNotas.ItemsSource = listaAtosNotas;
                        dataGridNotas.SelectedIndex = 0;

                        listaAtosProtesto = classAto.ListarAtoDataNaoPago(dataIni, dataFim, "PROTESTO", usuarioSelecionado);
                        dataGridProtesto.ItemsSource = listaAtosProtesto;

                        dataGridProtesto.SelectedIndex = 0;

                        listaAtosRgi = classAto.ListarAtoDataNaoPago(dataIni, dataFim, "RGI", usuarioSelecionado);
                        dataGridRgi.ItemsSource = listaAtosRgi;

                        dataGridRgi.SelectedIndex = 0;

                    }
                    else
                    {
                        MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void VerificarTodasPendencias()
        {
            try
            {
                classAto = new ClassAto();

                classBalcao = new ClassBalcao();

                if (dpData.SelectedDate != null)
                {
                    recibosBalcao = classBalcao.ListaRecibosBalcaoDataNomeNaoPagoTodos(dpData.SelectedDate.Value);


                    dataGridReciboBalcao.ItemsSource = recibosBalcao;
                    dataGridReciboBalcao.Items.Refresh();

                    dataGridReciboBalcao.SelectedIndex = 0;


                    DateTime dataIni, dataFim;

                    dataIni = dpData.SelectedDate.Value;
                    dataFim = dpData.SelectedDate.Value;

                    listaAtosNotas = classAto.ListarAtoDataNaoPagoTodos(dataIni, dataFim, "NOTAS");
                    dataGridNotas.ItemsSource = listaAtosNotas;

                    dataGridNotas.SelectedIndex = 0;

                    listaAtosProtesto = classAto.ListarAtoDataNaoPagoTodos(dataIni, dataFim, "PROTESTO");
                    dataGridProtesto.ItemsSource = listaAtosProtesto;

                    dataGridProtesto.SelectedIndex = 0;

                    listaAtosRgi = classAto.ListarAtoDataNaoPagoTodos(dataIni, dataFim, "RGI");
                    dataGridRgi.ItemsSource = listaAtosRgi;

                    dataGridRgi.SelectedIndex = 0;

                }
                else
                {
                    MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CalcularValores()
        {
            try
            {

                lbltotalBalcao.Content = string.Format("Balcão: {0:n2}", recibosBalcao.Where(p => p.Pago == true).Sum(p => p.Total));
                lbltotalNotas.Content = string.Format("Notas: {0:n2}", listaAtosNotas.Where(p => p.Pago == true).Sum(p => p.Total));
                lbltotalProtesto.Content = string.Format("Protesto: {0:n2}", listaAtosProtesto.Where(p => p.Pago == true).Sum(p => p.Total));
                lbltotalRgi.Content = string.Format("Rgi: {0:n2}", listaAtosRgi.Where(p => p.Pago == true).Sum(p => p.Total));


                lblTotal.Content = string.Format("{0:n2}", recibosBalcao.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosNotas.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosProtesto.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosRgi.Where(p => p.Pago == true).Sum(p => p.Total));

                CalculaTroco();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CalcularValoresTodos()
        {
            try
            {

                if (dpData.SelectedDate != null)
                {


                    lbltotalBalcao.Content = string.Format("Balcão: {0:n2}", recibosBalcao.Where(p => p.Pago == true).Sum(p => p.Total));
                    lbltotalNotas.Content = string.Format("Notas: {0:n2}", listaAtosNotas.Where(p => p.Pago == true).Sum(p => p.Total));
                    lbltotalProtesto.Content = string.Format("Protesto: {0:n2}", listaAtosProtesto.Where(p => p.Pago == true).Sum(p => p.Total));
                    lbltotalRgi.Content = string.Format("Rgi: {0:n2}", listaAtosRgi.Where(p => p.Pago == true).Sum(p => p.Total));

                    lblTotal.Content = string.Format("{0:n2}", recibosBalcao.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosNotas.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosProtesto.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosRgi.Where(p => p.Pago == true).Sum(p => p.Total));

                    CalculaTroco();

                    cmbFuncionario.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Informe da Data para consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CarregarComboBoxFuncionarios()
        {

            classAto = new ClassAto();

            classBalcao = new ClassBalcao();

            DateTime dataIni, dataFim;

            dataIni = dpData.SelectedDate.Value;
            dataFim = dpData.SelectedDate.Value;

            recibosBalcao = classBalcao.ListaRecibosBalcaoDataNomeNaoPagoTodos(dpData.SelectedDate.Value);

            listaAtosNotas = classAto.ListarAtoDataNaoPagoTodos(dataIni, dataFim, "NOTAS");

            listaAtosProtesto = classAto.ListarAtoDataNaoPagoTodos(dataIni, dataFim, "PROTESTO");

            listaAtosRgi = classAto.ListarAtoDataNaoPagoTodos(dataIni, dataFim, "RGI");


            listaFuncionarios = carregaNomesUsuarios.ListaUsuarios();

            cmbFuncionario.ItemsSource = listaFuncionarios.OrderBy(p => p.NomeUsu);
            cmbFuncionario.DisplayMemberPath = "NomeUsu";

            cmbFuncionarioTotalProtesto.ItemsSource = listaFuncionarios.OrderBy(p => p.NomeUsu);
            cmbFuncionarioTotalProtesto.DisplayMemberPath = "NomeUsu";

            cmbFuncionarioTotalRgi.ItemsSource = listaFuncionarios.OrderBy(p => p.NomeUsu);
            cmbFuncionarioTotalRgi.DisplayMemberPath = "NomeUsu";

            cmbFuncionarioTotalNotas.ItemsSource = listaFuncionarios.OrderBy(p => p.NomeUsu);
            cmbFuncionarioTotalNotas.DisplayMemberPath = "NomeUsu";


            Usuario usu = listaFuncionarios.Where(p => p.Id_Usuario == _usuario.Id_Usuario).FirstOrDefault();
            cmbFuncionario.SelectedItem = usu;

            cmbFuncionarioTotalProtesto.SelectedItem = usu;

            cmbFuncionarioTotalRgi.SelectedItem = usu;

            cmbFuncionarioTotalNotas.SelectedItem = usu;
        }

        private void cmbFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                lblTotal.Content = "0,00";

                txtValorPago.Text = "0,00";

                lblTroco.Content = "0,00";

                lbltotalBalcao.Content = "Balcão: 0,00";

                lbltotalNotas.Content = "Notas: 0,00";

                lbltotalProtesto.Content = "Protesto: 0,00";

                lbltotalRgi.Content = "Rgi: 0,00";


                checkBalcaoTodos.IsChecked = false;
                checkNotasTodos.IsChecked = false;
                checkProtestoTodos.IsChecked = false;
                checkRgiTodos.IsChecked = false;


                if (cmbFuncionario.SelectedItem != null && dpData.SelectedDate != null)
                {
                    VerificarPendencias();
                    MarcarTodosBalcao();
                    MarcarTodosNotas();
                    MarcarTodosProtesto();
                    MarcarTodosRgi();
                }


                txtValorPago.Text = string.Empty;
                txtValorPago.SelectAll();
                txtValorPago.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dpData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {


            if (dpData.SelectedDate != null)
            {
                VerificarPendencias();

                VerificarAtosPendentes();
                MarcarTodosBalcao();
                MarcarTodosNotas();
                MarcarTodosProtesto();
                MarcarTodosRgi();
            }

            if (ckbTodos.IsChecked == true)
            {
                cmbFuncionario.SelectedIndex = -1;
                cmbFuncionario.IsEnabled = false;
                VerificarTodasPendencias();
                VerificarAtosPendentes();
                MarcarTodosBalcao();
                MarcarTodosNotas();
                MarcarTodosProtesto();
                MarcarTodosRgi();
            }

        }

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
        //bool ativo2 = true;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (txtValorPago.Text != "0,00" && txtValorPago.Text != "" && lblTroco.Content != "0,00")
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

            //if (lblAtosPendentes.Content == "Atos Pendentes")
            //{
            //    if (ativo2 == true)
            //    {
            //        lblAtosPendentes.Foreground = new SolidColorBrush(Colors.Red);
            //        ativo2 = false;
            //    }
            //    else
            //    {
            //        lblAtosPendentes.Foreground = new SolidColorBrush(Colors.Transparent);
            //        ativo2 = true;
            //    }
            //}
        }

        private void txtValorPago_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var uie = e.OriginalSource as UIElement;

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    uie.MoveFocus(
                    new TraversalRequest(
                    FocusNavigationDirection.Next));

                }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtValorPago_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorPago.Text == "0,00")
            {
                txtValorPago.Text = "";
            }
        }

        private void grid1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtValorPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (txtValorPago.Text != "")
                {

                    if (recibosBalcao.Sum(p => p.Total) + listaAtosNotas.Sum(p => p.Total) + listaAtosProtesto.Sum(p => p.Total) + listaAtosRgi.Sum(p => p.Total) > 0)
                    {
                        CalculaTroco();
                    }
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
                if (txtValorPago.Text == "0,00" || txtValorPago.Text == "")
                {
                    lblTroco.Content = "0,00";
                }
                else
                {

                    var pago = 0M;

                    if (txtValorPago.Text != "")
                    {
                        pago = Convert.ToDecimal(txtValorPago.Text);
                    }

                    var total = recibosBalcao.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosNotas.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosProtesto.Where(p => p.Pago == true).Sum(p => p.Total) + listaAtosRgi.Where(p => p.Pago == true).Sum(p => p.Total);

                    var troco = pago - total;

                    lblTroco.Content = string.Format("{0:n2}", troco);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            bool salvarTroco = false;
            decimal valorPago = 0;
            if (recibosBalcao.Where(p => p.Pago == true).Count() + listaAtosNotas.Where(p => p.Pago == true).Count() + listaAtosProtesto.Where(p => p.Pago == true).Count() + listaAtosRgi.Where(p => p.Pago == true).Count() > 0)
            {
                if (checkBoxTroco.IsChecked == true)
                {
                    List<string> pagamentos = new List<string>();

                    if (recibosBalcao.Count > 0)
                    {
                        foreach (var item in recibosBalcao)
                        {
                            if (item.Pago == true)
                                pagamentos.Add(item.TipoPagamento);
                        }
                    }

                    if (listaAtosNotas.Count > 0)
                    {
                        foreach (var item in listaAtosNotas)
                        {
                            if (item.Pago == true)
                                pagamentos.Add(item.TipoPagamento);
                        }
                    }

                    if (listaAtosProtesto.Count > 0)
                    {
                        foreach (var item in listaAtosProtesto)
                        {
                            if (item.Pago == true)
                                pagamentos.Add(item.TipoPagamento);
                        }
                    }

                    if (listaAtosRgi.Count > 0)
                    {
                        foreach (var item in listaAtosRgi)
                        {
                            if (item.Pago == true)
                                pagamentos.Add(item.TipoPagamento);
                        }
                    }


                    var tipag = pagamentos.Distinct().Count();

                    if (tipag > 1)
                    {
                        MessageBox.Show("A função Salvar Troco é permitida apenas para pagamentos do mesmo tipo e diferentes PG MISTO ou MENSALISTA.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }


                    var recibosNaoAut = recibosBalcao.Where(p => p.TipoPagamento == "PG MISTO" || p.TipoPagamento == "MENSALISTA").ToList().Count;
                    var NotasNaoAut = listaAtosNotas.Where(p => p.TipoPagamento == "PG MISTO" || p.TipoPagamento == "MENSALISTA").ToList().Count;
                    var ProtestoNaoAut = listaAtosProtesto.Where(p => p.TipoPagamento == "PG MISTO" || p.TipoPagamento == "MENSALISTA").ToList().Count;
                    var RgiNaoAut = listaAtosRgi.Where(p => p.TipoPagamento == "PG MISTO" || p.TipoPagamento == "MENSALISTA").ToList().Count;

                    if (recibosNaoAut + NotasNaoAut + ProtestoNaoAut + RgiNaoAut > 0)
                    {
                        MessageBox.Show("A função Salvar Troco é permitida apenas para pagamentos diferentes PG MISTO ou MENSALISTA.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    if (Convert.ToDecimal(txtValorPago.Text) < Convert.ToDecimal(lblTotal.Content))
                    {
                        MessageBox.Show("O total deve ser menor que o valor pago, favor verifique.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    salvarTroco = true;
                    valorPago = Convert.ToDecimal(txtValorPago.Text);
                }

                if (MessageBox.Show("Confirmar pagamento do funcionário selecionado?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var aguarde = new WinAguardePagamento(recibosBalcao, listaAtosNotas, listaAtosProtesto, listaAtosRgi, salvarTroco, valorPago);
                    aguarde.Owner = this;
                    aguarde.ShowDialog();

                    Inicio();


                    cmbTipoConsulta.SelectedIndex = 0;
                    datePickerDataConsulta.SelectedDate = DateTime.Now.Date;
                    ConsultaRecibos();

                }
            }
            else
            {
                MessageBox.Show("Selecione ao menos um ato para efetuar o pagamento.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }



        private void ckbTodos_Checked(object sender, RoutedEventArgs e)
        {
            cmbFuncionario.SelectedIndex = -1;
            cmbFuncionario.IsEnabled = false;
            VerificarTodasPendencias();
            MarcarTodosBalcao();
            MarcarTodosNotas();
            MarcarTodosProtesto();
            MarcarTodosRgi();

        }

        private void ckbTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbFuncionario.IsEnabled = true;
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

            classBalcao = new ClassBalcao();

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
                            dataGridReciboBalcao1.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao1.IsEnabled = false;
                        }

                        dataGridReciboBalcao1.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao1.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));


                        listaSelosConsultaSelos = new List<CS_Caixa.Models.Ato>();

                        listaSelosConsultaSelos = classBalcao.ListarSelo(data).Where(p => p.Atribuicao == "BALCÃO").OrderBy(p => p.LetraSelo).OrderBy(p => p.NumeroSelo).ToList();


                        dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == true);
                        dataGridSelosPagos.Items.Refresh();

                        dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == false);
                        dataGridSelosNaoPagos.Items.Refresh();

                        lblTotalSelos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true).Sum(p => p.Total));
                        lblTotalNPagos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == false).Sum(p => p.Total));

                        listaSelosConsultaPendente = classBalcao.ListarSelo(data).Where(p => p.Atribuicao == "BALCÃO").OrderBy(p => p.LetraSelo).OrderBy(p => p.NumeroSelo).ToList();

                        dataGridPendentes.ItemsSource = listaSelosConsultaPendente;
                        dataGridPendentes.Items.Refresh();

                        listBoxPendentes.ItemsSource = VerificarTodosSelosNaoLancados();


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
                            dataGridReciboBalcao1.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao1.IsEnabled = false;
                        }
                        dataGridReciboBalcao1.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao1.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));

                        dataGridReciboBalcao1.SelectedIndex = 0;

                        if (dataGridReciboBalcao1.SelectedItem != null)
                        {

                            listaSelosConsultaSelos = classBalcao.ListaSelosBalcaoRecibo((ReciboBalcao)dataGridReciboBalcao1.SelectedItem);

                            dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == true);
                            dataGridSelosPagos.Items.Refresh();

                            dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == false);
                            dataGridSelosNaoPagos.Items.Refresh();

                            lblTotalSelos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true).Sum(p => p.Total));
                            lblTotalNPagos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == false).Sum(p => p.Total));
                        }
                        else
                        {
                            listaSelosConsultaSelos = null;
                            dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos;
                            dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos;
                            dataGridSelosPagos.Items.Refresh();
                            dataGridSelosNaoPagos.Items.Refresh();
                        }
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
                            dataGridReciboBalcao1.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao1.IsEnabled = false;
                        }
                        dataGridReciboBalcao1.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao1.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == true).Sum(p => p.Total));


                        dataGridReciboBalcao1.SelectedIndex = 0;

                        if (dataGridReciboBalcao1.SelectedItem != null)
                        {

                            listaSelosConsultaSelos = classBalcao.ListaSelosBalcaoRecibo((ReciboBalcao)dataGridReciboBalcao1.SelectedItem);

                            dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == true);
                            dataGridSelosPagos.Items.Refresh();

                            dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == false);
                            dataGridSelosNaoPagos.Items.Refresh();

                            lblTotalSelos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true).Sum(p => p.Total));
                            lblTotalNPagos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == false).Sum(p => p.Total));
                        }
                        else
                        {
                            listaSelosConsultaSelos = null;
                            dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos;
                            dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos;
                            dataGridSelosPagos.Items.Refresh();
                            dataGridSelosNaoPagos.Items.Refresh();
                        }


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
                            dataGridReciboBalcao1.IsEnabled = true;
                        }
                        else
                        {
                            dataGridReciboBalcao1.IsEnabled = false;
                        }

                        dataGridReciboBalcao1.ItemsSource = listaRecibosConsulta;

                        dataGridReciboBalcao1.Items.Refresh();

                        lbltotal.Content = string.Format("Total: {0:n2}", listaRecibosConsulta.Where(p => p.Pago == false).Sum(p => p.Total));


                        dataGridReciboBalcao1.SelectedIndex = 0;

                        if (dataGridReciboBalcao1.SelectedItem != null)
                        {

                            listaSelosConsultaSelos = classBalcao.ListaSelosBalcaoRecibo((ReciboBalcao)dataGridReciboBalcao1.SelectedItem);

                            dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == true);
                            dataGridSelosPagos.Items.Refresh();

                            dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos.Where(p => p.Pago == false);
                            dataGridSelosNaoPagos.Items.Refresh();

                            lblTotalSelos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == true).Sum(p => p.Total));
                            lblTotalNPagos.Content = string.Format("Total: {0:n2}", listaSelosConsultaSelos.Where(p => p.Pago == false).Sum(p => p.Total));
                        }
                        else
                        {
                            listaSelosConsultaSelos = null;
                            dataGridSelosNaoPagos.ItemsSource = listaSelosConsultaSelos;
                            dataGridSelosPagos.ItemsSource = listaSelosConsultaSelos;
                            dataGridSelosPagos.Items.Refresh();
                            dataGridSelosNaoPagos.Items.Refresh();
                        }


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


        private void SelosPendentes(List<CS_Caixa.Models.Ato> listaSelos)
        {
            List<string> listaPendentes = new List<string>();
            List<string> listaLetras = new List<string>();
            List<CS_Caixa.Models.Ato> listaAux = new List<CS_Caixa.Models.Ato>();
            try
            {
                listaLetras = listaSelos.Select(p => p.LetraSelo).Distinct().ToList();

                for (int i = 0; i < listaLetras.Count; i++)
                {
                    listaAux = new List<CS_Caixa.Models.Ato>();
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

        private List<string> VerificarTodosSelosNaoLancados()
        {
            var classAto = new ClassAto();
            List<string> selosNaoLancados = new List<string>();

            List<CS_Caixa.Models.Ato> selosCaixa = classAto.ListarAtoDataAto(datePickerDataConsulta.SelectedDate.Value, datePickerDataConsulta.SelectedDate.Value).Where(p => p.Atribuicao == "BALCÃO").ToList();


            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {

                string data = datePickerDataConsulta.SelectedDate.Value.ToShortDateString().Replace("/", ".");

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

                        if (selosCaixa.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero).Count() == 0 && listaSelosConsultaSelos.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero).Count() == 0)
                            selosNaoLancados.Add(selo);

                    }

                }

            }

            return selosNaoLancados;
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


        private void checkBalcaoTodos_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodosBalcao();
        }

        private void MarcarTodosBalcao()
        {
            if (recibosBalcao.Count == 0)
            {
                return;
            }
            foreach (var item in recibosBalcao)
            {
                item.Pago = true;
                dataGridReciboBalcao.Items.Refresh();
            }

        }

        private void checkBalcaoTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            if (desmarcar == false)
                DesmarcarTodosBalcao();

        }

        private void DesmarcarTodosBalcao()
        {
            if (recibosBalcao.Count == 0)
            {
                return;
            }

            foreach (var item in recibosBalcao)
            {
                item.Pago = false;
                dataGridReciboBalcao.Items.Refresh();
            }


        }

        private void checkedBalcaoUm_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();

            if (recibosBalcao.Where(p => p.Pago).Count() == recibosBalcao.Count())
                this.checkBalcaoTodos.IsChecked = true;
        }
        bool desmarcar = false;
        private void checkedBalcaoUm_Unchecked(object sender, RoutedEventArgs e)
        {
            desmarcar = true;
            this.checkBalcaoTodos.IsChecked = false;
            desmarcar = false;
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();
        }




        private void checkNotasTodos_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodosNotas();
        }

        private void checkNotasTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            if (desmarcar == false)
                desmarcarTodosNotas();
        }

        private void checkedNotasUm_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();

            if (listaAtosNotas.Where(p => p.Pago).Count() == listaAtosNotas.Count())
                this.checkNotasTodos.IsChecked = true;
        }

        private void checkedNotasUm_Unchecked(object sender, RoutedEventArgs e)
        {
            desmarcar = true;
            this.checkNotasTodos.IsChecked = false;
            desmarcar = false;
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();
        }

        private void MarcarTodosNotas()
        {
            if (listaAtosNotas.Count == 0)
            {
                return;
            }
            foreach (var item in listaAtosNotas)
            {
                item.Pago = true;
                dataGridNotas.Items.Refresh();
            }

        }

        private void desmarcarTodosNotas()
        {
            if (listaAtosNotas.Count == 0)
            {
                return;
            }
            foreach (var item in listaAtosNotas)
            {
                item.Pago = false;
                dataGridNotas.Items.Refresh();
            }

        }

        private void checkProtestoTodos_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodosProtesto();
        }

        private void checkProtestoTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            if (desmarcar == false)
                DesmarcarTodosProtesto();
        }

        private void checkedProtestoUm_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();

            if (listaAtosProtesto.Where(p => p.Pago).Count() == listaAtosProtesto.Count())
                this.checkProtestoTodos.IsChecked = true;
        }

        private void checkedProtestoUm_Unchecked(object sender, RoutedEventArgs e)
        {
            desmarcar = true;
            this.checkProtestoTodos.IsChecked = false;
            desmarcar = false;
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();
        }

        private void MarcarTodosProtesto()
        {
            if (listaAtosProtesto.Count == 0)
            {
                return;
            }
            foreach (var item in listaAtosProtesto)
            {
                item.Pago = true;
                dataGridProtesto.Items.Refresh();
            }

        }

        private void DesmarcarTodosProtesto()
        {
            if (listaAtosProtesto.Count == 0)
            {
                return;
            }
            foreach (var item in listaAtosProtesto)
            {
                item.Pago = false;
                dataGridProtesto.Items.Refresh();
            }

        }

        private void checkRgiTodos_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodosRgi();
        }

        private void checkRgiTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            if (desmarcar == false)
                DesmarcarTodosRgi();
        }

        private void checkedRgiUm_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();

            if (listaAtosRgi.Where(p => p.Pago).Count() == listaAtosRgi.Count())
                this.checkRgiTodos.IsChecked = true;
        }

        private void checkedRgiUm_Unchecked(object sender, RoutedEventArgs e)
        {
            desmarcar = true;
            this.checkRgiTodos.IsChecked = false;
            desmarcar = false;
            if (ckbTodos.IsChecked == false)
                CalcularValores();
            else
                CalcularValoresTodos();
        }


        private void MarcarTodosRgi()
        {
            if (listaAtosRgi.Count == 0)
            {
                return;
            }
            foreach (var item in listaAtosRgi)
            {
                item.Pago = true;
                dataGridRgi.Items.Refresh();
            }

        }

        private void DesmarcarTodosRgi()
        {
            if (listaAtosRgi.Count == 0)
            {
                return;
            }
            foreach (var item in listaAtosRgi)
            {
                item.Pago = false;
                dataGridRgi.Items.Refresh();
            }

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

        private void imageBalcao_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _principal.imageBalcao_MouseLeftButtonDown(sender, e);
        }

        private void dpDataTotalProtesto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDataTotalProtesto.SelectedDate.Value != null)
            {
                dataGridTotalProtesto.ItemsSource = ObterReciboProtesto();

                checkTotalProtestoTodos.IsChecked = false;
            }
        }

        private void cmbFuncionarioTotalProtesto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void checkTotalProtestoTodos_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosNaoLancados)
            {
                item.Pago = true;

            }
            dataGridTotalProtesto.Items.Refresh();
        }

        private void checkTotalProtestoTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosNaoLancados)
            {
                item.Pago = false;

            }
            dataGridTotalProtesto.Items.Refresh();
        }

        private void checkedTotalProtestoUm_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CS_Caixa.Models.Ato selecionado = (CS_Caixa.Models.Ato)dataGridTotalProtesto.SelectedItem;

                if (selecionado != null)
                {
                    selecionado.Pago = true;
                    CS_Caixa.Models.Ato marcar = atosNaoLancados.Where(p => p.Recibo == selecionado.Recibo && p.Pago == false).FirstOrDefault();

                    if (marcar != null)
                        if (marcar.Pago == false)
                            MarcarDesmarcarCheck(true, marcar);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }




        private void checkedTotalProtestoUm_Unchecked(object sender, RoutedEventArgs e)
        {




            try
            {
                CS_Caixa.Models.Ato selecionado = (CS_Caixa.Models.Ato)dataGridTotalProtesto.SelectedItem;

                if (selecionado != null)
                {
                    selecionado.Pago = false;
                    CS_Caixa.Models.Ato marcar = atosNaoLancados.Where(p => p.Recibo == selecionado.Recibo && p.Pago == true).FirstOrDefault();

                    if (marcar != null)
                        if (marcar.Pago == true)
                            MarcarDesmarcarCheck(false, marcar);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void MarcarDesmarcarCheck(bool check, CS_Caixa.Models.Ato marcar)
        {

            if (marcar.Pago != check)
            {
                marcar.Pago = check;

                dataGridTotalProtesto.Items.Refresh();
            }

        }


        private void MarcarDesmarcarCheckRgi(bool check, CS_Caixa.Models.Ato marcar)
        {

            if (marcar.Pago != check)
            {
                marcar.Pago = check;

                dataGridTotalRgi.Items.Refresh();
            }

        }

        private void MarcarDesmarcarCheckNotas(bool check, CS_Caixa.Models.Ato marcar)
        {

            if (marcar.Pago != check)
            {
                marcar.Pago = check;

                dataGridTotalNotas.Items.Refresh();
            }

        }


        private List<CS_Caixa.Models.Ato> ObterReciboProtesto()
        {
            ClassImportarAtosTotal classTotal = new ClassImportarAtosTotal();
            atosNaoLancados = classTotal.ObterReciboProtesto(dpDataTotalProtesto.SelectedDate.Value, _principal.usuarioLogado);
            return atosNaoLancados;

        }





        private void txtRequisicao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }


        private void cmbTipoPagamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoPagamento.Focus())
            {
                if (cmbTipoPagamento.SelectedIndex == 2)
                {
                    cmbMensalista.IsEnabled = true;
                    txtRequisicao.IsEnabled = true;


                    ClassMensalista classMensalista = new ClassMensalista();

                    cmbMensalista.ItemsSource = classMensalista.ListaMensalistas().Select(p => p.Nome);

                }
                else
                {
                    txtRequisicao.Text = "";
                    cmbMensalista.SelectedIndex = -1;
                    cmbMensalista.IsEnabled = false;
                    txtRequisicao.IsEnabled = false;
                    cmbMensalista.ItemsSource = null;
                }
            }
        }



        private void btnLancarProtesto_Click(object sender, RoutedEventArgs e)
        {

            if (atosNaoLancados.Count() <= 0)
            {
                MessageBox.Show("Nenhum ato encontrado para lançar.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (atosNaoLancados.Where(p => p.Pago == true).Count() <= 0)
            {
                MessageBox.Show("Selecione o(s) ato(s) que deseja lançar.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            ClassAto classAto = new ClassAto();


            if (cmbFuncionarioTotalProtesto.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Nome do Funcionário.", "Funcionário", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFuncionarioTotalProtesto.Focus();
                return;
            }

            if (cmbTipoPagamento.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Tipo de Pagamento.", "Tipo de Pagamento", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbTipoPagamento.Focus();
                return;
            }

            if (cmbTipoPagamento.SelectedIndex == 2)
            {
                if (cmbMensalista.SelectedIndex < 0)
                {
                    MessageBox.Show("Informe o Nome do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbMensalista.Focus();
                    return;


                }
                if (txtRequisicao.Text == "")
                {
                    MessageBox.Show("Informe o Numero da requisição do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtRequisicao.Focus();
                    return;
                }

            }
            ClassImportarAtosTotal importar = new ClassImportarAtosTotal();
            int idAtoTotal;

            int? folhas;

            foreach (var item in atosNaoLancados)
            {
                if (item.Pago == true)
                {

                    try
                    {
                        idAtoTotal = Convert.ToInt32(item.DescricaoAto);

                        // tipo de pagamento
                        item.TipoPagamento = cmbTipoPagamento.Text;

                        folhas = item.FolhaInical;

                        item.FolhaInical = null;

                        item.DescricaoAto = "I";

                        item.Pago = false;

                        if (item.Portador == "BOA VISTA SERVICOS S.A.")
                        {
                            item.Pago = true;
                            item.Natureza = "CERTIDÃO BOA VISTA";
                        }


                        if (item.Portador == "SERASA")
                        {
                            item.Pago = true;
                            item.Natureza = "CERTIDÃO SERASA";
                            item.Faixa = item.Faixa;
                        }

                        Usuario usu = (Usuario)cmbFuncionarioTotalProtesto.SelectedItem;

                        if (cmbTipoPagamento.SelectedIndex == 2)
                        {
                            // Mensalista
                            item.Mensalista = cmbMensalista.Text;

                            if (txtRequisicao.Text != "")
                                item.NumeroRequisicao = Convert.ToInt32(txtRequisicao.Text);
                        }
                        // Escrevente
                        item.Escrevente = usu.NomeUsu;

                        item.Usuario = usu.NomeUsu;

                        item.IdUsuario = usu.Id_Usuario;

                        if (item.TipoAto == "CANCELAMENTO")
                        {
                            item.Portador = item.FichaAto;
                            item.FichaAto = "";
                        }

                        int idAto = classAto.SalvarAto(item, "novo");

                        importar.SalvarItensCustas(idAtoTotal, idAto, folhas, item.TipoAto);

                        item.Id_Ato = idAto;



                        var valorPg = new ValorPago()
                        {
                            Data = item.DataPagamento,
                            Boleto = 0M,
                            Cheque = 0M,
                            ChequePre = 0M,
                            Deposito = 0M,
                            Dinheiro = 0M,
                            Mensalista = 0M,
                            CartaoCredito = 0M,
                            DataModificado = DateTime.Now.ToShortDateString(),
                            HoraModificado = DateTime.Now.ToLongTimeString(),
                            IdUsuario = usu.Id_Usuario,
                            NomeUsuario = usu.NomeUsu
                        };

                        valorPg.IdAto = idAto;
                        valorPg.IdReciboBalcao = 0;

                        if (cmbTipoPagamento.SelectedIndex == 0)
                            valorPg.Dinheiro = item.Total;

                        if (cmbTipoPagamento.SelectedIndex == 1)
                            valorPg.Deposito = item.Total;


                        if (cmbTipoPagamento.SelectedIndex == 2)
                            valorPg.Mensalista = item.Total;

                        if (cmbTipoPagamento.SelectedIndex == 3)
                            valorPg.Cheque = item.Total;

                        if (cmbTipoPagamento.SelectedIndex == 4)
                            valorPg.ChequePre = item.Total;

                        if (cmbTipoPagamento.SelectedIndex == 5)
                            valorPg.Boleto = item.Total;

                        if (cmbTipoPagamento.SelectedIndex == 6)
                            valorPg.CartaoCredito = item.Total;

                        valorPg.Total = item.Total;

                        classAto.SalvarValorPago(valorPg, "novo", "IdAto");



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro inesperado, por favor verifique se o registro foi salvo. Se não foi salvo tente novamente. >>>" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
            }

            Inicio();
            ConsultaRecibos();


            if (dpDataTotalProtesto.SelectedDate.Value != null)
            {
                dataGridTotalProtesto.ItemsSource = ObterReciboProtesto();
                dataGridTotalProtesto.Items.Refresh();
            }

        }




        private List<CS_Caixa.Models.Ato> ObterReciboRgi()
        {

            ClassImportarAtosTotal classTotal = new ClassImportarAtosTotal();
            atosNaoLancadosRgi = classTotal.ObterReciboRgi(dpDataTotalRgi.SelectedDate.Value, _principal.usuarioLogado);

            return atosNaoLancadosRgi;
        }






        private void cmbTipoCustasRgi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dpDataTotalRgi.IsLoaded)
                if (dpDataTotalRgi.SelectedDate.Value != null)
                {
                    dataGridTotalRgi.ItemsSource = ObterReciboRgi();

                    checkTotalRgiTodos.IsChecked = true;
                }
        }

        private void cmbTipoPagamentoRgi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoPagamentoRgi.Focus())
            {
                if (cmbTipoPagamentoRgi.SelectedIndex == 2)
                {
                    cmbMensalistaRgi.IsEnabled = true;
                    txtRequisicaoRgi.IsEnabled = true;


                    ClassMensalista classMensalista = new ClassMensalista();

                    cmbMensalistaRgi.ItemsSource = classMensalista.ListaMensalistas().Select(p => p.Nome);

                }
                else
                {
                    txtRequisicaoRgi.Text = "";
                    cmbMensalistaRgi.SelectedIndex = -1;
                    cmbMensalistaRgi.IsEnabled = false;
                    txtRequisicaoRgi.IsEnabled = false;
                    cmbMensalistaRgi.ItemsSource = null;
                }
            }
        }

        private void btnLancarRgi_Click(object sender, RoutedEventArgs e)
        {
            if (atosNaoLancadosRgi.Count() <= 0)
                return;

            if (atosNaoLancadosRgi.Where(p => p.Pago == true).Count() <= 0)
                return;


            ClassAto classAto = new ClassAto();


            if (cmbFuncionarioTotalRgi.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Nome do Funcionário.", "Funcionário", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFuncionarioTotalRgi.Focus();
                return;
            }

            if (cmbTipoAtoRgi.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Tipo de Ato.", "Tipo de Ato", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbTipoAtoRgi.Focus();
                return;
            }

            if (cmbTipoPagamentoRgi.SelectedIndex == -1)
            {
                MessageBox.Show("Informe o Tipo de Pagamento.", "Tipo de Pagamento", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbTipoPagamentoRgi.Focus();
                return;
            }



            if (cmbTipoPagamentoRgi.SelectedIndex == 2)
            {
                if (cmbMensalistaRgi.SelectedIndex < 0)
                {
                    MessageBox.Show("Informe o Nome do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbMensalistaRgi.Focus();
                    return;


                }
                if (txtRequisicaoRgi.Text == "")
                {
                    MessageBox.Show("Informe o Numero da requisição do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtRequisicaoRgi.Focus();
                    return;
                }

            }

            List<CS_Caixa.Models.Ato> recibos = atosNaoLancadosRgi.Where(p => p.Pago == true).ToList();

            CS_Caixa.Models.Ato ato;


            Usuario usu = (Usuario)cmbFuncionarioTotalRgi.SelectedItem;


            try
            {


                foreach (var item in recibos)
                {


                    ato = new CS_Caixa.Models.Ato();

                    ato.TipoPagamento = cmbTipoPagamentoRgi.Text;

                    ato.TipoCobranca = item.TipoCobranca;

                    if (cmbMensalistaRgi.SelectedIndex > -1)
                    {
                        ato.Mensalista = cmbMensalistaRgi.Text;

                        if (txtRequisicaoRgi.Text != "")
                            ato.NumeroRequisicao = Convert.ToInt32(txtRequisicaoRgi.Text);
                    }
                    ato.DataPagamento = item.DataPagamento;

                    ato.DataAto = item.DataAto;


                    ato.Atribuicao = "RGI";

                    ato.Portador = item.Portador;

                    ato.Pago = false;

                    ato.Natureza = item.Natureza.ToUpper();

                    ato.Faixa = item.Faixa;

                    ato.Recibo = item.Recibo;

                    ato.Protocolo = item.Protocolo;

                    ato.TipoPrenotacao = item.TipoPrenotacao;

                    ato.QtdAtos = item.QtdAtos;

                    ato.DescricaoAto = "I";

                    ato.Emolumentos = item.Emolumentos;

                    ato.Fetj = item.Fetj;

                    ato.Fundperj = item.Fundperj;

                    ato.Funperj = item.Funperj;

                    ato.Funarpen = item.Funarpen;

                    ato.Pmcmv = item.Pmcmv;

                    ato.Iss = item.Iss;

                    ato.Mutua = item.Mutua;

                    ato.Acoterj = item.Acoterj;

                    ato.Distribuicao = item.Distribuicao;

                    ato.QuantPrenotacao = item.QuantPrenotacao;

                    ato.QuantIndisp = item.QuantIndisp;

                    ato.Indisponibilidade = item.Indisponibilidade;

                    ato.Prenotacao = item.Prenotacao;

                    ato.IdUsuario = usu.Id_Usuario;

                    ato.Usuario = usu.NomeUsu;

                    ato.Escrevente = usu.NomeUsu;

                    ato.TipoAto = cmbTipoAtoRgi.Text;

                    ato.ValorAdicionar = 0;

                    ato.ValorDesconto = 0;

                    ato.Total = item.Total;

                    ato.FichaAto = null;

                    int idAto = classAto.SalvarAto(ato, "novo");


                    SalvarItemAto(idAto, item.ItensAtoRgis.ToList());


                    var valorPago = new ValorPago();
                    ato.Id_Ato = idAto;

                    var valorPg = new ValorPago()
                    {
                        Data = ato.DataPagamento,
                        Boleto = 0M,
                        Cheque = 0M,
                        ChequePre = 0M,
                        Deposito = 0M,
                        Dinheiro = 0M,
                        Mensalista = 0M,
                        CartaoCredito = 0M,
                        DataModificado = DateTime.Now.ToShortDateString(),
                        HoraModificado = DateTime.Now.ToLongTimeString(),
                        IdUsuario = usu.Id_Usuario,
                        NomeUsuario = usu.NomeUsu
                    };

                    valorPg.IdAto = idAto;
                    valorPg.IdReciboBalcao = 0;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 0)
                        valorPg.Dinheiro = ato.Total;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 1)
                        valorPg.Deposito = ato.Total;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 2)
                        valorPg.Mensalista = ato.Total;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 3)
                        valorPg.Cheque = ato.Total;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 4)
                        valorPg.ChequePre = ato.Total;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 5)
                        valorPg.Boleto = ato.Total;

                    if (cmbTipoPagamentoRgi.SelectedIndex == 6)
                        valorPg.CartaoCredito = ato.Total;

                    valorPg.Total = ato.Total;

                    classAto.SalvarValorPago(valorPg, "novo", "IdAto");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado, por favor verifique se o registro foi salvo. Se não foi salvo tente novamente. >>>" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            Inicio();
            ConsultaRecibos();

            if (dpDataTotalRgi.SelectedDate.Value != null)
            {
                dataGridTotalRgi.ItemsSource = ObterReciboRgi();
                dataGridTotalRgi.Items.Refresh();
            }
        }

        private void SalvarItemAto(int idAto, List<ItensAtoRgi> listaItensAtos)
        {
            try
            {

                ClassCustasRgi classCustasRgi = new ClassCustasRgi();
                ItensAtoRgi itemSalvar;
                int idAtoRgi = 0;
                int idAtoTotal = 0;

                for (int i = 0; i < listaItensAtos.Count; i++)
                {
                    idAtoTotal = listaItensAtos[i].Id_Ato;
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

                    idAtoRgi = classCustasRgi.SalvarItensListaAto(itemSalvar);
                    SalvarItemCustas(idAtoRgi, idAto, idAtoTotal, itemSalvar.Cont);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o registro. " + ex.Message);
            }
        }

        private void SalvarItemCustas(int idAtoRgi, int idAto, int idAtoTotal, int? count)
        {
            try
            {
                ClassCustasRgi classCustasRgi = new ClassCustasRgi();

                ClassImportarAtosTotal importar = new ClassImportarAtosTotal();

                List<ItensCustasRgi> listaItensCustas = importar.ObterCustasRgi(idAtoTotal);

                for (int cont = 0; cont <= listaItensCustas.Count - 1; cont++)
                {
                    ItensCustasRgi item = new ItensCustasRgi();

                    item.Id_AtoRgi = idAtoRgi;

                    item.Id_Ato = idAto;

                    item.Cont = count;

                    item.Tabela = listaItensCustas[cont].Tabela;

                    item.Item = listaItensCustas[cont].Item;

                    item.SubItem = listaItensCustas[cont].SubItem;

                    item.Quantidade = listaItensCustas[cont].Quantidade;

                    item.Valor = listaItensCustas[cont].Valor;

                    item.Total = listaItensCustas[cont].Total;

                    item.Descricao = listaItensCustas[cont].Descricao;

                    classCustasRgi.SalvarItensLista(item);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o registro. " + ex.Message);
            }
        }

        private void checkTotalRgiTodos_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosNaoLancadosRgi)
            {
                item.Pago = true;

            }
            dataGridTotalRgi.Items.Refresh();
        }

        private void checkTotalRgiTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosNaoLancadosRgi)
            {
                item.Pago = false;

            }
            dataGridTotalRgi.Items.Refresh();
        }

        private void checkedTotalRgiUm_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CS_Caixa.Models.Ato selecionado = (CS_Caixa.Models.Ato)dataGridTotalRgi.SelectedItem;

                if (selecionado != null)
                {
                    selecionado.Pago = true;
                    CS_Caixa.Models.Ato marcar = atosNaoLancadosRgi.Where(p => p.Recibo == selecionado.Recibo && p.Pago == false).FirstOrDefault();

                    if (marcar != null)
                        if (marcar.Pago == false)
                            MarcarDesmarcarCheckRgi(true, marcar);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void checkedTotalRgiUm_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CS_Caixa.Models.Ato selecionado = (CS_Caixa.Models.Ato)dataGridTotalRgi.SelectedItem;

                if (selecionado != null)
                {
                    //selecionado.Pago = false;
                    CS_Caixa.Models.Ato marcar = atosNaoLancadosRgi.Where(p => p.Recibo == selecionado.Recibo && p.Pago == true).FirstOrDefault();

                    if (marcar != null)
                        if (marcar.Pago == true)
                            MarcarDesmarcarCheckRgi(false, marcar);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dpDataTotalRgi_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDataTotalRgi.SelectedDate.Value != null)
            {
                dataGridTotalRgi.ItemsSource = ObterReciboRgi();

                checkTotalRgiTodos.IsChecked = false;
            }
        }

        private void dpDataTotalRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if (dpDataTotalRgi.SelectedDate.Value != null)
                {
                    dataGridTotalRgi.ItemsSource = ObterReciboRgi();
                    checkTotalRgiTodos.IsChecked = false;
                    dataGridTotalRgi.Items.Refresh();
                }
            }
        }








        private List<CS_Caixa.Models.Ato> ObterReciboNotas()
        {
            ClassImportarAtosTotal classTotal = new ClassImportarAtosTotal();
            atosNaoLancadosNotas = classTotal.ObterReciboNotas(dpDataTotalNotas.SelectedDate.Value, _principal.usuarioLogado);
            return atosNaoLancadosNotas;

        }


        private void dpDataTotalNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (dpDataTotalNotas.SelectedDate.Value != null)
                {
                    dataGridTotalNotas.ItemsSource = ObterReciboNotas();
                    checkNotasTodos.IsChecked = false;
                    dataGridTotalNotas.Items.Refresh();
                }
            }
        }

        private void btnLancarNotas_Click(object sender, RoutedEventArgs e)
        {
            if (atosNaoLancadosNotas.Count() <= 0)
                return;

            if (atosNaoLancadosNotas.Where(p => p.Pago == true).Count() <= 0)
                return;


            ClassAto classAto = new ClassAto();


            if (cmbFuncionarioTotalNotas.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Nome do Funcionário.", "Funcionário", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFuncionarioTotalNotas.Focus();
                return;
            }



            if (cmbTipoPagamentoNotas.SelectedIndex == -1)
            {
                MessageBox.Show("Informe o Tipo de Pagamento.", "Tipo de Pagamento", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbTipoPagamentoNotas.Focus();
                return;
            }


            if (cmbTipoPagamentoNotas.SelectedIndex == 2)
            {
                if (cmbMensalistaNotas.SelectedIndex < 0)
                {
                    MessageBox.Show("Informe o Nome do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbMensalistaNotas.Focus();
                    return;


                }
                if (txtRequisicaoNotas.Text == "")
                {
                    MessageBox.Show("Informe o Numero da requisição do Mensalista.", "Mensalista", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtRequisicaoNotas.Focus();
                    return;
                }

            }

            List<CS_Caixa.Models.Ato> recibos = atosNaoLancadosNotas.Where(p => p.Pago == true).ToList();

            CS_Caixa.Models.Ato ato;
            ClassImportarAtosTotal importar = new ClassImportarAtosTotal();


            try
            {


                foreach (var item in recibos)
                {

                    ato = new CS_Caixa.Models.Ato();

                    ato.TipoPagamento = cmbTipoPagamentoNotas.Text;

                    ato.TipoCobranca = item.TipoCobranca;

                    if (cmbMensalistaNotas.SelectedIndex > -1)
                    {
                        ato.Mensalista = cmbMensalistaNotas.Text;

                        if (txtRequisicaoNotas.Text != "")
                            ato.NumeroRequisicao = Convert.ToInt32(txtRequisicaoNotas.Text);
                    }
                    ato.DataPagamento = item.DataPagamento;

                    ato.DataAto = item.DataAto;

                    ato.TipoPagamento = cmbTipoPagamentoNotas.Text;

                    ato.Atribuicao = "NOTAS";

                    ato.Portador = item.Portador;

                    ato.Pago = false;

                    ato.Natureza = item.Natureza.ToUpper();

                    ato.Faixa = item.Faixa;

                    ato.Recibo = item.Recibo;

                    ato.Protocolo = item.Protocolo;

                    ato.TipoPrenotacao = item.TipoPrenotacao;

                    ato.QtdAtos = item.QtdAtos;

                    ato.DescricaoAto = "I";

                    ato.Emolumentos = item.Emolumentos;

                    ato.Fetj = item.Fetj;

                    ato.Fundperj = item.Fundperj;

                    ato.Funperj = item.Funperj;

                    ato.Funarpen = item.Funarpen;

                    ato.Pmcmv = item.Pmcmv;

                    ato.Iss = item.Iss;

                    ato.Mutua = item.Mutua;

                    ato.Acoterj = item.Acoterj;

                    ato.Distribuicao = item.Distribuicao;

                    ato.QuantPrenotacao = item.QuantPrenotacao;

                    ato.QuantIndisp = item.QuantIndisp;

                    ato.QuantDistrib = item.QuantDistrib;

                    ato.ValorAdicionar = 0;

                    ato.ValorCorretor = 0;

                    ato.ValorDesconto = 0;

                    ato.ValorEscrevente = 0;

                    ato.ValorPago = 0;

                    ato.Indisponibilidade = item.Indisponibilidade;

                    ato.Prenotacao = item.Prenotacao;

                    ato.Escrevente = cmbFuncionarioTotalNotas.Text;

                    ato.Usuario = cmbFuncionarioTotalNotas.Text;

                    ato.IdUsuario = ((Usuario)cmbFuncionarioTotalNotas.SelectedItem).Id_Usuario;

                    ato.TipoAto = "CERTIDÃO NOTAS";

                    ato.Total = item.Total;

                    int idAto = classAto.SalvarAto(ato, "novo");

                    importar.SalvarItensCustasNotas(item.ItensCustasNotas.ToList(), idAto);

                    var valorPago = new ValorPago();
                    ato.Id_Ato = idAto;

                    var valorPg = new ValorPago()
                    {
                        Data = ato.DataPagamento,
                        Boleto = 0M,
                        Cheque = 0M,
                        ChequePre = 0M,
                        Deposito = 0M,
                        Dinheiro = 0M,
                        Mensalista = 0M,
                        CartaoCredito = 0,
                        DataModificado = DateTime.Now.ToShortDateString(),
                        HoraModificado = DateTime.Now.ToLongTimeString(),
                        IdUsuario = _usuario.Id_Usuario,
                        NomeUsuario = _usuario.NomeUsu
                    };

                    valorPg.IdAto = idAto;
                    valorPg.IdReciboBalcao = 0;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 0)
                        valorPg.Dinheiro = ato.Total;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 1)
                        valorPg.Deposito = ato.Total;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 2)
                        valorPg.Mensalista = ato.Total;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 3)
                        valorPg.Cheque = ato.Total;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 4)
                        valorPg.ChequePre = ato.Total;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 5)
                        valorPg.Boleto = ato.Total;

                    if (cmbTipoPagamentoNotas.SelectedIndex == 6)
                        valorPg.CartaoCredito = ato.Total;

                    valorPg.Total = ato.Total;

                    classAto.SalvarValorPago(valorPg, "novo", "IdAto");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado, por favor verifique se o registro foi salvo. Se não foi salvo tente novamente. >>>" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            Inicio();
            ConsultaRecibos();

            if (dpDataTotalNotas.SelectedDate.Value != null)
            {
                dataGridTotalNotas.ItemsSource = ObterReciboNotas();
                dataGridTotalNotas.Items.Refresh();
            }
        }

        private void checkedTotalNotasUm_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CS_Caixa.Models.Ato selecionado = (CS_Caixa.Models.Ato)dataGridTotalNotas.SelectedItem;

                if (selecionado != null)
                {
                    selecionado.Pago = true;
                    CS_Caixa.Models.Ato marcar = atosNaoLancadosNotas.Where(p => p.Recibo == selecionado.Recibo && p.Pago == false).FirstOrDefault();

                    if (marcar != null)
                        if (marcar.Pago == false)
                            MarcarDesmarcarCheckNotas(true, marcar);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void checkedTotalNotasUm_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CS_Caixa.Models.Ato selecionado = (CS_Caixa.Models.Ato)dataGridTotalNotas.SelectedItem;

                if (selecionado != null)
                {
                    //selecionado.Pago = false;
                    CS_Caixa.Models.Ato marcar = atosNaoLancadosNotas.Where(p => p.Recibo == selecionado.Recibo && p.Pago == true).FirstOrDefault();

                    if (marcar != null)
                        if (marcar.Pago == true)
                            MarcarDesmarcarCheckNotas(false, marcar);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void checkTotalNotas_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosNaoLancadosNotas)
            {
                item.Pago = true;

            }
            dataGridTotalNotas.Items.Refresh();
        }

        private void checkTotalNotasTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosNaoLancadosNotas)
            {
                item.Pago = false;

            }
            dataGridTotalNotas.Items.Refresh();
        }

        private void dpDataTotalNotas_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDataTotalNotas.SelectedDate.Value != null)
            {
                dataGridTotalNotas.ItemsSource = ObterReciboNotas();

                checkTotalNotas.IsChecked = false;
            }
        }

        private void cmbTipoPagamentoNotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoPagamentoNotas.Focus())
            {
                if (cmbTipoPagamentoNotas.SelectedIndex == 2)
                {
                    cmbMensalistaNotas.IsEnabled = true;
                    txtRequisicaoNotas.IsEnabled = true;


                    ClassMensalista classMensalista = new ClassMensalista();

                    cmbMensalistaNotas.ItemsSource = classMensalista.ListaMensalistas().Select(p => p.Nome);

                }
                else
                {
                    txtRequisicaoNotas.Text = "";
                    cmbMensalistaNotas.SelectedIndex = -1;
                    cmbMensalistaNotas.IsEnabled = false;
                    txtRequisicaoNotas.IsEnabled = false;
                    cmbMensalistaNotas.ItemsSource = null;
                }
            }
        }



        private void cmbFuncionarioTotalNotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                CalculaTrocoValorPago();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CalculaTrocoValorPago()
        {
            try
            {
                if (txtValorPagoDinheiro != null && txtValorPagoDeposito != null && txtValorPagoCheque != null && txtValorPagoChequePre != null && txtValorPagoBoleto != null)
                {
                    decimal dinheiro, deposito, cheque, chequePre, boleto;


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


                    var valoresPagos = dinheiro + deposito + cheque + chequePre + boleto;


                    if (valoresPagos != 0M)
                        lblTrocoPagamento.Content = string.Format("{0:n2}", valoresPagos - Convert.ToDecimal(lblTotalPagamento.Content));
                    else
                        lblTrocoPagamento.Content = "0,00";
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

        private void txtValorPagoDeposito_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                CalculaTrocoValorPago();
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

                CalculaTrocoValorPago();
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

                CalculaTrocoValorPago();
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

                CalculaTrocoValorPago();
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

        List<ValorPago> pagamentos;
        List<CS_Caixa.Models.Ato> atos = new List<CS_Caixa.Models.Ato>();
        List<ReciboBalcao> balcao = new List<ReciboBalcao>();
        List<AtosValorPago> atosValorPago = new List<AtosValorPago>();

        List<ValorPago> pagamentosSeleciodo = new List<ValorPago>();
        List<CS_Caixa.Models.Ato> atosAtos = new List<CS_Caixa.Models.Ato>();
        List<ReciboBalcao> atosBalcao = new List<ReciboBalcao>();

        AtosValorPago atoSelecionado;
        private void SelecionarAtosPagamento()
        {
            atoSelecionado = (AtosValorPago)dataGridAtosValorPago.SelectedItem;

            pagamentosSeleciodo = new List<ValorPago>();

            atosBalcao = new List<ReciboBalcao>();

            atosAtos = new List<CS_Caixa.Models.Ato>();

            if (atoSelecionado != null)
            {
                pagamentosSeleciodo = pagamentos.Where(p => p.IdPagamento == atoSelecionado.IdPagamento).ToList();

                dataGridValorPago.ItemsSource = pagamentosSeleciodo;



                foreach (var item in pagamentosSeleciodo)
                {
                    if (item.IdReciboBalcao > 0)
                        atosBalcao.Add(balcao.Where(p => p.IdReciboBalcao == item.IdReciboBalcao).FirstOrDefault());

                    if (item.IdAto > 0)
                        atosAtos.Add(atos.Where(p => p.Id_Ato == item.IdAto).FirstOrDefault());
                }


                dataGridReciboBalcaoValorPago.ItemsSource = atosBalcao;
                dataGridGeralValorPago.ItemsSource = atosAtos;

                txtValorPagoDinheiro.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Dinheiro));
                txtValorPagoDeposito.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Deposito));
                txtValorPagoCheque.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Cheque));
                txtValorPagoChequePre.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.ChequePre));
                txtValorPagoBoleto.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Boleto));
                lblTrocoPagamento.Content = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Troco));
                lblTotalPagamento.Content = string.Format("{0:n2}", atosAtos.Sum(p => p.Total) + atosBalcao.Sum(p => p.Total));
            }
            else
            {
                dataGridValorPago.ItemsSource = pagamentosSeleciodo;
                dataGridReciboBalcaoValorPago.ItemsSource = atosBalcao;
                dataGridGeralValorPago.ItemsSource = atosAtos;

                txtValorPagoDinheiro.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Dinheiro));
                txtValorPagoDeposito.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Deposito));
                txtValorPagoCheque.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Cheque));
                txtValorPagoChequePre.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.ChequePre));
                txtValorPagoBoleto.Text = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Boleto));
                lblTrocoPagamento.Content = string.Format("{0:n2}", pagamentosSeleciodo.Sum(p => p.Troco));
                lblTotalPagamento.Content = string.Format("{0:n2}", atosAtos.Sum(p => p.Total) + atosBalcao.Sum(p => p.Total));
            }
        }

        private void dpDataValorPago_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDataValorPago.SelectedDate != null)
                ConsultaData(dpDataValorPago.SelectedDate.Value);
        }

        private void ConsultaData(DateTime data)
        {
            if (data != null)
            {
                pagamentos = new List<ValorPago>();

                pagamentos = classAto.ObterValorPagoPorDataIdPagamento(data);

                var atoValor = new AtosValorPago();

                atosValorPago = new List<AtosValorPago>();

                foreach (var item in pagamentos)
                {

                    if (item.IdAto > 0)
                    {
                        var ato = classAto.ObterAtoPorIdAto(item.IdAto);

                        if (ato != null)
                        {
                            atos.Add(ato);

                            atoValor = new AtosValorPago();

                            atoValor.IdPagamento = Convert.ToInt32(item.IdPagamento);
                            atoValor.IdAto = ato.Id_Ato;
                            atoValor.IdReciboBalcao = 0;
                            atoValor.Pago = ato.Pago;
                            if (ato.Protocolo != null)
                                atoValor.Protocolo = Convert.ToInt32(ato.Protocolo);
                            if (ato.Recibo != null)
                                atoValor.Recibo = Convert.ToInt32(ato.Recibo);
                            atoValor.TipoCobranca = ato.TipoCobranca;
                            atoValor.TipoPagamento = ato.TipoPagamento;
                            atoValor.Total = Convert.ToDecimal(ato.Total);
                            atoValor.Atribuicao = ato.Atribuicao;
                            atoValor.Funcionario = ato.Escrevente;
                            atosValorPago.Add(atoValor);
                        }
                    }
                    if (item.IdReciboBalcao > 0)
                    {
                        var balc = classAto.ObterReciboBalcaoPorIdAto(item.IdReciboBalcao);

                        if (balc != null)
                        {
                            balcao.Add(balc);

                            atoValor = new AtosValorPago();

                            atoValor.IdPagamento = Convert.ToInt32(item.IdPagamento);
                            atoValor.IdAto = 0;
                            atoValor.IdReciboBalcao = balc.IdReciboBalcao;
                            atoValor.Pago = balc.Pago;
                            atoValor.Protocolo = 0;
                            atoValor.Recibo = Convert.ToInt32(balc.NumeroRecibo);
                            atoValor.TipoCobranca = balc.TipoCustas;
                            atoValor.TipoPagamento = balc.TipoPagamento;
                            atoValor.Total = Convert.ToDecimal(balc.Total);
                            atoValor.Atribuicao = "BALCÃO";
                            atoValor.Funcionario = balc.Usuario;
                            atosValorPago.Add(atoValor);
                        }
                    }
                }

                dataGridAtosValorPago.ItemsSource = atosValorPago;

                if (atosValorPago.Count > 0)
                    dataGridAtosValorPago.SelectedIndex = 0;
            }
        }


        private void dataGridAtosValorPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelecionarAtosPagamento();
        }

        private void dataGridReciboBalcaoValorPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selecionado = (ReciboBalcao)dataGridReciboBalcaoValorPago.SelectedItem;

            if (selecionado != null)
            {

                int cont = 0;
                foreach (var item in dataGridValorPago.ItemsSource)
                {
                    var itemAtual = (ValorPago)item;



                    if (itemAtual.IdReciboBalcao == selecionado.IdReciboBalcao)
                    {
                        dataGridValorPago.SelectedIndex = cont;

                    }

                    cont = cont + 1;
                }
            }
        }

        private void dataGridReciboBalcaoValorPago_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridReciboBalcaoValorPago_GotFocus(object sender, RoutedEventArgs e)
        {
            var selecionado = (ReciboBalcao)dataGridReciboBalcaoValorPago.SelectedItem;

            if (selecionado != null)
            {

                int cont = 0;
                foreach (var item in dataGridValorPago.ItemsSource)
                {
                    var itemAtual = (ValorPago)item;

                    if (itemAtual.IdReciboBalcao == selecionado.IdReciboBalcao)
                    {
                        dataGridValorPago.SelectedIndex = cont;

                    }

                    cont = cont + 1;
                }
            }
        }

        private void dataGridGeralValorPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selecionado = (CS_Caixa.Models.Ato)dataGridGeralValorPago.SelectedItem;

            if (selecionado != null)
            {

                int cont = 0;
                foreach (var item in dataGridValorPago.ItemsSource)
                {
                    var itemAtual = (ValorPago)item;


                    if (itemAtual.IdAto == selecionado.Id_Ato)
                    {
                        dataGridValorPago.SelectedIndex = cont;

                    }

                    cont = cont + 1;
                }
            }
        }


        private void dataGridGeralValorPago_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridGeralValorPago_GotFocus(object sender, RoutedEventArgs e)
        {
            var selecionado = (CS_Caixa.Models.Ato)dataGridGeralValorPago.SelectedItem;

            if (selecionado != null)
            {

                int cont = 0;
                foreach (var item in dataGridValorPago.ItemsSource)
                {
                    var itemAtual = (ValorPago)item;

                    if (itemAtual.IdAto == selecionado.Id_Ato)
                    {
                        dataGridValorPago.SelectedIndex = cont;

                    }

                    cont = cont + 1;
                }
            }
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (_usuario.Master == true || _usuario.Caixa == true)
            {
                if (dataGridAtosValorPago.SelectedItem != null)
                {
                    var atoSelecionado = (AtosValorPago)dataGridAtosValorPago.SelectedItem;

                    switch (atoSelecionado.TipoPagamento)
                    {
                        case "DINHEIRO":
                            txtValorPagoDinheiro.IsEnabled = true;
                            break;

                        case "DEPÓSITO":
                            txtValorPagoDeposito.IsEnabled = true;
                            break;

                        case "CHEQUE":
                            txtValorPagoCheque.IsEnabled = true;
                            break;

                        case "CHEQUE-PRÉ":
                            txtValorPagoChequePre.IsEnabled = true;
                            break;

                        case "BOLETO":
                            txtValorPagoBoleto.IsEnabled = true;
                            break;
                        default:
                            break;
                    }
                    btnSalvarPagamento.IsEnabled = true;
                    btnCancelarPagamento.IsEnabled = true;
                    dpDataValorPago.IsEnabled = false;
                    dataGridAtosValorPago.IsEnabled = false;
                }


            }
            else
            {
                MessageBox.Show("Somente usuários Master ou Caixa podem alterar Valores Pagos.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        AtosValorPago ultimoAtoSelecionado;

        private void btnSalvarPagamento_Click(object sender, RoutedEventArgs e)
        {
            pagamentosSeleciodo = pagamentosSeleciodo.OrderBy(p => p.Troco).ToList();

            ultimoAtoSelecionado = atoSelecionado;

            ValorPago valorAlterar;

            var valorPago = Convert.ToDecimal(txtValorPagoDinheiro.Text) + Convert.ToDecimal(txtValorPagoDeposito.Text) + Convert.ToDecimal(txtValorPagoCheque.Text) + Convert.ToDecimal(txtValorPagoChequePre.Text) + Convert.ToDecimal(txtValorPagoBoleto.Text);

            var total = Convert.ToDecimal(lblTotalPagamento.Content);

            if (valorPago < Convert.ToDecimal(lblTotalPagamento.Content))
            {
                MessageBox.Show("O total deve ser menor que o valor pago, favor verifique.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string tipo = string.Empty;

            for (int i = 0; i < pagamentosSeleciodo.Count; i++)
            {

                if (pagamentosSeleciodo[i].IdAto > 0)
                {
                    valorAlterar = classAto.ObterValorPagoPorIdAto(pagamentosSeleciodo[i].IdAto);
                    tipo = "IdAto";
                }
                else
                {
                    valorAlterar = classAto.ObterValorPagoPorIdReciboBalcao(pagamentosSeleciodo[i].IdReciboBalcao);
                    tipo = "IdReciboBalcao";
                }

                valorAlterar.Dinheiro = 0M;
                valorAlterar.Deposito = 0M;
                valorAlterar.Cheque = 0M;
                valorAlterar.ChequePre = 0M;
                valorAlterar.Boleto = 0M;
                valorAlterar.Total = 0M;
                valorAlterar.Troco = 0M;
                valorAlterar.DataModificado = DateTime.Now.ToShortDateString();
                valorAlterar.HoraModificado = DateTime.Now.ToLongTimeString();
                valorAlterar.IdUsuario = _usuario.Id_Usuario;
                valorAlterar.NomeUsuario = _usuario.NomeUsu;

                classAto.SalvarValorPago(valorAlterar, "alterar", tipo);

                decimal valorAto;



                if (pagamentosSeleciodo[i].IdAto > 0)
                {
                    valorAlterar = classAto.ObterValorPagoPorIdAto(pagamentosSeleciodo[i].IdAto);
                    tipo = "IdAto";

                    var ato = atosAtos.Where(p => p.Id_Ato == pagamentosSeleciodo[i].IdAto).FirstOrDefault();
                    valorAto = Convert.ToDecimal(ato.Total);
                }
                else
                {
                    valorAlterar = classAto.ObterValorPagoPorIdReciboBalcao(pagamentosSeleciodo[i].IdReciboBalcao);
                    tipo = "IdReciboBalcao";

                    var recBalc = atosBalcao.Where(p => p.IdReciboBalcao == pagamentosSeleciodo[i].IdReciboBalcao).FirstOrDefault();
                    valorAto = Convert.ToDecimal(recBalc.Total);
                }


                if (txtValorPagoDinheiro.Text != "0,00")
                    valorAlterar.Dinheiro = valorAto;
                else
                    valorAlterar.Dinheiro = Convert.ToDecimal(txtValorPagoDinheiro.Text);

                if (txtValorPagoDeposito.Text != "0,00")
                    valorAlterar.Deposito = valorAto;
                else
                    valorAlterar.Deposito = Convert.ToDecimal(txtValorPagoDeposito.Text);

                if (txtValorPagoCheque.Text != "0,00")
                    valorAlterar.Cheque = valorAto;
                else
                    valorAlterar.Cheque = Convert.ToDecimal(txtValorPagoCheque.Text);

                if (txtValorPagoChequePre.Text != "0,00")
                    valorAlterar.ChequePre = valorAto;
                else
                    valorAlterar.ChequePre = Convert.ToDecimal(txtValorPagoChequePre.Text);

                if (txtValorPagoBoleto.Text != "0,00")
                    valorAlterar.Boleto = valorAto;
                else
                    valorAlterar.Boleto = Convert.ToDecimal(txtValorPagoBoleto.Text);

                if (i == pagamentosSeleciodo.Count - 1)
                {
                    valorAlterar.Troco = Convert.ToDecimal(lblTrocoPagamento.Content);

                    if (txtValorPagoDinheiro.Text != "0,00")
                        valorAlterar.Dinheiro = valorAto + valorAlterar.Troco;

                    if (txtValorPagoDeposito.Text != "0,00")
                        valorAlterar.Deposito = valorAto + valorAlterar.Troco;

                    if (txtValorPagoCheque.Text != "0,00")
                        valorAlterar.Cheque = valorAto + valorAlterar.Troco;

                    if (txtValorPagoChequePre.Text != "0,00")
                        valorAlterar.ChequePre = valorAto + valorAlterar.Troco;

                    if (txtValorPagoBoleto.Text != "0,00")
                        valorAlterar.Boleto = valorAto + valorAlterar.Troco;
                }
                else
                    valorAlterar.Troco = 0M;

                classAto.SalvarValorPago(valorAlterar, "alterar", tipo);

            }

            if (dpDataValorPago.SelectedDate != null)
                ConsultaData(dpDataValorPago.SelectedDate.Value);
            else
            {
                dpDataValorPago.SelectedDate = DateTime.Now.Date;
                ConsultaData(dpDataValorPago.SelectedDate.Value);
            }

            var selct = atosValorPago.Where(p => p.IdReciboBalcao == ultimoAtoSelecionado.IdReciboBalcao && p.IdAto == ultimoAtoSelecionado.IdAto).FirstOrDefault();

            dataGridAtosValorPago.SelectedItem = selct;
            dataGridAtosValorPago.ScrollIntoView(selct);

            btnSalvarPagamento.IsEnabled = false;
            txtValorPagoDinheiro.IsEnabled = false;
            txtValorPagoDeposito.IsEnabled = false;
            txtValorPagoCheque.IsEnabled = false;
            txtValorPagoChequePre.IsEnabled = false;
            txtValorPagoBoleto.IsEnabled = false;
            dataGridAtosValorPago.IsEnabled = true;

            dpDataValorPago.IsEnabled = true;
        }

        private void btnCancelarPagamento_Click(object sender, RoutedEventArgs e)
        {
            SelecionarAtosPagamento();
            btnSalvarPagamento.IsEnabled = false;
            btnCancelarPagamento.IsEnabled = false;
            txtValorPagoDinheiro.IsEnabled = false;
            txtValorPagoDeposito.IsEnabled = false;
            txtValorPagoCheque.IsEnabled = false;
            txtValorPagoChequePre.IsEnabled = false;
            txtValorPagoBoleto.IsEnabled = false;
            dataGridAtosValorPago.IsEnabled = true;

            dpDataValorPago.IsEnabled = true;
        }
    }
}
