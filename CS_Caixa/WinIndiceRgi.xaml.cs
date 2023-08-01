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
using CS_Caixa.RelatoriosForms;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using CS_Caixa.ObterToken;
using System.Security.Cryptography;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinIndiceRgi.xaml
    /// </summary>
    public partial class WinIndiceRgi : Window
    {


       

        string status = "pronto";

        ClassIndiceRgi classIndiceRgi = new ClassIndiceRgi();

        public List<IndiceRegistro> listaIndiceRegistro = new List<IndiceRegistro>();

        IndiceRegistro itemSelecionado = new IndiceRegistro();

        int idRegistro;

        public bool geradoSucesso = false;

        public string caminho = string.Empty;

        bool naoCalc = true;

        Usuario _usuario;

        public WinIndiceRgi(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VerificaComponentes();
            rbNomeCpfCnpj.IsChecked = true;
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
                btnCenib.IsEnabled = true;

                if (dataGrid1.SelectedIndex > -1)
                    btnImagens.IsEnabled = true;
                else
                    btnImagens.IsEnabled = false;

            }
            if (status == "novo" || status == "alterar")
            {
                btnSalvar.IsEnabled = true;
                btnNovo.IsEnabled = false;
                btnCancelar.IsEnabled = true;
                btnCenib.IsEnabled = false;
                btnImagens.IsEnabled = false;
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
            cmbReg.SelectedIndex = -1;
            txtLivro.Text = "";
            txtFls.Text = "";
            txtNumero.Text = "";
            txtOrdem.Text = "";
            rbPessoaFisica.IsChecked = false;
            rbPessoaJuridica.IsChecked = false;
            txtCpfCnpj.Text = "";
            txtTipoAto.Text = "";
            dpDataRegistro.SelectedDate = null;
            dpDataVenda.SelectedDate = null;
            txtDiaRegistro.Text = "";
            txtDiaVenda.Text = "";
            txtMesRegistro.Text = "";
            txtMesVenda.Text = "";
            txtAnoRegistro.Text = "";
            txtAnoVenda.Text = "";
        }

        private void PreencheCampos()
        {
            try
            {

                itemSelecionado = (IndiceRegistro)dataGrid1.SelectedItem;

                if (itemSelecionado != null)
                {
                    txtNome.Text = itemSelecionado.Nome;
                    txtLivro.Text = itemSelecionado.Livro;
                    cmbReg.Text = itemSelecionado.Reg;
                    txtFls.Text = itemSelecionado.Fls;
                    txtNumero.Text = itemSelecionado.Numero;
                    txtOrdem.Text = itemSelecionado.Ordem;
                    txtTipoAto.Text = itemSelecionado.TipoAto;




                    if (itemSelecionado.DataRegistro != null)
                    {
                        txtDiaRegistro.Text = string.Format("{0:00}", itemSelecionado.DataRegistro.Value.Day);
                        txtMesRegistro.Text = string.Format("{0:00}", itemSelecionado.DataRegistro.Value.Month);
                        txtAnoRegistro.Text = string.Format("{0:0000}", itemSelecionado.DataRegistro.Value.Year);

                        dpDataRegistro.SelectedDate = itemSelecionado.DataRegistro;
                    }
                    else
                    {
                        txtDiaRegistro.Text = "";
                        txtMesRegistro.Text = "";
                        txtAnoRegistro.Text = "";
                    }



                    if (itemSelecionado.DataVenda != null)
                    {
                        txtDiaVenda.Text = string.Format("{0:00}", itemSelecionado.DataVenda.Value.Day);
                        txtMesVenda.Text = string.Format("{0:00}", itemSelecionado.DataVenda.Value.Month);
                        txtAnoVenda.Text = string.Format("{0:0000}", itemSelecionado.DataVenda.Value.Year);

                        dpDataVenda.SelectedDate = itemSelecionado.DataVenda;
                    }
                    else
                    {
                        txtDiaVenda.Text = "";
                        txtMesVenda.Text = "";
                        txtAnoVenda.Text = "";
                    }




                    if (itemSelecionado.CpfCnpj != null)
                    {

                        if (itemSelecionado.CpfCnpj.Trim() != "")
                        {

                            if (itemSelecionado.TipoPessoa == "F")
                                rbPessoaFisica.IsChecked = true;
                            else
                                rbPessoaJuridica.IsChecked = true;
                            if (itemSelecionado.CpfCnpj.Length == 11)
                            {
                                if (ValidaCpfCnpj.ValidaCPF(itemSelecionado.CpfCnpj) == false)
                                {
                                    txtCpfCnpj.Background = Brushes.Red;
                                }
                                else
                                    txtCpfCnpj.Background = Brushes.White;

                                txtCpfCnpj.Text = string.Format("{0}.{1}.{2}-{3}", itemSelecionado.CpfCnpj.Substring(0, 3), itemSelecionado.CpfCnpj.Substring(3, 3), itemSelecionado.CpfCnpj.Substring(6, 3), itemSelecionado.CpfCnpj.Substring(9, 2));

                            }
                            else
                            {
                                if (itemSelecionado.CpfCnpj.Length == 14)
                                {
                                    if (ValidaCpfCnpj.ValidaCNPJ(itemSelecionado.CpfCnpj) == false)
                                    {
                                        txtCpfCnpj.Background = Brushes.Red;
                                    }
                                    else
                                        txtCpfCnpj.Background = Brushes.White;

                                    txtCpfCnpj.Text = string.Format("{0}.{1}.{2}/{3}-{4}", itemSelecionado.CpfCnpj.Substring(0, 2), itemSelecionado.CpfCnpj.Substring(2, 3), itemSelecionado.CpfCnpj.Substring(5, 3), itemSelecionado.CpfCnpj.Substring(8, 4), itemSelecionado.CpfCnpj.Substring(12, 2));
                                }
                                else
                                {
                                    txtCpfCnpj.Background = Brushes.Red;
                                }
                            }
                        }
                        else
                        {
                            txtCpfCnpj.Text = "";
                            rbPessoaFisica.IsChecked = false;
                            rbPessoaFisica.IsChecked = false;
                        }
                    }
                    else
                    {
                        txtCpfCnpj.Text = "";
                        rbPessoaFisica.IsChecked = false;
                        rbPessoaFisica.IsChecked = false;
                    }
                }
                else
                {
                    LimpaCampos();
                }

                if (dataGrid1.SelectedIndex > -1)
                    btnImagens.IsEnabled = true;
                else
                    btnImagens.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Salvar()
        {
            IndiceRegistro salvarIndice = new IndiceRegistro();

            try
            {
                salvarIndice.Nome = txtNome.Text;


                salvarIndice.Reg = cmbReg.Text;
                salvarIndice.Numero = txtNumero.Text;
                salvarIndice.Livro = txtLivro.Text;
                salvarIndice.Fls = txtFls.Text;
                salvarIndice.Ordem = txtOrdem.Text;
                
                if (rbPessoaFisica.IsChecked == true)
                    salvarIndice.TipoPessoa = "F";
                if (rbPessoaJuridica.IsChecked == true)
                    salvarIndice.TipoPessoa = "J";
                salvarIndice.CpfCnpj = txtCpfCnpj.Text.Replace(".", "").Replace("-", "").Replace("/", "");

                if (status == "alterar")
                    salvarIndice.IdIndiceRegistros = itemSelecionado.IdIndiceRegistros;

                salvarIndice.TipoAto = txtTipoAto.Text.Trim();
                if (txtDiaRegistro.Text.Length == 2 && txtMesRegistro.Text.Length == 2 && txtAnoRegistro.Text.Length == 4)
                    salvarIndice.DataRegistro = Convert.ToDateTime(string.Format("{0}/{1}/{2}", txtDiaRegistro.Text, txtMesRegistro.Text, txtAnoRegistro.Text));
                if (txtDiaVenda.Text.Length == 2 && txtMesVenda.Text.Length == 2 && txtAnoVenda.Text.Length == 4)
                    salvarIndice.DataVenda = Convert.ToDateTime(string.Format("{0}/{1}/{2}", txtDiaVenda.Text, txtMesVenda.Text, txtAnoVenda.Text));
                
                if (status == "novo")
                salvarIndice.Enviado = false;

                idRegistro = classIndiceRgi.SalvarIndiceRegistro(salvarIndice, status);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Consulta()
        {
            listaIndiceRegistro = classIndiceRgi.ListarIndiceRegistroNome(txtConsulta.Text);

            dataGrid1.ItemsSource = listaIndiceRegistro;
        }

        private void ConsultaPeriodo()
        {
            listaIndiceRegistro = classIndiceRgi.ListarIndiceRegistroPeriodo(dpDataConsultaInicio.SelectedDate.Value, dpDataConsultaFim.SelectedDate.Value);

            dataGrid1.ItemsSource = listaIndiceRegistro;
        }

        private void ConsultaMatricula()
        {
            listaIndiceRegistro = classIndiceRgi.ListarIndiceRegistroMatricula(txtConsulta.Text.Trim());

            dataGrid1.ItemsSource = listaIndiceRegistro;
        }

        private void Consulta(int id)
        {
            listaIndiceRegistro = classIndiceRgi.ListarIndiceRegistroSalvo(id);

            dataGrid1.ItemsSource = listaIndiceRegistro;
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
            rbPessoaFisica.Focus();
            rbPessoaFisica.IsChecked = true;
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                string dataStringRegistro = string.Format("{0}/{1}/{2}", txtDiaRegistro.Text, txtMesRegistro.Text, txtAnoRegistro.Text);

                var dataRegistro = new DateTime();

                dataRegistro = Convert.ToDateTime(dataStringRegistro);

                dpDataRegistro.SelectedDate = dataRegistro;


                //string dataStringVenda = string.Format("{0}/{1}/{2}", txtDiaVenda.Text, txtMesVenda.Text, txtAnoVenda.Text);

                //var dataVenda = new DateTime();

                //dataVenda = Convert.ToDateTime(dataStringVenda);

                //dpDataVenda.SelectedDate = dataVenda;


            }
            catch (Exception)
            {
                MessageBox.Show("Favor verificar a Data do Registro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }



            txtNome.Text = txtNome.Text.Trim();

            if (txtNome.Text != "" && txtNome.Text != " ")
            {

                if (txtOrdem.Text != "" && txtOrdem.Text != " ")
                {
                    if (txtTipoAto.Text != "" && txtTipoAto.Text != " ")
                    {
                        if (txtDiaRegistro.Text.Length == 2 && txtMesRegistro.Text.Length == 2 && txtAnoRegistro.Text.Length == 4)
                        {

                            Salvar();

                            if (status == "novo")
                                Consulta(idRegistro);
                            status = "pronto";
                            VerificaComponentes();
                            btnNovo.Focus();
                            dataGrid1.Items.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Informe a Data do Registro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                            txtDiaRegistro.Focus();
                            txtDiaRegistro.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Informe o Tipo de Registro.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                        txtTipoAto.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Informe a Matrícula.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtOrdem.Focus();
                }

            }
            else
            {
                MessageBox.Show("Informe o participante.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Stop);
                txtNome.Focus();
            }

            lblQtd.Content = string.Format("Qtd de Registros Encontrados: {0}", listaIndiceRegistro.Count);
        }




        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {


            if (rbNomeCpfCnpj.IsChecked == true)
            {

                txtConsulta.Text = ClassIndiceRgi.RemoveAcentos(txtConsulta.Text);

                try
                {

                    if (txtConsulta.Text != "" && txtConsulta.Text != " " && txtConsulta.Text != "  ")
                    {
                        MenuItemImprimir.IsEnabled = true;

                        if (txtConsulta.Text.Length > 2)
                        {
                            int n;
                            bool ehUmNumero1 = int.TryParse(txtConsulta.Text.Substring(0, 2), out n);

                            int n2;
                            bool ehUmNumero2 = int.TryParse(txtConsulta.Text.Substring(txtConsulta.Text.Length - 2, 2), out n2);

                            if (ehUmNumero1 && ehUmNumero2)
                                txtConsulta.Text = txtConsulta.Text.Replace(".", "").Replace("-", "").Replace("/", "");
                        }
                        Consulta();
                        DataGrid();
                    }
                    else
                    {

                        MenuItemImprimir.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (rbDataRegistro.IsChecked == true)
            {

                if (dpDataConsultaInicio.SelectedDate != null && dpDataConsultaFim.SelectedDate != null)
                {
                    ConsultaPeriodo();
                    DataGrid();
                }
                else
                {
                    MessageBox.Show("Informe a data início e fim.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (rbMatricula.IsChecked == true)
            {
                if (txtConsulta.Text != "")
                {
                    ConsultaMatricula();
                    DataGrid();
                }
                else
                {
                    MessageBox.Show("Informe a data início e fim.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            lblQtd.Content = string.Format("Qtd de Registros Encontrados: {0}", listaIndiceRegistro.Count);
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreencheCampos();
        }

        private void MenuItemExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir este registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                bool ok = classIndiceRgi.Excluir(itemSelecionado);
                if (ok)
                {
                    listaIndiceRegistro.Remove(itemSelecionado);
                    dataGrid1.ItemsSource = listaIndiceRegistro;
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
            txtNome.Focus();
            txtNome.SelectAll();
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

        private void txtNome_LostFocus(object sender, RoutedEventArgs e)
        {
            txtNome.Text = ClassIndiceRgi.RemoveAcentos(txtNome.Text);
        }

        private void cmbReg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbReg.Focus())
                txtNumero.Focus();
        }

        private void txtNumero_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtFls_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtOrdem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtConsulta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);

            if (rbMatricula.IsChecked == true)
            {
                int key = (int)e.Key;
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
            }

        }


        private void txtNumero_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNumero.Text = ClassIndiceRgi.RemoveAcentos(txtNumero.Text);
        }

        private void txtFls_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFls.Text = ClassIndiceRgi.RemoveAcentos(txtFls.Text);
        }

        private void txtOrdem_GotFocus(object sender, RoutedEventArgs e)
        {
            txtOrdem.Text = ClassIndiceRgi.RemoveAcentos(txtOrdem.Text);
        }

        private void MenuItemImprimir_Click(object sender, RoutedEventArgs e)
        {
            FrmIndiceRgi indice = new FrmIndiceRgi(txtConsulta.Text);
            indice.ShowDialog();
            indice.Close();

        }

        private void txtConsulta_TextChanged(object sender, TextChangedEventArgs e)
        {
            MenuItemImprimir.IsEnabled = false;
        }




        private void btnCenib_Click(object sender, RoutedEventArgs e)
        {
            

            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            openFileDialog1.Filter = "All files (*.xml)|*.xml";

            openFileDialog1.InitialDirectory = @"\\SERVIDOR\Cartorio\Arquivos Cartório\Arquivos RGI\CNIB";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                WinConsultaCenib cenib = new WinConsultaCenib(openFileDialog1.FileName);
                cenib.Owner = this;
                cenib.ShowDialog();
            }
        }

        private void rbPessoaFisica_Checked(object sender, RoutedEventArgs e)
        {
            txtCpfCnpj.Text = "";
            txtCpfCnpj.MaxLength = 11;
            txtCpfCnpj.Focus();
        }

        private void rbPessoaJuridica_Checked(object sender, RoutedEventArgs e)
        {
            txtCpfCnpj.Text = "";
            txtCpfCnpj.MaxLength = 14;
            txtCpfCnpj.Focus();
        }

        private void txtCpfCnpj_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCpfCnpj.Text = txtCpfCnpj.Text.Replace(".", "").Replace("-", "").Replace("/", "");
        }

        private void txtCpfCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCpfCnpj.Text.Length == 11)
                {
                    if (ValidaCpfCnpj.ValidaCPF(txtCpfCnpj.Text) == false)
                    {
                        MessageBox.Show("Cpf Inválido.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtCpfCnpj.Background = Brushes.Red;
                    }
                    else
                    {
                        txtCpfCnpj.Background = Brushes.White;


                        if (status == "novo")
                        {
                            ObterNomeTotalRgi();
                            if (txtNome.Text == "")
                                ObterNomeTotal();
                            if (txtNome.Text == "")
                            {
                                List<IndiceRegistro> lista = classIndiceRgi.ListarIndiceRegistroNome(txtCpfCnpj.Text);
                                if (lista != null)
                                    txtNome.Text = lista.FirstOrDefault().Nome;

                            }
                        }

                        txtCpfCnpj.Text = string.Format("{0}.{1}.{2}-{3}", txtCpfCnpj.Text.Substring(0, 3), txtCpfCnpj.Text.Substring(3, 3), txtCpfCnpj.Text.Substring(6, 3), txtCpfCnpj.Text.Substring(9, 2));
                    }
                }
                else
                {
                    if (txtCpfCnpj.Text.Length == 14)
                    {
                        if (ValidaCpfCnpj.ValidaCNPJ(txtCpfCnpj.Text) == false)
                        {
                            MessageBox.Show("Cnpj Inválido.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            txtCpfCnpj.Background = Brushes.Red;
                        }
                        else
                        {
                            txtCpfCnpj.Background = Brushes.White;

                            if (status == "novo")
                            {
                                ObterNomeTotalRgi();
                                if (txtNome.Text == "")
                                    ObterNomeTotal();
                                if (txtNome.Text == "")
                                {
                                    List<IndiceRegistro> lista = classIndiceRgi.ListarIndiceRegistroNome(txtCpfCnpj.Text);
                                    if (lista != null)
                                        txtNome.Text = lista.FirstOrDefault().Nome;

                                }
                            }

                            txtCpfCnpj.Text = string.Format("{0}.{1}.{2}/{3}-{4}", txtCpfCnpj.Text.Substring(0, 2), txtCpfCnpj.Text.Substring(2, 3), txtCpfCnpj.Text.Substring(5, 3), txtCpfCnpj.Text.Substring(8, 4), txtCpfCnpj.Text.Substring(12, 2));
                        }
                    }
                }

                txtNome.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ObterNomeTotal()
        {
            DataTable dtTotal = new DataTable();
            DataGrid dados = new DataGrid();
            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingCentral);
            conTotal.Open();
            try
            {

                FbCommand cmdTotal = new FbCommand("select nome from pessoas where cpf_cgc = '" + txtCpfCnpj.Text + "'", conTotal);
                cmdTotal.CommandType = CommandType.Text;


                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                dtTotal = new DataTable();
                dtTotal.Load(drTotal);

                txtNome.Text = dtTotal.Rows[0][0].ToString();
                conTotal.Close();
            }
            catch (Exception) { }
        }


        private void ObterNomeTotalRgi()
        {
            DataTable dtTotal = new DataTable();
            DataGrid dados = new DataGrid();
            FbConnection conTotal = new FbConnection(Properties.Settings.Default.SettingRgi);
            conTotal.Open();
            try
            {

                FbCommand cmdTotal = new FbCommand("select nome from ato_pessoa where cpf_cnpj = '" + txtCpfCnpj.Text + "'", conTotal);
                cmdTotal.CommandType = CommandType.Text;


                FbDataReader drTotal;
                drTotal = cmdTotal.ExecuteReader();

                dtTotal = new DataTable();
                dtTotal.Load(drTotal);

                txtNome.Text = dtTotal.Rows[0][0].ToString();
                conTotal.Close();
            }
            catch (Exception) { }
        }




        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            status = "alterar";
            VerificaComponentes();
            txtNome.Focus();
            txtNome.SelectAll();
        }

        private void rbPessoaFisica_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void rbPessoaJuridica_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void cmbReg_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtTipoAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void dpDataRegistro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void dpDataVenda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void MenuItemGerarXml_Click(object sender, RoutedEventArgs e)
        {

            var enviar = listaIndiceRegistro.Where(p => p.Enviado == true);

            if (enviar.Count() > 0)
            {

                var aguarde = new AguardeGerandoXml(this);
                aguarde.Owner = this;
                aguarde.ShowDialog();

                dataGrid1.ItemsSource = listaIndiceRegistro;
                dataGrid1.Items.Refresh();

                if (dpDataConsultaInicio.SelectedDate != null && dpDataConsultaFim.SelectedDate != null)
                {
                    ConsultaPeriodo();
                    DataGrid();
                }


                if (geradoSucesso)
                {
                    MessageBox.Show("Arquivo XML Gerado com sucesso! \n\n" + caminho, "XML", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("Selecione os nomes para enviar. ","XML", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void rbNomeCpfCnpj_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Visibility = Visibility.Visible;
            dpDataConsultaInicio.Visibility = Visibility.Hidden;
            dpDataConsultaFim.Visibility = Visibility.Hidden;
            txtConsulta.Focus();

        }

        private void rbDataRegistro_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Visibility = Visibility.Hidden;
            dpDataConsultaInicio.Visibility = Visibility.Visible;
            dpDataConsultaFim.Visibility = Visibility.Visible;
        }

        private void dpDataConsultaInicio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void dpDataConsultaFim_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void txtCpfCnpj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);

            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            if (rbPessoaFisica.IsChecked == false && rbPessoaJuridica.IsChecked == false)
                MessageBox.Show("Informe o Tipo de Pessoa.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void txtLivro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TabEnter(sender, e);
        }

        private void rbMatricula_Checked(object sender, RoutedEventArgs e)
        {
            txtConsulta.Text = "";
            txtConsulta.Visibility = Visibility.Visible;
            dpDataConsultaInicio.Visibility = Visibility.Hidden;
            dpDataConsultaFim.Visibility = Visibility.Hidden;
            txtConsulta.Focus();
        }

        private void txtOrdem_LostFocus(object sender, RoutedEventArgs e)
        {

        }


        private void CalcularDataRegistro()
        {
            if (txtDiaRegistro.Text.Length == 2 && txtMesRegistro.Text.Length == 2 && txtAnoRegistro.Text.Length == 4)
            {

                string dataString = string.Format("{0}/{1}/{2}", txtDiaRegistro.Text, txtMesRegistro.Text, txtAnoRegistro.Text);

                var data = new DateTime();

                data = Convert.ToDateTime(dataString);

                dpDataRegistro.SelectedDate = data;
            }

        }

        private void CalcularDataVenda()
        {
            if (txtDiaVenda.Text.Length == 2 && txtMesVenda.Text.Length == 2 && txtAnoVenda.Text.Length == 4)
            {

                string dataString = string.Format("{0}/{1}/{2}", txtDiaVenda.Text, txtMesVenda.Text, txtAnoVenda.Text);

                var data = new DateTime();

                data = Convert.ToDateTime(dataString);

                dpDataVenda.SelectedDate = data;
            }

        }


        private void txtDiaRegistro_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                if (txtDiaRegistro.Focus() == true)
                {
                    if (txtDiaRegistro.Text.Length > 0)
                    {
                        int diaInt = Convert.ToInt16(txtDiaRegistro.Text);

                        if (txtDiaRegistro.Text != "00" && diaInt < 32)
                        {
                            if (txtDiaRegistro.Text.Length == 2)
                            {
                                txtMesRegistro.Focus();
                                txtMesRegistro.SelectAll();
                                txtDiaRegistro.Background = Brushes.White;
                            }
                            else
                                txtDiaRegistro.Background = Brushes.Red;
                        }
                        else
                        {

                            txtDiaRegistro.Focus();

                            txtDiaRegistro.SelectAll();

                            txtDiaRegistro.Background = Brushes.Red;
                        }

                        if (naoCalc == false)
                            CalcularDataRegistro();
                    }
                }
            }
            catch (Exception)
            {

                txtDiaRegistro.Background = Brushes.Red;
            };
        }


        private void txtDiaRegistro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDiaRegistro.Text.Length > 0)
                {
                    txtDiaRegistro.Text = string.Format("{0:00}", Convert.ToInt16(txtDiaRegistro.Text));

                    int diaInt = Convert.ToInt16(txtDiaRegistro.Text);

                    if (txtDiaRegistro.Text != "00" && diaInt < 32)
                    {
                        if (txtDiaRegistro.Text.Length == 2)
                        {
                            txtMesRegistro.Focus();
                            txtMesRegistro.SelectAll();
                            txtDiaRegistro.Background = Brushes.White;
                        }
                    }
                    else
                    {

                        txtDiaRegistro.Text = "";
                        txtDiaRegistro.Focus();
                        txtDiaRegistro.Background = Brushes.Red;
                    }
                }

            }
            catch (Exception)
            {

                txtDiaRegistro.Background = Brushes.Red;
            };
        }

        private void txtDiaRegistro_PreviewKeyDown(object sender, KeyEventArgs e)
        {


            naoCalc = false;

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);


        }

        private void txtMesRegistro_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtMesRegistro.Focus() == true)
                {
                    if (txtMesRegistro.Text.Length > 0)
                    {
                        int mesInt = Convert.ToInt16(txtMesRegistro.Text);

                        if (txtMesRegistro.Text != "00" && mesInt < 13)
                        {
                            if (txtMesRegistro.Text.Length == 2)
                            {
                                txtAnoRegistro.Focus();
                                txtAnoRegistro.SelectAll();
                                txtMesRegistro.Background = Brushes.White;
                            }
                            else
                                txtMesRegistro.Background = Brushes.Red;
                        }
                        else
                        {

                            txtMesRegistro.Focus();

                            txtMesRegistro.SelectAll();



                            txtMesRegistro.Background = Brushes.Red;
                        }

                        if (naoCalc == false)
                            CalcularDataRegistro();
                    }
                }
            }
            catch (Exception)
            {

                txtMesRegistro.Background = Brushes.Red;
            };
        }

        private void txtMesRegistro_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMesRegistro.Text.Length > 0)
                {
                    txtMesRegistro.Text = string.Format("{0:00}", Convert.ToInt16(txtMesRegistro.Text));

                    int mesInt = Convert.ToInt16(txtMesRegistro.Text);

                    if (mesInt > 0 && mesInt < 13)
                    {
                        if (txtMesRegistro.Text.Length == 2)
                        {
                            txtAnoRegistro.Focus();
                            txtAnoRegistro.SelectAll();
                            txtMesRegistro.Background = Brushes.White;
                        }
                    }
                    else
                    {

                        txtMesRegistro.Text = "";
                        txtMesRegistro.Focus();
                        txtMesRegistro.Background = Brushes.Red;
                    }


                }

            }
            catch (Exception)
            {

                txtMesRegistro.Background = Brushes.Red;
            };
        }

        private void txtMesRegistro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            naoCalc = false;

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

        }

        private void txtAnoRegistro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtAnoRegistro.Text.Length == 4)
            {
                try
                {

                    int anoInt = Convert.ToInt16(txtAnoRegistro.Text);

                    if (anoInt > 1900 && anoInt <= DateTime.Now.Date.Year)
                    {
                        txtAnoRegistro.Background = Brushes.White;
                    }
                    else
                    {
                        txtAnoRegistro.Background = Brushes.Red;
                        return;
                    }

                    if (naoCalc == false)
                    {

                        CalcularDataRegistro();
                    }

                }
                catch (Exception)
                {

                };
            }
        }

        private void txtAnoRegistro_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAnoRegistro.Text.Length > 0)
                txtAnoRegistro.Text = string.Format("{0:0000}", Convert.ToInt16(txtAnoRegistro.Text));
        }

        private void txtAnoRegistro_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            naoCalc = false;

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            TabEnter(sender, e);
        }


        private void txtDiaVenda_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtDiaVenda.Focus() == true)
                {
                    if (txtDiaVenda.Text.Length > 0)
                    {
                        int diaInt = Convert.ToInt16(txtDiaVenda.Text);

                        if (txtDiaVenda.Text != "00" && diaInt < 32)
                        {
                            if (txtDiaVenda.Text.Length == 2)
                            {
                                txtMesVenda.Focus();
                                txtMesVenda.SelectAll();
                                txtDiaVenda.Background = Brushes.White;
                            }
                            else
                                txtDiaVenda.Background = Brushes.Red;
                        }
                        else
                        {

                            txtDiaVenda.Focus();

                            txtDiaVenda.SelectAll();

                            txtDiaVenda.Background = Brushes.Red;
                        }

                        if (naoCalc == false)
                            CalcularDataVenda();
                    }
                }
            }
            catch (Exception)
            {

                txtDiaVenda.Background = Brushes.Red;
            };
        }

        private void txtDiaVenda_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDiaVenda.Text.Length > 0)
                {
                    txtDiaVenda.Text = string.Format("{0:00}", Convert.ToInt16(txtDiaVenda.Text));

                    int diaInt = Convert.ToInt16(txtDiaVenda.Text);

                    if (txtDiaVenda.Text != "00" && diaInt < 32)
                    {
                        if (txtDiaVenda.Text.Length == 2)
                        {
                            txtMesVenda.Focus();
                            txtMesVenda.SelectAll();
                            txtDiaVenda.Background = Brushes.White;
                        }
                    }
                    else
                    {

                        txtDiaVenda.Text = "";
                        txtDiaVenda.Focus();
                        txtDiaVenda.Background = Brushes.Red;
                    }
                }

            }
            catch (Exception)
            {

                txtDiaVenda.Background = Brushes.Red;
            };
        }

        private void txtDiaVenda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            naoCalc = false;

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtMesVenda_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtMesVenda.Focus() == true)
                {
                    if (txtMesVenda.Text.Length > 0)
                    {
                        int mesInt = Convert.ToInt16(txtMesVenda.Text);

                        if (txtMesVenda.Text != "00" && mesInt < 13)
                        {
                            if (txtMesVenda.Text.Length == 2)
                            {
                                txtAnoVenda.Focus();
                                txtAnoVenda.SelectAll();
                                txtMesVenda.Background = Brushes.White;
                            }
                            else
                                txtMesVenda.Background = Brushes.Red;
                        }
                        else
                        {

                            txtMesVenda.Focus();

                            txtMesVenda.SelectAll();



                            txtMesVenda.Background = Brushes.Red;
                        }

                        if (naoCalc == false)
                            CalcularDataVenda();
                    }
                }
            }
            catch (Exception)
            {

                txtMesVenda.Background = Brushes.Red;
            };
        }

        private void txtMesVenda_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMesVenda.Text.Length > 0)
                {
                    txtMesVenda.Text = string.Format("{0:00}", Convert.ToInt16(txtMesVenda.Text));

                    int mesInt = Convert.ToInt16(txtMesVenda.Text);

                    if (mesInt > 0 && mesInt < 13)
                    {
                        if (txtMesVenda.Text.Length == 2)
                        {
                            txtAnoVenda.Focus();
                            txtAnoVenda.SelectAll();
                            txtMesVenda.Background = Brushes.White;
                        }
                    }
                    else
                    {

                        txtMesVenda.Text = "";
                        txtMesVenda.Focus();
                        txtMesVenda.Background = Brushes.Red;
                    }


                }

            }
            catch (Exception)
            {

                txtMesVenda.Background = Brushes.Red;
            };
        }

        private void txtMesVenda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            naoCalc = false;

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void txtAnoVenda_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtAnoVenda.Text.Length == 4)
            {
                try
                {

                    int anoInt = Convert.ToInt16(txtAnoVenda.Text);

                    if (anoInt > 1900 && anoInt <= DateTime.Now.Date.Year)
                    {
                        txtAnoVenda.Background = Brushes.White;
                    }
                    else
                    {
                        txtAnoVenda.Background = Brushes.Red;
                        return;
                    }

                    if (naoCalc == false)
                    {

                        CalcularDataVenda();
                    }

                }
                catch (Exception)
                {

                };
            }
        }

        private void txtAnoVenda_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAnoVenda.Text.Length > 0)
                txtAnoVenda.Text = string.Format("{0:0000}", Convert.ToInt16(txtAnoVenda.Text));
        }

        private void txtAnoVenda_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            naoCalc = false;

            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);


            TabEnter(sender, e);
        }

        private void dpDataRegistro_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dpDataRegistro.SelectedDate != null)
            {
                naoCalc = true;

                txtDiaRegistro.Text = string.Format("{0:00}", dpDataRegistro.SelectedDate.Value.Day);
                txtMesRegistro.Text = string.Format("{0:00}", dpDataRegistro.SelectedDate.Value.Month);
                txtAnoRegistro.Text = string.Format("{0:0000}", dpDataRegistro.SelectedDate.Value.Year);
            }
        }

        private void dpDataVenda_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDataVenda.SelectedDate != null)
            {
                naoCalc = true;

                txtDiaVenda.Text = string.Format("{0:00}", dpDataVenda.SelectedDate.Value.Day);
                txtMesVenda.Text = string.Format("{0:00}", dpDataVenda.SelectedDate.Value.Month);
                txtAnoVenda.Text = string.Format("{0:0000}", dpDataVenda.SelectedDate.Value.Year);
            }
        }

        private void btnImagens_Click(object sender, RoutedEventArgs e)
        {
            var visualizar = new WinVisualizarDigitalizarRgi(itemSelecionado, this, _usuario);
            visualizar.Owner = this;
            visualizar.ShowDialog();

        }

        private void checkTodosDisponiveis_Checked(object sender, RoutedEventArgs e)
        {
            MarcarTodosCheckesDisponiveis();
        }


        private void MarcarTodosCheckesDisponiveis()
        {
            if (listaIndiceRegistro == null)
                return;

            foreach (var item in listaIndiceRegistro)
            {
                item.Enviado = true;
                dataGrid1.Items.Refresh();
            }

        }

        private void DesmarcarTodosCheckesDisponiveis()
        {
            if (listaIndiceRegistro == null)
                return;

            foreach (var item in listaIndiceRegistro)
            {
                item.Enviado = false;
                dataGrid1.Items.Refresh();
            }

        }

        private void checkTodosDisponiveis_Unchecked(object sender, RoutedEventArgs e)
        {
            DesmarcarTodosCheckesDisponiveis();
        }

        private void checkedUmDisponivel_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkedUmDisponivel_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void btnLoteamentos_Click(object sender, RoutedEventArgs e)
        {
            var loteamento = new WinCadLoteamentos();
            loteamento.Owner = this;
            loteamento.ShowDialog();
        }
    }
}
