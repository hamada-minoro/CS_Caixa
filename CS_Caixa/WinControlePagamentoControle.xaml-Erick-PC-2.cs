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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinControlePagamentoControle.xaml
    /// </summary>
    public partial class WinControlePagamentoControle : Window
    {
        ClassControlePagamento classControlePagamento = new ClassControlePagamento();

        List<ControlePagamentoCredito> listaCredito = new List<ControlePagamentoCredito>();

        List<ControlePagamentoDebito> listaDebito = new List<ControlePagamentoDebito>();

        public ControlePagamentoCredito itemSelecionadoCredito = new ControlePagamentoCredito();

        public ControlePagamentoDebito itemSelecionadoDebito = new ControlePagamentoDebito();


        decimal totalCredito = 0;

        decimal totalDebito = 0;

        decimal diferenca = 0;



        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


        public WinControlePagamentoControle()
        {
            InitializeComponent();
        }

        private void datePickerInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicio.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicio.SelectedDate = DateTime.Now.Date;
            }

            datePickerFim.SelectedDate = datePickerInicio.SelectedDate;

            if (datePickerInicio.SelectedDate > datePickerFim.SelectedDate)
            {
                datePickerFim.SelectedDate = datePickerInicio.SelectedDate;
            }
        }

        private void datePickerFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicio.SelectedDate != null)
            {
                if (datePickerInicio.SelectedDate > datePickerFim.SelectedDate)
                {
                    datePickerFim.SelectedDate = datePickerInicio.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerInicio.SelectedDate = DateTime.Now.Date;
            datePickerFim.SelectedDate = DateTime.Now.Date;


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();

            ProcConsulta();
        }


        bool ativo = true;
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (diferenca < 0)
            {

                if (ativo == true)
                {
                    lblDiferenca.Foreground = new SolidColorBrush(Colors.Red);
                     ativo = false;
                }
                else
                {
                    lblDiferenca.Foreground = new SolidColorBrush(Colors.Transparent);
                    ativo = true;
                }
            }
            

        }




        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ProcConsulta();
        }


       
        private void ProcConsulta()
        {
            if (datePickerInicio.SelectedDate != null && datePickerInicio.SelectedDate != null)
            {
                listaCredito = classControlePagamento.ListarCredito(datePickerInicio.SelectedDate.Value, datePickerFim.SelectedDate.Value);
                listaDebito = classControlePagamento.ListarDebito(datePickerInicio.SelectedDate.Value, datePickerFim.SelectedDate.Value);

                CalcularValores();


            }
        }

        private void CalcularValores()
        {
           

            dataGridCredito.ItemsSource = listaCredito.OrderBy(p => p.Data);

            dataGridDebito.ItemsSource = listaDebito.OrderBy(p => p.Data);

            totalCredito = listaCredito.Sum(p => p.Valor);

            totalDebito = listaDebito.Sum(p => p.Valor);

            lblTotalCredito.Content = string.Format("Total de Receitas: {0:n2}", totalCredito);

            lblTotalDebito.Content = string.Format("Total de Despesas: {0:n2}", totalDebito);

            diferenca = totalCredito - totalDebito;


            lblDiferenca.Content = string.Format("Saldo: {0:n2}", diferenca);

            if (diferenca < 0)
            {
                lblDiferenca.Foreground = new SolidColorBrush(Colors.Red);
                dispatcherTimer.Start();

            }
            else
            {
                dispatcherTimer.Stop();
                lblDiferenca.Foreground = new SolidColorBrush(Colors.Blue);
            }

            if (listaCredito.Count > 0 || listaDebito.Count > 0)
            {
                btnImprimir.IsEnabled = true;
            }
            else
            {
                btnImprimir.IsEnabled = false;
            }

            if (dataGridCredito.Items.Count > 0)
            {
                dataGridCredito.IsEnabled = true;
                dataGridCredito.SelectedIndex = 0;
            }
            else
            {
                dataGridCredito.IsEnabled = false;
            }

            if (dataGridDebito.Items.Count > 0)
            {
                dataGridDebito.IsEnabled = true;
                dataGridDebito.SelectedIndex = 0;
            }
            else
            {
                dataGridDebito.IsEnabled = false;
            }
        }


        private void MenuItemAlterarCredito_Click(object sender, RoutedEventArgs e)
        {
            if (itemSelecionadoCredito != null)
            {
                if (dataGridCredito.SelectedIndex >= 0)
                {
                    WinAlterarControlePagamento alterar = new WinAlterarControlePagamento(this, itemSelecionadoCredito, "credito");
                    alterar.Owner = this;
                    alterar.ShowDialog();

                    listaCredito[dataGridCredito.SelectedIndex] = itemSelecionadoCredito;



                    if (dataGridCredito.SelectedIndex != -1)
                        dataGridCredito.ItemsSource = listaCredito;

                    dataGridCredito.Items.Refresh();

                    CalcularValores();
                }
                else
                {
                    MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MenuItemExcluirCredito_Click(object sender, RoutedEventArgs e)
        {
            if (itemSelecionadoCredito != null)
            {
                if (dataGridCredito.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Deseja realmente excluir " + itemSelecionadoCredito.Descricao + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (itemSelecionadoCredito != null)
                            classControlePagamento.ExcluirCredito(itemSelecionadoCredito);

                        listaCredito.Remove(itemSelecionadoCredito);
                        dataGridCredito.ItemsSource = listaCredito;
                        dataGridCredito.Items.Refresh();

                    }

                    CalcularValores();
                }
                else
                {
                    MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void menuItemExcluirDebito_Click(object sender, RoutedEventArgs e)
        {
            if (itemSelecionadoDebito != null)
            {
                if (dataGridDebito.SelectedIndex >= 0)
                {
                    if (MessageBox.Show("Deseja realmente excluir " + itemSelecionadoDebito.Descricao + "?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (itemSelecionadoDebito != null)
                            classControlePagamento.ExcluirDebito(itemSelecionadoDebito);

                        listaDebito.Remove(itemSelecionadoDebito);
                        dataGridDebito.ItemsSource = listaDebito;
                        dataGridDebito.Items.Refresh();

                    }

                    CalcularValores();
                }
                else
                {
                    MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void menuItemAlterarDebito_Click(object sender, RoutedEventArgs e)
        {
            if (itemSelecionadoDebito != null)
            {
                if (dataGridDebito.SelectedIndex >= 0)
                {
                    WinAlterarControlePagamento alterar = new WinAlterarControlePagamento(this, itemSelecionadoDebito, "debito");
                    alterar.Owner = this;
                    alterar.ShowDialog();

                    listaDebito[dataGridDebito.SelectedIndex] = itemSelecionadoDebito;



                    if (dataGridDebito.SelectedIndex != -1)
                        dataGridDebito.ItemsSource = listaDebito;

                    dataGridDebito.Items.Refresh();

                    CalcularValores();
                }
                else
                {
                    MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Selecione um item.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void dataGridCredito_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCredito.SelectedItem != null)
            {
                itemSelecionadoCredito = (ControlePagamentoCredito)dataGridCredito.SelectedItem;

            }
        }

        private void dataGridDebito_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridDebito.SelectedItem != null)
            {
                itemSelecionadoDebito = (ControlePagamentoDebito)dataGridDebito.SelectedItem;

            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerInicio.SelectedDate != null && datePickerFim.SelectedDate != null)
            {
                FrmRelatorioControlePagamento controle = new FrmRelatorioControlePagamento(datePickerInicio.SelectedDate.Value, datePickerFim.SelectedDate.Value);
                controle.ShowDialog();
                controle.Close();
            }
        }
    }
}
