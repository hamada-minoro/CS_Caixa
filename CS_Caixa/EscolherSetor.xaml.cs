using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Lógica interna para EscolherSetor.xaml
    /// </summary>
    public partial class EscolherSetor : Window
    {
        RetiradaSenha _retirarSenha;

        bool esclherSetor = false;

        public EscolherSetor(RetiradaSenha retirarSenha)
        {
            _retirarSenha = retirarSenha;
            InitializeComponent();
        }

        int contagem = 20;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        BackgroundWorker worker;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            InicializarForm();

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();


        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_retirarSenha.parametros.Voz_RetiradaSenha == true)
                    ClassFalarTexto.FalarTexto("ESCOLHA O SETOR DE ATENDIMENTO.", _retirarSenha.listaVozes, _retirarSenha.parametros.Voz_Botao_1);
            }
            catch (Exception) { }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _retirarSenha.setorSelecionado = 5;
            esclherSetor = true;
        }


        private void InicializarForm()
        {
            lblContagem.Content = string.Format("RETORNANDO EM: {0:00}", contagem);
            lblTitulo.Content = "ESCOLHA O SETOR DE ATENDIMENTO";


            if (_retirarSenha.parametros.Habilitado_Setor_1 == true)
                btnSetor1.Visibility = Visibility.Visible;
            else
                btnSetor1.Visibility = Visibility.Hidden;

            if (_retirarSenha.parametros.Habilitado_Setor_2 == true)
                btnSetor2.Visibility = Visibility.Visible;
            else
                btnSetor2.Visibility = Visibility.Hidden;

            if (_retirarSenha.parametros.Habilitado_Setor_3 == true)
                btnSetor3.Visibility = Visibility.Visible;
            else
                btnSetor3.Visibility = Visibility.Hidden;

            if (_retirarSenha.parametros.Habilitado_Setor_4 == true)
                btnSetor4.Visibility = Visibility.Visible;
            else
                btnSetor4.Visibility = Visibility.Hidden;


            btnSetor1.Content = _retirarSenha.parametros.Nome_Setor_1;
            btnSetor2.Content = _retirarSenha.parametros.Nome_Setor_2;
            btnSetor3.Content = _retirarSenha.parametros.Nome_Setor_3;
            btnSetor4.Content = _retirarSenha.parametros.Nome_Setor_4;


        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (esclherSetor == true)
            {
                if (_retirarSenha.parametros.Mostrar_Hora == true)
                {
                    lblHora.Content = DateTime.Now.ToString();
                    CommandManager.InvalidateRequerySuggested();
                }

                if (IsInitialized == true)
                {
                    contagem--;

                    lblContagem.Content = string.Format("RETORNANDO EM: {0:00}", contagem);

                    if (contagem == 0)
                        this.Close();
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (esclherSetor == true)
            {
                if (e.Key == Key.F12)
                {
                    if (btnSetor1.IsVisible == true)
                        btnSetor1_Click(sender, e);
                }


                if (e.Key == Key.F9)
                {
                    if (btnSetor2.IsVisible == true)
                        btnSetor2_Click(sender, e);
                }

                if (e.Key == Key.F5)
                {
                    if (btnSetor3.IsVisible == true)
                        btnSetor3_Click(sender, e);

                }

                if (e.Key == Key.F1)
                {
                    if (btnSetor4.IsVisible == true)
                        btnSetor4_Click(sender, e);

                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void btnSetor1_Click(object sender, RoutedEventArgs e)
        {
            if (esclherSetor == true)
            {
                _retirarSenha.setorSelecionado = 0;
                this.Close();
            }
        }

        private void btnSetor2_Click(object sender, RoutedEventArgs e)
        {
            if (esclherSetor == true)
            {
                _retirarSenha.setorSelecionado = 1;
                this.Close();
            }
        }

        private void btnSetor3_Click(object sender, RoutedEventArgs e)
        {
            if (esclherSetor == true)
            {
                _retirarSenha.setorSelecionado = 2;
                this.Close();
            }
        }

        private void btnSetor4_Click(object sender, RoutedEventArgs e)
        {
            if (esclherSetor == true)
            {
                _retirarSenha.setorSelecionado = 3;
                this.Close();
            }
        }



    }
}