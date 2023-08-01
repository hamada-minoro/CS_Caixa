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
using CS_Caixa.RelatoriosForms;


namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAtosRgi.xaml
    /// </summary>
    public partial class WinAtosRgi : Window
    {

        Usuario usuarioLogado = new Usuario();
        List<Ato> listaAtos = new List<Ato>();
        public Ato atoSelecionado = new Ato();
        WinPrincipal Principal;
        List<Ato> atosSelecionados;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;
        public int idAtoNovo = 0;

        public WinAtosRgi(Usuario usuarioLogado, WinPrincipal Principal, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.usuarioLogado = usuarioLogado;
            this.Principal = Principal;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
        }

        public WinAtosRgi(Usuario usuarioLogado, WinPrincipal Principal, Ato atoSelecionado, DateTime dataInicioConsulta, DateTime dataFimConsulta)
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
            this.Title = Principal.TipoAto;

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;

            ConsultaData();

            if (Principal.TipoAto != "CERTIDÃO RGI")
            {
                colRecibo.Visibility = Visibility.Hidden;                
            }
            else
            {
                colRecibo.Visibility = Visibility.Visible;
                colProtocolo.Visibility = Visibility.Hidden;
                lblTotalSelecionadoBib.Visibility = Visibility.Hidden;
                lblTotalSelecionadoDistribuicao.Visibility = Visibility.Hidden;
                lblTotalSelecionadoPrenotacao.Visibility = Visibility.Hidden;
            }

            var bc = new BrushConverter();

            if (Principal.TipoAto == "REGISTRO")
                this.Background = (Brush)bc.ConvertFrom("#FFD3E9EF");

            if (Principal.TipoAto == "AVERBAÇÃO")
                this.Background = (Brush)bc.ConvertFrom("#FFE2E0BA");

            if (Principal.TipoAto == "CERTIDÃO RGI")
                this.Background = (Brush)bc.ConvertFrom("#FF7FD3C0");

            datePickerdataConsulta.SelectedDate = DateTime.Now.AddDays(-10);
            datePickerdataConsultaFim.SelectedDate = DateTime.Now.Date;

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
                btnImprimir.IsEnabled = true;
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
                    atoSelecionado = (Ato)dataGrid1.SelectedItem;

                    atosSelecionados = (List<Ato>)dataGrid1.SelectedItems.Cast<Ato>().ToList();

                    lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));

                    lblTotalSelecionadoBib.Content = string.Format("Valor Total BIB: {0:n2}", atosSelecionados.Sum(p => p.Indisponibilidade));

                    lblTotalSelecionadoDistribuicao.Content = string.Format("Valor Total Distribuição: {0:n2}", atosSelecionados.Sum(p => p.Distribuicao));

                    lblTotalSelecionadoPrenotacao.Content = string.Format("Valor Total Prenotação: {0:n2}", atosSelecionados.Sum(p => p.Prenotacao));

                    if (atoSelecionado != null)
                        MenuContext();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            WinDigitarAtoRgi AtoRgi = new WinDigitarAtoRgi(Principal, usuarioLogado, "novo", this, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
            AtoRgi.Owner = Principal;
            AtoRgi.ShowDialog();

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;
            ConsultaData();

            dataGrid1.Items.Refresh();
            dataGrid1.SelectedItem = atoSelecionado;
            dataGrid1.ScrollIntoView(atoSelecionado);
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            ProcExluir();
        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {  
            ProcAlterar();
        }

        private void ProcAlterar()
        {
            if (usuarioLogado.Master == true || usuarioLogado.AlterarAtos == true)
            {
                if (atoSelecionado != null && atoSelecionado.Id_Ato != 0)
                {                        
                    WinDigitarAtoRgi digAto = new WinDigitarAtoRgi(Principal, usuarioLogado, "alterar", this, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
                    digAto.Owner = Principal;
                    digAto.ShowDialog();

                    for (int i = 0; i < listaAtos.Count(); i++)
                    {
                        if (listaAtos[i].Id_Ato == atoSelecionado.Id_Ato)
                        {
                            listaAtos[i] = atoSelecionado;
                            dataGrid1.Items.Refresh();
                            dataGrid1.SelectedItem = listaAtos[i];
                            dataGrid1.ScrollIntoView(listaAtos[i]);
                        }
                    }
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
                    btnImprimir.IsEnabled = false;
                }
                txtConsulta.Text = "";
            }
        }

        private void datePickerdataConsulta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnImprimir.IsEnabled = false;

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
            btnImprimir.IsEnabled = false;

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
                btnAlterar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }
            else
            {
                btnAlterar.IsEnabled = false;
                btnExcluir.IsEnabled = false;
            }

            if (dataGrid1.Items.Count > 0 && cmbTpConsulta.SelectedIndex == 0)
                btnImprimir.IsEnabled = true;
        }

        private void ConsultaData()
        {
            try
            {
                ClassAto classAto = new ClassAto();
                DateTime dataIni, dataFim;

                if (datePickerdataConsulta.SelectedDate != null)
                {
                    dataIni = datePickerdataConsulta.SelectedDate.Value.Date;
                }
                else
                {
                    MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (datePickerdataConsultaFim.SelectedDate != null)
                {
                    dataFim = datePickerdataConsultaFim.SelectedDate.Value.Date;
                }
                else
                {
                    MessageBox.Show("Informe a data Fim.", "Data Fim", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                listaAtos = classAto.ListarAtoData(dataIni, dataFim, Principal.TipoAto, Principal.Atribuicao);
                dataGrid1.ItemsSource = listaAtos;

                if (idAtoNovo > 0)
                    atoSelecionado = listaAtos.Where(p => p.Id_Ato == idAtoNovo).FirstOrDefault();

                if (atoSelecionado != null && atoSelecionado.Id_Ato > 0)
                {
                    var item = listaAtos.Where(p => p.Id_Ato == atoSelecionado.Id_Ato).FirstOrDefault();

                    dataGrid1.SelectedItem = item;
                    dataGrid1.ScrollIntoView(item);
                }
                else
                {
                    if (dataGrid1.Items.Count > 0)
                        dataGrid1.SelectedIndex = 0;
                }
            }
            catch (Exception) { }
        }

        private void ConsultaProtocolo()
        {
            ClassAto classAto = new ClassAto();

            if (txtConsulta.Text == "")
            {
                MessageBox.Show("Informe o Número do Protocolo.", "Protocolo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                dataGrid1.ItemsSource = classAto.ListarAtoProtocoloRgi(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto);
            else
                dataGrid1.ItemsSource = classAto.ListarAtoProtocoloRgiNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario);
            dataGrid1.Items.Refresh();
        }

        private void ConsultaAto()
        {
            ClassAto classAto = new ClassAto();
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

                dataGrid1.ItemsSource = classAto.ListarAtoNumeroAto(numeroAto, Principal.TipoAto);
                dataGrid1.Items.Refresh();

            }
            catch (Exception)
            {
                MessageBox.Show("Valor digitado não é número inteiro válido, favor verifique.", "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ConsultaSelo()
        {
            ClassAto classAto = new ClassAto();
            string letra;
            int numero;

            if (txtConsulta.Text.Length == 9)
            {
                try
                {
                    letra = txtConsulta.Text.Substring(0, 4);

                    numero = Convert.ToInt32(txtConsulta.Text.Substring(4, 5));

                    dataGrid1.ItemsSource = classAto.ListarAtoSelo(letra, numero, Principal.TipoAto);
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

        private void MenuContext()
        {

            if (dataGrid1.Items.Count > 0)
            {
                if (dataGrid1.SelectedItem != null)
                {
                    atoSelecionado = (Ato)dataGrid1.SelectedItem;

                    if (atoSelecionado.TipoPagamento == "DINHEIRO")
                    {

                        MenuItemDinheiro.IsChecked = true;
                    }
                    else
                    {
                        MenuItemDinheiro.IsChecked = false;
                    }
                    if (atoSelecionado.TipoPagamento == "DEPÓSITO")
                    {
                        MenuItemDeposito.IsChecked = true;
                    }
                    else
                    {
                        MenuItemDeposito.IsChecked = false;
                    }

                    if (atoSelecionado.TipoPagamento == "MENSALISTA")
                    {
                        MenuItemMensalista.IsChecked = true;
                    }
                    else
                    {
                        MenuItemMensalista.IsChecked = false;
                    }

                    if (atoSelecionado.TipoPagamento == "CHEQUE")
                    {
                        MenuItemCheque.IsChecked = true;
                    }
                    else
                    {
                        MenuItemCheque.IsChecked = false;
                    }

                    if (atoSelecionado.TipoPagamento == "PIX BRADESCO")
                    {
                        MenuItemPre.IsChecked = true;
                    }
                    else
                    {
                        MenuItemPre.IsChecked = false;
                    }

                    if (atoSelecionado.TipoPagamento == "PIX NUBANK")
                    {
                        MenuItemBoleto.IsChecked = true;
                    }
                    else
                    {
                        MenuItemBoleto.IsChecked = false;
                    }

                    if (atoSelecionado.TipoPagamento == "CARTÃO CRÉDITO")
                    {
                        MenuItemCartaoCredito.IsChecked = true;
                    }
                    else
                    {
                        MenuItemCartaoCredito.IsChecked = false;
                    }

                    if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                    {
                        MenuItemPago.IsEnabled = true;
                    }
                    else
                    {
                        MenuItemPago.IsEnabled = false;
                    } 
                }
            }
        }

        private void MenuItemDinheiro_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("DINHEIRO"); 
        }

        private void MenuItemDeposito_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("DEPÓSITO");            
        }

        private void MenuItemCheque_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("CHEQUE");
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            ProcExluir();
        }


        private void ProcExluir()
        {
            if (usuarioLogado.Master == true || usuarioLogado.ExcluirAtos == true)
            {
                if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string mensagem = string.Empty;
                    try
                    {
                        ClassAto classAto = new ClassAto();
                        mensagem = classAto.ExcluirAto(atoSelecionado.Id_Ato, Principal.Atribuicao);

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

                    if (mensagem == "Exclusão realizada com sucesso.")
                        MessageBox.Show("Ato excluído com sucesso.", "Excluir", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Ocorreu um erro ao tentar excluir o Ato.", "Excluir", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("O usuário logado não tem permissão para excluir atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            ProcAlterar();
        }

        private void MenuItemNovo_Click(object sender, RoutedEventArgs e)
        {
            WinDigitarAtoRgi AtoRgi = new WinDigitarAtoRgi(Principal, usuarioLogado, "novo", this, dataInicioConsulta, dataFimConsulta);
            AtoRgi.Owner = Principal;
            AtoRgi.ShowDialog();

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;
            ConsultaData();

            dataGrid1.Items.Refresh();
            dataGrid1.SelectedItem = atoSelecionado;
            dataGrid1.ScrollIntoView(atoSelecionado);
        }

        private void dataGrid1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuContext();
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAlterar();
        }

        private void MenuItemPago_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in atosSelecionados)
            {
                if (item.Pago == false)
                {
                    item.Pago = true;

                    ClassAto classAto = new ClassAto();

                    classAto.SalvarAto(item, "alterar");
                }
            }
            dataGrid1.Items.Refresh();
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            var relatorio = new WinRelatorioConsultaProtestoApontamento(listaAtos, datePickerdataConsulta.SelectedDate.Value.ToShortDateString(), datePickerdataConsultaFim.SelectedDate.Value.ToShortDateString(), Principal.TipoAto);
            relatorio.Owner = this;
            relatorio.WindowState = WindowState.Maximized;
            relatorio.WindowStyle = WindowStyle.ToolWindow;
            relatorio.ShowInTaskbar = false;
            relatorio.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            relatorio.ShowDialog();
            relatorio.Close();
        }

        private void MenuItemChequePre_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("PIX BRADESCO");           
        }

        private void MenuItemBoleto_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("PIX NUBANK");
        }


        private void AlterarTipoPagamento(string tipoPagamento)
        {
            foreach (var item in atosSelecionados)
            {
                item.TipoPagamento = tipoPagamento;

                ClassAto classAto = new ClassAto();

                classAto.SalvarAto(item, "alterar");

                var pago = new ValorPago();

                pago = classAto.ObterValorPagoPorIdAto(item.Id_Ato);

                if (pago != null)
                {
                    pago.Deposito = 0M;
                    pago.Mensalista = 0M;
                    pago.Cheque = 0M;
                    pago.ChequePre = 0M;
                    pago.Boleto = 0M;
                    pago.Dinheiro = 0M;
                    pago.CartaoCredito = 0M;
                    pago.DataModificado = DateTime.Now.ToShortDateString();
                    pago.HoraModificado = DateTime.Now.ToLongTimeString();
                    pago.IdUsuario = usuarioLogado.Id_Usuario;
                    pago.NomeUsuario = usuarioLogado.NomeUsu;
                    switch (tipoPagamento)
                    {
                        case "DEPÓSITO":
                            pago.Deposito = pago.Total;
                            break;
                        case "MENSALISTA":
                            pago.Mensalista = pago.Total;
                            break;
                        case "CHEQUE":
                            pago.Cheque = pago.Total;
                            break;
                        case "PIX BRADESCO":
                            pago.ChequePre = pago.Total;
                            break;
                        case "PIX NUBANK":
                            pago.Boleto = pago.Total;
                            break;
                        case "DINHEIRO":
                            pago.Dinheiro = pago.Total;
                            break;
                        case "CARTÃO CRÉDITO":
                            pago.CartaoCredito = pago.Total;
                            break;
                        default:
                            break;
                    }

                    classAto.SalvarValorPago(pago, "alterar", "IdAto");
                }
            }
            dataGrid1.Items.Refresh();
        }

        private void MenuItemCartaoCredito_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("CARTÃO CRÉDITO");
        }
    }
}
