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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinIndiceEscritura.xaml
    /// </summary>
    public partial class WinIndiceEscritura : Window
    {
        string status = "pronto";

        ClassIndiceEscritura classIndiceEscritura = new ClassIndiceEscritura();

        List<IndiceEscritura> listaIndiceEscritura = new List<IndiceEscritura>();

        IndiceEscritura itemSelecionado = new IndiceEscritura();

        int idEscritura;

        public WinIndiceEscritura()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VerificaComponentes();
        }

        private void VerificaComponentes()
        {
            Grids();
            Botoes();
            DataGrid();
        }

        private void Grids()
        {
            if (status == "pronto")
            {
                GridParte.IsEnabled = false;
                GridDados.IsEnabled = false;
                GridConsulta.IsEnabled = true;
            }
            if (status == "novo" || status == "alterar")
            {
                GridParte.IsEnabled = true;
                GridDados.IsEnabled = true;
                GridConsulta.IsEnabled = false;
            }
        }

        private void Botoes()
        {
            if (status == "pronto")
            {
                btnSalvar.IsEnabled = false;
                btnNovo.IsEnabled = true;
                btnCancelar.IsEnabled = false;
            }
            if (status == "novo" || status == "alterar")
            {
                btnSalvar.IsEnabled = true;
                btnNovo.IsEnabled = false;
                btnCancelar.IsEnabled = true;
            }
        }

        private void DataGrid()
        {
            if (status == "pronto")
            {
                if (dataGrid1.Items.Count > 0)
                {
                    dataGrid1.IsEnabled = true;
                }
                else
                {
                    dataGrid1.IsEnabled = false;
                }
            }
            if (status == "novo" || status == "alterar")
            {
                dataGrid1.IsEnabled = false;
            }


        }


        private void LimpaCampos()
        {
            if (!txtConsulta.Focus())
                txtConsulta.Text = "";

            txtOutorgado.Text = "";
            txtOutorgante.Text = "";
            DiaDist.Text = "Dia";
            MesDist.Text = "Mês";
            AnoDist.Text = "Ano";
            txtNatureza.Text = "";
            DiaEscr.Text = "Dia";
            MesEscr.Text = "Mês";
            AnoEscr.Text = "Ano";
            txtAto.Text = "";
            txtLivro.Text = "";
            txtFls.Text = "";
            txtOrdem.Text = "";
        }

        private void PreencheCampos()
        {
            itemSelecionado = (IndiceEscritura)dataGrid1.SelectedItem;

            if (itemSelecionado != null)
            {
                txtOutorgado.Text = itemSelecionado.Outorgado;
                txtOutorgante.Text = itemSelecionado.Outorgante;
                DiaDist.Text = itemSelecionado.DiaDist;
                MesDist.Text = itemSelecionado.MesDist;
                AnoDist.Text = itemSelecionado.AnoDist;
                txtNatureza.Text = itemSelecionado.Natureza;
                DiaEscr.Text = itemSelecionado.Dia;
                MesEscr.Text = itemSelecionado.Mes;
                AnoEscr.Text = itemSelecionado.Ano;
                txtAto.Text = itemSelecionado.Ato;
                txtLivro.Text = itemSelecionado.Livro;
                txtFls.Text = itemSelecionado.Fls;
                txtOrdem.Text = itemSelecionado.Ordem;
            }
            else
            {
                LimpaCampos();
            }
        }

        private void Salvar()
        {
            IndiceEscritura salvarIndice = new IndiceEscritura();

            salvarIndice.Outorgante = txtOutorgante.Text;
            salvarIndice.Outorgado = txtOutorgado.Text;
            if (DiaDist.Text != "Dia")
                salvarIndice.DiaDist = DiaDist.Text;

            if (MesDist.Text != "Mês")
                salvarIndice.MesDist = MesDist.Text;

            if (AnoDist.Text != "Ano")
                salvarIndice.AnoDist = AnoDist.Text;

            salvarIndice.Natureza = txtNatureza.Text;

            if (DiaEscr.Text != "Dia")
                salvarIndice.Dia = DiaEscr.Text;

            if (MesEscr.Text != "Mês")
                salvarIndice.Mes = MesEscr.Text;

            if (AnoEscr.Text != "Ano")
                salvarIndice.Ano = AnoEscr.Text;

            salvarIndice.Ato = txtAto.Text;
            salvarIndice.Livro = txtLivro.Text;
            salvarIndice.Fls = txtFls.Text;
            salvarIndice.Ordem = txtOrdem.Text;

            if (status == "alterar")
                salvarIndice.IdIndiceEscritura = itemSelecionado.IdIndiceEscritura;


            idEscritura = classIndiceEscritura.SalvarIndiceEscritura(salvarIndice, status);
        }

        private void Consulta()
        {
            listaIndiceEscritura = classIndiceEscritura.ListarIndiceEscrituraNome(txtConsulta.Text, cmbTipoParte.Text);

            dataGrid1.ItemsSource = listaIndiceEscritura;
        }

        private void Consulta(int id)
        {
            listaIndiceEscritura = classIndiceEscritura.ListarIndiceEscrituraSalvo(id);

            dataGrid1.ItemsSource = listaIndiceEscritura;
        }


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            status = "pronto";
            VerificaComponentes();

            PreencheCampos();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";
            VerificaComponentes();
            LimpaCampos();
            txtOutorgante.Focus();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (txtOutorgante.Text != "" || txtOutorgado.Text != "")
            {

                Salvar();
                Consulta(idEscritura);
                status = "pronto";
                VerificaComponentes();
                btnNovo.Focus();
            }
            else
            {
                MessageBox.Show("Informe pelo menos um participante.");
            }
        }

        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            if (txtConsulta.Text != "" && txtConsulta.Text != " " && txtConsulta.Text != "  ")
            {
                Consulta();
                DataGrid();
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreencheCampos();
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool ok = classIndiceEscritura.Excluir(itemSelecionado);
                if (ok)
                {
                    listaIndiceEscritura.Remove(itemSelecionado);
                    dataGrid1.ItemsSource = listaIndiceEscritura;
                    dataGrid1.Items.Refresh();
                    status = "pronto";
                    VerificaComponentes();
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro inesperado, não foi possível excluir o registro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            status = "alterar";
            VerificaComponentes();
            txtOutorgante.Focus();
            txtOutorgante.SelectAll();
        }

        private void TabEnter(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest
                    (FocusNavigationDirection.Next));

            }
        }

        private void txtOutorgante_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtOutorgante_LostFocus(object sender, RoutedEventArgs e)
        {

            txtOutorgante.Text = ClassIndiceEscritura.RemoveAcentos(txtOutorgante.Text);
        }

        private void txtOutorgado_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtOutorgado_LostFocus(object sender, RoutedEventArgs e)
        {
            txtOutorgado.Text = ClassIndiceEscritura.RemoveAcentos(txtOutorgado.Text);
        }



        private void DiaDist_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DiaDist.Text == "Dia")
            {
                DiaDist.Text = "";
            }
            else
            {

                DiaDist.Select(0, 2);
            }
        }

        private void DiaDist_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DiaDist.Text == "")
            {
                DiaDist.Text = "Dia";
            }
            else
            {
                DiaDist.Text = string.Format("{0:00}", Convert.ToInt16(DiaDist.Text));
            }
        }

        private void DiaDist_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (DiaDist.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void DiaDist_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DiaDist.Text.Length == 2)
            {
                MesDist.Focus();
            }
        }

        private void MesDist_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MesDist.Text == "Mês")
            {
                MesDist.Text = "";
            }
            else
            {
                MesDist.Select(0, 2);
            }
        }

        private void MesDist_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MesDist.Text == "")
            {
                MesDist.Text = "Mês";
            }
            else
            {
                MesDist.Text = string.Format("{0:00}", Convert.ToInt16(MesDist.Text));
            }
        }

        private void MesDist_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (MesDist.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void MesDist_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MesDist.Text.Length == 2)
            {
                AnoDist.Focus();
            }
        }

        private void AnoDist_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AnoDist.Text == "Ano")
            {
                AnoDist.Text = "";
            }
            else
            {
                AnoDist.Select(0, 4);
            }
        }

        private void AnoDist_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AnoDist.Text == "")
            {
                AnoDist.Text = "Ano";
            }
            else
            {
                AnoDist.Text = string.Format("{0:0000}", Convert.ToInt16(AnoDist.Text));
            }
        }

        private void AnoDist_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (AnoDist.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void AnoDist_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AnoDist.Text.Length == 4)
            {
                txtNatureza.Focus();
            }
        }

        private void DiaEscr_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DiaEscr.Text == "Dia")
            {
                DiaEscr.Text = "";
            }
            else
            {
                DiaEscr.Select(0, 2);
            }
        }

        private void DiaEscr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DiaEscr.Text == "")
            {
                DiaEscr.Text = "Dia";
            }
            else
            {
                DiaEscr.Text = string.Format("{0:00}", Convert.ToInt16(DiaEscr.Text));
            }
        }

        private void DiaEscr_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (DiaEscr.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void DiaEscr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DiaEscr.Text.Length == 2)
            {
                MesEscr.Focus();
            }
        }

        private void MesEscr_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MesEscr.Text == "Mês")
            {
                MesEscr.Text = "";
            }
            else
            {
                MesEscr.Select(0, 2);
            }
        }

        private void MesEscr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MesEscr.Text == "")
            {
                MesEscr.Text = "Mês";
            }
            else
            {
                MesEscr.Text = string.Format("{0:00}", Convert.ToInt16(MesEscr.Text));
            }
        }

        private void MesEscr_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (MesEscr.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void MesEscr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MesEscr.Text.Length == 2)
            {
                AnoEscr.Focus();
            }
        }

        private void AnoEscr_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AnoEscr.Text == "Ano")
            {
                AnoEscr.Text = "";
            }
            else
            {
                AnoEscr.Select(0, 4);
            }
        }

        private void AnoEscr_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AnoEscr.Text == "")
            {
                AnoEscr.Text = "Ano";
            }
            else
            {
                AnoEscr.Text = string.Format("{0:0000}", Convert.ToInt16(AnoEscr.Text));
            }
        }

        private void AnoEscr_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (AnoEscr.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void AnoEscr_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AnoEscr.Text.Length == 4)
            {
                txtAto.Focus();
            }
        }


        private void txtNatureza_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            TabEnter(sender, e);
        }


        private void txtLivro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtFls_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            TabEnter(sender, e);
        }

        private void txtOrdem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            TabEnter(sender, e);
        }

        private void txtOutorgante_GotFocus(object sender, RoutedEventArgs e)
        {
            txtOutorgante.Select(0, txtOutorgante.Text.Length);
        }

        private void txtOutorgado_GotFocus(object sender, RoutedEventArgs e)
        {
            txtOutorgado.Select(0, txtOutorgado.Text.Length);
        }

        private void txtNatureza_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNatureza.Select(0, txtNatureza.Text.Length);
        }

        private void txtAto_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAto.Select(0, txtAto.Text.Length);
        }

        private void txtLivro_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLivro.Select(0, txtLivro.Text.Length);
        }

        private void txtFls_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFls.Select(0, txtFls.Text.Length);
        }

        private void txtOrdem_GotFocus(object sender, RoutedEventArgs e)
        {
            txtOrdem.Select(0, txtOrdem.Text.Length);
        }



        private void cmbTipoParte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoParte.Focus())
            {
                DataGrid();
                txtConsulta.Focus();
                txtConsulta.SelectAll();

            }
        }

        private void txtConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }







    }
}
