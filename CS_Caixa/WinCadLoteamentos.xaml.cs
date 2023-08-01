using CS_Caixa.Controls;
using CS_Caixa.Models;
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
    /// Lógica interna para WinCadLoteamentos.xaml
    /// </summary>
    public partial class WinCadLoteamentos : Window
    {

        string status = "pronto";

        Loteamento itemSelecionado = new Loteamento();

        ClassLoteamentos classLoteamentos = new ClassLoteamentos();

        List<Loteamento> listaLoteamentos = new List<Loteamento>();

        public WinCadLoteamentos()
        {
            InitializeComponent();
        }

        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            status = "pronto";
            VerificaComponentes();

            PreencheCampos();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            Salvar();

            if (status == "novo")
            {
               listaLoteamentos = classLoteamentos.ListaLoteamentosTodos();
               dataGrid1.ItemsSource = listaLoteamentos;
               dataGrid1.Items.Refresh();
               dataGrid1.SelectedItem = listaLoteamentos.LastOrDefault();
               dataGrid1.ScrollIntoView(dataGrid1.SelectedItem);
            }
            status = "pronto";
            VerificaComponentes();
            btnNovo.Focus();

            
            dataGrid1.Items.Refresh();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";

            VerificaComponentes();
            LimpaCampos();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreencheCampos();
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            status = "alterar";
            VerificaComponentes();
            txtNome.Focus();
            txtNome.SelectAll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConsultarTodos();
            VerificaComponentes();            
            dataGrid1.SelectedIndex = 0;
        }

        private void ConsultarTodos()
        {
            listaLoteamentos = classLoteamentos.ListaLoteamentosTodos();
            dataGrid1.ItemsSource = listaLoteamentos;
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
                GridDados.IsEnabled = false;
                GridConsulta.IsEnabled = true;
            }
            if (status == "novo" || status == "alterar")
            {
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

            txtNome.Text = "";
            cmbLocalizacao.SelectedIndex = -1;
            txtProprietario.Text = "";
            txtMatricula.Text = "";
            txtInscricao.Text = "";
        }

        private void PreencheCampos()
        {
            try
            {

                itemSelecionado = (Loteamento)dataGrid1.SelectedItem;

                if (itemSelecionado != null)
                {
                    txtNome.Text = itemSelecionado.Nome;
                    cmbLocalizacao.Text = itemSelecionado.Localizacao;
                    txtProprietario.Text = itemSelecionado.Proprietario;
                    txtMatricula.Text = itemSelecionado.Matricula.ToString();
                    txtInscricao.Text = itemSelecionado.NumeroInscricao;
                                        
                }
                else
                {
                    LimpaCampos();
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            status = "alterar";
            VerificaComponentes();
            txtNome.Focus();
            txtNome.SelectAll();
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool ok = classLoteamentos.Excluir(itemSelecionado);
                if (ok)
                {
                    listaLoteamentos.Remove(itemSelecionado);
                    dataGrid1.ItemsSource = listaLoteamentos;
                    dataGrid1.Items.Refresh();
                    if (dataGrid1.Items.Count > 0)
                        dataGrid1.SelectedIndex = 0;
                    status = "pronto";
                    VerificaComponentes();
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro inesperado, não foi possível excluir o registro.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void Salvar()
        {
            Loteamento salvarLoteamento = new Loteamento();

            if (status == "alterar")
                salvarLoteamento = itemSelecionado;

            try
            {
                salvarLoteamento.Nome = txtNome.Text;


                salvarLoteamento.Localizacao = cmbLocalizacao.Text;
                salvarLoteamento.NumeroInscricao = txtInscricao.Text;
                salvarLoteamento.Nome = txtNome.Text;
                salvarLoteamento.Proprietario = txtProprietario.Text;
                if (txtMatricula.Text != "")
                    salvarLoteamento.Matricula = Convert.ToInt32(txtMatricula.Text);
                else
                    salvarLoteamento.Matricula = null;



              itemSelecionado =   classLoteamentos.Salvar(salvarLoteamento, status);

              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtMatricula_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtInscricao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
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

        private void txtNome_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void cmbLocalizacao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtProprietario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtConsulta_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtConsulta.Focus())
            {
                if (txtConsulta.Text == "")
                {
                    ConsultarTodos();
                }
                else
                {
                    if (rbNome.IsChecked == true)
                    {
                        dataGrid1.ItemsSource = classLoteamentos.ListaLoteamentosPorNome(txtConsulta.Text);
                        dataGrid1.Items.Refresh();
                    }

                    if (rbProprietario.IsChecked == true)
                    {
                        dataGrid1.ItemsSource = classLoteamentos.ListaLoteamentosPorProprietario(txtConsulta.Text);
                        dataGrid1.Items.Refresh();
                    }

                    if (rbMatricula.IsChecked == true)
                    {
                        if (txtConsulta.Text != "")
                        {
                            dataGrid1.ItemsSource = classLoteamentos.ListaLoteamentosPorMatricula(Convert.ToInt32(txtConsulta.Text));
                            dataGrid1.Items.Refresh();
                        }
                    }

                }
            }
        }

        

        private void txtConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (rbMatricula.IsChecked == true)
            {
                int key = (int)e.Key;
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
            }
        }

        private void rbNome_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Focus();
        }

        private void rbProprietario_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Focus();
        }

        private void rbMatricula_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Focus();
        }

    }
}
