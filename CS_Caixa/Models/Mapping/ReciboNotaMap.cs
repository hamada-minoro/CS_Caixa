using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ReciboNotaMap : EntityTypeConfiguration<ReciboNota>
    {
        public ReciboNotaMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ReciboNotasId, t.Recibo });

            // Properties
            this.Property(t => t.ReciboNotasId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Recibo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Atribuicao)
                .HasMaxLength(15);

            this.Property(t => t.Status)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ReciboNotas");
            this.Property(t => t.ReciboNotasId).HasColumnName("ReciboNotasId");
            this.Property(t => t.AtoId).HasColumnName("AtoId");
            this.Property(t => t.Recibo).HasColumnName("Recibo");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.DataEntrega).HasColumnName("DataEntrega");
            this.Property(t => t.ApresentanteId).HasColumnName("ApresentanteId");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
