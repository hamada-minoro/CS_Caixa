using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.Repositorios;
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
    /// Lógica interna para ClienteCadastro.xaml
    /// </summary>
    public partial class ClienteCadastro : Window
    {
        RepositorioCadastroCliente _IAppServicoCadastroCliente = new RepositorioCadastroCliente();

        CadastroCliente cadCli;

        ConfirmarChamadaSenha _confirmaChamada;

        public ClienteCadastro(ConfirmarChamadaSenha confirmaChamada)
        {
            _confirmaChamada = confirmaChamada;

            InitializeComponent();
        }

        private void txtTelefone_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtCpfCnpj.Focus();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                SalvarCadastroCliente();


            PassarDeUmCoponenteParaOutro(sender, e);
        }

        private void txtCpfCnpj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteNumeros(sender, e);
        }

        private void DigitarSomenteNumeros(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key == 23 || key == 25 || key == 32 || key == 90);
        }


        private void txtCpfCnpj_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidarCpfCnpj();
        }

        private void ValidarCpfCnpj()
        {
            imgValidaCpfCnpj.Visibility = Visibility.Hidden;
            txtCpfCnpj.Background = Brushes.Red;

            if (txtCpfCnpj.Text.Length == 11)
            {
                bool cpfValido = ValidaCpfCnpj.ValidaCPF(txtCpfCnpj.Text);

                if (cpfValido == true)
                {
                    imgValidaCpfCnpj.Visibility = Visibility.Visible;
                    txtCpfCnpj.Background = Brushes.White;
                    rbFisica.IsChecked = true;
                    rbMasculino.IsEnabled = true;
                    rbFeminino.IsEnabled = true;
                    dpDataNascimento.SelectedDate = null;
                    dpDataNascimento.IsEnabled = true;
                    txtIdentidade.Text = "";
                    txtIdentidade.IsEnabled = true;
                    txtNomeParte.Focus();


                    cadCli = _IAppServicoCadastroCliente.ObterClientesPorCpfCnpj(txtCpfCnpj.Text);

                    if (cadCli != null)
                        PreencherCamposClienteExistente();
                }
            }
            if (txtCpfCnpj.Text.Length == 14)
            {
                bool cnpjValido = ValidaCpfCnpj.ValidaCNPJ(txtCpfCnpj.Text);

                if (cnpjValido == true)
                {
                    imgValidaCpfCnpj.Visibility = Visibility.Visible;
                    txtCpfCnpj.Background = Brushes.White;
                    rbJuridica.IsChecked = true;
                    rbMasculino.IsChecked = false;
                    rbFeminino.IsChecked = false;
                    rbMasculino.IsEnabled = false;
                    rbFeminino.IsEnabled = false;
                    dpDataNascimento.SelectedDate = null;
                    dpDataNascimento.IsEnabled = false;
                    txtIdentidade.Text = "";
                    txtIdentidade.IsEnabled = false;
                    txtNomeParte.Focus();


                    cadCli = _IAppServicoCadastroCliente.ObterClientesPorCpfCnpj(txtCpfCnpj.Text);

                    if (cadCli != null)
                        PreencherCamposClienteExistente();
                }
            }
        }


        private void PassarDeUmCoponenteParaOutro(object sender, KeyEventArgs e)
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




        private void PreencherCamposClienteExistente()
        {
            txtNomeParte.Text = cadCli.Nome;

            if (cadCli.TipoPessoa == "F")
                rbFisica.IsChecked = true;

            if (cadCli.TipoPessoa == "J")
                rbJuridica.IsChecked = true;

            if (cadCli.Sexo == "F")
                rbFeminino.IsChecked = true;

            if (cadCli.Sexo == "M")
                rbMasculino.IsChecked = true;

            txtEndereco.Text = cadCli.Endereco;

            txtTelefone.Text = cadCli.Telefone;

            if (cadCli.DataNascimento.ToString() != "01/01/0001 00:00:00")
                dpDataNascimento.SelectedDate = cadCli.DataNascimento;

            txtIdentidade.Text = cadCli.RG;

            txtEmail.Text = cadCli.Email;

            txtNomeParte.SelectAll();
        }


        private void txtNomeParte_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNomeParte_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void rbMasculino_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbMasculino_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void rbFeminino_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbFeminino_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtEndereco_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dpDataNascimento_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dpDataNascimento_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtIdentidade_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtIdentidade_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtTelefone_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgSalvar.Width = 80;
            imgSalvar.Height = 80;

            SalvarCadastroCliente();
        }

        private void rbFisica_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbFisica_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void rbJuridica_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbJuridica_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            imgSalvar.Width = 60;
            imgSalvar.Height = 60;

        }

        private void imgSalvar_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void imgSalvar_MouseLeave(object sender, MouseEventArgs e)
        {
            imgSalvar.Width = 80;
            imgSalvar.Height = 80;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgSalvar.Width = 80;
            imgSalvar.Height = 80;
        }





        private void SalvarCadastroCliente()
        {
            try
            {

                if (cadCli == null)
                    cadCli = new CadastroCliente();



                if (txtCpfCnpj.Text.Length > 0)
                    cadCli.CPF_CNPJ = txtCpfCnpj.Text;
                else
                {
                    MessageBox.Show("Preencha o campo CPF/CNPJ");
                    return;
                }

                if (txtNomeParte.Text.Trim().Length > 0)
                    cadCli.Nome = txtNomeParte.Text.Trim();
                else
                {
                    MessageBox.Show("Preencha o campo Nome");
                    return;
                }

                if (rbFisica.IsChecked == true)
                    cadCli.TipoPessoa = "F";

                if (rbJuridica.IsChecked == true)
                    cadCli.TipoPessoa = "J";

                if (rbMasculino.IsChecked == true)
                    cadCli.Sexo = "M";

                if (rbFeminino.IsChecked == true)
                    cadCli.Sexo = "F";

                if (txtEndereco.Text.Trim().Length > 0)
                    cadCli.Endereco = txtEndereco.Text.Trim();

                if (txtTelefone.Text.Trim().Length > 0)
                    cadCli.Telefone = txtTelefone.Text.Trim();


                if (dpDataNascimento.SelectedDate != null)
                    cadCli.DataNascimento = dpDataNascimento.SelectedDate.Value;

                if (cadCli.CadastroClienteId == 0)
                    cadCli.DataCadastro = DateTime.Now.Date;

                cadCli.DataUltimaAtualizacao = DateTime.Now.Date.ToShortDateString();

                if (txtIdentidade.Text.Trim().Length > 0)
                    cadCli.RG = txtIdentidade.Text.Trim();

                if (txtEmail.Text.Trim().Length > 0)
                    cadCli.Email = txtEmail.Text.Trim();

                if (cadCli.CadastroClienteId == 0)
                    _IAppServicoCadastroCliente.Adicionar(cadCli);
                else
                    _IAppServicoCadastroCliente.Update(cadCli);



                _confirmaChamada._chamarSenha.CadastroCliente_Id = cadCli.CadastroClienteId;

                _confirmaChamada.AtualizarStatusSenha(_confirmaChamada._chamarSenha, "FINALIZADA");


                MessageBox.Show("Cadastro salvo com sucesso!. ", "Salvo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salva o cadastro. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}