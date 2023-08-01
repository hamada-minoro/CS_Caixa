using CS_Caixa.Agragador;
using CS_Caixa.RelatoriosForms;
using FirebirdSql.Data.FirebirdClient;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica interna para WinEtiquetaPrenotacao.xaml
    /// </summary>
    public partial class WinEtiquetaPrenotacao : Window
    {
        public WinEtiquetaPrenotacao()
        {
            InitializeComponent();
        }

        private void txtProtocolo_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {

            if(txtProtocolo.Text == "")
            {
                MessageBox.Show("Informe o Número do Protocolo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txtLivro.Text == "")
            {
                MessageBox.Show("Informe o Número do Livro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txtFolhas.Text == "")
            {
                MessageBox.Show("Informe o Número da Folha.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingRgi))
            {
                conn.Open();

                int cont = 0;
                try
                {
                    string comando = "select * from PRENOTACAO where PROTOCOLO = " + txtProtocolo.Text;

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr;

                        dr = cmdTotal.ExecuteReader();


                        AtoPrincipalQrCode ato;

                        while (dr.Read())
                        {
                            cont = 1;
                            ato = new AtoPrincipalQrCode();

                            ato.AtoId = (int)dr["ID_PRENOT"]; ;
                            ato.Data = (DateTime)dr["DATA"];
                            ato.Aleatorio = dr["ALEATORIO"].ToString();


                            ato.Natureza = dr["DESCRICAO"].ToString();
                           
                            
                            ato.Selo = dr["SELO"].ToString();
                            ato.Serventia = 1823;

                           
                            
                            ato.Protocolo = dr["TALAO"].ToString();

                            ato.Livro = txtLivro.Text;

                            ato.FolhasInicio = txtFolhas.Text;

                            ato.Protocolo = dr["PROTOCOLO"].ToString();

                            ato.Cerp = cmbEscrevente.Text;
                            
                            if (dr["EMOLUMENTOS"] != null)
                                ato.Emol = Convert.ToDecimal(dr["EMOLUMENTOS"]);

                            if (dr["FETJ"] != null)
                                ato.Fetj = Convert.ToDecimal(dr["FETJ"]);

                            if (dr["FUNDPERJ"] != null)
                                ato.Fund = Convert.ToDecimal(dr["FUNDPERJ"]);

                            if (dr["FUNPERJ"] != null)
                                ato.Funp = Convert.ToDecimal(dr["FUNPERJ"]);

                            if (dr["FUNARPEN"] != null)
                                ato.Funarpen = Convert.ToDecimal(dr["FUNARPEN"]);

                            if (dr["PMCMV"] != null)
                                ato.Pmcmv = Convert.ToDecimal(dr["PMCMV"]);

                            if (dr["ISS"] != null)
                                ato.Iss = Convert.ToDecimal(dr["ISS"]);

                            if (dr["TOTAL"] != null)
                                ato.Total = Convert.ToDecimal(dr["TOTAL"]);

                            ImprimirQrCode(ato);

                            
                        }

                        if (cont == 0)
                        {
                            MessageBox.Show("Protocolo não localizado. Favor verificar.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

        private void ImprimirQrCode(AtoPrincipalQrCode atoSelecionado)
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

                    string tipo = "prenotacao";

                    DateTime data = atoSelecionado.Data;

                                       


                    WinRelatorioImprimirQrCode imprimir = new WinRelatorioImprimirQrCode(arquivo.FullName, tipo, atoSelecionado);
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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProtocolo.Focus();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

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


        private void txtFolhas_PreviewKeyDown(object sender, KeyEventArgs e)
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
