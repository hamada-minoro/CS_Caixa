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
using CS_Caixa.Controls;

namespace CS_Caixa.RelatoriosForms
{
    public partial class FrmMovimentoDiarioProtesto : Form
    {

        DateTime dataInicio;
        DateTime dataFim;
        public FrmMovimentoDiarioProtesto(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }


        private void FrmMovimentoDiarioProtesto_Load(object sender, EventArgs e)
        {

            reportViewer1.ProcessingMode = ProcessingMode.Local;

            ClassAto classAto = new ClassAto();
            List<Ato> Pagamento = new List<Ato>();
            int ano;
            // PARAMETROS DO RELATORIO
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            Pagamento = classAto.ListarAtoDataProtesto(dataInicio, dataFim, "PAGAMENTO");


            int QtdPagamento = Pagamento.Count;

            if (Pagamento.Count > 0)
            {
                ano = Pagamento[0].DataAto.Year;


                ClassCustasProtesto custasEmissao = new ClassCustasProtesto();

                List<CustasProtesto> emissao = new List<CustasProtesto>();
                emissao = custasEmissao.ListaCustas();

                decimal porcentagemIss = 0;

                CustasProtesto ValorEmissao = new CustasProtesto();

                ValorEmissao = emissao.Where(p => p.DESCR == "EXPEDIÇÃO E EMISSÃO DE GUIAS E COMUNICAÇÕES" && p.ANO == ano).FirstOrDefault();

                porcentagemIss = Convert.ToDecimal(emissao.Where(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());

                decimal emol = 0;
                decimal fetj_20 = 0;
                decimal fundperj_5 = 0;
                decimal funperj_5 = 0;
                decimal funarpen_4 = 0;
                decimal iss = 0;
                decimal total = 0;
                string Semol = "0,00";
                string Sfetj_20 = "0,00";
                string Sfundperj_5 = "0,00";
                string Sfunperj_5 = "0,00";
                string Sfunarpen_4 = "0,00";
                string Siss= "0,00";
                int index;

                emol = Convert.ToDecimal(ValorEmissao.VALOR);

                fetj_20 = emol * 20 / 100;
                fundperj_5 = emol * 5 / 100;
                funperj_5 = emol * 5 / 100;
                funarpen_4 = emol * 4 / 100;
                iss = emol * porcentagemIss / 100;

                Semol = Convert.ToString(emol);

                Sfetj_20 = Convert.ToString(fetj_20);
                Sfundperj_5 = Convert.ToString(fundperj_5);
                Sfunperj_5 = Convert.ToString(funperj_5);
                Sfunarpen_4 = Convert.ToString(funarpen_4);
                Siss = Convert.ToString(iss);

                index = Semol.IndexOf(',');
                Semol = Semol.Substring(0, index + 3);

                index = Sfetj_20.IndexOf(',');
                Sfetj_20 = Sfetj_20.Substring(0, index + 3);

                index = Sfundperj_5.IndexOf(',');
                Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);

                index = Sfunperj_5.IndexOf(',');
                Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);

                index = Sfunarpen_4.IndexOf(',');
                Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                index = Siss.IndexOf(',');
                Siss = Siss.Substring(0, index + 3);

                emol = Convert.ToDecimal(Semol);
                fetj_20 = Convert.ToDecimal(Sfetj_20);
                fundperj_5 = Convert.ToDecimal(Sfundperj_5);
                funperj_5 = Convert.ToDecimal(Sfunperj_5);
                funarpen_4 = Convert.ToDecimal(Sfunarpen_4);
                iss = Convert.ToDecimal(Siss);

                total = emol + fetj_20 + fundperj_5 + funperj_5 + funarpen_4 + iss;



                // ADICIONANDO OS PARAMETROS
                reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
                reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));
                reportParameter.Add(new ReportParameter("QtdPagamento", QtdPagamento.ToString()));
                reportParameter.Add(new ReportParameter("Emol", string.Format("{0:n2}", emol * QtdPagamento)));
                reportParameter.Add(new ReportParameter("Fetj", string.Format("{0:n2}", fetj_20 * QtdPagamento)));
                reportParameter.Add(new ReportParameter("Fundperj", string.Format("{0:n2}", fundperj_5 * QtdPagamento)));
                reportParameter.Add(new ReportParameter("Funperj", string.Format("{0:n2}", funperj_5 * QtdPagamento)));
                reportParameter.Add(new ReportParameter("Funarpen", string.Format("{0:n2}", funarpen_4 * QtdPagamento)));
                reportParameter.Add(new ReportParameter("Total", string.Format("{0:n2}", total * QtdPagamento)));
                reportParameter.Add(new ReportParameter("Iss", string.Format("{0:n2}", iss * QtdPagamento)));
            }
            else
            {
                // ADICIONANDO OS PARAMETROS
                reportParameter.Add(new ReportParameter("DataInicio", dataInicio.ToShortDateString()));
                reportParameter.Add(new ReportParameter("DataFim", dataFim.ToShortDateString()));
                reportParameter.Add(new ReportParameter("QtdPagamento", "0"));
                reportParameter.Add(new ReportParameter("Emol", "0,00"));
                reportParameter.Add(new ReportParameter("Fetj", "0,00"));
                reportParameter.Add(new ReportParameter("Fundperj", "0,00"));
                reportParameter.Add(new ReportParameter("Funperj", "0,00"));
                reportParameter.Add(new ReportParameter("Funarpen", "0,00"));
                reportParameter.Add(new ReportParameter("Total", "0,00"));
                reportParameter.Add(new ReportParameter("Iss", "0,00"));
            }
            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.Ato'. Você pode movê-la ou removê-la conforme necessário.
            this.atoTableAdapter.FillByMovimentoDiarioProtesto(this.cS_CAIXA_DBDataSet.Ato, dataInicio, dataFim);

            reportViewer1.LocalReport.SetParameters(reportParameter);

            this.reportViewer1.RefreshReport();
        }
    }
}
