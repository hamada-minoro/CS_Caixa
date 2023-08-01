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
    /// Interaction logic for WinImportarRecolhimentoBalcao.xaml
    /// </summary>
    public partial class WinImportarRecolhimentoBalcao : Window
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
        public WinImportarRecolhimentoBalcao(WinRecolhimento winRecolhimento)
        {
            _winRecolhimento = winRecolhimento;
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpDataAto.SelectedDate = DateTime.Now.Date;
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dpDataAto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            atos = classAto.ListarAtoDataAto(dpDataAto.SelectedDate.Value, dpDataAto.SelectedDate.Value, "BALCÃO");

            foreach (var item in atos)
            {
                item.Checked = true;
            }

            dataGridBalcao.ItemsSource = null;
            dataGridBalcao.ItemsSource = atos;
            CarregarValores();

            btnImportar.IsEnabled = atos.Count > 0 ? true : false;
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

        private void checkBalcaoTodos_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodosBalcao();
        }

        private void checkBalcaoTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            DesmarcarTodosBalcao();
        }

        private void checkedBalcaoUm_Checked(object sender, RoutedEventArgs e)
        {
            CarregarValores();
        }

        private void checkedBalcaoUm_Unchecked(object sender, RoutedEventArgs e)
        {
            CarregarValores();
        }

        private void MarcarTodosBalcao()
        {
            if (atos.Count > 0)
                foreach (var item in atos)
                {
                    item.Checked = true;
                    dataGridBalcao.Items.Refresh();
                }

            CarregarValores();
        }

        private void DesmarcarTodosBalcao()
        {
            if (atos.Count > 0)
                foreach (var item in atos)
                {
                    item.Checked = false;
                    dataGridBalcao.Items.Refresh();
                }
            CarregarValores();
        }

        private void dataGridBalcao_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Importar a data e ato(s) selecionado(s)?", "Importar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                btnFechar.IsEnabled = false;
                btnImportar.IsEnabled = false;
                dpDataAto.IsEnabled = false;


                if (MessageBox.Show("Deseja limpar todos os registros da data selecionada?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    limparDataSelecionada = true;
                    excluir = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(dpDataAto.SelectedDate.Value, dpDataAto.SelectedDate.Value);
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

                if(atos[a].Checked == true)
                ProcedimentoSalvarBalcao(atos[a]);
            }
        }
        


        private void ProcedimentoSalvarBalcao(Ato ato)
        {
            try
            {
                recolhimentoSalvar = new Recolhimento();
                recolhimentoSalvar.TipoAto = ato.TipoAto;
                recolhimentoSalvar.Natureza = ato.DescricaoAto;
                recolhimentoSalvar.Data = ato.DataAto;
                recolhimentoSalvar.Gratuito = ato.TipoCobranca == "JUSTIÇA GRATUITA" ? true : false;
                recolhimentoSalvar.Protocolo = ato.ReciboBalcao;
                recolhimentoSalvar.Emol = ato.Emolumentos;
                recolhimentoSalvar.Fetj = ato.Fetj;
                recolhimentoSalvar.Fund = ato.Fundperj;
                recolhimentoSalvar.Funp = ato.Funperj;
                recolhimentoSalvar.Funa = ato.Funarpen;
                recolhimentoSalvar.Pmcmv = ato.Pmcmv;
                recolhimentoSalvar.Iss = ato.Iss;
                recolhimentoSalvar.Atribuicao = "BALCÃO";
                recolhimentoSalvar.Selo = string.Format("{0}{1}", ato.LetraSelo, ato.NumeroSelo);

                classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, "novo");

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
           
            _winRecolhimento.ConcluirProcSalvarBalcao();
            _winRecolhimento.datePickerInicioBalcao.SelectedDate = atos[0].DataAto;
            _winRecolhimento.datePickerFimBalcao.SelectedDate = atos[0].DataAto;
            _winRecolhimento.PesquisarBalcao();

            this.Close();
        }
    }
}
