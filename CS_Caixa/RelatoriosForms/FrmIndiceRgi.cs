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
    public partial class FrmIndiceRgi : Form
    {

        string Nome;

        public FrmIndiceRgi(string Nome)
        {
            this.Nome = Nome;
            InitializeComponent();
        }

        private void FrmIndiceRgi_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'cS_CAIXA_DBDataSet.IndiceRegistros'. Você pode movê-la ou removê-la conforme necessário.
            this.indiceRegistrosTableAdapter.Fill(this.cS_CAIXA_DBDataSet.IndiceRegistros);

            this.indiceRegistrosTableAdapter.FillByConsultaParteNome(this.cS_CAIXA_DBDataSet.IndiceRegistros, Nome);
            this.reportViewer2.RefreshReport();
        }

       
    }
}
