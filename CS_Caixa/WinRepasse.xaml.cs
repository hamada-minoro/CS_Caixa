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
    /// Lógica interna para WinRepasse.xaml
    /// </summary>
    public partial class WinRepasse : Window
    {
        ClassRepasseCaixa classRepasseCaixa;
        DateTime _dataSistema;
        List<RepasseCaixa> listaGrid = new List<RepasseCaixa>();
        RepasseCaixa registroSeleciondo = new RepasseCaixa();
        string status;
        bool calcular = false;
        public WinRepasse(DateTime dataSistema)
        {
            _dataSistema = dataSistema;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            classRepasseCaixa = new ClassRepasseCaixa();
            IniciaForm();
        }

        private void IniciaForm()
        {
            calcular = false;
            datePickerDataPgRestante.SelectedDate = null;
            datePickerData.SelectedDate = null;
            txtValorRepasse.Text = "0,00";
            txtValorRestante.Text = "0,00";
            txtValorDinheiroCaixa.Text = "0,00";
            txtDescricao.Text = "";
            groupBoxConsulta.IsEnabled = true;
            btnNovo.IsEnabled = true;
            btnSalvar.IsEnabled = false;
            GridAdicionar.IsEnabled = false;
            listaGrid = classRepasseCaixa.ObterTodosPorPeriodo(_dataSistema.Date.AddDays(-14), _dataSistema.Date).OrderByDescending(p => p.DataCaixa).ToList();
            dataGrid1.ItemsSource = listaGrid;
            if (dataGrid1.Items.Count > 0)
            {
                dataGrid1.IsEnabled = true;
                dataGrid1.SelectedIndex = 0;
            }
            else
                dataGrid1.IsEnabled = false;

        }

        private void ProcExcluir()
        {
            if (registroSeleciondo != null)
            {
                if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    calcular = false;
                    classRepasseCaixa.ExcluirRegistro(registroSeleciondo);
                    listaGrid.Remove(registroSeleciondo);
                    dataGrid1.ItemsSource = listaGrid;
                    dataGrid1.Items.Refresh();
                    if (dataGrid1.Items.Count > 0)
                        dataGrid1.IsEnabled = true;
                    else
                        dataGrid1.IsEnabled = false;
                }
            }
        }



        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {

            if (registroSeleciondo != null)
            {
                status = "alterar";
                calcular = true;

                GridAdicionar.IsEnabled = true;
                btnNovo.IsEnabled = false;
                btnSalvar.IsEnabled = true;
                groupBoxConsulta.IsEnabled = false;
                dataGrid1.IsEnabled = false;
            }
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

        private void datePickerDataInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate > DateTime.Now.Date)
            {
                datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            }

            datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;

            if (datePickerDataInicio.SelectedDate > datePickerDataFim.SelectedDate)
            {
                datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;
            }
        }

        private void datePickerDataFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate != null)
            {
                if (datePickerDataInicio.SelectedDate > datePickerDataFim.SelectedDate)
                {
                    datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate != null && datePickerDataFim.SelectedDate != null)
            {
                listaGrid = classRepasseCaixa.ObterTodosPorPeriodo(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value).OrderByDescending(p => p.DataCaixa).ToList(); ;
                dataGrid1.ItemsSource = listaGrid;
                dataGrid1.Items.Refresh();


                if (dataGrid1.Items.Count > 0)
                {
                    dataGrid1.IsEnabled = true;
                    dataGrid1.SelectedIndex = 0;
                }
                else
                    dataGrid1.IsEnabled = false;
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";
            calcular = true;
            GridAdicionar.IsEnabled = true;
            btnNovo.IsEnabled = false;
            btnSalvar.IsEnabled = true;
            groupBoxConsulta.IsEnabled = false;
            dataGrid1.IsEnabled = false;
            datePickerData.SelectedDate = null;
            datePickerDataPgRestante.SelectedDate = null;
            txtValorRepasse.Text = "0,00";
            txtValorRestante.Text = "0,00";
            txtValorDinheiroCaixa.Text = "0,00";
            txtDescricao.Text = "";
            datePickerData.Focus();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerData.SelectedDate == null)
            {
                MessageBox.Show("Informe a Data.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                datePickerData.Focus();
                return;
            }

            if (datePickerDataPgRestante.SelectedDate == null)
            {
                MessageBox.Show("Informe a Data do Pagamento Restante.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                datePickerDataPgRestante.Focus();
                return;
            }

            if (txtValorRepasse.Text == "0,00")
            {
                MessageBox.Show("Informe o Valor do Repasse.", "Valor do Repasse", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtValorRepasse.Focus();
                return;
            }



            try
            {
                RepasseCaixa SalvarItem;

                if (status == "novo")
                    SalvarItem = new RepasseCaixa();
                else
                    SalvarItem = registroSeleciondo;

                SalvarItem.DataCaixa = datePickerData.SelectedDate.Value.Date;
                SalvarItem.DataPagamentoRepasse = datePickerDataPgRestante.SelectedDate.Value.Date;
                SalvarItem.ValorCaixa = Convert.ToDecimal(txtValorDinheiroCaixa.Text);
                SalvarItem.ValorRepasse = Convert.ToDecimal(txtValorRepasse.Text);
                SalvarItem.ValorRestante = Convert.ToDecimal(txtValorRestante.Text);
                SalvarItem.Descricao = txtDescricao.Text;

                registroSeleciondo = classRepasseCaixa.SalvarRegistro(SalvarItem, status);

                dataGrid1.SelectedItem = registroSeleciondo;
                dataGrid1.ScrollIntoView(SalvarItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IniciaForm();

                if (registroSeleciondo != null)
                    CarregarDados();

                calcular = false;
            }
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            ProcExcluir();
            IniciaForm();
        }

        private void dataGrid1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ProcExcluir();
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
                registroSeleciondo = (RepasseCaixa)dataGrid1.SelectedItem;

            if (registroSeleciondo != null)
                CarregarDados();
            else
            {
                datePickerData.SelectedDate = null;
                datePickerDataPgRestante.SelectedDate = null;
                txtValorRepasse.Text = "0,00";
                txtValorRestante.Text = "0,00";
                txtValorDinheiroCaixa.Text = "0,00";
                txtDescricao.Text = "";
            }
        }

        private void CarregarDados()
        {

            datePickerData.SelectedDate = registroSeleciondo.DataCaixa;
            datePickerDataPgRestante.SelectedDate = registroSeleciondo.DataPagamentoRepasse;
            txtValorRepasse.Text = string.Format("{0:n2}", registroSeleciondo.ValorRepasse);
            txtValorRestante.Text = string.Format("{0:n2}", registroSeleciondo.ValorRestante);
            txtValorDinheiroCaixa.Text = string.Format("{0:n2}", registroSeleciondo.ValorCaixa);
            txtDescricao.Text = registroSeleciondo.Descricao;

        }

        private void datePickerData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                if (calcular == true)
                {
                    if (datePickerData.SelectedDate > _dataSistema.Date)
                        datePickerData.SelectedDate = _dataSistema.Date;

                    if (datePickerData.SelectedDate != null)
                    {
                        datePickerDataPgRestante.SelectedDate = diaUtil(datePickerData.SelectedDate.Value.AddDays(1));
                        txtValorRepasse.Text = "0,00";
                        txtValorDinheiroCaixa.Text = string.Format("{0:n2}", classRepasseCaixa.ObterValorDinheiroCaixaPorDataInicioDataFim(datePickerData.SelectedDate.Value, datePickerData.SelectedDate.Value));


                        if (datePickerDataPgRestante.SelectedDate != null && datePickerData.SelectedDate != null && txtValorRestante.Text != "")
                        {
                            CalcularValores();
                            CarregarDescricao();
                        }
                    }
                }
            }
        }

        private void CarregarDescricao()
        {
            if(calcular == true)
            txtDescricao.Text = string.Format("Data Repasse: {0} -> Valor: {1} / Data Restante: {2} -> Valor: {3}", datePickerData.SelectedDate.Value.ToShortDateString(), txtValorRepasse.Text, datePickerDataPgRestante.SelectedDate.Value.ToShortDateString(), txtValorRestante.Text);
        }

        public DateTime diaUtil(DateTime dt)
        {
            while (true)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    dt = dt.AddDays(2);
                    return diaUtil(dt);
                }
                else if (dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    dt = dt.AddDays(1);
                    return diaUtil(dt);
                }

                else return dt;
            }
        }


        private void txtValorRepasse_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            if (txtValorRepasse.Text.Length > 0)
            {
                if (txtValorRepasse.Text.Contains(","))
                {
                    int index = txtValorRepasse.Text.IndexOf(",");

                    if (txtValorRepasse.Text.Length == index + 3)
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

        private void CalcularValores()
        {
            if(calcular == true)
            if (datePickerData.SelectedDate != null)
            {
                var valorCaixa = Convert.ToDecimal(txtValorDinheiroCaixa.Text);
                var valorRepasse = Convert.ToDecimal(txtValorRepasse.Text);
                var valorRestante = valorCaixa - valorRepasse;

                txtValorRestante.Text = string.Format("{0:n2}", valorRestante);
            }
        }

        private void txtValorRepasse_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorRepasse.Text != "")
            {
                try
                {
                    txtValorRepasse.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorRepasse.Text));
                    if (datePickerDataPgRestante.SelectedDate != null && datePickerData.SelectedDate != null && txtValorRestante.Text != "")
                    {
                        CalcularValores();
                        CarregarDescricao();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtValorRepasse.Text = "0,00";
            }
        }

        private void txtValorRepasse_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorRepasse.Text == "0,00")
            {
                txtValorRepasse.Text = "";
            }
        }

        private void datePickerDataPgRestante_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (datePickerDataPgRestante.SelectedDate != null && datePickerData.SelectedDate != null && txtValorRestante.Text != "")
            {
                CalcularValores();
                CarregarDescricao();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                if (btnSalvar.IsEnabled == true)
                {
                    calcular = false;
                    datePickerDataPgRestante.SelectedDate = null;
                    datePickerData.SelectedDate = null;
                    txtValorRepasse.Text = "0,00";
                    txtValorRestante.Text = "0,00";
                    txtValorDinheiroCaixa.Text = "0,00";
                    txtDescricao.Text = "";
                    groupBoxConsulta.IsEnabled = true;
                    btnNovo.IsEnabled = true;
                    btnSalvar.IsEnabled = false;
                    GridAdicionar.IsEnabled = false;
                    dataGrid1.IsEnabled = true;
                    CarregarDados();
                }
        }



    }
}
