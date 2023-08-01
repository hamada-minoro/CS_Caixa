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

namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Interaction logic for WinMovimentoDiario.xaml
    /// </summary>
    public partial class WinMovimentoDiario : Window
    {

        string Titulo;
        Usuario usuarioLogado;
        public WinMovimentoDiario(string Titulo, Usuario usuarioLogado)
        {
            this.Titulo = Titulo;
            this.usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Movimento Diário";
            datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            datePickerDataFim.SelectedDate = DateTime.Now.Date;
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

            if (Titulo == "Movimento Diário Protesto")
            {
                FrmMovimentoDiarioProtesto frmShow = new FrmMovimentoDiarioProtesto(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
                frmShow.ShowDialog();
                frmShow.Close();
            }

            if (Titulo == "Movimento Diário Notas")
            {
                FrmMovimentoDiarioNotas frmShow = new FrmMovimentoDiarioNotas(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
                frmShow.ShowDialog();
                frmShow.Close();
            }
            if (Titulo == "Movimento Diário Rgi")
            {
                FrmMovimentoDiarioRgi frmShow = new FrmMovimentoDiarioRgi(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
                frmShow.ShowDialog();
                frmShow.Close();
            }

            if (Titulo == "Movimento Diário Balcão")
            {
                WinRelatorioMovimentoDiario frmShow = new WinRelatorioMovimentoDiario(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value);
                frmShow.Owner = this;
                frmShow.ShowDialog();
                frmShow.Close();
            }

            if (Titulo == "Relatório de Repasse dos Escreventes")
            {
                FrmRelatorioRepasseEscrevente frmShow = new FrmRelatorioRepasseEscrevente(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value, usuarioLogado);
                frmShow.ShowDialog();
                frmShow.Close();
            }
        }
    }
}
