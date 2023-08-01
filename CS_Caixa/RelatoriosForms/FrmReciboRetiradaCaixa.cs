using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CS_Caixa.RelatoriosForms
{
    public partial class FrmRecibo : Form
    {

        string construtor = string.Empty;


        string data;
        string descricao;
        string numeroRecibo;
        string valor;

        public FrmRecibo(string data, string descricao, string numeroRecibo, string valor)
        {
            this.data = data;
            this.descricao = descricao;
            this.numeroRecibo = numeroRecibo;
            this.valor = valor;
            construtor = "ReciboCaixa";
            InitializeComponent();
        }



        string _recibo, _empresa, _data, _qtdTitulos, _emol, _custas20, _custas5, _custas4, _custas2, _iss, _total, _escrevente;


        public FrmRecibo(string recibo, string empresa, string data, string qtdTitulos, string emol, string custas20, string custas5, string custas4, string custas2, string iss, string total, string escrevente)
        {
            _recibo = recibo;
            _empresa = empresa;
            _data = data;
            _qtdTitulos = qtdTitulos;
            _emol = emol;
            _custas20 = custas20;
            _custas5 = custas5;
            _custas4 = custas4;
            _custas2 = custas2;
            _iss = iss;
            _total = total;
            _escrevente = escrevente;
            construtor = "ReciboSerasa";
            InitializeComponent();
        }


        private void FrmReciboRetiradaCaixa_Load(object sender, EventArgs e)
        {
            reportViewer1.ProcessingMode = ProcessingMode.Local;

            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();



            if (construtor == "ReciboSerasa")
            {
                // ADICIONANDO OS PARAMETROS
                reportParameter.Add(new ReportParameter("Recibo", _recibo));
                reportParameter.Add(new ReportParameter("Empresa", _empresa));
                reportParameter.Add(new ReportParameter("Data", _data));
                reportParameter.Add(new ReportParameter("QtdTitulos", _qtdTitulos));

                reportParameter.Add(new ReportParameter("Emol", _emol));
                reportParameter.Add(new ReportParameter("Custas20", _custas20));
                reportParameter.Add(new ReportParameter("Custas5", _custas5));
                reportParameter.Add(new ReportParameter("Custas4", _custas4));
                reportParameter.Add(new ReportParameter("Custas2", _custas2));
                               
                reportParameter.Add(new ReportParameter("Iss", _iss));

                reportParameter.Add(new ReportParameter("Total", _total));
                reportParameter.Add(new ReportParameter("Escrevente", _escrevente));

                reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepReciboSeresa.rdlc";

                reportViewer1.LocalReport.SetParameters(reportParameter);
            }

            if (construtor == "ReciboCaixa")
            {
                // ADICIONANDO OS PARAMETROS
                reportParameter.Add(new ReportParameter("Data", data));
                reportParameter.Add(new ReportParameter("Descricao", descricao));
                reportParameter.Add(new ReportParameter("NumeroRecibo", numeroRecibo));
                reportParameter.Add(new ReportParameter("Valor", valor));

                reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepReciboVale.rdlc";
                reportViewer1.LocalReport.SetParameters(reportParameter);

                
            }


            this.reportViewer1.RefreshReport();
        }
    }
}
