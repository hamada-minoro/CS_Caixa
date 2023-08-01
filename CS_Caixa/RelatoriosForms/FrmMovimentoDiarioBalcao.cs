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
    public partial class FrmMovimentoDiarioBalcao : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        public FrmMovimentoDiarioBalcao(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

       
        private void FrmMovimentoDiarioBalcao_Load(object sender, EventArgs e)
        {

            ClassAto classAto = new ClassAto();

            List<Ato> atos = new List<Ato>();
            atos = classAto.ListarAtoDataAto(dataInicio, dataFim, "BALCÃO");

            var dataSource = new ReportDataSource("DataSetAto", atos);

            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));

            reportViewer1.LocalReport.DataSources.Add(dataSource);
            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            //this.atoTableAdapter.FillByMovimentoDiarioBalcao(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepMovimentoDiarioBalcao.rdlc";

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            ClassAto classAto = new ClassAto();

            List<Ato> atos = new List<Ato>();
            atos = classAto.ListarAtoDataAto(dataInicio, dataFim);

            var dataSource = new ReportDataSource("DataSetAto", atos);

            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));

            reportViewer1.LocalReport.DataSources.Add(dataSource);
            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            //this.atoTableAdapter.FillByMovimentoDiarioBalcao(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepMovimentoDiarioBalcao.rdlc";

            reportViewer1.RefreshReport();
        }
    }
}
