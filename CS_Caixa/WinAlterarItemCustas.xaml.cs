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

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAlterarItemCustas.xaml
    /// </summary>
    public partial class WinAlterarItemCustas : Window
    {
        WinCustasNotas custasNotas;
        
        WinCustasProtesto custasProtesto;

        WinCustasRgi custasRgi;

        string atribuicao;

        public WinAlterarItemCustas(WinCustasNotas custasNotas)
        {
            this.custasNotas = custasNotas;
            atribuicao = "notas";
            InitializeComponent();
        }
       
        public WinAlterarItemCustas(WinCustasProtesto custasProtesto)
        {
            this.custasProtesto = custasProtesto;
            atribuicao = "protesto";
            InitializeComponent();
        }

        public WinAlterarItemCustas(WinCustasRgi custasRgi)
        {
            this.custasRgi = custasRgi;
            atribuicao = "rgi";
            InitializeComponent();
        }


        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void textBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;



            if (textBox2.Text.Length > 0)
            {
                if (textBox2.Text.Contains(","))
                {
                    int index = textBox2.Text.IndexOf(",");

                    if (textBox2.Text.Length == index + 3)
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (atribuicao == "notas")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (textBox1.Text != "")
                    {
                        if (custasNotas.itemSelecionado != null)
                        {
                            custasNotas.itemSelecionado.Quantidade = textBox1.Text;
                        }
                        if (custasNotas.itemSelecionadoControleAtosNota != null)
                        {
                            custasNotas.itemSelecionadoControleAtosNota.Quantidade = textBox1.Text;
                        }

                    }
                    if (textBox2.Text != "")
                    {
                        if (custasNotas.itemSelecionado != null)
                        {
                            custasNotas.itemSelecionado.Valor = Convert.ToDecimal(textBox2.Text);
                        }
                        if (custasNotas.itemSelecionadoControleAtosNota != null)
                        {
                            custasNotas.itemSelecionadoControleAtosNota.Valor = Convert.ToDecimal(textBox2.Text);
                        }
                    }

                    if (custasNotas.itemSelecionado != null)
                    {
                        custasNotas.itemSelecionado.Total = Convert.ToInt32(custasNotas.itemSelecionado.Quantidade) * custasNotas.itemSelecionado.Valor;
                    }
                    if (custasNotas.itemSelecionadoControleAtosNota != null)
                    {
                        custasNotas.itemSelecionadoControleAtosNota.Total = Convert.ToInt32(custasNotas.itemSelecionadoControleAtosNota.Quantidade) * custasNotas.itemSelecionadoControleAtosNota.Valor;
                    }
                }
            }

            if (atribuicao == "protesto")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (textBox1.Text != "")
                    {
                        custasProtesto.itemSelecionado.Quantidade = textBox1.Text;
                    }
                    if (textBox2.Text != "")
                    {
                        custasProtesto.itemSelecionado.Valor = Convert.ToDecimal(textBox2.Text);
                    }

                    custasProtesto.itemSelecionado.Total = Convert.ToInt32(custasProtesto.itemSelecionado.Quantidade) * custasProtesto.itemSelecionado.Valor;
                }
            }
            if (atribuicao == "rgi")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (textBox1.Text != "")
                    {
                        custasRgi.itemSelecionado.Quantidade = textBox1.Text;
                    }
                    if (textBox2.Text != "")
                    {
                        custasRgi.itemSelecionado.Valor = Convert.ToDecimal(textBox2.Text);
                    }

                    custasRgi.itemSelecionado.Total = Convert.ToInt32(custasRgi.itemSelecionado.Quantidade) * custasRgi.itemSelecionado.Valor;
                }
            }


            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (atribuicao == "notas")
            {
                if (custasNotas.chamada == "winDigitarEscritura")
                {
                    textBox1.Text = custasNotas.itemSelecionado.Quantidade;
                    textBox2.Text = string.Format("{0:n2}", custasNotas.itemSelecionado.Valor);
                }
                else
                {
                    textBox1.Text = custasNotas.itemSelecionadoControleAtosNota.Quantidade;
                    textBox2.Text = string.Format("{0:n2}", custasNotas.itemSelecionadoControleAtosNota.Valor);
                }
            }

            if (atribuicao == "protesto")
            {
                textBox1.Text = custasProtesto.itemSelecionado.Quantidade;
                textBox2.Text = string.Format("{0:n2}", custasProtesto.itemSelecionado.Valor);
            }

            if (atribuicao == "rgi")
            {
                textBox1.Text = custasRgi.itemSelecionado.Quantidade;
                textBox2.Text = string.Format("{0:n2}", custasRgi.itemSelecionado.Valor);
            }
        }

        private void textBox1_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "0")
            {
                textBox1.Text = "1";
            }
        }

        private void grid1_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void textBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBox2.Text != "")
                textBox2.Text = string.Format("{0:n2}", Convert.ToDecimal(textBox2.Text));
        }
    }
}
