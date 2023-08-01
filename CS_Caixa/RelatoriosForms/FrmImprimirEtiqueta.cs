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
    public partial class FrmImprimirEtiqueta : Form
    {

        decimal _emol, _custa20, _custa5, _custa5_2, _custa4, _custa2, _iss, _total;
        decimal valorCertidao, valorBuscas, valorArquiv;
        decimal totalCertidao, totalBuscas, totalArquiv;
        int qtdCertidao, qtdBuscas, qtdArquiv;

        Ato ato;
        List<ItensCustasNota> itensCustas;

        public FrmImprimirEtiqueta(Ato ato, List<ItensCustasNota> itensCustas)
        {

            this.ato = ato;
            this.itensCustas = itensCustas;
            InitializeComponent();
        }

        private void FrmImprimirEtiqueta_Load(object sender, EventArgs e)
        {
            try
            {
                _emol = Convert.ToDecimal(ato.Emolumentos);
                _custa20 = Convert.ToDecimal(ato.Fetj);
                _custa5 = Convert.ToDecimal(ato.Funperj);
                _custa5_2 = Convert.ToDecimal(ato.Fundperj);
                _custa4 = Convert.ToDecimal(ato.Funarpen);
                _custa2 = Convert.ToDecimal(ato.Pmcmv);
                _iss = Convert.ToDecimal(ato.Iss);
                _total = Convert.ToDecimal(ato.Total);


                for (int i = 0; i < itensCustas.Count; i++)
                {
                    if (itensCustas[i].Tabela == "16" && itensCustas[i].Item == "2")
                    {
                        valorCertidao = Convert.ToDecimal(itensCustas[i].Valor);
                        totalCertidao = Convert.ToDecimal(itensCustas[i].Total);
                        qtdCertidao = Convert.ToInt16(itensCustas[i].Quantidade);
                    }

                    if (itensCustas[i].Tabela == "16" && itensCustas[i].Item == "1")
                    {
                        valorBuscas = Convert.ToDecimal(itensCustas[i].Valor);
                        totalBuscas = Convert.ToDecimal(itensCustas[i].Total);
                        qtdBuscas = Convert.ToInt16(itensCustas[i].Quantidade);
                    }

                    if (itensCustas[i].Tabela == "16" && itensCustas[i].Item == "4")
                    {
                        valorArquiv = Convert.ToDecimal(itensCustas[i].Valor);
                        totalArquiv = Convert.ToDecimal(itensCustas[i].Total);
                        qtdArquiv = Convert.ToInt16(itensCustas[i].Quantidade);
                    }
                }



                // PARAMETROS DO RELATORIO
                List<ReportParameter> reportParameter = new List<ReportParameter>();



                // ADICIONANDO OS PARAMETROS
                reportParameter.Add(new ReportParameter("emol", string.Format("{0:n2}", _emol)));
                reportParameter.Add(new ReportParameter("custa20", string.Format("{0:n2}", _custa20)));
                reportParameter.Add(new ReportParameter("custa5", string.Format("{0:n2}", _custa5)));
                reportParameter.Add(new ReportParameter("custa5_2", string.Format("{0:n2}", _custa5_2)));
                reportParameter.Add(new ReportParameter("custa4", string.Format("{0:n2}", _custa4)));
                reportParameter.Add(new ReportParameter("custa2", string.Format("{0:n2}", _custa2)));
                reportParameter.Add(new ReportParameter("iss", string.Format("{0:n2}", _iss)));
                reportParameter.Add(new ReportParameter("total", string.Format("{0:n2}", _total)));


                reportParameter.Add(new ReportParameter("valorCertidao", string.Format("{0:n2}", valorCertidao)));
                reportParameter.Add(new ReportParameter("valorBuscas", string.Format("{0:n2}", valorBuscas)));
                reportParameter.Add(new ReportParameter("valorArquiv", string.Format("{0:n2}", valorArquiv)));


                reportParameter.Add(new ReportParameter("totalCertidao", string.Format("{0:n2}", totalCertidao)));
                reportParameter.Add(new ReportParameter("totalBuscas", string.Format("{0:n2}", totalBuscas)));
                reportParameter.Add(new ReportParameter("totalArquiv", string.Format("{0:n2}", totalArquiv)));


                reportParameter.Add(new ReportParameter("qtdCertidao", string.Format("{0}", qtdCertidao)));
                reportParameter.Add(new ReportParameter("qtdBuscas", string.Format("{0}", qtdBuscas)));
                reportParameter.Add(new ReportParameter("qtdArquiv", string.Format("{0}", qtdArquiv)));


                reportViewer2.LocalReport.SetParameters(reportParameter);
                this.reportViewer2.RefreshReport();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível visualizar a Etiqueta. " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            this.reportViewer2.RefreshReport();
        }

        private void reportViewer2_Print(object sender, ReportPrintEventArgs e)
        {
            this.Close();
        }

       

    }
}
