using CS_Caixa.Agragador;
using CS_Caixa.RelatoriosForms;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinGerarQrCode.xaml
    /// </summary>
    public partial class WinGerarQrCode : Window
    {
        public string tipoConsulta;

        public List<AtoPrincipalQrCode> atoPrincipalQrCode = new List<AtoPrincipalQrCode>();

       
        public WinGerarQrCode()
     
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerdataConsulta.SelectedDate = DateTime.Now.Date.AddDays(-20);

            datePickerdataConsultaFim.SelectedDate = DateTime.Now.Date;

            btnConsultarData_Click(sender, e);
        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if (txtLivro.Text != "" && txtAto.Text != "")
            {
                tipoConsulta = "livroAto";
                AguardeCarregandoGridQrCode aguarde = new AguardeCarregandoGridQrCode(this);
                aguarde.Owner = this;
                aguarde.ShowDialog();

                dataGridConsulta.ItemsSource = atoPrincipalQrCode.OrderByDescending(p => p.Data);

                if (dataGridConsulta.Items.Count > 0)
                    dataGridConsulta.SelectedIndex = 0;
            }
            else
                MessageBox.Show("Informe Livro e Ato.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                AtoPrincipalQrCode atoSelecionado = (AtoPrincipalQrCode)dataGridConsulta.SelectedItem;

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

                    string nomeArquivo = @"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\QrCode\Notas\Livro" + atoSelecionado.Livro + "_Folha" + atoSelecionado.FolhasInicio + "_Ato" + atoSelecionado.Ato + "_" + atoSelecionado.Selo + atoSelecionado.Aleatorio + ".bmp";

                    FileInfo arquivo = new FileInfo(nomeArquivo);

                    FileInfo arquivoRemover = new FileInfo(nomeArquivo);

                    DirectoryInfo diretorio = new DirectoryInfo(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\QrCode\Notas");

                    if (!diretorio.Exists)
                        diretorio.Create();

                    if (arquivo.Exists)
                        arquivo.Delete();

                    qrCode.Save(arquivo.FullName, System.Drawing.Imaging.ImageFormat.Bmp);
                    if (ckbEmTest.IsChecked == true)
                    {
                        WinRelatorioImprimirQrCode imprimir = new WinRelatorioImprimirQrCode(arquivo.FullName, selos, cmbModelo.SelectedIndex, "notas", cmbEsquerda.SelectedIndex);
                        imprimir.Owner = this;
                        imprimir.ShowDialog();
                    }
                    else
                    {
                        WinRelatorioImprimirQrCode imprimir = new WinRelatorioImprimirQrCode(arquivo.FullName, selos, cmbModelo.SelectedIndex, "rgi", cmbEsquerda.SelectedIndex);
                        imprimir.Owner = this;
                        imprimir.ShowDialog();
                    }


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


        private void txtAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void txtLivro_PreviewKeyDown(object sender, KeyEventArgs e)
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

        

     
    }
}
