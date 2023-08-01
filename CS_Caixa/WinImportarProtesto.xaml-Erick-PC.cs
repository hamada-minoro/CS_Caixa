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
using CS_Caixa.Models;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.ComponentModel;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinImportarProtesto.xaml
    /// </summary>
    public partial class WinImportarProtesto : Window
    {
        WinPrincipal Principal;
        DataTable dtTotal = new DataTable();

        List<Ato> listaRetorno = new List<Ato>();
        ClassAto classAto = new ClassAto();
        WinAtoProtesto _winAtoProtesto;

        public WinImportarProtesto(WinPrincipal Principal, WinAtoProtesto winAtoProtesto)
        {
            this.Principal = Principal;
            _winAtoProtesto = winAtoProtesto;
            InitializeComponent();
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
                    txtConsulta.Focus();
                }
                txtConsulta.Text = "";

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

            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void grid1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void PassarDeUmObjetoParaOutro(object sender, KeyEventArgs e)
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

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            checkBoxCartao.IsChecked = false;
            ConsultaTitulos();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "IMPORTAR " + Principal.TipoAto;

            if (Principal.TipoAto == "APONTAMENTO")
            {
                colDataPag.Width = 0;
            }
            else
            {
                colData.Width = 0;
            }



            datePickerdataConsulta.SelectedDate = DateTime.Now.Date;
        }

        private List<Ato> obterListaDeAtosProtesto(FbDataReader drTotal)
        {

            while (drTotal.Read())
            {
                Ato ato = new Ato();

                if (drTotal["STATUS"].ToString() != "")
                    ato.TipoAto = drTotal["STATUS"].ToString();

                if (drTotal["PROTOCOLO"].ToString() != "")
                    ato.Protocolo = Convert.ToInt32(drTotal["PROTOCOLO"]);

                if (drTotal["DT_PROTOCOLO"].ToString() != "")
                {
                    ato.DataAto = Convert.ToDateTime(drTotal["DT_PROTOCOLO"]);
                }


                if (drTotal["DT_PAGAMENTO"].ToString() != "")
                {
                    ato.DataPagamento = Convert.ToDateTime(drTotal["DT_PAGAMENTO"]); // DATA PAGAMENTO ALTERA NO AGUARDE PROTESTO
                }

                ato.TipoCobranca = "DEPÓSITO";
                ato.Pago = true;
                ato.IdUsuario = Principal.usuarioLogado.Id_Usuario;
                ato.Usuario = Principal.usuarioLogado.NomeUsu;
                ato.Atribuicao = "PROTESTO";
                ato.ValorAdicionar = 0M;
                ato.ValorDesconto = 0M;
                ato.Portador = drTotal["APRESENTANTE"].ToString();
                ato.Checked = false;
                ato.Convenio = drTotal["CONVENIO"].ToString();
                ato.ValorTitulo = Convert.ToDecimal(drTotal["SALDO_TITULO"]);
                ato.Total = Convert.ToDecimal(drTotal["TOTAL"]);

                listaRetorno.Add(ato);
            }

            return listaRetorno;
        }

        private void ConsultaTitulos()
        {

            string dataInicio = string.Empty;
            string dataFim = string.Empty;

            if (cmbTpConsulta.SelectedIndex == 0)
            {
                dataInicio = DateTime.Now.ToShortDateString();
                dataFim = DateTime.Now.ToShortDateString();

                if (datePickerdataConsulta.SelectedDate != null && datePickerdataConsultaFim.SelectedDate != null)
                {
                    dataInicio = string.Format("{0:00}.{1:00}.{2}", datePickerdataConsulta.SelectedDate.Value.Day, datePickerdataConsulta.SelectedDate.Value.Month, datePickerdataConsulta.SelectedDate.Value.Year);

                    dataFim = string.Format("{0:00}.{1:00}.{2}", datePickerdataConsultaFim.SelectedDate.Value.Day, datePickerdataConsultaFim.SelectedDate.Value.Month, datePickerdataConsultaFim.SelectedDate.Value.Year);
                }
                else
                {
                    MessageBox.Show("Informe Data Inicio e Data Fim.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
            }



            if (Principal.TipoAto == "APONTAMENTO")
            {

                if (cmbTpConsulta.SelectedIndex == 0)
                {
                    FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
                    conTotal.Open();
                    try
                    {


                        FbCommand cmdTotal = new FbCommand("Select * from TITULOS where (DT_PROTOCOLO between '" + dataInicio + "' and '" + dataFim + "') or (dt_retirado between '" + dataInicio + "' and '" + dataFim + "' and postecipado = 'P')", conTotal);
                        cmdTotal.CommandType = CommandType.Text;


                        FbDataReader drTotal;
                        drTotal = cmdTotal.ExecuteReader();

                        //dtTotal = new DataTable();
                        //dtTotal.Load(drTotal);
                        dataGridConsulta.ItemsSource = null;
                        listaRetorno = new List<Ato>();
                        dataGridConsulta.ItemsSource = obterListaDeAtosProtesto(drTotal);
                        if (dataGridConsulta.Items.Count > 0)
                            dataGridConsulta.SelectedIndex = 0;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conTotal.Close();
                    }
                }

                if (cmbTpConsulta.SelectedIndex == 1)
                {
                    int protocolo = 0;

                    if (txtConsulta.Text != "")
                        protocolo = Convert.ToInt32(txtConsulta.Text);

                    FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
                    conTotal.Open();
                    try
                    {


                        FbCommand cmdTotal = new FbCommand("Select * from TITULOS where PROTOCOLO = " + protocolo + " and (status = 'RETIRADO' or status = 'APONTADO')", conTotal);
                        cmdTotal.CommandType = CommandType.Text;


                        FbDataReader drTotal;
                        drTotal = cmdTotal.ExecuteReader();

                        //dtTotal = new DataTable();
                        //dtTotal.Load(drTotal);

                        dataGridConsulta.ItemsSource = null;
                        listaRetorno = new List<Ato>();
                        dataGridConsulta.ItemsSource = obterListaDeAtosProtesto(drTotal);
                        if (dataGridConsulta.Items.Count > 0)
                            dataGridConsulta.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conTotal.Close();
                    }
                }


            }

            if (Principal.TipoAto == "PAGAMENTO")
            {
                if (cmbTpConsulta.SelectedIndex == 0)
                {
                    FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
                    conTotal.Open();
                    try
                    {


                        FbCommand cmdTotal = new FbCommand("Select * from TITULOS where STATUS = 'PAGO' AND DT_PAGAMENTO between '" + dataInicio + "' and '" + dataFim + "'", conTotal);
                        cmdTotal.CommandType = CommandType.Text;


                        FbDataReader drTotal;
                        drTotal = cmdTotal.ExecuteReader();

                        //dtTotal = new DataTable();
                        //dtTotal.Load(drTotal);

                        dataGridConsulta.ItemsSource = null;
                        listaRetorno = new List<Ato>();
                        dataGridConsulta.ItemsSource = obterListaDeAtosProtesto(drTotal);

                        if (dataGridConsulta.Items.Count > 0)
                            dataGridConsulta.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conTotal.Close();
                    }
                }

                if (cmbTpConsulta.SelectedIndex == 1)
                {
                    int protocolo = 0;

                    if (txtConsulta.Text != "")
                        protocolo = Convert.ToInt32(txtConsulta.Text);

                    FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingProtesto);
                    conTotal.Open();
                    try
                    {


                        FbCommand cmdTotal = new FbCommand("Select * from TITULOS where STATUS = 'PAGO' AND PROTOCOLO = " + protocolo, conTotal);
                        cmdTotal.CommandType = CommandType.Text;


                        FbDataReader drTotal;
                        drTotal = cmdTotal.ExecuteReader();

                        //dtTotal = new DataTable();
                        //dtTotal.Load(drTotal);

                        dataGridConsulta.ItemsSource = null;
                        listaRetorno = new List<Ato>();
                        dataGridConsulta.ItemsSource = obterListaDeAtosProtesto(drTotal);

                        if (dataGridConsulta.Items.Count > 0)
                            dataGridConsulta.SelectedIndex = 0;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conTotal.Close();
                    }
                }
            }


        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            if (listaRetorno.Count > 0)
            {
                _winAtoProtesto.importou = true;

                WinAguardeProtesto aguardeProtesto = new WinAguardeProtesto(listaRetorno, Principal);
                aguardeProtesto.Owner = this;
                aguardeProtesto.ShowDialog();
                this.Close();
            }
        }



        private void dataGridConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridConsulta.SelectedItem != null)
                checkBoxCartao.IsChecked = listaRetorno[dataGridConsulta.SelectedIndex].Checked;
        }

        private void btnCartao_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridConsulta.SelectedItem != null)
            {
                if (((Ato)dataGridConsulta.SelectedItem).Checked == true)
                {

                    listaRetorno[dataGridConsulta.SelectedIndex].Checked = false;
                    checkBoxCartao.IsChecked = false;
                    dataGridConsulta.Items.Refresh();
                }
                else
                {
                    listaRetorno[dataGridConsulta.SelectedIndex].Checked = true;
                    checkBoxCartao.IsChecked = true;
                    dataGridConsulta.Items.Refresh();
                }

            }
        }

        private void cmbTpConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void datePickerdataConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }

        private void datePickerdataConsultaFim_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmObjetoParaOutro(sender, e);
        }



    }



}
