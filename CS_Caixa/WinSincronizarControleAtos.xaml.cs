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
    /// Interaction logic for WinSincronizarControleAtos.xaml
    /// </summary>
    public partial class WinSincronizarControleAtos : Window
    {
        private WinControleAtosNotas winControleAtosNotas;

        public WinSincronizarControleAtos()
        {
            InitializeComponent();
        }

        public WinSincronizarControleAtos(WinControleAtosNotas winControleAtosNotas)
        {
            // TODO: Complete member initialization
            this.winControleAtosNotas = winControleAtosNotas;
            InitializeComponent();
        }

        private void btnSincronizar_Click(object sender, RoutedEventArgs e)
        {
            var sincronizar = new WinAguardeControleAtos(winControleAtosNotas, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
            sincronizar.Owner = winControleAtosNotas;
            this.Close();
            sincronizar.ShowDialog();
        }

        private void datePickerdataConsulta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerdataConsulta.SelectedDate > DateTime.Now.Date)
            {
                datePickerdataConsulta.SelectedDate = DateTime.Now.Date;
            }

            datePickerdataConsultaFim.SelectedDate = datePickerdataConsulta.SelectedDate;

            if (datePickerdataConsulta.SelectedDate > datePickerdataConsultaFim.SelectedDate)
            {
                datePickerdataConsultaFim.SelectedDate = datePickerdataConsulta.SelectedDate;
            }

        }

        private void datePickerdataConsultaFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerdataConsulta.SelectedDate != null)
            {
                if (datePickerdataConsulta.SelectedDate > datePickerdataConsultaFim.SelectedDate)
                {
                    datePickerdataConsultaFim.SelectedDate = datePickerdataConsulta.SelectedDate;
                }
                if(datePickerdataConsultaFim.SelectedDate.Value.Year != datePickerdataConsulta.SelectedDate.Value.Year)
                {
                    datePickerdataConsultaFim.SelectedDate = datePickerdataConsulta.SelectedDate;
                    MessageBox.Show("Informe o período dentro do mesmo ano.", "Período", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
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
            datePickerdataConsulta.SelectedDate = DateTime.Now.Date;
        }

        private void grid1_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
