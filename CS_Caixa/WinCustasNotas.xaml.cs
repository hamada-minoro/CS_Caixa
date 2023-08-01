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
    /// Interaction logic for WinCustasNotas.xaml
    /// </summary>
    public partial class WinCustasNotas : Window
    {

        WinDigitarAtoNotas winDigitarEscritura;
        WinDigitarControleAtosNotas winDigitarControleAtosNotas;
        WinPrincipal Principal;
        public ItensCustasNota itemSelecionado = new ItensCustasNota();
        public ItensCustasControleAtosNota itemSelecionadoControleAtosNota = new ItensCustasControleAtosNota();
        decimal emol;
        ClassCustasNotas classCustasNotas = new ClassCustasNotas();


        public string chamada;
        public WinCustasNotas(WinDigitarAtoNotas winDigitarEscritura, WinPrincipal Principal)
        {
            chamada = "winDigitarEscritura";
            this.winDigitarEscritura = winDigitarEscritura;
            this.Principal = Principal;
            InitializeComponent();
        }

        public WinCustasNotas(WinDigitarControleAtosNotas winDigitarControleAtosNotas, WinPrincipal Principal)
        {
            chamada = "winDigitarControleAtosNotas";
            this.winDigitarControleAtosNotas = winDigitarControleAtosNotas;
            this.Principal = Principal;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            winDigitarEscritura.valor2PorCentoAcima4000 = winDigitarEscritura.listaCustas.Where(p => p.TAB == "22" && p.SUB == "21").FirstOrDefault();
            if (chamada == "winDigitarEscritura")
            {
                dataGridItens.ItemsSource = winDigitarEscritura.listaCustasItens;
                dataGridItensSelecionados.ItemsSource = winDigitarEscritura.listaItens;

                txtEmol.Text = winDigitarEscritura.txtEmol.Text;
                txtFetj.Text = winDigitarEscritura.txtFetj.Text;
                txtFundperj.Text = winDigitarEscritura.txtFundperj.Text;
                txtFunperj.Text = winDigitarEscritura.txtFunperj.Text;
                txtFunarpen.Text = winDigitarEscritura.txtFunarpen.Text;
                txtPmcmv.Text = winDigitarEscritura.txtPmcmv.Text;
                txtIss.Text = winDigitarEscritura.txtIss.Text;
                txtIndisp.Text = winDigitarEscritura.txtIndisp.Text;
                txtDistribuicao.Text = winDigitarEscritura.txtDistribuicao.Text;
                txtMutua.Text = winDigitarEscritura.txtMutua.Text;
                txtAcoterj.Text = winDigitarEscritura.txtAcoterj.Text;
                emol = Convert.ToDecimal(winDigitarEscritura.listaItens.Sum(p => p.Total));
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                    txtTotal.Text = winDigitarEscritura.CalcularTotal("0,00", "0,00", winDigitarEscritura.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
                else
                    txtTotal.Text = winDigitarEscritura.txtTotal.Text;
            }
            else
            {
                dataGridItens.ItemsSource = winDigitarControleAtosNotas.listaCustasItens;
                dataGridItensSelecionados.ItemsSource = winDigitarControleAtosNotas.listaItens;


                txtEmol.Text = winDigitarControleAtosNotas.txtEmol.Text;
                txtFetj.Text = winDigitarControleAtosNotas.txtFetj.Text;
                txtFundperj.Text = winDigitarControleAtosNotas.txtFundperj.Text;
                txtFunperj.Text = winDigitarControleAtosNotas.txtFunperj.Text;
                txtFunarpen.Text = winDigitarControleAtosNotas.txtFunarpen.Text;
                txtPmcmv.Text = winDigitarControleAtosNotas.txtPmcmv.Text;
                txtIss.Text = winDigitarControleAtosNotas.txtIss.Text;
                txtIndisp.IsEnabled = false;
                txtDistribuicao.IsEnabled = false;
                txtMutua.Text = winDigitarControleAtosNotas.txtMutua.Text;
                txtAcoterj.Text = winDigitarControleAtosNotas.txtAcoterj.Text;
                emol = Convert.ToDecimal(winDigitarControleAtosNotas.listaItens.Sum(p => p.Total));
                txtTotal.IsEnabled = false;
            }


        }


        //private void CalcularValores()
        //{
        //    decimal emol = 0;
        //    decimal fetj_20 = 0;
        //    decimal fundperj_5 = 0;
        //    decimal funperj_5 = 0;
        //    decimal funarpen_4 = 0;
        //    decimal pmcmv_2 = 0;
        //    decimal iss = 0;
        //    decimal indisp = 0;
        //    decimal distrib = 0;
        //    string Semol = "0,00";
        //    string Sfetj_20 = "0,00";
        //    string Sfundperj_5 = "0,00";
        //    string Sfunperj_5 = "0,00";
        //    string Sfunarpen_4 = "0,00";
        //    string Spmcmv_2 = "0,00";
        //    string Siss = "0,00";
        //    int index;


        //    try
        //    {
        //        var qtdTabela7 = winDigitarEscritura.listaItens.Where(p => p.Complemento == "S").Sum(p => Convert.ToInt16(p.Quantidade));
        //        // var qtdTabela7 = winDigitarEscritura.listaItens.Where(p => p.Complemento == "S").Count();
        //        distrib = Convert.ToDecimal(winDigitarEscritura.listaDistribuicao.Where(p => p.Quant_Exced == Convert.ToDecimal(winDigitarEscritura.txtQtdExced.Text)).Select(p => p.Total).FirstOrDefault()) * qtdTabela7;

        //        foreach (var item in winDigitarEscritura.listaItens)
        //        {
        //            if (!(item.Tabela == "22" && item.Item == "NI" && item.SubItem == "21"))
        //            {
        //                emol = emol + Convert.ToDecimal(item.Total);
        //            }
        //        }


        //        fetj_20 = emol * 20 / 100;
        //        fundperj_5 = emol * 5 / 100;
        //        funperj_5 = emol * 5 / 100;
        //        funarpen_4 = emol * 4 / 100;


        //        iss = classCustasNotas.CalcularISSNotas(emol, true, 5);


        //        if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
        //        {
        //            winDigitarEscritura.emolumentos.VALOR = winDigitarEscritura.listaItens.Where(p => p.Complemento == "S").Sum(p => p.Total);

        //            var iten = winDigitarEscritura.listaItens.Where(p => p.Tabela == "22" && p.Item == "NI" && p.SubItem == "20").FirstOrDefault();
        //            if (iten != null)
        //            {

        //                pmcmv_2 = ((Convert.ToDecimal(winDigitarEscritura.emolumentos.VALOR + iten.Total) * 2) / 100);

        //                //pmcmv_2 = ((Convert.ToDecimal(winDigitarEscritura.emolumentos.VALOR) * 2) / 100);

        //                //pmcmv_2 = pmcmv_2 + (Convert.ToInt16(iten.Quantidade) * Convert.ToDecimal(winDigitarEscritura.valor2PorCentoAcima4000.VALOR));
        //            }
        //            else
        //                pmcmv_2 = Convert.ToDecimal(winDigitarEscritura.emolumentos.VALOR * 2) / 100;
        //        }
        //        Semol = Convert.ToString(emol);
        //        Sfetj_20 = Convert.ToString(fetj_20);
        //        Sfundperj_5 = Convert.ToString(fundperj_5);
        //        Sfunperj_5 = Convert.ToString(funperj_5);
        //        Sfunarpen_4 = Convert.ToString(funarpen_4);
        //        Siss = Convert.ToString(iss);
        //        if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
        //        {
        //            Spmcmv_2 = Convert.ToString(pmcmv_2);
        //        }
        //        indisp = winDigitarEscritura.indisponibilidade * Convert.ToInt16(winDigitarEscritura.txtQtdIndisp.Text);


        //        index = Sfetj_20.IndexOf(',');
        //        Sfetj_20 = Sfetj_20.Substring(0, index + 3);


        //        index = Sfundperj_5.IndexOf(',');
        //        Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);


        //        index = Sfunperj_5.IndexOf(',');
        //        Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);


        //        index = Sfunarpen_4.IndexOf(',');
        //        Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

        //        index = Siss.IndexOf(',');
        //        Siss = Siss.Substring(0, index + 3);


        //        if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
        //        {
        //            index = Spmcmv_2.IndexOf(',');
        //            Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

        //        }

        //        if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 0)
        //        {

        //            if (Principal.TipoAto != "APOSTILAMENTO HAIA")
        //            {
        //                txtFetj.Text = Sfetj_20;
        //                txtFundperj.Text = Sfundperj_5;
        //                txtFunperj.Text = Sfunperj_5;
        //                txtFunarpen.Text = Sfunarpen_4;
        //                txtIss.Text = Siss;
        //            }
        //            else
        //            {
        //                txtEmol.Text = "0,00";
        //                txtFetj.Text = "0,00";
        //                txtFundperj.Text = "0,00";
        //                txtFunperj.Text = "0,00";
        //                txtFunarpen.Text = "0,00";
        //                txtIss.Text = "0,00";
        //            }


        //            if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
        //            {
        //                txtPmcmv.Text = Spmcmv_2;
        //                txtIndisp.Text = string.Format("{0:n2}", indisp);
        //                txtDistribuicao.Text = string.Format("{0:n2}", distrib);
        //                txtMutua.Text = string.Format("{0:n2}", winDigitarEscritura.mutua * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
        //                txtAcoterj.Text = string.Format("{0:n2}", winDigitarEscritura.acoterj * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
        //            }
        //            else
        //            {
        //                txtPmcmv.Text = "0,00";
        //                txtIndisp.Text = "0,00";
        //                txtDistribuicao.Text = "0,00";
        //                txtMutua.Text = "0,00";
        //                txtAcoterj.Text = "0,00";
        //            }
        //        }
        //        if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 1)
        //        {
        //            txtEmol.Text = "0,00";
        //            txtFetj.Text = Sfetj_20;
        //            txtFundperj.Text = Sfundperj_5;
        //            txtFunperj.Text = Sfunperj_5;
        //            txtFunarpen.Text = Sfunarpen_4;
        //            txtIss.Text = Siss;
        //            if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
        //            {
        //                txtPmcmv.Text = Spmcmv_2;
        //                txtIndisp.Text = string.Format("{0:n2}", indisp);
        //                txtMutua.Text = string.Format("{0:n2}", winDigitarEscritura.mutua * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
        //                txtAcoterj.Text = string.Format("{0:n2}", winDigitarEscritura.acoterj * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
        //            }
        //            else
        //            {
        //                txtPmcmv.Text = "0,00";
        //                txtIndisp.Text = "0,00";
        //                txtDistribuicao.Text = "0,00";
        //                txtMutua.Text = "0,00";
        //                txtAcoterj.Text = "0,00";
        //            }

        //        }
        //        if (winDigitarEscritura.cmbTipoCustas.SelectedIndex > 1)
        //        {
        //            winDigitarEscritura.txtAdicionar.Text = "0,00";
        //            winDigitarEscritura.txtDesconto.Text = "0,00";
        //            winDigitarEscritura.txtVrCorretor.Text = "0,00";
        //            winDigitarEscritura.txtVrEscrevente.Text = "0,00";
        //            txtFetj.Text = "0,00";
        //            txtFundperj.Text = "0,00";
        //            txtFunperj.Text = "0,00";
        //            txtFunarpen.Text = "0,00";
        //            txtPmcmv.Text = "0,00";
        //            txtIss.Text = "0,00";
        //            txtIndisp.Text = "0,00";
        //            txtDistribuicao.Text = "0,00";
        //            txtMutua.Text = "0,00";
        //            txtAcoterj.Text = "0,00";
        //        }
        //        if (Principal.TipoAto != "APOSTILAMENTO HAIA")
        //            txtEmol.Text = string.Format("{0:n2}", emol);
        //        else
        //            txtEmol.Text = "0,00";
        //        winDigitarEscritura.txtEmol.Text = txtEmol.Text;
        //        winDigitarEscritura.txtFetj.Text = txtFetj.Text;
        //        winDigitarEscritura.txtFundperj.Text = txtFundperj.Text;
        //        winDigitarEscritura.txtFunperj.Text = txtFunperj.Text;
        //        winDigitarEscritura.txtFunarpen.Text = txtFunarpen.Text;
        //        winDigitarEscritura.txtPmcmv.Text = txtPmcmv.Text;
        //        winDigitarEscritura.txtIss.Text = txtIss.Text;
        //        winDigitarEscritura.txtDistribuicao.Text = txtDistribuicao.Text;

        //        if (Principal.TipoAto != "APOSTILAMENTO HAIA")
        //        {
        //            txtTotal.Text = winDigitarEscritura.CalcularTotal("0,00", "0,00", winDigitarEscritura.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
        //            winDigitarEscritura.txtTotal.Text = winDigitarEscritura.CalcularTotal(winDigitarEscritura.txtAdicionar.Text, winDigitarEscritura.txtDesconto.Text, winDigitarEscritura.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
        //            winDigitarEscritura.lblTotal.Content = string.Format("{0}", winDigitarEscritura.txtTotal.Text);
        //            winDigitarEscritura.lblTotal1.Content = winDigitarEscritura.lblTotal.Content;
        //        }
        //        else
        //        {
        //            txtTotal.Text = winDigitarEscritura.CalcularTotal("0,00", "0,00", Semol, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
        //            winDigitarEscritura.txtTotal.Text = winDigitarEscritura.CalcularTotal(winDigitarEscritura.txtAdicionar.Text, winDigitarEscritura.txtDesconto.Text, txtTotal.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
        //            winDigitarEscritura.lblTotal.Content = string.Format("{0}", winDigitarEscritura.txtTotal.Text);
        //            winDigitarEscritura.lblTotal1.Content = winDigitarEscritura.lblTotal.Content;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        private void CalcularValores(string valorDistribuicao)
        {
            decimal emol = 0;
            decimal fetj_20 = 0;
            decimal fundperj_5 = 0;
            decimal funperj_5 = 0;
            decimal funarpen_4 = 0;
            decimal pmcmv_2 = 0;
            decimal iss = 0;
            decimal indisp = 0;
            decimal distrib = 0;
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
                var qtdTabela7 = winDigitarEscritura.listaItens.Where(p => p.Complemento == "S").Sum(p => Convert.ToInt16(p.Quantidade));
                // var qtdTabela7 = winDigitarEscritura.listaItens.Where(p => p.Complemento == "S").Count();
                distrib = Convert.ToDecimal(valorDistribuicao);

                foreach (var item in winDigitarEscritura.listaItens)
                {
                    if (!(item.Tabela == "22" && item.Item == "NI" && item.SubItem == "21"))
                    {
                        emol = emol + Convert.ToDecimal(item.Total);
                    }
                }


                fetj_20 = emol * 20 / 100;
                fundperj_5 = emol * 5 / 100;
                funperj_5 = emol * 5 / 100;
                funarpen_4 = emol * 4 / 100;


                iss = classCustasNotas.CalcularISSNotas(emol, true, 5);


                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    winDigitarEscritura.emolumentos.VALOR = winDigitarEscritura.listaItens.Where(p => p.Complemento == "S").Sum(p => p.Total);

                    var iten = winDigitarEscritura.listaItens.Where(p => p.Tabela == "22" && p.Item == "NI" && p.SubItem == "20").FirstOrDefault();
                    if (iten != null)
                    {

                        pmcmv_2 = ((Convert.ToDecimal(winDigitarEscritura.emolumentos.VALOR + iten.Total) * 2) / 100);

                        //pmcmv_2 = ((Convert.ToDecimal(winDigitarEscritura.emolumentos.VALOR) * 2) / 100);

                        //pmcmv_2 = pmcmv_2 + (Convert.ToInt16(iten.Quantidade) * Convert.ToDecimal(winDigitarEscritura.valor2PorCentoAcima4000.VALOR));
                    }
                    else
                        pmcmv_2 = Convert.ToDecimal(winDigitarEscritura.emolumentos.VALOR * 2) / 100;
                }
                Semol = Convert.ToString(emol);
                Sfetj_20 = Convert.ToString(fetj_20);
                Sfundperj_5 = Convert.ToString(fundperj_5);
                Sfunperj_5 = Convert.ToString(funperj_5);
                Sfunarpen_4 = Convert.ToString(funarpen_4);
                Siss = Convert.ToString(iss);
                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    Spmcmv_2 = Convert.ToString(pmcmv_2);
                }
                indisp = winDigitarEscritura.indisponibilidade * Convert.ToInt16(winDigitarEscritura.txtQtdIndisp.Text);


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


                if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    index = Spmcmv_2.IndexOf(',');
                    Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

                }

                if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 0)
                {

                    if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                    {
                        txtFetj.Text = Sfetj_20;
                        txtFundperj.Text = Sfundperj_5;
                        txtFunperj.Text = Sfunperj_5;
                        txtFunarpen.Text = Sfunarpen_4;
                        txtIss.Text = Siss;
                    }
                    else
                    {
                        txtEmol.Text = "0,00";
                        txtFetj.Text = "0,00";
                        txtFundperj.Text = "0,00";
                        txtFunperj.Text = "0,00";
                        txtFunarpen.Text = "0,00";
                        txtIss.Text = "0,00";
                    }


                    if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                    {
                        txtPmcmv.Text = Spmcmv_2;
                        txtIndisp.Text = string.Format("{0:n2}", indisp);
                        txtDistribuicao.Text = string.Format("{0:n2}", distrib);
                        txtMutua.Text = string.Format("{0:n2}", winDigitarEscritura.mutua * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
                        txtAcoterj.Text = string.Format("{0:n2}", winDigitarEscritura.acoterj * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
                    }
                    else
                    {
                        txtPmcmv.Text = "0,00";
                        txtIndisp.Text = "0,00";
                        txtDistribuicao.Text = "0,00";
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }
                }
                if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 1)
                {
                    txtEmol.Text = "0,00";
                    txtFetj.Text = Sfetj_20;
                    txtFundperj.Text = Sfundperj_5;
                    txtFunperj.Text = Sfunperj_5;
                    txtFunarpen.Text = Sfunarpen_4;
                    txtIss.Text = Siss;
                    if (Principal.TipoAto != "CERTIDÃO NOTAS" && Principal.TipoAto != "APOSTILAMENTO HAIA")
                    {
                        txtPmcmv.Text = Spmcmv_2;
                        txtIndisp.Text = string.Format("{0:n2}", indisp);
                        txtMutua.Text = string.Format("{0:n2}", winDigitarEscritura.mutua * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
                        txtAcoterj.Text = string.Format("{0:n2}", winDigitarEscritura.acoterj * Convert.ToInt32(winDigitarEscritura.txtQtdAtos.Text));
                    }
                    else
                    {
                        txtPmcmv.Text = "0,00";
                        txtIndisp.Text = "0,00";
                        txtDistribuicao.Text = "0,00";
                        txtMutua.Text = "0,00";
                        txtAcoterj.Text = "0,00";
                    }

                }
                if (winDigitarEscritura.cmbTipoCustas.SelectedIndex > 1)
                {
                    winDigitarEscritura.txtAdicionar.Text = "0,00";
                    winDigitarEscritura.txtDesconto.Text = "0,00";
                    winDigitarEscritura.txtVrCorretor.Text = "0,00";
                    winDigitarEscritura.txtVrEscrevente.Text = "0,00";
                    txtFetj.Text = "0,00";
                    txtFundperj.Text = "0,00";
                    txtFunperj.Text = "0,00";
                    txtFunarpen.Text = "0,00";
                    txtPmcmv.Text = "0,00";
                    txtIss.Text = "0,00";
                    txtIndisp.Text = "0,00";
                    txtDistribuicao.Text = "0,00";
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";
                }
                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                    txtEmol.Text = string.Format("{0:n2}", emol);
                else
                    txtEmol.Text = "0,00";
                winDigitarEscritura.txtEmol.Text = txtEmol.Text;
                winDigitarEscritura.txtFetj.Text = txtFetj.Text;
                winDigitarEscritura.txtFundperj.Text = txtFundperj.Text;
                winDigitarEscritura.txtFunperj.Text = txtFunperj.Text;
                winDigitarEscritura.txtFunarpen.Text = txtFunarpen.Text;
                winDigitarEscritura.txtPmcmv.Text = txtPmcmv.Text;
                winDigitarEscritura.txtIss.Text = txtIss.Text;
                winDigitarEscritura.txtDistribuicao.Text = txtDistribuicao.Text;

                if (Principal.TipoAto != "APOSTILAMENTO HAIA")
                {
                    txtTotal.Text = winDigitarEscritura.CalcularTotal("0,00", "0,00", winDigitarEscritura.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
                    winDigitarEscritura.txtTotal.Text = winDigitarEscritura.CalcularTotal(winDigitarEscritura.txtAdicionar.Text, winDigitarEscritura.txtDesconto.Text, winDigitarEscritura.txtEmol.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
                    winDigitarEscritura.lblTotal.Content = string.Format("{0}", winDigitarEscritura.txtTotal.Text);
                    winDigitarEscritura.lblTotal1.Content = winDigitarEscritura.lblTotal.Content;
                }
                else
                {
                    txtTotal.Text = winDigitarEscritura.CalcularTotal("0,00", "0,00", Semol, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
                    winDigitarEscritura.txtTotal.Text = winDigitarEscritura.CalcularTotal(winDigitarEscritura.txtAdicionar.Text, winDigitarEscritura.txtDesconto.Text, txtTotal.Text, txtFetj.Text, txtFundperj.Text, txtFunperj.Text, txtFunarpen.Text, txtPmcmv.Text, txtIss.Text, txtIndisp.Text, txtDistribuicao.Text, txtMutua.Text, txtAcoterj.Text, winDigitarEscritura.txtEnotariado.Text);
                    winDigitarEscritura.lblTotal.Content = string.Format("{0}", winDigitarEscritura.txtTotal.Text);
                    winDigitarEscritura.lblTotal1.Content = winDigitarEscritura.lblTotal.Content;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void CalcularItensCustas()
        {
            ItensCustasNota novoIten;
            novoIten = new ItensCustasNota();
            var arqrivDesarquiv = (CustasNota)dataGridItens.SelectedItem;
            novoIten.Item = arqrivDesarquiv.ITEM;
            novoIten.SubItem = arqrivDesarquiv.SUB;
            novoIten.Tabela = arqrivDesarquiv.TAB;
            novoIten.Descricao = arqrivDesarquiv.TEXTO;
            novoIten.Complemento = (!(arqrivDesarquiv.TAB == "22" && arqrivDesarquiv.ITEM == "NI" && arqrivDesarquiv.SUB == "20")) ? arqrivDesarquiv.VAI : null;

            if (txtQtdItens.Text != "")
                novoIten.Quantidade = txtQtdItens.Text;
            else
                novoIten.Quantidade = "1";

            novoIten.Valor = arqrivDesarquiv.VALOR;
            novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);

            var itemExistente = winDigitarEscritura.listaItens.Where(p => p.Descricao == novoIten.Descricao && p.Item == novoIten.Item && p.Tabela == novoIten.Tabela && p.SubItem == novoIten.SubItem).FirstOrDefault();

            if (itemExistente == null)
                winDigitarEscritura.listaItens.Add(novoIten);
            else
            {
                var qtd = Convert.ToInt32(novoIten.Quantidade) + Convert.ToInt32(itemExistente.Quantidade);

                itemExistente.Quantidade = qtd.ToString();
                itemExistente.Total = itemExistente.Valor * qtd;
            }


            if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarEscritura.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValores(txtDistribuicao.Text);
        }

        private void CalcularItensCustasControleAtos()
        {
            ItensCustasControleAtosNota novoIten;
            novoIten = new ItensCustasControleAtosNota();
            var arqrivDesarquiv = (CustasNota)dataGridItens.SelectedItem;
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
            winDigitarControleAtosNotas.listaItens.Add(novoIten);


            if (winDigitarControleAtosNotas.ckbGratuito.IsChecked == false)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarControleAtosNotas.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValoresControleAtos();
        }


        private void CalcularValoresControleAtos()
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

            emol = Convert.ToDecimal(winDigitarControleAtosNotas.listaItens.Sum(p => p.Total));
            try
            {

                fetj_20 = emol * 20 / 100;
                fundperj_5 = emol * 5 / 100;
                funperj_5 = emol * 5 / 100;
                funarpen_4 = emol * 4 / 100;
                //iss = (100 - winDigitarControleAtosNotas.porcentagemIss) / 100;
                //iss = emol / iss - emol;
                //iss = emol * winDigitarControleAtosNotas.porcentagemIss / 100;

                iss = classCustasNotas.CalcularISSNotas(emol, true, winDigitarEscritura.porcentagemIss);

                pmcmv_2 = Convert.ToDecimal(winDigitarControleAtosNotas.emolumentos.VALOR * 2) / 100;


                if (winDigitarControleAtosNotas.ckbGratuito.IsChecked == false)
                {
                    Semol = Convert.ToString(emol);
                }
                else
                {
                    Semol = "0,00";
                }


                Sfetj_20 = Convert.ToString(fetj_20);
                Sfundperj_5 = Convert.ToString(fundperj_5);
                Sfunperj_5 = Convert.ToString(funperj_5);
                Sfunarpen_4 = Convert.ToString(funarpen_4);
                Siss = Convert.ToString(iss);
                Spmcmv_2 = Convert.ToString(pmcmv_2);






                if (winDigitarControleAtosNotas.ckbGratuito.IsChecked == true)
                {

                    emol = 0;
                    fetj_20 = 0;
                    fundperj_5 = 0;
                    funperj_5 = 0;
                    funarpen_4 = 0;
                    iss = 0;
                    if (Principal.TipoAto != "CERTIDÃO NOTAS")
                        pmcmv_2 = 0;

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

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);


                index = Spmcmv_2.IndexOf(',');
                Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(Semol));
                txtFetj.Text = string.Format("{0:n2}", Convert.ToDecimal(Sfetj_20));
                txtFundperj.Text = string.Format("{0:n2}", Convert.ToDecimal(Sfundperj_5));
                txtFunperj.Text = string.Format("{0:n2}", Convert.ToDecimal(Sfundperj_5));
                txtFunarpen.Text = string.Format("{0:n2}", Convert.ToDecimal(Sfunarpen_4));
                txtPmcmv.Text = string.Format("{0:n2}", Convert.ToDecimal(Spmcmv_2));
                txtIss.Text = string.Format("{0:n2}", Convert.ToDecimal(Siss));

                winDigitarControleAtosNotas.txtEmol.Text = txtEmol.Text;
                winDigitarControleAtosNotas.txtFetj.Text = txtFetj.Text;
                winDigitarControleAtosNotas.txtFundperj.Text = txtFundperj.Text;
                winDigitarControleAtosNotas.txtFunperj.Text = txtFundperj.Text;
                winDigitarControleAtosNotas.txtFunarpen.Text = txtFunarpen.Text;
                winDigitarControleAtosNotas.txtPmcmv.Text = txtPmcmv.Text;
                winDigitarControleAtosNotas.txtIss.Text = txtIss.Text;

                if (winDigitarControleAtosNotas.ckbGratuito.IsChecked == false)
                {
                    txtMutua.Text = string.Format("{0:N2}", (Convert.ToInt16(winDigitarControleAtosNotas.txtQtdAtos.Text) * winDigitarControleAtosNotas.mutua));
                    txtAcoterj.Text = string.Format("{0:N2}", (Convert.ToInt16(winDigitarControleAtosNotas.txtQtdAtos.Text) * winDigitarControleAtosNotas.acoterj));

                    winDigitarControleAtosNotas.txtMutua.Text = string.Format("{0:N2}", (Convert.ToInt16(winDigitarControleAtosNotas.txtQtdAtos.Text) * winDigitarControleAtosNotas.mutua));
                    winDigitarControleAtosNotas.txtAcoterj.Text = string.Format("{0:N2}", (Convert.ToInt16(winDigitarControleAtosNotas.txtQtdAtos.Text) * winDigitarControleAtosNotas.acoterj));
                }
                else
                {
                    txtMutua.Text = "0,00";
                    txtAcoterj.Text = "0,00";

                    winDigitarControleAtosNotas.txtMutua.Text = "0,00";
                    winDigitarControleAtosNotas.txtAcoterj.Text = "0,00";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (chamada == "winDigitarEscritura")
            {
                CalcularItensCustas();
                dataGridItensSelecionados.ItemsSource = winDigitarEscritura.listaItens;
                dataGridItensSelecionados.Items.Refresh();
                txtQtdItens.Text = "1";
            }
            else
            {
                CalcularItensCustasControleAtos();
                dataGridItensSelecionados.ItemsSource = winDigitarControleAtosNotas.listaItens;
                dataGridItensSelecionados.Items.Refresh();
                txtQtdItens.Text = "1";
            }
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
            if (chamada == "winDigitarEscritura")
            {
                if (winDigitarEscritura.listaItens[0] != (ItensCustasNota)dataGridItensSelecionados.SelectedItem)
                {
                    RemoverItem();
                    dataGridItensSelecionados.ItemsSource = winDigitarEscritura.listaItens;
                    dataGridItensSelecionados.Items.Refresh();
                }
            }
            else
            {
                if (winDigitarControleAtosNotas.listaItens[0] != (ItensCustasControleAtosNota)dataGridItensSelecionados.SelectedItem)
                {
                    RemoverItemControleAtos();
                    dataGridItensSelecionados.ItemsSource = winDigitarControleAtosNotas.listaItens;
                    dataGridItensSelecionados.Items.Refresh();
                }
            }
        }


        private void RemoverItem()
        {

            var item = (ItensCustasNota)dataGridItensSelecionados.SelectedItem;

            winDigitarEscritura.listaItens.Remove(item);


            if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarEscritura.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValores(txtDistribuicao.Text);
        }


        private void RemoverItemControleAtos()
        {

            var item = (ItensCustasControleAtosNota)dataGridItensSelecionados.SelectedItem;

            winDigitarControleAtosNotas.listaItens.Remove(item);


            if (winDigitarControleAtosNotas.ckbGratuito.IsChecked == false)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarControleAtosNotas.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValoresControleAtos();
        }

        private void LimparGrid()
        {
            int qtdGrid = dataGridItensSelecionados.Items.Count - 1;
            for (int cont = qtdGrid; cont >= 1; cont--)
            {
                var item = (ItensCustasNota)winDigitarEscritura.listaItens[cont];

                winDigitarEscritura.listaItens.Remove(item);
            }

            if (winDigitarEscritura.cmbTipoCustas.SelectedIndex == 0)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarEscritura.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValores(txtDistribuicao.Text);
        }

        private void LimparGridControleAtos()
        {
            int qtdGrid = dataGridItensSelecionados.Items.Count - 1;
            for (int cont = qtdGrid; cont >= 1; cont--)
            {
                var item = (ItensCustasControleAtosNota)winDigitarControleAtosNotas.listaItens[cont];

                winDigitarControleAtosNotas.listaItens.Remove(item);
            }

            if (winDigitarControleAtosNotas.ckbGratuito.IsChecked == false)
            {
                txtEmol.Text = string.Format("{0:n2}", Convert.ToDecimal(winDigitarControleAtosNotas.listaItens.Sum(p => p.Total)));
            }
            else
            {
                txtEmol.Text = "0,00";
            }

            CalcularValoresControleAtos();
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            if (chamada == "winDigitarEscritura")
            {
                LimparGrid();
                dataGridItensSelecionados.ItemsSource = winDigitarEscritura.listaItens;
                dataGridItensSelecionados.Items.Refresh();
            }
            else
            {
                LimparGridControleAtos();
                dataGridItensSelecionados.ItemsSource = winDigitarControleAtosNotas.listaItens;
                dataGridItensSelecionados.Items.Refresh();
            }
        }

        private void dataGridItens_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (chamada == "winDigitarEscritura")
            {
                CalcularItensCustas();
                dataGridItensSelecionados.ItemsSource = winDigitarEscritura.listaItens;
                dataGridItensSelecionados.Items.Refresh();
                txtQtdItens.Text = "1";
            }
            else
            {
                CalcularItensCustasControleAtos();
                dataGridItensSelecionados.ItemsSource = winDigitarControleAtosNotas.listaItens;
                dataGridItensSelecionados.Items.Refresh();
                txtQtdItens.Text = "1";
            }
        }

        private void dataGridItensSelecionados_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;

            if (chamada == "winDigitarEscritura")
            {
                if (key == 32)
                {
                    if (winDigitarEscritura.listaItens[0] != (ItensCustasNota)dataGridItensSelecionados.SelectedItem)
                    {
                        RemoverItem();
                        dataGridItensSelecionados.ItemsSource = winDigitarEscritura.listaItens;
                        dataGridItensSelecionados.Items.Refresh();
                    }
                }
            }
            else
            {
                if (key == 32)
                {
                    if (winDigitarControleAtosNotas.listaItens[0] != (ItensCustasControleAtosNota)dataGridItensSelecionados.SelectedItem)
                    {
                        RemoverItemControleAtos();
                        dataGridItensSelecionados.ItemsSource = winDigitarControleAtosNotas.listaItens;
                        dataGridItensSelecionados.Items.Refresh();
                    }
                }
            }
        }

        private void dataGridItensSelecionados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (chamada == "winDigitarEscritura")
            {
                WinAlterarItemCustas winAlterarItemCustas = new WinAlterarItemCustas(this);
                winAlterarItemCustas.Owner = this;
                winAlterarItemCustas.ShowDialog();

                dataGridItensSelecionados.Items.Refresh();

                CalcularValores(txtDistribuicao.Text);
            }
            else
            {
                WinAlterarItemCustas winAlterarItemCustas = new WinAlterarItemCustas(this);
                winAlterarItemCustas.Owner = this;
                winAlterarItemCustas.ShowDialog();

                dataGridItensSelecionados.Items.Refresh();

                CalcularValoresControleAtos();
            }
        }

        private void dataGridItensSelecionados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chamada == "winDigitarEscritura")
            {
                itemSelecionado = (ItensCustasNota)dataGridItensSelecionados.SelectedItem;
            }
            else
            {
                itemSelecionadoControleAtosNota = (ItensCustasControleAtosNota)dataGridItensSelecionados.SelectedItem;
            }
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

            try
            {
                if (txtDistribuicao.Text != "")
                {
                    try
                    {
                        txtDistribuicao.Text = string.Format("{0:n2}", Convert.ToDecimal(txtDistribuicao.Text));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Por favor verifique o valor digitado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    txtDistribuicao.Text = "0,00";
                }

                CalcularValores(txtDistribuicao.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDistribuicao_GotFocus(object sender, RoutedEventArgs e)
        {

        }




    }
}
