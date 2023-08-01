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
    public partial class FrmRelatorioFechamentoCaixaRgi : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        public FrmRelatorioFechamentoCaixaRgi(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

        
        private void FrmRelatorioFechamentoCaixaRgi_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            int qtdRegistroDinheiro;
            decimal totalRegistroDinheiro;
            int qtdAverbacaoDinheiro;
            decimal totalAverbacaoDinheiro;
            int qtdCertidaoRgiDinheiro;
            decimal totalCertidaoRgiDinheiro;

            int qtdRegistroDeposito;
            decimal totalRegistroDeposito;
            int qtdAverbacaoDeposito;
            decimal totalAverbacaoDeposito;
            int qtdCertidaoRgiDeposito;
            decimal totalCertidaoRgiDeposito;

            // Executando as consultas
            ClassAto classAto = new ClassAto();
            List<Ato> ato = new List<Ato>();

            ato = classAto.ListarTodosOsAtosPorData(dataInicio, dataFim);

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));

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


            reportParameter.Add(new ReportParameter("totalRegistroDinheiro", string.Format("{0:n2}", totalRegistroDinheiro)));
            reportParameter.Add(new ReportParameter("qtdRegistroDinheiro", qtdRegistroDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalAverbacaoDinheiro", string.Format("{0:n2}", totalAverbacaoDinheiro)));
            reportParameter.Add(new ReportParameter("qtdAverbacaoDinheiro", qtdAverbacaoDinheiro.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoRgiDinheiro", string.Format("{0:n2}", totalCertidaoRgiDinheiro)));
            reportParameter.Add(new ReportParameter("qtdCertidaoRgiDinheiro", qtdCertidaoRgiDinheiro.ToString()));


            reportParameter.Add(new ReportParameter("totalRegistroDeposito", string.Format("{0:n2}", totalRegistroDeposito)));
            reportParameter.Add(new ReportParameter("qtdRegistroDeposito", qtdRegistroDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalAverbacaoDeposito", string.Format("{0:n2}", totalAverbacaoDeposito)));
            reportParameter.Add(new ReportParameter("qtdAverbacaoDeposito", qtdAverbacaoDeposito.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoRgiDeposito", string.Format("{0:n2}", totalCertidaoRgiDeposito)));
            reportParameter.Add(new ReportParameter("qtdCertidaoRgiDeposito", qtdCertidaoRgiDeposito.ToString()));


            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            this.atoTableAdapter.FillByRGIData(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);


            reportViewer1.LocalReport.SetParameters(reportParameter);


            this.reportViewer1.RefreshReport();

        }
    }
}
