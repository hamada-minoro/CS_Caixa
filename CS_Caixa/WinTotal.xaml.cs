using CS_Caixa.Objetos_de_Valor;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WIA;


namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinTotal.xaml
    /// </summary>
    public partial class WinTotal : Window
    {

        public List<FichaFirmas> fichasConsultadas = new List<FichaFirmas>();

        public string caminho = @"\\SERVIDOR\Cartorio\Total\firmas\Arqs001";

        public int total = 0;

        public int totalPaginas = 0;

        public List<FileInfo> arquivos = new List<FileInfo>();

        public BitmapImage image;

        public DirectoryInfo diretorioAtual;

        public FileInfo arquivoSelecionado;

        public string nomeArquivo;

        public FichaFirmas fichaSelecionada;

        public bool consultaSinal = false;

        public int ultimaFichaSelecionada = 0;

        public bool desativarAuto = false;
        public bool CovertTotal = false;
        public bool pararCovert = false;

        public Bitmap img;
        public Bitmap imgLoad;
        public ImageCodecInfo jpgEncoder;
        public EncoderParameters myEncoderParameters;
        public EncoderParameter myEncoderParameter;


        public WinTotal()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitulo.Content = "Consulta de Fichas do Balcão";

            txtNome.Focus();
        }

        private void DeletarDosArquivos()
        {
            if (MessageBox.Show("Deseja realmente deletar todas as imagens?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (var item in arquivos)
                {
                    if (item.Exists)
                        item.Delete();
                }

                CarregarImagens();
            }
        }

        public void ConverterArquivoTotal()
        {

            try
            {
                
                for (int i = 0; i < totalPaginas; i++)
                {

                    //listView.SelectedItem = listView.Items[i];

                    arquivoSelecionado = arquivos[i];

                    if (arquivoSelecionado != null)
                    {

                        string fileTemp = string.Format(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\FichaBalcao\{0}", arquivoSelecionado.Name);

                        nomeArquivo = arquivoSelecionado.FullName;

                        if (File.Exists(fileTemp))
                            File.Delete(fileTemp);

                        imgLoad = new Bitmap(nomeArquivo);

                        if (imgLoad.Width >= 836)
                        {
                            img = new Bitmap(CropBitmap(imgLoad, 0, 0, 529, 390));

                            img.Save(fileTemp);
                            imgLoad.Dispose();

                            // int indexSelecionado = listView.SelectedIndex;

                            if (File.Exists(nomeArquivo))
                                File.Delete(nomeArquivo);



                            using (Bitmap bmp1 = new Bitmap(fileTemp))
                            {
                                jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                                // Create an Encoder object based on the GUID  
                                // for the Quality parameter category.  
                                System.Drawing.Imaging.Encoder myEncoder =
                                    System.Drawing.Imaging.Encoder.Quality;

                                // Create an EncoderParameters object.  
                                // An EncoderParameters object has an array of EncoderParameter  
                                // objects. In this case, there is only one  
                                // EncoderParameter object in the array.  
                                myEncoderParameters = new EncoderParameters(1);

                                myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                                myEncoderParameters.Param[0] = myEncoderParameter;
                                bmp1.Save(nomeArquivo, jpgEncoder, myEncoderParameters);
                            }


                            //CarregarImagens();



                            //listView.SelectedIndex = indexSelecionado;

                            img.Dispose();


                            if (File.Exists(fileTemp))
                                File.Delete(fileTemp);
                        }
                        else
                            pararCovert = true;
                    }

                }
                

                pararCovert = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F4)
            {
                if (txtQtdAuto.Text != "")
                {
                    CovertTotal = true;
                    pararCovert = false;

                    //ConverterArquivoTotal();

                    total = Convert.ToInt32(txtQtdAuto.Text);

                    for (int i = 0; i < total; i++)
                    {
                        var aguarde = new AguardeTotal(this);
                        aguarde.Owner = this;
                        aguarde.ShowDialog();

                    }

                }
            }

            if (e.Key == Key.F5)
            {
                rbPequena.IsChecked = true;
            }

            if (e.Key == Key.F6)
            {
                rbNormal.IsChecked = true;
            }

            if (e.Key == Key.F7)
            {
                rbMedia.IsChecked = true;
            }

            if (e.Key == Key.F8)
            {
                rbGrande.IsChecked = true;
            }
            
            if (e.Key == Key.PageUp)
            {
                PassarProximo();
            }

            if (e.Key == Key.PageDown)
            {
                PassarAnterior();
            }

            if (e.Key == Key.Escape)
                this.Close();

            if (e.Key == Key.End)
            {
                pararCovert = true;
                CovertTotal = false;
            }
            if (e.Key == Key.F1)
            {
                try
                {

                    if (!diretorioAtual.Exists)
                    {
                        diretorioAtual.Create();
                        caminho = diretorioAtual.FullName;
                    }

                    if (ckbAuto.IsChecked.Value == false)
                        Scannear();
                    else
                        if (MessageBox.Show("Clique em Ok para iniciar a digitalização automática.", "Iniciar", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                            AutoScanner();

                }
                catch
                {
                    MessageBox.Show("Erro na digitalização do documento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (e.Key == Key.F12)
            {
                try
                {

                    if (!diretorioAtual.Exists)
                        diretorioAtual.Create();


                    nomeArquivo = ((FileInfo)listView.SelectedItem).FullName;

                    AlterarScannear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro na digitalização do documento. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (e.Key == Key.F9)
            {
                try
                {

                    if (listView.Items.Count > 0)
                    {
                        var bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.UriSource = new Uri(arquivoSelecionado.FullName);
                        bi.EndInit();

                        var vis = new DrawingVisual();
                        var dc = vis.RenderOpen();
                        if (MessageBox.Show("Imprimir em papel de segurança?", "Papel de Segurança", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            dc.DrawImage(bi, new Rect { Width = bi.Width - 190, Height = bi.Height - 180, X = 100, Y = 150 });
                        else
                            dc.DrawImage(bi, new Rect { Width = bi.Width - 100, Height = bi.Height - 100 });
                        dc.Close();

                        var pdialog = new PrintDialog();
                        if (pdialog.ShowDialog() == true)
                        {
                            pdialog.PrintVisual(vis, arquivoSelecionado.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro na Impressão do documento. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private int ObterUltimaFicha()
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {
                string comando = "Select MAX(FICHA) from FICHAS";
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    return (int)cmdTotal.ExecuteScalar();
                }
            }
        }

        private int ObterpProximaFicha(int numeroFicha, int ultimaFinha)
        {

            if (numeroFicha == ultimaFinha)
                return ultimaFinha;
            else
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
                {
                    string comando = string.Empty;

                    if (ckbSinalPubilco.IsChecked.Value == false)
                        comando = "Select MIN(FICHA) from FICHAS WHERE SPU = 'N' AND FICHA > " + numeroFicha;
                    else
                        comando = "Select MIN(FICHA) from FICHAS WHERE SPU = 'S' AND FICHA > " + numeroFicha;
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;
                        return (int)cmdTotal.ExecuteScalar();
                    }
                }
        }


        public void PassarProximo()
        {
            try
            {
                if (fichaSelecionada != null || ultimaFichaSelecionada != 0)
                {
                    int ultimaFicha = ObterUltimaFicha();
                    int fichaProx = ObterpProximaFicha(fichaSelecionada.Ficha, ultimaFicha);

                    var proxima = string.Format("{0}", fichaProx);
                    txtNome.Text = proxima;
                    CosultaFirma();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int ObterpFichaAnterior(int numeroFicha)
        {
            if (numeroFicha == 1)
                return numeroFicha;
            else
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
                {
                    string comando = string.Empty;

                    if (ckbSinalPubilco.IsChecked.Value == false)
                        comando = "Select MAX(FICHA) from FICHAS WHERE SPU = 'N' AND FICHA < " + numeroFicha;
                    else
                        comando = "Select MAX(FICHA) from FICHAS WHERE SPU = 'S' AND FICHA < " + numeroFicha;
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        return (int)cmdTotal.ExecuteScalar();
                    }
                }
        }



        private void PassarAnterior()
        {
            try
            {
                if (fichaSelecionada != null || ultimaFichaSelecionada != 0)
                {
                    int fichaAnt = ObterpFichaAnterior(fichaSelecionada.Ficha);

                    var anterior = string.Format("{0}", fichaAnt);
                    txtNome.Text = anterior;
                    CosultaFirma();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (listView.ItemsSource != null)
                {
                    arquivoSelecionado = (FileInfo)listView.SelectedItem;

                    image = new BitmapImage();
                    image.BeginInit();
                    image = (byteArrayToImage(File.ReadAllBytes(arquivoSelecionado.FullName)));

                    image2.BeginInit();
                    image2.Source = image;
                    image2.EndInit();
                    btnAlterar.IsEnabled = true;
                }
                else
                {
                    image2.Source = null;
                    btnAlterar.IsEnabled = false;
                }


                if (listView.SelectedIndex == arquivos.Count() - 1)
                {
                    btnExcluir.IsEnabled = true;
                }
                else
                {
                    btnExcluir.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message);
            }
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void listView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (arquivos.Count > 0)
                    DeletarDosArquivos();
            }
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Confirma a exclusão do arquivo " + arquivoSelecionado.Name + "?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (listView.SelectedIndex > -1)
                    {
                        nomeArquivo = arquivoSelecionado.FullName;

                        if (File.Exists(nomeArquivo))
                            File.Delete(nomeArquivo);


                        CarregarImagens();
                        listView.SelectedItem = arquivos.LastOrDefault();
                        listView.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na Exclusão do documento. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnAlterar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!diretorioAtual.Exists)
                    diretorioAtual.Create();


                nomeArquivo = ((FileInfo)listView.SelectedItem).FullName;


                AlterarScannear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na digitalização do documento. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            PassarAnterior();
        }

        private void btnProximo_Click(object sender, RoutedEventArgs e)
        {
            PassarProximo();
        }

        private void txtNome_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CosultaFirma();
            }
        }

        private void CosultaFirma()
        {
            if (txtNome.Text.Trim() != "")
            {
                fichasConsultadas = new List<FichaFirmas>();

                ConsultarFirmaFicha("nome", consultaSinal);
                ConsultarFirmaFicha("ficha", consultaSinal);
                ConsultarFirmaFicha("cpf", consultaSinal);

                dataGridFicha.ItemsSource = fichasConsultadas.OrderBy(p => p.Ficha);

                dataGridFicha.SelectedIndex = -1;

                if (fichasConsultadas.Count > 0)
                {
                    dataGridFicha.SelectedIndex = 0;
                    btnNovo.IsEnabled = true;
                    btnAnterior.IsEnabled = true;
                    btnProximo.IsEnabled = true;
                    gridAuto.IsEnabled = true;
                }
                else
                {
                    image2.Source = null;
                    image2.BeginInit();
                    image2.EndInit();
                    listView.ItemsSource = null;
                    lblTitulo.Content = "Ficha não encontrada";
                    btnNovo.IsEnabled = false;
                    btnAnterior.IsEnabled = false;
                    btnProximo.IsEnabled = false;
                    gridAuto.IsEnabled = false;
                }

            }
        }

        private void txtNome_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            arquivos = null;

            arquivoSelecionado = null;

            diretorioAtual = null;
            image2.BeginInit();
            image2.EndInit();

            this.Close();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!diretorioAtual.Exists)
                {
                    diretorioAtual.Create();
                    caminho = diretorioAtual.FullName;
                }

                if (ckbAuto.IsChecked.Value == false)
                    Scannear();
                else
                    if (MessageBox.Show("Clique em Ok para iniciar a digitalização automática.", "Iniciar", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                        AutoScanner();

            }
            catch
            {
                MessageBox.Show("Erro na digitalização do documento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void AutoScanner()
        {
            int qtdFichas = Convert.ToInt32(txtQtdAuto.Text);

            int qtdPaginas = Convert.ToInt32(txtQtdPaginas.Text);

            for (int i = 0; i < qtdFichas; i++)
            {

                txtQtdAuto.Text = string.Format("{0} de {1}", i + 1, qtdFichas);

                //if (arquivos.Count > 0)
                //{
                //    foreach (var item in arquivos)
                //    {
                //        if (item.Exists)
                //            item.Delete();
                //    }
                //}

                CarregarImagens();

                for (int e = 0; e < qtdPaginas; e++)
                {
                    if (desativarAuto == true)
                    {
                        txtQtdAuto.Text = qtdFichas.ToString();
                        txtQtdPaginas.Text = qtdPaginas.ToString();
                        desativarAuto = false;
                        return;
                    }

                    txtQtdPaginas.Text = string.Format("{0} de {1}", e + 1, qtdPaginas);
                    Scannear();
                }

                CarregarImagens();

                if (desativarAuto == true)
                {
                    txtQtdAuto.Text = qtdFichas.ToString();
                    txtQtdPaginas.Text = qtdPaginas.ToString();
                    desativarAuto = false;
                    return;
                }


                if (i != qtdFichas - 1)
                    PassarProximo();

            }

            txtQtdAuto.Text = qtdFichas.ToString();

            txtQtdPaginas.Text = qtdPaginas.ToString();

            desativarAuto = false;
        }


        public static void ComprimirImagem(System.Drawing.Image imagem, long qualidade, string filepath)
        {
            var param = new EncoderParameters(1);
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualidade);
            var codec = ObterCodec(imagem.RawFormat);
            imagem.Save(filepath, codec, param);
        }


        public static void ComprimirImagemTotal(System.Drawing.Image imagem, long qualidade, string filepath)
        {
            var param = new EncoderParameters(1);
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualidade);
            var codec = GetEncoder(imagem.RawFormat);
            imagem.Save(filepath, codec, param);
        }

        private static ImageCodecInfo ObterCodec(ImageFormat formato)
        {
            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == formato.Guid);
            if (codec == null) throw new NotSupportedException();
            return codec;
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


      
        private void Scannear()
        {
            
            CommonDialogClass commonDialogClass = new CommonDialogClass();
            
            Device scannerDevice;

            
            if (ckbAuto.IsChecked.Value == false)
                scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);
            else
                scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);


            this.Activate();

            if (scannerDevice != null)
            {
                Item scannnerItem = scannerDevice.Items[1];

                if (consultaSinal == false)
                {

                    if (rbPequena.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1008, 685, 0, 0, 0);

                    if (rbNormal.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1063, 790, 0, 0, 0);

                    if (rbMedia.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1140, 835, 0, 0, 0);

                    if (rbGrande.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1400, 760, 0, 0, 0);
                }
                else
                {
                    AdjustScannerSettings(scannnerItem, 200, 0, 0, 1700, 2100, 0, 0, 0);
                }

                nomeArquivo = ObterNomeArquivo(arquivos.Count + 1);

                object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatJPEG, false);


                if (scanResult != null)
                {

                    ImageFile image = (ImageFile)scanResult;

                    string fileName = nomeArquivo;

                    string fileTemp = string.Format(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\FichaBalcao\{0}-{1}.jpg", fichaSelecionada.Ficha, arquivos.Count + 1);

                    if (File.Exists(fileTemp))
                        File.Delete(fileTemp);

                    SaveImageToTiffFile(image, fileTemp);

                    Bitmap img = new Bitmap(fileTemp);


                    ComprimirImagem(img, 20L, nomeArquivo);

                    var nomearq = new FileInfo(nomeArquivo);

                    arquivos.Add(nomearq);
                    arquivos.OrderBy(p => p.Name);
                    arquivoSelecionado = nomearq;

                    listView.ItemsSource = arquivos;

                    listView.Items.Refresh();

                    listView.SelectedItem = arquivoSelecionado;


                    img.Dispose();


                    if (File.Exists(fileTemp))
                        File.Delete(fileTemp);


                }
                else
                {
                    if (ckbAuto.IsChecked.Value == true)
                        desativarAuto = true;
                }
            }
            else
            {
                if (ckbAuto.IsChecked.Value == true)
                    desativarAuto = true;
            }
        }


        private void AlterarScannear()
        {
            CommonDialogClass commonDialogClass = new CommonDialogClass();
            Device scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);

            this.Activate();
            if (scannerDevice != null)
            {
                Item scannnerItem = scannerDevice.Items[1];

                if (consultaSinal == false)
                {
                    if (rbPequena.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1008, 685, 0, 0, 0);

                    if (rbNormal.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1063, 790, 0, 0, 0);

                    if (rbMedia.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1140, 835, 0, 0, 0);

                    if (rbGrande.IsChecked == true)
                        AdjustScannerSettings(scannnerItem, 200, 0, 0, 1400, 760, 0, 0, 0);
                }
                else
                {
                    AdjustScannerSettings(scannnerItem, 200, 0, 0, 1700, 2100, 0, 0, 0);
                }

                nomeArquivo = arquivoSelecionado.FullName;

                object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatJPEG, false);
                
                if (scanResult != null)
                {
                    ImageFile image = (ImageFile)scanResult;

                    string fileTemp = string.Format(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\FichaBalcao\{0}", arquivoSelecionado.Name);

                    if (File.Exists(fileTemp))
                        File.Delete(fileTemp);

                    SaveImageToTiffFile(image, fileTemp);

                    int indexSelecionado = listView.SelectedIndex;

                    if (File.Exists(nomeArquivo))
                        File.Delete(nomeArquivo);

                    Bitmap img = new Bitmap(fileTemp);

                    ComprimirImagem(img, 20L, nomeArquivo);

                    CarregarImagens();

                    listView.SelectedIndex = indexSelecionado;

                    img.Dispose();

                    if (File.Exists(fileTemp))
                        File.Delete(fileTemp);
                }
            }
        }

      
      

        public Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }





        private string ObterNomeArquivo(int qtd)
        {
            return string.Format(@"{0}\{1}-{2}.jpg", diretorioAtual, fichaSelecionada.Ficha, qtd);
        }


        private static void SaveImageToTiffFile(ImageFile image, string fileName)
        {
            ImageProcess imgProcess = new ImageProcess();
            object convertFilter = "Convert";
            string convertFilterID = imgProcess.FilterInfos.get_Item(ref convertFilter).FilterID;
            imgProcess.Filters.Add(convertFilterID, 0);

            SetWIAProperty(imgProcess.Filters[imgProcess.Filters.Count].Properties, "FormatID", WIA.FormatID.wiaFormatJPEG);


            image = imgProcess.Apply(image);

            if (File.Exists(fileName))
                File.Delete(fileName);


            image.SaveFile(fileName);


        }

        private static void AdjustScannerSettings(IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel,
        int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents, int colorMode)
        {
            const string WIA_SCAN_COLOR_MODE = "6146";
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";

            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_COLOR_MODE, colorMode);

        }


        private static void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);

            prop.set_Value(ref propValue);
        }

        private void ConsultarFirmaFicha(string tipo, bool sinal)
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {
                string comando = string.Empty;
                conn.Open();

                try
                {
                    if (sinal == false)
                    {
                        if (tipo == "ficha")
                            comando = "SELECT * FROM fichas where spu = 'N' and ficha = " + txtNome.Text;

                        if (tipo == "nome")
                            comando = "SELECT * FROM fichas where spu = 'N' and nome like '" + txtNome.Text + "%'";

                        if (tipo == "cpf")
                            comando = "SELECT * FROM fichas where spu = 'N' and cpf = '" + txtNome.Text + "'";
                    }
                    else
                    {
                        if (tipo == "ficha")
                            comando = "SELECT * FROM fichas where spu = 'S' and ficha = " + txtNome.Text;

                        if (tipo == "nome")
                            comando = "SELECT * FROM fichas where spu = 'S' and nome like '" + txtNome.Text + "%'";

                        if (tipo == "cpf")
                            comando = "SELECT * FROM fichas where spu = 'S' and cpf = '" + txtNome.Text + "'";
                    }

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        FbDataReader dr;

                        dr = cmdTotal.ExecuteReader();

                        FichaFirmas Ficha;

                        while (dr.Read())
                        {
                            Ficha = new FichaFirmas();

                            if (dr["ID"].ToString() != "")
                                Ficha.FichaId = (int)dr["ID"];

                            if (dr["FICHA"].ToString() != "")
                                Ficha.Ficha = (int)dr["FICHA"];

                            Ficha.Livro = dr["LIVRO"].ToString();

                            Ficha.Folha = dr["FOLHA"].ToString();

                            Ficha.Selo = dr["SELO"].ToString();

                            Ficha.Aleatorio = dr["ALEATORIO"].ToString();

                            if (dr["DATA"].ToString() != "")
                                Ficha.Data = (DateTime)dr["DATA"];

                            Ficha.Termo = dr["TERMO"].ToString();

                            Ficha.Nome = dr["NOME"].ToString();

                            Ficha.Nome2 = dr["NOME2"].ToString();

                            Ficha.Tpj = dr["TPJ"].ToString();

                            Ficha.Telefone = dr["TELEFONE"].ToString();

                            Ficha.Autenticidade = dr["AUTENTICIDADE"].ToString();

                            if (dr["DT_NASCIMENTO"].ToString() != "")
                                Ficha.DataNascimento = (DateTime)dr["DT_NASCIMENTO"];

                            Ficha.Rg = dr["RG"].ToString();

                            Ficha.OrgaoEmissor = dr["ORGAO_RG"].ToString();

                            if (dr["DT_EMISSAO_RG"].ToString() != "")
                                Ficha.DataEmissaoRg = (DateTime)dr["DT_EMISSAO_RG"];

                            Ficha.OrgaoEmissor = dr["NACIONALIDADE"].ToString();

                            if (dr["ONU"].ToString() != "")
                                Ficha.Onu = (int)dr["ONU"];

                            Ficha.Naturalidade = dr["NATURALIDADE"].ToString();

                            Ficha.Profissao = dr["PROFISSAO"].ToString();

                            if (dr["ESTADO_CIVIL"].ToString() != "")
                                Ficha.EstadoCivil = (int)dr["ESTADO_CIVIL"];

                            Ficha.Conjuge = dr["CONJUGE"].ToString();

                            Ficha.Cep = dr["CEP"].ToString();

                            Ficha.Endereco = dr["ENDERECO"].ToString();

                            Ficha.Bairro = dr["BAIRRO"].ToString();

                            Ficha.Numero = dr["NUMERO"].ToString();

                            Ficha.Complemento = dr["COMPLEMENTO"].ToString();

                            Ficha.Uf = dr["UF"].ToString();

                            Ficha.Cidade = dr["CIDADE"].ToString();

                            Ficha.NomePai = dr["NOME_PAI"].ToString();

                            Ficha.NomeMae = dr["NOME_MAE"].ToString();

                            Ficha.Observacao = dr["OBSERVACAO"].ToString();

                            Ficha.Cpf = dr["CPF"].ToString();

                            Ficha.Escrevente = dr["ESCREVENTE"].ToString();

                            Ficha.ComputerId = dr["COMPUTERID"].ToString();

                            Ficha.Cancelado = dr["CANCELADO"].ToString();

                            Ficha.Acervo = dr["ACERVO"].ToString();

                            Ficha.Externo = dr["EXTERNO"].ToString();

                            Ficha.Visual = dr["VISUAL"].ToString();

                            Ficha.Biometria = dr["BIOMETRIA"].ToString();

                            Ficha.Sincronizar = dr["SINCRONIZAR"].ToString();

                            Ficha.Email = dr["EMAIL"].ToString();

                            fichasConsultadas.Add(Ficha);

                        }
                    }



                }
                catch (Exception)
                {

                }

            }
        }

        private void dataGridFicha_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fichaSelecionada = (FichaFirmas)dataGridFicha.SelectedItem;



            if (fichaSelecionada != null)
            {
                ultimaFichaSelecionada = fichaSelecionada.Ficha;
                CarregarImagens();
                btnAnterior.IsEnabled = true;
                btnProximo.IsEnabled = true;
                lblTitulo.Content = string.Format("Ficha Selecionada: {0} - {1}", fichaSelecionada.Ficha, fichaSelecionada.Nome);
            }
            else
            {
                if (txtNome.Text == "")
                {
                    btnAnterior.IsEnabled = false;
                    btnProximo.IsEnabled = false;
                }
                lblTitulo.Content = "Consulta de Fichas do Balcão";
            }


        }






        private BitmapImage byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(byteArrayIn, 0, byteArrayIn.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public void CarregarImagens()
        {
            try
            {


                diretorioAtual = new DirectoryInfo(caminho);

                if (diretorioAtual.Exists)
                {
                    listView.ItemsSource = null;
                    listView.Items.Clear();

                    arquivos = diretorioAtual.GetFiles(string.Format("{0}-*", fichaSelecionada.Ficha)).ToList();

                    listView.ItemsSource = arquivos;

                    listView.SelectedIndex = 0;

                    totalPaginas = arquivos.Count;
                }
                else
                {
                    listView.ItemsSource = null;
                    listView.Items.Clear();

                    arquivos = new List<FileInfo>();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar as imagens desta matrícula. \n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            consultaSinal = true;
            caminho = @"\\SERVIDOR\Cartorio\Total\firmas\SP";

            CosultaFirma();
            image2.Height = 1119;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            consultaSinal = false;
            caminho = @"\\SERVIDOR\Cartorio\Total\firmas\Arqs001";

            CosultaFirma();
            image2.Height = 384;
        }

        private void image2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (arquivos.Count > 0)
                    DeletarDosArquivos();
            }
        }

        private void ScrollViewer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (arquivos.Count > 0)
                    DeletarDosArquivos();
            }
        }

        private void dataGridFicha_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (arquivos.Count > 0)
                    DeletarDosArquivos();
            }
        }

        private void ckbAuto_Checked(object sender, RoutedEventArgs e)
        {
            txtQtdAuto.IsEnabled = true;
            txtQtdAuto.Text = "20";
            txtQtdAuto.Focus();
            txtQtdAuto.SelectAll();

            txtQtdPaginas.IsEnabled = true;
            txtQtdPaginas.Text = "2";
        }

        private void ckbAuto_Unchecked(object sender, RoutedEventArgs e)
        {
            txtQtdAuto.IsEnabled = false;
            txtQtdAuto.Text = "";

            txtQtdPaginas.IsEnabled = false;
            txtQtdPaginas.Text = "";
        }

        private void txtQtdAuto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdAuto.Text == "")
            {
                txtQtdAuto.Text = "20";
            }
        }

        private void txtQtdAuto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);


            if (e.Key == Key.Enter)
            {
                txtQtdPaginas.Focus();
                txtQtdPaginas.SelectAll();
            }
        }


        private void txtQtdPaginas_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdPaginas.Text == "")
            {
                txtQtdPaginas.Text = "2";
            }
        }

        private void txtQtdPaginas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

            if (e.Key == Key.Enter)
            {
                btnNovo.Focus();
            }

        }

        private void gridAuto_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
