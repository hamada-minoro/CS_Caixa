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
    /// Interaction logic for WinCustasProtesto.xaml
    /// </summary>
    public partial class WinCustasProtesto : Window
    {

        WinDigitarAtoProtesto winDigitarProtesto;
        WinPrincipal Principal;
        public ItensCustasProtesto itemSelecionado = new ItensCustasProtesto();
        decimal emol;

        public WinCustasProtesto(WinDigitarAtoProtesto winDigitarProtesto, WinPrincipal Principal)
        {
            this.winDigitarProtesto = winDigitarProtesto;
            this.Principal = Principal;
            InitializeComponent();
        }
        
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridItens.ItemsSource = winDigitarProtesto.listaCustasItens;
            dataGridItensSelecionados.ItemsSource = winDigitarProtesto.listaItens;

            txtEmol.Text = winDigitarProtesto.txtEmol.Text;
            txtFetj.Text = winDigitarProtesto.txtFetj.Text;
            txtFundperj.Text = winDigitarProtesto.txtFundperj.Text;
            txtFunperj.Text = winDigitarProtesto.txtFunperj.Text;
            txtFunarpen.Text = winDigitarProtesto.txtFunarpen.Text;
            txtPmcmv.Text = winDigitarProtesto.txtPmcmv.Text;
            txtIss.Text = winDigitarProtesto.txtIss.Text;
            txtMutua.Text = winDigitarProtesto.txtMutua.Text;
            txtAcoterj.Text = winDigitarProtesto.txtAcoterj.Text;
            emol = Convert.ToDecimal(winDigitarProtesto.listaItens.Sum(p => p.Total));
            txtTotal.Text = winDigitarProtesto.CalcularTotal(winDigitarProtesto.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
            
        }


        private void CalcularValores()
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss = 0;


            string Semol = "0,00";
            string Sfetj_20 = "0,00";
            string Sfundperj_5 = "0,00";
            string Sfunperj_5 = "0,00";
            string Sfunarpen_4 = "0,00";
            string Spmcmv_2 = "0,00";
            string Siss = "0,00";
            int index;

            
            try
            {

                emol = Convert.ToDecimal(winDigitarProtesto.listaItens.Sum(p => p.Total));
                fetj_20 = emol * 20 / 100;
                fundperj_5 = emol * 5 / 100;
                funperj_5 = emol * 5 / 100;
                funarpen_4 = emol * 4 / 100;
                
                //iss = (100 - winDigitarProtesto.porcentagemIss) / 100;
                //iss = emol / iss - emol;

                iss = emol * winDigitarProtesto.porcentagemIss / 100;

                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {
                    winDigitarProtesto.emolumentos.VALOR = winDigitarProtesto.listaItens[0].Total;
                    pmcmv_2 = Convert.ToDecimal(winDigitarProtesto.emolumentos.VALOR * 2) / 100;
                }
                Semol = Convert.ToString(emol);
                Sfetj_20 = Convert.ToString(fetj_20);
                Sfundperj_5 = Convert.ToString(fundperj_5);
                Sfunperj_5 = Convert.ToString(funperj_5);
                Sfunarpen_4 = Convert.ToString(funarpen_4);
                Siss = Convert.ToString(iss);
                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                }



                
                index = Sfetj_20.IndexOf(',');
                Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                index = Sfundperj_5.IndexOf(',');
                Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                index = Sfunperj_5.IndexOf(',');
                Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                index = Sfunarpen_4.IndexOf(',');
                Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                {
                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

                }

                if (winDigitarProtesto.cmbTipoCustas.SelectedIndex == 0)
                {
                    txtFetj.Text = Sfetj_20;
                    txtFundperj.Text = Sfundperj_5;
                    txtFunperj.Text = Sfunperj_5;
                    txtFunarpen.Text = Sfunarpen_4;
                    txtIss.Text = Siss;
                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                    {
                        txtPmcmv.Text = Spmcmv_2;
                        txtMutua.Text = string.Format("{0:n2}", winDigitarProtesto.mutua);
                        txtAcoterj.Text = string.Format("{0:n2}", winDigitarProtesto.acoterj);
                    }
                    else
                    {
                        txtPmcmv.Text = "0,00";
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }
                }
                if (winDigitarProtesto.cmbTipoCustas.SelectedIndex == 1)
                {
                    txtEmol.Text = "0,00";
                    txtFetj.Text = Sfetj_20;
                    txtFundperj.Text = Sfundperj_5;
                    txtFunperj.Text = Sfunperj_5;
                    txtFunarpen.Text = Sfunarpen_4;
                    txtIss.Text = Siss;
                    if (Principal.TipoAto != "CERTIDÃO PROTESTO")
                    {
                        txtPmcmv.Text = Spmcmv_2;
                        txtMutua.Text = string.Format("{0:n2}", winDigitarProtesto.mutua);
                        txtAcoterj.Text = string.Format("{0:n2}", winDigitarProtesto.acoterj);
                    }
                    else
                    {
                        txtPmcmv.Text = "0,00";
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }

                }
                if (winDigitarProtesto.cmbTipoCustas.SelectedIndex > 1)
                {
                    txtFetj.Text = "0,00";
                    txtFundperj.Text = "0,00";
                    txtFunperj.Text = "0,00";
                    txtFunarpen.Text = "0,00";
                    txtPmcmv.Text = "0,00";
                    txtIss.Text = "0,00";
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }
                else
                {
                    if (winDigitarProtesto.cmbTipoCustas.SelectedIndex == 0)
                    {
                        txtEmol.Text = string.Format("{0:n2}", emol);
                        winDigitarProtesto.txtEmol.Text = txtEmol.Text;
                    }
                    else
                    {
                        txtEmol.Text = "0,00";
                        winDigitarProtesto.txtEmol.Text = "0,00";
                    }
                    winDigitarProtesto.txtFetj.Text = txtFetj.Text;
                    winDigitarProtesto.txtFundperj.Text = txtFundperj.Text;
                    winDigitarProtesto.txtFunperj.Text = txtFunperj.Text;
                    winDigitarProtesto.txtFunarpen.Text = txtFunarpen.Text;
                    winDigitarProtesto.txtPmcmv.Text = txtPmcmv.Text;
                    winDigitarProtesto.txtIss.Text = txtIss.Text;
                }

                txtTotal.Text = winDigitarProtesto.CalcularTotal(winDigitarProtesto.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
                winDigitarProtesto.txtTotal.Text = winDigitarProtesto.CalcularTotal(winDigitarProtesto.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtMutua.Text, txtAcoterj.Text);
                winDigitarProtesto.lblTotal.Content = string.Format("{0}", winDigitarProtesto.txtTotal.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void CalcularItensCustas()
        {
            ItensCustasProtesto novoIten;
            novoIten = new ItensCustasProtesto();
            var arqrivDesarquiv = (CustasProtesto)dataGridItens.SelectedItem;
            novoIten.Item = arqrivDesarquiv.ITEM;
            novoIten.SubItem = arqrivDesarquiv.SUB;
            novoIten.Tabela = arqrivDesarquiv.TAB;
            novoIten.Descricao = arqrivDesarquiv.TEXTO;
           
            if (txtQtdItens.Text != "")
                novoIten.Quantidade = txtQtdItens.Text;
            else
                novoIten.Quantidade = "1";

            novoIten.Valor = arqrivDesarquiv.VALOR;
            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
            winDigitarProtesto.listaItens.Add(novoIten);


            if (winDigitarProtesto.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarProtesto.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValores();
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            CalcularItensCustas();
            dataGridItensSelecionados.ItemsSource = winDigitarProtesto.listaItens;
            dataGridItensSelecionados.Items.Refresh();
            txtQtdItens.Text = "1";
        }

        private void txtQtdItens_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdItens.Text == "" || txtQtdItens.Text == "0")
            {
                txtQtdItens.Text = "1";
            }
        }

        private void txtQtdItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3);
        }

        private void txtQtdItens_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQtdItens.Text == "1")
            {
                txtQtdItens.Text = "";
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (winDigitarProtesto.listaItens[0] != (ItensCustasProtesto)dataGridItensSelecionados.SelectedItem)
            {
                RemoverItem();
                dataGridItensSelecionados.ItemsSource = winDigitarProtesto.listaItens;
                dataGridItensSelecionados.Items.Refresh();
            }
        }


        private void RemoverItem()
        {

            var item = (ItensCustasProtesto)dataGridItensSelecionados.SelectedItem;

            winDigitarProtesto.listaItens.Remove(item);


            if (winDigitarProtesto.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarProtesto.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValores();
        }


        private void LimparGrid()
        {
            int qtdGrid = dataGridItensSelecionados.Items.Count - 1;
            for (int cont = qtdGrid; cont >= 1; cont--)
            {
                var item = (ItensCustasProtesto)winDigitarProtesto.listaItens[cont];

                winDigitarProtesto.listaItens.Remove(item);
            }

            if (winDigitarProtesto.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarProtesto.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValores();
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparGrid();
            dataGridItensSelecionados.ItemsSource = winDigitarProtesto.listaItens;
            dataGridItensSelecionados.Items.Refresh();
        }

        private void dataGridItens_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CalcularItensCustas();
            dataGridItensSelecionados.ItemsSource = winDigitarProtesto.listaItens;
            dataGridItensSelecionados.Items.Refresh();
            txtQtdItens.Text = "1";
        }

        private void dataGridItensSelecionados_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;


            if (key == 32)
            {
                if (winDigitarProtesto.listaItens[0] != (ItensCustasProtesto)dataGridItensSelecionados.SelectedItem)
                {
                    RemoverItem();
                    dataGridItensSelecionados.ItemsSource = winDigitarProtesto.listaItens;
                    dataGridItensSelecionados.Items.Refresh();
                }
            }
        }

        private void dataGridItensSelecionados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WinAlterarItemCustas winAlterarItemCustas = new WinAlterarItemCustas(this);
            winAlterarItemCustas.Owner = this;
            winAlterarItemCustas.ShowDialog();

            dataGridItensSelecionados.Items.Refresh();

            CalcularValores();
        }

        private void dataGridItensSelecionados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemSelecionado = (ItensCustasProtesto)dataGridItensSelecionados.SelectedItem;
        }

       
       
       
    }
}