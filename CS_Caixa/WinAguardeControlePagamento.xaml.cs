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
using CS_Caixa.Controls;
using CS_Caixa.Models;
using System.ComponentModel;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAguardeControlePagamento.xaml
    /// </summary>
    public partial class WinAguardeControlePagamento : Window
    {

        WinControlePagamento controlePagamento;

        List<Ato> atos = new List<Ato>();
        List<ValorPago> valoresPagos = new List<ValorPago>();
        List<ReciboBalcao> recibosBalcao = new List<ReciboBalcao>();

        DateTime _dataInicio, _dataFim;

        List<AtosValores> atosValores = new List<AtosValores>();

        List<Adicionar_Caixa> valoresAdicionados = new List<Adicionar_Caixa>();

        List<Retirada_Caixa> valoresRetirados = new List<Retirada_Caixa>();

        public WinAguardeControlePagamento(DateTime dataInicio, DateTime dataFim, WinControlePagamento controlePagamento)
        {
            _dataInicio = dataInicio;
            _dataFim = dataFim;
            this.controlePagamento = controlePagamento;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
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

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void Processo()
        {
            ClassAto classAto = new ClassAto();

            atos = classAto.ListarTodosOsAtosPorData(_dataInicio, _dataFim);
            valoresPagos = classAto.ObterValorPagoPorData(_dataInicio, _dataFim);

            ClassBalcao classBalcao = new ClassBalcao();

            recibosBalcao = classBalcao.ListarTodosPorData(_dataInicio, _dataFim);


            AtosValores atoValor;

            foreach (var item in atos)
            {


                if (item.Atribuicao != "BALCÃO")
                {
                    var valorPago = valoresPagos.Where(p => p.IdAto == item.Id_Ato).FirstOrDefault();

                    atoValor = new AtosValores();

                    if (valorPago != null)
                    {
                        switch (item.TipoPagamento)
                        {
                            case "DINHEIRO":
                                atoValor.Dinheiro = valorPago.Dinheiro - valorPago.Troco;
                                break;
                            case "DEPÓSITO":
                                atoValor.Deposito = valorPago.Deposito - valorPago.Troco;
                                break;
                            case "MENSALISTA":
                                atoValor.VrMensalista = valorPago.Mensalista - valorPago.Troco;
                                break;
                            case "CHEQUE":
                                atoValor.Cheque = valorPago.Cheque - valorPago.Troco;
                                break;
                            case "PIX BRADESCO":
                                atoValor.ChequePre = valorPago.ChequePre - valorPago.Troco;
                                break;
                            case "PIX NUBANK":
                                atoValor.Boleto = valorPago.Boleto - valorPago.Troco;
                                break;
                            case "CARTÃO CRÉDITO":
                                atoValor.CartaoCredito = valorPago.CartaoCredito - valorPago.Troco;
                                break;
                            default:

                                if (item.TipoAto == "PAGAMENTO" || item.TipoAto == "EMISSÃO DE GUIA")
                                    atoValor.Deposito = valorPago.Deposito - valorPago.Troco;
                                else
                                    atoValor.Total = valorPago.Total - valorPago.Troco;


                                break;
                        }
                    }
                    else
                    {
                        atoValor.Total = item.Total;
                    }
                    atoValor.ValorTroco = valorPago.Troco;
                    atoValor.Acoterj = item.Acoterj;
                    atoValor.Atribuicao = item.Atribuicao;
                    atoValor.Bancaria = item.Bancaria;
                    atoValor.Convenio = item.Convenio;
                    atoValor.DataAto = item.DataAto;
                    atoValor.DataPagamento = item.DataPagamento;
                    atoValor.Distribuicao = item.Distribuicao;
                    atoValor.Emolumentos = item.Emolumentos;
                    atoValor.Escrevente = item.Escrevente;
                    atoValor.Fetj = item.Fetj;
                    atoValor.TipoAto = item.TipoAto;
                    atoValor.Livro = item.Livro;
                    atoValor.FolhaFinal = item.FolhaFinal;
                    atoValor.FolhaInical = item.FolhaInical;
                    atoValor.NumeroAto = item.NumeroAto;
                    atoValor.Faixa = item.Faixa;
                    atoValor.Funarpen = item.Funarpen;
                    atoValor.Fundperj = item.Fundperj;
                    atoValor.Funperj = item.Funperj;
                    atoValor.IdReciboBalcao = item.Id_Ato;
                    atoValor.IdUsuario = item.IdUsuario;
                    atoValor.Indisponibilidade = item.Indisponibilidade;
                    atoValor.Iss = item.Iss;
                    atoValor.LetraSelo = item.LetraSelo;
                    atoValor.Mensalista = item.Mensalista;
                    atoValor.Mutua = item.Mutua;
                    atoValor.Natureza = item.Natureza;
                    atoValor.NumeroRequisicao = item.NumeroRequisicao;
                    atoValor.NumeroSelo = item.NumeroSelo;
                    atoValor.Pago = item.Pago;
                    atoValor.Pmcmv = item.Pmcmv;
                    atoValor.Portador = item.Portador;
                    atoValor.Prenotacao = item.Prenotacao;
                    atoValor.Protocolo = item.Protocolo;
                    atoValor.QtdAtos = item.QtdAtos;
                    atoValor.QuantDistrib = item.QuantDistrib;
                    atoValor.QuantIndisp = item.QuantIndisp;
                    atoValor.QuantPrenotacao = item.QuantPrenotacao;
                    atoValor.Recibo = item.Recibo;
                    atoValor.TipoCobranca = item.TipoCobranca;

                    if (item.TipoAto == "PAGAMENTO" || item.TipoAto == "EMISSÃO DE GUIA")
                        atoValor.TipoPagamento = "DEPÓSITO";
                    else
                        atoValor.TipoPagamento = item.TipoPagamento;


                    atoValor.TipoPrenotacao = item.TipoPrenotacao;
                    atoValor.Usuario = item.Usuario;
                    atoValor.ValorAdicionar = item.ValorAdicionar;
                    atoValor.ValorCorretor = item.ValorCorretor;
                    atoValor.ValorDesconto = item.ValorDesconto;
                    atoValor.ValorEscrevente = item.ValorEscrevente;
                    atoValor.ValorPago = item.ValorPago;
                    atoValor.ValorTitulo = item.ValorTitulo;


                    atosValores.Add(atoValor);
                }

            }


            foreach (var item in recibosBalcao)
            {

                var valorPago = valoresPagos.Where(p => p.IdReciboBalcao == item.IdReciboBalcao).FirstOrDefault();

                atoValor = new AtosValores();


                if (valorPago != null)
                {
                    switch (item.TipoPagamento)
                    {
                        case "DINHEIRO":
                            atoValor.Dinheiro = valorPago.Dinheiro - valorPago.Troco;
                            break;
                        case "DEPÓSITO":
                            atoValor.Deposito = valorPago.Deposito - valorPago.Troco;
                            break;
                        case "MENSALISTA":
                            atoValor.VrMensalista = valorPago.Mensalista - valorPago.Troco;
                            break;
                        case "CHEQUE":
                            atoValor.Cheque = valorPago.Cheque - valorPago.Troco;
                            break;
                        case "PIX BRADESCO":
                            atoValor.ChequePre = valorPago.ChequePre - valorPago.Troco;
                            break;
                        case "PIX NUBANK":
                            atoValor.Boleto = valorPago.Boleto - valorPago.Troco;
                            break;
                        case "CARTÃO CRÉDITO":
                            atoValor.CartaoCredito = valorPago.CartaoCredito - valorPago.Troco;
                            break;
                        default:
                            atoValor.Total = valorPago.Total - valorPago.Troco;
                            break;
                    }
                }
                else
                {
                    atoValor.Total = item.Total;
                }
                atoValor.ValorTroco = valorPago.Troco;
                atoValor.Acoterj = item.Acoterj;
                atoValor.Atribuicao = "BALCÃO";

                if (item.Data != null)
                {
                    atoValor.DataAto = Convert.ToDateTime(item.Data);
                    atoValor.DataPagamento = Convert.ToDateTime(item.Data);
                }
                atoValor.Emolumentos = item.Emolumentos;
                atoValor.Escrevente = item.Usuario;
                atoValor.Fetj = item.Fetj;
                atoValor.Funarpen = item.Funarpen;
                atoValor.Fundperj = item.Fundperj;
                atoValor.Funperj = item.Funperj;
                atoValor.IdReciboBalcao = item.IdReciboBalcao;
                if (item.IdUsuario != null)
                    atoValor.IdUsuario = Convert.ToInt32(item.IdUsuario);
                atoValor.Iss = item.Iss;
                atoValor.Mensalista = item.Mensalista;
                atoValor.Mutua = item.Mutua;
                atoValor.NumeroRequisicao = item.NumeroRequisicao;
                atoValor.Pago = item.Pago;
                atoValor.Pmcmv = item.Pmcmv;
                atoValor.Recibo = item.NumeroRecibo;
                atoValor.TipoCobranca = item.TipoCustas;
                atoValor.TipoPagamento = item.TipoPagamento;
                atoValor.Usuario = item.Usuario;
                atoValor.ValorAdicionar = item.ValorAdicionar;
                atoValor.ValorDesconto = item.ValorDesconto;
                atoValor.ValorPago = item.ValorPago;
                atoValor.QuantAbert = item.QuantAbert;
                atoValor.QuantAut = item.QuantAut;
                atoValor.QuantCopia = item.QuantCopia;
                atoValor.QuantMaterializacao = item.QuantMaterializacao;
                atoValor.QuantRecAut = item.QuantRecAut;
                atoValor.QuantRecSem = item.QuantRecSem;

                atosValores.Add(atoValor);
            }


            int cont = _dataFim.Subtract(_dataInicio).Days + 1;


            for (int d = 0; d < cont; d++)
            {



                if (atosValores.Where(p => p.DataPagamento == _dataInicio.AddDays(d)).Count() > 0)
                {

                    ClassAdicionarCaixa adicionarCaixa = new ClassAdicionarCaixa();
                    valoresAdicionados = adicionarCaixa.ListaTodosPorData(_dataInicio.AddDays(d), _dataInicio.AddDays(d));

                    ClassRetiradaCaixa classRetirada = new ClassRetiradaCaixa();
                    valoresRetirados = classRetirada.ListaRetiradaCaixaData(_dataInicio.AddDays(d), _dataInicio.AddDays(d));


                    decimal totalNotasDinheiro = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "DINHEIRO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Dinheiro));
                    decimal totalNotasDeposito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "DEPÓSITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Deposito));
                    decimal totalNotasMensalista = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "MENSALISTA" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.VrMensalista));
                    decimal totalNotasCheque = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "CHEQUE" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Cheque));
                    decimal totalNotasChequePre = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "PIX BRADESCO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.ChequePre));
                    decimal totalNotasBoleto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "PIX NUBANK" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Boleto));
                    decimal totalNotasCartaoCredito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "CARTÃO CRÉDITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.CartaoCredito));
                    decimal totalNotasMisto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "NOTAS" && p.TipoPagamento == "PG MISTO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Total));


                    decimal totalBalcaoDinheiro = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "DINHEIRO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Dinheiro));
                    decimal totalBalcaoDeposito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "DEPÓSITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Deposito));
                    decimal totalBalcaoMensalista = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "MENSALISTA" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.VrMensalista));
                    decimal totalBalcaoCheque = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "CHEQUE" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Cheque));
                    decimal totalBalcaoChequePre = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "PIX BRADESCO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.ChequePre));
                    decimal totalBalcaoBoleto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "PIX NUBANK" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Boleto));
                    decimal totalBalcaoCartaoCredito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "CARTÃO CRÉDITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.CartaoCredito));
                    decimal totalBalcaoMisto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "BALCÃO" && p.TipoPagamento == "PG MISTO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Total));


                    decimal totalProtestoDinheiro = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "DINHEIRO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Dinheiro));
                    decimal totalProtestoDeposito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "DEPÓSITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Deposito));
                    decimal totalProtestoMensalista = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "MENSALISTA" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.VrMensalista));
                    decimal totalProtestoCheque = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "CHEQUE" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Cheque));
                    decimal totalProtestoChequePre = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "PIX BRADESCO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.ChequePre));
                    decimal totalProtestoBoleto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "PIX NUBANK" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Boleto));
                    decimal totalProtestoCartaoCredito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "CARTÃO CRÉDITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.CartaoCredito));
                    decimal totalProtestoMisto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "PROTESTO" && p.TipoPagamento == "PG MISTO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Total));


                    decimal totalRgiDinheiro = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "DINHEIRO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Dinheiro));
                    decimal totalRgiDeposito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "DEPÓSITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Deposito));
                    decimal totalRgiMensalista = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "MENSALISTA" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.VrMensalista));
                    decimal totalRgiCheque = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "CHEQUE" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Cheque));
                    decimal totalRgiChequePre = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "PIX BRADESCO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.ChequePre));
                    decimal totalRgiBoleto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "PIX NUBANK" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Boleto));
                    decimal totalRgiCartaoCredito = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "CARTÃO CRÉDITO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.CartaoCredito));
                    decimal totalRgiMisto = Convert.ToDecimal(atosValores.Where(p => p.Atribuicao == "RGI" && p.TipoPagamento == "PG MISTO" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Total));


                    decimal totalDut = Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Total)) - (Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Emolumentos)) + Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Fetj)) + Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Fundperj)) + Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Funperj)) + Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Funarpen)) + Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Pmcmv)) + Convert.ToDecimal(atos.Where(p => p.Natureza == "REC AUTENTICIDADE (DUT)" && p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Iss)));
                    decimal totalBib = Convert.ToDecimal(atosValores.Where(p => p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Indisponibilidade));
                    decimal totalDistrib = Convert.ToDecimal(atosValores.Where(p => p.DataPagamento == _dataInicio.AddDays(d)).Sum(p => p.Distribuicao));



                    if (controlePagamento.tela == "credito")
                    {

                        List<ControlePagamentoCredito> ListaCredito = new List<ControlePagamentoCredito>();
                        ClassControlePagamento classControlePagamento = new ClassControlePagamento();


                        ListaCredito = classControlePagamento.ListarCredito(_dataInicio.AddDays(d), _dataInicio.AddDays(d));

                        for (int i = 0; i < ListaCredito.Count; i++)
                        {
                            if (ListaCredito[i].Importado == true)
                            {
                                classControlePagamento.ExcluirCredito(ListaCredito[i]);
                            }


                        }



                        if (totalNotasDinheiro > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DINHEIRO NOTAS";

                                credito.Importado = true;

                                credito.Valor = totalNotasDinheiro;

                                credito.TipoCredito = "DINHEIRO";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalNotasDeposito > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DEPÓSITO NOTAS";

                                credito.Valor = totalNotasDeposito;

                                credito.Importado = true;

                                credito.TipoCredito = "DEPÓSITO";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (totalNotasMensalista > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL MENSALISTA NOTAS";

                                credito.Valor = totalNotasMensalista;

                                credito.Importado = true;

                                credito.TipoCredito = "MENSALISTA";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (totalNotasCheque > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CHEQUE NOTAS";

                                credito.Valor = totalNotasCheque;

                                credito.Importado = true;

                                credito.TipoCredito = "CHEQUE";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalNotasChequePre > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX BRADESCO NOTAS";

                                credito.Valor = totalNotasChequePre;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX BRADESCO";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalNotasBoleto > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX NUBANK NOTAS";

                                credito.Valor = totalNotasBoleto;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX NUBANK";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalNotasCartaoCredito > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CARTÃO CRÉDITO NOTAS";

                                credito.Valor = totalNotasCartaoCredito;

                                credito.Importado = true;

                                credito.TipoCredito = "CARTÃO CRÉDITO";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }



                        if (totalNotasMisto > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PG MISTO NOTAS";

                                credito.Valor = totalNotasMisto;

                                credito.Importado = true;

                                credito.TipoCredito = "PG MISTO";

                                credito.Origem = "NOTAS";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (totalBalcaoDinheiro > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {


                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DINHEIRO BALCÃO";

                                credito.Importado = true;

                                credito.Valor = totalBalcaoDinheiro;

                                credito.TipoCredito = "DINHEIRO";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoDeposito > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DEPÓSITO BALCÃO";

                                credito.Valor = totalBalcaoDeposito;

                                credito.Importado = true;

                                credito.TipoCredito = "DEPÓSITO";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoMensalista > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL MENSALISTA BALCÃO";

                                credito.Valor = totalBalcaoMensalista;

                                credito.Importado = true;

                                credito.TipoCredito = "MENSALISTA";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoCheque > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CHEQUE BALCÃO";

                                credito.Valor = totalBalcaoCheque;

                                credito.Importado = true;

                                credito.TipoCredito = "CHEQUE";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoChequePre > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX BRADESCO BALCÃO";

                                credito.Valor = totalBalcaoChequePre;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX BRADESCO";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoBoleto > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX NUBANK BALCÃO";

                                credito.Valor = totalBalcaoBoleto;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX NUBANK";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoCartaoCredito > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CARTÃO CRÉDITO BALCÃO";

                                credito.Valor = totalBalcaoCartaoCredito;

                                credito.Importado = true;

                                credito.TipoCredito = "CARTÃO CRÉDITO";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalBalcaoMisto > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PG MISTO BALCÃO";

                                credito.Valor = totalBalcaoMisto;

                                credito.Importado = true;

                                credito.TipoCredito = "PG MISTO";

                                credito.Origem = "BALCÃO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }



                        if (totalProtestoDinheiro > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {


                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DINHEIRO PROTESTO";

                                credito.Importado = true;

                                credito.Valor = totalProtestoDinheiro;

                                credito.TipoCredito = "DINHEIRO";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalProtestoDeposito > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DEPÓSITO PROTESTO";

                                credito.Valor = totalProtestoDeposito;

                                credito.Importado = true;

                                credito.TipoCredito = "DEPÓSITO";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalProtestoMensalista > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL MENSALISTA PROTESTO";

                                credito.Valor = totalProtestoMensalista;

                                credito.Importado = true;

                                credito.TipoCredito = "MENSALISTA";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalProtestoCheque > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CHEQUE PROTESTO";

                                credito.Valor = totalProtestoCheque;

                                credito.Importado = true;

                                credito.TipoCredito = "CHEQUE";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalProtestoChequePre > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX BRADESCO PROTESTO";

                                credito.Valor = totalProtestoChequePre;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX BRADESCO";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalProtestoBoleto > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX NUBANK PROTESTO";

                                credito.Valor = totalProtestoBoleto;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX NUBANK";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalProtestoCartaoCredito > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CARTÃO CRÉDITO PROTESTO";

                                credito.Valor = totalProtestoCartaoCredito;

                                credito.Importado = true;

                                credito.TipoCredito = "CARTÃO CRÉDITO";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (totalProtestoMisto > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PG MISTO PROTESTO";

                                credito.Valor = totalProtestoMisto;

                                credito.Importado = true;

                                credito.TipoCredito = "PG MISTO";

                                credito.Origem = "PROTESTO";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (totalRgiDinheiro > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {
                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DINHEIRO RGI";

                                credito.Importado = true;

                                credito.Valor = totalRgiDinheiro;

                                credito.TipoCredito = "DINHEIRO";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalRgiDeposito > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL DEPÓSITO RGI";

                                credito.Valor = totalRgiDeposito;

                                credito.Importado = true;

                                credito.TipoCredito = "DEPÓSITO";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalRgiMensalista > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL MENSALISTA RGI";

                                credito.Valor = totalRgiMensalista;

                                credito.Importado = true;

                                credito.TipoCredito = "MENSALISTA";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalRgiCheque > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CHEQUE RGI";

                                credito.Valor = totalRgiCheque;

                                credito.Importado = true;

                                credito.TipoCredito = "CHEQUE";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalRgiChequePre > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX BRADESCO RGI";

                                credito.Valor = totalRgiChequePre;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX BRADESCO";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalRgiBoleto > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PIX NUBANK RGI";

                                credito.Valor = totalRgiBoleto;

                                credito.Importado = true;

                                credito.TipoCredito = "PIX NUBANK";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }

                        if (totalRgiCartaoCredito > 0)
                        {
                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL CARTÃO CRÉDITO RGI";

                                credito.Valor = totalRgiCartaoCredito;

                                credito.Importado = true;

                                credito.TipoCredito = "CARTÃO CRÉDITO";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (totalRgiMisto > 0)
                        {


                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {

                                credito.Data = _dataInicio.AddDays(d);

                                credito.Descricao = "VALOR TOTAL PG MISTO RGI";

                                credito.Valor = totalRgiMisto;

                                credito.Importado = true;

                                credito.TipoCredito = "PG MISTO";

                                credito.Origem = "RGI";

                                classControlePagamento.SalvarCredito(credito, "novo");

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }


                        if (valoresAdicionados.Count > 0)
                        {

                            ControlePagamentoCredito credito = new ControlePagamentoCredito();
                            try
                            {
                                for (int i = 0; i < valoresAdicionados.Count(); i++)
                                {
                                    credito.Data = _dataInicio.AddDays(d);

                                    credito.Descricao = "ADICIONADO AO CAIXA " + valoresAdicionados[i].Descricao;

                                    credito.Valor = Convert.ToDecimal(valoresAdicionados[i].Valor);

                                    credito.Importado = true;

                                    credito.TipoCredito = valoresAdicionados[i].TpPagamento;

                                    credito.Origem = valoresAdicionados[i].Atribuicao;

                                    classControlePagamento.SalvarCredito(credito, "novo");

                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }
                        }



                    }
                    else
                    {
                        List<ControlePagamentoDebito> ListaDebito = new List<ControlePagamentoDebito>();
                        ClassControlePagamento classControlePagamento = new ClassControlePagamento();


                        ListaDebito = classControlePagamento.ListarDebito(_dataInicio.AddDays(d), _dataInicio.AddDays(d));

                        for (int i = 0; i < ListaDebito.Count; i++)
                        {
                            if (ListaDebito[i].Importado == true)
                                classControlePagamento.ExcluirDebito(ListaDebito[i]);

                        }



                        for (int i = 0; i < valoresRetirados.Count; i++)
                        {


                            ControlePagamentoDebito Debito = new ControlePagamentoDebito();
                            try
                            {
                                Debito.Data = _dataInicio.AddDays(d);

                                Debito.Descricao = valoresRetirados[i].Descricao;

                                Debito.Valor = Convert.ToDecimal(valoresRetirados[i].Valor);

                                Debito.TipoDebito = null;

                                Debito.Importado = true;

                                classControlePagamento.SalvarDebito(Debito, "novo", true);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                this.Close();
                            }


                        }

                    }

                }
            }
        }
    }
}

