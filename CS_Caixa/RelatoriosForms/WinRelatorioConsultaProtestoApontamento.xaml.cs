using CS_Caixa.Models;
using Microsoft.Reporting.WinForms;
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

namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Lógica interna para WinRelatorioConsultaProtestoApontamento.xaml
    /// </summary>
    public partial class WinRelatorioConsultaProtestoApontamento : Window
    {

        List<Ato> atosExibir;
        string _dataInicio, _dataFim;

        string _tipoAto;

        public WinRelatorioConsultaProtestoApontamento(List<Ato> Atos, string dataInicio, string dataFim, string tipoAto)
        {
            atosExibir = Atos;
            _dataInicio = dataInicio;
            _dataFim = dataFim;
            _tipoAto = tipoAto;
            InitializeComponent();
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {

            this.Title = this.Title + " DE " + _tipoAto;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            reportParameter.Add(new ReportParameter("Periodo", string.Format("Período: {0} até {1}", _dataInicio, _dataFim)));


            switch (_tipoAto)
            {
                //Protesto

                case "APONTAMENTO":

                    var dataSource = new ReportDataSource("DataSetAtos", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSource);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoApontamento.rdlc";
                    reportParameter.Add(new ReportParameter("TotalNaoConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "N").Sum(p => p.Total))));
                    reportParameter.Add(new ReportParameter("TotalConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "S").Sum(p => p.Total))));
                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "CANCELAMENTO":

                    var dataSourceCancelamento = new ReportDataSource("DataSetCancelamento", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceCancelamento);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoCancelamento.rdlc";
                    reportParameter.Add(new ReportParameter("TotalNaoConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "N").Sum(p => p.Total))));
                    reportParameter.Add(new ReportParameter("TotalConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "S").Sum(p => p.Total))));
                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "PAGAMENTO":

                    var dataSourcePagamento = new ReportDataSource("DataSetPagamentos", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourcePagamento);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoPagamento.rdlc";
                    reportParameter.Add(new ReportParameter("TotalNaoConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "N").Sum(p => p.Total))));
                    reportParameter.Add(new ReportParameter("TotalConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "S").Sum(p => p.Total))));

                    reportParameter.Add(new ReportParameter("TotalApontNaoConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "N").Sum(p => p.Emolumentos + p.Acoterj + p.Fetj + p.Funarpen + p.Fundperj + p.Funperj + p.Iss + p.Mutua + p.Pmcmv))));
                    reportParameter.Add(new ReportParameter("TotalApontConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "S").Sum(p => p.Emolumentos + p.Acoterj + p.Fetj + p.Funarpen + p.Fundperj + p.Funperj + p.Iss + p.Mutua + p.Pmcmv))));

                    reportParameter.Add(new ReportParameter("TotalTarifaNaoConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "N").Sum(p => p.Bancaria))));
                    reportParameter.Add(new ReportParameter("TotalTarifaConvenio", string.Format("{0:n2}", atosExibir.Where(t => t.Convenio == "S").Sum(p => p.Bancaria))));

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "CERTIDÃO PROTESTO":

                    var dataSourceCertidao = new ReportDataSource("DataSetCertidao", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceCertidao);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoCertidao.rdlc";
                    reportParameter.Add(new ReportParameter("TotalCertidao", string.Format("{0:n2}", atosExibir.Where(t => t.Natureza == "CERTIDÃO PROTESTO").Sum(p => p.Total))));
                    reportParameter.Add(new ReportParameter("TotalSerasa", string.Format("{0:n2}", atosExibir.Where(t => t.Natureza == "CERTIDÃO SERASA").Sum(p => p.Total))));
                    reportParameter.Add(new ReportParameter("TotalBoaVista", string.Format("{0:n2}", atosExibir.Where(t => t.Natureza == "CERTIDÃO BOA VISTA").Sum(p => p.Total))));
                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;


                //Notas

                case "ESCRITURA":

                    var dataSourceEscritura = new ReportDataSource("DataSetEscritura", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceEscritura);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoEscritura.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "PROCURAÇÃO":

                    var dataSourceProcuracao = new ReportDataSource("DataSetProcuracao", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceProcuracao);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoProcuracao.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "TESTAMENTO":

                    var dataSourceTestamento = new ReportDataSource("DataSetTestamento", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceTestamento);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoTestamento.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "CERTIDÃO NOTAS":

                    var dataSourceCertidaoNotas = new ReportDataSource("DataSetCertidaoNotas", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceCertidaoNotas);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoCertidaoNotas.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "APOSTILAMENTO HAIA":

                    var dataSourceApostilamento = new ReportDataSource("DataSetApostilamento", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceApostilamento);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoApostilamento.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;


                // Registro

                case "REGISTRO":

                    var dataSourceRegistro = new ReportDataSource("DataSetRegistro", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceRegistro);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoRegistro.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "AVERBAÇÃO":

                    var dataSourceAverbacao = new ReportDataSource("DataSetAverbacao", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceAverbacao);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoAverbacao.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                case "CERTIDÃO RGI":

                    var dataSourceCertidaoRgi = new ReportDataSource("DataSetCertidaoRgi", atosExibir);
                    ReportViewer.LocalReport.DataSources.Add(dataSourceCertidaoRgi);
                    ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepConsultaAtosProtestoCertidaoRgi.rdlc";

                    ReportViewer.LocalReport.SetParameters(reportParameter);

                    break;

                default:

                    break;
            }


            ReportViewer.RefreshReport();
        }
    }
}
