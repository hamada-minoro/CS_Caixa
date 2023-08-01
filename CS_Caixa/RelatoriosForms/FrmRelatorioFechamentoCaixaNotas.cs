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
    public partial class FrmRelatorioFechamentoCaixaNotas : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        public FrmRelatorioFechamentoCaixaNotas(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

      
        private void FrmRelatorioFechamentoCaixaNotas_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            int qtdEscrituraDinheiro;
            decimal totalEscrituraDinheiro;
            int qtdProcuracaoDinheiro;
            decimal totalProcuracaoDinheiro;
            int qtdTestamentoDinheiro;
            decimal totalTestamentoDinheiro;
            int qtdCertidaoNotasDinheiro;
            decimal totalCertidaoNotasDinheiro;
            int qtdApostilamentoHaiaDinheiro;
            decimal totalApostilamentoHaiaDinheiro;


            int qtdEscrituraDeposito;
            decimal totalEscrituraDeposito;
            int qtdProcuracaoDeposito;
            decimal totalProcuracaoDeposito;
            int qtdTestamentoDeposito;
            decimal totalTestamentoDeposito;
            int qtdCertidaoNotasDeposito;
            decimal totalCertidaoNotasDeposito;
            int qtdApostilamentoHaiaDeposito;
            decimal totalApostilamentoHaiaDeposito;


            // Executando as consultas
            ClassAto classAto = new ClassAto();
            List<Ato> ato = new List<Ato>();

            ato = classAto.ListarTodosOsAtosPorData(dataInicio, dataFim);

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));


            qtdEscrituraDinheiro = ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DINHEIRO").Count();
            totalEscrituraDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdProcuracaoDinheiro = ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DINHEIRO").Count();
            totalProcuracaoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdTestamentoDinheiro = ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DINHEIRO").Count();
            totalTestamentoDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdCertidaoNotasDinheiro = ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DINHEIRO").Count();
            totalCertidaoNotasDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));
            qtdApostilamentoHaiaDinheiro = ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DINHEIRO").Count();
            totalApostilamentoHaiaDinheiro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DINHEIRO").Sum(p => p.Total));


            qtdEscrituraDeposito = ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalEscrituraDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APONTAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdProcuracaoDeposito = ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalProcuracaoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdTestamentoDeposito = ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalTestamentoDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoNotasDeposito = ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoNotasDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdApostilamentoHaiaDeposito = ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "MENSALISTA").Count();
            totalApostilamentoHaiaDeposito = Convert.ToDecimal(ato.Where(p => p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "DEPÓSITO" || p.TipoAto == "APOSTILAMENTO HAIA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));


            reportParameter.Add(new ReportParameter("totalEscrituraDinheiro", string.Format("{0:n2}", totalEscrituraDinheiro)));
            reportParameter.Add(new ReportParameter("qtdEscrituraDinheiro", qtdEscrituraDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalProcuracaoDinheiro", string.Format("{0:n2}", totalProcuracaoDinheiro)));
            reportParameter.Add(new ReportParameter("qtdProcuracaoDinheiro", qtdProcuracaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalTestamentoDinheiro", string.Format("{0:n2}", totalTestamentoDinheiro)));
            reportParameter.Add(new ReportParameter("qtdTestamentoDinheiro", qtdTestamentoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoNotasDinheiro", string.Format("{0:n2}", totalCertidaoNotasDinheiro)));
            reportParameter.Add(new ReportParameter("qtdCertidaoNotasDinheiro", qtdCertidaoNotasDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalApostilamentoHaiaDinheiro", string.Format("{0:n2}", totalApostilamentoHaiaDinheiro)));
            reportParameter.Add(new ReportParameter("qtdApostilamentoHaiaDinheiro", qtdApostilamentoHaiaDinheiro.ToString()));


            reportParameter.Add(new ReportParameter("totalEscrituraDeposito", string.Format("{0:n2}", totalEscrituraDeposito)));
            reportParameter.Add(new ReportParameter("qtdEscrituraDeposito", qtdEscrituraDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalProcuracaoDeposito", string.Format("{0:n2}", totalProcuracaoDeposito)));
            reportParameter.Add(new ReportParameter("qtdProcuracaoDeposito", qtdProcuracaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalTestamentoDeposito", string.Format("{0:n2}", totalTestamentoDeposito)));
            reportParameter.Add(new ReportParameter("qtdTestamentoDeposito", qtdTestamentoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoNotasDeposito", string.Format("{0:n2}", totalCertidaoNotasDeposito)));
            reportParameter.Add(new ReportParameter("qtdCertidaoNotasDeposito", qtdCertidaoNotasDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalApostilamentoHaiaDeposito", string.Format("{0:n2}", totalApostilamentoHaiaDeposito)));
            reportParameter.Add(new ReportParameter("qtdApostilamentoHaiaDeposito", qtdApostilamentoHaiaDeposito.ToString()));
            

            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            this.atoTableAdapter.FillByNotasData(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);


            reportViewer1.LocalReport.SetParameters(reportParameter);


            this.reportViewer1.RefreshReport();
        }
    }
}
