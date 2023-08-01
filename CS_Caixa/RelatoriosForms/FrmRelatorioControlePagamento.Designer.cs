namespace CS_Caixa.RelatoriosForms
{
    partial class FrmRelatorioControlePagamento
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
            this.controlePagamentoCreditoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cS_CAIXA_DBDataSet = new CS_Caixa.CS_CAIXA_DBDataSet();
            this.controlePagamentoDebitoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.controlePagamentoCreditoTableAdapter = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.ControlePagamentoCreditoTableAdapter();
            this.tableAdapterManager = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager();
            this.controlePagamentoDebitoTableAdapter = new CS_Caixa.CS_CAIXA_DBDataSetTableAdapters.ControlePagamentoDebitoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.controlePagamentoCreditoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlePagamentoDebitoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // controlePagamentoCreditoBindingSource
            // 
            this.controlePagamentoCreditoBindingSource.DataMember = "ControlePagamentoCredito";
            this.controlePagamentoCreditoBindingSource.DataSource = this.cS_CAIXA_DBDataSet;
            // 
            // cS_CAIXA_DBDataSet
            // 
            this.cS_CAIXA_DBDataSet.DataSetName = "CS_CAIXA_DBDataSet";
            this.cS_CAIXA_DBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // controlePagamentoDebitoBindingSource
            // 
            this.controlePagamentoDebitoBindingSource.DataMember = "ControlePagamentoDebito";
            this.controlePagamentoDebitoBindingSource.DataSource = this.cS_CAIXA_DBDataSet;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSetCredito";
            reportDataSource1.Value = this.controlePagamentoCreditoBindingSource;
            reportDataSource2.Name = "DataSetDebito";
            reportDataSource2.Value = this.controlePagamentoDebitoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.RepRelatorioControlePagamento.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(642, 531);
            this.reportViewer1.TabIndex = 0;
            // 
            // controlePagamentoCreditoTableAdapter
            // 
            this.controlePagamentoCreditoTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.@__MigrationHistoryTableAdapter = null;
            this.tableAdapterManager.Adicionar_CaixaTableAdapter = null;
            this.tableAdapterManager.AtoTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CadChequeTableAdapter = null;
            this.tableAdapterManager.CadMensalistaTableAdapter = null;
            this.tableAdapterManager.ControlePagamentoCreditoTableAdapter = this.controlePagamentoCreditoTableAdapter;
            this.tableAdapterManager.ControlePagamentoDebitoTableAdapter = this.controlePagamentoDebitoTableAdapter;
            this.tableAdapterManager.CustasDistribuicaoTableAdapter = null;
            this.tableAdapterManager.CustasNotasTableAdapter = null;
            this.tableAdapterManager.CustasProtestoTableAdapter = null;
            this.tableAdapterManager.CustasRgiTableAdapter = null;
            this.tableAdapterManager.IndiceRegistrosTableAdapter = null;
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
            // controlePagamentoDebitoTableAdapter
            // 
            this.controlePagamentoDebitoTableAdapter.ClearBeforeFill = true;
            // 
            // FrmRelatorioControlePagamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 531);
            this.Controls.Add(this.reportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmRelatorioControlePagamento";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório de Controle de Pagamento";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormRelatorioControlePagamento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.controlePagamentoCreditoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cS_CAIXA_DBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.controlePagamentoDebitoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CS_CAIXA_DBDataSet cS_CAIXA_DBDataSet;
        private System.Windows.Forms.BindingSource controlePagamentoCreditoBindingSource;
        private CS_CAIXA_DBDataSetTableAdapters.ControlePagamentoCreditoTableAdapter controlePagamentoCreditoTableAdapter;
        private CS_CAIXA_DBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private CS_CAIXA_DBDataSetTableAdapters.ControlePagamentoDebitoTableAdapter controlePagamentoDebitoTableAdapter;
        private System.Windows.Forms.BindingSource controlePagamentoDebitoBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}