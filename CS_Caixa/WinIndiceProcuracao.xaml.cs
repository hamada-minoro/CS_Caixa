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
    /// Interaction logic for WinIndiceProcuracao.xaml
    /// </summary>
    public partial class WinIndiceProcuracao : Window
    {

        string status = "pronto";

        ClassIndiceProcuracao classIndiceProcuracao = new ClassIndiceProcuracao();

        List<IndiceProcuracao> listaIndiceProcuracao = new List<IndiceProcuracao>();

        IndiceProcuracao itemSelecionado = new IndiceProcuracao();

        int idProcuracao;

        public WinIndiceProcuracao()
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
            Dia.Text = "Dia";
            Mes.Text = "Mês";
            Ano.Text = "Ano";
            txtAto.Text = "";
            txtLivro.Text = "";
            txtFls.Text = "";
        }

        private void PreencheCampos()
        {
            itemSelecionado = (IndiceProcuracao)dataGrid1.SelectedItem;

            if (itemSelecionado != null)
            {
                txtOutorgado.Text = itemSelecionado.Outorgado;
                txtOutorgante.Text = itemSelecionado.Outorgante;
                Dia.Text = itemSelecionado.Dia;
                Mes.Text = itemSelecionado.Mes;
                Ano.Text = itemSelecionado.Ano;
                txtAto.Text = itemSelecionado.Ato;
                txtLivro.Text = itemSelecionado.Livro;
                txtFls.Text = itemSelecionado.Fls;
            }
            else
            {
                LimpaCampos();
            }
        }

        private void Salvar()
        {
            IndiceProcuracao salvarIndice = new IndiceProcuracao();

            salvarIndice.Outorgante = txtOutorgante.Text;
            salvarIndice.Outorgado = txtOutorgado.Text;

            if (Dia.Text != "Dia")
                salvarIndice.Dia = Dia.Text;

            if (Mes.Text != "Mês")
                salvarIndice.Mes = Mes.Text;

            if (Ano.Text != "Ano")
                salvarIndice.Ano = Ano.Text;

            salvarIndice.Ato = txtAto.Text;
            salvarIndice.Livro = txtLivro.Text;
            salvarIndice.Fls = txtFls.Text;

            if (status == "alterar")
                salvarIndice.IdIndiceProcuracao = itemSelecionado.IdIndiceProcuracao;

            idProcuracao = classIndiceProcuracao.SalvarIndiceProcuracao(salvarIndice, status);
        }

        private void Consulta()
        {
            listaIndiceProcuracao = classIndiceProcuracao.ListarIndiceProcuracaoNome(txtConsulta.Text, cmbTipoParte.Text);

            dataGrid1.ItemsSource = listaIndiceProcuracao;
        }

        private void Consulta(int id)
        {
            listaIndiceProcuracao = classIndiceProcuracao.ListarIndiceProcuracaoSalvo(id);

            dataGrid1.ItemsSource = listaIndiceProcuracao;
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
                Consulta(idProcuracao);
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
                bool ok = classIndiceProcuracao.Excluir(itemSelecionado);
                if (ok)
                {
                    listaIndiceProcuracao.Remove(itemSelecionado);
                    dataGrid1.ItemsSource = listaIndiceProcuracao;
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

            txtOutorgante.Text = ClassIndiceProcuracao.RemoveAcentos(txtOutorgante.Text);
        }

        private void txtOutorgado_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtOutorgado_LostFocus(object sender, RoutedEventArgs e)
        {
            txtOutorgado.Text = ClassIndiceProcuracao.RemoveAcentos(txtOutorgado.Text);
        }

        private void Dia_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Dia.Text == "Dia")
            {
                Dia.Text = "";
            }
            else
            {
                Dia.Select(0, 2);
            }
        }

        private void Dia_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Dia.Text == "")
            {
                Dia.Text = "Dia";
            }
            else
            {
                Dia.Text = string.Format("{0:00}", Convert.ToInt16(Dia.Text));
            }
        }

        private void Dia_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (Dia.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void Dia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Dia.Text.Length == 2)
            {
                Mes.Focus();
            }
        }

        private void Mes_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Mes.Text == "Mês")
            {
                Mes.Text = "";
            }
            else
            {
                Mes.Select(0, 2);
            }
        }

        private void Mes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Mes.Text == "")
            {
                Mes.Text = "Mês";
            }
            else
            {
                Mes.Text = string.Format("{0:00}", Convert.ToInt16(Mes.Text));
            }
        }

        private void Mes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (Mes.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void Mes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Mes.Text.Length == 2)
            {
                Ano.Focus();
            }
        }

        private void Ano_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Ano.Text == "Ano")
            {
                Ano.Text = "";
            }
            else
            {
                Ano.Select(0, 4);
            }
        }

        private void Ano_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Ano.Text == "")
            {
                Ano.Text = "Ano";
            }
            else
            {
                Ano.Text = string.Format("{0:0000}", Convert.ToInt16(Ano.Text));
            }
        }

        private void Ano_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (Ano.Text != "")
            {
                TabEnter(sender, e);
            }
        }

        private void Ano_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Ano.Text.Length == 4)
            {
                txtAto.Focus();
            }
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


        private void txtOutorgante_GotFocus(object sender, RoutedEventArgs e)
        {
            txtOutorgante.Select(0, txtOutorgante.Text.Length);
        }

        private void txtOutorgado_GotFocus(object sender, RoutedEventArgs e)
        {
            txtOutorgado.Select(0, txtOutorgado.Text.Length);
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
