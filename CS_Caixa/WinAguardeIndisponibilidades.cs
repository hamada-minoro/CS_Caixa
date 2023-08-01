using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using CS_Caixa.Controls;
using CS_Caixa.Models;

using Microsoft.Office.Interop.Word;



namespace CS_Caixa
{
    public partial class WinAguardeIndisponibilidades : Form
    {
        List<string> resultadoArquivos = new List<string>();
        ClassIndisponibilidade indisp = new ClassIndisponibilidade();
        WinIndisponibilidade indisponibilidade;

        bool ok = false;
       


        public WinAguardeIndisponibilidades(WinIndisponibilidade indisponibilidade)
        {
            this.indisponibilidade = indisponibilidade;
            InitializeComponent();
        }

        private void WinAguardeIndisponibilidades_Load(object sender, EventArgs e)
        {
            Thread trd = new Thread(new ThreadStart(this.ThreadTarefa));
            trd.IsBackground = true;
            trd.Start();
        }


        private void ThreadTarefa()
        {

            voidReadMsWord();

            this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                this.Close();
            });
        }

       

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
        


        private void Salvar(string titulo, string nome, string cpfCnpj, string aviso, string oficio, string processo, string obs, string caminho)
        {
            Indisponibilidade salvarIndisp = new Indisponibilidade();

            FileInfo file = new FileInfo(caminho);

            string nomeArq = file.Name;



            try
            {
                salvarIndisp.Titulo = titulo.Trim();

                salvarIndisp.Nome = nome.Trim();

                salvarIndisp.CpfCnpj = cpfCnpj.Trim();

                salvarIndisp.Oficio = aviso.Trim();

                salvarIndisp.Aviso = oficio.Trim();

                salvarIndisp.Processo = processo.Trim();

                salvarIndisp.Valor = obs.Trim();

                indisp.SalvarIndisp(salvarIndisp, "novo");

                if (caminho != "")
                {
                    if (ok)
                    {
                        string novoCaminho = @"\\SERVIDOR\Arquivos Cartório\Arquivos RGI\ARQUIVOS EGIDIO\INDISPONIBILIDADE\Importados\" + nomeArq;


                        if (File.Exists(novoCaminho))
                            File.Delete(novoCaminho);


                        File.Move(caminho, novoCaminho);

                       
                    }

                    ok = false;
                    caminho = "";
                }

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }

        }


        private void BuscaArquivos(DirectoryInfo dir)
        {
            // lista arquivos do diretorio corrente
            foreach (FileInfo file in dir.GetFiles())
            {
                // aqui no caso estou guardando o nome completo do arquivo em em controle ListBox
                // voce faz como quiser
                resultadoArquivos.Add(file.FullName);
            }

            //// busca arquivos do proximo sub-diretorio
            //foreach (DirectoryInfo subDir in dir.GetDirectories())
            //{
            //    BuscaArquivos(subDir);
            //}
        }

        public void voidReadMsWord()
        {

            DirectoryInfo dirInfo = new DirectoryInfo(@"\\SERVIDOR\Arquivos Cartório\Arquivos RGI\ARQUIVOS EGIDIO\INDISPONIBILIDADE");

            // procurar arquivos
            BuscaArquivos(dirInfo);

            SetControlPropertyValue(progressBar1, "Maximum", resultadoArquivos.Count);


            for (int r = 0; r < resultadoArquivos.Count; r++)
            {

                SetControlPropertyValue(progressBar1, "Value", r);

                string aguarde = string.Format("Aguarde...{0}/{1}", r + 1, resultadoArquivos.Count);

                SetControlPropertyValue(progressBar1, "Value", r + 1);

                SetControlPropertyValue(label1, "Text", aguarde);
                
                try
                {
                    //// create word application
                    //Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                    //// create object of missing value
                    //object miss = System.Reflection.Missing.Value;
                    //// create object of selected file path
                    //object path = resultadoArquivos[r];
                    //// set file path mode
                    //object readOnly = false;
                    //// open document                
                    //Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                    //// select whole data from active window document
                    //docs.ActiveWindow.Selection.WholeStory();
                    //// handover the data to cllipboard
                    //docs.ActiveWindow.Selection.Copy();
                    //// clipboard create reference of idataobject interface which transfer the data
                    //IDataObject data = Clipboard.GetDataObject();

                    RichTextBox richTextBox2 = new RichTextBox();
                   
                    // Open a doc file.
                    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                    Document document = word.Documents.Open(resultadoArquivos[r]);

                    // Loop through all words in the document.
                    int count = document.Words.Count;
                    for (int i = 1; i <= count; i++)
                    {
                        // Write the word.
                        richTextBox2.Text =  document.Words[i].Text;
                        
                    }
                    // Close word.
                    word.Quit();


                    int totalLinhas = richTextBox2.Lines.Count();

                    int linhaNome = 0;

                    string titulo = string.Empty;
                    string nome = string.Empty;
                    string cpfCnpj = string.Empty;
                    string oficio = string.Empty;
                    string aviso = string.Empty;
                    string processo = string.Empty;
                    string obs = string.Empty;

                    for (int i = 0; i < totalLinhas; i++)
                    {

                        if (richTextBox2.Lines[i].Contains("A") || richTextBox2.Lines[i].Contains("E") || richTextBox2.Lines[i].Contains("I") || richTextBox2.Lines[i].Contains("O") || richTextBox2.Lines[i].Contains("U") || richTextBox2.Lines[i].Contains("B") || richTextBox2.Lines[i].Contains("C") || richTextBox2.Lines[i].Contains("D") || richTextBox2.Lines[i].Contains("F") || richTextBox2.Lines[i].Contains("G") || richTextBox2.Lines[i].Contains("H") || richTextBox2.Lines[i].Contains("J") || richTextBox2.Lines[i].Contains("L") || richTextBox2.Lines[i].Contains("M") || richTextBox2.Lines[i].Contains("N") || richTextBox2.Lines[i].Contains("P") || richTextBox2.Lines[i].Contains("Q") || richTextBox2.Lines[i].Contains("R") || richTextBox2.Lines[i].Contains("S") || richTextBox2.Lines[i].Contains("T") || richTextBox2.Lines[i].Contains("V") || richTextBox2.Lines[i].Contains("X") || richTextBox2.Lines[i].Contains("Y") || richTextBox2.Lines[i].Contains("W") || richTextBox2.Lines[i].Contains("Z") || richTextBox2.Lines[i].Contains("a") || richTextBox2.Lines[i].Contains("e") || richTextBox2.Lines[i].Contains("i") || richTextBox2.Lines[i].Contains("o") || richTextBox2.Lines[i].Contains("u") || richTextBox2.Lines[i].Contains("b") || richTextBox2.Lines[i].Contains("c") || richTextBox2.Lines[i].Contains("d") || richTextBox2.Lines[i].Contains("f") || richTextBox2.Lines[i].Contains("g") || richTextBox2.Lines[i].Contains("h") || richTextBox2.Lines[i].Contains("j") || richTextBox2.Lines[i].Contains("l") || richTextBox2.Lines[i].Contains("m") || richTextBox2.Lines[i].Contains("n") || richTextBox2.Lines[i].Contains("p") || richTextBox2.Lines[i].Contains("q") || richTextBox2.Lines[i].Contains("r") || richTextBox2.Lines[i].Contains("s") || richTextBox2.Lines[i].Contains("t") || richTextBox2.Lines[i].Contains("v") || richTextBox2.Lines[i].Contains("x") || richTextBox2.Lines[i].Contains("y") || richTextBox2.Lines[i].Contains("w") || richTextBox2.Lines[i].Contains("z"))
                        {

                            if (linhaNome == 0)
                            {
                                titulo = richTextBox2.Lines[i];

                                titulo.Replace('-', ' ');

                                titulo = titulo.Remove(titulo.Length - 15, 15);

                                titulo = titulo.Trim(' ');
                            }

                            if (linhaNome == 1)
                            {
                                nome = richTextBox2.Lines[i];

                                nome = nome.Replace('-', ' ');

                                nome = nome.Trim();
                            }

                            if (linhaNome == 2)
                            {
                                cpfCnpj = richTextBox2.Lines[i];

                                cpfCnpj = cpfCnpj.Replace('-', ' ').Replace('C', ' ').Replace('P', ' ').Replace('F', ' ').Replace('N', ' ').Replace('J', ' ').Replace(':', ' ');

                                cpfCnpj = cpfCnpj.Trim();

                                cpfCnpj = cpfCnpj.Replace(' ', '-');
                            }

                            if (linhaNome == 3)
                            {
                                oficio = richTextBox2.Lines[i];

                                oficio = oficio.Replace('-', ' ');

                                oficio = oficio.Trim();
                            }

                            if (linhaNome == 4)
                            {
                                aviso = richTextBox2.Lines[i];

                                aviso = aviso.Replace('-', ' ');

                                aviso = aviso.Trim();
                            }

                            if (linhaNome == 5)
                            {
                                processo = richTextBox2.Lines[i];

                                processo = processo.Replace('-', ' ');

                                processo = processo.Trim();
                            }

                            if (linhaNome == 6)
                            {
                                obs = richTextBox2.Lines[i];

                                obs = obs.Replace('-', ' ');

                                obs = obs.Trim();
                            }

                            linhaNome++;
                        }


                    }



                    Salvar(titulo, nome, cpfCnpj, oficio, aviso, processo, obs, resultadoArquivos[r]);

                    ok = true;

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());

                }
            }



        }
    }


}
