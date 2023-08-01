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
    /// Interaction logic for WinControlePagamentoCredito.xaml
    /// </summary>
    public partial class WinControlePagamento : Window
    {
        Usuario _usuarioLogado;
        public string tela;
        public WinControlePagamento(string tela, Usuario usuarioLogado)
        {
            this.tela = tela;
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        ClassControlePagamento classControlePagamento = new ClassControlePagamento();
        string status = "pronto";

        public ControlePagamentoCredito itemSeleciondo = new ControlePagamentoCredito();

        public ControlePagamentoDebito itemSeleciondoDebito = new ControlePagamentoDebito();

        Controle_Interno itemSelecionadoCI = new Controle_Interno();

        public List<ControlePagamentoCredito> listaItens = new List<ControlePagamentoCredito>();

        public List<ControlePagamentoDebito> listaItensDebito = new List<ControlePagamentoDebito>();

        public List<Controle_Interno> listaItensCI = new List<Controle_Interno>();

        ClassControleInterno controle = new ClassControleInterno();

        decimal saldoControleInterno;

        List<string> tipos = new List<string>();

        private void txtValor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValor.Text.Length > 0)
            {
                if (txtValor.Text.Contains(","))
                {
                    int index = txtValor.Text.IndexOf(",");

                    if (txtValor.Text.Length == index + 3)
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
            PassarDeUmObjetoParaOutro(sender, e);

        }

        private void txtValor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text != "")
            {
                try
                {
                    txtValor.Text = txtValor.Text.Replace("R$", "").Replace(" ", "");
                    txtValor.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValor.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtValor.Text = "0,00";
            }
        }

        private void txtValor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text == "0,00")
            {
                txtValor.Text = "";
            }
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (tela == "credito")
            {
                if (itemSeleciondo != null)
                {
                    if (MessageBox.Show("Deseja realmente excluir " + itemSeleciondo.Descricao + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (itemSeleciondo != null)
                            classControlePagamento.ExcluirCredito(itemSeleciondo);

                        listaItens.Remove(itemSeleciondo);
                        dataGrid1.ItemsSource = listaItens;
                        dataGrid1.Items.Refresh();

                        if (dataGrid1.Items.Count > 0)
                        {
                            dataGrid1.IsEnabled = true;
                            dataGrid1.SelectedIndex = 0;
                        }
                        else
                        {
                            LimpaCampos();
                            itemSeleciondo = null;
                            dataGrid1.IsEnabled = false;
                        }

                        lblTotal.Content = string.Format("Total: {0:N2}", listaItens.Sum(p => p.Valor));
                    }
                }
            }
            else
            {
                if (itemSeleciondoDebito != null)
                {
                    if (MessageBox.Show("Deseja realmente excluir " + itemSeleciondoDebito.Descricao + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (itemSeleciondoDebito != null)
                            classControlePagamento.ExcluirDebito(itemSeleciondoDebito);

                        listaItensDebito.Remove(itemSeleciondoDebito);
                        dataGrid1.ItemsSource = listaItensDebito;
                        dataGrid1.Items.Refresh();

                        if (dataGrid1.Items.Count > 0)
                        {
                            dataGrid1.IsEnabled = true;
                            dataGrid1.SelectedIndex = 0;
                        }
                        else
                        {
                            LimpaCampos();
                            itemSeleciondoDebito = null;
                            dataGrid1.IsEnabled = false;
                        }

                        lblTotal.Content = string.Format("Total: {0:N2}", listaItensDebito.Sum(p => p.Valor));
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (tela == "credito")
            {
                this.Title = "Controle de Receitas";

                groupBoxRecebimentos.Header = "Recebimentos";
                groupBoxRecebimentosci.Header = "Recebimentos";

            }
            else
            {
                this.Title = "Controle de Despesas";
                groupBoxRecebimentos.Header = "Despesas";
                groupBoxRecebimentosci.Header = "Despesas";
            }


            IniciaForm();

            Consultar();
            ConsultarControleInterno();


            if (dataGrid1.Items.Count > 0)
            {
                dataGrid1.IsEnabled = true;
                dataGrid1.SelectedIndex = 0;

            }
            else
            {
                LimpaCampos();
                itemSeleciondo = null;
                dataGrid1.IsEnabled = false;
            }

            if (dataGridci.Items.Count > 0)
            {
                dataGridci.IsEnabled = true;
                dataGridci.SelectedIndex = 0;

            }
            else
            {
                LimpaCampos();
                itemSelecionadoCI = null;
                dataGridci.IsEnabled = false;
            }

            saldoControleInterno = controle.ObterSaldoControleInterno();

            if (saldoControleInterno != -1)
            {
                txtSaldo.Text = string.Format("{0:n2}", saldoControleInterno);
            }
            else
            {
                MessageBox.Show("Cadastre o Saldo do Controle Interno.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                tabItemInterno.Focus();
                btnAlterarSaldo.IsEnabled = false;
                txtSaldo.IsEnabled = true;
                stackPanel2.IsEnabled = false;
                dataGridci.IsEnabled = false;
                tabItemCaixa.IsEnabled = false;
            }
        }

        private void Consultar()
        {
            if (tela == "credito")
            {
                listaItens = classControlePagamento.ListarCredito(datePickerDataConsulta.SelectedDate.Value, datePickerDataConsultaFim.SelectedDate.Value).OrderBy(p => p.Data).ToList();

                dataGrid1.ItemsSource = listaItens;
                lblTotal.Content = string.Format("Total: {0:N2}", listaItens.Sum(p => p.Valor));

            }
            else
            {
                listaItensDebito = classControlePagamento.ListarDebito(datePickerDataConsulta.SelectedDate.Value, datePickerDataConsultaFim.SelectedDate.Value).OrderBy(p => p.Data).ToList();

                dataGrid1.ItemsSource = listaItensDebito;

                lblTotal.Content = string.Format("Total: {0:N2}", listaItensDebito.Sum(p => p.Valor));
            }
        }

        private void ConsultarControleInterno()
        {
            if (tela == "credito")
            {
                listaItensCI = controle.ListarEntrada(datePickerDataConsultaci.SelectedDate.Value, datePickerDataConsultaFimci.SelectedDate.Value).OrderBy(p => p.Data).ToList();

                dataGridci.ItemsSource = listaItensCI;
                lblTotalci.Content = string.Format("Total: {0:N2}", listaItensCI.Sum(p => p.Valor));

            }
            else
            {
                listaItensCI = controle.ListarSaida(datePickerDataConsultaci.SelectedDate.Value, datePickerDataConsultaFimci.SelectedDate.Value).OrderBy(p => p.Data).ToList();

                dataGridci.ItemsSource = listaItensCI;

                lblTotalci.Content = string.Format("Total: {0:N2}", listaItensCI.Sum(p => p.Valor));
            }
        }

        private void IniciaForm()
        {
            try
            {
                status = "pronto";
                GridRecebimentos.IsEnabled = false;
                GridConsultar.IsEnabled = true;
                btnCancelar.IsEnabled = false;
                btnImportar.IsEnabled = true;
                btnNovo.IsEnabled = true;
                btnSalvar.IsEnabled = false;
                dataGrid1.IsEnabled = true;
                tabItemInterno.IsEnabled = true;
                tabItemCaixa.IsEnabled = true;
                ckbCaixa.IsChecked = false;
                gridSaldo.IsEnabled = true;

                if (dataGrid1.Items.Count > 0)
                {
                    dataGrid1.IsEnabled = true;
                    dataGrid1.SelectedIndex = 0;

                }
                else
                {
                    LimpaCampos();
                    itemSeleciondo = null;
                    dataGrid1.IsEnabled = false;
                }


                GridRecebimentosci.IsEnabled = false;
                GridConsultarci.IsEnabled = true;
                btnCancelarci.IsEnabled = false;
                btnNovoci.IsEnabled = true;
                btnSalvarci.IsEnabled = false;
                dataGridci.IsEnabled = true;
                UltimoDiaUtilDataConsulta();

                if (dataGridci.Items.Count > 0)
                {
                    dataGridci.IsEnabled = true;
                    dataGridci.SelectedIndex = 0;

                }
                else
                {
                    LimpaCamposCI();
                    itemSeleciondo = null;
                    dataGridci.IsEnabled = false;
                }

                tipos = controle.ObterTiposControleInterno();
                cmbTipoci.ItemsSource = tipos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            status = "alterar";
            GridRecebimentos.IsEnabled = true;
            GridConsultar.IsEnabled = false;
            btnCancelar.IsEnabled = true;
            btnImportar.IsEnabled = false;
            btnNovo.IsEnabled = false;
            btnSalvar.IsEnabled = true;
            dataGrid1.IsEnabled = false;
            tabItemInterno.IsEnabled = false;
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";
            GridRecebimentos.IsEnabled = true;
            GridConsultar.IsEnabled = false;
            btnCancelar.IsEnabled = true;
            btnImportar.IsEnabled = false;
            btnNovo.IsEnabled = false;
            btnSalvar.IsEnabled = true;
            dataGrid1.IsEnabled = false;
            tabItemInterno.IsEnabled = false;
            LimpaCampos();
            txtDescricao.Focus();
        }

        public void LimpaCampos()
        {
            //UltimoDiaUtilData();

            datePickerData.SelectedDate = DateTime.Now.Date;

            txtDescricao.Text = "";

            txtOrigem.Text = "";

            cmbTipo.SelectedIndex = 0;

            txtValor.Text = "0,00";



        }


        private void UltimoDiaUtilData()
        {
            DateTime data = new DateTime();
            data = DateTime.Now.Date;

            data = data.AddDays(-1);

            if (data.DayOfWeek == DayOfWeek.Sunday)
            {
                data = data.AddDays(-2);
            }

            if (data.DayOfWeek == DayOfWeek.Saturday)
            {
                data = data.AddDays(-1);
            }

            datePickerData.SelectedDate = data;
        }


        private void UltimoDiaUtilDataConsulta()
        {
            DateTime data = new DateTime();
            data = DateTime.Now.Date;

            data = data.AddDays(-1);

            if (data.DayOfWeek == DayOfWeek.Sunday)
            {
                data = data.AddDays(-2);
            }

            if (data.DayOfWeek == DayOfWeek.Saturday)
            {
                data = data.AddDays(-1);
            }

            datePickerDataConsulta.SelectedDate = data;
            datePickerDataConsultaFim.SelectedDate = data;

            datePickerDataConsultaci.SelectedDate = data;
            datePickerDataConsultaFimci.SelectedDate = data;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            IniciaForm();
            PreencheCamposSelecionado();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (tela == "credito")
            {
                try
                {
                    ControlePagamentoCredito credito = new ControlePagamentoCredito();

                    if (datePickerData.SelectedDate != null)
                        credito.Data = datePickerData.SelectedDate.Value;
                    else
                    {
                        MessageBox.Show("Informe uma data válida.", "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        datePickerData.Focus();
                        return;
                    }


                    credito.Descricao = txtDescricao.Text = txtDescricao.Text.Trim();

                    if (credito.Descricao == "")
                    {
                        MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtDescricao.Focus();
                        return;
                    }

                    credito.TipoCredito = cmbTipo.Text;

                    credito.Origem = txtOrigem.Text;

                    credito.Valor = Convert.ToDecimal(txtValor.Text);

                    credito.Usuario = _usuarioLogado.NomeUsu;

                    credito.IdUsuario = _usuarioLogado.Id_Usuario;


                    if (status == "alterar")
                        credito.Id = itemSeleciondo.Id;


                    classControlePagamento.SalvarCredito(credito, status);


                    listaItens = classControlePagamento.ListarCredito(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value);

                    dataGrid1.ItemsSource = listaItens;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    ControlePagamentoDebito debito = new ControlePagamentoDebito();

                    if (datePickerData.SelectedDate != null)
                        debito.Data = datePickerData.SelectedDate.Value;
                    else
                    {
                        MessageBox.Show("Informe uma data válida.", "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        datePickerData.Focus();
                        return;
                    }


                    debito.Descricao = txtDescricao.Text = txtDescricao.Text.Trim();

                    if (debito.Descricao == "")
                    {
                        MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtDescricao.Focus();
                        return;
                    }

                    debito.TipoDebito = cmbTipo.Text;

                    debito.Origem = txtOrigem.Text;

                    debito.Valor = Convert.ToDecimal(txtValor.Text);

                    debito.Usuario = _usuarioLogado.NomeUsu;

                    debito.IdUsuario = _usuarioLogado.Id_Usuario;

                    if (status == "alterar")
                        debito.Id = itemSeleciondoDebito.Id;


                    classControlePagamento.SalvarDebito(debito, status, false);


                    listaItensDebito = classControlePagamento.ListarDebito(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value);

                    dataGrid1.ItemsSource = listaItensDebito;

                    IniciaForm();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (tela == "credito")
            {
                listaItens = classControlePagamento.ListarCredito(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value);

                dataGrid1.ItemsSource = listaItens;
                lblTotal.Content = string.Format("Total: {0:N2}", listaItens.Sum(p => p.Valor));

            }
            else
            {
                listaItensDebito = classControlePagamento.ListarDebito(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value);

                dataGrid1.ItemsSource = listaItensDebito;

                lblTotal.Content = string.Format("Total: {0:N2}", listaItensDebito.Sum(p => p.Valor));
            }

            IniciaForm();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tela == "credito")
            {
                if (dataGrid1.SelectedItem != null)
                {
                    itemSeleciondo = (ControlePagamentoCredito)dataGrid1.SelectedItem;

                    PreencheCamposSelecionado();
                }
            }
            else
            {
                if (dataGrid1.SelectedItem != null)
                {
                    itemSeleciondoDebito = (ControlePagamentoDebito)dataGrid1.SelectedItem;

                    PreencheCamposSelecionado();
                }
            }
        }

        private void PreencheCamposSelecionado()
        {
            if (tela == "credito")
            {
                if (itemSelecionadoCI != null)
                {
                    datePickerData.SelectedDate = itemSeleciondo.Data;
                    txtDescricao.Text = itemSeleciondo.Descricao;
                    txtValor.Text = string.Format("{0:n2}", itemSeleciondo.Valor);
                    cmbTipo.Text = itemSeleciondo.TipoCredito;
                    txtOrigem.Text = itemSeleciondo.Origem;
                }
            }
            else
            {
                if (itemSeleciondoDebito != null)
                {
                    datePickerData.SelectedDate = itemSeleciondoDebito.Data;
                    txtDescricao.Text = itemSeleciondoDebito.Descricao;
                    txtValor.Text = string.Format("{0:n2}", itemSeleciondoDebito.Valor);
                    cmbTipo.Text = itemSeleciondoDebito.TipoDebito;
                    txtOrigem.Text = itemSeleciondoDebito.Origem;
                }
            }
        }


        private void PreencheCamposSelecionadoCI()
        {

            if (itemSelecionadoCI != null)
            {
                datePickerDataci.SelectedDate = itemSelecionadoCI.Data;
                txtDescricaoci.Text = itemSelecionadoCI.Descricao;
                txtValorci.Text = string.Format("{0:n2}", itemSelecionadoCI.Valor);
                cmbTipoci.Text = itemSelecionadoCI.Tipo;

                if (itemSelecionadoCI.EntradaSaida == "ENTRADA")
                {
                    var ControleCaixa = classControlePagamento.ObterCredito(itemSelecionadoCI.Data, itemSelecionadoCI.Descricao, itemSelecionadoCI.Valor);
                    if (ControleCaixa != null)
                        ckbCaixa.IsChecked = true;
                    else
                        ckbCaixa.IsChecked = false;

                }
                else
                {
                    var ControleCaixa = classControlePagamento.ObterDebto(itemSelecionadoCI.Data, itemSelecionadoCI.Descricao, itemSelecionadoCI.Valor);
                    if (ControleCaixa != null)
                        ckbCaixa.IsChecked = true;
                    else
                        ckbCaixa.IsChecked = false;
                }

            }
        }


        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDataConsulta.SelectedDate != null && datePickerDataConsultaFim.SelectedDate != null)
            {
                Consultar();

                if (dataGrid1.Items.Count > 0)
                {
                    dataGrid1.IsEnabled = true;
                    dataGrid1.SelectedIndex = 0;

                }
                else
                {
                    LimpaCampos();
                    itemSeleciondo = null;
                    dataGrid1.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Informe um período válida.");
            }
        }


        private void stackPanel2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //var uie = e.OriginalSource as UIElement;

            //if (e.Key == Key.Enter)
            //{
            //    e.Handled = true;
            //    uie.MoveFocus(
            //    new TraversalRequest(
            //    FocusNavigationDirection.Next));

            //}
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            WinDataImportarCredito data = new WinDataImportarCredito(this);
            data.Owner = this;
            data.ShowDialog();

            Consultar();

        }

        private void datePickerData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (datePickerData.Text.Length == 8)
                {
                    datePickerData.Text = string.Format("{0}/{1}/{2}", datePickerData.Text.Substring(0, 2), datePickerData.Text.Substring(2, 2), datePickerData.Text.Substring(4, 4));
                }

                PassarDeUmObjetoParaOutro(sender, e);
            }
        }

        private void txtDescricao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void PassarDeUmObjetoParaOutro(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest
                    (FocusNavigationDirection.Next));

            }
        }

        private void datePickerDataConsulta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataConsulta.SelectedDate > DateTime.Now.Date)
            {
                datePickerDataConsulta.SelectedDate = DateTime.Now.Date;
            }

            datePickerDataConsultaFim.SelectedDate = datePickerDataConsulta.SelectedDate;

            if (datePickerDataConsultaFim.SelectedDate > datePickerDataConsultaFim.SelectedDate)
            {
                datePickerDataConsultaFim.SelectedDate = datePickerDataConsulta.SelectedDate;
            }
        }

        private void datePickerDataConsultaFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataConsulta.SelectedDate != null)
            {
                if (datePickerDataConsulta.SelectedDate > datePickerDataConsultaFim.SelectedDate)
                {
                    datePickerDataConsultaFim.SelectedDate = datePickerDataConsulta.SelectedDate;
                }

                if (datePickerDataConsultaFim.SelectedDate == null)
                    datePickerDataConsultaFim.SelectedDate = datePickerDataConsulta.SelectedDate;
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }



        // -------------------- Controle Interno ----------------------------------


        private void datePickerDataci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (datePickerDataci.Text.Length == 8)
                {
                    datePickerDataci.Text = string.Format("{0}/{1}/{2}", datePickerDataci.Text.Substring(0, 2), datePickerDataci.Text.Substring(2, 2), datePickerDataci.Text.Substring(4, 4));
                }

                var uie = e.OriginalSource as UIElement;

                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    uie.MoveFocus(new TraversalRequest
                        (FocusNavigationDirection.Next));

                }
            }
        }

        private void txtDescricaoci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest
                    (FocusNavigationDirection.Next));

            }
        }

        private void datePickerDataConsultaci_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataConsultaci.SelectedDate > DateTime.Now.Date)
            {
                datePickerDataConsultaci.SelectedDate = DateTime.Now.Date;
            }

            datePickerDataConsultaFimci.SelectedDate = datePickerDataConsultaci.SelectedDate;

            if (datePickerDataConsultaFimci.SelectedDate > datePickerDataConsultaFimci.SelectedDate)
            {
                datePickerDataConsultaFimci.SelectedDate = datePickerDataConsultaci.SelectedDate;
            }
        }

        private void txtValorci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValorci.Text.Length > 0)
            {
                if (txtValorci.Text.Contains(","))
                {
                    int index = txtValorci.Text.IndexOf(",");

                    if (txtValorci.Text.Length == index + 3)
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

            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void txtValorci_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorci.Text != "")
            {
                try
                {
                    txtValorci.Text = txtValorci.Text.Replace("R$", "").Replace(" ", "");
                    txtValorci.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorci.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtValorci.Text = "0,00";
            }
        }

        private void txtValorci_GotFocus(object sender, RoutedEventArgs e)
        {

            if (txtValorci.Text == "0,00")
            {
                txtValorci.Text = "";
            }

        }

        private void btnConsultaci_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDataConsultaci.SelectedDate != null && datePickerDataConsultaFimci.SelectedDate != null)
            {
                ConsultarControleInterno();

                if (dataGridci.Items.Count > 0)
                {
                    dataGridci.IsEnabled = true;
                    dataGridci.SelectedIndex = 0;

                }
                else
                {
                    LimpaCampos();
                    itemSelecionadoCI = null;
                    dataGridci.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Informe um período válida.");
            }
        }

        private void datePickerDataConsultaFimci_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataConsultaci.SelectedDate != null)
            {
                if (datePickerDataConsultaci.SelectedDate > datePickerDataConsultaFimci.SelectedDate)
                {
                    datePickerDataConsultaFimci.SelectedDate = datePickerDataConsultaci.SelectedDate;
                }

                if (datePickerDataConsultaFimci.SelectedDate == null)
                    datePickerDataConsultaFimci.SelectedDate = datePickerDataConsultaci.SelectedDate;
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void btnCancelarci_Click(object sender, RoutedEventArgs e)
        {
            IniciaForm();
            PreencheCamposSelecionadoCI();
        }

        private void btnNovoci_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";
            GridRecebimentosci.IsEnabled = true;
            GridConsultarci.IsEnabled = false;
            btnCancelarci.IsEnabled = true;
            btnNovoci.IsEnabled = false;
            btnSalvarci.IsEnabled = true;
            dataGridci.IsEnabled = false;
            tabItemCaixa.IsEnabled = false;
            gridSaldo.IsEnabled = false;
            ckbCaixa.IsChecked = false;
            LimpaCamposCI();
            txtDescricaoci.Focus();
        }

        private void btnSalvarci_Click(object sender, RoutedEventArgs e)
        {

            Controle_Interno controleInterno;
            
            try
            {
                decimal valor = 0;

                if (itemSelecionadoCI != null)
                valor = itemSelecionadoCI.Valor;


                if (status == "novo")
                    controleInterno = new Controle_Interno();
                else
                {
                    controleInterno = itemSelecionadoCI;            
                }
                if (datePickerDataci.SelectedDate != null)
                    controleInterno.Data = datePickerDataci.SelectedDate.Value;
                else
                {
                    MessageBox.Show("Informe uma data válida.", "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    datePickerData.Focus();
                    return;
                }
                controleInterno.Descricao = txtDescricaoci.Text = txtDescricaoci.Text.Trim();

                if (controleInterno.Descricao == "")
                {
                    MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtDescricaoci.Focus();
                    return;
                }

                controleInterno.Tipo = cmbTipoci.Text;

                controleInterno.IdUsuario = _usuarioLogado.Id_Usuario;

                controleInterno.Usuario = _usuarioLogado.NomeUsu;

                controleInterno.Valor = Convert.ToDecimal(txtValorci.Text);

                if (tela == "credito")
                    controleInterno.EntradaSaida = "ENTRADA";
                else
                    controleInterno.EntradaSaida = "SAÍDA";

                if (status != "novo")
                {
                    
                    if (itemSelecionadoCI != null)
                        controle.Excluir(itemSelecionadoCI);

                    if (tela == "credito")
                        txtSaldo.Text = string.Format("{0:n2}", controle.CalcularSaldo(valor, false));
                    else
                        txtSaldo.Text = string.Format("{0:n2}", controle.CalcularSaldo(valor, true));
                }

                controle.SalvarCredito(controleInterno, "novo");

                if (tela == "credito")
                    txtSaldo.Text = string.Format("{0:n2}", controle.CalcularSaldo(controleInterno.Valor, true));
                else
                    txtSaldo.Text = string.Format("{0:n2}", controle.CalcularSaldo(controleInterno.Valor, false));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            if (status == "novo")
                if (ckbCaixa.IsChecked == true)
                {

                    if (tela == "credito")
                    {
                        try
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();

                            credito.Data = controleInterno.Data;


                            credito.Descricao = controleInterno.Descricao;

                            if (credito.Descricao == "")
                            {
                                MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                txtDescricao.Focus();
                                return;
                            }

                            credito.TipoCredito = "DINHEIRO";

                            credito.Origem = controleInterno.Tipo;

                            credito.Valor = controleInterno.Valor;

                            credito.IdUsuario = controleInterno.IdUsuario;

                            credito.Usuario = controleInterno.Usuario;

                            classControlePagamento.SalvarCredito(credito, "novo");

                            listaItens = classControlePagamento.ListarCredito(controleInterno.Data, controleInterno.Data);

                            dataGrid1.ItemsSource = listaItens;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            ControlePagamentoDebito debito = new ControlePagamentoDebito();

                            debito.Data = controleInterno.Data;

                            debito.Descricao = controleInterno.Descricao;

                            debito.TipoDebito = "DINHEIRO";

                            debito.Origem = controleInterno.Tipo;

                            debito.Valor = controleInterno.Valor;

                            debito.IdUsuario = controleInterno.IdUsuario;

                            debito.Usuario = controleInterno.Usuario;

                            classControlePagamento.SalvarDebito(debito, "novo", false);

                            listaItensDebito = classControlePagamento.ListarDebito(controleInterno.Data, controleInterno.Data);

                            dataGrid1.ItemsSource = listaItensDebito;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

            if (tela == "credito")
            {
                if (datePickerData.SelectedDate != null)
                {
                    listaItens = classControlePagamento.ListarCredito(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value);

                    dataGrid1.ItemsSource = listaItens;
                    lblTotal.Content = string.Format("Total: {0:N2}", listaItens.Sum(p => p.Valor));
                }

                if (datePickerDataci.SelectedDate != null)
                {
                    listaItensCI = controle.ListarEntrada(datePickerDataci.SelectedDate.Value, datePickerDataci.SelectedDate.Value);

                    dataGridci.ItemsSource = listaItensCI;

                    lblTotalci.Content = string.Format("Total: {0:N2}", listaItensCI.Sum(p => p.Valor));
                }
            }
            else
            {
                if (datePickerData.SelectedDate != null)
                {
                    listaItensDebito = classControlePagamento.ListarDebito(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value);

                    dataGrid1.ItemsSource = listaItensDebito;

                    lblTotal.Content = string.Format("Total: {0:N2}", listaItensDebito.Sum(p => p.Valor));
                }
                if (datePickerDataci.SelectedDate != null)
                {
                    listaItensCI = controle.ListarSaida(datePickerDataci.SelectedDate.Value, datePickerDataci.SelectedDate.Value);

                    dataGridci.ItemsSource = listaItensCI;

                    lblTotalci.Content = string.Format("Total: {0:N2}", listaItensCI.Sum(p => p.Valor));
                }
            }

            IniciaForm();
        }

        private void dataGridci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridci.SelectedItem != null)
            {
                itemSelecionadoCI = (Controle_Interno)dataGridci.SelectedItem;

                PreencheCamposSelecionadoCI();
            }
        }

        private void ProcExcluirCI()
        {
            if (itemSelecionadoCI != null)
                controle.Excluir(itemSelecionadoCI);

            lblTotalci.Content = string.Format("Total: {0:N2}", listaItensCI.Sum(p => p.Valor));

            if (tela == "credito")
                txtSaldo.Text = string.Format("{0:n2}", controle.CalcularSaldo(itemSelecionadoCI.Valor, false));
            else
                txtSaldo.Text = string.Format("{0:n2}", controle.CalcularSaldo(itemSelecionadoCI.Valor, true));

            listaItensCI.Remove(itemSelecionadoCI);
            dataGridci.ItemsSource = listaItensCI;
            dataGridci.Items.Refresh();

            if (dataGridci.Items.Count > 0)
            {
                dataGridci.IsEnabled = true;
                dataGridci.SelectedIndex = 0;
            }
            else
            {
                LimpaCamposCI();
                itemSelecionadoCI = null;
                dataGridci.IsEnabled = false;
            }

            lblTotalci.Content = string.Format("Total: {0:N2}", listaItensCI.Sum(p => p.Valor));
        }

        private void MenuItemExcluirci_Click(object sender, RoutedEventArgs e)
        {
            if (itemSelecionadoCI != null)
            {
                if (MessageBox.Show("Deseja realmente excluir " + itemSelecionadoCI.Descricao + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ProcExcluirCI();
                }
            }
        }

        private void MenuItemAlterarci_Click(object sender, RoutedEventArgs e)
        {
            status = "alterar";
            GridRecebimentosci.IsEnabled = true;
            GridConsultarci.IsEnabled = false;
            btnCancelarci.IsEnabled = true;
            btnNovoci.IsEnabled = false;
            btnSalvarci.IsEnabled = true;
            dataGridci.IsEnabled = false;
            tabItemCaixa.IsEnabled = false;
            gridSaldo.IsEnabled = false;
        }

        private void cmbTipoci_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbTipoci.Text != "")
            {
                cmbTipoci.Text = cmbTipoci.Text.ToUpper();
            }
        }

        private void cmbTipo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void txtOrigem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void cmbTipoci_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }

        public void LimpaCamposCI()
        {

            datePickerDataci.SelectedDate = DateTime.Now.Date;

            txtDescricaoci.Text = "";

            cmbTipoci.Text = "";

            txtValorci.Text = "0,00";

        }

        private void txtSaldo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSaldo.Text != "")
                txtSaldo.Text = "";
        }

        private void txtSaldo_LostFocus(object sender, RoutedEventArgs e)
        {
            SalvaSaldo();

            stackPanel2.IsEnabled = true;
            if (dataGridci.Items.Count > 0)
                dataGridci.IsEnabled = true;
            tabItemCaixa.IsEnabled = true;
        }

        private void SalvaSaldo()
        {
            if (txtSaldo.Text != "")
            {
                try
                {
                    decimal valor = Convert.ToDecimal(txtSaldo.Text);
                    txtSaldo.Text = string.Format("{0:n2}", valor);
                    controle.SalvarSaldoControle(valor);
                    txtSaldo.IsEnabled = false;
                    btnAlterarSaldo.IsEnabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtSaldo.Text = "0,00";
                controle.SalvarSaldoControle(0M);
                txtSaldo.IsEnabled = false;
                btnAlterarSaldo.IsEnabled = true;
            }
        }

        private void txtSaldo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtSaldo.Text.Length > 0)
            {
                if (txtSaldo.Text.Contains(","))
                {
                    int index = txtSaldo.Text.IndexOf(",");

                    if (txtSaldo.Text.Length == index + 3)
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

            if (e.Key == Key.Enter)
                SalvaSaldo();

        }

        private void btnAlterarSaldo_Click(object sender, RoutedEventArgs e)
        {
            txtSaldo.IsEnabled = true;
            txtSaldo.Focus();
            txtSaldo.Background = Brushes.White;
            btnAlterarSaldo.IsEnabled = false;
            stackPanel2.IsEnabled = false;
            dataGridci.IsEnabled = false;
            tabItemCaixa.IsEnabled = false;
        }



        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //lblCodigo.Content = string.Format("Código: {0}", Convert.ToInt32(e.Key));
        }


    }
}
