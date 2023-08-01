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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAlterarControlePagamento.xaml
    /// </summary>
    public partial class WinAlterarControlePagamento : Window
    {
        public ControlePagamentoCredito credito;
        public ControlePagamentoDebito debito;
        string tipo;
        ClassControlePagamento classControlePagamento = new ClassControlePagamento();
        WinControlePagamentoControle controle;


        public WinAlterarControlePagamento(WinControlePagamentoControle controle, ControlePagamentoCredito credito, string tipo)
        {
            this.controle = controle;
            this.credito = credito;
            this.tipo = tipo;

            InitializeComponent();
        }

        public WinAlterarControlePagamento(WinControlePagamentoControle controle, ControlePagamentoDebito debito, string tipo)
        {
            this.controle = controle;
            this.debito = debito;
            this.tipo = tipo;

            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (tipo == "credito")
            {
                txtDescricao.Text = credito.Descricao;

                datePickerData.SelectedDate = credito.Data;

                txtValor.Text = string.Format("{0:n2}", credito.Valor);
            }
            else
            {
                txtDescricao.Text = debito.Descricao;

                datePickerData.SelectedDate = debito.Data;

                txtValor.Text = string.Format("{0:n2}", debito.Valor);
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (tipo == "credito")
            {
                try
                {
                    ControlePagamentoCredito creditoSalvar = new ControlePagamentoCredito();

                    if (datePickerData.SelectedDate != null)
                        creditoSalvar.Data = datePickerData.SelectedDate.Value;
                    else
                    {
                        MessageBox.Show("Informe uma data válida.", "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        datePickerData.Focus();
                        return;
                    }

                    creditoSalvar.Descricao = txtDescricao.Text = txtDescricao.Text.Trim();

                    if (creditoSalvar.Descricao == "")
                    {
                        MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtDescricao.Focus();
                        return;
                    }

                    creditoSalvar.Valor = Convert.ToDecimal(txtValor.Text);

                    if (creditoSalvar.Valor == 0)
                    {
                        MessageBox.Show("Informe o valor.", "Valor", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtValor.Focus();
                        return;
                    }

                    creditoSalvar.Id = credito.Id;

                    classControlePagamento.SalvarCredito(creditoSalvar, "alterar");

                    controle.itemSelecionadoCredito = creditoSalvar;


                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    ControlePagamentoDebito debitoSalvar = new ControlePagamentoDebito();

                    if (datePickerData.SelectedDate != null)
                        debitoSalvar.Data = datePickerData.SelectedDate.Value;
                    else
                    {
                        MessageBox.Show("Informe uma data válida.", "Data Inválida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        datePickerData.Focus();
                        return;
                    }


                    debitoSalvar.Descricao = txtDescricao.Text = txtDescricao.Text.Trim();

                    if (debitoSalvar.Descricao == "")
                    {
                        MessageBox.Show("Informe a Descrição.", "Descrição", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtDescricao.Focus();
                        return;
                    }


                    debitoSalvar.Valor = Convert.ToDecimal(txtValor.Text);

                    if (debitoSalvar.Valor == 0)
                    {
                        MessageBox.Show("Informe o valor.", "Valor", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtValor.Focus();
                        return;
                    }

                    debitoSalvar.Id = debito.Id;

                    classControlePagamento.SalvarDebito(debitoSalvar, "alterar", false);


                    controle.itemSelecionadoDebito = debitoSalvar;

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        
        private void stackPanel2_PreviewKeyDown(object sender, KeyEventArgs e)
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
                        e.Handled = !(key == 2 || key == 3);
                    }
                    else
                    {
                        e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
                    }
                }
                else
                {
                    e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 88);
                }
            }
            else
            {
                e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
            }
        }

        private void txtValor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text != "")
            {
                try
                {
                    txtValor.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValor.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtValor.Text = "0,00";
            }
        }

        private void txtValor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValor.Text == "0,00")
            {
                txtValor.Text = "";
            }
        }
    }
}
