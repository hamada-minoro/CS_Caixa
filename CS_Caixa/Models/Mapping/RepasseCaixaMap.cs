using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class RepasseCaixaMap : EntityTypeConfiguration<RepasseCaixa>
    {
        public RepasseCaixaMap()
        {
            // Primary Key
            this.HasKey(t => t.RepasseId);

            // Properties
            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("RepasseCaixa");
            this.Property(t => t.RepasseId).HasColumnName("RepasseId");
            this.Property(t => t.DataCaixa).HasColumnName("DataCaixa");
            this.Property(t => t.ValorCaixa).HasColumnName("ValorCaixa");
            this.Property(t => t.DataPagamentoRepasse).HasColumnName("DataPagamentoRepasse");
            this.Property(t => t.ValorRepasse).HasColumnName("ValorRepasse");
            this.Property(t => t.ValorRestante).HasColumnName("ValorRestante");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
        }
    }
}
