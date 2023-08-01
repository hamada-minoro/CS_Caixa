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
using CS_Caixa.Controls;
using CS_Caixa.Models;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinConsultaCenib.xaml
    /// </summary>
    public partial class WinConsultaCenib : Window
    {
        string caminhoArquivo;

        List<IndiceRegistro> listaCompleta = new List<IndiceRegistro>();

        ClassIndiceRgi classIndiceRgi = new ClassIndiceRgi();

        StreamReader vLeitor;

        List<string> ListaNomes = new List<string>();

        string linha;

        bool parar = false;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        
        public WinConsultaCenib(string caminhoArquivo)
        {
            this.caminhoArquivo = caminhoArquivo;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            listaCompleta = classIndiceRgi.ListarIndiceRegistroNomeConsulta("");

            vLeitor = new StreamReader(caminhoArquivo);

            while (!vLeitor.EndOfStream)
            {
                
                linha = vLeitor.ReadLine();

                if (linha.Contains("<NOME>"))
                {
                    linha = linha.Replace("<NOME>", "").Replace(@"</NOME>", "");
                    linha = linha.Trim();

                    ListaNomes.Add(linha);
                }
            }

            progressBar1.Maximum = ListaNomes.Count;
            

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();



        }




        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ListaNomes.Count; i++)
            {

                if (parar)
                    break;


                progressBar1.Value = i + 1;

                Refrescar(progressBar1);

                linha = ListaNomes[i];

                lblNome.Content = ListaNomes[i];

                Refrescar(lblNome);

                bool achou = PrimeiraVerificacao(linha);

                if (achou == false)
                    SegundaVerificacao(linha);
            }
            dispatcherTimer.Stop();

            MessageBox.Show("Fim da consulta.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private static Action EmptyDelegate = delegate() { };
        public static void Refrescar(UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }




        private bool PrimeiraVerificacao(string nome)
        {

            bool achou = false;
            List<IndiceRegistro> RegistrosEncontrados = new List<IndiceRegistro>();

            RegistrosEncontrados = listaCompleta.Where(p => p.Nome == nome).ToList();

            if (RegistrosEncontrados.Count > 0)
            {

                MessageBox.Show("Nome encontrado: " + nome + ".", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);

                WinMostrarNomesCenib mostrar = new WinMostrarNomesCenib(RegistrosEncontrados, nome);
                mostrar.Owner = this;
                mostrar.ShowDialog();
                achou = true;
            }

            return achou;
        }


        private void SegundaVerificacao(string nome)
        {
            List<IndiceRegistro> RegistrosEncontrados = new List<IndiceRegistro>();

            string parteNome1 = string.Empty;

            string parteNome2 = string.Empty;

            string parteNome3 = string.Empty;

            int index;

            string nomeAux = nome;

            if (nome.Length > 4)
            {
                if (nomeAux.Contains(" "))
                {

                    index = nomeAux.IndexOf(" ");

                    if (nome.Length > 12)
                        parteNome1 = nomeAux.Substring(0, index);
                    else
                        parteNome1 = nomeAux.Substring(0, nome.Length - 2);

                    nomeAux = nomeAux.Substring(index, nomeAux.Length - (index + 2));

                    RegistrosEncontrados = listaCompleta.Where(p => p.Nome.Contains(parteNome1)).ToList();
                }



                if (nomeAux.Length >= 5)
                {

                    parteNome2 = nomeAux.Substring(0, 5);

                    nomeAux = nomeAux.Substring(5, nomeAux.Length - 5);

                    RegistrosEncontrados = RegistrosEncontrados.Where(p => p.Nome.Contains(parteNome1 + parteNome2)).ToList();
                }

                if (!nomeAux.Contains("LTDA") && !nomeAux.Contains("-") && !nomeAux.Contains("("))
                    if (nomeAux.Length >= 5)
                    {

                        parteNome3 = nomeAux.Substring(nomeAux.Length - 5, 5);

                        RegistrosEncontrados = RegistrosEncontrados.Where(p => p.Nome.Contains(parteNome3)).ToList();

                    }



                if (RegistrosEncontrados.Count > 0)
                {

                    WinMostrarNomesCenib mostrar = new WinMostrarNomesCenib(RegistrosEncontrados, nome);
                    mostrar.Owner = this;
                    mostrar.ShowDialog();


                }

            }
        }

    }
}

