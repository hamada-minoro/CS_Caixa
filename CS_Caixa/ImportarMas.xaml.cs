using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for ImportarMas.xaml
    /// </summary>
    public partial class ImportarMas : Window
    {
        ClassImportarMas classImportarMas = new ClassImportarMas();

        ImportarMa atoSelecionado = new ImportarMa();

        public List<ImportarMa> atosConsultados = new List<ImportarMa>();

        List<string> tiposConsulta = new List<string>();

        public string tipoConsulta;

        string tipoSalvar = string.Empty;

        public ImportarMas()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            tiposConsulta.Add("ATRIBUIÇÃO");
            tiposConsulta.Add("TIPO DE ATO");
            tiposConsulta.Add("SELO");

            cmbTipoConsulta.ItemsSource = tiposConsulta;

            cmbTipoConsulta.SelectedIndex = 0;

            IniciarForm();
        }

        private void btnAdicionarAto_Click(object sender, RoutedEventArgs e)
        {

            tabItemRelatorios.IsEnabled = false;
            dataGridAtoPraticado.IsEnabled = false;
            gridAtoPraticado.IsEnabled = true;
            GridConsulta.IsEnabled = false;
            btnAlterarAto.IsEnabled = false;
            btnExcluirAto.IsEnabled = false;
            btnCancelarAto.IsEnabled = true;
            btnSalvarAto.IsEnabled = true;
            btnAdicionarAto.IsEnabled = false;

            tipoSalvar = "novo";

            LimparCamposParaAdicionar();
        }


        private void IniciarForm()
        {

            cmbAtribuicao.ItemsSource = classImportarMas.CarregarListaAtribuicoes();

            cmbTipoAto.ItemsSource = classImportarMas.CarregarListaTipoAtos();

            tabItemRelatorios.IsEnabled = true;
            dataGridAtoPraticado.IsEnabled = true;
            gridAtoPraticado.IsEnabled = false;
            GridConsulta.IsEnabled = true;

            if (dataGridAtoPraticado.Items.Count > 0)
            {
                btnAlterarAto.IsEnabled = true;
                btnExcluirAto.IsEnabled = true;
            }
            else
            {
                btnAlterarAto.IsEnabled = false;
                btnExcluirAto.IsEnabled = false;
            }


            btnCancelarAto.IsEnabled = false;
            btnSalvarAto.IsEnabled = false;
            btnAdicionarAto.IsEnabled = true;


            if (dataGridAtoPraticado.Items.Count > 0)
                dataGridAtoPraticado.SelectedIndex = 0;
            else
                LimparCamposParaAdicionar();


            if (atoSelecionado != null)
                PreencherCamposAtoSelecionado();
            else
                LimparCamposParaAdicionar();

            tipoSalvar = string.Empty;
        }

        private void btnAlterarAto_Click(object sender, RoutedEventArgs e)
        {
            tabItemRelatorios.IsEnabled = false;
            dataGridAtoPraticado.IsEnabled = false;
            gridAtoPraticado.IsEnabled = true;
            GridConsulta.IsEnabled = false;
            btnAlterarAto.IsEnabled = false;
            btnExcluirAto.IsEnabled = false;
            btnCancelarAto.IsEnabled = true;
            btnSalvarAto.IsEnabled = true;
            btnAdicionarAto.IsEnabled = false;

            tipoSalvar = "alterar";
        }

        private void btnExcluirAto_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                if (atoSelecionado != null)
                {
                    try
                    {
                        classImportarMas.ExcluirAto(atoSelecionado);
                        atosConsultados.Remove(atoSelecionado);
                        dataGridAtoPraticado.Items.Refresh();

                        CalcularTotaisRelatorio();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um erro inesperado ao tentar excluir o registro. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        classImportarMas = new ClassImportarMas();
                    }
                }

            IniciarForm();
        }

        private void btnCancelarAto_Click(object sender, RoutedEventArgs e)
        {
            IniciarForm();
            CalcularTotaisRelatorio();
        }


        private void LimparCamposParaAdicionar()
        {
            dpData.SelectedDate = DateTime.Now.Date;
            cmbAtribuicao.Text = "";
            cmbTipoAto.Text = "";
            txtSelo.Text = "";
            txtAleatorio.Text = "";
            txtTipoCobranca.Text = "";
            txtEmolumentos.Text = "0,00";
            txtFetj.Text = "0,00";
            txtFundperj.Text = "0,00";
            txtFunperj.Text = "0,00";
            txtFunarpen.Text = "0,00";
            txtRessag.Text = "0,00";
            txtMutua.Text = "0,00";
            txtAcoterj.Text = "0,00";
            txtDistribuidor.Text = "0,00";
            txtIss.Text = "0,00";
            txtTotal.Text = "0,00";

            txtTotalEmolumentos.Text = "0,00";
            txtTotalFetj.Text = "0,00";
            txtTotalFundperj.Text = "0,00";
            txtTotalFunperj.Text = "0,00";
            txtTotalFunarpen.Text = "0,00";
            txtTotalRessag.Text = "0,00";
            txtTotalMutua.Text = "0,00";
            txtTotalAcoterj.Text = "0,00";
            txtTotalDistribuidor.Text = "0,00";
            txtTotalIss.Text = "0,00";
            txtTotalTotal.Text = "0,00";
            txtTotalGrerj.Text = "0,00";
        }

        private void btnSalvarAto_Click(object sender, RoutedEventArgs e)
        {

            if (dpData.SelectedDate == null)
            {
                MessageBox.Show("Informe a Data.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ImportarMa salvarAto;

            if (tipoSalvar == "novo")
                salvarAto = new ImportarMa();
            else
                salvarAto = atoSelecionado;

            salvarAto.Data = dpData.SelectedDate.Value;
            salvarAto.Atribuicao = cmbAtribuicao.Text;
            salvarAto.TipoAto = cmbTipoAto.Text;
            salvarAto.Selo = txtSelo.Text;
            salvarAto.Aleatorio = txtAleatorio.Text;
            salvarAto.TipoCobranca = txtTipoCobranca.Text;
            salvarAto.Emolumentos = Convert.ToDecimal(txtEmolumentos.Text);
            salvarAto.Fetj = Convert.ToDecimal(txtFetj.Text);
            salvarAto.Fundperj = Convert.ToDecimal(txtFundperj.Text);
            salvarAto.Funperj = Convert.ToDecimal(txtFunperj.Text);
            salvarAto.Funarpen = Convert.ToDecimal(txtFunarpen.Text);
            salvarAto.Ressag = Convert.ToDecimal(txtRessag.Text);
            salvarAto.Mutua = Convert.ToDecimal(txtMutua.Text);
            salvarAto.Acoterj = Convert.ToDecimal(txtAcoterj.Text);
            salvarAto.Distribuidor = Convert.ToDecimal(txtDistribuidor.Text);
            salvarAto.Iss = Convert.ToDecimal(txtIss.Text);
            salvarAto.Total = Convert.ToDecimal(txtTotal.Text);

            try
            {
                classImportarMas.SalvarAto(salvarAto, tipoSalvar);

                dpConsultaInicio.SelectedDate = dpData.SelectedDate;
                dpConsultaFim.SelectedDate = dpData.SelectedDate;

                ConsultarPorPeriodo();

                MessageBox.Show("Registro salvo com sucesso.", "Salvar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao salvar o Registro. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                classImportarMas = new ClassImportarMas();
            }




            IniciarForm();

            dataGridAtoPraticado.SelectedItem = salvarAto;
            dataGridAtoPraticado.ScrollIntoView(salvarAto);
        }

        private void gridAtoPraticado_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtFetj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtFetj.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtFetj.Text.IndexOf(",");

                if (indexVirgula + 3 == txtFetj.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {

            tipoConsulta = "data";
            ConsultarPorPeriodo();

        }

        public void ConsultarPorPeriodo()
        {
            if (dpConsultaInicio.SelectedDate != null)
            {
                if (dpConsultaFim.SelectedDate == null)
                    dpConsultaFim.SelectedDate = dpConsultaInicio.SelectedDate;

                WinAguardeCosultaIssMas aguarde = new WinAguardeCosultaIssMas(this);
                aguarde.Owner = this;
                aguarde.ShowDialog();                

                CalcularTotaisRelatorio();

                dataGridAtoPraticado.ItemsSource = atosConsultados;
            }
            else
            {
                MessageBox.Show("Informe a data início.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


            if (dataGridAtoPraticado.Items.Count > 0)
            {
                btnAlterarAto.IsEnabled = true;
                btnExcluirAto.IsEnabled = true;
                dataGridAtoPraticado.SelectedIndex = 0;
            }
            else
            {
                btnAlterarAto.IsEnabled = false;
                btnExcluirAto.IsEnabled = false;
                LimparCamposParaAdicionar();
            }

        }

        private void CalcularTotaisRelatorio()
        {
            txtTotalEmolumentos.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Emolumentos));
            txtTotalFetj.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Fetj));
            txtTotalFundperj.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Fundperj));
            txtTotalFunperj.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Funperj));
            txtTotalFunarpen.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Funarpen));
            txtTotalRessag.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Ressag));
            txtTotalMutua.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Mutua));
            txtTotalAcoterj.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Acoterj));
            txtTotalIss.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Iss));
            txtTotalDistribuidor.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Distribuidor));
            txtTotalTotal.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Total));
            txtTotalGrerj.Text = string.Format("{0:n2}", atosConsultados.Sum(p => p.Fetj) + atosConsultados.Sum(p => p.Fundperj) + atosConsultados.Sum(p => p.Funperj) + atosConsultados.Sum(p => p.Funarpen));
        }

        private void btnSincronizar_Click(object sender, RoutedEventArgs e)
        {
            Sincronizar();

        }

        private void Sincronizar()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();

            openFileDialog1.Filter = "Arquivos xml (*.xml)|*.xml";

            openFileDialog1.InitialDirectory = @"\\Servidor\cgj-rj\ModuloApoioServico\Caixa de Entrada\Relatórios";

            try
            {
                if (openFileDialog1.ShowDialog() == true)
                {

                    FileInfo fi = new FileInfo(openFileDialog1.FileName);

                    string relatorio = @"\\Servidor\c\CS_Sistemas\CS_Caixa\relatorio.xml.xsd";

                    ValidadorSchemaXml.Validar(fi.FullName, relatorio);

                    List<ImportarMa> AtosImportar = classImportarMas.LerArquivoXml(openFileDialog1.FileName);

                    if (AtosImportar.Count == 0)
                    {
                        MessageBox.Show("Não constam atos para este arquivo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }

                    var aguardeIndisp = new AguardeIntegracaoIssMas(AtosImportar, openFileDialog1.FileName);
                    aguardeIndisp.Owner = this;
                    aguardeIndisp.ShowDialog();

                    DateTime data = Convert.ToDateTime(AtosImportar[0].Data);

                    dpConsultaInicio.SelectedDate = data;

                    dpConsultaFim.SelectedDate = data;

                    ConsultarPorPeriodo();

                    if (dataGridAtoPraticado.Items.Count > 0)
                        dataGridAtoPraticado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridAtoPraticado_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGridAtoPraticado.SelectedItem != null)
                atoSelecionado = (ImportarMa)dataGridAtoPraticado.SelectedItem;

            try
            {
                PreencherCamposAtoSelecionado();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                classImportarMas = new ClassImportarMas();
            }
        }

        private void PreencherCamposAtoSelecionado()
        {
            dpData.SelectedDate = atoSelecionado.Data;
            cmbAtribuicao.Text = atoSelecionado.Atribuicao;
            cmbTipoAto.Text = atoSelecionado.TipoAto;
            txtSelo.Text = atoSelecionado.Selo;
            txtAleatorio.Text = atoSelecionado.Aleatorio;
            txtTipoCobranca.Text = atoSelecionado.TipoCobranca;
            txtEmolumentos.Text = string.Format("{0:n2}", atoSelecionado.Emolumentos);
            txtFetj.Text = string.Format("{0:n2}", atoSelecionado.Fetj);
            txtFundperj.Text = string.Format("{0:n2}", atoSelecionado.Fundperj);
            txtFunperj.Text = string.Format("{0:n2}", atoSelecionado.Funperj);
            txtFunarpen.Text = string.Format("{0:n2}", atoSelecionado.Funarpen);
            txtRessag.Text = string.Format("{0:n2}", atoSelecionado.Ressag);
            txtMutua.Text = string.Format("{0:n2}", atoSelecionado.Mutua);
            txtAcoterj.Text = string.Format("{0:n2}", atoSelecionado.Acoterj);
            txtDistribuidor.Text = string.Format("{0:n2}", atoSelecionado.Distribuidor);
            txtIss.Text = string.Format("{0:n2}", atoSelecionado.Iss);
            txtTotal.Text = string.Format("{0:n2}", atoSelecionado.Total);
        }

        private void dpConsultaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpConsultaInicio.SelectedDate > DateTime.Now.Date)
            {
                dpConsultaInicio.SelectedDate = DateTime.Now.Date;
            }

            dpConsultaFim.SelectedDate = dpConsultaInicio.SelectedDate;

            if (dpConsultaInicio.SelectedDate > dpConsultaFim.SelectedDate)
            {
                dpConsultaFim.SelectedDate = dpConsultaInicio.SelectedDate;
            }
        }

        private void dpConsultaFim_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpConsultaInicio.SelectedDate != null)
            {
                if (dpConsultaInicio.SelectedDate > dpConsultaFim.SelectedDate)
                {
                    dpConsultaFim.SelectedDate = dpConsultaInicio.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void txtAcoterj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtAcoterj.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtAcoterj.Text.IndexOf(",");

                if (indexVirgula + 3 == txtAcoterj.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtAleatorio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
            DigitarSomenteLetras(sender, e);
        }

        private void txtDistribuidor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtDistribuidor.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtDistribuidor.Text.IndexOf(",");

                if (indexVirgula + 3 == txtDistribuidor.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtEmolumentos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmolumentos.Text != "" && txtEmolumentos.Text != ",")
            {
                txtEmolumentos.Text = string.Format("{0:n2}", Convert.ToDecimal(txtEmolumentos.Text));
            }
            else
                txtEmolumentos.Text = "0,00";
        }

        private void txtFundperj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtFundperj.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtFundperj.Text.IndexOf(",");

                if (indexVirgula + 3 == txtFundperj.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtFunperj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtFunperj.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtFunperj.Text.IndexOf(",");

                if (indexVirgula + 3 == txtFunperj.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtIss_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtIss.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtIss.Text.IndexOf(",");

                if (indexVirgula + 3 == txtIss.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtMutua_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtMutua.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtMutua.Text.IndexOf(",");

                if (indexVirgula + 3 == txtMutua.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtRessag_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtRessag.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtRessag.Text.IndexOf(",");

                if (indexVirgula + 3 == txtRessag.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (txtSelo.Text.Length < 4)
                DigitarSomenteLetras(sender, e);
            else
                DigitarSomenteNumeros(sender, e);
        }

        private void txtTipoCobranca_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
            DigitarSomenteLetras(sender, e);
        }

        private void txtEmolumentos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtEmolumentos.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtEmolumentos.Text.IndexOf(",");

                if (indexVirgula + 3 == txtEmolumentos.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void PassarDeUmCoponenteParaOutro(object sender, KeyEventArgs e)
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

        private void DigitarSomenteNumeros(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
        }

        private void DigitarSemNumeros(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
        }


        private void DigitarSomenteNumerosEmReaisComVirgual(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25 || key == 32 || key == 142 || key == 88);
        }

        private void DigitarSomenteLetras(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32 || key >= 44 && key <= 69);
        }

        private void txtTotal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtTotal.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtTotal.Text.IndexOf(",");

                if (indexVirgula + 3 == txtTotal.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        //private void cmbAtribuicao_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    cmbAtribuicao.Text = cmbAtribuicao.Text.ToUpper();
        //}

        //private void cmbTipoAto_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    cmbTipoAto.Text = cmbTipoAto.Text.ToUpper();
        //}

        private void txtFetj_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtFetj.Text != "" && txtFetj.Text != ",")
            {
                txtFetj.Text = string.Format("{0:n2}", Convert.ToDecimal(txtFetj.Text));
            }
            else
                txtFetj.Text = "0,00";
        }

        private void txtFundperj_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtFundperj.Text != "" && txtFundperj.Text != ",")
            {
                txtFundperj.Text = string.Format("{0:n2}", Convert.ToDecimal(txtFundperj.Text));
            }
            else
                txtFundperj.Text = "0,00";
        }

        private void txtFunperj_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtFunperj.Text != "" && txtFunperj.Text != ",")
            {
                txtFunperj.Text = string.Format("{0:n2}", Convert.ToDecimal(txtFunperj.Text));
            }
            else
                txtFunperj.Text = "0,00";
        }

        private void txtFunarpen_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (!txtFunarpen.Text.Contains(","))
                DigitarSomenteNumerosEmReaisComVirgual(sender, e);
            else
            {
                int indexVirgula = txtFunarpen.Text.IndexOf(",");

                if (indexVirgula + 3 == txtFunarpen.Text.Length)
                    DigitarSemNumeros(sender, e);
                else
                    DigitarSomenteNumeros(sender, e);
            }
        }

        private void txtFunarpen_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtFunarpen.Text != "" && txtFunarpen.Text != ",")
            {
                txtFunarpen.Text = string.Format("{0:n2}", Convert.ToDecimal(txtFunarpen.Text));
            }
            else
                txtFunarpen.Text = "0,00";
        }

        private void txtRessag_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRessag.Text != "" && txtRessag.Text != ",")
            {
                txtRessag.Text = string.Format("{0:n2}", Convert.ToDecimal(txtRessag.Text));
            }
            else
                txtRessag.Text = "0,00";
        }

        private void txtMutua_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtMutua.Text != "" && txtMutua.Text != ",")
            {
                txtMutua.Text = string.Format("{0:n2}", Convert.ToDecimal(txtMutua.Text));
            }
            else
                txtMutua.Text = "0,00";
        }

        private void txtAcoterj_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAcoterj.Text != "" && txtAcoterj.Text != ",")
            {
                txtAcoterj.Text = string.Format("{0:n2}", Convert.ToDecimal(txtAcoterj.Text));
            }
            else
                txtAcoterj.Text = "0,00";
        }

        private void txtIss_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtIss.Text != "" && txtIss.Text != ",")
            {
                txtIss.Text = string.Format("{0:n2}", Convert.ToDecimal(txtIss.Text));
            }
            else
                txtIss.Text = "0,00";
        }

        private void txtDistribuidor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDistribuidor.Text != "" && txtDistribuidor.Text != ",")
            {
                txtDistribuidor.Text = string.Format("{0:n2}", Convert.ToDecimal(txtDistribuidor.Text));
            }
            else
                txtDistribuidor.Text = "0,00";
        }


        private void txtTotal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtTotal.Text != "" && txtTotal.Text != ",")
            {
                txtTotal.Text = string.Format("{0:n2}", Convert.ToDecimal(txtTotal.Text));
            }
            else
                txtTotal.Text = "0,00";
        }

        private void txtEmolumentos_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmolumentos.Text == "0,00")
                txtEmolumentos.Text = "";


        }

        private void txtFetj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtFetj.Text == "0,00")
                txtFetj.Text = "";
        }

        private void txtFundperj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtFundperj.Text == "0,00")
                txtFundperj.Text = "";
        }

        private void txtFunperj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtFunperj.Text == "0,00")
                txtFunperj.Text = "";
        }

        private void txtFunarpen_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtFunarpen.Text == "0,00")
                txtFunarpen.Text = "";
        }

        private void txtRessag_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRessag.Text == "0,00")
                txtRessag.Text = "";
        }

        private void txtMutua_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMutua.Text == "0,00")
                txtMutua.Text = "";
        }

        private void txtAcoterj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtAcoterj.Text == "0,00")
                txtAcoterj.Text = "";
        }

        private void txtIss_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtIss.Text == "0,00")
                txtIss.Text = "";
        }

        private void txtDistribuidor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDistribuidor.Text == "0,00")
                txtDistribuidor.Text = "";
        }

        private void txtTotal_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTotal.Text == "0,00")
                txtTotal.Text = "";
        }

        private void dpData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void cmbAtribuicao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void cmbTipoAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }


        private void lblSincronizar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Sincronizar();
        }

        private void cmbAtribuicao_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void btnRelatorioIss_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRelatorioTotal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dpConsultaInicio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void dpConsultaFim_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void cmbTipoConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbTipoConsulta.SelectedIndex <= 1)
            {
                txtDadosConsulta.Visibility = Visibility.Hidden;
                cmbDadosConsulta.Visibility = Visibility.Visible;

                if (cmbTipoConsulta.SelectedIndex == 0)
                {
                    cmbDadosConsulta.ItemsSource = classImportarMas.CarregarListaAtribuicoes();
                }
                else
                {
                    cmbDadosConsulta.ItemsSource = classImportarMas.CarregarListaTipoAtos();
                }

                cmbDadosConsulta.Focus();
            }
            else
            {
                txtDadosConsulta.Visibility = Visibility.Visible;
                cmbDadosConsulta.Visibility = Visibility.Hidden;
                txtDadosConsulta.Text = "";
                txtDadosConsulta.Focus();
            }
        }

        private void txtDadosConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarDeUmCoponenteParaOutro(sender, e);

            if (txtDadosConsulta.Text.Length < 4)
                DigitarSomenteLetras(sender, e);
            else
                DigitarSomenteNumeros(sender, e);
        }

        private void btnConsultaDetalhada_Click(object sender, RoutedEventArgs e)
        {
            tipoConsulta = "detalhada";
                        

            WinAguardeCosultaIssMas aguarde = new WinAguardeCosultaIssMas(this);
            aguarde.Owner = this;
            aguarde.ShowDialog();

            CalcularTotaisRelatorio();

            dataGridAtoPraticado.ItemsSource = atosConsultados;


            if (dataGridAtoPraticado.Items.Count > 0)
            {
                btnAlterarAto.IsEnabled = true;
                btnExcluirAto.IsEnabled = true;
                dataGridAtoPraticado.SelectedIndex = 0;
            }
            else
            {
                btnAlterarAto.IsEnabled = false;
                btnExcluirAto.IsEnabled = false;
                LimparCamposParaAdicionar();
            }



        }

       

    }
}
