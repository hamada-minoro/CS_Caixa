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
    /// Interaction logic for AguardeGerandoXml.xaml
    /// </summary>
    public partial class AguardeGerandoXml : Window
    {


        BackgroundWorker worker;
        
        WinIndiceRgi _indiceRgi;


        List<IndiceRegistro> indiceRegistroEnviar = new List<IndiceRegistro>();

        public AguardeGerandoXml(WinIndiceRgi indiceRgi)
        {
            _indiceRgi = indiceRgi;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            lblContagem.Content = "Gerando Arquivo XML";

            indiceRegistroEnviar = _indiceRgi.listaIndiceRegistro.Where(p => p.Enviado == true).ToList();

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
            
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void Processo()
        {
                try
                {
                    
                    var datasInvalidas = GerarXmlIndiceRgi.VerificarDatas(indiceRegistroEnviar);


                    if(datasInvalidas.Count > 0)
                    {

                        MessageBox.Show("Constam datas inválidas. Para gerar o XML é necessário acertar as datas inválidas. \n\n Verifique as datas dos registros abaixo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);

                        indiceRegistroEnviar = datasInvalidas;

                        _indiceRgi.geradoSucesso = false;

                       return;
                    }

                    if (GerarXmlIndiceRgi.GerarXml(indiceRegistroEnviar, out _indiceRgi.caminho))
                    {
                        _indiceRgi.geradoSucesso = true;                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível gerar o XML. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                    _indiceRgi.geradoSucesso = false;
                }

                _indiceRgi.listaIndiceRegistro = indiceRegistroEnviar;
        }
    }
}

