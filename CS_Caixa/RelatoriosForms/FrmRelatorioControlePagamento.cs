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
    public partial class FrmRelatorioControlePagamento : Form
    {
        DateTime dataInicio;
        DateTime dataFim;

        public FrmRelatorioControlePagamento(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

        private void controlePagamentoCreditoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.controlePagamentoCreditoBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cS_CAIXA_DBDataSet);

        }

        private void FormRelatorioControlePagamento_Load(object sender, EventArgs e)
        {
            
           
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();




            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("dataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("dataFim", dataFim.ToShortDateString()));
             
            this.controlePagamentoCreditoTableAdapter.FillByData(this.cS_CAIXA_DBDataSet.ControlePagamentoCredito, dataInicio, dataFim);

            this.controlePagamentoDebitoTableAdapter.FillByData(this.cS_CAIXA_DBDataSet.ControlePagamentoDebito, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            this.reportViewer1.RefreshReport();
        }
    }
}
