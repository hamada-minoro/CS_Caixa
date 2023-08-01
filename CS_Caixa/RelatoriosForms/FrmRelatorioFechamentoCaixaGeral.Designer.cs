namespace CS_Caixa.RelatoriosForms
{
    partial class FrmRelatorioFechamentoCaixaGeral
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.retirada_CaixaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cS_CAIXA_DBDataSet = new CS_Caixa.CS_CAIXA_DBDataSet();
            this.Adicionar_CaixaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.adicionar_CaixaTableAdapter1 = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.Adicionar_CaixaTableAdapter();
            this.adicionar_CaixaTableAdapter = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.Adicionar_CaixaTableAdapter();
            this.tableAdapterManager = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager();
            this.retirada_CaixaTableAdapter = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.Retirada_CaixaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.retirada_CaixaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Adicionar_CaixaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // retirada_CaixaBindingSource
            // 
            this.retirada_CaixaBindingSource.DataMember = "Retirada_Caixa";
            this.retirada_CaixaBindingSource.DataSource = this.cS_CAIXA_DBDataSet;
            // 
            // cS_CAIXA_DBDataSet
            // 
            this.cS_CAIXA_DBDataSet.DataSetName = "CS_CAIXA_DBDataSet";
            this.cS_CAIXA_DBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Adicionar_CaixaBindingSource
            // 
            this.Adicionar_CaixaBindingSource.DataMember = "Adicionar_Caixa";
            this.Adicionar_CaixaBindingSource.DataSource = this.cS_CAIXA_DBDataSet;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSetRetirada";
            reportDataSource1.Value = this.retirada_CaixaBindingSource;
            reportDataSource2.Name = "DataSetAdicionarCaixa";
            reportDataSource2.Value = this.Adicionar_CaixaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepFechamentoCaixa.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(760, 556);
            this.reportViewer1.TabIndex = 0;
            // 
            // adicionar_CaixaTableAdapter1
            // 
            this.adicionar_CaixaTableAdapter1.ClearBeforeFill = true;
            // 
            // adicionar_CaixaTableAdapter
            // 
            this.adicionar_CaixaTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.@__MigrationHistoryTableAdapter = null;
            this.tableAdapterManager.Adicionar_CaixaTableAdapter = null;
            this.tableAdapterManager.AtoTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CadChequeTableAdapter = null;
            this.tableAdapterManager.CadMensalistaTableAdapter = null;
            this.tableAdapterManager.ControlePagamentoCreditoTableAdapter = null;
            this.tableAdapterManager.ControlePagamentoDebitoTableAdapter = null;
            this.tableAdapterManager.CustasDistribuicaoTableAdapter = null;
            this.tableAdapterManager.CustasNotasTableAdapter = null;
            this.tableAdapterManager.CustasProtestoTableAdapter = null;
            this.tableAdapterManager.CustasRgiTableAdapter = null;
            this.tableAdapterManager.IndisponibilidadeTableAdapter = null;
            this.tableAdapterManager.ItensAtoNotasTableAdapter = null;
            this.tableAdapterManager.ItensAtoRgiTableAdapter = null;
            this.tableAdapterManager.ItensCustasNotasTableAdapter = null;
            this.tableAdapterManager.ItensCustasProtestoTableAdapter = null;
            this.tableAdapterManager.ItensCustasRgiTableAdapter = null;
            this.tableAdapterManager.LogTableAdapter = null;
            this.tableAdapterManager.PortadorTableAdapter = null;
            this.tableAdapterManager.ReciboBalcaoTableAdapter = null;
            this.tableAdapterManager.Retirada_CaixaTableAdapter = this.retirada_CaixaTableAdapter;
            this.tableAdapterManager.SeloAtualBalcaoTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.UsuariosTableAdapter = null;
            // 
            // retirada_CaixaTableAdapter
            // 
            this.retirada_CaixaTableAdapter.ClearBeforeFill = true;
            // 
            // FrmRelatorioFechamentoCaixaGeral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 556);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmRelatorioFechamentoCaixaGeral";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório Fechamento Caixa";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmRelatorioFechamentoCaixaGeral_Load);
            ((System.ComponentModel.ISupportInitialize)(this.retirada_CaixaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Adicionar_CaixaBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private CS_CAIXA_DBDataSetTableAdapters.Adicionar_CaixaTableAdapter adicionar_CaixaTableAdapter1;
        private CS_CAIXA_DBDataSetTableAdapters.Adicionar_CaixaTableAdapter adicionar_CaixaTableAdapter;
        private CS_CAIXA_DBDataSet cS_CAIXA_DBDataSet;
        private CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private CS_CAIXA_DBDataSetTableAdapters.Retirada_CaixaTableAdapter retirada_CaixaTableAdapter;
        private System.Windows.Forms.BindingSource Adicionar_CaixaBindingSource;
        private System.Windows.Forms.BindingSource retirada_CaixaBindingSource;
    }
}