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
using CS_Caixa.RelatoriosForms;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinRetiradaCaixa.xaml
    /// </summary>
    public partial class WinRetiradaCaixa : Window
    {

        Usuario usuarioLogado;
        ClassRetiradaCaixa classRetiradaCaixa = new ClassRetiradaCaixa();
        string status;
        Retirada_Caixa registroSeleciondo = new Retirada_Caixa();
        List<Retirada_Caixa> listaGrid = new List<Retirada_Caixa>();
        public WinRetiradaCaixa(Usuario usuarioLogado)
        {
            this.usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            datePickerDataFim.SelectedDate = DateTime.Now.Date;

            IniciaForm();
        }

        private void IniciaForm()
        {
            datePickerData.SelectedDate = DateTime.Now.Date;
            txtValor.Text = "0,00";
            txtDescricao.Text = "";
            groupBoxConsulta.IsEnabled = true;
            btnNovo.IsEnabled = true;
            btnSalvar.IsEnabled = false;
            GridAdicionar.IsEnabled = false;
            listaGrid = classRetiradaCaixa.ListaRetiradaCaixaData(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
            dataGrid1.ItemsSource = listaGrid;
            if (dataGrid1.Items.Count > 0)
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
                classRetiradaCaixa.ExcluirRegistro(registroSeleciondo);
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
            txtValor.Text = string.Format("{0:n2}", registroSeleciondo.Valor);
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
            if (datePickerData.SelectedDate == null)
            {
                MessageBox.Show("Informe a data.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                datePickerData.Focus();
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
                Retirada_Caixa SalvarItem = new Retirada_Caixa();
                if (status == "novo")
                {
                    SalvarItem.Data = datePickerData.SelectedDate;
                    SalvarItem.Descricao = txtDescricao.Text;
                    SalvarItem.Valor = Convert.ToDecimal(txtValor.Text);
                    
                    int numeroRecibo = classRetiradaCaixa.SalvarRegistro(SalvarItem, status);
                    string data = datePickerData.SelectedDate.Value.ToShortDateString();
                    if (MessageBox.Show("Imprimir Recibo?", "Imprimir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        FrmRecibo frmRecibo = new FrmRecibo(data, txtDescricao.Text, numeroRecibo.ToString(), txtValor.Text);
                        frmRecibo.ShowDialog();
                        frmRecibo.Dispose();
                        
                    }
                }
                else
                {
                    registroSeleciondo.Data = datePickerData.SelectedDate;
                    registroSeleciondo.Descricao = txtDescricao.Text;
                    registroSeleciondo.Valor = Convert.ToDecimal(txtValor.Text);
                    classRetiradaCaixa.SalvarRegistro(registroSeleciondo, status);
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
            if (dataGrid1.Items.Count > 0)
                registroSeleciondo = (Retirada_Caixa)dataGrid1.SelectedItem;
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
            if (datePickerDataInicio.SelectedDate != null && datePickerDataFim.SelectedDate != null)
            {
                listaGrid = classRetiradaCaixa.ListaRetiradaCaixaData(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
                dataGrid1.ItemsSource = listaGrid;
                dataGrid1.Items.Refresh();

                if (dataGrid1.Items.Count > 0)
                    dataGrid1.IsEnabled = true;
                else
                    dataGrid1.IsEnabled = false;
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

        private void MenuItemImprimirRecibo_Click(object sender, RoutedEventArgs e)
        {
            Retirada_Caixa retirada = new Retirada_Caixa();

            retirada = (Retirada_Caixa)dataGrid1.SelectedItem;

            FrmRecibo frmRecibo = new FrmRecibo(retirada.Data.Value.ToShortDateString(), retirada.Descricao, string.Format("{0}", retirada.NumeroRecibo), string.Format("{0:n2}", retirada.Valor));
            frmRecibo.ShowDialog();
            frmRecibo.Dispose();
        }

       
    }
}
