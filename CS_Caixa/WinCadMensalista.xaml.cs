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
    /// Interaction logic for WinCadMensalista.xaml
    /// </summary>
    public partial class WinCadMensalista : Window
    {

        ClassMensalista classMensalista = new ClassMensalista();

        List<CadMensalista> mensalistas = new List<CadMensalista>();

        CadMensalista mensalistaSelecionado = new CadMensalista();

        string tipoSalvar;

        public WinCadMensalista()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarInicio();
        }

        private void CarregarInicio()
        {
            tipoSalvar = "pronto";
            GridDados.IsEnabled = false;
            GridConsulta.IsEnabled = true;
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            btnNovo.IsEnabled = true;
            dataGrid1.IsEnabled = true;

            mensalistas = classMensalista.ListaMensalistas();
            dataGrid1.ItemsSource = mensalistas;
            if (mensalistas.Count > 0)
            {
                dataGrid1.SelectedIndex = 0;
                dataGrid1.ScrollIntoView(dataGrid1.Items.CurrentItem);
            }
        }

        private void CarregarDepoisDeSalvar()
        {
            tipoSalvar = "pronto";
            GridDados.IsEnabled = false;
            GridConsulta.IsEnabled = true;
            btnCancelar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            btnNovo.IsEnabled = true;
            dataGrid1.IsEnabled = true;

            mensalistas = classMensalista.ListaMensalistas();
            dataGrid1.ItemsSource = mensalistas;

            mensalistaSelecionado = mensalistas.Where(p => p.Codigo == mensalistaSelecionado.Codigo).FirstOrDefault();

            dataGrid1.SelectedItem = mensalistaSelecionado;
            dataGrid1.ScrollIntoView(mensalistaSelecionado);
        }

        private void ProcNovo()
        {
            tipoSalvar = "novo";
            GridDados.IsEnabled = true;
            GridConsulta.IsEnabled = false;
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
            btnNovo.IsEnabled = false;
            dataGrid1.IsEnabled = false;
            txtCodigo.Text = "";
            txtMensalista.Text = "";
            txtCodigo.Focus();
        }

        private void ProcAlterar()
        {
            tipoSalvar = "alterar";
            GridDados.IsEnabled = true;
            GridConsulta.IsEnabled = false;
            btnCancelar.IsEnabled = true;
            btnSalvar.IsEnabled = true;
            btnNovo.IsEnabled = false;
            dataGrid1.IsEnabled = false;
        }

        private void ProcSalvar()
        {
            CadMensalista salvarMensalista;

            txtCodigo.Text = txtCodigo.Text.Trim();

            txtMensalista.Text = txtMensalista.Text.Trim();

            if (txtCodigo.Text == "")
            {
                MessageBox.Show("Digite o Código do Mensalista.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txtMensalista.Text == "")
            {
                MessageBox.Show("Digite o Nome do Mensalista.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (tipoSalvar == "novo")
            {
                salvarMensalista = new CadMensalista();
                var cod = classMensalista.ConsultarPorCod(txtCodigo.Text);

                if (cod != null)
                {
                    MessageBox.Show("Código do Mensalista já está cadastrado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            else
            {
                salvarMensalista = (CadMensalista)dataGrid1.SelectedItem;
                var cod = classMensalista.ConsultarPorCod(txtCodigo.Text);

                if (cod != null && cod.Cod != salvarMensalista.Cod)
                {
                    MessageBox.Show("Código do Mensalista já está cadastrado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            salvarMensalista.Cod = txtCodigo.Text;
            salvarMensalista.Nome = txtMensalista.Text;

            int ok = classMensalista.SalvarMensalista(salvarMensalista, tipoSalvar);

            if (ok == 0)
                MessageBox.Show("Erro ao tentar salvar Mensalista.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                salvarMensalista.Codigo = ok;
                mensalistaSelecionado = salvarMensalista;
                CarregarDepoisDeSalvar();

                MessageBox.Show("Registro salvo com sucesso!", "Salvo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void ConsultarNome()
        {
            mensalistas = classMensalista.Consultar("nome", txtConsulta.Text.Trim());
        }

        private void ConsultarCod()
        {
            mensalistas = classMensalista.Consultar("cod", txtConsulta.Text.Trim());
        }


        private void txtConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtConsulta.Text.Trim() != "")
                {
                    if (rbCodigo.IsChecked == true)
                        ConsultarCod();
                   
                    if (rbMensalista.IsChecked == true)
                        ConsultarNome();                   
                }
                else
                    mensalistas = classMensalista.ListaMensalistas();

                dataGrid1.ItemsSource = mensalistas;
                if (mensalistas.Count > 0)
                {
                    dataGrid1.SelectedIndex = 0;
                    dataGrid1.ScrollIntoView(dataGrid1.Items.CurrentItem);
                }
            }

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            CarregarInicio();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            ProcSalvar();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            ProcNovo();
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAlterar();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                mensalistaSelecionado = (CadMensalista)dataGrid1.SelectedItem;
                txtCodigo.Text = mensalistaSelecionado.Cod;
                txtMensalista.Text = mensalistaSelecionado.Nome;
            }
            else
            {
                txtCodigo.Text = "";
                txtMensalista.Text = "";
            }
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            ProcAlterar();
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                if (MessageBox.Show("Deseja realmente excluir este ítem?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classMensalista.ExcluirMensalista(mensalistaSelecionado);
                    CarregarInicio();
                }
            }
        }

        private void txtCodigo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtMensalista.Focus();
        }

        private void txtMensalista_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSalvar.Focus();
        }

        private void rbMensalista_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Focus();

            mensalistas = classMensalista.ListaMensalistas();

            dataGrid1.ItemsSource = mensalistas;
            if (mensalistas.Count > 0)
            {
                dataGrid1.SelectedIndex = 0;
                dataGrid1.ScrollIntoView(dataGrid1.Items.CurrentItem);
            }

        }

        private void rbCodigo_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Focus();

            mensalistas = classMensalista.ListaMensalistas();

            dataGrid1.ItemsSource = mensalistas;
            if (mensalistas.Count > 0)
            {
                dataGrid1.SelectedIndex = 0;
                dataGrid1.ScrollIntoView(dataGrid1.Items.CurrentItem);
            }
        }
    }
}
