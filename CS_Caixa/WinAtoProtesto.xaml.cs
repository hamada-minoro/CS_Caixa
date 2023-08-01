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
using CS_Caixa.Agragador;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAtoProtesto.xaml
    /// </summary>
    public partial class WinAtoProtesto : Window
    {
        private Models.Usuario usuarioLogado;
        private List<Ato> listaAtos;
        private WinPrincipal Principal;
        Ato atoSelecionado = new Ato();
        Ato atoSelecionadoConvenio = new Ato();
        Ato atoSelecionadoNaoConvenio = new Ato();
        Ato atoSelecionadoAlterado = new Ato();

        List<Ato> atosSelecionados;
        public bool importou = false;
        DateTime dataInicioConsulta;
        DateTime dataFimConsulta;

        public System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

        public WinAtoProtesto()
        {
            InitializeComponent();
        }

        public WinAtoProtesto(Usuario usuarioLogado, WinPrincipal winPrincipal, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            // TODO: Complete member initialization
            this.usuarioLogado = usuarioLogado;
            this.Principal = winPrincipal;
            this.dataInicioConsulta = dataInicioConsulta;
            this.dataFimConsulta = dataFimConsulta;
            InitializeComponent();
        }

        public WinAtoProtesto(Usuario usuarioLogado, WinPrincipal Principal, Ato atoSelecionado, DateTime dataInicioConsulta, DateTime dataFimConsulta)
        {
            this.usuarioLogado = usuarioLogado;
            this.Principal = Principal;
            this.atoSelecionadoAlterado = atoSelecionado;
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

            if (Principal.TipoAto == "CERTIDÃO PROTESTO" || Principal.TipoAto == "CANCELAMENTO")
            {
                colRecibo.Visibility = Visibility.Visible;
                colProtocolo.Visibility = Visibility.Hidden;
                colPortador.Visibility = Visibility.Hidden;
                colVrTitulo.Visibility = Visibility.Hidden;

                colRecibo1.Visibility = Visibility.Visible;
                colProtocolo1.Visibility = Visibility.Hidden;
                colPortador1.Visibility = Visibility.Hidden;
                colVrTitulo1.Visibility = Visibility.Hidden;

                colRecibo2.Visibility = Visibility.Visible;
                colProtocolo2.Visibility = Visibility.Hidden;
                colPortador2.Visibility = Visibility.Hidden;
                colVrTitulo2.Visibility = Visibility.Hidden;

                if (Principal.TipoAto == "CANCELAMENTO")
                {
                    colConvenio.Visibility = Visibility.Visible;
                    colConvenio1.Visibility = Visibility.Visible;
                    colConvenio2.Visibility = Visibility.Visible;
                    colProtocolo.Visibility = Visibility.Visible;
                    colProtocolo1.Visibility = Visibility.Visible;
                    colProtocolo2.Visibility = Visibility.Visible;
                }

                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    colConvenio.Visibility = Visibility.Hidden;
                    colConvenio1.Visibility = Visibility.Hidden;
                    colConvenio2.Visibility = Visibility.Hidden;

                    tabItemConvenio.Header = "Certidão Serasa";
                    tabItemNaoConvenio.Header = "Certidão Boa Vista";
                }
                colFaixa.Visibility = Visibility.Hidden;
                colTpCustas.Visibility = Visibility.Visible;
                colTpPagamento.Visibility = Visibility.Visible;

                colFaixa1.Visibility = Visibility.Hidden;
                colTpCustas1.Visibility = Visibility.Visible;
                colTpPagamento1.Visibility = Visibility.Visible;

                colFaixa2.Visibility = Visibility.Hidden;
                colTpCustas2.Visibility = Visibility.Visible;
                colTpPagamento2.Visibility = Visibility.Visible;

                cmbTpConsulta.Items.Add("RECIBO");
            }

            if (Principal.TipoAto == "APONTAMENTO")
            {
                
                btnImportar.Visibility = Visibility.Visible;
                gridEnviar.Visibility = Visibility.Visible;

                colRecibo.Visibility = Visibility.Visible;
                colProtocolo.Visibility = Visibility.Visible;
                colPortador.Visibility = Visibility.Visible;
                colVrTitulo.Visibility = Visibility.Visible;
                colConvenio.Visibility = Visibility.Visible;
                colFaixa.Visibility = Visibility.Visible;
                colTpCustas.Visibility = Visibility.Hidden;
                colTpPagamento.Visibility = Visibility.Hidden;

                colRecibo1.Visibility = Visibility.Visible;
                colProtocolo1.Visibility = Visibility.Visible;
                colPortador1.Visibility = Visibility.Visible;
                colVrTitulo1.Visibility = Visibility.Visible;
                colConvenio1.Visibility = Visibility.Visible;
                colFaixa1.Visibility = Visibility.Visible;
                colTpCustas1.Visibility = Visibility.Hidden;
                colTpPagamento1.Visibility = Visibility.Hidden;

                colRecibo2.Visibility = Visibility.Visible;
                colProtocolo2.Visibility = Visibility.Visible;
                colPortador2.Visibility = Visibility.Visible;
                colVrTitulo2.Visibility = Visibility.Visible;
                colConvenio2.Visibility = Visibility.Visible;
                colFaixa2.Visibility = Visibility.Visible;
                colTpCustas2.Visibility = Visibility.Hidden;
                colTpPagamento2.Visibility = Visibility.Hidden;

                cmbTpConsulta.Items.Add("PROTOCOLO");
                cmbTpConsulta.Items.Add("RECIBO");
            }

            if (Principal.TipoAto == "PAGAMENTO")
            {
                
                btnImportar.Visibility = Visibility.Visible;

                colRecibo.Visibility = Visibility.Hidden;
                colProtocolo.Visibility = Visibility.Visible;
                colPortador.Visibility = Visibility.Visible;
                colVrTitulo.Visibility = Visibility.Visible;
                colConvenio.Visibility = Visibility.Visible;
                colFaixa.Visibility = Visibility.Visible;
                colTpCustas.Visibility = Visibility.Hidden;
                colTpPagamento.Visibility = Visibility.Hidden;

                colRecibo1.Visibility = Visibility.Hidden;
                colProtocolo1.Visibility = Visibility.Visible;
                colPortador1.Visibility = Visibility.Visible;
                colVrTitulo1.Visibility = Visibility.Visible;
                colConvenio1.Visibility = Visibility.Visible;
                colFaixa1.Visibility = Visibility.Visible;
                colTpCustas1.Visibility = Visibility.Hidden;
                colTpPagamento1.Visibility = Visibility.Hidden;

                colRecibo2.Visibility = Visibility.Hidden;
                colProtocolo2.Visibility = Visibility.Visible;
                colPortador2.Visibility = Visibility.Visible;
                colVrTitulo2.Visibility = Visibility.Visible;
                colConvenio2.Visibility = Visibility.Visible;
                colFaixa2.Visibility = Visibility.Visible;
                colTpCustas2.Visibility = Visibility.Hidden;
                colTpPagamento2.Visibility = Visibility.Hidden;

                colRecibo3.Visibility = Visibility.Hidden;
                colProtocolo3.Visibility = Visibility.Visible;
                colPortador3.Visibility = Visibility.Visible;
                colVrTitulo3.Visibility = Visibility.Visible;
                colConvenio3.Visibility = Visibility.Visible;
                colFaixa3.Visibility = Visibility.Visible;
                colTpCustas3.Visibility = Visibility.Hidden;
                colTpPagamento3.Visibility = Visibility.Hidden;

                tabItemEmissaoDeGuia.Visibility = Visibility.Visible;
            }

            var bc = new BrushConverter();

            if (Principal.TipoAto == "APONTAMENTO")
                this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFD3E9EF");

            if (Principal.TipoAto == "CANCELAMENTO")
                this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFE2E0BA");

            if (Principal.TipoAto == "PAGAMENTO")
                this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FF7FD3C0");

            if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                this.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#FFEBDED0");
            
            if (dataGrid1.Items.Count > 0)
            {
                if (atoSelecionadoAlterado != null && atoSelecionadoAlterado.Id_Ato > 0)
                {
                    atoSelecionado = listaAtos.Where(p => p.Id_Ato == atoSelecionadoAlterado.Id_Ato).FirstOrDefault();
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

            dataGridConvenio.SelectedIndex = 0;
            dataGridNaoConvenio.SelectedIndex = 0;
            dataGridEmissaoGuia.SelectedIndex = 0;
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
                    case 2:
                        ConsultaRecibo();
                        break;
                }
            }

            if (dataGrid1.Items.Count > 0 && cmbTpConsulta.SelectedIndex == 0)
                btnImprimir.IsEnabled = true;
        }

        private void ConsultaData()
        {
            try
            {
                tabItemTodos.IsSelected = true;
                
                ClassAto classAto = new ClassAto();
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

                dataGrid1.ItemsSource = null;
                dataGrid1.ItemsSource = listaAtos;
                
                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    dataGridConvenio.ItemsSource = null;
                    dataGridNaoConvenio.ItemsSource = null;
                    dataGridConvenio.ItemsSource = listaAtos.Where(p => p.Natureza == "CERTIDÃO SERASA");
                    dataGridNaoConvenio.ItemsSource = listaAtos.Where(p => p.Natureza == "CERTIDÃO BOA VISTA");
                    VerificaTabControl();
                    VerificarDataGrid();
                }
                else
                {
                    dataGridConvenio.ItemsSource = null;
                    dataGridNaoConvenio.ItemsSource = null;
                    dataGridConvenio.ItemsSource = listaAtos.Where(p => p.Convenio == "S");
                    dataGridNaoConvenio.ItemsSource = listaAtos.Where(p => p.Convenio == "N");
                    
                    if (Principal.TipoAto == "PAGAMENTO")
                    {
                        dataGrid1.ItemsSource = null;
                        dataGrid1.ItemsSource = listaAtos.Where(p => p.TipoAto == "PAGAMENTO");
                        dataGridConvenio.ItemsSource = null;
                        dataGridNaoConvenio.ItemsSource = null;
                        dataGridConvenio.ItemsSource = listaAtos.Where(p => p.TipoAto == "PAGAMENTO" && p.Convenio == "S");
                        dataGridNaoConvenio.ItemsSource = listaAtos.Where(p => p.TipoAto == "PAGAMENTO" && p.Convenio == "N");
                        dataGridEmissaoGuia.ItemsSource = null;
                        dataGridEmissaoGuia.ItemsSource = listaAtos.Where(p => p.TipoAto == "EMISSÃO DE GUIA");
                    }
                    VerificaTabControl();
                    VerificarDataGrid();
                }

                if (dataGrid1.Items.Count > 0)
                {
                    dataGrid1.SelectedIndex = 0;
                    dataGridConvenio.SelectedIndex = 0;
                    dataGridNaoConvenio.SelectedIndex = 0;

                    if (Principal.TipoAto == "PAGAMENTO")
                        dataGridEmissaoGuia.SelectedIndex = 0;
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
            {
                dataGrid1.ItemsSource = classAto.ListarAtoProtocolo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto);


                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoProtocolo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Natureza == "CERTIDÃO SERASA");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoProtocolo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Natureza == "CERTIDÃO BOA VISTA");
                }
                else
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoProtocolo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Convenio == "S");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoProtocolo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Convenio == "N");
                }
            }
            else
            {
                dataGrid1.ItemsSource = classAto.ListarAtoProtocoloNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario);

                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoProtocoloNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Natureza == "CERTIDÃO SERASA");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoProtocoloNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Natureza == "CERTIDÃO BOA VISTA");
                }
                else
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoProtocoloNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Convenio == "S");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoProtocoloNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Convenio == "N");
                }
            }
            dataGrid1.Items.Refresh();
        }


        private void ConsultaRecibo()
        {
            ClassAto classAto = new ClassAto();

            if (txtConsulta.Text == "")
            {
                MessageBox.Show("Informe o Número do Protocolo.", "Protocolo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
            {
                dataGrid1.ItemsSource = classAto.ListarAtoRecibo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto);


                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoRecibo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Natureza == "CERTIDÃO SERASA");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoRecibo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Natureza == "CERTIDÃO BOA VISTA");
                }
                else
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoRecibo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Convenio == "S");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoRecibo(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto).Where(p => p.Convenio == "N");
                }
            }
            else
            {
                dataGrid1.ItemsSource = classAto.ListarAtoReciboNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario);

                if (Principal.TipoAto == "CERTIDÃO PROTESTO")
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoReciboNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Natureza == "CERTIDÃO SERASA");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoReciboNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Natureza == "CERTIDÃO BOA VISTA");
                }
                else
                {
                    dataGridConvenio.ItemsSource = classAto.ListarAtoReciboNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Convenio == "S");
                    dataGridNaoConvenio.ItemsSource = classAto.ListarAtoReciboNomeEscrevente(Convert.ToInt32(txtConsulta.Text), Principal.TipoAto, usuarioLogado.Id_Usuario).Where(p => p.Convenio == "N");
                }
            }
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
                    btnImprimir.IsEnabled = false;
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
            ProcExcluir();
        }

        private void VerificarDataGrid()
        {
            if (tabControl.SelectedIndex == 0)
            {
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
            }

            if (tabControl.SelectedIndex == 1)
            {
                if (dataGridConvenio.Items.Count > 0)
                {
                    btnAlterar.IsEnabled = true;
                    btnExcluir.IsEnabled = true;
                }
                else
                {
                    btnAlterar.IsEnabled = false;
                    btnExcluir.IsEnabled = false;
                }
            }
            
            if (tabControl.SelectedIndex == 2)
            {
                if (dataGridNaoConvenio.Items.Count > 0)
                {
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


        private void ProcExcluir()
        {
            if (atoSelecionado == null)
            {
                MessageBox.Show("Selecione um ato para excluir.", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (usuarioLogado.Master == true || usuarioLogado.ExcluirAtos == true)
            {
                string mensagem = string.Empty;
                ClassAto classAto = new ClassAto();

                List<Ato> atosExcluir = new List<Ato>();

                if (atoSelecionado.Protocolo != null)
                    atosExcluir = classAto.ListarAtosPorProtocoloIgualRecibo(Convert.ToInt32(atoSelecionado.Protocolo), atoSelecionado.DataAto);

                if (dataGrid1.Focus())
                    if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            if (atosExcluir.Count > 1)
                                foreach (var item in atosExcluir)
                                {
                                    mensagem = classAto.ExcluirAto(item.Id_Ato, Principal.Atribuicao);
                                }
                            else
                            {
                                mensagem = classAto.ExcluirAto(atoSelecionado.Id_Ato, Principal.Atribuicao);
                            }

                            ConsultaData();

                            dataGrid1.Items.Refresh();
                            dataGrid1.SelectedIndex = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(mensagem + " " + ex.Message);
                        }
                    }


                if (dataGridConvenio.Focus())
                    if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {

                        try
                        {
                            if (atosExcluir.Count > 1)
                                foreach (var item in atosExcluir)
                                {
                                    mensagem = classAto.ExcluirAto(item.Id_Ato, Principal.Atribuicao);
                                }
                            else
                            {
                                mensagem = classAto.ExcluirAto(atoSelecionado.Id_Ato, Principal.Atribuicao);
                            }


                            ConsultaData();


                            dataGridConvenio.Items.Refresh();
                            dataGridConvenio.SelectedIndex = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(mensagem + " " + ex.Message);
                        }

                    }


                if (dataGridNaoConvenio.Focus())
                    if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {

                        try
                        {
                            if (atosExcluir.Count > 1)
                                foreach (var item in atosExcluir)
                                {
                                    mensagem = classAto.ExcluirAto(item.Id_Ato, Principal.Atribuicao);
                                }
                            else
                            {
                                mensagem = classAto.ExcluirAto(atoSelecionado.Id_Ato, Principal.Atribuicao);
                            }


                            ConsultaData();


                            dataGridNaoConvenio.Items.Refresh();
                            dataGridNaoConvenio.SelectedIndex = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(mensagem + " " + ex.Message);
                        }

                    }

                if (dataGridEmissaoGuia.Focus())
                    if (MessageBox.Show("Deseja realmente excluir este Ato?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {

                        try
                        {
                            var excluir = atosExcluir.Where(p => p.TipoAto == "EMISSÃO DE GUIA").ToList();


                            if (excluir.Count > 1)
                                foreach (var item in atosExcluir)
                                {
                                    mensagem = classAto.ExcluirAto(item.Id_Ato, Principal.Atribuicao);
                                }
                            else
                            {
                                mensagem = classAto.ExcluirAto(atoSelecionado.Id_Ato, Principal.Atribuicao);
                            }


                            ConsultaData();


                            dataGridEmissaoGuia.Items.Refresh();
                            dataGridEmissaoGuia.SelectedIndex = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(mensagem + " " + ex.Message);
                        }

                    }

                VerificarDataGrid();

                if (mensagem == "Exclusão realizada com sucesso.")
                    MessageBox.Show("Ato excluído com sucesso.", "Excluir", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    if (mensagem == "")
                        MessageBox.Show("Exclusão cancelada pelo usuário.", "Excluir", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                        MessageBox.Show("Ocorreu um erro ao tentar excluir o Ato.", "Excluir", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("O usuário logado não tem permissão para excluir atos.", "Permissão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ProcAlterar()
        {

            //if (atoSelecionado.Faixa == "*" || atoSelecionado.DescricaoAto == "I")
            //{
            //    MessageBox.Show("Atos importados não podem ser alterados. Exclua e importe novamente.", "Acesso negado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return;
            //}


            if (usuarioLogado.Master == true || usuarioLogado.AlterarAtos == true)
            {
                if (dataGrid1.Focus())
                    if (atoSelecionado != null && atoSelecionado.Id_Ato != 0)
                    {

                        WinDigitarAtoProtesto digAto = new WinDigitarAtoProtesto(Principal, usuarioLogado, "alterar", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);

                        digAto.Owner = Principal;
                        digAto.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("Selecione um item.", "Seleção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }


                if (dataGridConvenio.Focus())
                    if (atoSelecionado != null && atoSelecionado.Id_Ato != 0)
                    {

                        WinDigitarAtoProtesto digAto = new WinDigitarAtoProtesto(Principal, usuarioLogado, "alterar", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);

                        digAto.Owner = Principal;
                        digAto.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Selecione um item.", "Seleção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }


                if (dataGridNaoConvenio.Focus())
                    if (atoSelecionado != null && atoSelecionado.Id_Ato != 0)
                    {

                        WinDigitarAtoProtesto digAto = new WinDigitarAtoProtesto(Principal, usuarioLogado, "alterar", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);

                        digAto.Owner = Principal;
                        digAto.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Selecione um item.", "Seleção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }


                dataGrid1.Items.Refresh();
                dataGridNaoConvenio.Items.Refresh();
                dataGridConvenio.Items.Refresh();
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

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            WinDigitarAtoProtesto DigProtesto = new WinDigitarAtoProtesto(Principal, usuarioLogado, "novo", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
            DigProtesto.Owner = Principal;
            DigProtesto.ShowDialog();

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;

            ConsultaData();
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

        private void SelecionarAto()
        {
            //if (dataGrid1.Focus())
            //{

            //    if (dataGrid1.Items.Count > 0)
            //    {

            //        atoSelecionado = (Ato)dataGrid1.SelectedItem;

            //        //MenuContext();

            //        atosSelecionados = (List<Ato>)dataGrid1.SelectedItems.Cast<Ato>().ToList();

            //        lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


            //    }
            //}


            //if (dataGridConvenio.Focus())
            //{

            //    if (dataGridConvenio.Items.Count > 0)
            //    {

            //        atoSelecionado = (Ato)dataGridConvenio.SelectedItem;

            //        MenuContext();

            //        atosSelecionados = (List<Ato>)dataGridConvenio.SelectedItems.Cast<Ato>().ToList();

            //        lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


            //    }
            //}


            //if (dataGridNaoConvenio.Focus())
            //{

            //    if (dataGridNaoConvenio.Items.Count > 0)
            //    {

            //        atoSelecionado = (Ato)dataGridNaoConvenio.SelectedItem;

            //        MenuContext();

            //        atosSelecionados = (List<Ato>)dataGridNaoConvenio.SelectedItems.Cast<Ato>().ToList();

            //        lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


            //    }
            //}
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (dataGrid1.SelectedItem != null)
                {
                    atoSelecionado = (Ato)dataGrid1.SelectedItem;

                    MenuContext();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            ProcExcluir();
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {

            ProcAlterar();
        }

        private void MenuItemNovo_Click(object sender, RoutedEventArgs e)
        {
            WinDigitarAtoProtesto DigProtesto = new WinDigitarAtoProtesto(Principal, usuarioLogado, "novo", atoSelecionado, datePickerdataConsulta.SelectedDate.Value, datePickerdataConsultaFim.SelectedDate.Value);
            DigProtesto.Owner = Principal;
            DigProtesto.ShowDialog();

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;

            ConsultaData();
        }


        private void AlterarTipoPagamento(string tipoPagamento)
        {
            ClassAto classAto = new ClassAto();

            List<Ato> atosAlterar = new List<Ato>();

            foreach (var item0 in atosSelecionados)
            {
                if (item0.Protocolo != null)
                    atosAlterar = classAto.ListarAtosPorProtocoloIgualRecibo(Convert.ToInt32(atoSelecionado.Protocolo), atoSelecionado.DataAto);

                if (atosAlterar.Count > 1)
                {
                    foreach (var item in atosAlterar)
                    {

                        item.TipoPagamento = tipoPagamento;


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
                }
                else
                {
                    item0.TipoPagamento = tipoPagamento;


                    classAto.SalvarAto(item0, "alterar");

                    var pago = new ValorPago();

                    pago = classAto.ObterValorPagoPorIdAto(item0.Id_Ato);
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


                item0.TipoPagamento = tipoPagamento;
            }
            dataGrid1.Items.Refresh();
            dataGridNaoConvenio.Items.Refresh();
            dataGridConvenio.Items.Refresh();
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



        private void MenuContext()
        {


            if (dataGrid1.Items.Count > 0 && atoSelecionado != null)
            {

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


                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    MenuItemPago.IsEnabled = true;
                }
                else
                {
                    MenuItemPago.IsEnabled = false;
                }


                if (atoSelecionado.TipoAto == "CERTIDÃO PROTESTO" && atoSelecionado.Natureza == "CERTIDÃO SERASA" || atoSelecionado.TipoAto == "CERTIDÃO PROTESTO" && atoSelecionado.Natureza == "CERTIDÃO BOA VISTA")
                {
                    MenuItemRecibo.IsEnabled = true;
                    MenuItemInteiroTeor.IsEnabled = false;
                }
                else
                {
                    MenuItemRecibo.IsEnabled = false;

                    if (atoSelecionado.TipoAto == "CERTIDÃO PROTESTO")
                        MenuItemInteiroTeor.IsEnabled = true;
                }
            }

            if (dataGridConvenio.Items.Count > 0 && atoSelecionado != null)
            {

                if (atoSelecionado.TipoPagamento == "DINHEIRO")
                {

                    MenuItemDinheiro1.IsChecked = true;
                }
                else
                {
                    MenuItemDinheiro1.IsChecked = false;
                }
                if (atoSelecionado.TipoPagamento == "DEPÓSITO")
                {
                    MenuItemDeposito1.IsChecked = true;
                }
                else
                {
                    MenuItemDeposito1.IsChecked = false;
                }

                if (atoSelecionado.TipoPagamento == "MENSALISTA")
                {
                    MenuItemMensalista1.IsChecked = true;
                }
                else
                {
                    MenuItemMensalista1.IsChecked = false;
                }

                if (atoSelecionado.TipoPagamento == "CHEQUE")
                {
                    MenuItemCheque1.IsChecked = true;
                }
                else
                {
                    MenuItemCheque1.IsChecked = false;
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
                    MenuItemCartaoCredito1.IsChecked = true;
                }
                else
                {
                    MenuItemCartaoCredito1.IsChecked = false;
                }

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    MenuItemPago1.IsEnabled = true;
                }
                else
                {
                    MenuItemPago1.IsEnabled = false;
                }

                if (atoSelecionado.TipoAto == "CERTIDÃO PROTESTO" && atoSelecionado.Natureza == "CERTIDÃO SERASA" || atoSelecionado.TipoAto == "CERTIDÃO PROTESTO" && atoSelecionado.Natureza == "CERTIDÃO BOA VISTA")
                {
                    MenuItemRecibo1.IsEnabled = true;
                    MenuItemInteiroTeor.IsEnabled = false;
                }
                else
                {
                    MenuItemRecibo1.IsEnabled = false;

                    if (atoSelecionado.TipoAto == "CERTIDÃO PROTESTO")
                        MenuItemInteiroTeor.IsEnabled = true;
                }
            }

            if (dataGridNaoConvenio.Items.Count > 0 && atoSelecionado != null)
            {


                if (atoSelecionado.TipoPagamento == "DINHEIRO")
                {

                    MenuItemDinheiro2.IsChecked = true;
                }
                else
                {
                    MenuItemDinheiro2.IsChecked = false;
                }
                if (atoSelecionado.TipoPagamento == "DEPÓSITO")
                {
                    MenuItemDeposito2.IsChecked = true;
                }
                else
                {
                    MenuItemDeposito2.IsChecked = false;
                }

                if (atoSelecionado.TipoPagamento == "MENSALISTA")
                {
                    MenuItemMensalista2.IsChecked = true;
                }
                else
                {
                    MenuItemMensalista2.IsChecked = false;
                }

                if (atoSelecionado.TipoPagamento == "CHEQUE")
                {
                    MenuItemCheque2.IsChecked = true;
                }
                else
                {
                    MenuItemCheque2.IsChecked = false;
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
                    MenuItemCartaoCredito2.IsChecked = true;
                }
                else
                {
                    MenuItemCartaoCredito2.IsChecked = false;
                }

                if (usuarioLogado.Master == true || usuarioLogado.Caixa == true)
                {
                    MenuItemPago2.IsEnabled = true;
                }
                else
                {
                    MenuItemPago2.IsEnabled = false;
                }


                if (atoSelecionado.TipoAto == "CERTIDÃO PROTESTO" && atoSelecionado.Natureza == "CERTIDÃO SERASA" || atoSelecionado.TipoAto == "CERTIDÃO PROTESTO" && atoSelecionado.Natureza == "CERTIDÃO BOA VISTA")
                {
                    MenuItemRecibo2.IsEnabled = true;
                    MenuItemInteiroTeor.IsEnabled = false;
                }
                else
                {
                    MenuItemRecibo2.IsEnabled = false;

                    if (atoSelecionado.TipoAto == "CERTIDÃO PROTESTO")
                        MenuItemInteiroTeor.IsEnabled = true;
                }
            }
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
            dataGridNaoConvenio.Items.Refresh();
            dataGridConvenio.Items.Refresh();
        }

        private void MenuItemRecibo_Click(object sender, RoutedEventArgs e)
        {
            //atoSelecionado = (Ato)dataGrid1.SelectedItem;
            VerificaTabControl();
            FrmRecibo frmRecibo = new FrmRecibo(atoSelecionado.Recibo.ToString(), atoSelecionado.Natureza, atoSelecionado.DataAto.ToShortDateString(), atoSelecionado.Faixa, string.Format("{0:n2}", atoSelecionado.Emolumentos), string.Format("{0:n2}", atoSelecionado.Fetj), string.Format("{0:n2}", atoSelecionado.Funperj), string.Format("{0:n2}", atoSelecionado.Funarpen), string.Format("{0:n2}", atoSelecionado.Pmcmv), string.Format("{0:n2}", atoSelecionado.Iss), string.Format("{0:n2}", atoSelecionado.Total), atoSelecionado.Escrevente);
            frmRecibo.ShowDialog();
            frmRecibo.Dispose();
        }


        private void VerificaTabControl()
        {
            if (tabControl.SelectedIndex == 0)
            {

                atoSelecionado = (Ato)dataGrid1.SelectedItem;

                atosSelecionados = (List<Ato>)dataGrid1.SelectedItems.Cast<Ato>().ToList();

                lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


                lblTotalSelecionadoTarifa.Content = "";

                lblTotalSelecionadoApontamento.Content = "";

            }

            if (tabControl.SelectedIndex == 1)
            {

                atoSelecionado = (Ato)dataGridConvenio.SelectedItem;

                atosSelecionados = (List<Ato>)dataGridConvenio.SelectedItems.Cast<Ato>().ToList();

                lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


                lblTotalSelecionadoTarifa.Content = "";

                lblTotalSelecionadoApontamento.Content = "";

            }

            if (tabControl.SelectedIndex == 2)
            {

                atoSelecionado = (Ato)dataGridNaoConvenio.SelectedItem;

                atosSelecionados = (List<Ato>)dataGridNaoConvenio.SelectedItems.Cast<Ato>().ToList();

                lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


                lblTotalSelecionadoTarifa.Content = "";

                lblTotalSelecionadoApontamento.Content = "";

            }

            if (tabControl.SelectedIndex == 3)
            {

                atoSelecionado = (Ato)dataGridEmissaoGuia.SelectedItem;

                atosSelecionados = (List<Ato>)dataGridEmissaoGuia.SelectedItems.Cast<Ato>().ToList();

                lblTotalSelecionado.Content = string.Format("Valor Total dos Ítens Selecionados: {0:n2}", atosSelecionados.Sum(p => p.Total));


                lblTotalSelecionado.Content = string.Format("Total Tarifas: {0:n2}", atosSelecionados.Sum(p => p.Bancaria));

                lblTotalSelecionadoTarifa.Content = string.Format("Total Emissão de Guia: {0:n2}", atosSelecionados.Sum(p => p.Emolumentos + p.Fetj + p.Fundperj + p.Funperj + p.Funarpen + p.Iss));

                lblTotalSelecionadoApontamento.Content = string.Format("Total: {0:n2}", atosSelecionados.Sum(p => p.Total));


            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            VerificaTabControl();

            VerificarDataGrid();
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

        private void dataGridConvenio_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuContext();
        }

        private void dataGridConvenio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridConvenio.SelectedItem != null)
                {
                    atoSelecionadoConvenio = (Ato)dataGridConvenio.SelectedItem;

                    MenuContext();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridConvenio_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAlterar();
        }

        private void dataGridNaoConvenio_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuContext();
        }

        private void dataGridNaoConvenio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridNaoConvenio.SelectedItem != null)
                {
                    atoSelecionadoNaoConvenio = (Ato)dataGridNaoConvenio.SelectedItem;

                    MenuContext();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridNaoConvenio_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAlterar();
        }

        private void MenuItemChequePre_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("PIX BRADESCO");

        }

        private void MenuItemBoleto_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("PIX NUBANK");

        }

        private void MenuItemInteiroTeor_Click(object sender, RoutedEventArgs e)
        {

            if (atoSelecionado == null)
                return;


            TituloProtesto titulo = new TituloProtesto();

            titulo.RECIBO = Convert.ToInt32(atoSelecionado.Recibo);


            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {

                try
                {
                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from certidoes where recibo = " + titulo.RECIBO);

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr;

                        dr = cmdTotal.ExecuteReader();
                        while (dr.Read())
                        {
                            titulo.ID_ATO = Convert.ToInt32(dr["ID_ATO"]);
                            titulo.DT_PROTOCOLO = Convert.ToDateTime(dr["DT_CERTIDAO"]);
                            titulo.EMOLUMENTOS = Convert.ToDecimal(dr["EMOLUMENTOS"]);
                            titulo.FETJ = Convert.ToDecimal(dr["FETJ"]);
                            titulo.FUNDPERJ = Convert.ToDecimal(dr["FUNDPERJ"]);
                            titulo.FUNPERJ = Convert.ToDecimal(dr["FUNPERJ"]);
                            titulo.FUNARPEN = Convert.ToDecimal(dr["FUNARPEN"]);
                            titulo.PMCMV = Convert.ToDecimal(dr["PMCMV"]);
                            titulo.ISS = Convert.ToDecimal(dr["ISS"]);
                            titulo.MUTUA = Convert.ToDecimal(dr["MUTUA"]);
                            titulo.ACOTERJ = Convert.ToDecimal(dr["ACOTERJ"]);
                            titulo.DISTRIBUICAO = Convert.ToDecimal(dr["DISTRIBUICAO"]);
                            titulo.SELO_REGISTRO = dr["SELO"].ToString();
                            titulo.ALEATORIO_PROTESTO = dr["ALEATORIO"].ToString();
                            titulo.TOTAL = Convert.ToDecimal(dr["TOTAL"]);
                        }
                    }

                    string comando2 = string.Empty;

                    comando2 = string.Format("select * from titulos where id_ato = " + titulo.ID_ATO);

                    using (FbCommand cmdTotal = new FbCommand(comando2, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr2;

                        dr2 = cmdTotal.ExecuteReader();
                        while (dr2.Read())
                        {
                            if (dr2["LIVRO_REGISTRO"].ToString() != "")
                                titulo.LIVRO_REGISTRO = Convert.ToInt32(dr2["LIVRO_REGISTRO"]);

                            titulo.FOLHA_REGISTRO = dr2["FOLHA_REGISTRO"].ToString();
                            titulo.PROTOCOLO = Convert.ToInt32(dr2["PROTOCOLO"]);
                            titulo.DT_REGISTRO = Convert.ToDateTime(dr2["DT_REGISTRO"]);
                        }
                    }


                    string texto;

                    if (titulo != null)
                    {
                        string selos;

                        selos = titulo.SELO_REGISTRO + titulo.ALEATORIO_PROTESTO;




                        texto = string.Format("SELO={0}|SERV=1823|TIPO=P|DATA={1}", selos, titulo.DT_PROTOCOLO.ToShortDateString());

                        Bitmap qrCode = GerarQRCode(texto);

                        string nomeArquivo = @"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\QrCode\Protesto\Livro" + titulo.LIVRO_REGISTRO + "_Folha" + titulo.FOLHA_REGISTRO + "_Prot" + titulo.PROTOCOLO + "_" + titulo.SELO_REGISTRO + titulo.ALEATORIO_PROTESTO + ".bmp";

                        FileInfo arquivo = new FileInfo(nomeArquivo);

                        FileInfo arquivoRemover = new FileInfo(nomeArquivo);

                        DirectoryInfo diretorio = new DirectoryInfo(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\QrCode\Protesto");

                        if (!diretorio.Exists)
                            diretorio.Create();

                        if (arquivo.Exists)
                            arquivo.Delete();

                        qrCode.Save(arquivo.FullName, System.Drawing.Imaging.ImageFormat.Bmp);

                        WinRelatorioImprimirQrCode imprimir = new WinRelatorioImprimirQrCode(arquivo.FullName, titulo, atoSelecionado.Escrevente);
                        imprimir.Owner = this;
                        imprimir.ShowDialog();



                        if (arquivoRemover.Exists)
                            arquivoRemover.Delete();

                    }



                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível gerar a Certidão de Inteiro Teor.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public Bitmap GerarQRCode(string text)
        {
            try
            {
                QRCodeEncoder qrCodecEncoder = new QRCodeEncoder();
                qrCodecEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
                qrCodecEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                qrCodecEncoder.CharacterSet = "UTF-8";
                qrCodecEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodecEncoder.QRCodeScale = 6;
                qrCodecEncoder.QRCodeVersion = 0;
                qrCodecEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                Bitmap imageQRCode;

                imageQRCode = new Bitmap(qrCodecEncoder.Encode(text), 90, 90);


                return imageQRCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {

            importou = false;

            WinImportarProtesto import = new WinImportarProtesto(Principal, this);
            import.Owner = this;

            import.ShowDialog();

            if (importou)
                if (MessageBox.Show("Deseja enviar as atualizações para o site do cartório? \n\n É recomendado que mantenha o site atualizado.", "Atualização do Site", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    WinAguardeEnviarSite enviar = new WinAguardeEnviarSite(Principal._dataSistema, true);
                    enviar.Owner = this;
                    enviar.ShowDialog();
                }

            datePickerdataConsulta.SelectedDate = dataInicioConsulta;
            datePickerdataConsultaFim.SelectedDate = dataFimConsulta;
            cmbTpConsulta.SelectedIndex = 0;
            ConsultaData();
        }

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog1.Filter = "All files (*.pdf)|*.pdf";

            openFileDialog1.InitialDirectory = @"\\SERVIDOR\Total\Protesto\BOLETOS";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCaminho.Text = openFileDialog1.FileName;

                try
                {

                    if (!string.IsNullOrWhiteSpace(txtCaminho.Text))
                    {

                        if (VerificarExistente())
                        {
                            DeletarExistente();
                        }

                        DataTable excluir = VerificarMaisCincoDias();

                        if (excluir.Rows.Count > 0)
                            DeletarMaisCincoDias(excluir);

                        SalvarArquivo(txtCaminho.Text);


                        FileInfo arquivoEnviado = new FileInfo(openFileDialog1.FileName);

                        MessageBox.Show("Arquivo " + arquivoEnviado.Name.Replace(".pdf", "") + " enviado com sucesso.\n\n Para visualizar acesse: 1oficioararuama.com.br/boleto", "Enviado com Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                txtCaminho.Text = "";
        }



        private bool VerificarExistente()
        {
            bool protocolo = false;

            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "select Protocolo from Boletos where Protocolo = '" + System.IO.Path.GetFileName(txtCaminho.Text).Replace(".pdf", "") + "'";

                    var reader = comm.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    if (table.Rows.Count > 0)
                        return true;

                }
            }


            return protocolo;
        }

        private DataTable VerificarMaisCincoDias()
        {
            string data = string.Empty;
            string ano = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.AddDays(-5).Day.ToString();

            data = string.Format("{0}-{1}-{2}", ano, mes, dia);

            var table = new DataTable();
            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "select * from Boletos where data_envio <= '" + data + "'";

                    var reader = comm.ExecuteReader();

                    table.Load(reader);
                    if (table.Rows.Count > 0)
                        return table;

                }
            }

            return table;
        }


        private void DeletarMaisCincoDias(DataTable excluir)
        {

            for (int i = 0; i < excluir.Rows.Count; i++)
            {
                using (var conn = AbrirConexao())
                {
                    conn.Open();
                    using (var comm = conn.CreateCommand())
                    {
                        comm.CommandText = "delete from Boletos where Protocolo = '" + excluir.Rows[i]["Protocolo"].ToString() + "'";

                        comm.ExecuteNonQuery();

                    }
                }
            }

        }

        private void DeletarExistente()
        {


            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "delete from Boletos where Protocolo = '" + System.IO.Path.GetFileName(txtCaminho.Text).Replace(".pdf", "") + "'";

                    comm.ExecuteNonQuery();

                }
            }

        }


        private IDbConnection AbrirConexao()
        {
            return new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
        }

        private void SalvarArquivo(string arquivo)
        {
            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "INSERT INTO Boletos (Protocolo, Arquivo, data_envio, Visualizado, QtdVisualizacao) VALUES (@Protocolo, @Arquivo, @data_envio, @Visualizado, 0)";
                    ConfigurarParametrosSalvar(comm, arquivo);
                    comm.ExecuteNonQuery();
                }
            }
        }
        private void ConfigurarParametrosSalvar(IDbCommand comm, string arquivo)
        {
            comm.Parameters.Add(new MySqlParameter("Protocolo", System.IO.Path.GetFileName(arquivo).Replace(".pdf", "")));
            comm.Parameters.Add(new MySqlParameter("Arquivo", File.ReadAllBytes(arquivo)));
            comm.Parameters.Add(new MySqlParameter("data_envio", DateTime.Now.Date));
            comm.Parameters.Add(new MySqlParameter("Visualizado", false));
        }

        private void MenuItemExcluirEmissao_Click(object sender, RoutedEventArgs e)
        {
            ProcExcluir();

        }

        private void dataGridEmissaoGuia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridEmissaoGuia.SelectedItem != null)
                {
                    atoSelecionado = (Ato)dataGrid1.SelectedItem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItemCartaoCredito_Click(object sender, RoutedEventArgs e)
        {
            AlterarTipoPagamento("CARTÃO CRÉDITO");
        }



    }
}
