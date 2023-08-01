using CS_Caixa.Controls;
using CS_Caixa.Models;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for WinAguardeVerificaBackup.xaml
    /// </summary>
    public partial class WinAguardeVerificaBackup : Window
    {
        BackgroundWorker worker;
        DateTime _data;
        string status = "Backups validados com sucesso.";

        int _vb;

        public WinAguardeVerificaBackup(DateTime data, int vb)
        {
            _data = data;
            _vb = vb;
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
            Processo();
        }

        private void Processo()
        {
            ConsultarStorage();
        }


        private void ConsultarStorage()
        {
            try
            {
                
                FileInfo arquivoFirmasStorage = new FileInfo(@"\\192.168.254.156\Cartorio\Bancos\Bancos Total\Firmas\TOTAL_ASSINA.FDB");
                FileInfo arquivoFirmasLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos Total\Firmas\TOTAL_ASSINA.FDB");


                if (arquivoFirmasStorage.LastWriteTime != arquivoFirmasLocal.LastWriteTime)
                {
                    status = "Backups desatualizados.";
                    return;
                }

                

                //FileInfo arquivoNotasStorage = new FileInfo(@"\\192.168.254.156\Cartorio\Bancos\Bancos Total\Notas\NOTAS.GDB");
                //FileInfo arquivoNotasLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos Total\Notas\NOTAS.GDB");


                //if (arquivoNotasStorage.LastWriteTime != arquivoNotasLocal.LastWriteTime)
                //{
                //    status = "Backups desatualizados.";
                //    return;
                //}


                //FileInfo arquivoProtestoStorage = new FileInfo(@"\\192.168.254.156\Cartorio\Bancos\Bancos Total\Protesto\BANCO\BANCO_PROTESTO.FDB");
                //FileInfo arquivoProtestoLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos Total\Protesto\BANCO\BANCO_PROTESTO.FDB");


                //if (arquivoProtestoStorage.LastWriteTime != arquivoProtestoLocal.LastWriteTime)
                //{
                //    status = "Backups desatualizados.";
                //    return;
                //}


                //FileInfo arquivoRgiStorage = new FileInfo(@"\\192.168.254.156\Cartorio\Bancos\Bancos Total\RGI\Banco\BANCO_RGI.FDB");
                //FileInfo arquivoRgiLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos Total\RGI\Banco\BANCO_RGI.FDB");


                //if (arquivoRgiStorage.LastWriteTime != arquivoRgiLocal.LastWriteTime)
                //{
                //    status = "Backups desatualizados.";
                //    return;
                //}

                //FileInfo arquivoGerencialStorage = new FileInfo(@"\\192.168.254.156\Cartorio\Bancos\Bancos Total\Gerencial\BDGERENCIAL.FDB");
                //FileInfo arquivoGerencialLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos Total\Gerencial\BDGERENCIAL.FDB");


                //if (arquivoGerencialStorage.LastWriteTime != arquivoGerencialLocal.LastWriteTime)
                //{
                //    status = "Backups desatualizados.";
                //    return;
                //}

                //FileInfo arquivoSqlServerStorage = new FileInfo(@"\\192.168.1.254\Cartorio\Bancos\SqlServer\DATA\CS_CAIXA_DB.mdf");
                //FileInfo arquivoSqlServerLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos\SqlServer\DATA\CS_CAIXA_DB.mdf");

                //if (arquivoSqlServerStorage.LastWriteTime != arquivoSqlServerLocal.LastWriteTime)
                //{
                //    status = "Backups desatualizados.";
                //    return;
                //}


                //FileInfo arquivoMySqlStorage = new FileInfo(@"\\192.168.1.254\Cartorio\Bancos\MySql\Data1\ibdata1");
                //FileInfo arquivoMySqlLocal = new FileInfo(@"\\SERVIDOR\Total\Bancos\MySql\Data1\ibdata1");


                //if (arquivoMySqlStorage.LastWriteTime != arquivoMySqlLocal.LastWriteTime)
                //{
                //    status = "Backups desatualizados.";
                //    return;
                //}

            }
            catch (Exception)
            {
                status = "Ocorreu um erro durante a valicação do backup.";
                
            }
        }


        private bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }



        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ClassVerificaBackup classVerificaBackup = new ClassVerificaBackup();

            classVerificaBackup.AlterarVerificaBackup(_vb, status);

            this.Close();
        }
    }


}

