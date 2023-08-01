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

namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Interaction logic for WinRelatorioMensalista.xaml
    /// </summary>
    public partial class WinRelatorioMensalista : Window
    {
        ClassMensalista mensalistas = new ClassMensalista();

        public WinRelatorioMensalista()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            datePickerDataInicio.SelectedDate = DateTime.Now.Date;
            datePickerDataFim.SelectedDate = DateTime.Now.Date;


            cmbNomeMensalista.ItemsSource = mensalistas.ListaMensalistas().Select(p => p.Nome);

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


            FrmRelatorioMensalista frmShow = new FrmRelatorioMensalista(datePickerDataInicio.SelectedDate.Value, datePickerDataFim.SelectedDate.Value, cmbNomeMensalista.Text);
                frmShow.ShowDialog();
                frmShow.Close();
            
        }
    }
}
