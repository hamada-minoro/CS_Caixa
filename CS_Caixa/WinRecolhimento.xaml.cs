using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.RelatoriosForms;
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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinRecolhimento.xaml
    /// </summary>
    public partial class WinRecolhimento : Window
    {

        List<Recolhimento> RecolhimentosRgi;
        Recolhimento recolhimentoSelecionadoRgi;

        List<Recolhimento> RecolhimentosNotas;
        Recolhimento recolhimentoSelecionadoNotas;

        List<Recolhimento> RecolhimentosProtesto;
        Recolhimento recolhimentoSelecionadoProtesto;

        List<Recolhimento> RecolhimentosBalcao;
        Recolhimento recolhimentoSelecionadoBalcao;


        string tipoSalvar;

        ClassRecolhimento classRecolhimento = new ClassRecolhimento();

        public WinRecolhimento()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuscarTodos();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarTodos();
        }

        private void btnImprimirTodos_Click(object sender, RoutedEventArgs e)
        {

            if (datePickerInicioTodos.SelectedDate == null)
            {
                MessageBox.Show("É necessário informar a Data Início.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                datePickerInicioTodos.Focus();
                return;
            }

            if (datePickerFimTodos.SelectedDate == null)
            {
                MessageBox.Show("É necessário informar a Data Fim.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                datePickerFimTodos.Focus();
                return;
            }

            BuscarTodos();

            var ImprimirRecolhimento = new WinImprimirRecolhimento(cmbAtribuicaoTodos.Text, datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value, RecolhimentosRgi, RecolhimentosNotas, RecolhimentosProtesto, RecolhimentosBalcao);
            ImprimirRecolhimento.Owner = this;
            ImprimirRecolhimento.ShowDialog();
        }

        private void BuscarTodos()
        {
            if (datePickerInicioTodos.SelectedDate == null)
                datePickerInicioTodos.SelectedDate = DateTime.Now.Date;

            if (datePickerFimTodos.SelectedDate == null)
                datePickerFimTodos.SelectedDate = DateTime.Now.Date;

            switch (cmbAtribuicaoTodos.SelectedIndex)
            {
                case 0:
                    RecolhimentosRgi = classRecolhimento.ObterRecolhimentoPorPeriodoRgi(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);
                    RecolhimentosNotas = classRecolhimento.ObterRecolhimentoPorPeriodoNotas(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);
                    RecolhimentosProtesto = classRecolhimento.ObterRecolhimentoPorPeriodoProtesto(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);
                    RecolhimentosBalcao = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);                    
                    break;
                case 1:
                     RecolhimentosRgi = classRecolhimento.ObterRecolhimentoPorPeriodoRgi(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);
                    RecolhimentosNotas.RemoveRange(0,RecolhimentosNotas.Count);
                    RecolhimentosProtesto.RemoveRange(0, RecolhimentosProtesto.Count);
                    RecolhimentosBalcao.RemoveRange(0, RecolhimentosBalcao.Count);
                    break;
                case 2:
                    RecolhimentosRgi.RemoveRange(0, RecolhimentosRgi.Count);
                    RecolhimentosNotas = classRecolhimento.ObterRecolhimentoPorPeriodoNotas(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);
                    RecolhimentosProtesto.RemoveRange(0, RecolhimentosProtesto.Count);
                    RecolhimentosBalcao.RemoveRange(0, RecolhimentosBalcao.Count);
                    break;
                case 3:
                    RecolhimentosRgi.RemoveRange(0, RecolhimentosRgi.Count);
                    RecolhimentosNotas.RemoveRange(0, RecolhimentosNotas.Count);
                    RecolhimentosProtesto = classRecolhimento.ObterRecolhimentoPorPeriodoProtesto(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);
                    RecolhimentosBalcao.RemoveRange(0, RecolhimentosBalcao.Count);               
                    break;
                case 4:
                    RecolhimentosRgi.RemoveRange(0, RecolhimentosRgi.Count);
                    RecolhimentosNotas.RemoveRange(0, RecolhimentosNotas.Count);
                    RecolhimentosProtesto.RemoveRange(0, RecolhimentosProtesto.Count);
                    RecolhimentosBalcao = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(datePickerInicioTodos.SelectedDate.Value, datePickerFimTodos.SelectedDate.Value);   
                    break;
                default:
                    break;
            }

            txbQtdAtosTodos.Text = string.Format("{0}", RecolhimentosRgi.Count + RecolhimentosNotas.Count + RecolhimentosProtesto.Count + RecolhimentosBalcao.Count);
            txbQtdAtosGratuitosTodos.Text = string.Format("Atos Gratuitos: {0}", RecolhimentosRgi.Where(p => p.Gratuito == true).Count() + RecolhimentosNotas.Where(p => p.Gratuito == true).Count() + RecolhimentosProtesto.Where(p => p.Gratuito == true).Count() + RecolhimentosBalcao.Where(p => p.Gratuito == true).Count());
            txbEmolTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Emol).Sum() + RecolhimentosNotas.Select(p => p.Emol).Sum() + RecolhimentosProtesto.Select(p => p.Emol).Sum() + RecolhimentosBalcao.Select(p => p.Emol).Sum());
            txbFetjTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Fetj).Sum() + RecolhimentosNotas.Select(p => p.Fetj).Sum() + RecolhimentosProtesto.Select(p => p.Fetj).Sum() + RecolhimentosBalcao.Select(p => p.Fetj).Sum());
            txbFundTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Fund).Sum() + RecolhimentosNotas.Select(p => p.Fund).Sum() + RecolhimentosProtesto.Select(p => p.Fund).Sum() + RecolhimentosBalcao.Select(p => p.Fund).Sum());
            txbFunpTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Funp).Sum() + RecolhimentosNotas.Select(p => p.Funp).Sum() + RecolhimentosProtesto.Select(p => p.Funp).Sum() + RecolhimentosBalcao.Select(p => p.Funp).Sum());
            txbFunaTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Funa).Sum() + RecolhimentosNotas.Select(p => p.Funa).Sum() + RecolhimentosProtesto.Select(p => p.Funa).Sum() + RecolhimentosBalcao.Select(p => p.Funa).Sum());
            txbPmcmvTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Pmcmv).Sum() + RecolhimentosNotas.Select(p => p.Pmcmv).Sum() + RecolhimentosProtesto.Select(p => p.Pmcmv).Sum() + RecolhimentosBalcao.Select(p => p.Pmcmv).Sum());
            txbIssTodos.Text = string.Format("{0:n2}", RecolhimentosRgi.Select(p => p.Iss).Sum() + RecolhimentosNotas.Select(p => p.Iss).Sum() + RecolhimentosProtesto.Select(p => p.Iss).Sum() + RecolhimentosBalcao.Select(p => p.Iss).Sum());
            txbTotalTodos.Text = string.Format("{0:n2}", Convert.ToDecimal(txbEmolTodos.Text) + Convert.ToDecimal(txbFetjTodos.Text) + Convert.ToDecimal(txbFundTodos.Text) + Convert.ToDecimal(txbFunpTodos.Text) + Convert.ToDecimal(txbFunaTodos.Text) + Convert.ToDecimal(txbPmcmvTodos.Text) + Convert.ToDecimal(txbIssTodos.Text));

        }

        private void ExibirImagem(Image img)
        {
            imagemTodos.Visibility = Visibility.Hidden;
            imagemNotas.Visibility = Visibility.Hidden;
            imagemRgi.Visibility = Visibility.Hidden;
            imagemProtesto.Visibility = Visibility.Hidden;
            imagemBalcao.Visibility = Visibility.Hidden;
            img.Visibility = Visibility.Visible;
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnTodos_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            ExibirImagem(imagemTodos);
        }

        private void btnRgi_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
            ExibirImagem(imagemRgi);
            ProcedimentoInicialRgi();
        }

        private void btnNotas_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 2;
            ExibirImagem(imagemNotas);
            ProcedimentoInicialNotas();
        }

        private void btnProtesto_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 3;
            ExibirImagem(imagemProtesto);
            ProcedimentoInicialProtesto();
        }


        private void btnBalcao_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 4;
            ExibirImagem(imagemBalcao);
            ProcedimentoInicialBalcao();
        }

        private void gridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }





        // PARTE DE RGI ---------------------------------------------------
        private void CarregarCamposDoItemSelecionadoRgi()
        {
            cmbAtosRgi.Text = recolhimentoSelecionadoRgi.TipoAto;
            ckbGratuitoRgi.IsChecked = recolhimentoSelecionadoRgi.Gratuito;
            datePickerDataAtoRgi.SelectedDate = recolhimentoSelecionadoRgi.Data;
            txtProtocoloRgi.Text = recolhimentoSelecionadoRgi.Protocolo.ToString();
            txtMatriculaRgi.Text = recolhimentoSelecionadoRgi.Matricula;
            txtLivroRgi.Text = recolhimentoSelecionadoRgi.Livro;
            txtRegistroRgi.Text = recolhimentoSelecionadoRgi.Ato;
            txtEmolRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Emol);
            txtFetjRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Fetj);
            txtFundRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Fund);
            txtFunpRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Funp);
            txtFunaRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Funa);
            txtPmcmvRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Pmcmv);
            txtIssRgi.Text = string.Format("{0:N2}", recolhimentoSelecionadoRgi.Iss);
            txtSeloRgi.Text = recolhimentoSelecionadoRgi.Selo;
        }

        private void LimparCamposDadosRgi()
        {
            cmbAtosRgi.SelectedIndex = -1;
            ckbGratuitoRgi.IsChecked = false;
            datePickerDataAtoRgi.SelectedDate = null;
            txtProtocoloRgi.Text = "";
            txtMatriculaRgi.Text = "";
            txtLivroRgi.Text = "";
            txtRegistroRgi.Text = "";
            txtEmolRgi.Text = "0,00";
            txtFetjRgi.Text = "0,00";
            txtFundRgi.Text = "0,00";
            txtFunpRgi.Text = "0,00";
            txtFunaRgi.Text = "0,00";
            txtPmcmvRgi.Text = "0,00";
            txtIssRgi.Text = "0,00";
            txtSeloRgi.Text = "";
        }

        private void listViewRgi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewRgi.SelectedItem != null)
            {
                recolhimentoSelecionadoRgi = (Recolhimento)listViewRgi.SelectedItem;
                CarregarCamposDoItemSelecionadoRgi();
                btnExcluirRgi.IsEnabled = true;
                btnAlterarRgi.IsEnabled = true;
            }
            else
            {
                LimparCamposDadosRgi();

                btnExcluirRgi.IsEnabled = false;
                btnAlterarRgi.IsEnabled = false;
            }
        }

        private void btnExcluirRgi_Click(object sender, RoutedEventArgs e)
        {
            if (recolhimentoSelecionadoRgi != null)
                if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classRecolhimento.ExcluirRecolhimento(recolhimentoSelecionadoRgi);

                    recolhimentoSelecionadoRgi = null;

                    if (datePickerInicioRGI.SelectedDate == null)
                        datePickerInicioRGI.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimRGI.SelectedDate == null)
                        datePickerFimRGI.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimRGI.SelectedDate != null && datePickerInicioRGI.SelectedDate != null)
                    {
                        RecolhimentosRgi = classRecolhimento.ObterRecolhimentoPorPeriodoRgi(datePickerInicioRGI.SelectedDate.Value, datePickerFimRGI.SelectedDate.Value);

                        listViewRgi.ItemsSource = null;
                        listViewRgi.ItemsSource = RecolhimentosRgi;
                    }
                }
        }

        private void btnSalvarRgi_Click(object sender, RoutedEventArgs e)
        {
            ProcedimentoSalvarRgi();
        }

        private void ProcedimentoSalvarRgi()
        {
            try
            {

                Recolhimento recolhimentoSalvar;

                if (tipoSalvar == "novo")
                    recolhimentoSalvar = new Recolhimento();
                else
                    recolhimentoSalvar = recolhimentoSelecionadoRgi;

                if (cmbAtosRgi.SelectedIndex > -1)
                {
                    recolhimentoSalvar.TipoAto = cmbAtosRgi.Text;
                    recolhimentoSalvar.Natureza = cmbAtosRgi.Text;
                }
                else
                {
                    MessageBox.Show("É necessário informar o Ato.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbAtosRgi.Focus();
                    return;
                }

                if (datePickerDataAtoRgi.SelectedDate != null)
                    recolhimentoSalvar.Data = datePickerDataAtoRgi.SelectedDate;
                else
                {
                    MessageBox.Show("É necessário informar a Data.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    datePickerInicioRGI.Focus();
                    return;
                }

                if (txtProtocoloRgi.Text.Trim() != "")
                    recolhimentoSalvar.Protocolo = Convert.ToInt32(txtProtocoloRgi.Text.Trim());
                else
                {
                    MessageBox.Show("É necessário informar o Protocolo/Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtProtocoloRgi.Focus();
                    return;
                }

                recolhimentoSalvar.Gratuito = ckbGratuitoRgi.IsChecked.Value;
                recolhimentoSalvar.Matricula = txtMatriculaRgi.Text.Trim();
                recolhimentoSalvar.Livro = txtLivroRgi.Text.Trim();
                recolhimentoSalvar.Ato = txtRegistroRgi.Text.Trim();
                recolhimentoSalvar.Emol = Convert.ToDecimal(txtEmolRgi.Text);
                recolhimentoSalvar.Fetj = Convert.ToDecimal(txtFetjRgi.Text);
                recolhimentoSalvar.Fund = Convert.ToDecimal(txtFundRgi.Text);
                recolhimentoSalvar.Funp = Convert.ToDecimal(txtFunpRgi.Text);
                recolhimentoSalvar.Funa = Convert.ToDecimal(txtFunaRgi.Text);
                recolhimentoSalvar.Pmcmv = Convert.ToDecimal(txtPmcmvRgi.Text);
                recolhimentoSalvar.Iss = Convert.ToDecimal(txtIssRgi.Text);
                recolhimentoSalvar.Atribuicao = "RGI";
                recolhimentoSalvar.Selo = txtSeloRgi.Text;




                if (tipoSalvar == "novo")
                {
                    classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    datePickerInicioRGI.SelectedDate = recolhimentoSalvar.Data;
                    datePickerFimRGI.SelectedDate = recolhimentoSalvar.Data;
                    RecolhimentosRgi = classRecolhimento.ObterRecolhimentoPorPeriodoRgi(datePickerInicioRGI.SelectedDate.Value, datePickerFimRGI.SelectedDate.Value);

                    listViewRgi.ItemsSource = null;
                    listViewRgi.ItemsSource = RecolhimentosRgi;
                }
                else
                {
                    var atoSalvo = RecolhimentosRgi.Where(p => p.RecolhimentoId == recolhimentoSelecionadoRgi.RecolhimentoId).FirstOrDefault();

                    atoSalvo = classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    listViewRgi.Items.Refresh();

                }
                MessageBox.Show("Registro salvo com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                ConcluirProcSalvarRgi();
            }
            catch (Exception)
            {
                ConcluirProcSalvarRgi();

                MessageBox.Show("Ocorreu um erro ao tentar salvar o registro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ConcluirProcSalvarRgi()
        {
            btnNovoRgi.IsEnabled = true;

            if (listViewRgi.SelectedItem != null)
            {
                btnExcluirRgi.IsEnabled = true;
                btnAlterarRgi.IsEnabled = true;
            }

            gridDadosRgi.IsEnabled = false;
            spBotoesRgi.IsEnabled = false;
            gridBuscarRgi.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewRgi.IsEnabled = true;
        }

        private void btnNovoRgi_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "novo";
            ProcedimentoNovoRgi();
        }

        private void ProcedimentoInicialRgi()
        {
            btnNovoRgi.IsEnabled = true;
            gridDadosRgi.IsEnabled = false;
            spBotoesRgi.IsEnabled = false;
            gridBuscarRgi.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewRgi.IsEnabled = true;

            if (datePickerInicioRGI.SelectedDate == null)
                datePickerInicioRGI.SelectedDate = DateTime.Now.Date;

            if (datePickerFimRGI.SelectedDate == null)
                datePickerFimRGI.SelectedDate = DateTime.Now.Date;

            RecolhimentosRgi = classRecolhimento.ObterRecolhimentoPorPeriodoRgi(datePickerInicioRGI.SelectedDate.Value, datePickerFimRGI.SelectedDate.Value);
            listViewRgi.ItemsSource = null;
            listViewRgi.ItemsSource = RecolhimentosRgi;
        }


        private void ProcedimentoNovoRgi()
        {
            btnNovoRgi.IsEnabled = false;
            btnExcluirRgi.IsEnabled = false;
            btnAlterarRgi.IsEnabled = false;
            gridDadosRgi.IsEnabled = true;
            spBotoesRgi.IsEnabled = true;
            gridBuscarRgi.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewRgi.IsEnabled = false;
            LimparCamposDadosRgi();
        }


        private void ProcedimentoAlterarRgi()
        {
            btnNovoRgi.IsEnabled = false;
            btnExcluirRgi.IsEnabled = false;
            btnAlterarRgi.IsEnabled = false;
            gridDadosRgi.IsEnabled = true;
            spBotoesRgi.IsEnabled = true;
            gridBuscarRgi.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewRgi.IsEnabled = false;
        }



        private void datePickerInicioRGI_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (datePickerFimRGI.SelectedDate != null && datePickerInicioRGI.SelectedDate != null)
                btnBuscarRGI.IsEnabled = true;
            else
                btnBuscarRGI.IsEnabled = false;

            if (datePickerInicioRGI.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioRGI.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimRGI.SelectedDate = datePickerInicioRGI.SelectedDate;

            if (datePickerInicioRGI.SelectedDate > datePickerFimRGI.SelectedDate)
            {
                datePickerFimRGI.SelectedDate = datePickerInicioRGI.SelectedDate;
            }
        }

        private void datePickerFimRGI_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioRGI.SelectedDate != null)
            {
                if (datePickerInicioRGI.SelectedDate > datePickerFimRGI.SelectedDate)
                {
                    datePickerFimRGI.SelectedDate = datePickerInicioRGI.SelectedDate;
                }
            }


            if (datePickerFimRGI.SelectedDate != null && datePickerInicioRGI.SelectedDate != null)
                btnBuscarRGI.IsEnabled = true;
            else
                btnBuscarRGI.IsEnabled = false;
        }


        private void datePickerInicioTodos_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioTodos.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioTodos.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimTodos.SelectedDate = datePickerInicioTodos.SelectedDate;

            if (datePickerInicioTodos.SelectedDate > datePickerFimTodos.SelectedDate)
            {
                datePickerFimTodos.SelectedDate = datePickerInicioTodos.SelectedDate;
            }
        }

        private void datePickerFimTodos_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioTodos.SelectedDate != null)
            {
                if (datePickerInicioTodos.SelectedDate > datePickerFimTodos.SelectedDate)
                {
                    datePickerFimTodos.SelectedDate = datePickerInicioTodos.SelectedDate;
                }
            }
            else
            {
                MessageBox.Show("Informe a data Inicial.", "Data Inicial", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void btnBuscarRGI_Click(object sender, RoutedEventArgs e)
        {
            PesquisarRgi();
        }

        public void PesquisarRgi()
        {
            if (datePickerInicioRGI.SelectedDate == null)
                return;

            if (datePickerFimRGI.SelectedDate == null)
                return;

            RecolhimentosRgi = classRecolhimento.ObterRecolhimentoPorPeriodoRgi(datePickerInicioRGI.SelectedDate.Value, datePickerFimRGI.SelectedDate.Value);
            listViewRgi.ItemsSource = null;
            listViewRgi.ItemsSource = RecolhimentosRgi;
            listViewRgi.Items.Refresh();
        }

        private void btnCancelarRgi_Click(object sender, RoutedEventArgs e)
        {

            btnNovoRgi.IsEnabled = true;
            if (listViewRgi.SelectedItem != null)
            {
                btnExcluirRgi.IsEnabled = true;
                btnAlterarRgi.IsEnabled = true;
            }
            gridDadosRgi.IsEnabled = false;
            spBotoesRgi.IsEnabled = false;
            gridBuscarRgi.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewRgi.IsEnabled = true;

            if (listViewRgi.SelectedItem != null)
                CarregarCamposDoItemSelecionadoRgi();
            else
                LimparCamposDadosRgi();
        }

        private void btnAlterarRgi_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "alterar";
            ProcedimentoAlterarRgi();
        }


        private void DigitarValoresReais(object sender, KeyEventArgs e, TextBox txt)
        {
            int key = (int)e.Key;


            if (txt.Text.Length > 0)
            {
                if (txt.Text.Contains(","))
                {
                    int index = txt.Text.IndexOf(",");

                    if (txt.Text.Length == index + 3)
                    {
                        e.Handled = !(key == 2 || key == 3 || key == 23 || key == 25 || key == 32);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25) || key == 32;
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88 || key == 142 || key == 23 || key == 25 || key == 32);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 32);
            }
        }

        private void txtEmolRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtEmolRgi);
        }


        private void TxtLostFocus(object sender, RoutedEventArgs e, TextBox txt)
        {
            if (txt.Text != "")
            {
                try
                {
                    txt.Text = string.Format("{0:n2}", Convert.ToDecimal(txt.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txt.Text = "0,00";
            }
        }

        private void TxtGotFocus(object sender, RoutedEventArgs e, TextBox txt)
        {
            if (txt.Text == "0,00")
            {
                txt.Text = "";
            }
        }

        private void txtEmolRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtEmolRgi);
        }

        private void txtEmolRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtEmolRgi);
        }




        private void txtFetjRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFetjRgi);
        }

        private void txtFetjRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFetjRgi);
        }

        private void txtFetjRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFetjRgi);
        }





        private void txtFundRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFundRgi);
        }

        private void txtFundRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFundRgi);
        }

        private void txtFundRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFundRgi);
        }




        private void txtFunpRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunpRgi);
        }

        private void txtFunpRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunpRgi);
        }

        private void txtFunpRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunpRgi);
        }




        private void txtFunaRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunaRgi);
        }

        private void txtFunaRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunaRgi);
        }

        private void txtFunaRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunaRgi);
        }





        private void txtPmcmvRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtPmcmvRgi);
        }

        private void txtPmcmvRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtPmcmvRgi);
        }

        private void txtPmcmvRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtPmcmvRgi);
        }





        private void txtIssRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtIssRgi);
        }

        private void txtIssRgi_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtIssRgi);
        }

        private void txtIssRgi_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtIssRgi);
        }


        private void DigitarNumerosInteiros(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtProtocoloRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumerosInteiros(sender, e);
        }

        private void gridDadosRgi_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassaUmControleParaOutro(sender, e);
        }


        private void PassaUmControleParaOutro(object sender, KeyEventArgs e)
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

        private void btnImportarRgi_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAtosRgi.SelectedIndex > -1 && datePickerDataAtoRgi.SelectedDate != null)
            {
                var importar = new WinImportarRecolhimentoRgi(this, cmbAtosRgi.Text, datePickerDataAtoRgi.SelectedDate.Value);
                importar.Owner = this;
                importar.ShowDialog();
            }
            else
            {
                MessageBox.Show("Informe o Tipo de Ato e a Data do Ato.", "Ops!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }






        // PARTE DE NOTAS ---------------------------------------------------
        private void CarregarCamposDoItemSelecionadoNotas()
        {
            cmbAtosNotas.Text = recolhimentoSelecionadoNotas.TipoAto;
            ckbGratuitoNotas.IsChecked = recolhimentoSelecionadoNotas.Gratuito;
            datePickerDataAtoNotas.SelectedDate = recolhimentoSelecionadoNotas.Data;
            txtNaturezaNotas.Text = recolhimentoSelecionadoNotas.Natureza;
            txtExcedenteNotas.Text = recolhimentoSelecionadoNotas.Excedente != null ? recolhimentoSelecionadoNotas.Excedente.ToString() : "0";
            txtLivroFolhasNotas.Text = recolhimentoSelecionadoNotas.Livro;
            txtProtReciboNotas.Text = recolhimentoSelecionadoNotas.Protocolo.ToString();
            txtValorNotas.Text = recolhimentoSelecionadoNotas.Matricula;

            txtEmolNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Emol);
            txtFetjNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Fetj);
            txtFundNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Fund);
            txtFunpNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Funp);
            txtFunaNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Funa);
            txtPmcmvNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Pmcmv);
            txtIssNotas.Text = string.Format("{0:N2}", recolhimentoSelecionadoNotas.Iss);
            txtSeloNotas.Text = recolhimentoSelecionadoNotas.Selo;
        }

        private void ProcedimentoInicialNotas()
        {
            btnNovoNotas.IsEnabled = true;
            gridDadosNotas.IsEnabled = false;
            spBotoesNotas.IsEnabled = false;
            gridBuscarNotas.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewNotas.IsEnabled = true;

            if (datePickerInicioNotas.SelectedDate == null)
                datePickerInicioNotas.SelectedDate = DateTime.Now.Date;

            if (datePickerFimNotas.SelectedDate == null)
                datePickerFimNotas.SelectedDate = DateTime.Now.Date;

            RecolhimentosNotas = classRecolhimento.ObterRecolhimentoPorPeriodoNotas(datePickerInicioNotas.SelectedDate.Value, datePickerFimNotas.SelectedDate.Value);
            listViewNotas.ItemsSource = null;
            listViewNotas.ItemsSource = RecolhimentosNotas;
        }


        private void btnBuscarNotas_Click(object sender, RoutedEventArgs e)
        {
            PesquisarNotas();
        }

        public void PesquisarNotas()
        {
            if (datePickerInicioNotas.SelectedDate == null)
                return;

            if (datePickerFimNotas.SelectedDate == null)
                return;

            RecolhimentosNotas = classRecolhimento.ObterRecolhimentoPorPeriodoNotas(datePickerInicioNotas.SelectedDate.Value, datePickerFimNotas.SelectedDate.Value);
            listViewNotas.ItemsSource = null;
            listViewNotas.ItemsSource = RecolhimentosNotas;
            listViewNotas.Items.Refresh();
        }

        private void btnNovoNotas_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "novo";
            ProcedimentoNovoNotas();
        }

        private void ProcedimentoNovoNotas()
        {
            btnNovoNotas.IsEnabled = false;
            btnExcluirNotas.IsEnabled = false;
            btnAlterarNotas.IsEnabled = false;
            gridDadosNotas.IsEnabled = true;
            spBotoesNotas.IsEnabled = true;
            spBotoesNotas.IsEnabled = true;
            gridBuscarNotas.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewNotas.IsEnabled = false;
            LimparCamposDadosNotas();
        }

        private void LimparCamposDadosNotas()
        {
            cmbAtosNotas.SelectedIndex = -1;
            ckbGratuitoNotas.IsChecked = false;
            datePickerDataAtoNotas.SelectedDate = null;
            txtNaturezaNotas.Text = "";
            txtExcedenteNotas.Text = "0";
            txtValorNotas.Text = "";
            txtProtReciboNotas.Text = "";
            txtLivroFolhasNotas.Text = "";
            txtEmolNotas.Text = "0,00";
            txtFetjNotas.Text = "0,00";
            txtFundNotas.Text = "0,00";
            txtFunpNotas.Text = "0,00";
            txtFunaNotas.Text = "0,00";
            txtPmcmvNotas.Text = "0,00";
            txtIssNotas.Text = "0,00";
            txtSeloNotas.Text = "";
        }

        private void btnAlterarNotas_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "alterar";
            ProcedimentoAlterarNotas();
        }

        private void ProcedimentoAlterarNotas()
        {
            btnNovoNotas.IsEnabled = false;
            btnExcluirNotas.IsEnabled = false;
            btnAlterarNotas.IsEnabled = false;
            gridDadosNotas.IsEnabled = true;
            spBotoesNotas.IsEnabled = true;
            gridBuscarNotas.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewNotas.IsEnabled = false;
        }

        private void btnExcluirNotas_Click(object sender, RoutedEventArgs e)
        {
            if (recolhimentoSelecionadoNotas != null)
                if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classRecolhimento.ExcluirRecolhimento(recolhimentoSelecionadoNotas);

                    recolhimentoSelecionadoNotas = null;

                    if (datePickerInicioNotas.SelectedDate == null)
                        datePickerInicioNotas.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimNotas.SelectedDate == null)
                        datePickerFimNotas.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimNotas.SelectedDate != null && datePickerInicioNotas.SelectedDate != null)
                    {
                        RecolhimentosNotas = classRecolhimento.ObterRecolhimentoPorPeriodoNotas(datePickerInicioNotas.SelectedDate.Value, datePickerFimNotas.SelectedDate.Value);

                        listViewNotas.ItemsSource = null;
                        listViewNotas.ItemsSource = RecolhimentosNotas;
                    }
                }
        }

        private void gridDadosNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassaUmControleParaOutro(sender, e);
        }


        private void txtEmolNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtEmolNotas);
        }

        private void txtEmolNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtEmolNotas);
        }

        private void txtEmolNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtEmolNotas);
        }

        private void txtFetjNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFetjNotas);
        }

        private void txtFetjNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFetjNotas);
        }

        private void txtFetjNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFetjNotas);
        }

        private void txtFundNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFundNotas);
        }

        private void txtFundNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFundNotas);
        }

        private void txtFundNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFundNotas);
        }

        private void txtFunpNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunpNotas);
        }

        private void txtFunpNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunpNotas);
        }

        private void txtFunpNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunpNotas);
        }

        private void txtFunaNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunaNotas);
        }

        private void txtFunaNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunaNotas);
        }

        private void txtFunaNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunaNotas);
        }

        private void txtPmcmvNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtPmcmvNotas);
        }

        private void txtPmcmvNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtPmcmvNotas);
        }

        private void txtPmcmvNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtPmcmvNotas);
        }

        private void txtIssNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtIssNotas);
        }

        private void txtIssNotas_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtIssNotas);
        }

        private void txtIssNotas_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtIssNotas);
        }

        private void btnImportarNotas_Click(object sender, RoutedEventArgs e)
        {
            var importar = new WinImportarRecolhimentoNotas(this);
            importar.Owner = this;
            importar.ShowDialog();
        }

        private void btnCancelarNotas_Click(object sender, RoutedEventArgs e)
        {
            btnNovoNotas.IsEnabled = true;
            if (listViewNotas.SelectedItem != null)
            {
                btnExcluirNotas.IsEnabled = true;
                btnAlterarNotas.IsEnabled = true;
            }
            gridDadosNotas.IsEnabled = false;
            spBotoesNotas.IsEnabled = false;
            gridBuscarNotas.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewNotas.IsEnabled = true;

            if (listViewNotas.SelectedItem != null)
                CarregarCamposDoItemSelecionadoNotas();
            else
                LimparCamposDadosNotas();
        }

        private void btnSalvarNotas_Click(object sender, RoutedEventArgs e)
        {
            ProcedimentoSalvarNotas();


        }

        private void ProcedimentoSalvarNotas()
        {
            try
            {

                Recolhimento recolhimentoSalvar;

                if (tipoSalvar == "novo")
                    recolhimentoSalvar = new Recolhimento();
                else
                    recolhimentoSalvar = recolhimentoSelecionadoNotas;

                if (cmbAtosNotas.SelectedIndex > -1)
                {
                    recolhimentoSalvar.TipoAto = cmbAtosNotas.Text;
                }
                else
                {
                    MessageBox.Show("É necessário informar o Ato.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbAtosNotas.Focus();
                    return;
                }

                if (datePickerDataAtoNotas.SelectedDate != null)
                    recolhimentoSalvar.Data = datePickerDataAtoNotas.SelectedDate;
                else
                {
                    MessageBox.Show("É necessário informar a Data.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    datePickerInicioNotas.Focus();
                    return;
                }

                recolhimentoSalvar.Protocolo = txtProtReciboNotas.Text != "" ? Convert.ToInt32(txtProtReciboNotas.Text) : 0;


                recolhimentoSalvar.Natureza = txtNaturezaNotas.Text;
                recolhimentoSalvar.Gratuito = ckbGratuitoNotas.IsChecked.Value;

                recolhimentoSalvar.Livro = txtLivroFolhasNotas.Text.Trim();
                recolhimentoSalvar.Emol = Convert.ToDecimal(txtEmolNotas.Text);
                recolhimentoSalvar.Fetj = Convert.ToDecimal(txtFetjNotas.Text);
                recolhimentoSalvar.Fund = Convert.ToDecimal(txtFundNotas.Text);
                recolhimentoSalvar.Funp = Convert.ToDecimal(txtFunpNotas.Text);
                recolhimentoSalvar.Funa = Convert.ToDecimal(txtFunaNotas.Text);
                recolhimentoSalvar.Pmcmv = Convert.ToDecimal(txtPmcmvNotas.Text);
                recolhimentoSalvar.Iss = Convert.ToDecimal(txtIssNotas.Text);
                recolhimentoSalvar.Atribuicao = "NOTAS";
                recolhimentoSalvar.Selo = txtSeloNotas.Text;
                recolhimentoSalvar.Excedente = Convert.ToInt16(txtExcedenteNotas.Text);

                recolhimentoSalvar.Matricula = txtValorNotas.Text.Trim();

                if (tipoSalvar == "novo")
                {
                    classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    datePickerInicioNotas.SelectedDate = recolhimentoSalvar.Data;
                    datePickerFimNotas.SelectedDate = recolhimentoSalvar.Data;
                    RecolhimentosNotas = classRecolhimento.ObterRecolhimentoPorPeriodoNotas(datePickerInicioNotas.SelectedDate.Value, datePickerFimNotas.SelectedDate.Value);

                    listViewNotas.ItemsSource = null;
                    listViewNotas.ItemsSource = RecolhimentosNotas;
                }
                else
                {
                    var atoSalvo = RecolhimentosNotas.Where(p => p.RecolhimentoId == recolhimentoSelecionadoNotas.RecolhimentoId).FirstOrDefault();

                    atoSalvo = classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    listViewNotas.Items.Refresh();

                }
                MessageBox.Show("Registro salvo com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                ConcluirProcSalvarNotas();
            }
            catch (Exception)
            {
                ConcluirProcSalvarNotas();

                MessageBox.Show("Ocorreu um erro ao tentar salvar o registro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ConcluirProcSalvarNotas()
        {
            btnNovoNotas.IsEnabled = true;

            if (listViewNotas.SelectedItem != null)
            {
                btnExcluirNotas.IsEnabled = true;
                btnAlterarNotas.IsEnabled = true;
            }

            gridDadosNotas.IsEnabled = false;
            spBotoesNotas.IsEnabled = false;
            gridBuscarNotas.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewNotas.IsEnabled = true;
        }

        private void listViewNotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewNotas.SelectedItem != null)
            {
                recolhimentoSelecionadoNotas = (Recolhimento)listViewNotas.SelectedItem;
                CarregarCamposDoItemSelecionadoNotas();
                btnExcluirNotas.IsEnabled = true;
                btnAlterarNotas.IsEnabled = true;
            }
            else
            {
                LimparCamposDadosNotas();

                btnExcluirNotas.IsEnabled = false;
                btnAlterarNotas.IsEnabled = false;
            }
        }

        private void datePickerInicioNotas_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerFimNotas.SelectedDate != null && datePickerInicioNotas.SelectedDate != null)
                btnBuscarNotas.IsEnabled = true;
            else
                btnBuscarNotas.IsEnabled = false;

            if (datePickerInicioNotas.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioNotas.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimNotas.SelectedDate = datePickerInicioNotas.SelectedDate;

            if (datePickerInicioNotas.SelectedDate > datePickerFimNotas.SelectedDate)
            {
                datePickerFimNotas.SelectedDate = datePickerInicioNotas.SelectedDate;
            }
        }

        private void datePickerFimNotas_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioNotas.SelectedDate != null)
            {
                if (datePickerInicioNotas.SelectedDate > datePickerFimNotas.SelectedDate)
                {
                    datePickerFimNotas.SelectedDate = datePickerInicioNotas.SelectedDate;
                }
            }


            if (datePickerFimNotas.SelectedDate != null && datePickerInicioNotas.SelectedDate != null)
                btnBuscarNotas.IsEnabled = true;
            else
                btnBuscarNotas.IsEnabled = false;
        }

        private void cmbAtosNotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtProtReciboNotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumerosInteiros(sender, e);
        }


        // PROTESTO ---------------------------------


        private void datePickerInicioProtesto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerFimProtesto.SelectedDate != null && datePickerInicioProtesto.SelectedDate != null)
                btnBuscarProtesto.IsEnabled = true;
            else
                btnBuscarProtesto.IsEnabled = false;

            if (datePickerInicioProtesto.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioProtesto.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimProtesto.SelectedDate = datePickerInicioProtesto.SelectedDate;

            if (datePickerInicioProtesto.SelectedDate > datePickerFimProtesto.SelectedDate)
            {
                datePickerFimProtesto.SelectedDate = datePickerInicioProtesto.SelectedDate;
            }
        }

        private void datePickerFimProtesto_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioProtesto.SelectedDate != null)
            {
                if (datePickerInicioProtesto.SelectedDate > datePickerFimProtesto.SelectedDate)
                {
                    datePickerFimProtesto.SelectedDate = datePickerInicioProtesto.SelectedDate;
                }
            }


            if (datePickerFimProtesto.SelectedDate != null && datePickerInicioProtesto.SelectedDate != null)
                btnBuscarProtesto.IsEnabled = true;
            else
                btnBuscarProtesto.IsEnabled = false;
        }

        private void btnBuscarProtesto_Click(object sender, RoutedEventArgs e)
        {
            PesquisarProtesto();
        }

        public void PesquisarProtesto()
        {
            if (datePickerInicioProtesto.SelectedDate == null)
                return;

            if (datePickerFimProtesto.SelectedDate == null)
                return;

            RecolhimentosProtesto = classRecolhimento.ObterRecolhimentoPorPeriodoProtesto(datePickerInicioProtesto.SelectedDate.Value, datePickerFimProtesto.SelectedDate.Value);
            listViewProtesto.ItemsSource = null;
            listViewProtesto.ItemsSource = RecolhimentosProtesto;
            listViewProtesto.Items.Refresh();
        }

        private void btnNovoProtesto_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "novo";
            ProcedimentoNovoProtesto();
        }

        private void ProcedimentoInicialProtesto()
        {
            btnNovoProtesto.IsEnabled = true;
            gridDadosProtesto.IsEnabled = false;
            spBotoesProtesto.IsEnabled = false;
            gridBuscarProtesto.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewProtesto.IsEnabled = true;

            if (datePickerInicioProtesto.SelectedDate == null)
                datePickerInicioProtesto.SelectedDate = DateTime.Now.Date;

            if (datePickerFimProtesto.SelectedDate == null)
                datePickerFimProtesto.SelectedDate = DateTime.Now.Date;

            RecolhimentosProtesto = classRecolhimento.ObterRecolhimentoPorPeriodoProtesto(datePickerInicioProtesto.SelectedDate.Value, datePickerFimProtesto.SelectedDate.Value);
            listViewProtesto.ItemsSource = null;
            listViewProtesto.ItemsSource = RecolhimentosProtesto;
        }

        private void ProcedimentoNovoProtesto()
        {
            btnNovoProtesto.IsEnabled = false;
            btnExcluirProtesto.IsEnabled = false;
            btnAlterarProtesto.IsEnabled = false;
            gridDadosProtesto.IsEnabled = true;
            spBotoesProtesto.IsEnabled = true;
            gridBuscarProtesto.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewProtesto.IsEnabled = false;
            LimparCamposDadosProtesto();
        }


        private void btnAlterarProtesto_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "alterar";
            ProcedimentoAlterarProtesto();
        }

        private void ProcedimentoAlterarProtesto()
        {
            btnNovoProtesto.IsEnabled = false;
            btnExcluirProtesto.IsEnabled = false;
            btnAlterarProtesto.IsEnabled = false;
            gridDadosProtesto.IsEnabled = true;
            spBotoesProtesto.IsEnabled = true;
            gridBuscarProtesto.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewProtesto.IsEnabled = false;
        }


        private void btnExcluirProtesto_Click(object sender, RoutedEventArgs e)
        {
            if (recolhimentoSelecionadoProtesto != null)
                if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classRecolhimento.ExcluirRecolhimento(recolhimentoSelecionadoProtesto);

                    recolhimentoSelecionadoProtesto = null;

                    if (datePickerInicioProtesto.SelectedDate == null)
                        datePickerInicioProtesto.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimProtesto.SelectedDate == null)
                        datePickerFimProtesto.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimProtesto.SelectedDate != null && datePickerInicioProtesto.SelectedDate != null)
                    {
                        RecolhimentosProtesto = classRecolhimento.ObterRecolhimentoPorPeriodoProtesto(datePickerInicioProtesto.SelectedDate.Value, datePickerFimProtesto.SelectedDate.Value);

                        listViewProtesto.ItemsSource = null;
                        listViewProtesto.ItemsSource = RecolhimentosProtesto;
                    }
                }
        }

        private void gridDadosProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassaUmControleParaOutro(sender, e);
        }

        private void txtProtocoloProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumerosInteiros(sender, e);
        }

        private void txtPmcmvProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtPmcmvProtesto);
        }

        private void txtPmcmvProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtPmcmvProtesto);
        }

        private void txtPmcmvProtesto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtPmcmvProtesto);
        }

        private void txtIssProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtIssProtesto);
        }

        private void txtIssProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtIssProtesto);
        }

        private void txtIssProtestoGotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtIssProtesto);
        }

        private void txtFunpProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunpProtesto);
        }

        private void txtFunpProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunpProtesto);
        }

        private void txtFunpProtesto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunpProtesto);
        }

        private void txtFundProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFundProtesto);
        }

        private void txtFundProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFundProtesto);
        }

        private void txtFundProtesto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFundProtesto);
        }

        private void txtFunaProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunaProtesto);
        }

        private void txtFunaProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunaProtesto);
        }

        private void txtFunaProtesto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunaProtesto);
        }

        private void txtEmolProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtEmolProtesto);
        }

        private void txtEmolProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtEmolProtesto);
        }

        private void txtEmolProtesto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtEmolProtesto);
        }

        private void btnSalvarProtesto_Click(object sender, RoutedEventArgs e)
        {
            ProcedimentoSalvarProtesto();
        }

        private void ProcedimentoSalvarProtesto()
        {
            try
            {

                Recolhimento recolhimentoSalvar;

                if (tipoSalvar == "novo")
                    recolhimentoSalvar = new Recolhimento();
                else
                    recolhimentoSalvar = recolhimentoSelecionadoProtesto;

                if (cmbAtosProtesto.SelectedIndex > -1)
                {
                    recolhimentoSalvar.TipoAto = cmbAtosProtesto.Text;
                    recolhimentoSalvar.Natureza = txtNaturezaProtesto.Text;
                }
                else
                {
                    MessageBox.Show("É necessário informar o Ato.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbAtosProtesto.Focus();
                    return;
                }

                if (datePickerDataAtoProtesto.SelectedDate != null)
                    recolhimentoSalvar.Data = datePickerDataAtoProtesto.SelectedDate;
                else
                {
                    MessageBox.Show("É necessário informar a Data.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    datePickerInicioProtesto.Focus();
                    return;
                }

                if (txtProtocoloProtesto.Text.Trim() != "")
                    recolhimentoSalvar.Protocolo = Convert.ToInt32(txtProtocoloProtesto.Text.Trim());
                else
                {
                    MessageBox.Show("É necessário informar o Protocolo/Recibo.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtProtocoloProtesto.Focus();
                    return;
                }

                recolhimentoSalvar.Gratuito = ckbGratuitoProtesto.IsChecked.Value;
                recolhimentoSalvar.Convenio = cmbConvenioProtesto.SelectedIndex <= 0 ? false : true;
                recolhimentoSalvar.Natureza = txtNaturezaProtesto.Text.Trim();
                recolhimentoSalvar.Emol = Convert.ToDecimal(txtEmolProtesto.Text);
                recolhimentoSalvar.Fetj = Convert.ToDecimal(txtFetjProtesto.Text);
                recolhimentoSalvar.Fund = Convert.ToDecimal(txtFundProtesto.Text);
                recolhimentoSalvar.Funp = Convert.ToDecimal(txtFunpProtesto.Text);
                recolhimentoSalvar.Funa = Convert.ToDecimal(txtFunaProtesto.Text);
                recolhimentoSalvar.Pmcmv = Convert.ToDecimal(txtPmcmvProtesto.Text);
                recolhimentoSalvar.Iss = Convert.ToDecimal(txtIssProtesto.Text);
                recolhimentoSalvar.Atribuicao = "PROTESTO";
                recolhimentoSalvar.Selo = txtSeloProtesto.Text;




                if (tipoSalvar == "novo")
                {
                    classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    datePickerInicioProtesto.SelectedDate = recolhimentoSalvar.Data;
                    datePickerFimProtesto.SelectedDate = recolhimentoSalvar.Data;
                    RecolhimentosProtesto = classRecolhimento.ObterRecolhimentoPorPeriodoProtesto(datePickerInicioProtesto.SelectedDate.Value, datePickerFimProtesto.SelectedDate.Value);

                    listViewProtesto.ItemsSource = null;
                    listViewProtesto.ItemsSource = RecolhimentosProtesto;
                }
                else
                {
                    var atoSalvo = RecolhimentosProtesto.Where(p => p.RecolhimentoId == recolhimentoSelecionadoProtesto.RecolhimentoId).FirstOrDefault();

                    atoSalvo = classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    listViewProtesto.Items.Refresh();

                }
                MessageBox.Show("Registro salvo com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                ConcluirProcSalvarProtesto();
            }
            catch (Exception)
            {
                ConcluirProcSalvarProtesto();

                MessageBox.Show("Ocorreu um erro ao tentar salvar o registro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void ConcluirProcSalvarProtesto()
        {
            btnNovoProtesto.IsEnabled = true;

            if (listViewProtesto.SelectedItem != null)
            {
                btnExcluirProtesto.IsEnabled = true;
                btnAlterarProtesto.IsEnabled = true;
            }

            gridDadosProtesto.IsEnabled = false;
            spBotoesProtesto.IsEnabled = false;
            gridBuscarProtesto.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewProtesto.IsEnabled = true;
        }

        private void btnCancelarProtesto_Click(object sender, RoutedEventArgs e)
        {
            btnNovoProtesto.IsEnabled = true;
            if (listViewProtesto.SelectedItem != null)
            {
                btnExcluirProtesto.IsEnabled = true;
                btnAlterarProtesto.IsEnabled = true;
            }
            gridDadosProtesto.IsEnabled = false;
            spBotoesProtesto.IsEnabled = false;
            gridBuscarProtesto.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewProtesto.IsEnabled = true;

            if (listViewProtesto.SelectedItem != null)
                CarregarCamposDoItemSelecionadoProtesto();
            else
                LimparCamposDadosProtesto();
        }

        private void btnImportarProtesto_Click(object sender, RoutedEventArgs e)
        {
            var importar = new WinImportarRecolhimentoProtesto(this);
            importar.Owner = this;
            importar.ShowDialog();
        }

        private void txtFetjProtesto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFetjProtesto);
        }

        private void txtFetjProtesto_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFetjProtesto);
        }

        private void txtFetjProtesto_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFetjProtesto);
        }

        private void listViewProtesto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewProtesto.SelectedItem != null)
            {
                recolhimentoSelecionadoProtesto = (Recolhimento)listViewProtesto.SelectedItem;
                CarregarCamposDoItemSelecionadoProtesto();
                btnExcluirProtesto.IsEnabled = true;
                btnAlterarProtesto.IsEnabled = true;
            }
            else
            {
                LimparCamposDadosProtesto();

                btnExcluirProtesto.IsEnabled = false;
                btnAlterarProtesto.IsEnabled = false;
            }
        }

        private void CarregarCamposDoItemSelecionadoProtesto()
        {
            cmbAtosProtesto.Text = recolhimentoSelecionadoProtesto.TipoAto;
            ckbGratuitoProtesto.IsChecked = recolhimentoSelecionadoProtesto.Gratuito;
            datePickerDataAtoProtesto.SelectedDate = recolhimentoSelecionadoProtesto.Data;
            txtProtocoloProtesto.Text = recolhimentoSelecionadoProtesto.Protocolo.ToString();
            txtNaturezaProtesto.Text = recolhimentoSelecionadoProtesto.Natureza;
            cmbConvenioProtesto.SelectedIndex = recolhimentoSelecionadoProtesto.Convenio == false ? 0 : 1;
            txtEmolProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Emol);
            txtFetjProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Fetj);
            txtFundProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Fund);
            txtFunpProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Funp);
            txtFunaProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Funa);
            txtPmcmvProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Pmcmv);
            txtIssProtesto.Text = string.Format("{0:N2}", recolhimentoSelecionadoProtesto.Iss);
            txtSeloProtesto.Text = recolhimentoSelecionadoProtesto.Selo;

        }

        private void LimparCamposDadosProtesto()
        {
            cmbAtosProtesto.SelectedIndex = -1;
            ckbGratuitoProtesto.IsChecked = false;
            datePickerDataAtoProtesto.SelectedDate = null;
            txtProtocoloProtesto.Text = "";
            txtNaturezaProtesto.Text = "";
            cmbConvenioProtesto.SelectedIndex = -1;
            txtEmolProtesto.Text = "0,00";
            txtFetjProtesto.Text = "0,00";
            txtFundProtesto.Text = "0,00";
            txtFunpProtesto.Text = "0,00";
            txtFunaProtesto.Text = "0,00";
            txtPmcmvProtesto.Text = "0,00";
            txtIssProtesto.Text = "0,00";
            txtSeloProtesto.Text = "";
        }



        // BALCÃO ------------------------------------------
        private void btnBuscarBalcao_Click(object sender, RoutedEventArgs e)
        {
            PesquisarBalcao();
        }


        public void PesquisarBalcao()
        {
            if (datePickerInicioBalcao.SelectedDate == null)
                return;

            if (datePickerFimBalcao.SelectedDate == null)
                return;

            RecolhimentosBalcao = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(datePickerInicioBalcao.SelectedDate.Value, datePickerFimBalcao.SelectedDate.Value);
            listViewBalcao.ItemsSource = null;
            listViewBalcao.ItemsSource = RecolhimentosBalcao;
            listViewBalcao.Items.Refresh();
        }

        private void datePickerInicioBalcao_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerFimBalcao.SelectedDate != null && datePickerInicioBalcao.SelectedDate != null)
                btnBuscarBalcao.IsEnabled = true;
            else
                btnBuscarBalcao.IsEnabled = false;

            if (datePickerInicioBalcao.SelectedDate > DateTime.Now.Date)
            {
                datePickerInicioBalcao.SelectedDate = DateTime.Now.Date;
            }

            datePickerFimBalcao.SelectedDate = datePickerInicioBalcao.SelectedDate;

            if (datePickerInicioBalcao.SelectedDate > datePickerFimBalcao.SelectedDate)
            {
                datePickerFimBalcao.SelectedDate = datePickerInicioBalcao.SelectedDate;
            }
        }

        private void datePickerFimBalcao_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerInicioBalcao.SelectedDate != null)
            {
                if (datePickerInicioBalcao.SelectedDate > datePickerFimBalcao.SelectedDate)
                {
                    datePickerFimBalcao.SelectedDate = datePickerInicioBalcao.SelectedDate;
                }
            }


            if (datePickerFimBalcao.SelectedDate != null && datePickerInicioBalcao.SelectedDate != null)
                btnBuscarBalcao.IsEnabled = true;
            else
                btnBuscarBalcao.IsEnabled = false;
        }

        private void btnNovoBalcao_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "novo";
            ProcedimentoNovoBalcao();
        }


        private void ProcedimentoInicialBalcao()
        {
            btnNovoBalcao.IsEnabled = true;
            gridDadosBalcao.IsEnabled = false;
            spBotoesBalcao.IsEnabled = false;
            gridBuscarBalcao.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewBalcao.IsEnabled = true;

            if (datePickerInicioBalcao.SelectedDate == null)
                datePickerInicioBalcao.SelectedDate = DateTime.Now.Date;

            if (datePickerFimBalcao.SelectedDate == null)
                datePickerFimBalcao.SelectedDate = DateTime.Now.Date;

            RecolhimentosBalcao = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(datePickerInicioBalcao.SelectedDate.Value, datePickerFimBalcao.SelectedDate.Value);
            listViewBalcao.ItemsSource = null;
            listViewBalcao.ItemsSource = RecolhimentosBalcao;
        }

        private void ProcedimentoNovoBalcao()
        {
            btnNovoBalcao.IsEnabled = false;
            btnExcluirBalcao.IsEnabled = false;
            btnAlterarBalcao.IsEnabled = false;
            gridDadosBalcao.IsEnabled = true;
            spBotoesBalcao.IsEnabled = true;
            gridBuscarBalcao.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewBalcao.IsEnabled = false;
            LimparCamposDadosBalcao();
        }


        private void CarregarCamposDoItemSelecionadoBalcao()
        {
            cmbAtosBalcao.Text = recolhimentoSelecionadoBalcao.TipoAto;
            ckbGratuitoBalcao.IsChecked = recolhimentoSelecionadoBalcao.Gratuito;
            datePickerDataAtoBalcao.SelectedDate = recolhimentoSelecionadoBalcao.Data;
            txtProtocoloBalcao.Text = recolhimentoSelecionadoBalcao.Protocolo.ToString();
            txtNaturezaBalcao.Text = recolhimentoSelecionadoBalcao.Natureza;
            txtEmolBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Emol);
            txtFetjBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Fetj);
            txtFundBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Fund);
            txtFunpBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Funp);
            txtFunaBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Funa);
            txtPmcmvBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Pmcmv);
            txtIssBalcao.Text = string.Format("{0:N2}", recolhimentoSelecionadoBalcao.Iss);
            txtSeloBalcao.Text = recolhimentoSelecionadoBalcao.Selo;
        }

        private void LimparCamposDadosBalcao()
        {
            cmbAtosBalcao.SelectedIndex = -1;
            ckbGratuitoBalcao.IsChecked = false;
            datePickerDataAtoBalcao.SelectedDate = null;
            txtProtocoloBalcao.Text = "";
            txtNaturezaBalcao.Text = "";
            txtEmolBalcao.Text = "0,00";
            txtFetjBalcao.Text = "0,00";
            txtFundBalcao.Text = "0,00";
            txtFunpBalcao.Text = "0,00";
            txtFunaBalcao.Text = "0,00";
            txtPmcmvBalcao.Text = "0,00";
            txtIssBalcao.Text = "0,00";
            txtSeloBalcao.Text = "";
        }


        private void btnAlterarBalcao_Click(object sender, RoutedEventArgs e)
        {
            tipoSalvar = "alterar";
            ProcedimentoAlterarBalcao();
        }


        private void ProcedimentoAlterarBalcao()
        {
            btnNovoBalcao.IsEnabled = false;
            btnExcluirBalcao.IsEnabled = false;
            btnAlterarBalcao.IsEnabled = false;
            gridDadosBalcao.IsEnabled = true;
            spBotoesBalcao.IsEnabled = true;
            gridBuscarBalcao.IsEnabled = false;
            gridMenu.IsEnabled = false;
            gridListViewBalcao.IsEnabled = false;
        }


        private void btnExcluirBalcao_Click(object sender, RoutedEventArgs e)
        {
            if (recolhimentoSelecionadoBalcao != null)
                if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classRecolhimento.ExcluirRecolhimento(recolhimentoSelecionadoBalcao);

                    recolhimentoSelecionadoBalcao = null;

                    if (datePickerInicioBalcao.SelectedDate == null)
                        datePickerInicioBalcao.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimBalcao.SelectedDate == null)
                        datePickerFimBalcao.SelectedDate = DateTime.Now.Date;

                    if (datePickerFimBalcao.SelectedDate != null && datePickerInicioBalcao.SelectedDate != null)
                    {
                        RecolhimentosBalcao = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(datePickerInicioBalcao.SelectedDate.Value, datePickerFimBalcao.SelectedDate.Value);

                        listViewBalcao.ItemsSource = null;
                        listViewBalcao.ItemsSource = RecolhimentosBalcao;
                    }
                }
        }

        private void gridDadosBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassaUmControleParaOutro(sender, e);
        }

        private void txtProtocoloBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarNumerosInteiros(sender, e);
        }

        private void txtEmolBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtEmolBalcao);
        }

        private void txtEmolBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtEmolBalcao);
        }

        private void txtEmolBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtEmolBalcao);
        }

        private void txtFetjBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFetjBalcao);
        }

        private void txtFetjBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFetjBalcao);
        }

        private void txtFetjBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFetjBalcao);
        }

        private void txtFundBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFundBalcao);
        }

        private void txtFundBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFundBalcao);
        }

        private void txtFundBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFundBalcao);
        }

        private void txtFunpBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunpBalcao);
        }

        private void txtFunpBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunpBalcao);
        }

        private void txtFunpBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunpBalcao);
        }

        private void txtFunaBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtFunaBalcao);
        }

        private void txtFunaBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtFunaBalcao);
        }

        private void txtFunaBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtFunaBalcao);
        }

        private void txtPmcmvBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtPmcmvBalcao);
        }

        private void txtPmcmvBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtPmcmvBalcao);
        }

        private void txtPmcmvBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtPmcmvBalcao);
        }

        private void txtIssBalcao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarValoresReais(sender, e, txtIssBalcao);
        }

        private void txtIssBalcao_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtLostFocus(sender, e, txtIssBalcao);
        }


        private void txtIssBalcao_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtGotFocus(sender, e, txtIssBalcao);
        }

        private void listViewBalcao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewBalcao.SelectedItem != null)
            {
                recolhimentoSelecionadoBalcao = (Recolhimento)listViewBalcao.SelectedItem;
                CarregarCamposDoItemSelecionadoBalcao();
                btnExcluirBalcao.IsEnabled = true;
                btnAlterarBalcao.IsEnabled = true;
            }
            else
            {
                LimparCamposDadosBalcao();

                btnExcluirBalcao.IsEnabled = false;
                btnAlterarBalcao.IsEnabled = false;
            }
        }

        private void btnImportarBalcao_Click(object sender, RoutedEventArgs e)
        {
            var importar = new WinImportarRecolhimentoBalcao(this);
            importar.Owner = this;
            importar.ShowDialog();
        }

        private void btnCancelarBalcao_Click(object sender, RoutedEventArgs e)
        {
            btnNovoBalcao.IsEnabled = true;
            if (listViewBalcao.SelectedItem != null)
            {
                btnExcluirBalcao.IsEnabled = true;
                btnAlterarBalcao.IsEnabled = true;
            }
            gridDadosBalcao.IsEnabled = false;
            spBotoesBalcao.IsEnabled = false;
            gridBuscarBalcao.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewBalcao.IsEnabled = true;

            if (listViewBalcao.SelectedItem != null)
                CarregarCamposDoItemSelecionadoBalcao();
            else
                LimparCamposDadosBalcao();
        }

        private void btnSalvarBalcao_Click(object sender, RoutedEventArgs e)
        {
            ProcedimentoSalvarBalcao();
        }

        private void ProcedimentoSalvarBalcao()
        {
            try
            {

                Recolhimento recolhimentoSalvar;

                if (tipoSalvar == "novo")
                    recolhimentoSalvar = new Recolhimento();
                else
                    recolhimentoSalvar = recolhimentoSelecionadoBalcao;

                if (cmbAtosBalcao.SelectedIndex > -1)
                {
                    recolhimentoSalvar.TipoAto = cmbAtosBalcao.Text;
                    recolhimentoSalvar.Natureza = txtNaturezaBalcao.Text;
                }
                else
                {
                    MessageBox.Show("É necessário informar o Ato.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cmbAtosBalcao.Focus();
                    return;
                }

                if (datePickerDataAtoBalcao.SelectedDate != null)
                    recolhimentoSalvar.Data = datePickerDataAtoBalcao.SelectedDate;
                else
                {
                    MessageBox.Show("É necessário informar a Data.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    datePickerInicioBalcao.Focus();
                    return;
                }


                recolhimentoSalvar.Gratuito = ckbGratuitoBalcao.IsChecked.Value;
                recolhimentoSalvar.Natureza = txtNaturezaBalcao.Text.Trim();
                recolhimentoSalvar.Protocolo = txtProtocoloBalcao.Text != "" ? Convert.ToInt32(txtProtocoloBalcao.Text) : 0;
                recolhimentoSalvar.Emol = Convert.ToDecimal(txtEmolBalcao.Text);
                recolhimentoSalvar.Fetj = Convert.ToDecimal(txtFetjBalcao.Text);
                recolhimentoSalvar.Fund = Convert.ToDecimal(txtFundBalcao.Text);
                recolhimentoSalvar.Funp = Convert.ToDecimal(txtFunpBalcao.Text);
                recolhimentoSalvar.Funa = Convert.ToDecimal(txtFunaBalcao.Text);
                recolhimentoSalvar.Pmcmv = Convert.ToDecimal(txtPmcmvBalcao.Text);
                recolhimentoSalvar.Iss = Convert.ToDecimal(txtIssBalcao.Text);
                recolhimentoSalvar.Atribuicao = "BALCÃO";
                recolhimentoSalvar.Selo = txtSeloBalcao.Text;




                if (tipoSalvar == "novo")
                {
                    classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    datePickerInicioBalcao.SelectedDate = recolhimentoSalvar.Data;
                    datePickerFimBalcao.SelectedDate = recolhimentoSalvar.Data;
                    RecolhimentosBalcao = classRecolhimento.ObterRecolhimentoPorPeriodoBalcao(datePickerInicioBalcao.SelectedDate.Value, datePickerFimBalcao.SelectedDate.Value);

                    listViewBalcao.ItemsSource = null;
                    listViewBalcao.ItemsSource = RecolhimentosBalcao;
                }
                else
                {
                    var atoSalvo = RecolhimentosBalcao.Where(p => p.RecolhimentoId == recolhimentoSelecionadoBalcao.RecolhimentoId).FirstOrDefault();

                    atoSalvo = classRecolhimento.SalvarRecolhimento(recolhimentoSalvar, tipoSalvar);
                    listViewBalcao.Items.Refresh();

                }
                MessageBox.Show("Registro salvo com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                ConcluirProcSalvarBalcao();
            }
            catch (Exception)
            {
                ConcluirProcSalvarBalcao();

                MessageBox.Show("Ocorreu um erro ao tentar salvar o registro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ConcluirProcSalvarBalcao()
        {
            btnNovoBalcao.IsEnabled = true;

            if (listViewBalcao.SelectedItem != null)
            {
                btnExcluirBalcao.IsEnabled = true;
                btnAlterarBalcao.IsEnabled = true;
            }

            gridDadosBalcao.IsEnabled = false;
            spBotoesBalcao.IsEnabled = false;
            gridBuscarBalcao.IsEnabled = true;
            gridMenu.IsEnabled = true;
            gridListViewBalcao.IsEnabled = true;
        }





    }
}
