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
    public partial class FrmControleDiarioProcuracao : Form
    {
        DateTime dataInicio;
        DateTime dataFim;
        Usuario _usuario;
        public FrmControleDiarioProcuracao(DateTime dataInicio, DateTime dataFim, Usuario usuario)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            this._usuario = usuario;
            InitializeComponent();
        }

    
        private void FrmControleDiarioProcuracao_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            // ADICIONANDO OS PARAMETROS
            reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
            reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));
            reportParameter.Add(new ReportParameter("NomeEscrevente", _usuario.NomeUsu));

            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            this.atoTableAdapter.FillByNotasDataNomeNPago(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim, _usuario.Id_Usuario);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            this.reportViewer1.RefreshReport();
        }
    }
}
