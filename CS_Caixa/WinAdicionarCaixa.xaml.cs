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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAdicionarCaixa.xaml
    /// </summary>
    public partial class WinAdicionarCaixa : Window
    {
        Usuario usuarioLogado;
        ClassAdicionarCaixa classAdicionarCaixa = new ClassAdicionarCaixa();
        string status;
        Adicionar_Caixa registroSeleciondo = new Adicionar_Caixa();
        List<Adicionar_Caixa> listaGrid = new List<Adicionar_Caixa>();
        DateTime _dataSistema;
        public WinAdicionarCaixa(Usuario usuarioLogado, DateTime dataSistema)
        {
            this.usuarioLogado = usuarioLogado;
            _dataSistema = dataSistema;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerDataInicio.SelectedDate = _dataSistema.Date;
            datePickerDataFim.SelectedDate = _dataSistema.Date;

            IniciaForm();
        }


        private void IniciaForm()
        {
            datePickerData.SelectedDate = _dataSistema.Date;
            cmbAtribuicao.SelectedIndex = -1;
            cmbTipo.SelectedIndex = -1;
            txtValor.Text = "0,00";
            txtDescricao.Text = "";
            groupBoxConsulta.IsEnabled = true;
            btnNovo.IsEnabled = true;
            btnSalvar.IsEnabled = false;
            GridAdicionar.IsEnabled = false;
            listaGrid = classAdicionarCaixa.ListaAdicionarCaixaData(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
            dataGrid1.ItemsSource = listaGrid;
            if(dataGrid1.Items.Count > 0)
            dataGrid1.IsEnabled = true;
            else
                dataGrid1.IsEnabled = false;

        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            ProcExcluir();
        }

        private void ProcExcluir()
        {
            if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                classAdicionarCaixa.ExcluirRegistro(registroSeleciondo);
                listaGrid.Remove(registroSeleciondo);
                dataGrid1.ItemsSource = listaGrid;
                dataGrid1.Items.Refresh();
                if (dataGrid1.Items.Count > 0)
                    dataGrid1.IsEnabled = true;
                else
                    dataGrid1.IsEnabled = false;
            }
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            status = "alterar";
            datePickerData.SelectedDate = registroSeleciondo.Data;
            cmbAtribuicao.Text = registroSeleciondo.Atribuicao;
            cmbTipo.Text = registroSeleciondo.TpPagamento;
            txtValor.Text =string.Format("{0:n2}", registroSeleciondo.Valor);
            txtDescricao.Text = registroSeleciondo.Descricao;
            GridAdicionar.IsEnabled = true;
            btnNovo.IsEnabled = false;
            btnSalvar.IsEnabled = true;
            groupBoxConsulta.IsEnabled = false;
            dataGrid1.IsEnabled = false;
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";
            GridAdicionar.IsEnabled = true;
            btnNovo.IsEnabled = false;
            btnSalvar.IsEnabled = true;
            groupBoxConsulta.IsEnabled = false;
            dataGrid1.IsEnabled = false;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {

            if (datePickerData.SelectedDate.Value != _dataSistema.Date)
            {
                if (usuarioLogado.Master == false)
                {
                    MessageBox.Show("Data diferente da data do sistema. Nesse caso, somente usuários Master podem adicionar esse registro.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }



            if (datePickerData.SelectedDate == null)
            {
                MessageBox.Show("Informe a data.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                datePickerData.Focus();
                return;
            }
            if (cmbAtribuicao.SelectedIndex == -1)
            {
                MessageBox.Show("Informe a Atribuição.", "Atribuição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cmbAtribuicao.Focus();
                return;
            }
            if (cmbTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Informe o Tipo.", "Tipo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                cmbTipo.Focus();
                return;
            }
            if (txtValor.Text == "0,00")
            {
                MessageBox.Show("Informe o Valor.", "Valor", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtValor.Focus();
                return;
            }

            if (txtDescricao.Text == "")
            {
                MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtDescricao.Focus();
                return;
            }


            try
            {
                Adicionar_Caixa SalvarItem = new Adicionar_Caixa();
                if (status == "novo")
                {
                    SalvarItem.Data = datePickerData.SelectedDate.Value;
                    SalvarItem.Atribuicao = cmbAtribuicao.Text;
                    SalvarItem.Descricao = txtDescricao.Text;
                    SalvarItem.TpPagamento = cmbTipo.Text;
                    SalvarItem.Valor = Convert.ToDecimal(txtValor.Text);
                    classAdicionarCaixa.SalvarRegistro(SalvarItem, status);
                }
                else
                {
                    registroSeleciondo.Data = datePickerData.SelectedDate.Value;
                    registroSeleciondo.Atribuicao = cmbAtribuicao.Text;
                    registroSeleciondo.Descricao = txtDescricao.Text;
                    registroSeleciondo.TpPagamento = cmbTipo.Text;
                    registroSeleciondo.Valor = Convert.ToDecimal(txtValor.Text);
                    classAdicionarCaixa.SalvarRegistro(registroSeleciondo, status);
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IniciaForm();
            }
           
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dataGrid1.Items.Count > 0)
            registroSeleciondo = (Adicionar_Caixa)dataGrid1.SelectedItem;
        }

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
                        e.Handled = !(key == 2 || key == 3);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
            }

        }

        private void txtValor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text == "0,00")
            {
                txtValor.Text = "";
            }
        }

        private void txtValor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text != "")
            {
                try
                {
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

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            listaGrid = classAdicionarCaixa.ListaAdicionarCaixaData(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
            dataGrid1.ItemsSource = listaGrid;
            dataGrid1.Items.Refresh();

            if (dataGrid1.Items.Count > 0)
                dataGrid1.IsEnabled = true;
            else
                dataGrid1.IsEnabled = false;

        }

        private void datePickerDataInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate.Value > _dataSistema.Date)
            {
                datePickerDataInicio.SelectedDate = _dataSistema.Date;
            }

            datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate;

            if (datePickerDataInicio.SelectedDate.Value > datePickerDataFim.SelectedDate.Value)
            {
                datePickerDataFim.SelectedDate = datePickerDataInicio.SelectedDate.Value;
            }
        }

        private void datePickerDataFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate.Value != null)
            {
                if (datePickerDataInicio.SelectedDate.Value > datePickerDataFim.SelectedDate.Value)
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

        private void dataGrid1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ProcExcluir();
            }
        }
    }
}
