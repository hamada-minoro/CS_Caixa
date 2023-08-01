using CS_Caixa.Controls;
using CS_Caixa.Models;
using Microsoft.Win32;
using NTwain;
using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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
using WIA;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinVisualizarDigitalizarRgi.xaml
    /// </summary>
    public partial class WinVisualizarDigitalizarRgi : Window
    {
        IndiceRegistro _indiceRegistro;

        string caminho = @"\\SERVIDOR\Cartorio\Digitalizacao\Imagens livro OUTROS";

        string pasta;

        List<FileInfo> arquivos = new List<FileInfo>();

        BitmapImage image;

        DirectoryInfo diretorioAtual;

        FileInfo arquivoSelecionado;

        string nomeArquivo;

        WinIndiceRgi _indiceRgi;

        Usuario _usuario;


        public WinVisualizarDigitalizarRgi(IndiceRegistro indiceRegistro, WinIndiceRgi indiceRgi, Usuario usuario)
        {
            _indiceRegistro = indiceRegistro;
            _indiceRgi = indiceRgi;
            _usuario = usuario;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarInicio();
        }


        private void CarregarInicio()
        {
            try
            {
                pasta = null;

                if (_indiceRegistro.Livro == "2")
                    caminho = @"\\SERVIDOR\c\Cartorio\Digitalizacao\Imagens Matriculas RGI";

                if (_indiceRegistro.Livro == "3AUX")
                    caminho = @"\\SERVIDOR\c\Cartorio\Digitalizacao\Imagens livro 3AUX";


                diretorioAtual = null;

                pasta = ClassIndiceRgi.VerificarPastaImagem(Convert.ToInt32(_indiceRegistro.Ordem));

                if (pasta != "Maior que 99999")
                {

                    diretorioAtual = new DirectoryInfo(caminho);

                    if (!diretorioAtual.Exists)
                        diretorioAtual.Create();

                    caminho = string.Format(@"{0}\{1}", caminho, pasta);

                    lblTitulo.Content = string.Format("Matrícula {0} - Livro {1}", _indiceRegistro.Ordem, _indiceRegistro.Livro);

                    CarregarImagens();
                }
                else
                {
                    MessageBox.Show("Matrícula maior que 99999. Favor entrar em contado com o Desenvolvedor do Sistema.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível identificar a imagem desta matrícula. \n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Bitmap bitmap;
        string caminhoPadrao = string.Format(@"C:\Users\{0}\Documents\Eloam\Image", Environment.MachineName);
        private void ObterImagem(string tipo)
        {

            string arquivoSel = "";

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = caminhoPadrao;
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                arquivoSel = dlg.FileName;

                bitmap = new Bitmap(arquivoSel);
                var aguarde = new Aguarde(this);
                aguarde.ShowIcon = false;
                aguarde.ShowDialog();
                bitmap = ResizeImage(System.Drawing.Image.FromFile(arquivoSel), 1700, 2100);
                caminhoPadrao = dlg.FileName;


                if (tipo == "novo")
                {
                    if (bitmap != null)
                    {

                        nomeArquivo = ObterNomeArquivo();

                        if (cmbPosicaoNovo.Items.Count != cmbPosicaoNovo.SelectedIndex + 1)
                            nomeArquivo = ReordenarMatriculas(cmbPosicaoNovo.Text);

                        if (File.Exists(nomeArquivo))
                            File.Delete(nomeArquivo);

                        try
                        {
                            SalvarArquivo(bitmap, nomeArquivo);
                        }
                        catch (Exception)
                        {

                        }

                        var nomearq = new FileInfo(nomeArquivo);

                        CarregarImagens();

                        listView.ItemsSource = arquivos;

                        listView.Items.Refresh();

                        listView.SelectedItem = arquivoSelecionado;
                    }
                }
                else
                {

                    int indexSelecionado = listView.SelectedIndex;

                    if (File.Exists(nomeArquivo))
                        File.Delete(nomeArquivo);


                    try
                    {
                        SalvarArquivo(bitmap, nomeArquivo);
                    }
                    catch (Exception)
                    {

                    }

                    CarregarImagens();

                    listView.SelectedIndex = indexSelecionado;
                }
            }
        }



        private string ReordenarMatriculas(string posicao)
        {
            string nomeMatricula = arquivos[0].FullName.Substring(0, arquivos[0].FullName.Length - 4) + posicao;
            var posi = Convert.ToInt32(posicao);

            FileInfo arquivo;

            for (int i = arquivos.Count - 1; i >= posi - 1; i--)
            {
                arquivo = arquivos[i];
                string valor = string.Format("{0:0000}", i + 2);
                string caminho = arquivo.FullName.Substring(0, arquivo.FullName.Length - 4) + valor;
                File.Move(arquivo.FullName, caminho);
            }



            return nomeMatricula;
        }


        public void SalvarArquivo(Bitmap imagem, string caminho)
        {
            Bitmap myBitmap;
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Create a Bitmap object based on a BMP file.
            myBitmap = imagem;

            // Get an ImageCodecInfo object that represents the TIFF codec.
            myImageCodecInfo = GetEncoderInfo("image/tiff");

            // Create an Encoder object based on the GUID
            // for the ColorDepth parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.ColorDepth;

            myEncoderParameters = new EncoderParameters(3);

            myEncoderParameter =
                new EncoderParameter(myEncoder, 1L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            myEncoderParameter =
               new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionLZW);
            myEncoderParameters.Param[1] = myEncoderParameter;

            myEncoderParameter =
               new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.CompressionLZW);
            myEncoderParameters.Param[2] = myEncoderParameter;


            myBitmap.Save(caminho, myImageCodecInfo, myEncoderParameters);

        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }


        public Bitmap GrayScaleFilter(Bitmap image)
        {
            Bitmap grayScale = new Bitmap(image.Width, image.Height);

            for (Int32 y = 0; y < grayScale.Height; y++)
                for (Int32 x = 0; x < grayScale.Width; x++)
                {
                    System.Drawing.Color c = image.GetPixel(x, y);

                    Int32 gs = (Int32)(c.R * 0.03 + c.G * 0.59 + c.B * 0.11);

                    grayScale.SetPixel(x, y, System.Drawing.Color.FromArgb(gs, gs, gs));
                }

            return grayScale;
        }


        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {

            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);


            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;


                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void Scannear()
        {


            CommonDialogClass commonDialogClass = new CommonDialogClass();

            Device scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);

            this.Activate();


            if (scannerDevice != null)
            {
                Item scannnerItem = scannerDevice.Items[1];

                AdjustScannerSettings(scannnerItem, 200, 0, 0, 1700, 2565, 0, 0, 4);

                nomeArquivo = ObterNomeArquivo();

                object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatTIFF, false);


                if (scanResult != null)
                {

                    ImageFile image = (ImageFile)scanResult;
                    string fileName = nomeArquivo;


                    if (File.Exists(nomeArquivo))
                        File.Delete(nomeArquivo);

                    SaveImageToTiffFile(image, fileName);

                    var nomearq = new FileInfo(nomeArquivo);

                    arquivos.Add(nomearq);
                    arquivos.OrderBy(p => p.Name);
                    arquivoSelecionado = nomearq;

                    listView.ItemsSource = arquivos;

                    listView.Items.Refresh();

                    listView.SelectedItem = arquivoSelecionado;

                }

            }
        }

        private void AlterarScannear()
        {
            CommonDialogClass commonDialogClass = new CommonDialogClass();
            Device scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);

            this.Activate();
            if (scannerDevice != null)
            {
                Item scannnerItem = scannerDevice.Items[1];

                AdjustScannerSettings(scannnerItem, 200, 0, 0, 1700, 2565, 0, 0, 4);

                nomeArquivo = arquivoSelecionado.FullName;

                object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatTIFF, false);



                if (scanResult != null)
                {

                    ImageFile image = (ImageFile)scanResult;


                    int indexSelecionado = listView.SelectedIndex;

                    if (File.Exists(nomeArquivo))
                        File.Delete(nomeArquivo);
                    SaveImageToTiffFile(image, nomeArquivo);

                    CarregarImagens();

                    listView.SelectedIndex = indexSelecionado;

                }

            }
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

        private static void SaveImageToTiffFile(ImageFile image, string fileName)
        {
            ImageProcess imgProcess = new ImageProcess();
            object convertFilter = "Convert";
            string convertFilterID = imgProcess.FilterInfos.get_Item(ref convertFilter).FilterID;
            imgProcess.Filters.Add(convertFilterID, 0);
            SetWIAProperty(imgProcess.Filters[imgProcess.Filters.Count].Properties, "FormatID", WIA.FormatID.wiaFormatTIFF);
            image = imgProcess.Apply(image);

            if (File.Exists(fileName))
                File.Delete(fileName);


            image.SaveFile(fileName);
        }


        private void CarregarImagens()
        {
            try
            {
                diretorioAtual = new DirectoryInfo(caminho);

                if (diretorioAtual.Exists)
                {
                    listView.ItemsSource = null;
                    listView.Items.Clear();

                    arquivos = diretorioAtual.GetFiles().Where(p => p.Name.Contains(string.Format("{0:00000000}", Convert.ToInt32(_indiceRegistro.Ordem)))).ToList();

                    listView.ItemsSource = arquivos;

                    listView.SelectedIndex = 0;

                    List<string> posicoes = new List<string>();

                    foreach (var item in arquivos)
                    {
                        posicoes.Add(item.Name.Substring(item.Name.Length - 4, 4));
                    }


                    var valor = Convert.ToInt32(posicoes.LastOrDefault()) + 1;
                    string ultima = string.Format("{0:0000}", valor);
                    posicoes.Add(ultima);
                    cmbPosicaoNovo.ItemsSource = posicoes;
                    cmbPosicaoNovo.SelectedIndex = 0;
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

      
        private string ObterNomeArquivo()
        {

            try
            {
                if (arquivos.Count > 0)
                {
                    var nomeArquivo = arquivos.LastOrDefault();

                    int retornoPagina = 0;

                    for (int i = 0; i < arquivos.Count; i++)
                    {
                        var pagina = Convert.ToInt16(arquivos[i].Name.Substring(9, 4));

                        if (pagina != i + 1)
                        {
                            return string.Format(@"{0}\{1:00000000}.{2:0000}", diretorioAtual, Convert.ToInt32(_indiceRegistro.Ordem), i + 1);
                        }

                        retornoPagina = i + 2;
                    }

                    return string.Format(@"{0}\{1:00000000}.{2:0000}", diretorioAtual, Convert.ToInt32(_indiceRegistro.Ordem), retornoPagina);
                }
                else
                {
                    if (!diretorioAtual.Exists)
                        diretorioAtual.Create();


                    return string.Format(@"{0}\{1:00000000}.{2:0000}", diretorioAtual, Convert.ToInt32(_indiceRegistro.Ordem), 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível obter o nome do arquivo. \n" + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
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
                Scannear();


            }
            catch
            {
                MessageBox.Show("Erro na digitalização do documento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    btnImportarAlterar.IsEnabled = true;
                }
                else
                {
                    image2.Source = null;
                    btnAlterar.IsEnabled = false;
                    btnImportarAlterar.IsEnabled = false;
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



        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

            arquivos = null;

            arquivoSelecionado = null;

            diretorioAtual = null;
            image2.BeginInit();
            image2.EndInit();

            this.Close();
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

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (arquivoSelecionado != null)
                    System.Diagnostics.Process.Start(arquivoSelecionado.FullName);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void listView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (listView.SelectedIndex == arquivos.Count() - 1)
            {

                if (e.Key == Key.Delete)
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
        }




        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F11)
            {
                ObterImagem("novo");
            }

            if (e.Key == Key.F2)
            {
                ObterImagem("alterar");
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

            if (e.Key == Key.F1)
            {
                try
                {

                    if (!diretorioAtual.Exists)
                    {
                        diretorioAtual.Create();
                        caminho = diretorioAtual.FullName;
                    }

                    Scannear();


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
                    if (_usuario.ImprimirMatricula == true)
                    {
                        if (listView.Items.Count > 0)
                        {
                            var imprimir = new WinImpressaoMatricula(arquivos);
                            imprimir.Owner = this;
                            imprimir.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuário logado não tem permissão para imprimir matrícula.", "Permissão negada", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro na Impressão do documento. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }



        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            PassarAnterior();
        }


        private void PassarProximo()
        {
            try
            {

                var novoNumeroMat = Convert.ToInt32(_indiceRegistro.Ordem) + 1;
                var classIndiceRgi = new ClassIndiceRgi();
                var novoRegistro = classIndiceRgi.ListarIndiceRegistroMatricula(string.Format("{0}", novoNumeroMat));
                if (novoRegistro.Count > 0)
                {

                    _indiceRegistro = novoRegistro.FirstOrDefault();

                    CarregarInicio();
                }
                else
                {
                    for (int i = novoNumeroMat + 1; i < novoNumeroMat + 1000; i++)
                    {
                        novoRegistro = classIndiceRgi.ListarIndiceRegistroMatricula(i.ToString());
                        if (novoRegistro.Count > 0)
                        {
                            _indiceRegistro = novoRegistro.FirstOrDefault();

                            CarregarInicio();

                            break;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PassarAnterior()
        {
            try
            {
                var novoNumeroMat = Convert.ToInt32(_indiceRegistro.Ordem) - 1;
                var classIndiceRgi = new ClassIndiceRgi();
                var novoRegistro = classIndiceRgi.ListarIndiceRegistroMatricula(novoNumeroMat.ToString());


                if (novoRegistro.Count > 0)
                {

                    _indiceRegistro = novoRegistro.FirstOrDefault();

                    CarregarInicio();

                }
                else
                {
                    for (int i = novoNumeroMat - 1; i > novoNumeroMat - 1000; i--)
                    {
                        novoRegistro = classIndiceRgi.ListarIndiceRegistroMatricula(i.ToString());
                        if (novoRegistro.Count > 0)
                        {
                            _indiceRegistro = novoRegistro.FirstOrDefault();

                            CarregarInicio();

                            break;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProximo_Click(object sender, RoutedEventArgs e)
        {
            PassarProximo();
        }


        public static string[] ConvertJpegToTiff(string[] fileNames, bool isMultipage)
        {
            EncoderParameters encoderParams = new EncoderParameters(1);
            ImageCodecInfo tiffCodecInfo = ImageCodecInfo.GetImageEncoders()
                .First(ie => ie.MimeType == "image/tiff");

            string[] tiffPaths = null;
            if (isMultipage)
            {
                tiffPaths = new string[1];
                System.Drawing.Image tiffImg = null;
                try
                {
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        if (i == 0)
                        {
                            tiffPaths[i] = String.Format("{0}\\{1}.tif",
                                System.IO.Path.GetDirectoryName(fileNames[i]),
                                System.IO.Path.GetFileNameWithoutExtension(fileNames[i]));

                            // Initialize the first frame of multipage tiff.
                            tiffImg = System.Drawing.Image.FromFile(fileNames[i]);
                            encoderParams.Param[0] = new EncoderParameter(
                                System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                            tiffImg.Save(tiffPaths[i], tiffCodecInfo, encoderParams);
                        }
                        else
                        {
                            // Add additional frames.
                            encoderParams.Param[0] = new EncoderParameter(
                                System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                            using (System.Drawing.Image frame = System.Drawing.Image.FromFile(fileNames[i]))
                            {
                                tiffImg.SaveAdd(frame, encoderParams);
                            }
                        }

                        if (i == fileNames.Length - 1)
                        {
                            // When it is the last frame, flush the resources and closing.
                            encoderParams.Param[0] = new EncoderParameter(
                                System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                            tiffImg.SaveAdd(encoderParams);
                        }
                    }
                }
                finally
                {
                    if (tiffImg != null)
                    {
                        tiffImg.Dispose();
                        tiffImg = null;
                    }
                }
            }
            else
            {
                tiffPaths = new string[fileNames.Length];

                for (int i = 0; i < fileNames.Length; i++)
                {
                    tiffPaths[i] = String.Format("{0}\\{1}.tif",
                        System.IO.Path.GetDirectoryName(fileNames[i]),
                        System.IO.Path.GetFileNameWithoutExtension(fileNames[i]));

                    // Save as individual tiff files.
                    using (System.Drawing.Image tiffImg = System.Drawing.Image.FromFile(fileNames[i]))
                    {

                        using (Bitmap newBitmap = new Bitmap(tiffImg))
                        {
                            newBitmap.SetResolution(200, 200);

                            newBitmap.Save(tiffPaths[i], ImageFormat.Tiff);
                        }

                    }
                }
            }

            return tiffPaths;
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            ObterImagem("novo");
        }

        private void btnImportarAlterar_Click(object sender, RoutedEventArgs e)
        {
            ObterImagem("alterar");
        }


    }


}

