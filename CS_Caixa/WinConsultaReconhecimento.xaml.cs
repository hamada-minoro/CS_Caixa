using CS_Caixa.Objetos_de_Valor;
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
    /// Lógica interna para WinConsultaReconhecimento.xaml
    /// </summary>
    public partial class WinConsultaReconhecimento : Window
    {

        public List<AtosFirmas> atosFirmas = new List<AtosFirmas>();

        public string tipoConsulta = string.Empty;


        public WinConsultaReconhecimento()
        {
            InitializeComponent();
        }

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerdataConsulta.SelectedDate = DateTime.Now.Date;

            datePickerdataConsultaFim.SelectedDate = DateTime.Now.Date;
        }


        private void btnConsultarNomeCpf_Click(object sender, RoutedEventArgs e)
        {

            if (txtConsulta.Text.Trim() == "")
                return;

            tipoConsulta = "NomeCPF";

            dataGridConsulta.ItemsSource = null;

            txtConsulta.Text = txtConsulta.Text.Trim();

            atosFirmas = new List<AtosFirmas>();

            WinAguardeConsultaReconhecimento consulta = new WinAguardeConsultaReconhecimento(this, txtConsulta.Text);
            consulta.Owner = this;
            consulta.ShowDialog();


            dataGridConsulta.ItemsSource = atosFirmas;
            dataGridConsulta.Items.Refresh();
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

        private void datePickerdataConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void datePickerdataConsultaFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerdataConsulta.SelectedDate != null)
            {
                if (datePickerdataConsulta.SelectedDate > datePickerdataConsultaFim.SelectedDate)
                {
                    datePickerdataConsultaFim.SelectedDate = datePickerdataConsulta.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void datePickerdataConsultaFim_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void btnConsultarData_Click(object sender, RoutedEventArgs e)
        {

            if (datePickerdataConsulta.SelectedDate == null || datePickerdataConsultaFim.SelectedDate == null)
                return;


            txtConsulta.Text = txtConsulta.Text.Trim();

            tipoConsulta = "Data";

            dataGridConsulta.ItemsSource = null;

            atosFirmas = new List<AtosFirmas>();

            WinAguardeConsultaReconhecimento consulta = new WinAguardeConsultaReconhecimento(this, txtConsulta.Text);
            consulta.Owner = this;
            consulta.ShowDialog();


            dataGridConsulta.ItemsSource = atosFirmas;
            dataGridConsulta.Items.Refresh();
        }

    }
}
