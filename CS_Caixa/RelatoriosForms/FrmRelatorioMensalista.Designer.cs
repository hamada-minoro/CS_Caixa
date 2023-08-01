namespace CS_Caixa.RelatoriosForms
{
    partial class FrmRelatorioMensalista
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
            this.atoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cS_CAIXA_DBDataSet = new CS_Caixa.CS_CAIXA_DBDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.atoTableAdapter = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.AtoTableAdapter();
            this.tableAdapterManager = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.atoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // atoBindingSource
            // 
            this.atoBindingSource.DataMember = "Ato";
            this.atoBindingSource.DataSource = this.cS_CAIXA_DBDataSet;
            // 
            // cS_CAIXA_DBDataSet
            // 
            this.cS_CAIXA_DBDataSet.DataSetName = "CS_CAIXA_DBDataSet";
            this.cS_CAIXA_DBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSetAto";
            reportDataSource1.Value = this.atoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepRelatorioMensalista.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(681, 579);
            this.reportViewer1.TabIndex = 0;
            // 
            // atoTableAdapter
            // 
            this.atoTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.@__MigrationHistoryTableAdapter = null;
            this.tableAdapterManager.Adicionar_CaixaTableAdapter = null;
            this.tableAdapterManager.AtoTableAdapter = this.atoTableAdapter;
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
            this.tableAdapterManager.Retirada_CaixaTableAdapter = null;
            this.tableAdapterManager.SeloAtualBalcaoTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.UsuariosTableAdapter = null;
            // 
            // FrmRelatorioMensalista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 579);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmRelatorioMensalista";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório Mensalista";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmRelatorioMensalista_Load);
            ((System.ComponentModel.ISupportInitialize)(this.atoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private CS_CAIXA_DBDataSet cS_CAIXA_DBDataSet;
        private System.Windows.Forms.BindingSource atoBindingSource;
        private CS_CAIXA_DBDataSetTableAdapters.AtoTableAdapter atoTableAdapter;
        private CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
    }
}