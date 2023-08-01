using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class MensagemMap : EntityTypeConfiguration<Mensagem>
    {
        public MensagemMap()
        {
            // Primary Key
            this.HasKey(t => t.Mensagem_Id);

            // Properties
            this.Property(t => t.Texto)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Mensagem");
            this.Property(t => t.Mensagem_Id).HasColumnName("Mensagem_Id");
            this.Property(t => t.Texto).HasColumnName("Texto");
            this.Property(t => t.Grau_Importacia).HasColumnName("Grau_Importacia");
            this.Property(t => t.Pisca).HasColumnName("Pisca");
            this.Property(t => t.Cor).HasColumnName("Cor");
        }
    }
}
