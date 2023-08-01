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
using Microsoft.Reporting.WinForms;
using CS_Caixa.Objetos_de_Valor;
using System.Drawing;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinImpressaoMatricula.xaml
    /// </summary>
    public partial class WinImpressaoMatricula : Window
    {
        List<MatriculaImprimir> MatriculaImprimir;

        List<FileInfo> _arquivos;
        public WinImpressaoMatricula(List<FileInfo> arquivos)
        {
            _arquivos = arquivos;
            InitializeComponent();
        }


        private List<MatriculaImprimir> ObterMatricula()
        {
            MatriculaImprimir = new List<MatriculaImprimir>();

            MatriculaImprimir mat;
            var pc = Environment.MachineName;

            foreach (var item in _arquivos)
            {
                Bitmap imagem = new Bitmap(item.FullName);



                string nomeArquivo = @"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\Mat\Matricula_" + item.Name + "_" + pc + "_" + DateTime.Now.ToLongTimeString() + ".jpg";


                nomeArquivo = nomeArquivo.Replace(":", "_");

                FileInfo arquivo = new FileInfo(nomeArquivo);

                FileInfo arquivoRemover = new FileInfo(nomeArquivo);

                DirectoryInfo diretorio = new DirectoryInfo(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\Mat");

                if (!diretorio.Exists)
                    diretorio.Create();

                if (arquivo.Exists)
                    arquivo.Delete();

                imagem.Save(arquivo.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);



                mat = new MatriculaImprimir();
                mat.Nome = item.Name;
                mat.CaminhoImagem = string.Format("File://{0}", arquivo.FullName);
                mat.Direita = txtDireita.Text;
                mat.Esquerda = txtEsquerda.Text;

                mat.Inferior = txtInferior.Text;
                mat.Superior = txtSuperior.Text;

                MatriculaImprimir.Add(mat);
            }

            return MatriculaImprimir;
        }


        private void ReportViewer_Load(object sender, EventArgs e)
        {
            CarregarRelatorio();
        }


        private void CarregarRelatorio()
        {

            var dataSource = new ReportDataSource("Matricula", MatriculaImprimir);

            reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.Matriculas.RepImpressaoMatricula1.rdlc";

            reportViewer.LocalReport.EnableExternalImages = true;

            reportViewer.LocalReport.DataSources.Add(dataSource);

            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer.RefreshReport();
        }


        private void ProximoComponente(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            txtDireita.Text = "0cm";
            txtEsquerda.Text = "0cm";
            txtSuperior.Text = "0cm";
            txtInferior.Text = "0cm";

            cmbMatricula.ItemsSource = ObterMatricula();
            cmbMatricula.DisplayMemberPath = "Nome";
            cmbMatricula.SelectedIndex = 0;

            CarregarRelatorio();
        }

        private void cmbMatricula_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReceberValoresDoObjeto();            
        }


        private void ReceberValoresDoObjeto()
        {
            var matriculaSelecionada = (MatriculaImprimir)cmbMatricula.SelectedItem;

            txtSuperior.Text = matriculaSelecionada.Superior;
            txtInferior.Text = matriculaSelecionada.Inferior;
            txtDireita.Text = matriculaSelecionada.Direita;
            txtEsquerda.Text = matriculaSelecionada.Esquerda;
        }


        private void CarregarTamanho()
        {

            var matricula = (MatriculaImprimir)cmbMatricula.SelectedItem;

            matricula.Direita = txtDireita.Text;
            matricula.Esquerda = txtEsquerda.Text;
            matricula.Inferior = txtInferior.Text;
            matricula.Superior = txtSuperior.Text;
        }

        private void txtSuperior_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSuperior.Text == "")
                txtSuperior.Text = superior;
            else
                txtSuperior.Text = string.Format("{0}cm", txtSuperior.Text);

            CarregarTamanho();
        }


        string superior = "0cm";
        private void txtSuperior_GotFocus(object sender, RoutedEventArgs e)
        {
            superior = txtSuperior.Text;

            txtSuperior.Text = "";

        }

        private void txtSuperior_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            int key = (int)e.Key;

            if (txtSuperior.Text.Contains("."))
            {
                int index = txtSuperior.Text.IndexOf(".");

                if (txtSuperior.Text.Length == index + 3)
                {
                    e.Handled = !(key == 2 || key == 3);
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 148 || key == 144);
            }


            ProximoComponente(sender, e);
        }


        string inferior = "0cm";
        private void txtInferior_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtInferior.Text == "")
                txtInferior.Text = inferior;
            else
                txtInferior.Text = string.Format("{0}cm", txtInferior.Text);

            CarregarTamanho();
        }

        private void txtInferior_GotFocus(object sender, RoutedEventArgs e)
        {
            inferior = txtInferior.Text;

            txtInferior.Text = "";

        }

        private void txtInferior_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtInferior.Text.Contains("."))
            {
                int index = txtInferior.Text.IndexOf(".");

                if (txtInferior.Text.Length == index + 3)
                {
                    e.Handled = !(key == 2 || key == 3);
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 148 || key == 144);
            }


            ProximoComponente(sender, e);
        }

        string direita = "0cm";
        private void txtDireita_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDireita.Text == "")
                txtDireita.Text = direita;
            else
                txtDireita.Text = string.Format("{0}cm", txtDireita.Text);

            CarregarTamanho();
        }

        private void txtDireita_GotFocus(object sender, RoutedEventArgs e)
        {
            direita = txtDireita.Text;

            txtDireita.Text = "";
        }

        private void txtDireita_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtDireita.Text.Contains("."))
            {
                int index = txtDireita.Text.IndexOf(".");

                if (txtDireita.Text.Length == index + 3)
                {
                    e.Handled = !(key == 2 || key == 3);
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 148 || key == 144);
            }


            ProximoComponente(sender, e);
        }


        string esquerda = "0cm";
        private void txtEsquerda_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEsquerda.Text == "")
                txtEsquerda.Text = esquerda;
            else
                txtEsquerda.Text = string.Format("{0}cm", txtEsquerda.Text);

            CarregarTamanho();
        }

        private void txtEsquerda_GotFocus(object sender, RoutedEventArgs e)
        {
            esquerda = txtEsquerda.Text;

            txtEsquerda.Text = "";
        }

        private void txtEsquerda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtEsquerda.Text.Contains("."))
            {
                int index = txtEsquerda.Text.IndexOf(".");

                if (txtEsquerda.Text.Length == index + 3)
                {
                    e.Handled = !(key == 2 || key == 3);
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 148 || key == 144);
            }

            ProximoComponente(sender, e);
        }

        private void cmbMatricula_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProximoComponente(sender, e);
            }
        }


        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            CarregarRelatorio();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in MatriculaImprimir)
                {

                    string arquivoExcluir = item.CaminhoImagem.Replace("File://", "");

                    FileInfo arquivo = new FileInfo(arquivoExcluir);

                    DirectoryInfo diretorio = new DirectoryInfo(@"\\SERVIDOR\Cartorio\CS_Sistemas\CS_Caixa\Mat");

                    if (!diretorio.Exists)
                        diretorio.Create();

                    if (arquivo.Exists)
                        arquivo.Delete();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MatriculaImprimir.Count; i++)
            {
                MatriculaImprimir[i].Superior = "4.5cm";
                MatriculaImprimir[i].Direita = "0.5cm";
                MatriculaImprimir[i].Esquerda = "0.5cm";
                if (i == MatriculaImprimir.Count - 1)
                    MatriculaImprimir[i].Inferior = "8cm";
                else
                    MatriculaImprimir[i].Inferior = "0.5cm";
            }

            ReceberValoresDoObjeto();

            CarregarRelatorio();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MatriculaImprimir.Count; i++)
            {
                MatriculaImprimir[i].Superior = "0cm";
                MatriculaImprimir[i].Direita = "0cm";
                MatriculaImprimir[i].Esquerda = "0cm";
                MatriculaImprimir[i].Inferior = "0cm";
            }

            ReceberValoresDoObjeto();

            CarregarRelatorio();
        }
    }
}
