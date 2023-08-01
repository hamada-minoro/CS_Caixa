using CS_Caixa.Controls;
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
    /// Lógica interna para WinRelatorioMovimentoDiario.xaml
    /// </summary>
    public partial class WinRelatorioMovimentoDiario : Window
    {

        DateTime _dataInicio;
        DateTime _dataFim;
        public WinRelatorioMovimentoDiario(DateTime dataInicio, DateTime dataFim)
        {
            _dataInicio = dataInicio;
            _dataFim = dataFim;
            InitializeComponent();
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            reportParameter.Add(new ReportParameter("Periodo", string.Format("Período: {0} até {1}", _dataInicio.ToShortDateString(), _dataFim.ToShortDateString())));
            ClassAto classAto = new ClassAto();

            List<Ato> atos = new List<Ato>();
            atos = classAto.ListarAtoDataAto(_dataInicio, _dataFim);

            var dataSource = new ReportDataSource("DataSetAto", atos);

            reportViewer.LocalReport.DataSources.Add(dataSource);

            reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepMovimentoDiarioBalcao.rdlc";
            reportViewer.LocalReport.SetParameters(reportParameter);
            reportViewer.RefreshReport();

        }
    }
}
