using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CS_Caixa.RelatoriosForms
{
    public partial class FrmMovimentoDiarioRgi : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        public FrmMovimentoDiarioRgi(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

       
        private void FrmMovimentoDiarioRgi_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));


            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            this.atoTableAdapter.FillByMovimentoDiarioRgi(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            this.reportViewer1.RefreshReport();
        }
    }
}
