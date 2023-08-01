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
    public partial class FrmRelatorioFechamentoCaixaProtesto : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        public FrmRelatorioFechamentoCaixaProtesto(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

        private void FrmRelatorioFechamentoCaixaProtesto_Load(object sender, EventArgs e)
        {

            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            int qtdApontamentoDinheiro;
            decimal totalApontamentoDinheiro;
            int qtdCancelamentoDinheiro;
            decimal totalCancelamentoDinheiro;
            int qtdCertidaoProtestoDinheiro;
            decimal totalCertidaoProtestoDinheiro;

            
            int qtdApontamentoDeposito;
            decimal totalApontamentoDeposito;
            int qtdCancelamentoDeposito;
            decimal totalCancelamentoDeposito;
            int qtdCertidaoProtestoDeposito;
            decimal totalCertidaoProtestoDeposito;

            // pagamento de protesto
            int qtdPagamento;
            decimal totalPagamento;

            int qtdPagamentoConvenio;
            decimal totalPagamentoConvenio;

            int qtdTarifa;
            decimal totalTarifa;

            int qtdTarifaConvenio;
            decimal totalTarifaConvenio;

            decimal totalPagoTodos;

            // Executando as consultas
            ClassAto classAto = new ClassAto();
            List<Ato> ato = new List<Ato>();

            ato = classAto.ListarTodosOsAtosPorData(dataInicio, dataFim);



           

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

            qtdPagamento = ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "N").Count();
            totalPagamento = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "N").Sum(p => p.Total));

            qtdPagamentoConvenio = ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "S").Count();
            totalPagamentoConvenio = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "S").Sum(p => p.ValorTitulo));

            qtdTarifa = ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "N").Count();
            totalTarifa = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "N").Sum(p => p.Bancaria));

            qtdTarifaConvenio = ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "S").Count();
            totalTarifaConvenio = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO" && p.Convenio == "S").Sum(p => p.Bancaria));

            totalPagoTodos = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PAGAMENTO" && p.TipoPagamento == "DEPÓSITO").Sum(p => p.Total));



            reportParameter.Add(new ReportParameter("TotalApontamentoDinheiro", string.Format("{0:n2}", totalApontamentoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdApontamentoDinheiro", qtdApontamentoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCancelamentoDinheiro", string.Format("{0:n2}", totalCancelamentoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCancelamentoDinheiro", qtdCancelamentoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoProtestoDinheiro", string.Format("{0:n2}", totalCertidaoProtestoDinheiro)));
            reportParameter.Add(new ReportParameter("QtdCertidaoProtestoDinheiro", qtdCertidaoProtestoDinheiro.ToString()));

            reportParameter.Add(new ReportParameter("TotalPagamento", string.Format("{0:n2}", totalPagamento)));
            reportParameter.Add(new ReportParameter("QtdPagamento", qtdPagamento.ToString()));
            reportParameter.Add(new ReportParameter("TotalPagamentoConvenio", string.Format("{0:n2}", totalPagamentoConvenio)));
            reportParameter.Add(new ReportParameter("QtdPagamentoConvenio", qtdPagamentoConvenio.ToString()));
            reportParameter.Add(new ReportParameter("TotalTarifa", string.Format("{0:n2}", totalTarifa)));
            reportParameter.Add(new ReportParameter("QtdTarifa", qtdTarifa.ToString()));
            reportParameter.Add(new ReportParameter("TotalTarifaConvenio", string.Format("{0:n2}", totalTarifaConvenio)));
            reportParameter.Add(new ReportParameter("QtdTarifaConvenio", qtdTarifaConvenio.ToString()));
            reportParameter.Add(new ReportParameter("TotalPagoTodos", totalPagoTodos.ToString()));


            reportParameter.Add(new ReportParameter("TotalApontamentoDeposito", string.Format("{0:n2}", totalApontamentoDeposito)));
            reportParameter.Add(new ReportParameter("QtdApontamentoDeposito", qtdApontamentoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCancelamentoDeposito", string.Format("{0:n2}", totalCancelamentoDeposito)));
            reportParameter.Add(new ReportParameter("QtdCancelamentoDeposito", qtdCancelamentoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("TotalCertidaoProtestoDeposito", string.Format("{0:n2}", totalCertidaoProtestoDeposito)));
            reportParameter.Add(new ReportParameter("QtdCertidaoProtestoDeposito", qtdCertidaoProtestoDeposito.ToString()));

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));

            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            this.atoTableAdapter.FillByDataProtesto(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);


            reportViewer1.LocalReport.SetParameters(reportParameter);


            this.reportViewer1.RefreshReport();
        }

    }
}
