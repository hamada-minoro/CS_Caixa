using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CS_Caixa.Models;

namespace CS_Caixa.RelatoriosForms
{
    public partial class FrmRelatorioRepasseEscrevente : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        Usuario usuarioLogado;
        public FrmRelatorioRepasseEscrevente(DateTime dataInicio, DateTime dataFim, Usuario usuarioLogado)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            this.usuarioLogado = usuarioLogado;
            InitializeComponent();
        }

        private void FrmRelatorioRepasseEscrevente_Load(object sender, EventArgs e)
        {
           

            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();



            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));


            if (usuarioLogado.Master == true)
            {
                // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
                this.atoTableAdapter.FillByNotasData(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);
            }
            else
            {
                // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
                this.atoTableAdapter.FillByNotasDataNomeEscrevente(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim, usuarioLogado.Id_Usuario);
            }

            reportViewer1.LocalReport.SetParameters(reportParameter);


            this.reportViewer1.RefreshReport();
        }

    }
}
