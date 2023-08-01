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
    /// Interaction logic for WinImprimirRecolhimento.xaml
    /// </summary>
    public partial class WinImprimirRecolhimento : Window
    {
        List<ReportParameter> reportParameter = new List<ReportParameter>();
        string _tipo;
        List<Recolhimento> _Rgi, _Notas, _Protesto, _Balcao;
        DateTime _dataInicio, _dataFim;

        public WinImprimirRecolhimento(string tipo, DateTime dataInicio, DateTime dataFim , List<Recolhimento> Rgi, List<Recolhimento> Notas, List<Recolhimento> Protesto, List<Recolhimento> Balcao)
        {
            _tipo = tipo;
            _dataInicio = dataInicio;
            _dataFim = dataFim;
            _Rgi = Rgi.Count == 0? new List<Recolhimento>(): Rgi;
            _Notas = Notas.Count == 0 ? new List<Recolhimento>() : Notas;
            _Protesto = Protesto.Count == 0 ? new List<Recolhimento>() : Protesto;
            _Balcao = Balcao.Count == 0 ? new List<Recolhimento>() : Balcao;
            InitializeComponent();
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {

            reportParameter.Add(new ReportParameter("Periodo", string.Format("Período Recolhimento: {0} - {1}", _dataInicio.ToShortDateString(), _dataFim.ToShortDateString())));

            switch (_tipo)
            {
                case "TODOS":
                    ImprimirTodos();
                    break;

                case "RGI":
                    ImprimirRGI();
                    break;

                case "NOTAS":
                    ImprimirNotas();
                    break;

                case "PROTESTO":
                    ImprimirProtesto();
                    break;

                case "BALCÃO":
                    ImprimirBalcao();
                    break;

                default:
                    break;
            }

            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            ReportViewer.RefreshReport();
        }

        private void ImprimirBalcao()
        {
            var dataSource = new ReportDataSource("DataSetBalcao", _Balcao);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepImprimirBalcaoRecolhimento.rdlc";
            ReportViewer.LocalReport.SetParameters(reportParameter);
        }

        private void ImprimirProtesto()
        {
            var dataSource = new ReportDataSource("DataSetProtesto", _Protesto);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepImprimirProtestoRecolhimento.rdlc";
            ReportViewer.LocalReport.SetParameters(reportParameter);
        }

        private void ImprimirNotas()
        {
            var dataSource = new ReportDataSource("DataSetNotas", _Notas);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepImprimirNotasRecolhimento.rdlc";
            ReportViewer.LocalReport.SetParameters(reportParameter);
        }

        private void ImprimirRGI()
        {
            var dataSource = new ReportDataSource("DataSetRgi", _Rgi);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepImprimirRgiRecolhimento.rdlc";
            ReportViewer.LocalReport.SetParameters(reportParameter);
        }

        private void ImprimirTodos()
        {
            var dataSource = new ReportDataSource("DataSetRgi", _Rgi);
            var dataSource2 = new ReportDataSource("DataSetNotas", _Notas);
            var dataSource3 = new ReportDataSource("DataSetProtesto", _Protesto);
            var dataSource4 = new ReportDataSource("DataSetBalcao", _Balcao);

            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.DataSources.Add(dataSource2);
            ReportViewer.LocalReport.DataSources.Add(dataSource3);
            ReportViewer.LocalReport.DataSources.Add(dataSource4);

            ReportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepImprimirTodosRecolhimento.rdlc";
            ReportViewer.LocalReport.SetParameters(reportParameter);
        }
    }
}
