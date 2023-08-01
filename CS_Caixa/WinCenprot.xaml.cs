using CS_Caixa.Agragador;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for WinCenprot.xaml
    /// </summary>
    public partial class WinCenprot : Window
    {
        public TituloProtesto tituloProtesto = new TituloProtesto();
        public List<TituloProtesto> titulos;
        public string tipoConsulta = string.Empty;
        public System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        public WinCenprot()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerdataConsulta.SelectedDate = DateTime.Now.Date.AddDays(-5);

            datePickerdataConsultaFim.SelectedDate = DateTime.Now.Date;
        }

        private void btnConsultarData_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerdataConsulta.SelectedDate != null && datePickerdataConsultaFim.SelectedDate != null)
            {
                qtdTitulos.Content = "Aguarde...";

                listViewDocumento.ItemsSource = null;
                listViewEspecies.ItemsSource = null;
                listViewIrregularidades.ItemsSource = null;
                listViewApresentantes.ItemsSource = null;
                dataGridConsulta.ItemsSource = null;

                qtdTitulosProtestados.Content = "Protestados: 0";
                qtdTitulosCancelados.Content = "Cancelados: 0";
                qtdTitulosPagos.Content = "Pagos: 0";
                qtdTitulosRetirados.Content = "Retirados: 0";
                qtdTitulosSustados.Content = "Sustados: 0";
                qtdTitulosDevolvidos.Content = "Devolvidos: 0";
                QtdEspecies.Content = "Espécies dos Títulos: 0";
                QtdApresentantes.Content = "Apresentantes: 0";
                QtdIrrgularidades.Content = "Irregularidades: 0";

                tipoConsulta = "data";

                WinAguardeCenprot cenprot = new WinAguardeCenprot(this);
                cenprot.Owner = this;
                cenprot.ShowDialog();
                qtdTitulos.Content = string.Format("Qtd. de Titulos: {0}", titulos.Count.ToString());
                colCPF_CNPJ_DEVEDOR.Visibility = Visibility.Visible;
            }
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

        private void btnArquivo_Click(object sender, RoutedEventArgs e)
        {
            

            openFileDialog1.Filter = "All files (*.xml)|*.xml";

            openFileDialog1.InitialDirectory = @"\\SERVIDOR\Total\Protesto\Arquivos\CENPROT";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                qtdTitulos.Content = "Aguarde...";

                listViewDocumento.ItemsSource = null;
                listViewEspecies.ItemsSource = null;
                listViewIrregularidades.ItemsSource = null;
                listViewApresentantes.ItemsSource = null;
                dataGridConsulta.ItemsSource = null;

                qtdTitulosProtestados.Content = "Protestados: 0";
                qtdTitulosCancelados.Content = "Cancelados: 0";
                qtdTitulosPagos.Content = "Pagos: 0";
                qtdTitulosRetirados.Content = "Retirados: 0";
                qtdTitulosSustados.Content = "Sustados: 0";
                qtdTitulosDevolvidos.Content = "Devolvidos: 0";
                QtdEspecies.Content = "Espécies dos Títulos: 0";
                QtdApresentantes.Content = "Apresentantes: 0";
                QtdIrrgularidades.Content = "Irregularidades: 0";
                tipoConsulta = "arquivo";

                WinAguardeCenprot cenprot = new WinAguardeCenprot(this);
                cenprot.Owner = this;
                cenprot.ShowDialog();
                qtdTitulos.Content = string.Format("Qtd. de Titulos: {0}", titulos.Count.ToString());
                colCPF_CNPJ_DEVEDOR.Visibility = Visibility.Hidden;
            }
        }
    }
}
