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
    /// Interaction logic for WinReciboNotas.xaml
    /// </summary>
    public partial class WinReciboNotas : Window
    {
        WinDigitarAtoNotas _winDigitarAtoNotas;

        public WinReciboNotas(WinDigitarAtoNotas winDigitarAtoNotas)
        {
            _winDigitarAtoNotas = winDigitarAtoNotas;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblNumeroRecibo.Content = string.Format("Recibo Nº: {0}", _winDigitarAtoNotas.Recibo.Recibo);

            if (_winDigitarAtoNotas.partes != null)
            {

                txtNomeApresentante.Text = _winDigitarAtoNotas.partes.Nome;
                rbCpfApresentante.IsChecked = (_winDigitarAtoNotas.partes.Cpf.Length == 11 ? true : false);
                rbCnpjApresentante.IsChecked = (_winDigitarAtoNotas.partes.Cpf.Length == 14 ? true : false);
                txtCpfApresentante.Text = _winDigitarAtoNotas.partes.Cpf;
                txtEndereçoApresentante.Text = _winDigitarAtoNotas.partes.Endereco;
                txtTelefone.Text = _winDigitarAtoNotas.partes.Telefone;
                txtCelular.Text = _winDigitarAtoNotas.partes.Celular;
                txtEmailApresentante.Text = _winDigitarAtoNotas.partes.Email;
                txtNomeOutorgado.Text = _winDigitarAtoNotas.partes.Outorgado;
                txtCpfOutorgado.Text = _winDigitarAtoNotas.partes.CpfOutorgado;
                rbCpfOutorgado.IsChecked = (_winDigitarAtoNotas.partes.CpfOutorgado.Length == 11 ? true : false);
                rbCnpjOutorgado.IsChecked = (_winDigitarAtoNotas.partes.CpfOutorgado.Length == 14 ? true : false);

                datePickerDataRecibo.SelectedDate = _winDigitarAtoNotas.Recibo.Data;
                datePickerDataEntrega.SelectedDate = _winDigitarAtoNotas.Recibo.DataEntrega;
            }
            else
            {
                datePickerDataRecibo.SelectedDate = _winDigitarAtoNotas.datePickerDataAto.SelectedDate.Value;
                datePickerDataEntrega.SelectedDate = _winDigitarAtoNotas.datePickerDataAto.SelectedDate.Value;
            }
            txtNomeApresentante.Focus();
        }

        private void btnRecibo_Click(object sender, RoutedEventArgs e)
        {
            if (txtNomeApresentante.Text.Trim() == "" || txtCpfApresentante.Text.Trim() == "")
            {
                MessageBox.Show("Os campos Nome do Apresentante e CPF do Apresentante são obrigatórios.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            if (_winDigitarAtoNotas.partes == null)
                _winDigitarAtoNotas.partes = new Parte();

            _winDigitarAtoNotas.partes.Nome = txtNomeApresentante.Text;
            _winDigitarAtoNotas.partes.Cpf = txtCpfApresentante.Text;
            _winDigitarAtoNotas.partes.Endereco = txtEndereçoApresentante.Text;
            _winDigitarAtoNotas.partes.Telefone = txtTelefone.Text;
            _winDigitarAtoNotas.partes.Celular = txtCelular.Text;
            _winDigitarAtoNotas.partes.Email = txtEmailApresentante.Text;
            _winDigitarAtoNotas.partes.Outorgado = txtNomeOutorgado.Text;
            _winDigitarAtoNotas.partes.CpfOutorgado = txtCpfOutorgado.Text;

           
            _winDigitarAtoNotas.Recibo.Data = datePickerDataRecibo.SelectedDate;
            _winDigitarAtoNotas.Recibo.DataEntrega = datePickerDataEntrega.SelectedDate;

            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
           
        }

        private void txtCpfApresentante_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtTelefone_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtCelular_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtCpfOutorgado_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void SelecionarConteudoTXT(TextBox txt)
        {
            txt.SelectAll();
        }

        private void txtNomeApresentante_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtNomeApresentante);
        }

        private void txtCpfApresentante_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtCpfApresentante);
        }

        private void txtEndereçoApresentante_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtEndereçoApresentante);
        }

        private void txtEmailApresentante_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtEmailApresentante);
        }

        private void txtTelefone_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtTelefone);
        }

        private void txtCelular_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtCelular);
        }

        private void txtNomeOutorgado_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtNomeOutorgado);
        }

        private void txtCpfOutorgado_GotFocus(object sender, RoutedEventArgs e)
        {
            SelecionarConteudoTXT(txtCpfOutorgado);
        }

        private void btnCopiarApresentante_Click(object sender, RoutedEventArgs e)
        {
            
            txtNomeOutorgado.Text = txtNomeApresentante.Text;
            txtCpfOutorgado.Text = txtCpfApresentante.Text;
            rbCpfOutorgado.IsChecked = rbCpfApresentante.IsChecked.Value;
            rbCnpjOutorgado.IsChecked = rbCnpjApresentante.IsChecked.Value;
        }

      

        private void rbCpfApresentante_Checked(object sender, RoutedEventArgs e)
        {
            
            txtCpfApresentante.MaxLength = 11;

            if (txtCpfApresentante.Text.Length > 11)
                txtCpfApresentante.Text = txtCpfApresentante.Text.Substring(0, 11);

            if (ValidaCpfCnpj.ValidaCPF(txtCpfApresentante.Text))
            {
                txtCpfApresentante.Background = Brushes.White;
            }
            else
            {
                txtCpfApresentante.Background = Brushes.OrangeRed;
            }
           
        }

        private void rbCnpjApresentante_Checked(object sender, RoutedEventArgs e)
        {
            txtCpfApresentante.MaxLength = 14;

            if (ValidaCpfCnpj.ValidaCNPJ(txtCpfApresentante.Text))
            {
                txtCpfApresentante.Background = Brushes.White;
            }
            else
            {
                txtCpfApresentante.Background = Brushes.OrangeRed;
            }
           
        }

        private void rbCpfOutorgado_Checked(object sender, RoutedEventArgs e)
        {
            txtCpfOutorgado.MaxLength = 11;

            txtCpfOutorgado.MaxLength = 11;

            if (txtCpfOutorgado.Text.Length > 11)
                txtCpfOutorgado.Text = txtCpfOutorgado.Text.Substring(0, 11);

            if (ValidaCpfCnpj.ValidaCPF(txtCpfOutorgado.Text))
            {
                txtCpfOutorgado.Background = Brushes.White;
            }
            else
            {
                txtCpfOutorgado.Background = Brushes.OrangeRed;
            }
          
        }

        private void rbCnpjOutorgado_Checked(object sender, RoutedEventArgs e)
        {            
            txtCpfOutorgado.MaxLength = 14;

            txtCpfOutorgado.MaxLength = 14;

            if (ValidaCpfCnpj.ValidaCNPJ(txtCpfOutorgado.Text))
            {
                txtCpfOutorgado.Background = Brushes.White;
            }
            else
            {
                txtCpfOutorgado.Background = Brushes.OrangeRed;
            }

        }

        private void txtCpfApresentante_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rbCpfApresentante.IsChecked == true)
            {
                if (txtCpfApresentante.Text.Length == 11)
                if (ValidaCpfCnpj.ValidaCPF(txtCpfApresentante.Text))
                {
                    txtCpfApresentante.Background = Brushes.White;
                }
                else
                {
                    txtCpfApresentante.Background = Brushes.OrangeRed;
                }
            }
            else
            {
                if (txtCpfApresentante.Text.Length == 14)
                if (ValidaCpfCnpj.ValidaCNPJ(txtCpfApresentante.Text))
                {
                    txtCpfApresentante.Background = Brushes.White;
                }
                else
                {
                    txtCpfApresentante.Background = Brushes.OrangeRed;
                }
            }
        }

        private void txtCpfOutorgado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rbCpfOutorgado.IsChecked == true)
            {
                if(txtCpfOutorgado.Text.Length == 11)
                if (ValidaCpfCnpj.ValidaCPF(txtCpfOutorgado.Text))
                {
                    txtCpfOutorgado.Background = Brushes.White;
                }
                else
                {
                    txtCpfOutorgado.Background = Brushes.OrangeRed;
                }
            }
            else
            {
                if (txtCpfOutorgado.Text.Length == 14)
                if (ValidaCpfCnpj.ValidaCNPJ(txtCpfOutorgado.Text))
                {
                    txtCpfOutorgado.Background = Brushes.White;
                }
                else
                {
                    txtCpfOutorgado.Background = Brushes.OrangeRed;
                }
            }
        }

        private void txtCpfApresentante_LostFocus(object sender, RoutedEventArgs e)
        {
            if (rbCpfApresentante.IsChecked == true)
            {
                if (ValidaCpfCnpj.ValidaCPF(txtCpfApresentante.Text))
                {
                    txtCpfApresentante.Background = Brushes.White;
                }
                else
                {
                    txtCpfApresentante.Background = Brushes.OrangeRed;
                }
            }
            else
            {
                if (ValidaCpfCnpj.ValidaCNPJ(txtCpfApresentante.Text))
                {
                    txtCpfApresentante.Background = Brushes.White;
                }
                else
                {
                    txtCpfApresentante.Background = Brushes.OrangeRed;
                }
            }
        }

        private void txtCpfOutorgado_LostFocus(object sender, RoutedEventArgs e)
        {
            if (rbCpfOutorgado.IsChecked == true)
            {
                if (ValidaCpfCnpj.ValidaCPF(txtCpfOutorgado.Text))
                {
                    txtCpfOutorgado.Background = Brushes.White;
                }
                else
                {
                    txtCpfOutorgado.Background = Brushes.OrangeRed;
                }
            }
            else
            {
                if (ValidaCpfCnpj.ValidaCNPJ(txtCpfOutorgado.Text))
                {
                    txtCpfOutorgado.Background = Brushes.White;
                }
                else
                {
                    txtCpfOutorgado.Background = Brushes.OrangeRed;
                }
            }
        }
    }
}
