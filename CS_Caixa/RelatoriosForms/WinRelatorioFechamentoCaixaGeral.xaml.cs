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
using CS_Caixa.Models;
using CS_Caixa.Controls;
using System.IO;
using System.Diagnostics;



namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Interaction logic for WinRelatorioFechamentoCaixaGeral.xaml
    /// </summary>
    public partial class WinRelatorioFechamentoCaixaGeral : Window
    {
        string Titulo;
        public WinRelatorioFechamentoCaixaGeral(string Titulo)
        {
            this.Titulo = Titulo;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Titulo;
            datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            datePickerDataFim.SelectedDate = DateTime.Now.Date;
            rbSimplificado.IsChecked = true;
            rbFinanceiro.IsChecked = true;

            //if (Titulo != "Relatório de Fechamento de Caixa")
            //{
            //    rbSimplificado.IsEnabled = false;
            //    rbFinanceiro.IsEnabled = false;
            //    rbDetalhado.IsEnabled = false;
            //    rbAtosPraticados.IsEnabled = false;
            //}
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

        private void btnRelatorio_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDataInicio.SelectedDate == null || datePickerDataFim.SelectedDate == null)
            {
                MessageBox.Show("Por favor informe a data inicial e final.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            string tipoRelatorio = string.Empty;

            bool ExibirRepasse = ckbExibirRepasse.IsChecked.Value;

            if (Titulo == "Relatório de Fechamento de Caixa")
            {

                if (rbSimplificado.IsChecked == true && rbFinanceiro.IsChecked == true)
                {
                    tipoRelatorio = "SimplificadoFinanceiro";
                }

                if (rbSimplificado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                {
                    tipoRelatorio = "SimplificadoAtosPraticados";
                }

                if (rbDetalhado.IsChecked == true && rbFinanceiro.IsChecked == true)
                {
                    tipoRelatorio = "DetalhadoFinanceiro";
                }

                if (rbDetalhado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                {
                    tipoRelatorio = "DetalhadoAtosPraticados";
                }

            }
            else
            {
                switch (Titulo)
                {
                    case "Relatório de Fechamento de Caixa Protesto":

                        if (rbSimplificado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "ProtestoSimplificadoFinanceiro";
                        }
                        if (rbSimplificado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "ProtestoSimplificadoAtosPraticados";
                        }

                        if (rbDetalhado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "ProtestoDetalhadoFinanceiro";
                        }

                        if (rbDetalhado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "ProtestoDetalhadoAtosPraticados";
                        }

                        break;

                    case "Relatório de Fechamento de Caixa Notas":

                        if (rbSimplificado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "NotasSimplificadoFinanceiro";
                        }
                        if (rbSimplificado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "NotasSimplificadoAtosPraticados";
                        }

                        if (rbDetalhado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "NotasDetalhadoFinanceiro";
                        }

                        if (rbDetalhado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "NotasDetalhadoAtosPraticados";
                        }

                        break;

                    case "Relatório de Fechamento de Caixa Rgi":

                        if (rbSimplificado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "RgiSimplificadoFinanceiro";
                        }
                        if (rbSimplificado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "RgiSimplificadoAtosPraticados";
                        }

                        if (rbDetalhado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "RgiDetalhadoFinanceiro";
                        }

                        if (rbDetalhado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "RgiDetalhadoAtosPraticados";
                        }

                        break;

                    case "Relatório de Fechamento de Caixa Balcão":

                        if (rbSimplificado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "BalcãoSimplificadoFinanceiro";
                        }
                        if (rbSimplificado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "BalcãoSimplificadoAtosPraticados";
                        }

                        if (rbDetalhado.IsChecked == true && rbFinanceiro.IsChecked == true)
                        {
                            tipoRelatorio = "BalcãoDetalhadoFinanceiro";
                        }

                        if (rbDetalhado.IsChecked == true && rbAtosPraticados.IsChecked == true)
                        {
                            tipoRelatorio = "BalcãoDetalhadoAtosPraticados";
                        }

                        break;

                    default:
                        break;
                }

            }

            WinAguardeRelatorioFechamentoCaixa winShow = new WinAguardeRelatorioFechamentoCaixa(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value, tipoRelatorio, ExibirRepasse, this);
            winShow.ShowDialog();
            winShow.Close();
        }

        private void ckbAntigo_Checked(object sender, RoutedEventArgs e)
        {
            border1.IsEnabled = false;
            border2.IsEnabled = false;
        }

        private void ckbAntigo_Unchecked(object sender, RoutedEventArgs e)
        {
            border1.IsEnabled = true;
            border2.IsEnabled = true;
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void gridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();              
        }
    }
}
