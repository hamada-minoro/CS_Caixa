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
    /// Interaction logic for WinImportarRecolhimentoRgi.xaml
    /// </summary>
    public partial class WinImportarRecolhimentoRgi : Window
    {
        List<Ato> atos = new List<Ato>();
        List<Recolhimento> excluir = new List<Recolhimento>();
        ClassAto classAto = new ClassAto();
        BackgroundWorker worker;
        ClassRecolhimento classRecolhimento = new ClassRecolhimento();
        bool limparDataSelecionada = false;
        string tipoAcao = string.Empty;
        Recolhimento recolhimentoSalvar;

        WinRecolhimento _winRecolhimento;
        ClassReciboNotas classReciboNotas = new ClassReciboNotas();
        string _tipoAto;
        DateTime _dataAto;

        string matricula, livro, RegistroAto;

        public WinImportarRecolhimentoRgi(WinRecolhimento winRecolhimento, string tipoAto, DateTime dataAto)
        {
            _winRecolhimento = winRecolhimento;
            _tipoAto = tipoAto;
            _dataAto = dataAto;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProtRecibo.Focus();
            matricula = _winRecolhimento.txtMatriculaRgi.Text;
            livro = _winRecolhimento.txtLivroRgi.Text;
            RegistroAto = _winRecolhimento.txtRegistroRgi.Text;
            txtProtRecibo.Text = _winRecolhimento.txtProtocoloRgi.Text;
            txtProtRecibo.SelectAll();
            BuscarRegistros();
        }


        private void CarregarValores()
        {
            txbEmol.Text = string.Format("EMOLS: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Emolumentos));
            txbFetj.Text = string.Format("FETJ: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Fetj));
            txbFuna.Text = string.Format("FUNA: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Funarpen));
            txbFund.Text = string.Format("FUND: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Fundperj));
            txbFunp.Text = string.Format("FUNP: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Funperj));
            txbIss.Text = string.Format("ISS: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Iss));
            txbPmcmv.Text = string.Format("PMCMV: {0:N2}", atos.Where(p => p.Checked == true).Sum(p => p.Pmcmv));
        }

        private void MarcarTodos()
        {
            if (atos.Count > 0)
                foreach (var item in atos)
                {
                    item.Checked = true;
                    dataGrid.Items.Refresh();
                }

            CarregarValores();
        }

        private void DesmarcarTodos()
        {
            if (atos.Count > 0)
                foreach (var item in atos)
                {
                    item.Checked = false;
                    dataGrid.Items.Refresh();
                }
            CarregarValores();
        }


        private void checkTodos_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodos();
        }

        private void checkTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            DesmarcarTodos();
        }

        private void checkedUm_Checked(object sender, RoutedEventArgs e)
        {
            CarregarValores();
        }

        private void checkedUm_Unchecked(object sender, RoutedEventArgs e)
        {
            CarregarValores();

        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Importar a data e ato(s) selecionado(s)?", "Importar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                btnFechar.IsEnabled = false;
                btnImportar.IsEnabled = false;
                gridBuscar.IsEnabled = false;


                if (MessageBox.Show("Deseja limpar todos os registros referentes a este protocolo?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    limparDataSelecionada = true;
                    excluir = classRecolhimento.ObterAtoRgiPorProtocolo(Convert.ToInt32(txtProtRecibo.Text));

                }
                progressBar.Maximum = atos.Count;
                progressBar.Visibility = Visibility.Visible;
                txbAguarde.Visibility = Visibility.Visible;
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWorker;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_Completed;
                worker.RunWorkerAsync();
            }

        }

        private void worker_DoWorker(object sender, DoWorkEventArgs e)
        {

            if (limparDataSelecionada)
            {
                tipoAcao = "limpando";

                for (int i = 0; i < excluir.Count; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(i + 1);

                    classRecolhimento.ExcluirRecolhimento(excluir[i]);
                }
            }


            for (int a = 0; a < atos.Count; a++)
            {
                tipoAcao = "importando";

                Thread.Sleep(1);
                worker.ReportProgress(a + 1);

                if (atos[a].Checked == true)
                    ProcedimentoSalvar(atos[a]);
            }
        }



        private void ProcedimentoSalvar(Ato ato)
        {
            try
            {
                var itensAtos = classAto.ListarItensAtoIdAtoRgi(ato.Id_Ato);


                foreach (var item in itensAtos)
                {
                    recolhimentoSalvar = new Recolhimento();
                    recolhimentoSalvar.TipoAto = _tipoAto;
                    recolhimentoSalvar.Natureza = ato.Natureza;
                    recolhimentoSalvar.Data = _dataAto;
                    recolhimentoSalvar.Gratuito = ato.TipoCobranca == "JUSTIÇA GRATUITA" ? true : false;
                    recolhimentoSalvar.Protocolo = ato.TipoAto == "CERTIDÃO RGI" ? ato.Recibo : ato.Protocolo;
                    recolhimentoSalvar.Emol = item.Emolumentos;
                    recolhimentoSalvar.Fetj = item.Fetj;
                    recolhimentoSalvar.Fund = item.Fundperj;
                    recolhimentoSalvar.Funp = item.Funperj;
                    recolhimentoSalvar.Funa = item.Funarpen;
                    recolhimentoSalvar.Pmcmv = item.Pmcmv;
                    recolhimentoSalvar.Iss = item.Iss;
                    recolhimentoSalvar.Atribuicao = "RGI";
                    recolhimentoSalvar.Selo = string.Format("{0}{1}", ato.LetraSelo, ato.NumeroSelo);
                    recolhimentoSalvar.Matricula = matricula;
                    recolhimentoSalvar.Livro = livro;
                    recolhimentoSalvar.Ato = RegistroAto;

                    classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, "novo");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao tentar salvar o registro. " + ex, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (tipoAcao == "limpando")
            {
                progressBar.Maximum = excluir.Count;
                txbTitulo.Text = "Aguarde... Limpando registros anteriores.";
                progressBar.Value = e.ProgressPercentage;
            }

            if (tipoAcao == "importando")
            {
                progressBar.Maximum = atos.Count;
                txbTitulo.Text = "Aguarde... Importando registros selecionados.";
                progressBar.Value = e.ProgressPercentage;
            }
        }

        private void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            _winRecolhimento.ConcluirProcSalvarRgi();
            _winRecolhimento.datePickerInicioRGI.SelectedDate = _dataAto;
            _winRecolhimento.datePickerFimRGI.SelectedDate = _dataAto;
            _winRecolhimento.PesquisarRgi();
            this.Close();
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarRegistros();
            
        }

        private void BuscarRegistros()
        {
            if (txtProtRecibo.Text != string.Empty)
            {
                atos = new List<Ato>();

                var atosRgiProtocolo = classAto.ListarAtoPorProtocoloRgi(Convert.ToInt32(txtProtRecibo.Text)).ToList();

                foreach (var item in atosRgiProtocolo)
                {
                    atos.Add(item);
                }

                var atosRgiRecibo = classAto.ListarAtoPorReciboRgi(Convert.ToInt32(txtProtRecibo.Text)).ToList();

                foreach (var item in atosRgiRecibo)
                {
                    atos.Add(item);
                }

                foreach (var item in atos)
                {
                    item.Checked = true;
                }

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = atos;
                CarregarValores();

                btnImportar.IsEnabled = atos.Count > 0 ? true : false;

            }
        }

        private void txtProtRecibo_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
                btnBuscar.Focus();


            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);

        }
    }

}
