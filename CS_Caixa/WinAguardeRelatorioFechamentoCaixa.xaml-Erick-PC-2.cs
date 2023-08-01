using CS_Caixa.Controls;
using CS_Caixa.Models;
using CS_Caixa.RelatoriosForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica interna para WinAguardeRelatorioFechamentoCaixa.xaml
    /// </summary>
    public partial class WinAguardeRelatorioFechamentoCaixa : Window
    {
        BackgroundWorker worker;

        List<Ato> atos = new List<Ato>();
        List<Ato> atosDut = new List<Ato>();
        List<ValorPago> valoresPagos = new List<ValorPago>();
        List<ReciboBalcao> recibosBalcao = new List<ReciboBalcao>();
        Ato ato = new Ato();
        DateTime _dataInicio, _dataFim;
        List<AtosValores> atosValores = new List<AtosValores>();
       

        string _tipoRelatorio;

        WinRelatorioFechamentoCaixaGeral _fechamento;

        List<Adicionar_Caixa> valoresAdicionados = new List<Adicionar_Caixa>();

        List<Retirada_Caixa> valoresRetirados = new List<Retirada_Caixa>();

        List<Enotariado> eNotariado = new List<Enotariado>();
       

        public WinAguardeRelatorioFechamentoCaixa(DateTime dataInicio, DateTime dataFim, string tipoRelatorio, WinRelatorioFechamentoCaixaGeral fechamento)
        {
            _tipoRelatorio = tipoRelatorio;
            _dataInicio = dataInicio;
            _dataFim = dataFim;
            _fechamento = fechamento;
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
            ClassAto classAto = new ClassAto();

            atos = classAto.ListarTodosOsAtosPorData(_dataInicio, _dataFim);
            atosDut = atos.Where(p => p.TipoAto == "REC AUTENTICIDADE (DUT)").ToList();
            atos = atos.Where(p => p.TipoAto != "REC AUTENTICIDADE (DUT)").ToList();
            valoresPagos = classAto.ObterValorPagoPorData(_dataInicio, _dataFim);

            ClassBalcao classBalcao = new ClassBalcao();

            recibosBalcao = classBalcao.ListarTodosPorData(_dataInicio, _dataFim);

            ClassAdicionarCaixa adicionarCaixa = new ClassAdicionarCaixa();
            valoresAdicionados = adicionarCaixa.ListaTodosPorData(_dataInicio, _dataFim);

            ClassRetiradaCaixa classRetirada = new ClassRetiradaCaixa();
            valoresRetirados = classRetirada.ListaRetiradaCaixaData(_dataInicio, _dataFim);

            ClassEnotariado classEnotariado = new ClassEnotariado();
            eNotariado = classEnotariado.ObterPorData(_dataInicio, _dataFim);


            ClassCustasNotas custas = new ClassCustasNotas();
            List<CustasNota> listaCustas = custas.ListaCustas();
           

            foreach (var item in atos)
            {
                if (item.Atribuicao != "BALCÃO")
                {
                    var valorPago = valoresPagos.Where(p => p.IdAto == item.Id_Ato).FirstOrDefault();

                    AtosValores atoValor = new AtosValores();

                    if (valorPago != null)
                    {
                        atoValor.Boleto = valorPago.Boleto;
                        atoValor.Cheque = valorPago.Cheque;
                        atoValor.ChequePre = valorPago.ChequePre;
                        atoValor.Deposito = valorPago.Deposito;
                        atoValor.Dinheiro = valorPago.Dinheiro;
                        atoValor.Total = valorPago.Total;
                        atoValor.VrMensalista = valorPago.Mensalista;
                        if (valorPago.CartaoCredito == null)
                            atoValor.CartaoCredito = 0M;
                        else
                            atoValor.CartaoCredito = valorPago.CartaoCredito;
                        atoValor.ValorTroco = valorPago.Troco;
                    }
                    else
                    {
                        atoValor.Total = item.Total;
                    }

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


            foreach (var item in atosDut)
            {
                ato = new Ato();
                ato.Emolumentos = item.Emolumentos;
                ato.Fetj = item.Fetj;
                ato.Fundperj = item.Fundperj;
                ato.Funperj = item.Funperj;
                ato.Funarpen = item.Funarpen;
                ato.Pmcmv = item.Pmcmv;
                ato.Iss = item.Iss;
                ato.Total = ato.Emolumentos + ato.Fetj + ato.Fundperj + ato.Funperj + ato.Funarpen + ato.Pmcmv + ato.Iss;


                ato.Atribuicao = "BALCÃO";
                ato.Bancaria = item.Bancaria;
                ato.Convenio = item.Convenio;
                ato.DataAto = item.DataAto;
                ato.DataPagamento = item.DataPagamento;
                ato.Distribuicao = item.Distribuicao;
                ato.Escrevente = item.Escrevente;
                ato.TipoAto = item.TipoAto;
                ato.Livro = item.Livro;
                ato.FolhaFinal = item.FolhaFinal;
                ato.FolhaInical = item.FolhaInical;
                ato.NumeroAto = item.NumeroAto;
                ato.Faixa = item.Faixa;
                ato.IdReciboBalcao = item.Id_Ato;
                ato.IdUsuario = item.IdUsuario;
                ato.Indisponibilidade = item.Indisponibilidade;
                ato.LetraSelo = item.LetraSelo;
                ato.Mensalista = item.Mensalista;
                ato.Natureza = item.Natureza;
                ato.NumeroRequisicao = item.NumeroRequisicao;
                ato.NumeroSelo = item.NumeroSelo;
                ato.Pago = item.Pago;
                ato.Portador = item.Portador;
                ato.Prenotacao = item.Prenotacao;
                ato.Protocolo = item.Protocolo;
                ato.QtdAtos = item.QtdAtos;
                ato.QuantDistrib = item.QuantDistrib;
                ato.QuantIndisp = item.QuantIndisp;
                ato.QuantPrenotacao = item.QuantPrenotacao;
                ato.Recibo = item.Recibo;
                ato.TipoCobranca = item.TipoCobranca;
                ato.TipoPagamento = item.TipoPagamento;
                ato.TipoPrenotacao = item.TipoPrenotacao;
                ato.Usuario = item.Usuario;
                ato.ValorAdicionar = item.ValorAdicionar;
                ato.ValorCorretor = item.ValorCorretor;
                ato.ValorDesconto = item.ValorDesconto;
                ato.ValorEscrevente = item.ValorEscrevente;
                ato.ValorPago = item.ValorPago;
                ato.ValorTitulo = item.ValorTitulo;
                ato.ValorTroco = item.ValorTroco;

                atos.Add(ato);
                
                                
                decimal dutRegistro = Convert.ToDecimal(listaCustas.SingleOrDefault(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES" && p.ANO == item.DataAto.Year).VALOR);
                decimal rtd = Convert.ToDecimal(listaCustas.SingleOrDefault(p => p.DESCR == "RTD" && p.ANO == item.DataAto.Year).VALOR);
                decimal porcentagemIss = Convert.ToDecimal(listaCustas.SingleOrDefault(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == item.DataAto.Year).VALOR);

                var valores =  ObterValoresDutRegistro(dutRegistro, rtd, porcentagemIss);
                             

                ato = new Ato();
                ato.Emolumentos = valores[0];
                ato.Fetj = valores[1];
                ato.Fundperj = valores[2];
                ato.Funperj = valores[3];
                ato.Funarpen = valores[4];
                ato.Pmcmv = valores[5];
                ato.Iss = valores[6];
                ato.Total = valores[7];

                ato.Atribuicao = "BALCÃO";
                ato.Bancaria = item.Bancaria;
                ato.Convenio = item.Convenio;
                ato.DataAto = item.DataAto;
                ato.DataPagamento = item.DataPagamento;
                ato.Distribuicao = item.Distribuicao;
                ato.Escrevente = item.Escrevente;
                ato.TipoAto = "REGISTRO (DUT)";
                ato.Livro = item.Livro;
                ato.FolhaFinal = item.FolhaFinal;
                ato.FolhaInical = item.FolhaInical;
                ato.NumeroAto = item.NumeroAto;
                ato.Faixa = item.Faixa;
                ato.IdReciboBalcao = item.Id_Ato;
                ato.IdUsuario = item.IdUsuario;
                ato.Indisponibilidade = item.Indisponibilidade;
                ato.LetraSelo = item.LetraSelo;
                ato.Mensalista = item.Mensalista;
                ato.Natureza = item.Natureza;
                ato.NumeroRequisicao = item.NumeroRequisicao;
                ato.NumeroSelo = item.NumeroSelo;
                ato.Pago = item.Pago;
                ato.Portador = item.Portador;
                ato.Prenotacao = item.Prenotacao;
                ato.Protocolo = item.Protocolo;
                ato.QtdAtos = item.QtdAtos;
                ato.QuantDistrib = item.QuantDistrib;
                ato.QuantIndisp = item.QuantIndisp;
                ato.QuantPrenotacao = item.QuantPrenotacao;
                ato.Recibo = item.Recibo;
                ato.TipoCobranca = item.TipoCobranca;
                ato.TipoPagamento = item.TipoPagamento;
                ato.TipoPrenotacao = item.TipoPrenotacao;
                ato.Usuario = item.Usuario;
                ato.ValorAdicionar = item.ValorAdicionar;
                ato.ValorCorretor = item.ValorCorretor;
                ato.ValorDesconto = item.ValorDesconto;
                ato.ValorEscrevente = item.ValorEscrevente;
                ato.ValorPago = item.ValorPago;
                ato.ValorTitulo = item.ValorTitulo;
                ato.ValorTroco = item.ValorTroco;


                atos.Add(ato);



            }


            foreach (var item in recibosBalcao)
            {

                var valorPago = valoresPagos.Where(p => p.IdReciboBalcao == item.IdReciboBalcao).FirstOrDefault();
                
                AtosValores atoValor = new AtosValores();


                if (valorPago != null)
                {
                    atoValor.Boleto = valorPago.Boleto;
                    atoValor.Cheque = valorPago.Cheque;
                    atoValor.ChequePre = valorPago.ChequePre;
                    atoValor.Deposito = valorPago.Deposito;
                    atoValor.Dinheiro = valorPago.Dinheiro;
                    atoValor.Total = valorPago.Total;
                    atoValor.VrMensalista = valorPago.Mensalista;
                    atoValor.ValorTroco = valorPago.Troco;
                    
                    if (valorPago.CartaoCredito == null)
                        atoValor.CartaoCredito = 0M;
                    else
                    atoValor.CartaoCredito = valorPago.CartaoCredito;
                }
                else
                {
                    atoValor.Total = item.Total;
                }

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





        }



        private List<decimal> ObterValoresDutRegistro(decimal dutRegistro, decimal rtd, decimal porcentagemIss)
        {
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

            decimal emol = dutRegistro + rtd;
            try
            {


                fetj_20 = emol * 20 / 100;
                fundperj_5 = emol * 5 / 100;
                funperj_5 = emol * 5 / 100;
                funarpen_4 = emol * 4 / 100;
                pmcmv_2 = rtd * 2 / 100;

                iss = emol * porcentagemIss / 100;

                Semol = Convert.ToString(emol);

                Sfetj_20 = Convert.ToString(fetj_20);
                Sfundperj_5 = Convert.ToString(fundperj_5);
                Sfunperj_5 = Convert.ToString(funperj_5);
                Sfunarpen_4 = Convert.ToString(funarpen_4);
                Spmcmv_2 = Convert.ToString(pmcmv_2);
                Siss = Convert.ToString(iss);

            }
            catch (Exception)
            {
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

            emol = Convert.ToDecimal(Semol);

            fetj_20 = Convert.ToDecimal(Sfetj_20);
            fundperj_5 = Convert.ToDecimal(Sfundperj_5);
            funperj_5 = Convert.ToDecimal(Sfunperj_5);
            funarpen_4 = Convert.ToDecimal(Sfunarpen_4);
            pmcmv_2 = Convert.ToDecimal(Spmcmv_2);
            iss = Convert.ToDecimal(Siss);

            List<decimal> valores = new List<decimal>();

            valores.Add(emol);

            valores.Add(fetj_20);
            valores.Add(fundperj_5);
            valores.Add(funperj_5);
            valores.Add(funarpen_4);
            valores.Add(pmcmv_2);
            valores.Add(iss);
            valores.Add(emol + fetj_20 + fundperj_5 + funperj_5 + funarpen_4 + pmcmv_2 + iss);

            return valores;
        }





        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            this.Close();

            if (atosValores.Count() > 0)
            {
                var relatorio = new WinRelatorioFechamentoCaixa(atosValores, atos, _tipoRelatorio, _dataInicio, _dataFim, valoresAdicionados, valoresRetirados, eNotariado);
                relatorio.Owner = _fechamento;
                relatorio.ShowDialog();
                relatorio.Close();
            }
            else
                MessageBox.Show("Não existe movimento referente a data selecionada.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
