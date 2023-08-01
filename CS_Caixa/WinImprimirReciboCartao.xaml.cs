using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Lógica interna para WinImprimirReciboCartao.xaml
    /// </summary>
    public partial class WinImprimirReciboCartao : Window
    {
        System.Windows.Forms.RichTextBox richTextBox = new System.Windows.Forms.RichTextBox();
        string texto = "";
        public WinImprimirReciboCartao()
        {
            InitializeComponent();
        }

        private void btnImprimirRecibo_Click(object sender, RoutedEventArgs e)
        {           
            ImprimirRecibo();

            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }


        private void CarregarListaDeImpressoras()
        {
            impressoraComboBox.Items.Clear();
            List<string> impressoras = new List<string>();

            foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                impressoras.Add(printer.ToString());

            impressoraComboBox.ItemsSource = impressoras;
            impressoraComboBox.SelectedItem = impressoras.Where(p => p.Contains("MP") || p.Contains("mp")).FirstOrDefault();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarListaDeImpressoras();

            
        }


        void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            var printDocument = sender as System.Drawing.Printing.PrintDocument;

            if (printDocument != null)
            {
                if (printDocument != null)
                {
                    using (var font = new System.Drawing.Font("Times New Roman", 9))
                    using (var brush = new SolidBrush(System.Drawing.Color.Black))
                    {
                        e.Graphics.DrawString(
                            texto,
                            font,
                            brush,
                            new RectangleF(0, 0, printDocument.DefaultPageSettings.PrintableArea.Width, printDocument.DefaultPageSettings.PrintableArea.Height));
                    }
                }
            }
        }


        private void AlterarDadosRecibo(string parametro, string dado)
        {
            System.Text.RegularExpressions.Regex regExpRecibo = new System.Text.RegularExpressions.Regex(parametro);
            foreach (System.Text.RegularExpressions.Match matchRecibo in regExpRecibo.Matches(richTextBox.Text))
            {
                richTextBox.Select(matchRecibo.Index, matchRecibo.Length);
                richTextBox.SelectedText = string.Format("{0}", dado);
            }
        }


        
        private void ImprimirRecibo()
        {

            try
            {

                string[] lines = System.IO.File.ReadAllLines(@"\\SERVIDOR\Arquivos Cartório\VAREJOTEF\cupom_viacliente.txt");

                foreach (string line in lines)
                {
                    texto = texto += line + "\n";
                }

                richTextBox.Text = texto;
                AlterarDadosRecibo("LOJISTA", "TITULAR");
                AlterarDadosRecibo(txtValorErrado.Text, txtValorCorreto.Text);

                texto = richTextBox.Text;

                using (var printDocument = new System.Drawing.Printing.PrintDocument())
                {
                    printDocument.PrintPage += printDocument_PrintPage;
                    printDocument.PrinterSettings.PrinterName = impressoraComboBox.SelectedItem.ToString();
                    printDocument.Print();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void txtNome_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void txtValorErrado_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;



            if (txtValorErrado.Text.Length > 0)
            {
                if (txtValorErrado.Text.Contains(","))
                {
                    int index = txtValorErrado.Text.IndexOf(",");

                    if (txtValorErrado.Text.Length == index + 3)
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

            if (e.Key == Key.Enter)
            {
                txtValorCorreto.Focus();
                txtValorCorreto.SelectAll();
            }
        }

        private void txtValorCorreto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;



            if (txtValorCorreto.Text.Length > 0)
            {
                if (txtValorCorreto.Text.Contains(","))
                {
                    int index = txtValorCorreto.Text.IndexOf(",");

                    if (txtValorCorreto.Text.Length == index + 3)
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

            if (e.Key == Key.Enter)
            {
                btnImprimirRecibo.Focus();
            }
        }

        private void txtValorErrado_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorErrado.Text == "")
                txtValorErrado.Text = "0,00";
            else
                txtValorErrado.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorErrado.Text));
        }

        private void txtValorErrado_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorErrado.Text == "0,00")
            {
                txtValorErrado.Text = "";
            }
        }

        private void txtValorCorreto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorCorreto.Text == "")
                txtValorCorreto.Text = "0,00";
            else
                txtValorCorreto.Text = string.Format("{0:n2}", Convert.ToDecimal(txtValorCorreto.Text));
        }

        private void txtValorCorreto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtValorCorreto.Text == "0,00")
            {
                txtValorCorreto.Text = "";
            }
        }




    }
}