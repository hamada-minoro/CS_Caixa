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
    /// Interaction logic for WinCustasRgi.xaml
    /// </summary>
    public partial class WinCustasRgi : Window
    {
        WinDigitarAtoRgi winDigitarRgi;
        WinPrincipal Principal;
        public ItensCustasRgi itemSelecionado = new ItensCustasRgi();
        ItensAtoRgi atoAlterar;
        string alteraItensNoGrid;

        ItensCustasRgi emolumentos = new ItensCustasRgi();

        public WinCustasRgi(WinDigitarAtoRgi winDigitarRgi, WinPrincipal Principal, string alteraItensNoGrid)
        {
            this.alteraItensNoGrid = alteraItensNoGrid;
            this.winDigitarRgi = winDigitarRgi;
            this.Principal = Principal;
            InitializeComponent();
        }

        public WinCustasRgi(WinDigitarAtoRgi winDigitarRgi, WinPrincipal Principal, string alteraItensNoGrid, ItensAtoRgi atoAlterar)
        {
            this.alteraItensNoGrid = alteraItensNoGrid;
            this.winDigitarRgi = winDigitarRgi;
            this.Principal = Principal;
            this.atoAlterar = atoAlterar;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridItens.ItemsSource = winDigitarRgi.itensCustasRgi;

            if (alteraItensNoGrid == "nao")
            {
                dataGridItensSelecionados.ItemsSource = winDigitarRgi.listaItensCalculo;
                txtDistribuicao.Text = winDigitarRgi.txtDistribuicao.Text;
                emolumentos = (ItensCustasRgi)winDigitarRgi.listaItensCalculo[0];
                CalcularValoresEmolumentos();
                CalcularTotalValoresEmol();
            }
            else
            {
                dataGridItensSelecionados.ItemsSource = winDigitarRgi.listaItensCalculo;
                txtDistribuicao.Text = string.Format("{0:n2}", atoAlterar.Distribuicao);
                txtDistribuicao.IsReadOnly = false;
                emolumentos = (ItensCustasRgi)winDigitarRgi.listaItensCalculo[0];

                CalcularValoresEmolumentos();
                CalcularTotalValoresEmol();
            }
        }


        /// <summary>
        /// CALCULAR TODOS OS VALORES PARA ADICIONAR NO GRID
        /// </summary>
        private void CalcularTotalValoresEmol()
        {
            decimal Total;


            try
            {
                Total = Convert.ToDecimal(txtEmol.Text) + Convert.ToDecimal(txtFetj.Text) + Convert.ToDecimal(txtFundperj.Text) + Convert.ToDecimal(txtFunperj.Text) + Convert.ToDecimal(txtFunarpen.Text) + Convert.ToDecimal(txtIss.Text) + Convert.ToDecimal(txtPmcmv.Text) + Convert.ToDecimal(txtMutua.Text) + Convert.ToDecimal(txtAcoterj.Text) + Convert.ToDecimal(txtDistribuicao.Text);
                txtTotal.Text = string.Format("{0:n2}", Total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        /// <summary>
        /// CALCULAR OS VALORES DE CUSTAS APARTIR DOS EMOLUMENTOS DA LISTA CALCULAR
        /// </summary>
        private void CalcularValoresEmolumentos()
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



            emol = Convert.ToDecimal(winDigitarRgi.listaItensCalculo.Sum(p => p.Total));

            try
            {
                if (winDigitarRgi.cmbTipoCustas.SelectedIndex <= 1)
                {
                    fetj_20 = emol * 20 / 100;
                    fundperj_5 = emol * 5 / 100;
                    funperj_5 = emol * 5 / 100;
                    funarpen_4 = emol * 4 / 100;
                    iss = (100 - winDigitarRgi.porcentagemIss) / 100;
                    iss = emol / iss - emol;




                    winDigitarRgi.emolumentos.VALOR = winDigitarRgi.listaItensCalculo[0].Total;

                    var iten = winDigitarRgi.listaItensCalculo.Where(p => p.Tabela == "20.1" && p.Item == "NI" && p.SubItem == "1").FirstOrDefault();
                    var iten2 = winDigitarRgi.listaItensCalculo.Where(p => p.Tabela == "20.2" && p.Item == "NI" && p.SubItem == "*").FirstOrDefault();
                    decimal valorIten = 0M;


                    if (iten != null)
                        valorIten = valorIten + Convert.ToDecimal(iten.Total);
                   if(iten2 != null)
                       valorIten = valorIten + Convert.ToDecimal(iten2.Total);
                    
                    pmcmv_2 = Convert.ToDecimal((emolumentos.Valor + valorIten) * 2) / 100;

                    if (winDigitarRgi.cmbTipoCustas.SelectedIndex == 0)
                        Semol = Convert.ToString(emol);

                    Sfetj_20 = Convert.ToString(fetj_20);
                    Sfundperj_5 = Convert.ToString(fundperj_5);
                    Sfunperj_5 = Convert.ToString(funperj_5);
                    Sfunarpen_4 = Convert.ToString(funarpen_4);
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                    Siss = Convert.ToString(iss);
                }

                if (winDigitarRgi.cmbTipoCustas.SelectedIndex > 1)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    pmcmv_2 = 0;
                    iss = 0;

                    Semol = "0,00";
                    Sfetj_20 = "0,00";
                    Sfundperj_5 = "0,00";
                    Sfunperj_5 = "0,00";
                    Sfunarpen_4 = "0,00";
                    Spmcmv_2 = "0,00";
                    Siss = "0,00";
                }


                index = Semol.IndexOf(',');
                Semol = Semol.Substring(0, index + 3);

                index = Sfetj_20.IndexOf(',');
                Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                index = Sfundperj_5.IndexOf(',');
                Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


                index = Sfunperj_5.IndexOf(',');
                Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


                index = Sfunarpen_4.IndexOf(',');
                Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);


                index = Spmcmv_2.IndexOf(',');
                Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                if (winDigitarRgi.cmbPrenotacao.SelectedIndex <= 1 || winDigitarRgi.cmbPrenotacao.SelectedIndex == 3)
                {
                    txtEmol.Text = Semol;
                    txtFetj.Text = Sfetj_20;
                    txtFundperj.Text = Sfundperj_5;
                    txtFunperj.Text = Sfunperj_5;
                    txtFunarpen.Text = Sfunarpen_4;
                    txtPmcmv.Text = Spmcmv_2;
                    txtIss.Text = Siss;
                    if (winDigitarRgi.cmbTipoCustas.SelectedIndex <= 1)
                    {
                        if (Principal.TipoAto != "CERTIDÃO RGI")
                        {
                            txtMutua.Text = string.Format("{0:n2}", winDigitarRgi.mutua);
                            txtAcoterj.Text = string.Format("{0:n2}", winDigitarRgi.acoterj);
                        }
                        else
                        {
                            txtMutua.Text = "0,00";
                            txtAcoterj.Text = "0,00";
                        }
                    }
                    else
                    {
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }
                }
                else
                {
                    txtEmol.Text = Semol;
                    txtFetj.Text = "0,00";
                    txtFundperj.Text = "0,00";
                    txtFunperj.Text = "0,00";
                    txtFunarpen.Text = "0,00";
                    txtPmcmv.Text = "0,00";
                    txtIss.Text = "0,00";
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            ProcAdicionar();
        }

        private void ProcAdicionar()
        {
            CalcularItensCustas();
            dataGridItensSelecionados.ItemsSource = winDigitarRgi.listaItensCalculo;
            dataGridItensSelecionados.Items.Refresh();
            txtQtdItens.Text = "1";
        }


        /// <summary>
        /// CARREGAR ITENS PARA A LISTA DE CALCULO
        /// </summary>
        private void CalcularItensCustas()
        {
            try
            {
                ItensCustasRgi novoIten;
                novoIten = new ItensCustasRgi();
                var arqrivDesarquiv = (CustasRgi)dataGridItens.SelectedItem;
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

                var itemExistente = winDigitarRgi.listaItensCalculo.Where(p => p.Descricao == novoIten.Descricao && p.Item == novoIten.Item && p.Tabela == novoIten.Tabela && p.SubItem == novoIten.SubItem).FirstOrDefault();

                if (itemExistente == null)
                    winDigitarRgi.listaItensCalculo.Add(novoIten);
                else
                {
                    var qtd = Convert.ToInt32(novoIten.Quantidade) + Convert.ToInt32(itemExistente.Quantidade);

                    itemExistente.Quantidade = qtd.ToString();
                    itemExistente.Total = itemExistente.Valor * qtd;
                }



                CalcularValoresEmolumentos();
                CalcularTotalValoresEmol();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void txtQtdItens_LostFocus(object sender, RoutedEventArgs e)
        {
            
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
            ProcRemoverItens();
        }

        private void ProcRemoverItens()
        {
            if (winDigitarRgi.listaItensCalculo[0] != (ItensCustasRgi)dataGridItensSelecionados.SelectedItem)
            {
                RemoverItem();
                dataGridItensSelecionados.ItemsSource = winDigitarRgi.listaItensCalculo;
                dataGridItensSelecionados.Items.Refresh();
            }
        }

        private void RemoverItem()
        {

            var item = (ItensCustasRgi)dataGridItensSelecionados.SelectedItem;

            winDigitarRgi.listaItensCalculo.Remove(item);


            if (winDigitarRgi.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarRgi.listaItensCalculo.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValoresEmolumentos();
            CalcularTotalValoresEmol();
        }

        private void LimparGrid()
        {
            int qtdGrid = dataGridItensSelecionados.Items.Count - 1;
            for (int cont = qtdGrid; cont >= 1; cont--)
            {
                var item = (ItensCustasRgi)winDigitarRgi.listaItensCalculo[cont];

                winDigitarRgi.listaItensCalculo.Remove(item);
            }

            if (winDigitarRgi.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarRgi.listaItensCalculo.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValoresEmolumentos();
            CalcularTotalValoresEmol();
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparGrid();
            dataGridItensSelecionados.ItemsSource = winDigitarRgi.listaItensCalculo;
            dataGridItensSelecionados.Items.Refresh();
        }

        private void dataGridItens_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcAdicionar();
        }

        private void dataGridItensSelecionados_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ProcRemoverItens();
            }
        }

        private void dataGridItensSelecionados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WinAlterarItemCustas winAlterarItemCustas = new WinAlterarItemCustas(this);
            winAlterarItemCustas.Owner = this;
            winAlterarItemCustas.ShowDialog();

            dataGridItensSelecionados.Items.Refresh();

            CalcularValoresEmolumentos();
            CalcularTotalValoresEmol();
        }

        private void dataGridItensSelecionados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemSelecionado = (ItensCustasRgi)dataGridItensSelecionados.SelectedItem;
        }

        private void txtDistribuicao_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;



            if (txtDistribuicao.Text.Length > 0)
            {
                if (txtDistribuicao.Text.Contains(","))
                {
                    int index = txtDistribuicao.Text.IndexOf(",");

                    if (txtDistribuicao.Text.Length == index + 3)
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

            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void txtDistribuicao_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtDistribuicao.Text == "")
                txtDistribuicao.Text = "0,00";
            else
                txtDistribuicao.Text = string.Format("{0:n2}", Convert.ToDecimal(txtDistribuicao.Text));


            atoAlterar.Distribuicao = Convert.ToDecimal(txtDistribuicao.Text);

            CalcularTotalValoresEmol();
        }

        private void txtDistribuicao_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtDistribuicao.Text == "0,00")
            {
                txtDistribuicao.Text = "";
            }
        }
    }
}
