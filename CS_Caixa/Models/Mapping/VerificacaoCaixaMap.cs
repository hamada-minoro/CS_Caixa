using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class VerificacaoCaixaMap : EntityTypeConfiguration<VerificacaoCaixa>
    {
        public VerificacaoCaixaMap()
        {
            // Primary Key
            this.HasKey(t => t.VerificacaoCaixaId);

            // Properties
            this.Property(t => t.Status)
                .HasMaxLength(1);

            this.Property(t => t.Valor)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VerificacaoCaixa");
            this.Property(t => t.VerificacaoCaixaId).HasColumnName("VerificacaoCaixaId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
