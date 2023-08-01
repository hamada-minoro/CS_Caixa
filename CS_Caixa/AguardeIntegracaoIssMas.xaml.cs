using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for AguardeIntegracaoIssMas.xaml
    /// </summary>
    public partial class AguardeIntegracaoIssMas : Window
    {
        List<ImportarMa> _listaAtosImportados;
        string _caminhoArquivo;
        BackgroundWorker worker;
        string acao;
        ClassImportarMas classImportarMas = new ClassImportarMas();
        List<ImportarMa> listaExistentes = new List<ImportarMa>();
        FileInfo arquivo;
        bool erro = false;

        public AguardeIntegracaoIssMas(List<ImportarMa> listaAtosImportados, string caminhoArquivo)
        {
            _listaAtosImportados = listaAtosImportados;
            _caminhoArquivo = caminhoArquivo;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            lblContagem.Content = string.Format("{0} / {1}", 0, _listaAtosImportados.Count);

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Processo();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

            if (acao == "Adicionando registro ")
            {
                progressBar1.Maximum = _listaAtosImportados.Count();
                lblContagem.Content = string.Format("{0} {1} de {2}", acao, e.ProgressPercentage, _listaAtosImportados.Count);
            }
            else
            {
                progressBar1.Maximum = listaExistentes.Count();
                lblContagem.Content = string.Format("{0} {1} de {2}", acao, e.ProgressPercentage, listaExistentes.Count);
            }


        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            arquivo = new FileInfo(_caminhoArquivo);
            try
            {
                if(erro == false)
                if (arquivo.Exists)
                    arquivo.Delete();
            }
            catch (Exception)
            {

            }

            this.Close();

        }

        private void Processo()
        {
            try
            {
                acao = "Removendo registro ";




                DateTime data = Convert.ToDateTime(_listaAtosImportados[0].Data);

                listaExistentes = classImportarMas.VerificarRegistrosExistentesPorData(data);

                if (listaExistentes.Count > 0)
                {


                    for (int i = 0; i < listaExistentes.Count(); i++)
                    {
                        classImportarMas.ExcluirAto(listaExistentes[i]);
                        Thread.Sleep(1);
                        worker.ReportProgress(i + 1);
                    }


                }



                for (int i = 0; i < _listaAtosImportados.Count(); i++)
                {
                    acao = "Adicionando registro ";


                    ImportarMa AtoSalvar = _listaAtosImportados[i];


                    AtoSalvar = classImportarMas.CalcularValores(AtoSalvar);


                    classImportarMas.SalvarAto(AtoSalvar, "novo");

                    Thread.Sleep(1);
                    worker.ReportProgress(i + 1);

                }


            }
            catch (Exception ex)
            {
                erro = true;
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
    }
}
