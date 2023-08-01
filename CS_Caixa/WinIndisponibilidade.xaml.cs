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
using CS_Caixa.Models;
using CS_Caixa.Controls;
using CS_Caixa.RelatoriosForms;
using System.IO;
using System.Diagnostics;



namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinIndisponibilidade.xaml
    /// </summary>
    public partial class WinIndisponibilidade : Window
    {

        ClassIndisponibilidade indisp = new ClassIndisponibilidade();
        Indisponibilidade selecaoIndisponibilidade = new Indisponibilidade();
        List<Indisponibilidade> listaIndisp = new List<Indisponibilidade>();

        string status = "pronto";

        int indiceGrid;


        public WinIndisponibilidade()
        {

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IniciaForm();

            if (dataGrid1.Items.Count > 0)
                dataGrid1.SelectedIndex = 0;

        }

        private void IniciaForm()
        {
            caminho = string.Empty;

            btnSalvar.IsEnabled = false;
            btnNovo.IsEnabled = true;
            btnCancelar.IsEnabled = false;
            groupBoxConsulta.IsEnabled = true;
            FechaCampos();

            if(status == "novo" || status == "pronto")
            ConsultaRegistros();

            dataGrid1.IsEnabled = true;

            if (dataGrid1.Items.Count > 0)
                indiceGrid = dataGrid1.SelectedIndex;
        }
        
       
        private void FechaCampos()
        {
            txtTitulo.IsEnabled = false;
            txtNome.IsEnabled = false;
            txtCpfCnpj.IsEnabled = false;
            txtAviso.IsEnabled = false;
            txtOficio.IsEnabled = false;
            txtObs.IsEnabled = false;
            txtProcesso.IsEnabled = false;
        }

        private void AbreCampos()
        {
            txtTitulo.IsEnabled = true;
            txtNome.IsEnabled = true;
            txtCpfCnpj.IsEnabled = true;
            txtAviso.IsEnabled = true;
            txtOficio.IsEnabled = true;
            txtProcesso.IsEnabled = true;
            txtObs.IsEnabled = true;
        }


        private void LimpaCampos()
        {
            //txtTitulo.Text = "";
            txtNome.Text = "";
            txtCpfCnpj.Text = "";
            //txtAviso.Text = "";
            //txtOficio.Text = "";
            //txtProcesso.Text = "";
            //txtObs.Text = "";
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.Items.Count > 0)
            {
                if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool ok = indisp.ExcluirIndisp(selecaoIndisponibilidade);

                        if (ok)
                        {
                            listaIndisp.RemoveAt(dataGrid1.SelectedIndex);
                            ConsultaRegistros();
                        }
                        if (selecaoIndisponibilidade == null)
                        {
                            dataGrid1.SelectedIndex = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void MenuItemAlterar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.Items.Count > 0)
            {
                status = "alterar";
                btnCancelar.IsEnabled = true;
                btnNovo.IsEnabled = false;
                btnSalvar.IsEnabled = true;
                AbreCampos();
                dataGrid1.IsEnabled = false;
                groupBoxConsulta.IsEnabled = false;
                txtTitulo.Focus();
            }
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            status = "novo";
            AbreCampos();
            LimpaCampos();
            dataGrid1.IsEnabled = false;
            btnNovo.IsEnabled = false;
            btnSalvar.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            groupBoxConsulta.IsEnabled = false;
            txtTitulo.Focus();
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selecaoIndisponibilidade = (Indisponibilidade)dataGrid1.SelectedItem;


                if (selecaoIndisponibilidade == null)
                {
                    dataGrid1.SelectedIndex = 0;
                }

                if (dataGrid1.SelectedIndex > -1)
                    indiceGrid = dataGrid1.SelectedIndex;


            }
            catch (Exception)
            {

            }

            if (dataGrid1.Items.Count > 0)
                PreencheOsCampos();
            else
                LimpaCampos();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            FechaCampos();
            btnCancelar.IsEnabled = false;

            btnNovo.IsEnabled = true;

            btnSalvar.IsEnabled = false;

            dataGrid1.IsEnabled = true;

            groupBoxConsulta.IsEnabled = true;
            try
            {
                selecaoIndisponibilidade = (Indisponibilidade)dataGrid1.SelectedItem;

                if (selecaoIndisponibilidade == null)
                {
                    dataGrid1.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {

            }
            if (dataGrid1.Items.Count > 0)
                PreencheOsCampos();
            else
                LimpaCampos();


        }


        private void PreencheOsCampos()
        {

            txtTitulo.Text = selecaoIndisponibilidade.Titulo;
            txtNome.Text = selecaoIndisponibilidade.Nome;
            txtCpfCnpj.Text = selecaoIndisponibilidade.CpfCnpj;
            txtAviso.Text = selecaoIndisponibilidade.Aviso;
            txtOficio.Text = selecaoIndisponibilidade.Oficio;
            txtProcesso.Text = selecaoIndisponibilidade.Processo;
            txtObs.Text = selecaoIndisponibilidade.Valor;

        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            Indisponibilidade salvarIndisp = new Indisponibilidade();
            try
            {
                salvarIndisp.Titulo = txtTitulo.Text.Trim();

                salvarIndisp.Nome = txtNome.Text.Trim();

                salvarIndisp.CpfCnpj = txtCpfCnpj.Text.Trim();

                salvarIndisp.Oficio = txtOficio.Text.Trim();

                salvarIndisp.Aviso = txtAviso.Text.Trim();

                salvarIndisp.Processo = txtProcesso.Text.Trim();

                salvarIndisp.Valor = txtObs.Text.Trim();

                if (status == "alterar")
                {
                    salvarIndisp.IdIndisponibilidade = selecaoIndisponibilidade.IdIndisponibilidade;

                    listaIndisp[dataGrid1.SelectedIndex] = selecaoIndisponibilidade;

                    dataGrid1.ItemsSource = listaIndisp;

                    dataGrid1.Items.Refresh();
                }

                 indisp.SalvarIndisp(salvarIndisp, status);

               
                



                IniciaForm();
                dataGrid1.SelectedIndex = indiceGrid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void GridDados_PreviewKeyDown(object sender, KeyEventArgs e)
        {

           
               
           
        }

        private void PassarNoEnter(object sender, KeyEventArgs e)
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
        private void txtConsulta_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConsultaRegistros();
        }


        private void ConsultaRegistros()
        {
            listaIndisp = indisp.ListarIndisponibilidade(cmbTipoConsulta.Text, txtConsulta.Text);

            dataGrid1.ItemsSource = listaIndisp;
            dataGrid1.Items.Refresh();

            label7.Content = string.Format("Total de Registros Encontrados: {0}", listaIndisp.Count);

            if (dataGrid1.Items.Count > 0)
            {
                dataGrid1.SelectedIndex = 0;
                PreencheOsCampos();
            }
            else
                LimpaCampos();
        }



        private void cmbTipoConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoConsulta.Focus())
                txtConsulta.Focus();
        }

        private void MenuItemImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.Items.Count > 0)
            {
                FrmFichaIndisponibilidade ficha = new FrmFichaIndisponibilidade(selecaoIndisponibilidade.Titulo, selecaoIndisponibilidade.Nome, selecaoIndisponibilidade.CpfCnpj, selecaoIndisponibilidade.Oficio, selecaoIndisponibilidade.Aviso, selecaoIndisponibilidade.Processo, selecaoIndisponibilidade.Valor);
                ficha.ShowDialog();
                ficha.Dispose();
            }
        }



        string caminho;

        private void MenuItemImportar_Click(object sender, RoutedEventArgs e)
        {


            status = "importar";
            WinAguardeIndisponibilidade aguarde = new WinAguardeIndisponibilidade(this);
            aguarde.Owner = this;
            aguarde.ShowDialog();


        }

        private void txtCpfCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtCpfCnpj.Text != "")
            {
                string documento = txtCpfCnpj.Text;

                if (documento.Contains('.') || documento.Contains('-') || documento.Contains('/'))
                {
                    documento = documento.Replace(".", "").Replace("-", "").Replace("/", "");
                }
                try
                {

                    if (documento.Length == 11)
                    {
                        string parte0 = documento.Substring(0, 3);
                        string parte1 = documento.Substring(3, 3);
                        string parte2 = documento.Substring(6, 3);
                        string parte3 = documento.Substring(9, 2);

                        txtCpfCnpj.Text = string.Format("{0}.{1}.{2}-{3}", parte0, parte1, parte2, parte3);
                    }
                    else
                    {
                        if (documento.Length == 14)
                        {
                            string parte0 = documento.Substring(0, 2);
                            string parte1 = documento.Substring(2, 3);
                            string parte2 = documento.Substring(5, 3);
                            string parte3 = documento.Substring(8, 4);
                            string parte4 = documento.Substring(12, 2);

                            txtCpfCnpj.Text = string.Format("{0}.{1}.{2}/{3}-{4}", parte0, parte1, parte2, parte3, parte4);
                        }
                        else
                        {
                            MessageBox.Show("Favor Verifique o documento digitado.", "Cpf/Cnpj", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Favor Verifique o documento digitado.", "Cpf/Cnpj", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void txtCpfCnpj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25 || key == 32);

            PassarNoEnter(sender, e);
        }

        private void txtCpfCnpj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtCpfCnpj.Text.Contains('.') || txtCpfCnpj.Text.Contains('-') || txtCpfCnpj.Text.Contains('/'))
            {
                txtCpfCnpj.Text = txtCpfCnpj.Text.Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }

        private void txtTitulo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarNoEnter(sender, e);
        }

        private void txtNome_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarNoEnter(sender, e);
        }

      

        private void txtOficio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarNoEnter(sender, e);
        }

        private void txtAviso_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarNoEnter(sender, e);
        }

        private void txtProcesso_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PassarNoEnter(sender, e);
        }



    }
}
