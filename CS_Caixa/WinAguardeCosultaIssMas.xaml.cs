using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for WinAguardeCosultaIssMas.xaml
    /// </summary>
    public partial class WinAguardeCosultaIssMas : Window
    {

        BackgroundWorker worker;
        ImportarMas _importarMas;

        ClassImportarMas classImportarMas = new ClassImportarMas();
        public WinAguardeCosultaIssMas(ImportarMas importarMas)
        {

            InitializeComponent();
            _importarMas = importarMas;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            lblContagem.Content = "Processando Dados Solicitados";
            progressBar1.Maximum = 5;


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

            if (e.ProgressPercentage == 2)
            {
                if (_importarMas.tipoConsulta == "data")
                    _importarMas.atosConsultados = classImportarMas.ListarAtosPorPeriodo(_importarMas.dpConsultaInicio.SelectedDate.Value, _importarMas.dpConsultaFim.SelectedDate.Value);
                else
                {
                    if (_importarMas.cmbTipoConsulta.SelectedIndex <= 1)
                        _importarMas.atosConsultados = classImportarMas.ConsultaDetalhada(_importarMas.cmbTipoConsulta.Text, _importarMas.cmbDadosConsulta.Text);
                    else
                        _importarMas.atosConsultados = classImportarMas.ConsultaDetalhada(_importarMas.cmbTipoConsulta.Text, _importarMas.txtDadosConsulta.Text);
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            this.Close();

        }

        private void Processo()
        {
            try
            {

                for (int i = 0; i < 5; i++)
                {

                    Thread.Sleep(1);
                    worker.ReportProgress(i + 1);
                }





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
    }
}

