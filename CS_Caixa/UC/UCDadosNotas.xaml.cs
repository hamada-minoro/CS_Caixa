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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS_Caixa.UC
{
    /// <summary>
    /// Interação lógica para UCDadosNotas.xam
    /// </summary>
    public partial class UCDadosNotas : UserControl
    {
        public UCDadosNotas()
        {
            InitializeComponent();
        }


        private void DigitarSomenteNumeros(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void DigitarSomenteLetras(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 44 && key <= 69 || key == 2 || key == 3);
        }

        private void txtFlsInicial_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteNumeros(sender, e);
        }

        private void txtFlsFinal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteNumeros(sender, e);
        }

        private void txtAto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteNumeros(sender, e);
        }

        private void txtLetraSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteLetras(sender, e);
        }

        private void txtLetraSelo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLetraSelo.Text.Length == 4)
            {
                txtNumeroSelo.IsEnabled = true;
                txtNumeroSelo.Focus();
            }
            else
            {
                txtNumeroSelo.IsEnabled = false;
                txtNumeroSelo.Text = "";
            }
        }

        private void txtNumeroSelo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteNumeros(sender, e);
        }

        private void txtNumeroSelo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumeroSelo.Text != "")
                txtNumeroSelo.Text = string.Format("{0:00000}", Convert.ToInt32(txtNumeroSelo.Text));
        }

        private void txtRequisicao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DigitarSomenteNumeros(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ucDadosNotas_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
