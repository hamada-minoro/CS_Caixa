using CS_Caixa.Controls;
using CS_Caixa.Models;
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
    /// Interaction logic for WinControleAtosProtesto.xaml
    /// </summary>
    public partial class WinControleAtosProtesto : Window
    {
        private Models.Usuario usuarioLogado;
        private List<ControleAto> listaAtos;
        private WinPrincipal Principal;
        ControleAto atoSelecionado = new ControleAto();
        List<ControleAto> atosSelecionados;

        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        public WinControleAtosProtesto()
        {
            InitializeComponent();
        }

        public WinControleAtosProtesto(Usuario usuarioLogado, WinPrincipal winPrincipal, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            // TODO: Complete member initialization
            this.usuarioLogado = usuarioLogado;
            this.Principal = winPrincipal;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
        }

        public WinControleAtosProtesto(Usuario usuarioLogado, WinPrincipal Principal, ControleAto atoSelecionado, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.usuarioLogado = usuarioLogado;
            this.Principal = Principal;
            this.atoSelecionado = atoSelecionado;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;


            ConsultaData();
                   

            if (dataGrid1.Items.Count > 0)
            {
                if (atoSelecionado != null)
                {
                    dataGrid1.SelectedItem = atoSelecionado;
                    dataGrid1.ScrollIntoView(atoSelecionado);
                }
                else
                    dataGrid1.SelectedIndex = 0;

                btnAlterar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }
            else
            {
                btnAlterar.IsEnabled = false;
                btnExcluir.IsEnabled = false;
            }


        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTpConsulta.SelectedIndex >= 0)
            {
                switch (cmbTpConsulta.SelectedIndex)
                {
                    case 0:
                        ConsultaData();
                        break;
                    case 1:
                        ConsultaProtocolo();
                        break;
                }
            }
            dataGrid1.Focus();
            if (dataGrid1.Items.Count > 0)
            {
                dataGrid1.SelectedIndex = 0;
                btnAlterar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }
            else
            {
                btnAlterar.IsEnabled = false;
                btnExcluir.IsEnabled = false;
            }
        }

        private void ConsultaData()
        {

            try
            {
                ClassControleAto classAto = new ClassControleAto();
                DateTime dataIni, dataFim;

                if (datePickerdataConsulta.SelectedDate != null)
                {
                    dataIni = datePickerdataConsulta.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (datePickerdataConsultaFim.SelectedDate != null)
                {
                    dataFim = datePickerdataConsultaFim.SelectedDate.Value;
                }
                else
                {
                    MessageBox.Show("Informe a data Fim.", "Data Fim", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                listaAtos = classAto.ListarAtoData(dataIni, dataFim, Principal.TipoAto, Principal.Atribuicao);
                dataGrid1.ItemsSource = listaAtos;

                if (atoSelecionado.Id_Ato != 0 && atoSelecionado != null)
                {
                    atoSelecionado = listaAtos.Where(p => p.Id_Ato == atoSelecionado.Id_Ato).FirstOrDefault();
                    dataGrid1.SelectedItem = atoSelecionado;
                    dataGrid1.ScrollIntoView(atoSelecionado);
                }
                else
                {
                    dataGrid1.SelectedIndex = 0;
                }
            }
            catch (Exception) { }
        }

        private void ConsultaProtocolo()
        {
            ClassControleAto classAto = new ClassControleAto();

            if (txtConsulta.Text == "")
            {
                MessageBox.Show("Informe o Número do Protocolo.", "Protocolo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                dataGrid1.ItemsSource = classAto.ListarAtoProtocolo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto);
            else
                dataGrid1.ItemsSource = classAto.ListarAtoProtocoloNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario);

            dataGrid1.Items.Refresh();
        }



        private void txtConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (cmbTpConsulta.SelectedIndex == 1 || cmbTpConsulta.SelectedIndex == 2)
            {
                int key = (int)e.Key;

                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
            }
            else
            {
                int key = (int)e.Key;

                if (txtConsulta.Text.Length <= 3)
                    e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);

                if (txtConsulta.Text.Length > 3)
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
            }
        }

        private void cmbTpConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTpConsulta.Focus())
            {
                if (cmbTpConsulta.SelectedIndex == 0)
                {
                    datePickerdataConsulta.Visibility = Visibility.Visible;
                    datePickerdataConsultaFim.Visibility = Visibility.Visible;
                    txtConsulta.Visibility = Visibility.Hidden;
                }
                else
                {
                    datePickerdataConsulta.Visibility = Visibility.Hidden;
                    datePickerdataConsultaFim.Visibility = Visibility.Hidden;
                    txtConsulta.Visibility = Visibility.Visible;
                }
                txtConsulta.Text = "";

            }
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

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            //if (usuarioLogado.Master == true || usuarioLogado.ExcluirAtos == true)
            //{
            //    if (dataGrid1.Focus())
            //        if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //        {
            //            string mensagem = string.Empty;
            //            try
            //            {
            //                ClassControleAto classAto = new ClassControleAto();
            //                mensagem = classAto.ExcluirAto(atoSelecionado.Id_Ato, Principal.Atribuicao);

                            
            //                    ConsultaData();
                           

            //                dataGrid1.Items.Refresh();
            //                dataGrid1.SelectedIndex = 0;
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(mensagem + " " + ex.Message);
            //            }


            //            if (dataGrid1.Items.Count > 0)
            //            {
            //                dataGrid1.SelectedIndex = 0;
            //                btnAlterar.IsEnabled = true;
            //                btnExcluir.IsEnabled = true;
            //            }
            //            else
            //            {
            //                btnAlterar.IsEnabled = false;
            //                btnExcluir.IsEnabled = false;
            //            }


            //        }
            //}
            //else
            //{
            //    MessageBox.Show("O usuário logado não tem permissão para excluir atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        private void ProcAlterar()
        {
            //if (usuarioLogado.Master == true || usuarioLogado.AlterarAtos == true)
            //{
            //    if (atoSelecionado != null && atoSelecionado.Id_Ato != 0)
            //    {

            //        WinDigitarAtoProtesto digAto = new WinDigitarAtoProtesto(Principal, usuarioLogado, "alterar", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);

            //        digAto.Owner = Principal;
            //        this.Close();
            //        digAto.ShowDialog();

            //    }
            //    else
            //    {
            //        MessageBox.Show("Selecione um item.", "Seleção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("O usuário logado não tem permissão para alterar atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            ProcAlterar();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WinDigitarControleAtosProtesto DigProtesto = new WinDigitarControleAtosProtesto(Principal, usuarioLogado, "novo", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
            DigProtesto.Owner = Principal;
            DigProtesto.ShowDialog();
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



        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.Items.Count > 0)
            {

                try
                {
                    atoSelecionado = (ControleAto)dataGrid1.SelectedItem;

                    atosSelecionados = (List<ControleAto>)dataGrid1.SelectedItems.Cast<ControleAto>().ToList();

                    lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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

       
        private void dataGrid1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            
        }


       

        private void dataGrid1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAlterar();
        }

       
    }
}
