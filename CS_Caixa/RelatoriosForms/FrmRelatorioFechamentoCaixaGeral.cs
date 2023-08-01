using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CS_Caixa.Controls;
using CS_Caixa.Models;


namespace CS_Caixa.RelatoriosForms
{
    public partial class FrmRelatorioFechamentoCaixaGeral : Form
    {

       DateTime dataInicio;
       DateTime dataFim;
        public FrmRelatorioFechamentoCaixaGeral(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
 
            InitializeComponent();
        }

        private void FrmRelatorioFechamentoCaixaGeral_Load(object sender, EventArgs e)
        {
           
            reportViewer1.ProcessingMode = ProcessingMode.Local;


            // CARREGANDO OS PARAMENTROS 

            // variaveis dinheiro
            int qtdEscrituraDinheiro;
            decimal totalEscrituraDinheiro;
            int qtdProcuracaoDinheiro;
            decimal totalProcuracaoDinheiro;
            int qtdCertidaoDinheiro;
            decimal totalCertidaoDinheiro;
            int qtdTestamentoDinheiro;
            decimal totalTestamentoDinheiro;
            int qtdApostilamentoHaiaDinheiro;
            decimal totalApostilamentoHaiaDinheiro;

            int qtdAutenticacaoDinheiro;
            decimal totalAutenticacaoDinheiro;
            int qtdAberturaDinheiro;
            decimal totalAberturaDinheiro;
            int qtdAutenticidadeDinheiro;
            decimal totalAutenticidadeDinheiro;
            int qtdAutenticidadeDutDinheiro;
            decimal totalAutenticidadeDutDinheiro;
            int qtdSemelhancaDinheiro;
            decimal totalSemelhancaDinheiro;            
            int qtdCopiasDinheiro;
            decimal totalCopiasDinheiro;
            int qtdMaterializacaoDinheiro;
            decimal totalMaterializacaoDinheiro;

                       

            int qtdApontamentoDinheiro;
            decimal totalApontamentoDinheiro;
            int qtdCancelamentoDinheiro;
            decimal totalCancelamentoDinheiro;
            int qtdCertidaoProtestoDinheiro;
            decimal totalCertidaoProtestoDinheiro;


            int qtdRegistroDinheiro;
            decimal totalRegistroDinheiro;
            int qtdAverbacaoDinheiro;
            decimal totalAverbacaoDinheiro;
            int qtdCertidaoRgiDinheiro;
            decimal totalCertidaoRgiDinheiro;
            
            
            
            // pagamento de protesto
            int qtdPagamento;
            decimal totalPagamento;

            int qtdTarifa;
            decimal totalTarifa;


            // variaveis deposito
            int qtdEscrituraDeposito;
            decimal totalEscrituraDeposito;
            int qtdProcuracaoDeposito;
            decimal totalProcuracaoDeposito;
            int qtdCertidaoDeposito;
            decimal totalCertidaoDeposito;
            int qtdTestamentoDeposito;
            decimal totalTestamentoDeposito;
            int qtdApostilamentoHaiaDeposito;
            decimal totalApostilamentoHaiaDeposito;

            int qtdAutenticacaoDeposito;
            decimal totalAutenticacaoDeposito;
            int qtdAberturaDeposito;
            decimal totalAberturaDeposito;
            int qtdAutenticidadeDeposito;
            decimal totalAutenticidadeDeposito;
            int qtdAutenticidadeDutDeposito;
            decimal totalAutenticidadeDutDeposito;
            int qtdSemelhancaDeposito;
            decimal totalSemelhancaDeposito;
            int qtdCopiasDeposito;
            decimal totalCopiasDeposito;
            int qtdMaterializacaoDeposito;
            decimal totalMaterializacaoDeposito;


            int qtdApontamentoDeposito;
            decimal totalApontamentoDeposito;
            int qtdCancelamentoDeposito;
            decimal totalCancelamentoDeposito;
            int qtdCertidaoProtestoDeposito;
            decimal totalCertidaoProtestoDeposito;


            int qtdRegistroDeposito;
            decimal totalRegistroDeposito;
            int qtdAverbacaoDeposito;
            decimal totalAverbacaoDeposito;
            int qtdCertidaoRgiDeposito;
            decimal totalCertidaoRgiDeposito;


            decimal TotalAdicionarDinheiro;
            decimal TotalAdicionarDeposito;

            decimal TotalCheque;


            // Executando as consultas
            ClassAto classAto = new ClassAto();
            List<Ato> ato = new List<Ato>();

            ato = classAto.ListarTodosOsAtosPorData(dataInicio, dataFim);

            ClassBalcao classBalcao = new ClassBalcao();
            List<ReciboBalcao> reciboBalcao = new List<ReciboBalcao>();

            reciboBalcao = classBalcao.ListarTodosPorData(dataInicio, dataFim);

            ClassCustasNotas classCustasNotas = new ClassCustasNotas();
            decimal copia = classCustasNotas.ValorCopia(dataInicio.Year);

            ClassAdicionarCaixa classAdicionarCaixa = new ClassAdicionarCaixa();
            List<Adicionar_Caixa> adicionarCaixa = new List<Adicionar_Caixa>();
            adicionarCaixa = classAdicionarCaixa.ListaAdicionarCaixaData(dataInicio, dataFim);

            ClassCadastroCheque classCadastroCheque = new ClassCadastroCheque();
            List<CadCheque> cadCheque = new List<CadCheque>();
            cadCheque = classCadastroCheque.ListarTodosPorData(dataInicio, dataFim);
            


            qtdEscrituraDinheiro =  ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DINHEIRO").Count();
            totalEscrituraDinheiro = Convert.ToDecimal( ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdProcuracaoDinheiro = ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DINHEIRO").Count();
            totalProcuracaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCertidaoDinheiro =  ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DINHEIRO").Count();
            totalCertidaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdTestamentoDinheiro = ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DINHEIRO").Count();
            totalTestamentoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdApostilamentoHaiaDinheiro = ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DINHEIRO").Count();
            totalApostilamentoHaiaDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));


