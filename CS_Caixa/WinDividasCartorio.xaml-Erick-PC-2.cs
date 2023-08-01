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
    /// Lógica interna para WinDividasCartorio.xaml
    /// </summary>
    public partial class WinDividasCartorio : Window
    {
        ClassUsuario classUsuarios = new ClassUsuario();
        List<Usuario> usuarios = new List<Usuario>();
        Usuario _usuario;
        List<Parcela> parcelas = new List<Parcela>();
        List<Divida> dividaUsuario = new List<Divida>();
        string status = string.Empty;
        ClassDivida classDivida = new ClassDivida();
        Divida dividaSelecionada = new Divida();
        Parcela parcelaSelecionada = new Parcela();


        public WinDividasCartorio(Usuario usuario)
        {
            _usuario = usuario;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitulo.Content = "Dívidas e Descontos";
            CarregamentoInicial();
            CarregarComboBoxUsuarios();
            
        }

        private void CarregamentoInicial()
        {
            
                txtId.Text = "0";
                cmbTipoDivida.SelectedIndex = -1;
                txtValor.Text = "0,00";
                txtQtdParcelas.Text = "1";
                cmbDiaPagamento.SelectedIndex = 0;
                txtDescricao.Text = "";
                txtData1Parcela.Text = "";
                txtDataUltParcela.Text = "";
                txtQtdPaga.Text = "";
                txtValorPago.Text = "";
                checkBoxPago.IsChecked = false;
                               
                parcelas = null;
                parcelas = new List<Parcela>();
                dataGridParcelas.ItemsSource = parcelas;
                dataGridParcelas.Items.Refresh();
            

            if (_usuario.Adm == true)
            {
                cmbFuncionario.IsEnabled = true;
                btnNovo.IsEnabled = true;
            }

            gridCalcular.IsEnabled = false;
            dataGridDividasPendentes.IsEnabled = true;
            dataGridDividasQuitadas.IsEnabled = true;
            btnSalvar.IsEnabled = false;
            btnCancelar.IsEnabled = false;

            tabControl1.IsEnabled = true;
        }

        private void CarregarComboBoxUsuarios()
        {
            usuarios = classUsuarios.ListaUsuarios();
            cmbFuncionario.ItemsSource = usuarios;
            cmbFuncionario.DisplayMemberPath = "NomeUsu";

            if (_usuario.Adm == false)
            {

                var usu = usuarios.SingleOrDefault(p => p.Id_Usuario == _usuario.Id_Usuario);

                cmbFuncionario.SelectedItem = usu;
                cmbFuncionario.IsEnabled = false;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void cmbFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarregarDataGridsDividas((Usuario)cmbFuncionario.SelectedItem);
        }

        private void CarregarDataGridsDividas(Usuario usuario)
        {
            classDivida = new ClassDivida();

            dividaUsuario = classDivida.ObterDividasPorUsuario(usuario.Id_Usuario);

            dataGridDividasPendentes.ItemsSource = dividaUsuario.Where(p => p.Divida_Paga == false).OrderByDescending(p => p.Id_Divida);
            dataGridDividasPendentes.Items.Refresh();
            if (dataGridDividasPendentes.Items.Count > 0)
                dataGridDividasPendentes.SelectedIndex = 0;


            dataGridDividasQuitadas.ItemsSource = dividaUsuario.Where(p => p.Divida_Paga == true).OrderByDescending(p => p.Id_Divida); ;
            dataGridDividasQuitadas.Items.Refresh();
            if (dataGridDividasQuitadas.Items.Count > 0)
                dataGridDividasQuitadas.SelectedIndex = 0;
        }

        private void ckbTodos_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ckbTodos_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTipoDivida.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Tipo Dívida", "Tipo Dívida", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbTipoDivida.Focus();
                return;
            }

            if (txtValor.Text == "" || txtValor.Text == "0,00")
            {
                MessageBox.Show("Informe o Valor", "Valor", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtValor.Focus();
                txtValor.SelectAll();
                return;
            }


            if (txtQtdParcelas.Text == "0" || txtQtdParcelas.Text == "")
            {
                MessageBox.Show("Informe a Quantidade de Parcelas", "Quantidade de Parcelas", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQtdParcelas.Focus();
                txtQtdParcelas.SelectAll();
                return;
            }



            if (cmbDiaPagamento.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Dia de Pagamento", "Dia de Pagamento", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbDiaPagamento.Focus();
                return;
            }

            CalcularParcelas();

            Divida divida = new Divida();

            if (status == "novo")
                divida.Data = DateTime.Now.Date;


            if (txtDataUltParcela.Text != "")
                divida.Data_Fim_Parcela = Convert.ToDateTime(txtDataUltParcela.Text);

            if (txtData1Parcela.Text != "")
                divida.Data_Inicio_Parcela = Convert.ToDateTime(txtData1Parcela.Text);

            divida.Descricao = txtDescricao.Text;
            divida.Dia_Pagamento = cmbDiaPagamento.SelectedIndex + 1;
            divida.Divida_Paga = checkBoxPago.IsChecked;
            divida.Id_Usuario = ((Usuario)cmbFuncionario.SelectedItem).Id_Usuario;
            divida.Qtd_Parcelas = Convert.ToInt16(txtQtdParcelas.Text);
            if (txtQtdPaga.Text != "")
                divida.Qtd_Parcelas_Pagas = Convert.ToInt16(txtQtdPaga.Text);
            divida.Tipo_Divida = cmbTipoDivida.Text;
            divida.Valor_Divida = Convert.ToDecimal(txtValor.Text);
            if (txtValorPago.Text != "")
                divida.Valor_Pago = Convert.ToDecimal(txtValorPago.Text);

            classDivida = new ClassDivida();

            int id = classDivida.SalvarDivida(divida);

            int salvou = 0;

            if (id > -1)
            {
                classDivida = new ClassDivida();
                salvou = classDivida.SalvarParcelas(parcelas, id);
            }
            else
                MessageBox.Show("Não foi possível salvar. Ocorreu um erro inesperado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

            if (salvou == 0)
            {
                classDivida = new ClassDivida();
                classDivida.ExcluirDivida(id);
                MessageBox.Show("Não foi possível salvar. Ocorreu um erro inesperado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CarregamentoInicial();

            tabControl1.SelectedIndex = 0;

            dividaSelecionada = divida;

            CarregarDataGridsDividas((Usuario)cmbFuncionario.SelectedItem);

            if (tabControl1.SelectedIndex == 0)
            {
                dataGridDividasPendentes.SelectedItem = dividaSelecionada;
                dataGridDividasPendentes.Items.Refresh();
            }
            else
            {
                dataGridDividasQuitadas.SelectedItem = dividaSelecionada;
                dataGridDividasQuitadas.Items.Refresh();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            CarregamentoInicial();

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    CarregarParcelasPendentes();
                    break;

                case 1:
                    CarregarParcelasQuitadas();
                    break;


                default:
                    break;
            }
        }



        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            CarregamentoInicial();
            PrepararParaNovo();
            VisivelHidden(true);
            status = "novo";
        }


        private void PrepararParaNovo()
        {

            tabControl1.IsEnabled = false;
            cmbFuncionario.IsEnabled = false;
            gridCalcular.IsEnabled = true;
            btnCancelar.IsEnabled = true;
            btnNovo.IsEnabled = false;
            btnExcluir.IsEnabled = false;

        }


        private void CarregarParcelas()
        {
            classDivida = new ClassDivida();


            parcelas = classDivida.ObterParcelasPorIdDivida(dividaSelecionada.Id_Divida);

            dataGridParcelas.ItemsSource = parcelas;
            dataGridParcelas.Items.Refresh();
        }

        private void VisivelHidden(bool visivel)
        {
            if (visivel == true)
            {
                labelData1Parcela.Visibility = Visibility.Visible;
                datePickerData.Visibility = Visibility.Visible;
            }
            else
            {
                labelData1Parcela.Visibility = Visibility.Hidden;
                datePickerData.Visibility = Visibility.Hidden;
            }
        }

        private void CarregarDivida()
        {
            txtId.Text = dividaSelecionada.Id_Divida.ToString();
            cmbTipoDivida.Text = dividaSelecionada.Tipo_Divida;
            txtValor.Text = string.Format("{0:n2}", dividaSelecionada.Valor_Divida);
            txtQtdParcelas.Text = dividaSelecionada.Qtd_Parcelas.ToString();
            cmbDiaPagamento.Text = dividaSelecionada.Dia_Pagamento.ToString();
            txtDescricao.Text = dividaSelecionada.Descricao;
            checkBoxPago.IsChecked = dividaSelecionada.Divida_Paga.Value;
            if (dividaSelecionada.Data_Inicio_Parcela != null)
            {
                txtData1Parcela.Text = dividaSelecionada.Data_Inicio_Parcela.Value.ToShortDateString();
                VisivelHidden(true);
                datePickerData.SelectedDate = dividaSelecionada.Data_Inicio_Parcela.Value;
            }
            else
            {
                txtData1Parcela.Text = "";
                VisivelHidden(true);
            }
            if (dividaSelecionada.Data_Fim_Parcela != null)
                txtDataUltParcela.Text = dividaSelecionada.Data_Fim_Parcela.Value.ToShortDateString();
            else
                txtDataUltParcela.Text = "";

            if (dividaSelecionada.Qtd_Parcelas_Pagas != null)
                txtQtdPaga.Text = dividaSelecionada.Qtd_Parcelas_Pagas.ToString();
            else
                txtQtdPaga.Text = "";

            if (dividaSelecionada.Valor_Pago != null)
                txtValorPago.Text = string.Format("{0:n2}", dividaSelecionada.Valor_Pago);
            else
                txtValorPago.Text = "";
        }

        private void DescarregarDivida()
        {
            txtId.Text = "";
            cmbTipoDivida.SelectedIndex = -1;
            txtValor.Text = "0,00";
            txtQtdParcelas.Text = "0"; ;
            cmbDiaPagamento.SelectedIndex = -1;
            txtDescricao.Text = "";
            txtData1Parcela.Text = "";
            txtDataUltParcela.Text = "";
            txtQtdPaga.Text = "";
            txtValorPago.Text = "";
            VisivelHidden(false);
        }

        private void CarregarParcelasPendentes()
        {
            if (dataGridDividasPendentes.SelectedItem != null)
            {
                dividaSelecionada = (Divida)dataGridDividasPendentes.SelectedItem;
                CarregarDivida();
                CarregarParcelas();

                if (_usuario.Adm == true)
                    btnExcluir.IsEnabled = true;
            }
            else
            {
                btnExcluir.IsEnabled = false;
                DescarregarDivida();
                parcelas = null;
                parcelas = new List<Parcela>();
                dataGridParcelas.ItemsSource = parcelas;
                dataGridParcelas.Items.Refresh();
            }
        }

        private void dataGridDividasPendentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarregarParcelasPendentes();
        }


        private void CarregarParcelasQuitadas()
        {
            if (dataGridDividasQuitadas.SelectedItem != null)
            {
                dividaSelecionada = (Divida)dataGridDividasQuitadas.SelectedItem;
                CarregarDivida();
                CarregarParcelas();

                if (_usuario.Adm == true)
                    btnExcluir.IsEnabled = true;
            }
            else
            {
                btnExcluir.IsEnabled = false;
                DescarregarDivida();
                parcelas = null;
                parcelas = new List<Parcela>();
                dataGridParcelas.ItemsSource = parcelas;
                dataGridParcelas.Items.Refresh();
            }
        }

        private void dataGridDividasQuitadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CarregarParcelasQuitadas();
        }



        private void txtValor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text == "")
                txtValor.Text = "0,00";
            else
                txtValor.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValor.Text));

        }

        private void txtValor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (txtValor.Text.Length > 0)
            {
                if (txtValor.Text.Contains(","))
                {
                    int index = txtValor.Text.IndexOf(",");

                    if (txtValor.Text.Length == index + 3)
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

        private void txtQtdParcelas_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdParcelas.Text == "1" || txtQtdParcelas.Text == "0")
                txtQtdParcelas.Text = "";
            else
                txtQtdParcelas.SelectAll();
        }

        private void txtQtdParcelas_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdParcelas.Text == "" || txtQtdParcelas.Text == "0")
                txtQtdParcelas.Text = "1";
        }

        private void txtQtdParcelas_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtQtdParcelas.Text == "1" || txtQtdParcelas.Text == "0")
                txtQtdParcelas.Text = "";
            else
                txtQtdParcelas.SelectAll();
        }

        private void txtQtdParcelas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTipoDivida.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Tipo Dívida", "Tipo Dívida", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbTipoDivida.Focus();
                return;
            }

            if (txtValor.Text == "" || txtValor.Text == "0,00")
            {
                MessageBox.Show("Informe o Valor", "Valor", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtValor.Focus();
                txtValor.SelectAll();
                return;
            }


            if (txtQtdParcelas.Text == "0" || txtQtdParcelas.Text == "")
            {
                MessageBox.Show("Informe a Quantidade de Parcelas", "Quantidade de Parcelas", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQtdParcelas.Focus();
                txtQtdParcelas.SelectAll();
                return;
            }



            if (cmbDiaPagamento.SelectedIndex < 0)
            {
                MessageBox.Show("Informe o Dia de Pagamento", "Dia de Pagamento", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbDiaPagamento.Focus();
                return;
            }

            if (datePickerData.SelectedDate == null)
            {
                MessageBox.Show("Informe a data da 1º parcela.", "Data 1º Parcela", MessageBoxButton.OK, MessageBoxImage.Warning);
                datePickerData.Focus();
                return;
            }


            CalcularParcelas();

            btnSalvar.IsEnabled = true;
        }

        private void CalcularParcelas()
        {

            CalcularParcelasDatas();
            txtData1Parcela.Text = parcelas.FirstOrDefault().Data_Vencimento.ToShortDateString();
            txtDataUltParcela.Text = parcelas.LastOrDefault().Data_Vencimento.ToShortDateString();
            txtQtdPaga.Text = parcelas.Where(p => p.Pago == true).ToList().Count.ToString();
            txtValorPago.Text = string.Format("{0:n2}", parcelas.Where(p => p.Pago == true).Sum(p => p.Valor));
        }


        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (dividaSelecionada != null)
                if (MessageBox.Show("Deseja realmente excluir esta dívida?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    classDivida.ExcluirDivida(dividaSelecionada.Id_Divida);

                    CarregarDataGridsDividas((Usuario)cmbFuncionario.SelectedItem);
                }
        }



        private void txtValor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text == "0,00")
            {
                txtValor.Text = "";
            }
            else
            {
                txtValor.SelectAll();
            }
        }

        private void cmbDiaPagamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CalcularParcelasDatas()
        {
            if (cmbDiaPagamento.SelectedIndex > -1 && txtQtdParcelas.Text != "")
            {
                parcelas = null;
                parcelas = new List<Parcela>();

                var diaPagamento = Convert.ToInt16(cmbDiaPagamento.SelectedIndex + 1);

                var Data = datePickerData.SelectedDate.Value;

                var qtdParcelas = Convert.ToInt16(txtQtdParcelas.Text);

                var valorTotal = Convert.ToDecimal(txtValor.Text);

                var valorParcela = Convert.ToDecimal(string.Format("{0:n2}", valorTotal / qtdParcelas));

                for (int i = 0; i < qtdParcelas; i++)
                {
                    var parc = new Parcela();

                   

                    parc.Data_Emissao = DateTime.Now.Date;
                    if (i == 0)
                        parc.Data_Vencimento = Data;
                    else
                    {
                        Data = Data.AddDays(31);
                        parc.Data_Vencimento = Convert.ToDateTime(string.Format("{0}/{1}/{2}", diaPagamento, Data.Month, Data.Year));
                    }
                    parc.Id_Usuario = ((Usuario)cmbFuncionario.SelectedItem).Id_Usuario;
                    parc.Pago = false;
                    parc.Data_Pagamento = "";

                    if (i != qtdParcelas - 1)
                        parc.Valor = Convert.ToDecimal(string.Format("{0:n2}", valorParcela));
                    else
                    {

                        var totalParcelasMenosUm = Convert.ToDecimal(string.Format("{0:n2}", valorParcela * i));

                        parc.Valor = Convert.ToDecimal(string.Format("{0:n2}", valorTotal - totalParcelasMenosUm));
                    }
                    parcelas.Add(parc);
                }


                dataGridParcelas.ItemsSource = parcelas;
                dataGridParcelas.Items.Refresh();
            }
        }




        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    CarregarParcelasPendentes();
                    break;

                case 1:
                    CarregarParcelasQuitadas();
                    break;


                default:
                    break;
            }
        }

        private void txtValor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtQtdParcelas_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dataGridParcelas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridParcelas.SelectedItem != null)
                parcelaSelecionada = (Parcela)dataGridParcelas.SelectedItem;

        }

        private void dataGridParcelas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_usuario.Adm == true)
            {
                var parcela = (Parcela)dataGridParcelas.SelectedItem;

                if (parcela.Id_Divida == 0)
                    return;


                classDivida = new ClassDivida();

                if (parcela.Pago == false)
                    parcelaSelecionada = classDivida.PagarDatarParcela(parcela);
                else
                    parcelaSelecionada = classDivida.DespagarDatarParcela(parcela);

                CarregarParcelas();

                dividaSelecionada.Qtd_Parcelas_Pagas = parcelas.Where(p => p.Pago == true).Count();
                dividaSelecionada.Valor_Pago = parcelas.Where(p => p.Pago == true).Sum(p => p.Valor);

                if (dividaSelecionada.Qtd_Parcelas_Pagas == parcelas.Count())
                    dividaSelecionada.Divida_Paga = true;
                else
                    dividaSelecionada.Divida_Paga = false;

                txtQtdPaga.Text = dividaSelecionada.Qtd_Parcelas_Pagas.ToString();
                txtValorPago.Text = string.Format("{0:n2}", dividaSelecionada.Valor_Pago);
                checkBoxPago.IsChecked = dividaSelecionada.Divida_Paga;

                classDivida.SalvarDivida(dividaSelecionada);


                classDivida = new ClassDivida();

                dividaUsuario = classDivida.ObterDividasPorUsuario(((Usuario)cmbFuncionario.SelectedItem).Id_Usuario);

                dataGridDividasPendentes.ItemsSource = dividaUsuario.Where(p => p.Divida_Paga == false).OrderByDescending(p => p.Id_Divida);
                dataGridDividasPendentes.Items.Refresh();

                dataGridDividasQuitadas.ItemsSource = dividaUsuario.Where(p => p.Divida_Paga == true).OrderByDescending(p => p.Id_Divida); ;
                dataGridDividasQuitadas.Items.Refresh();




                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        CarregarParcelasPendentes();
                        var div = dividaUsuario.Where(p => p.Id_Divida == dividaSelecionada.Id_Divida).FirstOrDefault();

                        dataGridDividasPendentes.SelectedItem = div;
                        dataGridDividasPendentes.ScrollIntoView(div);
                        break;

                    case 1:
                        CarregarParcelasQuitadas();

                        var div2 = dividaUsuario.Where(p => p.Id_Divida == dividaSelecionada.Id_Divida).FirstOrDefault();

                        dataGridDividasQuitadas.SelectedItem = div2;
                        dataGridDividasQuitadas.ScrollIntoView(div2);
                        break;


                    default:
                        break;
                }
            }
        }

        private void gridCalcular_PreviewKeyDown(object sender, KeyEventArgs e)
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


    }
}
