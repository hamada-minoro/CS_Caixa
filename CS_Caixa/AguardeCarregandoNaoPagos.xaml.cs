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
    /// Interaction logic for AguardeCarregandoNaoPagos.xaml
    /// </summary>
    public partial class AguardeCarregandoNaoPagos : Window
    {
        BackgroundWorker worker;
        WinNaoPagos _winNaoPagos;
        public AguardeCarregandoNaoPagos(WinNaoPagos winNaoPagos)
        {
            _winNaoPagos = winNaoPagos;
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
            Thread.Sleep(1);
            worker.ReportProgress(0);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _winNaoPagos.Inicio();
            _winNaoPagos.VerificarAtosPendentes();
            _winNaoPagos.ConsultaRecibos();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
