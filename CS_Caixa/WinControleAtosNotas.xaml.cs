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
    /// Interaction logic for WinControleAtosNotas.xaml
    /// </summary>
    public partial class WinControleAtosNotas : Window
    {
        Usuario usuarioLogado = new Usuario();
        List<ControleAto> listaAtos = new List<ControleAto>();
        ControleAto atoSelecionado = new ControleAto();
        WinPrincipal Principal;
        List<ControleAto> atosSelecionados;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        public WinControleAtosNotas(Usuario usuarioLogado, WinPrincipal Principal, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.usuarioLogado = usuarioLogado;
            this.Principal = Principal;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
        }

        public WinControleAtosNotas(Usuario usuarioLogado, WinPrincipal Principal, ControleAto atoSelecionado, DateTime dataInicioConsulta, DateTime dataFimConsulta)
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

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.Items.Count > 0)
            {
                try
                {
                    atoSelecionado = (ControleAto)dataGrid1.SelectedItem;

                    atosSelecionados = (List<ControleAto>)dataGrid1.SelectedItems.Cast<ControleAto>().ToList();

                    if (atosSelecionados.Count > 0)
                    {
                        lblTotalSelecionado.Content = string.Format("Emol.: {0:n2}  -  Fetj: {1:n2}  -  Fundperj: {2:n2}  -  Funperj: {3:n2}  -  Funarpen: {4:n2}  -  Pmcmv: {5:n2}  -  Iss: {6:n2}  -  Mútua: {7:n2}  -  Acoterj: {8:n2}", atosSelecionados.Sum(p => p.Emolumentos), atosSelecionados.Sum(p => p.Fetj), atosSelecionados.Sum(p => p.Fundperj), atosSelecionados.Sum(p => p.Funperj), atosSelecionados.Sum(p => p.Funarpen), atosSelecionados.Sum(p => p.Pmcmv), atosSelecionados.Sum(p => p.Iss), atosSelecionados.Sum(p => p.Mutua), atosSelecionados.Sum(p => p.Acoterj));
                        lblQtdBalcao.Content = string.Format("Qtd Autenticação: {0}  -  Qtd Abertura de Firmas: {1}  -  Qtd Reconhecimento por Autenticidade: {2}  -  Qtd Reconhecimento por Semelhança: {3}", atosSelecionados.Where(p => p.Natureza == "AUTENTICAÇÃO").Count(), atosSelecionados.Where(p => p.Natureza == "ABERTURA DE FIRMAS").Count(), atosSelecionados.Where(p => p.Natureza == "REC AUTENTICIDADE").Count(), atosSelecionados.Where(p => p.Natureza == "REC SEMELHANÇA").Count());
                        lblQtdNotas.Content = string.Format("Qtd Escritura: {0}  -  Qtd Procuração: {1}  -  Qtd Testamento: {2}  -  Qtd Certidão: {3}", atosSelecionados.Where(p => p.TipoAto == "ESCRITURA").Count(), atosSelecionados.Where(p => p.TipoAto == "PROCURAÇÃO").Count(), atosSelecionados.Where(p => p.TipoAto == "TESTAMENTO").Count(), atosSelecionados.Where(p => p.TipoAto == "CERTIDÃO NOTAS").Count());
                    }
                    else
                    {
                        lblTotalSelecionado.Content = "";
                        lblQtdBalcao.Content = "";
                        lblQtdNotas.Content = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var DigEscritura = new WinDigitarControleAtosNotas(Principal, usuarioLogado, "novo", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
            DigEscritura.Owner = Principal;
            DigEscritura.ShowDialog();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.ExcluirAtos == true)
            {
                if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string mensagem = string.Empty;
                    try
                    {
                        ClassControleAto classAto = new ClassControleAto();
                        mensagem = classAto.ExcluirAto(atoSelecionado.Id_ControleAtos, Principal.Atribuicao);
                        ProcConsultar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(mensagem + " " + ex.Message);
                    }

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
            }
            else
            {
                MessageBox.Show("O usuário logado não tem permissão para excluir atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ProcAlterar()
        {
            if (usuarioLogado.Master == true || usuarioLogado.AlterarAtos == true)
            {
                if (atoSelecionado != null && atoSelecionado.Id_ControleAtos != 0)
                {

                    this.Close();
                    WinDigitarControleAtosNotas digAto = new WinDigitarControleAtosNotas(Principal, usuarioLogado, "alterar", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
                    digAto.Owner = Principal;
                    digAto.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Selecione um item.", "Seleção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("O usuário logado não tem permissão para alterar atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            ProcAlterar();
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
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            ProcConsultar();
        }

        public void ProcConsultar()
        {
            if (cmbTpConsulta.SelectedIndex >= 0)
            {
                switch (cmbTpConsulta.SelectedIndex)
                {
                    case 0:
                        ConsultaData();
                        break;
                    case 1:
                        ConsultaLivro();
                        break;
                    case 2:
                        ConsultaAto();
                        break;
                    case 3:
                        ConsultaSelo();
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

                lblTotalSelecionado.Content = "";
                lblQtdBalcao.Content = "";
                lblQtdNotas.Content = "";
            }
        }

        private void ConsultaData()
        {
            try
            {
                var classControleAto = new ClassControleAto();
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

                listaAtos = classControleAto.ListarAtoData(dataIni, dataFim, Principal.TipoAto, Principal.Atribuicao);
                dataGrid1.ItemsSource = listaAtos;

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    atoSelecionado = listaAtos.Where(p => p.Id_ControleAtos == atoSelecionado.Id_ControleAtos).FirstOrDefault();
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

        private void ConsultaLivro()
        {
            var classAto = new ClassControleAto();

            if (txtConsulta.Text == "")
            {
                MessageBox.Show("Informe o Número do Livro.", "Livro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                dataGrid1.ItemsSource = classAto.ListarAtoLivro(txtConsulta.Text, Principal.TipoAto);
            else
                dataGrid1.ItemsSource = classAto.ListarAtoLivroNomeEscrevente(txtConsulta.Text, Principal.TipoAto, usuarioLogado.Id_Usuario);
            dataGrid1.Items.Refresh();
        }

        private void ConsultaAto()
        {
            var classAto = new ClassControleAto();
            int numeroAto;
            try
            {
                if (txtConsulta.Text != "")
                {
                    numeroAto = Convert.ToInt32(txtConsulta.Text);
                }
                else
                {
                    MessageBox.Show("Informe o Número do Ato.", "Ato", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                    dataGrid1.ItemsSource = classAto.ListarAtoNumeroAto(numeroAto, Principal.TipoAto);
                else
                    dataGrid1.ItemsSource = classAto.ListarAtoNumeroAtoNomeEscrevente(numeroAto, Principal.TipoAto, usuarioLogado.Id_Usuario);
                dataGrid1.Items.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Valor digitado não é número inteiro válido, favor verifique.", "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ConsultaSelo()
        {
            var classAto = new ClassControleAto();
            string letra;
            int numero;

            if (txtConsulta.Text.Length == 9)
            {
                try
                {
                    letra = txtConsulta.Text.Substring(0, 4);

                    numero = Convert.ToInt32(txtConsulta.Text.Substring(4, 5));

                    if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                        dataGrid1.ItemsSource = classAto.ListarAtoSelo(letra, numero, Principal.TipoAto);
                    else
                        dataGrid1.ItemsSource = classAto.ListarAtoSeloNomeEscrevente(letra, numero, Principal.TipoAto, usuarioLogado.Id_Usuario);
                    dataGrid1.Items.Refresh();
                }
                catch (Exception)
                {
                    MessageBox.Show("O número de selo informado está incorreto, favor verifique.", "Selo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("O número de selo informado está incorreto, favor verifique.", "Selo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
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


        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioLogado.Master == true || usuarioLogado.ExcluirAtos == true)
            {
                if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string mensagem = string.Empty;
                    try
                    {
                        var classAto = new ClassControleAto();
                        mensagem = classAto.ExcluirAto(atoSelecionado.Id_ControleAtos, Principal.Atribuicao);

                        listaAtos.Remove(atoSelecionado);
                        dataGrid1.ItemsSource = listaAtos;
                        dataGrid1.Items.Refresh();
                        dataGrid1.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(mensagem + " " + ex.Message);
                    }

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
            }
            else
            {
                MessageBox.Show("O usuário logado não tem permissão para excluir atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        
        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAlterar();
        }

        private void btnSincronizar_Click(object sender, RoutedEventArgs e)
        {
            var sincronizar = new WinSincronizarControleAtos(this);
            sincronizar.Owner = this;
            sincronizar.ShowDialog();
        }
    }
}
