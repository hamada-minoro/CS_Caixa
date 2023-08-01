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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinDataImportarCredito.xaml
    /// </summary>
    public partial class WinDataImportarCredito : Window
    {
        WinControlePagamento tipo;
        public WinDataImportarCredito(WinControlePagamento tipo)
        {
            this.tipo = tipo;
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
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

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {

            if (datePickerData.SelectedDate != null)
            {
                WinAguardeControlePagamento aguarde = new WinAguardeControlePagamento(datePickerData.SelectedDate.Value, datePickerData2.SelectedDate.Value, tipo);
                aguarde.Owner = tipo;
                aguarde.ShowDialog();

                tipo.datePickerData.SelectedDate = datePickerData.SelectedDate.Value;
                tipo.datePickerDataConsulta.SelectedDate = datePickerData.SelectedDate;
                tipo.datePickerDataConsultaFim.SelectedDate = datePickerData2.SelectedDate;


                ClassControlePagamento classControlePagamento = new ClassControlePagamento();

                if (tipo.tela == "credito")
                {
                    tipo.listaItens = classControlePagamento.ListarCredito(datePickerData.SelectedDate.Value, datePickerData2.SelectedDate.Value);
                    tipo.dataGrid1.ItemsSource = tipo.listaItens;
                    tipo.dataGrid1.Items.Refresh();
                }
                else
                {
                    tipo.listaItensDebito = classControlePagamento.ListarDebito(datePickerData.SelectedDate.Value, datePickerData2.SelectedDate.Value);
                    tipo.dataGrid1.ItemsSource = tipo.listaItensDebito;
                    tipo.dataGrid1.Items.Refresh();
                }



                if (tipo.dataGrid1.Items.Count > 0)
                {
                    tipo.dataGrid1.IsEnabled = true;
                    tipo.dataGrid1.SelectedIndex = 0;

                }
                else
                {

                    tipo.itemSeleciondo = null;
                    tipo.dataGrid1.IsEnabled = false;
                }

                this.Close();
            }



        }

        private void datePickerData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerData.SelectedDate > DateTime.Now.Date)
            {
                datePickerData.SelectedDate = DateTime.Now.Date;
            }

            datePickerData2.SelectedDate = datePickerData.SelectedDate;

            if (datePickerData2.SelectedDate > datePickerData2.SelectedDate)
            {
                datePickerData2.SelectedDate = datePickerData.SelectedDate;
            }
        }

        private void datePickerDataFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerData.SelectedDate != null)
            {
                if (datePickerData.SelectedDate > datePickerData2.SelectedDate)
                {
                    datePickerData2.SelectedDate = datePickerData.SelectedDate;
                }

                if (datePickerData2.SelectedDate == null)
                    datePickerData2.SelectedDate = datePickerData.SelectedDate;
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }




        }




    }
}
