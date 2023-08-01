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
    public partial class FrmFichaIndisponibilidade : Form
    {
        string titulo, nome, cpfCnpj, oficio, aviso, processo, obs, ultimoNome;

        public FrmFichaIndisponibilidade(string titulo, string nome, string cpfCnpj, string oficio, string aviso, string processo, string obs)
        {
            this.titulo = titulo;
            this.nome = nome;
            this.cpfCnpj = cpfCnpj;
            this.oficio = oficio;
            this.aviso = aviso;
            this.processo = processo;
            this.obs = obs;
            InitializeComponent();
        }

        private void FrmFichaIndisponibilidade_Load(object sender, EventArgs e)
        {
            try
            {


                if (titulo == null || titulo == "")
                {
                    titulo = " ";
                }

                if (nome == null || nome == "")
                {
                    nome = " ";
                }

                if (cpfCnpj == null || cpfCnpj == "")
                {
                    cpfCnpj = " ";
                }

                if (oficio == null || oficio == "")
                {
                    oficio = " ";
                }

                if (aviso == null || aviso == "")
                {
                    aviso = " ";
                }

                if (processo == null || processo == "")
                {
                    processo = " ";
                }

                if (obs == null || obs == "")
                {
                    obs = " ";
                }

                int index = 0;


                if (nome.Contains(" "))
                {
                    index = nome.LastIndexOf(" ");
                    ultimoNome = nome.Substring(index + 1, nome.Length - index - 1);
                }
                else
                {
                    ultimoNome = nome;
                }
                if(nome == " ")
                {
                    ultimoNome = nome;
                }
                // PARAMETROS DO RELATORIO
                List<ReportParameter> reportParameter = new List<ReportParameter>();


                // ADICIONANDO OS PARAMETROS
                reportParameter.Add(new ReportParameter("titulo", titulo));
                reportParameter.Add(new ReportParameter("ultimoNome", ultimoNome));
                reportParameter.Add(new ReportParameter("nome", nome));
                reportParameter.Add(new ReportParameter("cpfCnpj", cpfCnpj));
                reportParameter.Add(new ReportParameter("oficio", oficio));
                reportParameter.Add(new ReportParameter("aviso", aviso));
                reportParameter.Add(new ReportParameter("processo", processo));
                reportParameter.Add(new ReportParameter("obs", obs));

                reportViewer1.LocalReport.SetParameters(reportParameter);
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível visualizar a ficha. " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
    }
}
