using System;
using System.Collections.Generic;
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
using System.IO;
using CS_Caixa.Controls;
using CS_Caixa.Models;
using System.Windows.Threading;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAguardeIndisponibilidade.xaml
    /// </summary>
    public partial class WinAguardeIndisponibilidade : Window
    {
        WinIndisponibilidade indisponibilidade;

        ClassIndiceRgi classIndiceRegistro = new ClassIndiceRgi();

        DataTable tabela = new DataTable();

        IndiceRegistro indiceRegistro = new IndiceRegistro();

        public WinAguardeIndisponibilidade(WinIndisponibilidade indisponibilidade)
        {
            InitializeComponent();
            this.indisponibilidade = indisponibilidade;
        }

        public WinAguardeIndisponibilidade()
        {
            InitializeComponent();
        }

               

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PegarTabela();
            

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                cont = i;

                (sender as BackgroundWorker).ReportProgress(i);

                indiceRegistro.Nome = tabela.Rows[i]["Nome"].ToString().ToUpper();

                indiceRegistro.Nome = ClassIndiceEscritura.RemoveAcentos(indiceRegistro.Nome);

                indiceRegistro.Livro = tabela.Rows[i]["Livro"].ToString();
                indiceRegistro.Fls = tabela.Rows[i]["Folhas"].ToString();
                indiceRegistro.Numero = tabela.Rows[i]["Nº"].ToString();
                indiceRegistro.Ordem = tabela.Rows[i]["Ordem"].ToString();
                indiceRegistro.Reg = tabela.Rows[i]["Reg"].ToString();
                try
                {
                    classIndiceRegistro.SalvarIndiceRegistro(indiceRegistro, "novo");
                    System.Threading.Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }


        int cont;
        

        private void PegarTabela()
        {
            SqlConnection con = new SqlConnection("Data Source=192.168.254.1;Initial Catalog=CartorioUtility;Persist Security Info=True;User ID=sa;Password=P@$$w0rd");
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.[Indice Registros]", con);

                SqlDataReader Dr;
                Dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(Dr);

                tabela = dt;

                progressBar1.Maximum = dt.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
