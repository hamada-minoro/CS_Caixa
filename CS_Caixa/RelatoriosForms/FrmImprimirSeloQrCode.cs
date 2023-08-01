using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CS_Caixa.RelatoriosForms
{
    public partial class FrmImprimirSeloQrCode : Form
    {
        string _caminhoImagem;
        string _selo;

        public FrmImprimirSeloQrCode(string caminhoImagem, string selo)
        {
            _caminhoImagem = caminhoImagem;
            _selo = selo;
            InitializeComponent();
        }

        private void FrmImprimirSeloQrCode_Load(object sender, EventArgs e)
        {
            


            List<ReportParameter> reportParameter = new List<ReportParameter>();

            reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));
            reportParameter.Add(new ReportParameter("Selo", _selo));

            reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepQrCodeModelo1.rdlc";
            reportViewer1.LocalReport.SetParameters(reportParameter);            
            reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.RefreshReport();
        }
    }
}
