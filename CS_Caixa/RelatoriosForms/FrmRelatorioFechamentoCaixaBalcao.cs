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
    public partial class FrmRelatorioFechamentoCaixaBalcao : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        public FrmRelatorioFechamentoCaixaBalcao(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }


        private void FrmRelatorioFechamentoCaixaBalcao_Load(object sender, EventArgs e)
        {

            reportViewer1.ProcessingMode = ProcessingMode.Local;


            int qtdAutenticacaoDinheiro;
            decimal totalAutenticacaoDinheiro;
            int qtdAberturaDinheiro;
            decimal totalAberturaDinheiro;
            int qtdAutenticidadeDinheiro;
            decimal totalAutenticidadeDinheiro;
            int qtdSemelhancaDinheiro;
            decimal totalSemelhancaDinheiro;
            int qtdCopiasDinheiro;
            decimal totalCopiasDinheiro;



            int qtdAutenticacaoDeposito;
            decimal totalAutenticacaoDeposito;
            int qtdAberturaDeposito;
            decimal totalAberturaDeposito;
            int qtdAutenticidadeDeposito;
            decimal totalAutenticidadeDeposito;
            int qtdSemelhancaDeposito;
            decimal totalSemelhancaDeposito;
            int qtdCopiasDeposito;
            decimal totalCopiasDeposito;
            int qtdMaterializacaoDinheiro;
            decimal totalMaterializacaoDinheiro;
            int qtdMaterializacaoDeposito;
            decimal totalMaterializacaoDeposito;



            // Executando as consultas
            ClassAto classAto = new ClassAto();
            List<Ato> ato = new List<Ato>();

            ato = classAto.ListarTodosOsAtosPorData(dataInicio, dataFim);

            ClassBalcao classBalcao = new ClassBalcao();
            List<ReciboBalcao> reciboBalcao = new List<ReciboBalcao>();

            reciboBalcao = classBalcao.ListarTodosPorData(dataInicio, dataFim);

            ClassCustasNotas classCustasNotas = new ClassCustasNotas();
            decimal copia = classCustasNotas.ValorCopia(dataInicio.Year);




            qtdAutenticacaoDinheiro = ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DINHEIRO").Count();
            totalAutenticacaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdAberturaDinheiro = ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DINHEIRO").Count();
            totalAberturaDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdAutenticidadeDinheiro = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DINHEIRO").Count();
            totalAutenticidadeDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdSemelhancaDinheiro = ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DINHEIRO").Count();
            totalSemelhancaDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCopiasDinheiro = Convert.ToInt32(reciboBalcao.Where(p => p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "DINHEIRO").Sum(p => p.QuantCopia));
            totalCopiasDinheiro = qtdCopiasDinheiro * copia;



            qtdAutenticacaoDeposito = ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticacaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAberturaDeposito = ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "MENSALISTA").Count();
            totalAberturaDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAutenticidadeDeposito = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticidadeDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC AUTENTICIDADE" && p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdSemelhancaDeposito = ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "MENSALISTA").Count();
            totalSemelhancaDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCopiasDeposito = Convert.ToInt32(reciboBalcao.Where(p => p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "DEPÓSITO" || p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "MENSALISTA").Sum(p => p.QuantCopia));
            totalCopiasDeposito = qtdCopiasDeposito * copia;
            qtdMaterializacaoDinheiro = ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DINHEIRO").Count();
            totalMaterializacaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdMaterializacaoDeposito = ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "MENSALISTA").Count();
            totalMaterializacaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "MATERIALIZAÇÃO DE DOCUMENTOS ELETRÔNICOS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();


            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));
            reportParameter.Add(new ReportParameter("totalAutenticacaoDinheiro", string.Format("{0:n2}", totalAutenticacaoDinheiro)));
            reportParameter.Add(new ReportParameter("qtdAutenticacaoDinheiro", qtdAutenticacaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalAberturaDinheiro", string.Format("{0:n2}", totalAberturaDinheiro)));
            reportParameter.Add(new ReportParameter("qtdAberturaDinheiro", qtdAberturaDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalAutenticidadeDinheiro", string.Format("{0:n2}", totalAutenticidadeDinheiro)));
            reportParameter.Add(new ReportParameter("qtdAutenticidadeDinheiro", qtdAutenticidadeDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalSemelhancaDinheiro", string.Format("{0:n2}", totalSemelhancaDinheiro)));
            reportParameter.Add(new ReportParameter("qtdSemelhancaDinheiro", qtdSemelhancaDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalCopiasDinheiro", string.Format("{0:n2}", totalCopiasDinheiro)));
            reportParameter.Add(new ReportParameter("qtdCopiasDinheiro", qtdCopiasDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("totalAutenticacaoDeposito", string.Format("{0:n2}", totalAutenticacaoDeposito)));
            reportParameter.Add(new ReportParameter("qtdAutenticacaoDeposito", qtdAutenticacaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalAberturaDeposito", string.Format("{0:n2}", totalAberturaDeposito)));
            reportParameter.Add(new ReportParameter("qtdAberturaDeposito", qtdAberturaDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalAutenticidadeDeposito", string.Format("{0:n2}", totalAutenticidadeDeposito)));
            reportParameter.Add(new ReportParameter("qtdAutenticidadeDeposito", qtdAutenticidadeDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalSemelhancaDeposito", string.Format("{0:n2}", totalSemelhancaDeposito)));
            reportParameter.Add(new ReportParameter("qtdSemelhancaDeposito", qtdSemelhancaDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalCopiasDeposito", string.Format("{0:n2}", totalCopiasDeposito)));
            reportParameter.Add(new ReportParameter("qtdCopiasDeposito", qtdCopiasDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalMaterializacaoDinheiro", string.Format("{0:n2}", totalMaterializacaoDinheiro)));
            reportParameter.Add(new ReportParameter("qtdMaterializacaoDinheiro", qtdMaterializacaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalMaterializacaoDeposito", string.Format("{0:n2}", totalMaterializacaoDeposito)));
            reportParameter.Add(new ReportParameter("qtdMaterializacaoDeposito", qtdMaterializacaoDeposito.ToString()));

            this.atoTableAdapter.FillByBalcaoData(cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.RefreshReport();
        }
    }
}
