using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para AguardeTotal.xaml
    /// </summary>
    public partial class AguardeTotal : Window
    {

        BackgroundWorker worker;
        WinTotal _winTotal;
        int ficha = 0;
        public AguardeTotal(WinTotal winTotal)
        {
            _winTotal = winTotal;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }


        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int a = 0; a < _winTotal.totalPaginas; a++)
            {
                ficha = a + 1;
                Thread.Sleep(1);
                worker.ReportProgress(a + 1);

                _winTotal.arquivoSelecionado = _winTotal.arquivos[a];

                if (_winTotal.arquivoSelecionado != null)
                {

                    string fileTemp = string.Format(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\FichaBalcao\{0}", _winTotal.arquivoSelecionado.Name);

                    _winTotal.nomeArquivo = _winTotal.arquivoSelecionado.FullName;

                    if (File.Exists(fileTemp))
                        File.Delete(fileTemp);

                    _winTotal.imgLoad = new Bitmap(_winTotal.nomeArquivo);

                    if (_winTotal.imgLoad.Width >= 836)
                    {
                        _winTotal.img = new Bitmap(_winTotal.CropBitmap(_winTotal.imgLoad, 0, 0, 529, 390));

                        _winTotal.img.Save(fileTemp);
                        _winTotal.imgLoad.Dispose();

                        // int indexSelecionado = listView.SelectedIndex;

                        if (File.Exists(_winTotal.nomeArquivo))
                            File.Delete(_winTotal.nomeArquivo);



                        using (Bitmap bmp1 = new Bitmap(fileTemp))
                        {
                            _winTotal.jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                            // Create an Encoder object based on the GUID  
                            // for the Quality parameter category.  
                            System.Drawing.Imaging.Encoder myEncoder =
                                System.Drawing.Imaging.Encoder.Quality;

                            // Create an EncoderParameters object.  
                            // An EncoderParameters object has an array of EncoderParameter  
                            // objects. In this case, there is only one  
                            // EncoderParameter object in the array.  
                            _winTotal.myEncoderParameters = new EncoderParameters(1);

                            _winTotal.myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                            _winTotal.myEncoderParameters.Param[0] = _winTotal.myEncoderParameter;
                            bmp1.Save(_winTotal.nomeArquivo, _winTotal.jpgEncoder, _winTotal.myEncoderParameters);
                        }


                        

                        _winTotal.img.Dispose();


                        if (File.Exists(fileTemp))
                            File.Delete(fileTemp);

                        Thread.Sleep(1);
                        worker.ReportProgress(a + 1);
                    }
                    else
                        _winTotal.pararCovert = true;
                }

            }
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


        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _winTotal.CarregarImagens();
            _winTotal.PassarProximo();
            this.Close();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            progressBar1.Maximum = _winTotal.totalPaginas;

            progressBar1.Value = e.ProgressPercentage;

            lblContagem.Content = string.Format("{0} - Página {1}", _winTotal.txtNome.Text, e.ProgressPercentage);
        }
    }
}
