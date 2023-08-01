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
    public partial class FrmRelatorioMensalista : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        string NomeMensalista;
        public FrmRelatorioMensalista(DateTime dataInicio, DateTime dataFim, string NomeMensalista)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            this.NomeMensalista = NomeMensalista;
            InitializeComponent();
        }

        private void FrmRelatorioMensalista_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // CARREGANDO OS PARAMENTROS 

            // variaveis dinheiro
            int qtdEscritura;
            decimal totalEscritura;
            int qtdProcuracao;
            decimal totalProcuracao;
            int qtdCertidaoNotas;
            decimal totalCertidaoNotas;
            int qtdTestamento;
            decimal totalTestamento;

            int qtdAutenticacao;
            decimal totalAutenticacao;
            int qtdAbertura;
            decimal totalAbertura;
            int qtdAutenticidade;
            decimal totalAutenticidade;
            int qtdSemelhanca;
            decimal totalSemelhanca;
            int qtdCopias;
            decimal totalCopias;



            int qtdApontamento;
            decimal totalApontamento;
            int qtdCancelamento;
            decimal totalCancelamento;
            int qtdCertidaoProtesto;
            decimal totalCertidaoProtesto;


            int qtdRegistro;
            decimal totalRegistro;
            int qtdAverbacao;
            decimal totalAverbacao;
            int qtdCertidaoRgi;
            decimal totalCertidaoRgi;


            // Executando as consultas
            ClassAto classAto = new ClassAto();
            List<Ato> ato = new List<Ato>();

            ato = classAto.ListarTodosOsAtosPorData(dataInicio, dataFim);

            ato = ato.Where(p => p.Mensalista == NomeMensalista).ToList();

            ClassBalcao classBalcao = new ClassBalcao();
            List<ReciboBalcao> reciboBalcao = new List<ReciboBalcao>();

            reciboBalcao = classBalcao.ListarTodosPorData(dataInicio, dataFim);

            ClassCustasNotas classCustasNotas = new ClassCustasNotas();
            decimal copia = classCustasNotas.ValorCopia(dataInicio.Year);


            qtdEscritura = ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "MENSALISTA").Count();
            totalEscritura = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ESCRITURA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdProcuracao = ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalProcuracao = Convert.ToDecimal(ato.Where(p => p.TipoAto == "PROCURAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoNotas = ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoNotas = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO NOTAS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdTestamento = ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalTestamento = Convert.ToDecimal(ato.Where(p => p.TipoAto == "TESTAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));


            qtdAutenticacao = ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticacao = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AUTENTICAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAbertura = ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "MENSALISTA").Count();
            totalAbertura = Convert.ToDecimal(ato.Where(p => p.TipoAto == "ABERTURA DE FIRMAS" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAutenticidade = ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "MENSALISTA").Count();
            totalAutenticidade = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC AUTENTICIDADE" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdSemelhanca = ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "MENSALISTA").Count();
            totalSemelhanca = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REC SEMELHANÇA" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCopias = Convert.ToInt32(reciboBalcao.Where(p => p.Pago == true && p.QuantCopia > 0 && p.TipoPagamento == "MENSALISTA" && p.Mensalista == NomeMensalista).Sum(p => p.QuantCopia));
            totalCopias = qtdCopias * copia;

            qtdApontamento = ato.Where(p => p.TipoAto.Contains("APONTAMENTO") && p.TipoPagamento == "MENSALISTA").Count();
            totalApontamento = Convert.ToDecimal(ato.Where(p => p.TipoAto.Contains("APONTAMENTO") && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCancelamento = ato.Where(p => p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalCancelamento = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CANCELAMENTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoProtesto = ato.Where(p => p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoProtesto = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO PROTESTO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            qtdRegistro = ato.Where(p => p.TipoAto == "REGISTRO" && p.TipoPagamento == "MENSALISTA").Count();
            totalRegistro = Convert.ToDecimal(ato.Where(p => p.TipoAto == "REGISTRO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdAverbacao = ato.Where(p => p.TipoAto == "AVERBEÇÃO" && p.TipoPagamento == "MENSALISTA").Count();
            totalAverbacao = Convert.ToDecimal(ato.Where(p => p.TipoAto == "AVERBAÇÃO" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));
            qtdCertidaoRgi = ato.Where(p => p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "MENSALISTA").Count();
            totalCertidaoRgi = Convert.ToDecimal(ato.Where(p => p.TipoAto == "CERTIDÃO RGI" && p.TipoPagamento == "MENSALISTA").Sum(p => p.Total));

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));
            reportParameter.Add(new ReportParameter("NomeMensalista", NomeMensalista));

            reportParameter.Add(new ReportParameter("totalEscritura", string.Format("{0:n2}", totalEscritura)));
            reportParameter.Add(new ReportParameter("qtdEscritura", qtdEscritura.ToString()));
            reportParameter.Add(new ReportParameter("totalProcuracao", string.Format("{0:n2}", totalProcuracao)));
            reportParameter.Add(new ReportParameter("qtdProcuracao", qtdProcuracao.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoNotas", string.Format("{0:n2}", totalCertidaoNotas)));
            reportParameter.Add(new ReportParameter("qtdCertidaoNotas", qtdCertidaoNotas.ToString()));
            reportParameter.Add(new ReportParameter("totalTestamento", string.Format("{0:n2}", totalTestamento)));
            reportParameter.Add(new ReportParameter("qtdTestamento", qtdTestamento.ToString()));

            reportParameter.Add(new ReportParameter("totalAutenticacao", string.Format("{0:n2}", totalAutenticacao)));
            reportParameter.Add(new ReportParameter("qtdAutenticacao", qtdAutenticacao.ToString()));
            reportParameter.Add(new ReportParameter("totalAbertura", string.Format("{0:n2}", totalAbertura)));
            reportParameter.Add(new ReportParameter("qtdAbertura", qtdAbertura.ToString()));
            reportParameter.Add(new ReportParameter("totalAutenticidade", string.Format("{0:n2}", totalAutenticidade)));
            reportParameter.Add(new ReportParameter("qtdAutenticidade", qtdAutenticidade.ToString()));
            reportParameter.Add(new ReportParameter("totalSemelhanca", string.Format("{0:n2}", totalSemelhanca)));
            reportParameter.Add(new ReportParameter("qtdSemelhanca", qtdSemelhanca.ToString()));
            reportParameter.Add(new ReportParameter("totalCopias", string.Format("{0:n2}", totalCopias)));
            reportParameter.Add(new ReportParameter("qtdCopias", qtdCopias.ToString()));

            reportParameter.Add(new ReportParameter("totalApontamento", string.Format("{0:n2}", totalApontamento)));
            reportParameter.Add(new ReportParameter("qtdApontamento", qtdApontamento.ToString()));
            reportParameter.Add(new ReportParameter("totalCancelamento", string.Format("{0:n2}", totalCancelamento)));
            reportParameter.Add(new ReportParameter("qtdCancelamento", qtdCancelamento.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoProtesto", string.Format("{0:n2}", totalCertidaoProtesto)));
            reportParameter.Add(new ReportParameter("qtdCertidaoProtesto", qtdCertidaoProtesto.ToString()));

            reportParameter.Add(new ReportParameter("totalRegistro", string.Format("{0:n2}", totalRegistro)));
            reportParameter.Add(new ReportParameter("qtdRegistro", qtdRegistro.ToString()));
            reportParameter.Add(new ReportParameter("totalAverbacao", string.Format("{0:n2}", totalAverbacao)));
            reportParameter.Add(new ReportParameter("qtdAverbacao", qtdAverbacao.ToString()));
            reportParameter.Add(new ReportParameter("totalCertidaoRgi", string.Format("{0:n2}", totalCertidaoRgi)));
            reportParameter.Add(new ReportParameter("qtdCertidaoRgi", qtdCertidaoRgi.ToString()));


            this.atoTableAdapter.FillByMensalista(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim, NomeMensalista);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.RefreshReport();

        }

        
    }
}
