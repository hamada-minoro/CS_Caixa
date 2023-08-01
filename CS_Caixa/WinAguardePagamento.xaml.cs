using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for WinAguardePagamento.xaml
    /// </summary>
    public partial class WinAguardePagamento : Window
    {
        List<ReciboBalcao> _reciboBalcao;
        List<Ato> _notas;
        List<Ato> _protesto;
        List<Ato> _rgi;

        ReciboBalcao reciboPago;

        Ato notas;

        Ato protesto;

        Ato rgi;

        bool _salvarTroco;

        decimal _valorPago;

        string acao = string.Empty;

        ClassBalcao classBalcao = new ClassBalcao();

        ClassAto classAto = new ClassAto();

        BackgroundWorker worker;
        public WinAguardePagamento(List<ReciboBalcao> reciboBalcao, List<Ato> notas, List<Ato> protesto, List<Ato> rgi, bool salvarTroco, decimal valorPago)
        {

            _reciboBalcao = reciboBalcao;
            _notas = notas;
            _protesto = protesto;
            _rgi = rgi;
            _salvarTroco = salvarTroco;
            _valorPago = valorPago;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Processo();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

            if (acao == "Pagando Balcão")
            {
                progressBar1.Maximum = _reciboBalcao.Count;
                label2.Content = string.Format("Alterando Recibo {0} para Pago.", reciboPago.NumeroRecibo);
            }
            if (acao == "Pagando Notas")
            {
                progressBar1.Maximum = _notas.Count;
                label2.Content = string.Format("Alterando {0} / {1} para Pago.", notas.Atribuicao, notas.TipoAto);
            }
            if (acao == "Pagando Protesto")
            {
                progressBar1.Maximum = _protesto.Count;
                label2.Content = string.Format("Alterando {0} / {1} para Pago.", protesto.Atribuicao, protesto.TipoAto);
            }
            if (acao == "Pagando Rgi")
            {
                progressBar1.Maximum = _rgi.Count;
                label2.Content = string.Format("Alterando {0} / {1} para Pago.", rgi.Atribuicao, rgi.TipoAto);
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }


        void Processo()
        {

            if (_reciboBalcao.Count > 0 || _notas.Count > 0 || _protesto.Count > 0 || _rgi.Count > 0)
            {
                try
                {
                    if (_salvarTroco == false)
                    {


                        for (int i = 0; i < _reciboBalcao.Count; i++)
                        {
                            if (_reciboBalcao[i].Pago == true)
                            {
                                acao = "Pagando Balcão";
                                reciboPago = _reciboBalcao[i];
                                _reciboBalcao[i].Pago = true;
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                classBalcao.PagarSelo(_reciboBalcao[i]);
                                classBalcao.PagarRecibo(_reciboBalcao[i]);
                            }
                        }

                        for (int i = 0; i < _notas.Count; i++)
                        {
                            if (_notas[i].Pago == true)
                            {
                                acao = "Pagando Notas";
                                _notas[i].Pago = true;
                                notas = _notas[i];
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);


                                classAto.SalvarAto(_notas[i], "alterar");
                            }
                        }

                        for (int i = 0; i < _protesto.Count; i++)
                        {
                            if (_protesto[i].Pago == true)
                            {
                                acao = "Pagando Protesto";
                                _protesto[i].Pago = true;
                                protesto = _protesto[i];
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                classAto.SalvarAto(_protesto[i], "alterar");
                            }
                        }

                        for (int i = 0; i < _rgi.Count; i++)
                        {
                            if (_rgi[i].Pago == true)
                            {
                                acao = "Pagando Rgi";
                                _rgi[i].Pago = true;
                                rgi = _rgi[i];
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                classAto.SalvarAto(_rgi[i], "alterar");
                            }
                        }

                    }
                    else
                    {

                        int idPagamento = classAto.ObterProximoIdPagamentoValorPago();

                        int qtdAtos = _reciboBalcao.Where(p => p.Pago == true).ToList().Count + _notas.Where(p => p.Pago == true).ToList().Count + _protesto.Where(p => p.Pago == true).ToList().Count + _rgi.Where(p => p.Pago == true).ToList().Count;
                        

                        for (int i = 0; i < _reciboBalcao.Count; i++)
                        {
                            if (_reciboBalcao[i].Pago == true)
                            {
                                qtdAtos = qtdAtos - 1;

                                acao = "Pagando Balcão";
                                reciboPago = _reciboBalcao[i];
                                _reciboBalcao[i].Pago = true;
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);

                                string tipoPag = _reciboBalcao[i].TipoPagamento;

                                classBalcao.PagarSelo(_reciboBalcao[i]);
                                classBalcao.PagarRecibo(_reciboBalcao[i]);                                

                                var valorPago = classAto.ObterValorPagoPorIdReciboBalcao(_reciboBalcao[i].IdReciboBalcao);
                                valorPago.IdPagamento = idPagamento;

                                _valorPago = _valorPago - Convert.ToDecimal(valorPago.Total);

                                if (qtdAtos == 0)
                                {
                                    valorPago.Troco = _valorPago;
                                    switch (tipoPag)
                                    {
                                        case "DINHEIRO":
                                            valorPago.Dinheiro = valorPago.Dinheiro + _valorPago;
                                            break;
                                        case "DEPÓSITO":
                                            valorPago.Deposito = valorPago.Deposito + _valorPago;
                                            break;
                                        case "CHEQUE":
                                            valorPago.Cheque = valorPago.Cheque + _valorPago;
                                            break;
                                        case "PIX BRADESCO":
                                            valorPago.ChequePre = valorPago.ChequePre + _valorPago;
                                            break;
                                        case "PIX NUBANK":
                                            valorPago.Boleto = valorPago.Boleto + _valorPago;
                                            break;
                                        case "CARTÃO CRÉDITO":
                                            valorPago.CartaoCredito = valorPago.CartaoCredito + _valorPago;
                                            break;
                                        default:
                                            break;
                                    }


                                }
                                classAto.SalvarValorPago(valorPago, "alterar", "IdReciboBalcao");
                            }
                        }

                        for (int i = 0; i < _notas.Count; i++)
                        {
                            if (_notas[i].Pago == true)
                            {

                                qtdAtos = qtdAtos - 1;

                                acao = "Pagando Notas";
                                _notas[i].Pago = true;
                                notas = _notas[i];
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);
                                string tipoPag = _notas[i].TipoPagamento;

                                classAto.SalvarAto(_notas[i], "alterar");

                                var valorPago = classAto.ObterValorPagoPorIdAto(_notas[i].Id_Ato);
                                valorPago.IdPagamento = idPagamento;

                                _valorPago = _valorPago - Convert.ToDecimal(valorPago.Total);

                                if (qtdAtos == 0)
                                {
                                    valorPago.Troco = _valorPago;
                                    switch (tipoPag)
                                    {
                                        case "DINHEIRO":
                                            valorPago.Dinheiro = valorPago.Dinheiro + _valorPago;
                                            break;
                                        case "DEPÓSITO":
                                            valorPago.Deposito = valorPago.Deposito + _valorPago;
                                            break;
                                        case "CHEQUE":
                                            valorPago.Cheque = valorPago.Cheque + _valorPago;
                                            break;
                                        case "PIX BRADESCO":
                                            valorPago.ChequePre = valorPago.ChequePre + _valorPago;
                                            break;
                                        case "PIX NUBANK":
                                            valorPago.Boleto = valorPago.Boleto + _valorPago;
                                            break;
                                        case "CARTÃO CRÉDITO":
                                            valorPago.CartaoCredito = valorPago.CartaoCredito + _valorPago;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                classAto.SalvarValorPago(valorPago, "alterar", "IdAto");
                            }
                        }

                        for (int i = 0; i < _protesto.Count; i++)
                        {
                            if (_protesto[i].Pago == true)
                            {
                                qtdAtos = qtdAtos - 1;

                                acao = "Pagando Protesto";
                                _protesto[i].Pago = true;
                                protesto = _protesto[i];
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);
                                string tipoPag = _protesto[i].TipoPagamento;

                                classAto.SalvarAto(_protesto[i], "alterar");

                                var valorPago = classAto.ObterValorPagoPorIdAto(_protesto[i].Id_Ato);
                                valorPago.IdPagamento = idPagamento;

                                _valorPago = _valorPago - Convert.ToDecimal(valorPago.Total);

                                if (qtdAtos == 0)
                                {
                                    valorPago.Troco = _valorPago;

                                    switch (tipoPag)
                                    {
                                        case "DINHEIRO":
                                            valorPago.Dinheiro = valorPago.Dinheiro + _valorPago;
                                            break;
                                        case "DEPÓSITO":
                                            valorPago.Deposito = valorPago.Deposito + _valorPago;
                                            break;
                                        case "CHEQUE":
                                            valorPago.Cheque = valorPago.Cheque + _valorPago;
                                            break;
                                        case "PIX BRADESCO":
                                            valorPago.ChequePre = valorPago.ChequePre + _valorPago;
                                            break;
                                        case "PIX NUBANK":
                                            valorPago.Boleto = valorPago.Boleto + _valorPago;
                                            break;
                                        case "CARTÃO CRÉDITO":
                                            valorPago.CartaoCredito = valorPago.CartaoCredito + _valorPago;
                                            break;
                                        default:
                                            break;
                                    }

                                }
                                classAto.SalvarValorPago(valorPago, "alterar", "IdAto");
                            }
                        }

                        for (int i = 0; i < _rgi.Count; i++)
                        {
                            if (_rgi[i].Pago == true)
                            {
                                qtdAtos = qtdAtos - 1;

                                acao = "Pagando Rgi";
                                _rgi[i].Pago = true;
                                rgi = _rgi[i];
                                Thread.Sleep(1);
                                worker.ReportProgress(i + 1);
                                string tipoPag = _rgi[i].TipoPagamento;
                                classAto.SalvarAto(_rgi[i], "alterar");

                                var valorPago = classAto.ObterValorPagoPorIdAto(_rgi[i].Id_Ato);
                                valorPago.IdPagamento = idPagamento;

                                _valorPago = _valorPago - Convert.ToDecimal(valorPago.Total);

                                if (qtdAtos == 0)
                                {
                                    valorPago.Troco = _valorPago;
                                    switch (tipoPag)
                                    {
                                        case "DINHEIRO":
                                            valorPago.Dinheiro = valorPago.Dinheiro + _valorPago;
                                            break;
                                        case "DEPÓSITO":
                                            valorPago.Deposito = valorPago.Deposito + _valorPago;
                                            break;
                                        case "CHEQUE":
                                            valorPago.Cheque = valorPago.Cheque + _valorPago;
                                            break;
                                        case "PIX BRADESCO":
                                            valorPago.ChequePre = valorPago.ChequePre + _valorPago;
                                            break;
                                        case "PIX NUBANK":
                                            valorPago.Boleto = valorPago.Boleto + _valorPago;
                                            break;
                                        case "CARTÃO CRÉDITO":
                                            valorPago.CartaoCredito = valorPago.CartaoCredito + _valorPago;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                classAto.SalvarValorPago(valorPago, "alterar", "IdAto");
                            }
                        }
                    }          
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }
    }
}
