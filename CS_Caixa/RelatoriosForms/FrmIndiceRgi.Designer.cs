namespace CS_Caixa.RelatoriosForms
{
    partial class FrmIndiceRgi
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
            this.indiceRegistrosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cS_CAIXA_DBDataSet = new CS_Caixa.CS_CAIXA_DBDataSet();
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.indiceRegistrosTableAdapter = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.IndiceRegistrosTableAdapter();
            this.tableAdapterManager = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.indiceRegistrosBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // indiceRegistrosBindingSource
            // 
            this.indiceRegistrosBindingSource.DataMember = "IndiceRegistros";
            this.indiceRegistrosBindingSource.DataSource = this.cS_CAIXA_DBDataSet;
            // 
            // cS_CAIXA_DBDataSet
            // 
            this.cS_CAIXA_DBDataSet.DataSetName = "CS_CAIXA_DBDataSet";
            this.cS_CAIXA_DBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer2
            // 
            this.reportViewer2.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet";
            reportDataSource1.Value = this.indiceRegistrosBindingSource;
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepIndiceRgi.rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(0, 0);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.Size = new System.Drawing.Size(754, 570);
            this.reportViewer2.TabIndex = 0;
            // 
            // indiceRegistrosTableAdapter
            // 
            this.indiceRegistrosTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.IndiceRegistrosTableAdapter = this.indiceRegistrosTableAdapter;
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
            // FrmIndiceRgi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 570);
            this.Controls.Add(this.reportViewer2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmIndiceRgi";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Índice Rgi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmIndiceRgi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.indiceRegistrosBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private CS_CAIXA_DBDataSet cS_CAIXA_DBDataSet;
        private System.Windows.Forms.BindingSource indiceRegistrosBindingSource;
        private CS_CAIXA_DBDataSetTableAdapters.IndiceRegistrosTableAdapter indiceRegistrosTableAdapter;
        private CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
    }
}