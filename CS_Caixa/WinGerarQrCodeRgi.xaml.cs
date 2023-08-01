using CS_Caixa.Agragador;
using CS_Caixa.RelatoriosForms;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinGerarQrCodeRgi.xaml
    /// </summary>
    public partial class WinGerarQrCodeRgi : Window
    {
        public List<AtoPrincipalQrCode> atoPrincipalQrCode = new List<AtoPrincipalQrCode>();
        public string tipoConsulta;

        AtoPrincipalQrCode atoSelecionado;


        public WinGerarQrCodeRgi()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerdataConsulta.SelectedDate = DateTime.Now.Date.AddDays(-20);

            datePickerdataConsultaFim.SelectedDate = DateTime.Now.Date;

            btnConsultarData_Click(sender, e);
        }

        private void btnConsultarData_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerdataConsulta.SelectedDate != null && datePickerdataConsultaFim.SelectedDate != null)
            {
                tipoConsulta = "Data";
                AguardeCarregandoGridQrCode aguarde = new AguardeCarregandoGridQrCode(this);
                aguarde.Owner = this;
                aguarde.ShowDialog();

                dataGridConsulta.ItemsSource = atoPrincipalQrCode.OrderByDescending(p => p.Data);

                if (dataGridConsulta.Items.Count > 0)
                    dataGridConsulta.SelectedIndex = 0;
            }
            else
                MessageBox.Show("Informe Data Início e Data Fim.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        private void btnGerar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string texto;
                

                if (atoSelecionado != null)
                {
                    
                    string selos;

                    selos = atoSelecionado.Selo + atoSelecionado.Aleatorio;

                    if (atoSelecionado.AtosConjuntos != null)
                        foreach (var item in atoSelecionado.AtosConjuntos)
                        {
                            selos = selos += "=" + item.Selo + item.Aleatorio;
                        }

                    if (atoSelecionado.Natureza.Contains("ELETRÔNICA"))
                        texto = string.Format("SELO={0}|SERV={1}|TIPO={2}|DATA={3}|CERP={4}", selos, atoSelecionado.Serventia, atoSelecionado.Tipo, atoSelecionado.Data.ToShortDateString(), atoSelecionado.Cerp.ToUpper());
                    else
                        texto = string.Format("SELO={0}|SERV={1}|TIPO={2}|DATA={3}", selos, atoSelecionado.Serventia, atoSelecionado.Tipo, atoSelecionado.Data.ToShortDateString());

                    Bitmap qrCode = GerarQRCode(texto);

                    string nomeArquivo = @"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\QrCode\RGI\Talão_" + atoSelecionado.Protocolo + "_" + atoSelecionado.Selo + atoSelecionado.Aleatorio + ".bmp";

                    FileInfo arquivo = new FileInfo(nomeArquivo);

                    FileInfo arquivoRemover = new FileInfo(nomeArquivo);

                    DirectoryInfo diretorio = new DirectoryInfo(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\QrCode\RGI");

                    if (!diretorio.Exists)
                        diretorio.Create();

                    if (arquivo.Exists)
                        arquivo.Delete();

                    qrCode.Save(arquivo.FullName, System.Drawing.Imaging.ImageFormat.Bmp);

                    string tipo = "rgi";

                    DateTime data = atoSelecionado.Data;

                    if (ckbImprimirEtiqueta.IsChecked == true)
                    {
                        if (rbLivreOnus.IsChecked == true)
                            tipo = "rgiEtiquetaLivre";
                        if (rbGravame.IsChecked == true)
                            tipo = "rgiEtiquetaGravame";
                        if (rbInteiroTeor.IsChecked == true)
                            tipo = "rgiEtiquetaInteiroTeor";

                        if (dpData.SelectedDate == null)
                        {
                            MessageBox.Show("O campo 'Data' é obrigatório.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (txtRecibo.Text == "")
                        {
                            MessageBox.Show("O campo 'Recibo' é obrigatório.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        data = dpData.SelectedDate.Value;
                    }

                    string obs = string.Empty;

                    if (ckbImprimirObs.IsChecked == true && ckbImprimirEtiqueta.IsChecked == true)
                        obs = txtObservacao.Text;
                    else
                        obs = "*-*-*-*";

                    if(obs == "")
                        obs = "*-*-*-*";


                    WinRelatorioImprimirQrCode imprimir = new WinRelatorioImprimirQrCode(arquivo.FullName, atoSelecionado.Selo + atoSelecionado.Aleatorio, cmbModelo.SelectedIndex, tipo, cmbEsquerda.SelectedIndex, atoSelecionado, cmbEscrevente.Text, data, txtMatricula.Text, txtRecibo.Text, txtGravame.Text, obs);
                    imprimir.Owner = this;
                    imprimir.ShowDialog();

                    if (arquivoRemover.Exists)
                        arquivoRemover.Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void ckbImprimirEtiqueta_Checked(object sender, RoutedEventArgs e)
        {
            gridEtiquetas.IsEnabled = true;
            rbLivreOnus.IsChecked = true;

            if (ckbImprimirObs.IsChecked == true)
                txtObservacao.IsEnabled = true;
        }

        private void ckbImprimirEtiqueta_Unchecked(object sender, RoutedEventArgs e)
        {
            gridEtiquetas.IsEnabled = false;
            ckbImprimirObs.IsEnabled = false;
        }

        private void rbLivreOnus_Checked(object sender, RoutedEventArgs e)
        {
            txtGravame.Text = "";
            txtGravame.IsEnabled = false;
        }

        private void rbGravame_Checked(object sender, RoutedEventArgs e)
        {
            txtGravame.IsEnabled = true;
        }

        private void rbInteiroTeor_Checked(object sender, RoutedEventArgs e)
        {
            txtGravame.Text = "";
            txtGravame.IsEnabled = false;
        }

        private void dataGridConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridConsulta.SelectedIndex > -1)
                {
                    atoSelecionado = (AtoPrincipalQrCode)dataGridConsulta.SelectedItem;

                    dpData.SelectedDate = atoSelecionado.Data;

                    txtMatricula.Text = string.Format("{0:0.0}", atoSelecionado.Matricula);

                    txtRecibo.Text = string.Format("{0}/{1}", atoSelecionado.Protocolo, atoSelecionado.Data.Year);

                    txtObservacao.Text = atoSelecionado.Obs;
                }
            }
            catch (Exception) { }

        }

        private void ckbImprimirObs_Checked(object sender, RoutedEventArgs e)
        {
            if(ckbImprimirEtiqueta.IsChecked == true)
            txtObservacao.IsEnabled = true;
        }

        private void ckbImprimirObs_Unchecked(object sender, RoutedEventArgs e)
        {
            txtObservacao.IsEnabled = false;
        }
    }
}