            qtdEscrituraDeposito = ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "ESCRITURA" && p.TipoPagamento == "MENSALISTA").Count();
            totalEscrituraDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "ESCRITURA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdProcuracaoDeposito = ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalProcuracaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoDeposito = ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdTestamentoDeposito = ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalTestamentoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdApostilamentoHaiaDeposito = ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "MENSALISTA").Count();
            totalApostilamentoHaiaDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            qtdAutenticacaoDinheiro = ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DINHEIRO").Count();
            totalAutenticacaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdAberturaDinheiro = ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DINHEIRO").Count();
            totalAberturaDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdAutenticidadeDinheiro = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DINHEIRO").Count();
            totalAutenticidadeDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdAutenticidadeDutDinheiro = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoPagamento == "DINHEIRO").Count();
            totalAutenticidadeDutDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdSemelhancaDinheiro = ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DINHEIRO").Count();
            totalSemelhancaDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCopiasDinheiro = Convert.ToInt32(reciboBalcao.Where(p => p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "DINHEIRO").Sum(p => p.QuantCopia));
            totalCopiasDinheiro = qtdCopiasDinheiro * copia;
            qtdMaterializacaoDinheiro = ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DINHEIRO").Count();
            totalMaterializacaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));

            qtdAutenticacaoDeposito = ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DEPÓSITO"  || p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticacaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAberturaDeposito = ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "MENSALISTA").Count();
            totalAberturaDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAutenticidadeDeposito = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticidadeDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC AUTENTICIDADE" && p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAutenticidadeDutDeposito = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticidadeDutDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoAto == "REC AUTENTICIDADE (DUT)" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdSemelhancaDeposito = ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "MENSALISTA").Count();
            totalSemelhancaDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCopiasDeposito = Convert.ToInt32(reciboBalcao.Where(p => p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "DEPÓSITO" || p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "MENSALISTA").Sum(p => p.QuantCopia));
            totalCopiasDeposito = qtdCopiasDeposito * copia;
            qtdMaterializacaoDeposito = ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "MENSALISTA").Count();
            totalMaterializacaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            qtdApontamentoDinheiro = ato.Where(p => p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "DINHEIRO").Count();
            totalApontamentoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCancelamentoDinheiro = ato.Where(p => p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "DINHEIRO").Count();
            totalCancelamentoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCertidaoProtestoDinheiro = ato.Where(p => p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "DINHEIRO").Count();
            totalCertidaoProtestoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));

            qtdApontamentoDeposito = ato.Where(p => p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalApontamentoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCancelamentoDeposito = ato.Where(p => p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalCancelamentoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoProtestoDeposito = ato.Where(p => p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoProtestoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            qtdPagamento = ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO").Count();
            totalPagamento = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO").Sum(p => p.Total));

            qtdTarifa = ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO").Count();
            totalTarifa = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO").Sum(p => p.Bancaria));


            qtdRegistroDinheiro = ato.Where(p => p.TipoAto == "REGISTRO" && p.TipoPagamento == "DINHEIRO").Count();
            totalRegistroDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REGISTRO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdAverbacaoDinheiro = ato.Where(p => p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "DINHEIRO").Count();
            totalAverbacaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCertidaoRgiDinheiro = ato.Where(p => p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "DINHEIRO").Count();
            totalCertidaoRgiDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));

            qtdRegistroDeposito = ato.Where(p => p.TipoAto == "REGISTRO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REGISTRO" && p.TipoPagamento == "MENSALISTA").Count();
            totalRegistroDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REGISTRO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REGISTRO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAverbacaoDeposito = ato.Where(p => p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalAverbacaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoRgiDeposito = ato.Where(p => p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoRgiDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            TotalAdicionarDinheiro = Convert.ToDecimal(adicionarCaixa.Where(p => p.TpPagamento == "DINHEIRO").Sum(p => p.Valor));
            TotalAdicionarDeposito = Convert.ToDecimal(adicionarCaixa.Where(p => p.TpPagamento == "DEPÓSITO").Sum(p => p.Valor));

            TotalCheque = Convert.ToDecimal(cadCheque.Sum(p => p.Valor));

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();




            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));
            reportParameter.Add(new ReportParameter("TotalEscrituraDinheiro", string.Format("{0:n2}", totalEscrituraDinheiro)));
            reportParameter.Add(new ReportParameter("QtdEscrituraDinheiro", qtdEscrituraDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalProcuracaoDinheiro",  string.Format("{0:n2}",totalProcuracaoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdProcuracaoDinheiro", qtdProcuracaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoDinheiro",  string.Format("{0:n2}",totalCertidaoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCertidaoDinheiro", qtdCertidaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalTestamentoDinheiro",  string.Format("{0:n2}",totalTestamentoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdTestamentoDinheiro", qtdTestamentoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalApostilamentoHaiaDinheiro", string.Format("{0:n2}", totalApostilamentoHaiaDinheiro)));
            reportParameter.Add(new ReportParameter("QtdApostilamentoHaiaDinheiro", qtdApostilamentoHaiaDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("TotalEscrituraDeposito",  string.Format("{0:n2}",totalEscrituraDeposito)));
            reportParameter.Add(new ReportParameter("QtdEscrituraDeposito", qtdEscrituraDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalProcuracaoDeposito",  string.Format("{0:n2}",totalProcuracaoDeposito)));
            reportParameter.Add(new ReportParameter("QtdProcuracaoDeposito", qtdProcuracaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoDeposito",  string.Format("{0:n2}",totalCertidaoDeposito)));
            reportParameter.Add(new ReportParameter("QtdCertidaoDeposito", qtdCertidaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalTestamentoDeposito",  string.Format("{0:n2}",totalTestamentoDeposito)));
            reportParameter.Add(new ReportParameter("QtdTestamentoDeposito", qtdTestamentoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalApostilamentoHaiaDeposito", string.Format("{0:n2}", totalApostilamentoHaiaDeposito)));
            reportParameter.Add(new ReportParameter("QtdApostilamentoHaiaDeposito", qtdApostilamentoHaiaDeposito.ToString()));


            reportParameter.Add(new ReportParameter("TotalAutenticacaoDinheiro", string.Format("{0:n2}", totalAutenticacaoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdAutenticacaoDinheiro", qtdAutenticacaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalAberturaDinheiro", string.Format("{0:n2}", totalAberturaDinheiro)));
            reportParameter.Add(new ReportParameter("QtdAberturaDinheiro", qtdAberturaDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalAutenticidadeDinheiro", string.Format("{0:n2}", totalAutenticidadeDinheiro)));
            reportParameter.Add(new ReportParameter("QtdAutenticidadeDinheiro", qtdAutenticidadeDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalAutenticidadeDutDinheiro", string.Format("{0:n2}", totalAutenticidadeDutDinheiro)));
            reportParameter.Add(new ReportParameter("QtdAutenticidadeDutDinheiro", qtdAutenticidadeDutDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalSemelhancaDinheiro", string.Format("{0:n2}", totalSemelhancaDinheiro)));
            reportParameter.Add(new ReportParameter("QtdSemelhancaDinheiro", qtdSemelhancaDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCopiasDinheiro", string.Format("{0:n2}", totalCopiasDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCopiasDinheiro", qtdCopiasDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalMaterializacaoDinheiro", string.Format("{0:n2}", totalMaterializacaoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdMaterializacaoDinheiro", qtdMaterializacaoDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("TotalAutenticacaoDeposito", string.Format("{0:n2}", totalAutenticacaoDeposito)));
            reportParameter.Add(new ReportParameter("QtdAutenticacaoDeposito", qtdAutenticacaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalAberturaDeposito", string.Format("{0:n2}", totalAberturaDeposito)));
            reportParameter.Add(new ReportParameter("QtdAberturaDeposito", qtdAberturaDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalAutenticidadeDeposito", string.Format("{0:n2}", totalAutenticidadeDeposito)));
            reportParameter.Add(new ReportParameter("QtdAutenticidadeDeposito", qtdAutenticidadeDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalAutenticidadeDutDeposito", string.Format("{0:n2}", totalAutenticidadeDutDeposito)));
            reportParameter.Add(new ReportParameter("QtdAutenticidadeDutDeposito", qtdAutenticidadeDutDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalSemelhancaDeposito", string.Format("{0:n2}", totalSemelhancaDeposito)));
            reportParameter.Add(new ReportParameter("QtdSemelhancaDeposito", qtdSemelhancaDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCopiasDeposito", string.Format("{0:n2}", totalCopiasDeposito)));
            reportParameter.Add(new ReportParameter("QtdCopiasDeposito", qtdCopiasDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalMaterializacaoDeposito", string.Format("{0:n2}", totalMaterializacaoDeposito)));
            reportParameter.Add(new ReportParameter("QtdMaterializacaoDeposito", qtdMaterializacaoDeposito.ToString()));

            reportParameter.Add(new ReportParameter("TotalApontamentoDinheiro", string.Format("{0:n2}", totalApontamentoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdApontamentoDinheiro", qtdApontamentoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCancelamentoDinheiro", string.Format("{0:n2}", totalCancelamentoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCancelamentoDinheiro", qtdCancelamentoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoProtestoDinheiro", string.Format("{0:n2}", totalCertidaoProtestoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCertidaoProtestoDinheiro", qtdCertidaoProtestoDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("TotalPagamento", string.Format("{0:n2}", totalPagamento)));
            reportParameter.Add(new ReportParameter("QtdPagamento", qtdPagamento.ToString()));
            reportParameter.Add(new ReportParameter("TotalTarifa", string.Format("{0:n2}", totalTarifa)));
            reportParameter.Add(new ReportParameter("QtdTarifa", qtdTarifa.ToString()));

            reportParameter.Add(new ReportParameter("TotalApontamentoDeposito", string.Format("{0:n2}", totalApontamentoDeposito)));
            reportParameter.Add(new ReportParameter("QtdApontamentoDeposito", qtdApontamentoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCancelamentoDeposito", string.Format("{0:n2}", totalCancelamentoDeposito)));
            reportParameter.Add(new ReportParameter("QtdCancelamentoDeposito", qtdCancelamentoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoProtestoDeposito", string.Format("{0:n2}", totalCertidaoProtestoDeposito)));
            reportParameter.Add(new ReportParameter("QtdCertidaoProtestoDeposito", qtdCertidaoProtestoDeposito.ToString()));

            reportParameter.Add(new ReportParameter("TotalRegistroDinheiro", string.Format("{0:n2}", totalRegistroDinheiro)));
            reportParameter.Add(new ReportParameter("QtdRegistroDinheiro", qtdRegistroDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalAverbacaoDinheiro", string.Format("{0:n2}", totalAverbacaoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdAverbacaoDinheiro", qtdAverbacaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoRgiDinheiro", string.Format("{0:n2}", totalCertidaoRgiDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCertidaoRgiDinheiro", qtdCertidaoRgiDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("TotalRegistroDeposito", string.Format("{0:n2}", totalRegistroDeposito)));
            reportParameter.Add(new ReportParameter("QtdRegistroDeposito", qtdRegistroDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalAverbacaoDeposito", string.Format("{0:n2}", totalAverbacaoDeposito)));
            reportParameter.Add(new ReportParameter("QtdAverbacaoDeposito", qtdAverbacaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoRgiDeposito", string.Format("{0:n2}", totalCertidaoRgiDeposito)));
            reportParameter.Add(new ReportParameter("QtdCertidaoRgiDeposito", qtdCertidaoRgiDeposito.ToString()));

            reportParameter.Add(new ReportParameter("TotalAdicionarDeposito", string.Format("{0:n2}", TotalAdicionarDeposito)));
            reportParameter.Add(new ReportParameter("TotalAdicionarDinheiro", TotalAdicionarDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("TotalCheque", TotalCheque.ToString()));

            this.retirada_CaixaTableAdapter.FillByData(cS_CAIXA_DBDataSet.Retirada_Caixa, dataInicio, dataFim);

            this.adicionar_CaixaTableAdapter.FillByData(cS_CAIXA_DBDataSet.Adicionar_Caixa, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.RefreshReport();
        }
    }
}
