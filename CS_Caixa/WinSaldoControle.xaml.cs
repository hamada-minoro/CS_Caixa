using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.RelatoriosForms;
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
    /// Lógica interna para WinSaldoControle.xaml
    /// </summary>
    public partial class WinSaldoControle : Window
    {

        ClassControlePagamento classControlePagamento = new ClassControlePagamento();

        List<ControlePagamentoCredito> listaCredito = new List<ControlePagamentoCredito>();

        List<ControlePagamentoDebito> listaDebito = new List<ControlePagamentoDebito>();

        public ControlePagamentoCredito itemSelecionadoCredito = new ControlePagamentoCredito();

        public ControlePagamentoDebito itemSelecionadoDebito = new ControlePagamentoDebito();

       
        List<Controle_Interno> listaItensCICredito = new List<Controle_Interno>();
        List<Controle_Interno> listaItensCIDebito = new List<Controle_Interno>();

        Controle_Interno itemSelecionadoCiCredito = new Controle_Interno();
        Controle_Interno itemSelecionadoCiDebito = new Controle_Interno();

        ClassControleInterno controle = new ClassControleInterno();

        decimal saldoControleInterno;


        decimal totalCredito = 0;

        decimal totalDebito = 0;

        decimal diferenca = 0;

        decimal totalCreditoCi = 0;

        decimal totalDebitoCi = 0;

        decimal diferencaCi = 0;



        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        System.Windows.Threading.DispatcherTimer dispatcherTimerCi = new System.Windows.Threading.DispatcherTimer();

        public WinSaldoControle()
        {
            InitializeComponent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerInicio.SelectedDate = DateTime.Now.Date;
            datePickerFim.SelectedDate = DateTime.Now.Date;
            datePickerInicioCi.SelectedDate = DateTime.Now.Date;
            datePickerFimCi.SelectedDate = DateTime.Now.Date;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimer.Start();

            dispatcherTimerCi.Tick += new EventHandler(dispatcherTimerCi_Tick);
            dispatcherTimerCi.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dispatcherTimerCi.Start();
            
            saldoControleInterno = controle.ObterSaldoControleInterno();

            if (saldoControleInterno != -1)
            {
                lblSaldo.Content = string.Format("{0:n2}", saldoControleInterno);
            }
            ProcConsulta();
            ProcConsultaCi();
        }

        private void ProcConsulta()
        {
            if (datePickerInicio.SelectedDate != null && datePickerFim.SelectedDate != null)
            {
                listaCredito = classControlePagamento.ListarCredito(datePickerInicio.SelectedDate.Value, datePickerFim.SelectedDate.Value);
                listaDebito = classControlePagamento.ListarDebito(datePickerInicio.SelectedDate.Value, datePickerFim.SelectedDate.Value);

                CalcularValores();
                ProcConsultaCi();
            }
        }

        private void ProcConsultaCi()
        {
            if (datePickerInicioCi.SelectedDate != null && datePickerFimCi.SelectedDate != null)
            {
                listaItensCICredito = controle.ListarEntrada(datePickerInicioCi.SelectedDate.Value, datePickerFimCi.SelectedDate.Value).OrderBy(p => p.Data).ToList();
                listaItensCIDebito = controle.ListarSaida(datePickerInicioCi.SelectedDate.Value, datePickerFimCi.SelectedDate.Value).OrderBy(p => p.Data).ToList();
                CalcularValoresCi();
            }
        }

        private void CalcularValoresCi()
        {


            listViewReceitasCi.ItemsSource = listaItensCICredito.OrderBy(p => p.Data);

            listViewDespesasCi.ItemsSource = listaItensCIDebito.OrderBy(p => p.Data);

            totalCreditoCi = listaItensCICredito.Sum(p => p.Valor);

            totalDebitoCi = listaItensCIDebito.Sum(p => p.Valor);

            lblTotalCreditoCi.Content = string.Format("Total de Receitas: {0:n2}", totalCreditoCi);

            lblTotalDebitoCi.Content = string.Format("Total de Despesas: {0:n2}", totalDebitoCi);

            diferencaCi = totalCreditoCi - totalDebitoCi;


            lblDiferencaCi.Content = string.Format("R$ {0:n2}", diferencaCi);

            if (diferencaCi < 0)
            {
                lblDiferencaCi.Foreground = new SolidColorBrush(Colors.Red);
                dispatcherTimerCi.Start();

            }
            else
            {
                dispatcherTimerCi.Stop();
                lblDiferencaCi.Foreground = new SolidColorBrush(Colors.Blue);
            }
                       

            if (listViewReceitasCi.Items.Count > 0)
            {
                listViewReceitasCi.IsEnabled = true;
                listViewReceitasCi.SelectedIndex = 0;
            }
            else
            {
                listViewReceitasCi.IsEnabled = false;
            }

            if (listViewDespesasCi.Items.Count > 0)
            {
                listViewDespesasCi.IsEnabled = true;
                listViewDespesasCi.SelectedIndex = 0;
            }
            else
            {
                listViewDespesasCi.IsEnabled = false;
            }
        }


          private void CalcularValores()
        {


            listViewReceitas.ItemsSource = listaCredito.OrderBy(p => p.Data);

            listViewDespesas.ItemsSource = listaDebito.OrderBy(p => p.Data);

            totalCredito = listaCredito.Sum(p => p.Valor);

            totalDebito = listaDebito.Sum(p => p.Valor);

            lblTotalCredito.Content = string.Format("Total de Receitas: {0:n2}", totalCredito);

            lblTotalDebito.Content = string.Format("Total de Despesas: {0:n2}", totalDebito);

            diferenca = totalCredito - totalDebito;


            lblDiferenca.Content = string.Format("R$ {0:n2}", diferenca);

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

            if (listViewReceitas.Items.Count > 0)
            {
                listViewReceitas.IsEnabled = true;
                listViewReceitas.SelectedIndex = 0;
            }
            else
            {
                listViewReceitas.IsEnabled = false;
            }

            if (listViewDespesas.Items.Count > 0)
            {
                listViewDespesas.IsEnabled = true;
                listViewDespesas.SelectedIndex = 0;
            }
            else
            {
                listViewDespesas.IsEnabled = false;
            }
        }

        bool ativo = true;
        bool ativoCi = true;
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


        private void dispatcherTimerCi_Tick(object sender, EventArgs e)
        {

            
            if (diferencaCi < 0)
            {

                if (ativoCi == true)
                {
                    lblDiferencaCi.Foreground = new SolidColorBrush(Colors.Red);
                    ativoCi = false;
                }
                else
                {
                    lblDiferencaCi.Foreground = new SolidColorBrush(Colors.Transparent);
                    ativoCi = true;
                }
            }

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

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ProcConsulta();
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

        private void listViewReceitas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewReceitas.SelectedItem != null)
            {
                itemSelecionadoCredito = (ControlePagamentoCredito)listViewReceitas.SelectedItem;

            }
        }

        private void listViewDespesas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewDespesas.SelectedItem != null)
            {
                itemSelecionadoDebito = (ControlePagamentoDebito)listViewDespesas.SelectedItem;

            }
        }




        // ---------------- Controle Interno ----------------------------------

        private void datePickerInicioCi_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioCi.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioCi.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimCi.SelectedDate = datePickerInicioCi.SelectedDate;

            if (datePickerInicioCi.SelectedDate > datePickerFimCi.SelectedDate)
            {
                datePickerFimCi.SelectedDate = datePickerInicioCi.SelectedDate;
            }
        }

        private void datePickerFimCi_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioCi.SelectedDate != null)
            {
                if (datePickerInicioCi.SelectedDate > datePickerFimCi.SelectedDate)
                {
                    datePickerFimCi.SelectedDate = datePickerInicioCi.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void btnConsultarCi_Click(object sender, RoutedEventArgs e)
        {
            ProcConsultaCi();
        }

        private void listViewReceitasCi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewReceitasCi.SelectedItem != null)
            {
                itemSelecionadoCiCredito = (Controle_Interno)listViewReceitasCi.SelectedItem;
            }
        }

        private void listViewDespesasCi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewDespesas.SelectedItem != null)
            {
                itemSelecionadoCiDebito = (Controle_Interno)listViewDespesasCi.SelectedItem;

            }
        }

       
    }
}
